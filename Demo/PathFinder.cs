﻿using FloodSpill;
using FloodSpill.Utilities;
using Core.Map;
using System.Collections.Generic;
using System.Linq;
using Core.GameObject;

namespace Core.AI
{
    public class PathNode
    {
        public Point Position;
        public bool Visited;
        public bool Blocked;
        public int Cost;

        public PathNode(Point position, int cost, bool blocked)
        {
            Position = position;
            Cost = cost;
            Blocked = blocked;
        }
    }

    public class PathFinder : IPathFinder
    {
        private static PathFinder _instance;
        FloodSpiller _floodSpiller;

        public PathFinder(FloodSpiller floodSpiller)
        {
            _floodSpiller = floodSpiller;
        }

        public virtual List<Point> PathFind(Point start, Point goal, Map.Map mapRef)
        {
            return PathFindInternal(start, goal, mapRef);
        }

        protected List<Point> PathFindInternal(Point start, Point goal, Map.Map mapRef)
        {
            var nodes = GetNodes(goal, mapRef);

            var startNode = new PathNode(start, 0, false);

            List<PathNode> path = new List<PathNode>();
            BuildPath(startNode, goal, nodes, path);
            return path.Select(x => x.Position).ToList();
        }

        private void BuildPath(PathNode currentNode, Point goal, List<PathNode> allNodes, List<PathNode> path)
        {
            var adjacentNodes = allNodes.Where(x => !x.Blocked && x.Position.IsAdjacentTo(currentNode.Position)).ToList();
            var nextNode = adjacentNodes.Where(x => !x.Visited).OrderBy(x => x.Cost).FirstOrDefault();

            if(nextNode == null)
            {
                return; 
            }

            adjacentNodes.ForEach(x => x.Visited = true);
            path.Add(nextNode);

            if (nextNode.Position.IsAdjacentTo(goal)){
                return;
            }

            BuildPath(nextNode, goal, allNodes, path);
        }

        private List<PathNode> GetNodes(Point goal, Map.Map mapRef)
        {
            var markMatrix = new int[mapRef.GetWidth(), mapRef.GetHeight()];
            Predicate<int, int> positionQualifier = (x, y) => !mapRef.Blocked(new Point(x,y)); 
            var floodParameters = new FloodParameters(startX: goal.XPos, startY: goal.YPos)
            {
                NeighbourhoodType = NeighbourhoodType.Four,
                Qualifier = positionQualifier
            };

            _floodSpiller.SpillFlood(floodParameters, markMatrix);
            string representation = MarkMatrixVisualiser.Visualise(markMatrix);

            var nodes = new List<PathNode>();
            for(int x = 0; x < mapRef.GetWidth(); x++)
            {
                for (int y = mapRef.GetHeight() - 1; y >= 0; y--)
                {
                    nodes.Add(new PathNode(new Point(x, y), markMatrix[x, y], mapRef.Tiles[x][y].Type == TileType.Wall));
                }
            }
            return nodes;
        }
    }
}