using System.Collections.Generic;
using Core;
using Core.AI.Goals;
using Core.GameObject;
using Goap.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Goap.Tests
{
    [TestClass]
    public class GoapPlannerTests
    {
        private ICondition GetPlayerDeadCondition(Creature creature)
        {
            return new TargetEliminatedCondition(creature);
        }

        [TestMethod]
        public void Plan_ReturnsPlan_ForValidGoalAndActions()
        {
            var dummyCreature = new Creature(new MapComponent(), new CombatComponent(), null, null, null);
            var dummyTarget = new Creature(new MapComponent(), new CombatComponent(), null, null, null);
            var mockAgent = new Mock<IAgent>();

            // Create an empty world state and get some actions
            var emptyState = new WorldState();
            emptyState.Conditions.Add(new TargetVisibleCondition(dummyTarget), true);
            var readyWeapon = new ReadyWeapon(dummyCreature);
            var attackTarget = new AttackTargetRanged(dummyCreature, dummyTarget);

            mockAgent.Setup(x => x.GetWorldState()).Returns(emptyState);
            mockAgent.Setup(x => x.CreateGoalStates()).Returns(new List<WorldState> { new WorldState(new Dictionary<ICondition, bool> { { GetPlayerDeadCondition(dummyTarget), true } }) });
            mockAgent.Setup(x => x.AvailableActions()).Returns(new List<IAction>() { readyWeapon, attackTarget });

            var sut = new GoapPlanner();
            var plan = sut.Plan(mockAgent.Object);

            CollectionAssert.AreEqual(new List<IAction> { attackTarget, readyWeapon }, plan);
        }

        [TestMethod]
        public void Plan_ReturnsEmpty_WhenActionMissing()
        {
            var dummyTarget = new Creature(new MapComponent(), new CombatComponent(), null, null, null);
            var mockAgent = new Mock<IAgent>();

            // Create an empty world state and get some actions
            var emptyState = new WorldState();
            var attackTarget = new AttackTargetRanged(dummyTarget, dummyTarget);

            mockAgent.Setup(x => x.GetWorldState()).Returns(emptyState);
            mockAgent.Setup(x => x.CreateGoalStates()).Returns(new List<WorldState> { new WorldState(new Dictionary<ICondition, bool> { { GetPlayerDeadCondition(dummyTarget), true } }) });
            mockAgent.Setup(x => x.AvailableActions()).Returns(new List<IAction>() { attackTarget });

            var sut = new GoapPlanner();
            var plan = sut.Plan(mockAgent.Object);

            Assert.AreEqual(0, plan.Count);
        }

        [TestMethod]
        public void Plan_ReturnsCheaperPlan_WhenMultiplePossibilities()
        {
            var dummyCreature = new Creature(new MapComponent(), new CombatComponent(),null, null, null);
            var dummyTarget = new Creature(new MapComponent(), new CombatComponent(),null, null, null);
            var mockAgent = new Mock<IAgent>();

            // Create a world state and get some actions
            var targetInFovState = new WorldState();
            targetInFovState.Conditions.Add(new TargetVisibleCondition(dummyTarget), true);
            var readyWeapon = new ReadyWeapon(dummyCreature);
            var attackTargetRanged = new AttackTargetRanged(dummyCreature, dummyTarget);
            var attackTargetMelee = new AttackTargetMelee(dummyCreature, dummyTarget);

            mockAgent.Setup(x => x.GetWorldState()).Returns(targetInFovState);
            mockAgent.Setup(x => x.CreateGoalStates()).Returns(new List<WorldState> { new WorldState(new Dictionary<ICondition, bool> { { GetPlayerDeadCondition(dummyTarget), true } }) });
            mockAgent.Setup(x => x.AvailableActions()).Returns(new List<IAction>() { attackTargetRanged, attackTargetMelee, readyWeapon });

            var sut = new GoapPlanner();
            var plan = sut.Plan(mockAgent.Object);

            CollectionAssert.AreEqual(new List<IAction> { attackTargetMelee }, plan);
        }

        [TestMethod]
        public void Plan_ReturnsPlanForHighestPriorityGoal_WhenMultipleGoals()
        {
            var dummyCreature = new Creature(new MapComponent(), new CombatComponent(),null, null, null);
            var mockAgent = new Mock<IAgent>();

            // Create an empty world state and get some actions
            var emptyState = new WorldState();
            var readyWeapon = new ReadyWeapon(dummyCreature);
            var attackTargetRanged = new AttackTargetRanged(null, null);
            var attackTargetMelee = new AttackTargetMelee(null, null);
            var heal = new Heal(dummyCreature);

            mockAgent.Setup(x => x.GetWorldState()).Returns(emptyState);
            mockAgent.Setup(x => x.CreateGoalStates()).Returns(new List<WorldState> { new WorldState(new Dictionary<ICondition, bool> { { new HealthyCondition(dummyCreature), true } }), new WorldState(new Dictionary<ICondition, bool> { { new TargetEliminatedCondition(null), true } }) });
            mockAgent.Setup(x => x.AvailableActions()).Returns(new List<IAction>() { attackTargetRanged, attackTargetMelee, readyWeapon, heal });

            var sut = new GoapPlanner();
            var plan = sut.Plan(mockAgent.Object);

            CollectionAssert.AreEqual(new List<IAction> { heal }, plan);
        }

        //[TestMethod]
        //public void Plan_ReturnsEmpty_WhenNoGoals()
        //{
        //    var mockAgent = new Mock<IAgent>();

        //    // Create an empty world state and get some actions
        //    var emptyState = new WorldState();
        //    var readyWeapon = new LoadWeapon();
        //    var attackTargetRanged = new AttackTargetRanged(null, null);
        //    var attackTargetMelee = new AttackTargetMelee(null, null);
        //    var heal = new Heal();

        //    mockAgent.Setup(x => x.GetWorldState()).Returns(emptyState);
        //    mockAgent.Setup(x => x.CreateGoalStates()).Returns(new List<WorldState>());
        //    mockAgent.Setup(x => x.AvailableActions()).Returns(new List<IAction>() { attackTargetRanged, attackTargetMelee, readyWeapon, heal });

        //    var sut = new GoapPlanner();
        //    var plan = sut.Plan(mockAgent.Object);

        //    CollectionAssert.AreEqual(new List<IAction>(), plan);
        //}
    }
}
