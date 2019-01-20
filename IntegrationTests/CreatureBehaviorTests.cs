using Core;
using Core.AI;
using Core.AI.Goals;
using Core.GameObject;
using FloodSpill;
using Goap.Actions;
using Goap.AgentState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Goap.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private CombatComponent GetCombatComponent()
        {
            return new CombatComponent();
        }

        private Creature GetMeleeCreature(Creature target)
        {
            var mapComponent = new MapComponent
            {
                Position = new Point(5, 5)
            };
            var creature = new Creature(mapComponent, GetCombatComponent(), null, null, new PathFinder(new FloodSpiller()))
            {
                Fov = new List<Point>
            {
                target.MapComponent.Position
            }
            };

            var action = new AttackTargetMelee(creature, target);
            var goal = new WorldState()
            {
                Conditions = new Dictionary<ICondition, bool> { { new TargetEliminatedCondition(target), true } }
            };

            creature.Actions.Add(action);
            creature.Goals.Add(goal);

            return creature;
        }

        private Creature GetRangedCreature(Creature target)
        {
            var mapComponent = new MapComponent
            {
                Position = new Point(5, 5)
            };
            var creature = new Creature(mapComponent, GetCombatComponent(), null, null, null);

            var reload = new ReadyWeapon(creature);
            var rangedAttack = new AttackTargetRanged(creature, target);
            var goal = new WorldState()
            {
                Conditions = new Dictionary<ICondition, bool>
                {
                    { new TargetEliminatedCondition(target), true }
                }
            };

            creature.Actions.Add(rangedAttack);
            creature.Actions.Add(reload);
            creature.Goals.Add(goal);

            return creature;
        }

        private Creature GetTarget()
        {
            var mapComponent = new MapComponent
            {
                Position = new Point(0, 0)
            };

            return new Creature(new List<IAction>(), new List<WorldState>(), mapComponent, GetCombatComponent(), null, null);
        }

        private IAgent GetAgent()
        {
            var fsm = new AgentStateMachine();
            var planner = new GoapPlanner();

            return new GoapAgent(fsm, planner);
        }

        [TestMethod]
        public void Creature_KillsTarget_WithMeleeAttack()
        {
            var target = GetTarget();

            var state = new WorldState(new Dictionary<ICondition, bool>
                {
                    { new TargetVisibleCondition(target), true }
                });

            var creature = GetMeleeCreature(target);

            var agent = GetAgent();
            agent.Start(creature, state);

            while (target.IsAlive())
            {
                agent.Update();
            }
        }

        [TestMethod]
        public void Creature_KillsTarget_WithRangedAttack()
        {
            var target = GetTarget();

            var state = new WorldState(new Dictionary<ICondition, bool>
                {
                    { new TargetVisibleCondition(target), true }
                });

            var creature = GetRangedCreature(target);
            var agent = GetAgent();
            agent.Start(creature, state);

            while (target.IsAlive())
            {
                agent.Update();
            }
        }
    }
}