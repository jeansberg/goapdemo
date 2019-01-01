namespace Goap.AgentState
{
    /// <summary>
    /// Used by an AI agent to maintain a stack of states
    /// </summary>
    public interface IAgentStateMachine
    {
        /// <summary>
        /// Remove the topmost state
        /// </summary>
        void PopState();
        /// <summary>
        /// Add a new state to the top
        /// </summary>
        /// <param name="state"></param>
        void PushState(IAgentState state);
        /// <summary>
        /// Remove the topmost state and add a new state to the top
        /// </summary>
        /// <param name="state"></param>
        void Transition(IAgentState state);
        /// <summary>
        /// Update the topmost state
        /// </summary>
        void Update();
    }
}