using System.Collections.Generic;
using Core;
using Core.GameObject;
using Core.Map;

namespace Demo.Fov
{
    public interface IFovCalculator
    {
        List<Point> GetVisibleCells(Point source, Map map, IRenderer renderer);
    }
}