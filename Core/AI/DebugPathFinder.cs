using Core.GameObject;
using FloodSpill;
using System.Collections.Generic;
using System.Linq;

namespace Core.AI
{
    public class DebugPathFinder : PathFinder
    {
        public DebugPathFinder(FloodSpiller floodSpiller) : base(floodSpiller)
        {
        }

        public override List<Point> PathFind(Point start, Point goal, Map.Map mapRef)
        {
            ClearPath(mapRef);

            var path = PathFindInternal(start, goal, mapRef);

            PaintPath(path, mapRef);

            return path;
        }

        private void ClearPath(Map.Map mapRef)
        {
            foreach (var point in mapRef.Tiles.SelectMany(x => x))
            {
                if (point.Graphic.BackColor.Equals(new RgbColor { Red = 100, Green = 0, Blue = 0 }))
                {
                    point.Graphic.BackColor = new RgbColor { Red = 0, Green = 0, Blue = 0 };
                }
            }
        }

        private void PaintPath(List<Point> path, Map.Map mapRef)
        {
            foreach (var point in path)
            {
                mapRef.Tiles[point.xPos][point.yPos].Graphic.BackColor = new RgbColor { Red = 100, Green = 0, Blue = 0 };
            }
        }
    }
}
