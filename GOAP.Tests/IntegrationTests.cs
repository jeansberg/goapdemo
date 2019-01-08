//using Core;
//using Core.GameObject;
//using Goap.Actions;
//using Goap.AgentState;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Collections.Generic;

//namespace Goap.Tests
//{
//    [TestClass]
//    public class IntegrationTests
//    {
//        private Creature GetMeleeCreature(Creature target)
//        {
//            var mapComponent = new MapLocation
//            {
//                Position = new Point(5, 5)
//            };
//            var creature = new Creature(mapComponent, null, null);

//            var action = new AttackTargetMelee(creature, target);
//            var goal = new WorldState()
//            {
//                Conditions = new Dictionary<string, bool>
//                {
//                    { "playerDamaged", true }
//                }
//            };

//            creature.Actions.Add(action);
//            creature.Goals.Add(goal);

//            return creature;
//        }

//        private Creature GetRangedCreature(Creature target)
//        {
//            var mapComponent = new MapLocation
//            {
//                Position = new Point(5, 5)
//            };
//            var creature = new Creature(mapComponent, null, null);

//            var reload = new LoadWeapon();
//            var rangedAttack = new AttackTargetRanged(creature, target);
//            var goal = new WorldState()
//            {
//                Conditions = new Dictionary<string, bool>
//                {
//                    { "playerDamaged", true }
//                }
//            };

//            creature.Actions.Add(rangedAttack);
//            creature.Actions.Add(reload);
//            creature.Goals.Add(goal);

//            return creature;
//        }

//        private Creature GetTarget()
//        {
//            var mapComponent = new MapLocation
//            {
//                Position = new Point(0, 0)
//            };

//            return new Creature(new List<IAction>(), new List<WorldState>(), mapComponent, null);
//        }

//        private IAgent GetAgent()
//        {
//            var fsm = new AgentStateMachine();
//            var planner = new GoapPlanner();

//            return new GoapAgent(fsm, planner);
//        }

//        [TestMethod]
//        public void Creature_KillsTarget_WithMeleeAttack()
//        {
//            var state = new WorldState();

//            var target = GetTarget();
//            var creature = GetMeleeCreature(target);
//            var agent = GetAgent();
//            agent.Start(creature, state);

//            while (target.IsAlive())
//            {
//                agent.Update();
//            }
//        }

//        [TestMethod]
//        public void Creature_KillsTarget_WithRangedAttack()
//        {
//            var state = new WorldState();

//            var target = GetTarget();
//            var creature = GetRangedCreature(target);
//            var agent = GetAgent();
//            agent.Start(creature, state);

//            while (target.IsAlive())
//            {
//                agent.Update();
//            }
//        }
//    }
//}