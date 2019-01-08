using Core.AI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.GameObject
{
    public class Creature : GameObject
    {
        private int _health;
        public List<IAction> Actions { get; set; }
        public List<WorldState> Goals { get; set; }
        private Map.Map mapRef;
        private readonly IPathFinder _pathfinder;

        public Creature(MapComponent mapComponent, Map.Map map, IPathFinder pathfinder, int health = 5) : base(mapComponent)
        {
            Actions = new List<IAction>();
            Goals = new List<WorldState>();
            mapRef = map;
            _pathfinder = pathfinder;
            _health = health;
        }

        public Creature(List<IAction> actions, List<WorldState> goals, MapComponent mapComponent, Map.Map map, int health = 5) : base(mapComponent)
        {
            Actions = actions;
            Goals = goals;
            mapRef = map;
            _health = health;
        }

        public void MoveAttack(Direction direction)
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
                var creature = mapRef.GetCreature(destination);
                if (creature == null)
                {
                    _mapLocation.Position = destination;
                    return;
                }

                Attack(creature);
            }
        }

        public void Attack(Creature target)
        {
            target.Damage(1);
        }

        public bool IsAlive() { return _health > 0; }

        public void Damage(int points)
        {
            _health -= points;
            Console.WriteLine($"Health is now {_health}");
        }

        public void MoveToward(MapComponent otherMapComponent)
        {
            if (_mapLocation.Position.IsAdjacentTo(otherMapComponent.Position))
            {
                // Cannot get any closer
                return;
            }

            var path = _pathfinder.PathFind(_mapLocation.Position, otherMapComponent.Position, mapRef);

            _mapLocation.Position = path[0];

            Console.WriteLine($"Creature moved to {_mapLocation.Position}");
        }
    }
}