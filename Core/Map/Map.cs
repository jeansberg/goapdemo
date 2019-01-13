using Core.GameObject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Map
{
    public class Map
    {

        public List<List<Tile>> Tiles { get; set; }
        public List<Creature> Creatures { get; set; }

        public Map(int width, int height)
        {
            Tiles = new List<List<Tile>>();

            CreateFloor(width, height);
            CreateBounds(width, height);
            CreateObstacles();
        }

        public Creature GetCreature(Point location)
        {
            return Creatures.FirstOrDefault(x => x.MapComponent.Position.Equals(location));
        }

        private void CreateObstacles()
        {
            Tiles[8][5] = new Tile { Type = TileType.Wall , Graphic = new Graphic {Character = 'X', ForeColor = new RgbColor(100,100,100) } };
            Tiles[9][5] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[9][6] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[10][6] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[11][6] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[12][6] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[13][6] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[14][6] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[15][6] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };


            Tiles[11][8] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[12][8] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[13][8] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[14][8] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[15][8] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[16][8] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[17][8] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[18][8] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[19][8] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[20][8] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
            Tiles[21][8] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };


        }

        internal bool Blocked(Point position)
        {
            var blockedTile = !Tiles[position.xPos][position.yPos].Walkable();
            var hasCreature =Creatures.Exists(x => x.MapComponent.Position.Equals(position));
            return blockedTile || hasCreature;
        }

        public void AddCreatures(List<Creature> creatures)
        {
            Creatures = creatures;
        }

        public int GetWidth()
        {
            return Tiles.Count;
        }

        public int GetHeight()
        {
            return Tiles[0].Count;
        }

        private void CreateBounds(int width, int height)
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if(x == 0 || x == width - 1)
                    {
                        Tiles[x][y] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
                    }
                    if (y == 0 || y == height - 1)
                    {
                        Tiles[x][y] = new Tile { Type = TileType.Wall, Graphic = new Graphic { Character = 'X', ForeColor = new RgbColor(100, 100, 100) } };
                    }
                }
            }
        }

        private void CreateFloor(int width, int height)
        {
            for(var x = 0; x < width; x++)
            {
                Tiles.Add(new List<Tile>());
                for(var y = 0; y < height; y++)
                {
                    Tiles[x].Add(new Tile { Type = TileType.Floor, Graphic = new Graphic { Character = '.', ForeColor = new RgbColor(100, 100, 100) } });
                }
            }
        }
    }
}
