using FloodSpill;
using FloodSpill.Utilities;
using Core.Map;
using System.Collections.Generic;
using System.Linq;
using Core.GameObjects;

namespace Core.AI
{
    public class PathFinder : IPathFinder
    {
        FloodSpiller _floodSpiller;

        public PathFinder(FloodSpiller floodSpiller)
        {
            _floodSpiller = floodSpiller;
        }

        public void CreateSafetyMap()
        {
            throw new System.NotImplementedException();
        }

        public virtual List<Point> PathFind(Point start, Point goal, Map.Map mapRef)
        {
            return PathFindInternal(start, goal, mapRef).pathNodes.Select(x => x.Position).ToList();
        }

        protected (List<PathNode> pathNodes, List<PathNode> allNodes) PathFindInternal(Point start, Point goal, Map.Map mapRef)
        {
            var nodes = GetNodes(goal, mapRef);

            var startNode = new PathNode(start, 0, false);

            var path = new List<PathNode>();
            BuildPath(startNode, goal, nodes, path);
            return (path, nodes);
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
            bool isNotBlocked(int xPos, int yPos) => !mapRef.Blocked(new Point(xPos, yPos));
            var floodParameters = new FloodParameters(startX: goal.XPos, startY: goal.YPos)
            {
                NeighbourhoodType = NeighbourhoodType.Four,
                Qualifier = isNotBlocked
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