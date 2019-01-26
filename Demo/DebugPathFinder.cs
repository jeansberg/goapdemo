using Core.GameObject;
using FloodSpill;
using System.Collections.Generic;
using System.Linq;

namespace Core.AI
{
    public class DebugPathFinder : PathFinder
    {
        private IRenderer _renderer;

        public DebugPathFinder(FloodSpiller floodSpiller, IRenderer renderer) : base(floodSpiller)
        {
            _renderer = renderer;
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
                if (point.Type == Map.TileType.Debug)
                {
                    point.Type = Map.TileType.Floor;
                }
            }
        }

        private void PaintPath(List<Point> path, Map.Map mapRef)
        {
            foreach (var point in path)
            {
                mapRef.Tiles[point.XPos][point.YPos].Type = Map.TileType.Debug;
            }
        }
    }
}
