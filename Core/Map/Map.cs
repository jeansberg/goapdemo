using System;
using System.Collections.Generic;

namespace Core.Map
{
    public class Map
    {
        private List<List<Tile>> _tiles;

        public List<List<Tile>> Tiles { get => _tiles; set => _tiles = value; }

        public Map(int width, int height)
        {
            _tiles = new List<List<Tile>>();

            CreateFloor(width, height);
            CreateWalls(width, height);
        }

        private void CreateWalls(int width, int height)
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if(x == 0 || x == width - 1)
                    {
                        _tiles[x][y] = new Tile { Type = TileType.Wall };
                    }
                    if (y == 0 || y == height - 1)
                    {
                        _tiles[x][y] = new Tile { Type = TileType.Wall };
                    }
                }
            }
        }

        private void CreateFloor(int width, int height)
        {
            for(var x = 0; x < width; x++)
            {
                _tiles.Add(new List<Tile>());
                for(var y = 0; y < height; y++)
                {
                    _tiles[x].Add(new Tile { Type = TileType.Floor });
                }
            }
        }
    }
}
