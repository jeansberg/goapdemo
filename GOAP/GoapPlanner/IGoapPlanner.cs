using Core;
using System.Collections.Generic;

namespace Goap
{
    /// <summary>
    /// Is responsible for creating a plan for an AI agent based on the agent's goals and actions
    /// </summary>
    public interface IGoapPlanner
    {
        /// <summary>
        /// Attempts to create a plan
        /// </summary>
        /// <param name="agent">The agent to plan for</param>
        /// <returns>A list of actions to execute in order</returns>
        List<IAction> Plan(IGoapAgent agent);
    }
}