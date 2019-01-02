using FloodSpill;
using FloodSpill.Utilities;
using System;
using System.Collections.Generic;

namespace Core.GameObject
{
    public class Creature : GameObject
    {
        private int _health = 5;
        public List<IAction> Actions { get; set; }
        public List<WorldState> Goals { get; set; }
        private Map.Map mapRef;

        public Creature(MapLocation mapComponent, Map.Map map) : base(mapComponent)
        {
            Actions = new List<IAction>();
            Goals = new List<WorldState>();
            mapRef = map;
        }

        public Creature(List<IAction> actions, List<WorldState> goals, MapLocation mapComponent, Map.Map map) : base(mapComponent)
        {
            Actions = actions;
            Goals = goals;
            mapRef = map;
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

            PathFind(otherMapComponent.Position);

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

        private void PathFind(Point point)
        {
            var markMatrix = new int[mapRef.Tiles.Count, mapRef.Tiles[0].Count];
            var floodParameters = new FloodParameters(startX: _mapLocation.Position.xPos, startY: _mapLocation.Position.yPos);

            new FloodSpiller().SpillFlood(floodParameters, markMatrix);

            // Find the first step counting FROM the target
            Point step = null;
            var minVal = int.MaxValue;
            //List<Point> path = GetPath()
            for (int x = 0; x < mapRef.Tiles.Count; x++)
            {
                for (int y = 0; y < mapRef.Tiles[0].Count; y++)
                {
                    if (point.IsAdjacentTo(new Point(x, y)))
                    {
                        if(markMatrix[x, y] < minVal)
                        {
                            minVal = markMatrix[x, y];
                            step = new Point(x, y);
                        }
                    }

                }
            }

        }
    }
}