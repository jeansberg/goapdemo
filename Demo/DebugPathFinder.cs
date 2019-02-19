using Core.GameObjects;
using Core.Map;
using FloodSpill;
using System;
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

            (var path, var allNodes) = PathFindInternal(start, goal, mapRef);

            PaintSafetyMap(allNodes, mapRef);

            PaintPath(path.Select(x => x.Position).ToList(), mapRef);

            return path.Select(x => x.Position).ToList();
        }

        private void PaintSafetyMap(List<PathNode> allNodes, Map.Map mapRef)
        {
            foreach (var node in allNodes)
            {
                var point = node.Position;
                var color = new RgbColor(0, Math.Min((node.Cost * 2), 255), 0);
                _renderer.Highlight(point.XPos, point.YPos, color);
            }
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
