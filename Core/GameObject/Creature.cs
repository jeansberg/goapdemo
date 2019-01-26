using Core.AI;
using Core.AI.Goals;
using System;
using System.Collections.Generic;

namespace Core.GameObject
{
    public class Creature : GameObject
    {
        public List<IAction> Actions { get; set; }
        public List<WorldState> Goals { get; set; }
        public List<Point> Fov { get; set; }

        private Map.Map mapRef;
        private readonly IPathFinder _pathfinder;
        private readonly string _name;

        public Creature(MapComponent mapComponent, CombatComponent combatComponent, GraphicsComponent graphicsComponent, Map.Map map, IPathFinder pathfinder, string name = "creature") : base(mapComponent, combatComponent, graphicsComponent)
        {
            Actions = new List<IAction>();
            Goals = new List<WorldState>();
            mapRef = map;
            _pathfinder = pathfinder;
            _name = name;
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

            if (mapRef.Tiles[destination.XPos][destination.YPos].Walkable())
            {
                var creature = mapRef.GetCreature(destination);
                if (creature == null)
                {
                    _mapComponent.SetPosition(destination);
                    return;
                }

                Attack(creature);
            }
        }

        public bool CanSee(Creature target)
        {
            return Fov.Contains(target.MapComponent.GetPosition());
        }

        public void Attack(Creature target)
        {
            target.TakeDamage(1);
        }

        public bool IsAlive() { return _combatComponent.GetHealth() > 0; }

        public void TakeDamage(int points)
        {
            _combatComponent.TakeDamage(points);
        }

        public void MoveToward(MapComponent otherMapComponent)
        {
            if (_mapComponent.GetPosition().IsAdjacentTo(otherMapComponent.GetPosition()))
            {
                // Cannot get any closer
                return;
            }

            var path = _pathfinder.PathFind(_mapComponent.GetPosition(), otherMapComponent.GetPosition(), mapRef);

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