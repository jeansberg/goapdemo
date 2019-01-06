using Core;
using Core.GameObject;
using Goap.AgentState;
using System.Collections.Generic;

namespace Goap
{
    public class GoapAgent : IGoapAgent
    {
        private readonly IAgentStateMachine _fsm;
        private readonly IGoapPlanner _planner;
        private IAgentState _idle;
        private IAgentState _moveTo;
        private IAgentState _act;
        private WorldState _worldState;
        private Creature _owner;
        private List<IAction> _currentActions;

        public GoapAgent(IAgentStateMachine fsm, IGoapPlanner planner)
        {
            _fsm = fsm;
            _planner = planner;
        }

        public void Start(Creature owner, WorldState worldState)
        {
            _owner = owner;
            _worldState = worldState;

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
            return _owner.Actions;
        }

        public List<WorldState> CreateGoalStates()
        {
            return _owner.Goals;
        }

        public WorldState GetWorldState()
        {
            return _worldState;
        }

        public Creature GetOwner()
        {
            return _owner;
        }

        public void UpdateWorldState(WorldState changed)
        {
            _worldState = _worldState.GetUpdatedState(changed);
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
