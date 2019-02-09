using Core;
using Core.GameObjects;
using Core.Map;
using System;
using System.Collections.Generic;

namespace Demo.Fov
{

    /// <summary>
    /// Implementation of "FOV using recursive shadowcasting - improved" as
    /// described on http://roguebasin.roguelikedevelopment.org/index.php?title=FOV_using_recursive_shadowcasting_-_improved
    /// </summary>
    public class ShadowCastFovCalculator : IFovCalculator
    {
        private int _range;
        public ShadowCastFovCalculator(int range)
        {
            _range = range;
        }

        public List<Point> GetVisibleCells(Point source, Map map, IRenderer renderer)
        {
            var visiblePoints = new List<Point>();
            Implementation.ComputeVisibility(map, source, _range, visiblePoints);

            return visiblePoints;
        }
    }
}