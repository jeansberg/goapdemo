using Core.AI;
using Core.AI.Goals;
using Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Core.GameObjects
{
    public class Creature : GameObject
    {
        public List<IAction> Actions { get; set; }
        public List<WorldState> Goals { get; set; }
        public List<Point> Fov { get; set; }
        public List<InventoryItem> Inventory {get;set;}

        private readonly IPathFinder _pathfinder;
        private readonly string _name;

        public Creature(MapComponent mapComponent, CombatComponent combatComponent, GraphicsComponent graphicsComponent, Map.Map map, IPathFinder pathfinder, string name = "creature") : base(mapComponent, combatComponent, graphicsComponent, map)
        {
            Actions = new List<IAction>();
            Goals = new List<WorldState>();
            _pathfinder = pathfinder;
            _name = name;
            Inventory = new List<InventoryItem>();
        }

        public void MoveAttack(Direction direction)
        {
            Point destination;
            switch (direction)
            {
                case Direction.Left:
                    {
                        destination = new Point(_mapComponent.GetPosition().XPos - 1, _mapComponent.GetPosition().YPos);
                        break;
                    }
                case Direction.Right:
                    {
                        destination = new Point(_mapComponent.GetPosition().XPos + 1, _mapComponent.GetPosition().YPos);
                        break;
                    }
                case Direction.Up:
                    {
                        destination = new Point(_mapComponent.GetPosition().XPos, _mapComponent.GetPosition().YPos - 1);
                        break;
                    }
                case Direction.Down:
                    {
                        destination = new Point(_mapComponent.GetPosition().XPos, _mapComponent.GetPosition().YPos + 1);
                        break;
                    }
                default:
                    return;
            }

            if (_mapRef.Tiles[destination.XPos][destination.YPos].Walkable())
            {
                var creature = _mapRef.GetCreature(destination);
                if (creature == null)
                {
                    _mapComponent.SetPosition(destination);
                    return;
                }

                Attack(creature);
            }
        }

        public bool CanSee(GameObject target)
        {
            return Fov.Contains(target.MapComponent.GetPosition());
        }

        public void Attack(GameObject target)
        {
            target.TakeDamage(1);
        }

        public void MoveToward(MapComponent otherMapComponent)
        {
            var path = _pathfinder.PathFind(_mapComponent.GetPosition(), otherMapComponent.GetPosition(), _mapRef);

            _mapComponent.SetPosition(path[0]);

            Console.WriteLine($"Creature moved to {_mapComponent.GetPosition()}");
        }

        public void Draw(IRenderer renderer)
        {
            renderer.Draw(_graphicsComponent, _mapComponent.GetPosition());
        }

        public override string ToString()
        {
            return _name;
        }
    }
}