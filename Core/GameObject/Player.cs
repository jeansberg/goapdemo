using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Map;

namespace Core.GameObject
{
    public class Player : Creature
    {
        public Player(MapLocation mapComponent, Map.Map map, Pathfinder pathfinder, int health = 5) : base(mapComponent, map, pathfinder, health)
        {
        }

        public Player(List<IAction> actions, List<WorldState> goals, MapLocation mapComponent, Map.Map map, int health = 5) : base(actions, goals, mapComponent, map, health)
        {
        }
    }
}
