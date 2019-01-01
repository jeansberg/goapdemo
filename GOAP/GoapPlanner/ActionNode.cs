using Core;

namespace Goap
{
    /// <summary>
    /// Represents a node in an action graph
    /// </summary>
    public class ActionNode
    {
        public ActionNode(ActionNode parent, int runningCost, WorldState state, IAction action)
        {
            Parent = parent;
            RunningCost = runningCost;
            State = state;
            Action = action;
        }

        public WorldState State { get; set; }

        /// <summary>
        /// Gets the starting node based on a world state
        /// </summary>
        public static ActionNode StartNode(WorldState worldState)
        {
            return new ActionNode(null, 0, worldState, null);
        }

        public int RunningCost { get; private set; }
        public IAction Action { get; private set; }
        public ActionNode Parent { get; private set; }
    }
}
