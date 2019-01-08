using System.Collections.Generic;
using System.Linq;
using Core.AI;
using Core.AI.Goals;
using Goap;
using Goap.Actions;

namespace Core.GameObject
{
    public class CreatureFactory
    {
        private readonly IPathFinder _pathFinder;

        public CreatureFactory(IPathFinder pathFinder)
        {
            _pathFinder = pathFinder;
        }

        public Player CreatePlayer(Map.Map mapRef, Point position)
        {
            var mapComponent = new MapComponent
            {
                Position = position,
                Graphic = new Graphic
                {
                    Character = 'P',
                    ForeColor = new RgbColor(100, 255, 100)
                }
            };

            // The player does not need actions or goals, should be moved somewhere...
            var player = new Player(new List<IAction>(), new List<WorldState>(), mapComponent, mapRef, 15);
            return player;
        }

        public Creature CreateMonster(Map.Map mapRef, Point position, IAgent agent, List<Creature> enemies, WorldState worldState, Dictionary<Creature, IAgent> agentMap)
        {
            var mapComponent = new MapComponent
            {
                Position = position,
                Graphic = new Graphic
                {
                    Character = 'M',
                    ForeColor = new RgbColor(255, 100, 100)
                }
            };
            var monster = new Creature(mapComponent, mapRef, _pathFinder);

            monster.Actions = new List<IAction>();
            monster.Actions.AddRange(enemies.Select(e => new AttackTargetMelee(monster, e)));

            monster.Goals = new List<WorldState>();
            monster.Goals.AddRange(enemies.Select(e => new WorldState(new Dictionary<ICondition, bool> { { new TargetEliminatedCondition(e), true } })));

            agentMap[monster] = agent;
            agent.Start(monster, worldState);

            return monster;
        }

        public Creature CreateNpc(Map.Map mapRef, Point position)
        {
            var mapComponent = new MapComponent
            {
                Position = position,
                Graphic = new Graphic
                {
                    Character = 'N',
                    ForeColor = new RgbColor(100, 100, 255)
                }
            };
            var npc = new Creature(mapComponent, mapRef, _pathFinder);

            return npc;
        }
    }
}
