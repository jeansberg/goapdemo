using Core;
using Core.AI.Goals;
using Core.GameObjects;
using Goap.AgentState;
using System.Collections.Generic;

namespace Goap
{
    public class GoapAgent : IAgent
    {
        private readonly IAgentStateMachine _fsm;
        private readonly IGoapPlanner _planner;
        private IAgentState _idle;
        private IAgentState _moveTo;
        private IAgentState _act;
        private WorldState _worldState;
        private Creature _owner;
        private List<IAction> _currentActions;
        private ILogger _logger;

        public GoapAgent(IAgentStateMachine fsm, IGoapPlanner planner, ILogger logger)
        {
            _fsm = fsm;
            _planner = planner;
            _logger = logger;
        }

        public void Start(Creature owner, WorldState worldState)
        {
            _owner = owner;
            _worldState = worldState;

            _idle = new IdleState(_fsm, this, _planner, _logger);
            _moveTo = new MoveToState(_fsm, this, _logger);
            _act = new ActState(_fsm, this, _logger);

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

        public bool MoveToward(IAction action)
        {
            var actor = action.Actor;
            var target = action.Target;

            if (actor.CanSee(target)){
                actor.MoveToward(action.Target.MapComponent);
                return true;
            }

            return false;
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
