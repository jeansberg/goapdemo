using Core;

namespace Goap.AgentState
{
    public class MoveToState : IAgentState
    {
        private IAgent _agent;
        private readonly IAgentStateMachine _fsm;
        private readonly ILogger _logger;

        public MoveToState(IAgentStateMachine fsm, IAgent agent, ILogger logger)
        {
            _agent = agent;
            _fsm = fsm;
            _logger = logger;
        }

        public void Update()
        {
            var action = _agent.NextAction();

            if (action.IsInRange())
            {
                _logger.Log("In range!");
                _fsm.PopState();
                _agent.Update();
            }
            else
            {
                _logger.Log("Moving towards target");
                _agent.MoveToward(action);
            }
        }
    }
}
