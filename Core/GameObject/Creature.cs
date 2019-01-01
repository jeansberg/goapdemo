using System;
using System.Collections.Generic;

namespace Core.GameObject
{
    public class Creature : GameObject
    {
        public Creature(List<IAction> actions, List<WorldState> goals, MapLocation mapComponent) : base(mapComponent)
        {
            IsAlive = true;
            Actions = actions;
            Goals = goals;
        }

        public bool IsAlive { get; set; }
        public List<IAction> Actions { get; set; }
        public List<WorldState> Goals { get; set; }

        public void MoveToward(MapLocation otherMapComponent)
        {
            if (_mapLocation.Position.IsAdjacentTo(otherMapComponent.Position))
            {
                // Already there
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
        }
    }
}