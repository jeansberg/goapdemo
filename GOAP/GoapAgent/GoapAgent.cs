using Core;
using Core.GameObject;
using Goap;
using Goap.AgentState;
using System.Collections.Generic;

namespace GOAP.GoapAgent
{
    public class GoapAgent : IGoapAgent
    {
        private readonly IAgentStateMachine _fsm;
        private readonly IGoapPlanner _planner;
        private IAgentState _idle;
        private IAgentState _moveTo;
        private IAgentState _act;
        private List<IAction> _availableActions;
        private List<WorldState> _goals;
        private List<IAction> _currentActions;

        public GoapAgent(IAgentStateMachine fsm, IGoapPlanner planner)
        {
            _fsm = fsm;
            _planner = planner;
        }

        public void Start(Creature owner)
        {
            _availableActions = owner.Actions;
            _goals = owner.Goals;

            _idle = new IdleState(_fsm, this, _planner);
            _moveTo = new MoveToState(_fsm, this);
            _act = new ActState(_fsm, this);

            _fsm.PushState(_idle);
        }

        public void Update()
        {
            _fsm.Update();
        }

        public IAgentState IdleState()
        {
            return _idle;
        }

        public IAgentState MoveToState()
        {
            return _moveTo;
        }

        public IAgentState ActState()
        {
            return _act;
        }

        public List<IAction> AvailableActions()
        {
            return _availableActions;
        }

        public List<WorldState> CreateGoalStates()
        {
            return _goals;
        }

        public WorldState GetWorldState()
        {
            return new WorldState();
        }

        public bool HasActionPlan()
        {
            return _currentActions.Count > 0;
        }

        public void MoveToward(IAction action)
        {
            var actor = action.GetActor();

            actor.MoveToward(action.GetTarget().MapComponent);
        }

        public IAction NextAction()
        {
            return _currentActions[0];
        }

        public void SetActionPlan(List<IAction> actions)
        {
            _currentActions = actions;
        }
    }
}
