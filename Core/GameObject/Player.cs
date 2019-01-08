using Core.AI;
using System.Collections.Generic;

namespace Core.GameObject
{
    public class Player : Creature
    {
        public Player(MapComponent mapComponent, Map.Map map, PathFinder pathfinder, int health = 5) : base(mapComponent, map, pathfinder, health)
        {
        }

        public Player(List<IAction> actions, List<WorldState> goals, MapComponent mapComponent, Map.Map map, int health = 5) : base(actions, goals, mapComponent, map, health)
        {
        }
    }
}
