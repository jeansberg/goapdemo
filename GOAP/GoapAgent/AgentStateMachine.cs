using System.Collections.Generic;

namespace Goap.AgentState
{
    public class AgentStateMachine : IAgentStateMachine
    {
        private readonly Stack<IAgentState> _states;

        public AgentStateMachine()
        {
            _states = new Stack<IAgentState>();
        }

        public void Update()
        {
            if(_states.Count > 0)
            {
                _states.Peek().Update();
            }
        }

        public void Transition(IAgentState state)
        {
            _states.Pop();
            _states.Push(state);
        }

        public void PopState()
        {
            _states.Pop();
        }

        public void PushState(IAgentState state)
        {
            _states.Push(state);
        }
    }
}