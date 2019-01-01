using System.Collections.Generic;
using Core;
using GOAP.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Goap.Tests
{
    [TestClass]
    public class GoapPlannerTests
    {
        [TestMethod]
        public void Plan_ReturnsPlan_ForValidGoalAndActions()
        {
            var mockAgent = new Mock<IGoapAgent>();

            // Create an empty world state and get some actions
            var emptyState = new WorldState();
            var readyWeapon = new LoadWeapon();
            var attackTarget = new AttackTargetRanged();

            mockAgent.Setup(x => x.GetWorldState()).Returns(emptyState);
            mockAgent.Setup(x => x.CreateGoalStates()).Returns(new List<WorldState> { new WorldState(new Dictionary<string, bool> { { "targetDamaged", true } }) });
            mockAgent.Setup(x => x.AvailableActions()).Returns(new List<IAction>() {readyWeapon, attackTarget});

            var sut = new GoapPlanner();
            var plan = sut.Plan(mockAgent.Object);

            CollectionAssert.AreEqual(new List<IAction> { attackTarget, readyWeapon }, plan);
        }

        [TestMethod]
        public void Plan_ReturnsEmpty_WhenActionMissing()
        {
            var mockAgent = new Mock<IGoapAgent>();

            // Create an empty world state and get some actions
            var emptyState = new WorldState();
            var attackTarget = new AttackTargetRanged();

            mockAgent.Setup(x => x.GetWorldState()).Returns(emptyState);
            mockAgent.Setup(x => x.CreateGoalStates()).Returns(new List<WorldState> { new WorldState(new Dictionary<string, bool> { { "targetDamaged", true } }) });
            mockAgent.Setup(x => x.AvailableActions()).Returns(new List<IAction>() { attackTarget });

            var sut = new GoapPlanner();
            var plan = sut.Plan(mockAgent.Object);

            Assert.AreEqual(0, plan.Count);
        }

        [TestMethod]
        public void Plan_ReturnsCheaperPlan_WhenMultiplePossibilities()
        {
            var mockAgent = new Mock<IGoapAgent>();

            // Create an empty world state and get some actions
            var emptyState = new WorldState();
            var readyWeapon = new LoadWeapon();
            var attackTargetRanged = new AttackTargetRanged();
            var attackTargetMelee = new AttackTargetMelee(null, null);

            mockAgent.Setup(x => x.GetWorldState()).Returns(emptyState);
            mockAgent.Setup(x => x.CreateGoalStates()).Returns(new List<WorldState> { new WorldState(new Dictionary<string, bool> { { "targetDamaged", true } }) });
            mockAgent.Setup(x => x.AvailableActions()).Returns(new List<IAction>() { attackTargetRanged, attackTargetMelee, readyWeapon });

            var sut = new GoapPlanner();
            var plan = sut.Plan(mockAgent.Object);

            CollectionAssert.AreEqual(new List<IAction> { attackTargetMelee }, plan);
        }

        [TestMethod]
        public void Plan_ReturnsPlanForHighestPriorityGoal_WhenMultipleGoals ()
        {
            var mockAgent = new Mock<IGoapAgent>();

            // Create an empty world state and get some actions
            var emptyState = new WorldState();
            var readyWeapon = new LoadWeapon();
            var attackTargetRanged = new AttackTargetRanged();
            var attackTargetMelee = new AttackTargetMelee(null, null);
            var heal = new Heal();

            mockAgent.Setup(x => x.GetWorldState()).Returns(emptyState);
            mockAgent.Setup(x => x.CreateGoalStates()).Returns(new List<WorldState> { new WorldState(new Dictionary<string, bool> { { "healthy", true } }), new WorldState(new Dictionary<string, bool> { { "targetDamaged", true } }) });
            mockAgent.Setup(x => x.AvailableActions()).Returns(new List<IAction>() { attackTargetRanged, attackTargetMelee, readyWeapon, heal });

            var sut = new GoapPlanner();
            var plan = sut.Plan(mockAgent.Object);

            CollectionAssert.AreEqual(new List<IAction> { heal }, plan);
        }

        [TestMethod]
        public void Plan_ReturnsEmpty_WhenNoGoals()
        {
            var mockAgent = new Mock<IGoapAgent>();

            // Create an empty world state and get some actions
            var emptyState = new WorldState();
            var readyWeapon = new LoadWeapon();
            var attackTargetRanged = new AttackTargetRanged();
            var attackTargetMelee = new AttackTargetMelee(null, null);
            var heal = new Heal();

            mockAgent.Setup(x => x.GetWorldState()).Returns(emptyState);
            mockAgent.Setup(x => x.CreateGoalStates()).Returns(new List<WorldState>());
            mockAgent.Setup(x => x.AvailableActions()).Returns(new List<IAction>() { attackTargetRanged, attackTargetMelee, readyWeapon, heal });

            var sut = new GoapPlanner();
            var plan = sut.Plan(mockAgent.Object);

            CollectionAssert.AreEqual(new List<IAction>(), plan);
        }
    }
}
