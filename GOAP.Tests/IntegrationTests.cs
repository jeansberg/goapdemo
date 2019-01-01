using Core;
using Core.GameObject;
using Goap;
using Goap.Actions;
using Goap.AgentState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goap.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private Creature GetCreature(Creature target)
        {
            var mapComponent = new MapLocation
            {
                Position = new Point(5, 5)
            };
            var creature = new Creature(mapComponent);

            var action = new AttackTargetMelee(creature, target);
            var goal = new WorldState()
            {
                Conditions = new Dictionary<string, bool>
                {
                    { "targetDamaged", true }
                }
            };

            creature.Actions.Add(action);
            creature.Goals.Add(goal);

            return creature;
        }

        private Creature GetTarget()
        {
            var mapComponent = new MapLocation
            {
                Position = new Point(0, 0)
            };

            return new Creature(new List<IAction>(), new List<WorldState>(), mapComponent);
        }

        private IGoapAgent GetAgent()
        {
            var fsm = new AgentStateMachine();
            var planner = new GoapPlanner();

            return new GoapAgent(fsm, planner);
        }

        [TestMethod]
        public void Creature_KillsTarget_WithMeleeAttack()
        {
            var target = GetTarget();
            var creature = GetCreature(target);
            var agent = GetAgent();
            agent.Start(creature);

            while (target.IsAlive())
            {
                agent.Update();
            }
        }
    }
}
