using System;

namespace Core.GameObject
{
    public class MapComponent : IMapComponent
    {
        private Point position;
        private readonly bool blocking;

        public MapComponent(Point position, bool blocking = true)
        {
            this.position = position;
            this.blocking = blocking;
        }

        public Point GetPosition()
        {
            return position;
        }

        public void SetPosition(Point value)
        {
            position = value;
        }

        public bool IsBlocking()
        {
            return blocking;
        }
    }
}
