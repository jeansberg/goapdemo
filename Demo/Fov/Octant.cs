using Core;
using Core.GameObject;
using Core.Map;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Demo.Fov
{
    internal abstract class Octant
    {
        internal abstract void GetVisiblePoints(int depth, double startSlope, double endSlope, int range, Point source, Map map, List<Point> visiblePoints, Renderer renderer);
        protected abstract bool SlopeInBounds(int x, int y, double endSlope, Point source, Map map);

        protected bool PointInRange(int x, int y, Point source, int range)
        {
            return GetDistance(x, y, source.XPos, source.YPos) <= range;
        }

        protected bool Blocked(int x, int y, Map map)
        {
            return !map.Tiles[x][y].Transparent();
        }

        protected double GetSlope(double pX1, double pY1, double pX2, double pY2, bool pInvert)
        {
            double slope;
            if (pInvert)
                slope = (pY1 - pY2) / (pX1 - pX2);
            else
                slope = (pX1 - pX2) / (pY1 - pY2);

            Debug.WriteLine($"Slope from {pX1} {pY1} to {pX2} {pY2} is {slope} with invert = {pInvert}");

            return slope;
        }

        protected int GetDistance(int pX1, int pY1, int pX2, int pY2)
        {
            return ((pX1 - pX2) * (pX1 - pX2)) + ((pY1 - pY2) * (pY1 - pY2));
        }

        protected void Continue(int x, int y, Map map, int depth, int range, double startSlope, double endSlope, Point source, List<Point> visiblePoints, Renderer renderer)
        {
            if (x < 0)
                x = 0;
            else if (x >= map.GetWidth())
                x = map.GetWidth() - 1;

            if (y < 0)
                y = 0;
            else if (y >= map.GetHeight())
                y = map.GetHeight() - 1;

            if (depth < range & map.Tiles[x][y].Transparent())
                GetVisiblePoints(depth + 1, startSlope, endSlope, range, source, map, visiblePoints, renderer);
        }
    }

    internal class NnwOctant : Octant
    {
        internal override void GetVisiblePoints(int depth, double startSlope, double endSlope, int range, Point source, Map map, List<Point> visiblePoints, Renderer renderer)
        {
            int y = source.YPos - depth;

            if (y < 0)
            {
                return;
            }

            int x = Math.Max(source.XPos -  Convert.ToInt32((startSlope * Convert.ToDouble(depth))), 0);

            //renderer.DrawLine(source.xPos, source.yPos, x, y);

            while (SlopeInBounds(x, y, endSlope, source, map))
            {
                if (PointInRange(x, y, source, range^2))
                {
                    if (Blocked(x, y, map))
                    {
                        if (PreviousPointAccessible(x, y, map)) //prior cell within range AND open...
                        {
                            //...incremenet the depth, adjust the endslope and recurse
                            Debug.WriteLine($"Found blocking cell");
                            GetVisiblePoints(depth + 1, startSlope, GetSlope(x - 0.5, y + 0.5, source.XPos, source.YPos, false), range, source, map, visiblePoints, renderer);
                        }
                    }
                    else
                    {
                        if (PreviousPointAccessible(x, y, map)) //prior cell within range AND open...
                        {
                            //..adjust the startslope
                            startSlope = GetSlope(x - 0.5, y - 0.5, source.XPos, source.YPos, false);
                        }

                        visiblePoints.Add(new Point(x, y));
                        renderer.LightUp(x, y);
                        Debug.WriteLine($"Added {x} {y}, slopes {startSlope} {endSlope}");
                    }
                }
                x++;
            }
            x--;


            Debug.WriteLine($"Continue to depth {depth} starting at {x} {y}");
            Continue(x, y, map, depth, range, startSlope, endSlope, source, visiblePoints, renderer);
        }

        private bool PreviousPointAccessible(int x, int y, Map map)
        {
            return x - 1 >= 0 && !Blocked(x - 1, y, map);
        }

        protected override bool SlopeInBounds(int x, int y, double endSlope, Point source, Map map)
        {
            return GetSlope(x, y,  source.XPos, source.YPos, false) >= endSlope;
        }
    }

    //internal class WnwOctant : Octant
    //{
    //    internal override void GetVisiblePoints(int depth, double startSlope, double endSlope, int range, Point source, Map map, List<Point> visiblePoints)
    //    {
    //        int x = source.xPos - depth;

    //        if (x < 0)
    //        {
    //            return;
    //        }

            

    //        int y = Math.Max(source.yPos - Convert.ToInt32((startSlope * Convert.ToDouble(depth))), 0);

    //        while (SlopeInBounds(x, y, endSlope, source, map))
    //        {
    //            if (PointInRange(x, y, source, range))
    //            {
    //                if (Blocked(x, y, map))
    //                {
    //                    if (PreviousPointAccessible(x, y, map)) //prior cell within range AND open...
    //                    {
    //                        //...incremenet the depth, adjust the endslope and recurse
    //                        GetVisiblePoints(depth + 1, startSlope, GetSlope(x + 0.5, y - 0.5, source.xPos, source.yPos, true), range, source, map, visiblePoints);
    //                    }
    //                }
    //                else
    //                {
    //                    if (PreviousPointAccessible(x, y, map)) //prior cell within range AND open...
    //                    {
    //                        //..adjust the startslope
    //                        startSlope = GetSlope(x - 0.5, y - 0.5, source.xPos, source.yPos, true);
    //                    }

    //                    visiblePoints.Add(new Point(x, y));
    //                }
    //            }
    //            y++;
    //        }
    //        y--;

    //        Continue(x, y, map, depth, range, startSlope, endSlope, source, visiblePoints);
    //    }

        //private bool PreviousPointAccessible(int x, int y, Map map)
        //{
        //    return y - 1 >= 0 && !Blocked(x, y-1, map);
        //}

        //protected override bool SlopeInBounds(int x, int y, double endSlope, Point source, Map map)
        //{
        //    return GetSlope(x, y, source.xPos, source.yPos, true) >= endSlope;
        //}
    //}
}