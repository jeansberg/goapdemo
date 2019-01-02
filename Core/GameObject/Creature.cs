using System;
using System.Collections.Generic;

namespace Core.GameObject
{
    public class Creature : GameObject
    {
        private int _health = 5;
        public List<IAction> Actions { get; set; }
        public List<WorldState> Goals { get; set; }

        public Creature(MapLocation mapComponent) : base(mapComponent)
        {
            Actions = new List<IAction>();
            Goals = new List<WorldState>();
        }

        public Creature(List<IAction> actions, List<WorldState> goals, MapLocation mapComponent) : base(mapComponent)
        {
            Actions = actions;
            Goals = goals;
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

            if(Math.Abs(_mapLocation.Position.xPos - otherMapComponent.Position.xPos) > 1)
            {
                if(otherMapComponent.Position.xPos > _mapLocation.Position.xPos)
                {
                    _mapLocation.Position.xPos++;
                }
                else
                {
                    _mapLocation.Position.xPos--;
                }
            }
            else
            {
                if (otherMapComponent.Position.yPos > _mapLocation.Position.yPos)
                {
                    _mapLocation.Position.yPos++;
                }
                else
                {
                    _mapLocation.Position.yPos--;
                }
            }

            Console.WriteLine($"Creature moved to {_mapLocation.Position}");
        }
    }
}