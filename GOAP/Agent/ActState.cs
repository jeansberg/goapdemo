namespace Goap.AgentState
{
    public class ActState : IAgentState
    {
        private IGoapAgent _agent;
        private readonly IAgentStateMachine _fsm;

        public ActState(IAgentStateMachine fsm, IGoapAgent agent)
        {
            _agent = agent;
            _fsm = fsm;
        }

        public void Update()
        {
            if (_agent.HasActionPlan())
            {
                var action = _agent.NextAction();
                bool inRange = action.NeedsInRange() ? action.IsInRange() : true;

                if (inRange)
                {
                    var success = action.Perform();
                    if (success)
                    {
                        // Action was successful - go back to being idle
                        _fsm.Transition(_agent.IdleState());
                    }
                }
                else
                {
                    // Action not in range - start moving towards it
                    _fsm.PushState(_agent.MoveToState());
                    _agent.Update();
                }
            }
            else
            {
                // No actions to take - go back to being idle
                _fsm.Transition(_agent.IdleState());
                _agent.Update();
            }
        }
    }
}
