namespace Goap.AgentState
{
    public class MoveToState : IAgentState
    {
        private IAgent _agent;
        private readonly IAgentStateMachine _fsm;

        public MoveToState(IAgentStateMachine fsm, IAgent agent)
        {
            _agent = agent;
            _fsm = fsm;
        }

        public void Update()
        {
            var action = _agent.NextAction();

            if (action.IsInRange())
            {
                _fsm.PopState();
                _agent.Update();
            }
            else
            {
                _agent.MoveToward(action);
            }
        }
    }
}
