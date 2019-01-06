using System;

namespace Core.Map
{
    public enum TileType
    {
        Floor,
        Wall,
        Debug
    }

    public class Tile
    {
        public TileType Type;

        internal bool Walkable()
        {
            return Type != TileType.Wall;
        }
    }
}
