using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.GameObject
{
    public class Creature : GameObject
    {
        private int _health = 5;
        public List<IAction> Actions { get; set; }
        public List<WorldState> Goals { get; set; }
        private Map.Map mapRef;
        private readonly Pathfinder _pathfinder;

        public Creature(MapLocation mapComponent, Map.Map map, Pathfinder pathfinder) : base(mapComponent)
        {
            Actions = new List<IAction>();
            Goals = new List<WorldState>();
            mapRef = map;
            _pathfinder = pathfinder;
        }

        public Creature(List<IAction> actions, List<WorldState> goals, MapLocation mapComponent, Map.Map map) : base(mapComponent)
        {
            Actions = actions;
            Goals = goals;
            mapRef = map;
        }

        public void Move(Direction direction)
        {
            Point destination;
            switch (direction)
            {
                case Direction.Left:
                    {
                        destination = new Point(_mapLocation.Position.xPos - 1, _mapLocation.Position.yPos);
                        break;
                    }
                case Direction.Right:
                    {
                        destination = new Point(_mapLocation.Position.xPos + 1, _mapLocation.Position.yPos);
                        break;
                    }
                case Direction.Up:
                    {
                        destination = new Point(_mapLocation.Position.xPos, _mapLocation.Position.yPos - 1);
                        break;
                    }
                case Direction.Down:
                    {
                        destination = new Point(_mapLocation.Position.xPos, _mapLocation.Position.yPos + 1);
                        break;
                    }
                default:
                    return;
            }

            if (mapRef.Tiles[destination.xPos][destination.yPos].Walkable())
            {
                _mapLocation.Position = destination;
            }
        }

        public bool IsAlive() { return _health > 0; }

        public void Damage(int points)
        {
            _health -= points;
            Console.WriteLine($"Health is now {_health}");
        }

        public void MoveToward(MapLocation otherMapComponent)
        {
            if (_mapLocation.Position.IsAdjacentTo(otherMapComponent.Position))
            {
                // Cannot get any closer
                return;
            }

            foreach (var point in mapRef.Tiles.SelectMany(x => x))
            {
                if(point.Type == Map.TileType.Debug)
                {
                    point.Type = Map.TileType.Floor;
                }
            }

            var path = _pathfinder.PathFind(_mapLocation.Position, otherMapComponent.Position, mapRef);
            foreach(var point in path)
            {
                mapRef.Tiles[point.xPos][point.yPos].Type = Map.TileType.Debug;
            }

            _mapLocation.Position = path[0];

            Console.WriteLine($"Creature moved to {_mapLocation.Position}");
        }
    }
}