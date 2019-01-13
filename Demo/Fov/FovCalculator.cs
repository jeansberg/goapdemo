using Core;
using Core.GameObject;
using Core.Map;
using System;
using System.Collections.Generic;

namespace Demo.Fov
{

    /// <summary>
    /// Implementation of "FOV using recursive shadowcasting - improved" as
    /// described on http://roguebasin.roguelikedevelopment.org/index.php?title=FOV_using_recursive_shadowcasting_-_improved
    /// </summary>
    public class FovCalculator
    {
        private int _range;
        public FovCalculator(int range)
        {
            _range = range;
        }

        /// <summary>
        /// The octants which a player can see
        /// </summary>
        List<int> VisibleOctants = new List<int>() {  3, 8 };

        //  Octant data
        //
        //    \ 1 | 2 /
        //   8 \  |  / 3
        //   -----+-----
        //   7 /  |  \ 4
        //    / 6 | 5 \
        //
        //  1 = NNW, 2 =NNE, 3=ENE, 4=ESE, 5=SSE, 6=SSW, 7=WSW, 8 = WNW

        /// <summary>
        /// Start here: go through all the octants which surround the player to
        /// determine which open cells are visible
        /// </summary>
        public List<Point> GetVisibleCells(Point source, Map map, Renderer renderer)
        {
            //foreach (int o in VisibleOctants)
            //{
            //    ScanOctant(1, o, 1.0, 0.0, source, map, visiblePoints);
            //}

            //var nnw = new NnwOctant();
            //var wnw = new WnwOctant();
            // nnw.GetVisiblePoints(1, 1.0, 0.0, _range, source, map, visiblePoints);
            //renderer.ResetBackGround();
            //nnw.GetVisiblePoints(1, 1.0, 0.0, _range, source, map, visiblePoints, renderer);

            var visiblePoints = new List<Point>();
            ShadowCast.ComputeVisibility(map, source, _range, visiblePoints);

            return visiblePoints;
        }

        /// <summary>
        /// Examine the provided octant and calculate the visible cells within it.
        /// </summary>
        /// <param name="pDepth">Depth of the scan</param>
        /// <param name="pOctant">Octant being examined</param>
        /// <param name="pStartSlope">Start slope of the octant</param>
        /// <param name="pEndSlope">End slope of the octance</param>
        protected void ScanOctant(int pDepth, int pOctant, double pStartSlope, double pEndSlope, Point source, Map map, List<Point> visiblePoints)
        {

            int rangeSquared = _range;
            int x = 0;
            int y = 0;

            switch (pOctant)
            {

                case 1: //nnw
                    y = source.yPos - pDepth;
                    if (y < 0)
                    {
                        return;
                    }

                    x = source.xPos - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x < 0)
                    {
                        x = 0;
                    }

                    while (GetSlope(x, y, source.xPos, source.yPos, false) >= pEndSlope)
                    {
                        if (GetVisDistance(x, y, source.xPos, source.yPos) <= rangeSquared)
                        {
                            if (!map.Tiles[x][y].Transparent()) //current cell blocked
                            {
                                if (x - 1 >= 0 && map.Tiles[x - 1][y].Transparent()) //prior cell within range AND open...
                                {
                                    //...incremenet the depth, adjust the endslope and recurse
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, source.xPos, source.yPos, false), source, map, visiblePoints);
                                }
                            }
                            else
                            {
                                if (x - 1 >= 0 && map.Tiles[x - 1][y].Transparent()) //prior cell within range AND open...
                                {
                                    //..adjust the startslope
                                    pStartSlope = GetSlope(x - 0.5, y - 0.5, source.xPos, source.yPos, false);
                                }

                                visiblePoints.Add(new Point(x, y));
                            }
                        }
                        x++;
                    }
                    x--;
                    break;

                case 2: //nne

                    y = source.yPos - pDepth;
                    if (y < 0) return;

                    x = source.xPos + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x >= map.GetWidth()) x = map.GetWidth() - 1;

                    while (GetSlope(x, y, source.xPos, source.yPos, false) <= pEndSlope)
                    {
                        if (GetVisDistance(x, y, source.xPos, source.yPos) <= rangeSquared)
                        {
                            if (!map.Tiles[x][y].Transparent())
                            {
                                if (x + 1 < map.GetWidth() && map.Tiles[x + 1][y].Transparent())
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, source.xPos, source.yPos, false), source, map, visiblePoints);
                            }
                            else
                            {
                                if (x + 1 < map.GetWidth() && map.Tiles[x + 1][y].Transparent())
                                    pStartSlope = -GetSlope(x + 0.5, y - 0.5, source.xPos, source.yPos, false);

                                visiblePoints.Add(new Point(x, y));
                            }
                        }
                        x--;
                    }
                    x++;
                    break;

                case 3:

                    x = source.xPos + pDepth;
                    if (x >= map.GetWidth()) return;

                    y = source.yPos - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y < 0) y = 0;

                    while (GetSlope(x, y, source.xPos, source.yPos, true) <= pEndSlope)
                    {

                        if (GetVisDistance(x, y, source.xPos, source.yPos) <= rangeSquared)
                        {

                            if (!map.Tiles[x][y].Transparent())
                            {
                                if (y - 1 >= 0 && map.Tiles[x][y - 1].Transparent())
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, source.xPos, source.yPos, true), source, map, visiblePoints);
                            }
                            else
                            {
                                if (y - 1 >= 0 && map.Tiles[x][y - 1].Transparent())
                                    pStartSlope = -GetSlope(x + 0.5, y - 0.5, source.xPos, source.yPos, true);

                                visiblePoints.Add(new Point(x, y));
                            }
                        }
                        y++;
                    }
                    y--;
                    break;

                case 4:

                    x = source.xPos + pDepth;
                    if (x >= map.GetWidth()) return;

                    y = source.yPos + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y >= map.GetHeight()) y = map.GetHeight();

                    while (GetSlope(x, y, source.xPos, source.yPos, false) >= pEndSlope)
                    {

                        if (GetVisDistance(x, y, source.xPos, source.yPos) <= rangeSquared)
                        {

                            if (!map.Tiles[x][y].Transparent())
                            {
                                if (y + 1 < map.GetHeight() && map.Tiles[x][y + 1].Transparent())
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, source.xPos, source.yPos, true), source, map, visiblePoints);
                            }
                            else
                            {
                                if (y + 1 < map.GetHeight() && map.Tiles[x][y + 1].Transparent())
                                    pStartSlope = GetSlope(x + 0.5, y + 0.5, source.xPos, source.yPos, true);

                                visiblePoints.Add(new Point(x, y));
                            }
                        }
                        y--;
                    }
                    y++;
                    break;

                case 5:

                    y = source.yPos + pDepth;
                    if (y >= map.GetHeight()) return;

                    x = source.xPos + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x >= map.GetWidth()) x = map.GetWidth() - 1;

                    while (GetSlope(x, y, source.xPos, source.yPos, false) >= pEndSlope)
                    {
                        if (GetVisDistance(x, y, source.xPos, source.yPos) <= rangeSquared)
                        {

                            if (!map.Tiles[x][y].Transparent())
                            {
                                if (x + 1 < map.GetHeight() && map.Tiles[x + 1][y].Transparent())
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, source.xPos, source.yPos, false), source, map, visiblePoints);
                            }
                            else
                            {
                                if (x + 1 < map.GetHeight()
                                        && map.Tiles[x + 1][y].Transparent())
                                    pStartSlope = GetSlope(x + 0.5, y + 0.5, source.xPos, source.yPos, false);

                                visiblePoints.Add(new Point(x, y));
                            }
                        }
                        x--;
                    }
                    x++;
                    break;

                case 6:

                    y = source.yPos + pDepth;
                    if (y >= map.GetHeight()) return;

                    x = source.xPos - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x < 0) x = 0;

                    while (GetSlope(x, y, source.xPos, source.yPos, false) <= pEndSlope)
                    {
                        if (GetVisDistance(x, y, source.xPos, source.yPos) <= rangeSquared)
                        {

                            if (!map.Tiles[x][y].Transparent())
                            {
                                if (x - 1 >= 0 && map.Tiles[x - 1][y].Transparent())
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, source.xPos, source.yPos, false), source, map, visiblePoints);
                            }
                            else
                            {
                                if (x - 1 >= 0
                                        && map.Tiles[x - 1][y].Transparent())
                                    pStartSlope = -GetSlope(x - 0.5, y + 0.5, source.xPos, source.yPos, false);

                                visiblePoints.Add(new Point(x, y));
                            }
                        }
                        x++;
                    }
                    x--;
                    break;

                case 7:

                    x = source.xPos - pDepth;
                    if (x < 0) return;

                    y = source.yPos + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y >= map.GetHeight()) y = map.GetHeight() - 1;

                    while (GetSlope(x, y, source.xPos, source.yPos, true) <= pEndSlope)
                    {

                        if (GetVisDistance(x, y, source.xPos, source.yPos) <= rangeSquared)
                        {

                            if (!map.Tiles[x][y].Transparent())
                            {
                                if (y + 1 < map.GetHeight() && map.Tiles[x][y + 1].Transparent())
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, source.xPos, source.yPos, true), source, map, visiblePoints);
                            }
                            else
                            {
                                if (y + 1 < map.GetHeight() && map.Tiles[x][y + 1].Transparent())
                                    pStartSlope = -GetSlope(x - 0.5, y + 0.5, source.xPos, source.yPos, true);

                                visiblePoints.Add(new Point(x, y));
                            }
                        }
                        y--;
                    }
                    y++;
                    break;

                case 8: //wnw

                    x = source.xPos - pDepth;
                    if (x < 0) return;

                    y = source.yPos - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y < 0) y = 0;

                    while (GetSlope(x, y, source.xPos, source.yPos, true) >= pEndSlope)
                    {

                        if (GetVisDistance(x, y, source.xPos, source.yPos) <= rangeSquared)
                        {

                            if (!map.Tiles[x][y].Transparent())
                            {
                                if (y - 1 >= 0 && map.Tiles[x][y - 1].Transparent())
                                    ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, source.xPos, source.yPos, true), source, map, visiblePoints);

                            }
                            else
                            {
                                if (y - 1 >= 0 && map.Tiles[x][y - 1].Transparent())
                                    pStartSlope = GetSlope(x - 0.5, y - 0.5, source.xPos, source.yPos, true);

                                visiblePoints.Add(new Point(x, y));
                            }
                        }
                        y++;
                    }
                    y--;
                    break;
            }


            if (x < 0)
                x = 0;
            else if (x >= map.GetWidth())
                x = map.GetWidth() - 1;

            if (y < 0)
                y = 0;
            else if (y >= map.GetHeight())
                y = map.GetHeight() - 1;

            if (pDepth < _range & map.Tiles[x][y].Transparent())
                ScanOctant(pDepth + 1, pOctant, pStartSlope, pEndSlope, source, map, visiblePoints);

        }

        /// <summary>
        /// Get the gradient of the slope formed by the two points
        /// </summary>
        /// <param name="pX1"></param>
        /// <param name="pY1"></param>
        /// <param name="pX2"></param>
        /// <param name="pY2"></param>
        /// <param name="pInvert">Invert slope</param>
        /// <returns></returns>
        private double GetSlope(double pX1, double pY1, double pX2, double pY2, bool pInvert)
        {
            if (pInvert)
                return (pY1 - pY2) / (pX1 - pX2);
            else
                return (pX1 - pX2) / (pY1 - pY2);
        }


        /// <summary>
        /// Calculate the distance between the two points
        /// </summary>
        /// <param name="pX1"></param>
        /// <param name="pY1"></param>
        /// <param name="pX2"></param>
        /// <param name="pY2"></param>
        /// <returns>Distance</returns>
        private int GetVisDistance(int pX1, int pY1, int pX2, int pY2)
        {
            return ((pX1 - pX2) * (pX1 - pX2)) + ((pY1 - pY2) * (pY1 - pY2));
        }
    }
}