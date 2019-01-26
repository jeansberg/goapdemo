using Core.GameObject;
using System.Collections.Generic;

namespace Core.AI
{
    public interface IPathFinder
    {
        List<Point> PathFind(Point start, Point goal, Map.Map mapRef);
    }
}