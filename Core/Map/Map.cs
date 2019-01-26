using Core.GameObject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Map
{
    public class Map
    {
        private static GraphicsComponent FloorGraphic = new GraphicsComponent('.', RgbColor.CreateDarkGrey());
        private static GraphicsComponent WallGraphic = new GraphicsComponent('X', RgbColor.CreateDarkGrey());
        private static GraphicsComponent DebugGraphic = new GraphicsComponent('.', RgbColor.CreateRed());


        public List<List<Tile>> Tiles { get; set; }
        public List<Creature> Creatures { get; set; }
        public List<Item> Items { get; set; }

        public Map(int width, int height)
        {
            Tiles = new List<List<Tile>>();

            CreateFloor(width, height);
            CreateBounds(width, height);
            CreateObstacles();
        }

        public Creature GetCreature(Point location)
        {
            return Creatures.FirstOrDefault(x => x.MapComponent.GetPosition().Equals(location));
        }

        private void CreateObstacles()
        {
            Tiles[8][5] = new Tile { Type = TileType.Wall };
            Tiles[9][5] = new Tile { Type = TileType.Wall };
            Tiles[9][6] = new Tile { Type = TileType.Wall };
            Tiles[10][6] = new Tile { Type = TileType.Wall };
            Tiles[11][6] = new Tile { Type = TileType.Wall };
            Tiles[12][6] = new Tile { Type = TileType.Wall };
            Tiles[13][6] = new Tile { Type = TileType.Wall };
            Tiles[14][6] = new Tile { Type = TileType.Wall };
            Tiles[15][6] = new Tile { Type = TileType.Wall };


            Tiles[11][8] = new Tile { Type = TileType.Wall };
            Tiles[12][8] = new Tile { Type = TileType.Wall };
            Tiles[13][8] = new Tile { Type = TileType.Wall };
            Tiles[14][8] = new Tile { Type = TileType.Wall };
            Tiles[15][8] = new Tile { Type = TileType.Wall };
            Tiles[16][8] = new Tile { Type = TileType.Wall };
            Tiles[17][8] = new Tile { Type = TileType.Wall };
            Tiles[18][8] = new Tile { Type = TileType.Wall };
            Tiles[19][8] = new Tile { Type = TileType.Wall };
            Tiles[20][8] = new Tile { Type = TileType.Wall };
            Tiles[21][8] = new Tile { Type = TileType.Wall };


        }

        public void Draw(IRenderer renderer)
        {
            for (var x = 0; x < GetWidth(); x++)
            {
                for (var y = 0; y < GetHeight(); y++)
                {
                    DrawTile(renderer, Tiles[x][y], new Point(x, y));
                }
            }
        }

        private void DrawTile(IRenderer renderer, Tile tile, Point position)
        {
            switch (tile.Type)
            {
                case TileType.Floor:
                    renderer.Draw(FloorGraphic, position);
                    break;
                case TileType.Wall:
                    renderer.Draw(WallGraphic, position);
                    break;
                case TileType.Debug:
                    renderer.Draw(DebugGraphic, position);
                    break;
            }
        }

        public bool Blocked(Point position)
        {
            var blockedTile = !Tiles[position.XPos][position.YPos].Walkable();
            var hasCreature =Creatures.Exists(x => x.MapComponent.GetPosition().Equals(position));
            return blockedTile || hasCreature;
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
                        Tiles[x][y] = new Tile { Type = TileType.Wall };
                    }
                    if (y == 0 || y == height - 1)
                    {
                        Tiles[x][y] = new Tile { Type = TileType.Wall };
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
                    Tiles[x].Add(new Tile { Type = TileType.Floor });
                }
            }
        }
    }
}
