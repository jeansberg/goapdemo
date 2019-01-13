using Core.GameObject;
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
        public Graphic Graphic;

        public bool Walkable()
        {
            return Type != TileType.Wall;
        }

        public bool Transparent()
        {
            return Type != TileType.Wall;
        }
    }
}
