using System;

namespace Core.GameObject
{
    public struct MapComponent
    {
        public Point Position { get; set; }
        public bool Blocking { get; set; }
        public Graphic Graphic { get; set; }
    }
}
