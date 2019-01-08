using Core.AI.Goals;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    /// <summary>
    /// Encapsulates a set of conditions representing the (known) state of the game world
    /// </summary>
    public class WorldState
    {
        public Dictionary<ICondition, bool> Conditions { get; set; }

        public WorldState()
        {
            Conditions = new Dictionary<ICondition, bool>();
        }

        public WorldState(Dictionary<ICondition, bool> initialConditions)
        {
            Conditions = initialConditions;
        }
    }

    public static class WorldStateExtensions
    {
        /// <summary>
        /// Returns a new state by updating or inserting conditions to the current one
        /// </summary>
        public static WorldState GetUpdatedState(this WorldState currentState, WorldState stateChange)
        {
            var newState = new Dictionary<ICondition, bool>();

            foreach (var currentCondition in currentState.Conditions)
            {
                newState.Add(currentCondition.Key, currentCondition.Value);
            }

            foreach (var change in stateChange.Conditions)
            {
                newState[change.Key] = change.Value;
            }
            return new WorldState(newState);
        }

        /// <summary>
        /// Checks if the current state fulfills all of the conditions of the target state
        /// </summary>
        public static bool Fulfills(this WorldState current, WorldState target)
        {
            bool allMatch = true;

            var relevantConditions = current.Conditions
                .Where(x => target.Conditions.Keys.Count(y => y.Equals(x.Key)) > 0)
                .ToDictionary(x => x.Key, x => x.Value);

            foreach (var targetCondition in target.Conditions)
            {
                bool match = false;
                foreach (var currentCondition in relevantConditions)
                {
                    if (currentCondition.Equals(targetCondition))
                    {
                        match = true;
                        break;
                    }
                }
                if (!match)
                    allMatch = false;
            }
            return allMatch;
        }
    }
}
