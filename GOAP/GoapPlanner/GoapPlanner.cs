using Core;
using Core.AI.Goals;
using System.Collections.Generic;
using System.Linq;

namespace Goap
{
    public class GoapPlanner : IGoapPlanner
    {
        public List<IAction> Plan(IAgent agent)
        {
            var usableActions = agent.AvailableActions();
            usableActions.ForEach(a => a.Reset());

            var nodes = new List<ActionNode>();
            var worldState = agent.GetWorldState();
            var startNode = ActionNode.StartNode(worldState);
            var goals = agent.CreateGoalStates();

            // Try to make a plan for each of the goal states until successful
            bool success = false;

            success = FindPlan(usableActions, nodes, startNode, goals, success);

            if (!success || nodes.Count == 0)
            {
                return new List<IAction>();
            }

            var cheapest = nodes.OrderBy(x => x.RunningCost).FirstOrDefault();

            var actionPlan = new List<IAction>();

            while (cheapest != null)
            {
                if (cheapest.Action != null)
                {
                    actionPlan.Add(cheapest.Action);
                }
                cheapest = cheapest.Parent;
            }

            actionPlan.Reverse();
            return actionPlan;
        }

        /// <summary>
        /// Attempts to find an action plan for each goal in order. Stops as soon as it finds one.
        /// </summary>
        private bool FindPlan(List<IAction> usableActions, List<ActionNode> nodes, ActionNode startNode, List<WorldState> goals, bool success)
        {
            foreach (var goal in goals)
            {
                success = BuildTree(startNode, nodes, usableActions, goal);
                if (success)
                {
                    break;
                }
            }

            return success;
        }

        /// <summary>
        /// Recursively builds a tree of actions with all branches leading from the parent node to the goal
        /// </summary>
        private bool BuildTree(ActionNode parent, List<ActionNode> nodes, List<IAction> usableActions, WorldState goal)
        {
            bool foundSolution = false;
            var state = parent.State;

            foreach (var action in usableActions)
            {
                var canPerform = state.Fulfills(action.GetPreconditions());

                if (canPerform)
                {
                    var newState = state.GetUpdatedState(action.GetEffects());

                    var node = new ActionNode(parent, parent.RunningCost + action.Cost, newState, action);

                    if (newState.Fulfills(goal))
                    {
                        nodes.Add(node);
                        foundSolution = true;
                    }
                    else
                    {
                        // not at a solution yet, so test all the remaining actions and branch out the tree
                        var subset = usableActions.Except(new List<IAction> { action }).ToList();
                        bool found = BuildTree(node, nodes, subset, goal);

                        if (found)
                        {
                            foundSolution = true;
                        }
                    }
                }
            }

            return foundSolution;
        }
    }
}