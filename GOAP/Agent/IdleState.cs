using Core;

namespace Goap.AgentState
{
    /// <summary>
    /// Represents a state in which the agent does not have an action plan
    /// </summary>
    public class IdleState : IAgentState
    {
        private IAgent _agent;
        private readonly IAgentStateMachine _fsm;
        private readonly IGoapPlanner _planner;
        private readonly ILogger _logger;

        public IdleState(IAgentStateMachine fsm, IAgent agent, IGoapPlanner planner, ILogger logger)
        {
            _agent = agent;
            _fsm = fsm;
            _planner = planner;
            _logger = logger;
        }

        public void Update()
        {
            var actionPlan = _planner.Plan(_agent);

            if(actionPlan.Count == 0)
            {
                _logger.Log("No plan...");
                return;
            }

            _logger.Log("New plan!");

            _agent.SetActionPlan(actionPlan);
            _fsm.Transition(_agent.ActState());
            _agent.Update();
        }
    }
}
