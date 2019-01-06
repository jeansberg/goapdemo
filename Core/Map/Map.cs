using Core.GameObject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Map
{
    public class Map
    {
        private List<List<Tile>> _tiles;
        private List<Creature> _creatures;

        public List<List<Tile>> Tiles { get => _tiles; set => _tiles = value; }

        public Map(int width, int height)
        {
            _tiles = new List<List<Tile>>();

            CreateFloor(width, height);
            CreateBounds(width, height);
            CreateObstacles();
        }

        public Creature GetCreature(Point location)
        {
            return _creatures.FirstOrDefault(x => x.MapComponent.Position.Equals(location));
        }

        private void CreateObstacles()
        {
            _tiles[8][5] = new Tile { Type = TileType.Wall };
            _tiles[9][5] = new Tile { Type = TileType.Wall };
            _tiles[9][6] = new Tile { Type = TileType.Wall };
            _tiles[10][6] = new Tile { Type = TileType.Wall };
            _tiles[11][6] = new Tile { Type = TileType.Wall };
            _tiles[12][6] = new Tile { Type = TileType.Wall };

            _tiles[11][8] = new Tile { Type = TileType.Wall };
            _tiles[12][8] = new Tile { Type = TileType.Wall };
            _tiles[13][8] = new Tile { Type = TileType.Wall };
            _tiles[14][8] = new Tile { Type = TileType.Wall };

            _tiles[9][6] = new Tile { Type = TileType.Wall };
            _tiles[10][6] = new Tile { Type = TileType.Wall };
            _tiles[11][6] = new Tile { Type = TileType.Wall };
            _tiles[12][6] = new Tile { Type = TileType.Wall };
        }

        public void AddCreatures(List<Creature> creatures)
        {
            _creatures = creatures;
        }

        public int GetWidth()
        {
            return _tiles.Count;
        }

        public int GetHeight()
        {
            return _tiles[0].Count;
        }

        private void CreateBounds(int width, int height)
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
