using Core;
using Goap;
using Goap.AgentState;
using Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Goap.Tests
{
    [TestClass]
    public class AgentStateTests
    {
        private ILogger logger;

        [TestInitialize]
        public void Init()
        {
            logger = new NullLogger();
        }

        [TestMethod]
        public void IdleState_Update_TransitionsToActState()
        {
            var mockStateMachine = new Mock<IAgentStateMachine>();
            var mockAgent = new Mock<IAgent>();
            var mockActState = new Mock<IAgentState>();
            var mockPlanner = new Mock<IGoapPlanner>();
            var plan = new List<IAction> { new Mock<IAction>().Object };

            mockPlanner.Setup(x => x.Plan(mockAgent.Object)).Returns(plan);
            mockAgent.Setup(x => x.ActState()).Returns(mockActState.Object);

            var sut = new IdleState(mockStateMachine.Object, mockAgent.Object, mockPlanner.Object, logger);
            sut.Update();

            mockAgent.Verify(x => x.SetActionPlan(plan), Times.Once);
            mockAgent.Verify(x => x.Update(), Times.Once);
            mockStateMachine.Verify(x => x.Transition(mockActState.Object), Times.Once);
        }

        [TestMethod]
        public void ActState_Update_TransitionsToIdleState_IfNoPlan()
        {
            var mockStateMachine = new Mock<IAgentStateMachine>();
            var mockAgent = new Mock<IAgent>();
            var mockIdleState = new Mock<IAgentState>();

            mockAgent.Setup(x => x.HasActionPlan()).Returns(false);
            mockAgent.Setup(x => x.IdleState()).Returns(mockIdleState.Object);

            var sut = new ActState(mockStateMachine.Object, mockAgent.Object, logger);
            sut.Update();

            mockStateMachine.Verify(x => x.Transition(mockIdleState.Object), Times.Once);
            mockAgent.Verify(x => x.Update(), Times.Once);
        }

        [TestMethod]
        public void ActState_Update_TransitionsToIdleState_IfActionSuccesful()
        {
            var mockStateMachine = new Mock<IAgentStateMachine>();
            var mockAgent = new Mock<IAgent>();
            var mockIdleState = new Mock<IAgentState>();
            var mockAction = new Mock<IAction>();

            mockAgent.Setup(x => x.HasActionPlan()).Returns(true);
            mockAgent.Setup(x => x.IdleState()).Returns(mockIdleState.Object);
            mockAgent.Setup(x => x.NextAction()).Returns(mockAction.Object);
            mockAction.Setup(x => x.NeedsInRange()).Returns(false);
            mockAction.Setup(x => x.Perform()).Returns(true);

            var sut = new ActState(mockStateMachine.Object, mockAgent.Object, logger);
            sut.Update();

            mockStateMachine.Verify(x => x.Transition(mockIdleState.Object), Times.Once);
        }

        [TestMethod]
        public void ActState_Update_TransitionsToMoveToState_IfNotInRange()
        {
            var mockStateMachine = new Mock<IAgentStateMachine>();
            var mockAgent = new Mock<IAgent>();
            var mockMoveToState = new Mock<IAgentState>();
            var mockAction = new Mock<IAction>();

            mockAgent.Setup(x => x.HasActionPlan()).Returns(true);
            mockAgent.Setup(x => x.MoveToState()).Returns(mockMoveToState.Object);
            mockAgent.Setup(x => x.NextAction()).Returns(mockAction.Object);
            mockAction.Setup(x => x.NeedsInRange()).Returns(true);
            mockAction.Setup(x => x.IsInRange()).Returns(false);

            var sut = new ActState(mockStateMachine.Object, mockAgent.Object, logger);
            sut.Update();

            mockStateMachine.Verify(x => x.PushState(mockMoveToState.Object), Times.Once);
            mockAgent.Verify(x => x.Update(), Times.Once);
        }

        [TestMethod]
        public void MoveToState_Update_UpdatesLastState_IfInRange()
        {
            var mockStateMachine = new Mock<IAgentStateMachine>();
            var mockAgent = new Mock<IAgent>();
            var mockAction = new Mock<IAction>();

            mockAgent.Setup(x => x.NextAction()).Returns(mockAction.Object);
            mockAction.Setup(x => x.IsInRange()).Returns(true);

            var sut = new MoveToState(mockStateMachine.Object, mockAgent.Object, logger);
            sut.Update();

            mockStateMachine.Verify(x => x.PopState(), Times.Once);
            mockAgent.Verify(x => x.Update(), Times.Once);
        }

        [TestMethod]
        public void MoveToState_Update_MovesAgent_IfNotInRange()
        {
            var mockStateMachine = new Mock<IAgentStateMachine>();
            var mockAgent = new Mock<IAgent>();
            var mockAction = new Mock<IAction>();

            mockAgent.Setup(x => x.NextAction()).Returns(mockAction.Object);
            mockAction.Setup(x => x.IsInRange()).Returns(false);

            var sut = new MoveToState(mockStateMachine.Object, mockAgent.Object, logger);
            sut.Update();

            mockAgent.Verify(x => x.MoveToward(mockAction.Object), Times.Once);
        }
    }
}
