using System.Collections.Generic;
using System.Linq;
using Core.AI;
using Core.AI.Goals;
using Goap;
using Goap.Actions;
using GOAP.Actions;

namespace Core.GameObjects
{
    public class CreatureFactory
    {
        private readonly IPathFinder _pathFinder;

        public CreatureFactory(IPathFinder pathFinder)
        {
            _pathFinder = pathFinder;
        }

        public Creature CreatePlayer(Map.Map mapRef, Point position)
        {
            var mapComponent = new MapComponent(position, true);

            var graphicsComponent = new GraphicsComponent('P', new RgbColor(100, 255, 100));

            var combatComponent = new CombatComponent(10);

            // The player does not need actions or goals, should be moved somewhere...
            var player = new Creature(mapComponent, combatComponent, graphicsComponent, mapRef, null, "Player");
            return player;
        }

        public Creature CreateMonster(Map.Map mapRef, Point position, IAgent agent, List<Creature> enemies, WorldState worldState, Dictionary<Creature, IAgent> agentMap)
        {
            var mapComponent = new MapComponent(position, true);

            var graphicsComponent = new GraphicsComponent('M', new RgbColor(255, 100, 100));
            
            var combatCompnent = new CombatComponent(5);

            var monster = new Creature(mapComponent, combatCompnent, graphicsComponent, mapRef, _pathFinder, "Monster");

            monster.Actions = new List<IAction>();
            monster.Actions.AddRange(mapRef.Items
                .Where(x => x.InventoryItem.Type == Interfaces.ItemType.MeleeWeapon)
                .Select(i => new PickUpMeleeWeapon(monster, i)));
            monster.Actions.AddRange(enemies.Select(e => new AttackTargetMelee(monster, e)));

            monster.Goals = new List<WorldState>();
            monster.Goals.AddRange(enemies.Select(e => new WorldState(new Dictionary<ICondition, bool> { { new EliminatedTarget(e), true } })));

            agentMap[monster] = agent;
            agent.Start(monster, worldState);

            return monster;
        }

        public Creature CreateNpc(Map.Map mapRef, Point position, IAgent agent, Dictionary<Creature, IAgent> agentMap, WorldState worldState)
        {
            var mapComponent = new MapComponent(position, true);

            var graphicsComponent = new GraphicsComponent('N', new RgbColor(100, 100, 255));

            var combatCompnent = new CombatComponent(5);

            var npc = new Creature(mapComponent, combatCompnent, graphicsComponent, mapRef, _pathFinder);

            npc.Actions = new List<IAction>();
            npc.Actions.AddRange(mapRef.Items
                .Where(x => x.InventoryItem.Type == Interfaces.ItemType.Loot)
                .Select(i => new PickUpLoot(npc, i)));

            npc.Goals = new List<WorldState>();
            npc.Goals.Add(new WorldState(new Dictionary<ICondition, bool> { { new IsRich(npc), true } }));

            agentMap[npc] = agent;
            agent.Start(npc, worldState);

            return npc;
        }
    }
}
