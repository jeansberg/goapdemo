using Core.GameObjects;

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
}