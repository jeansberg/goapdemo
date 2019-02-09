using Core.AI.Goals;
using Core.GameObjects;

namespace Core
{
    /// <summary>
    /// An action that can be performed by an AI agent
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// The target the action will be performed on
        /// </summary>
        GameObject Target { get; }
        /// <summary>
        /// The entity performing the action
        /// </summary>
        Creature Actor { get; }
        /// <summary>
        /// The conditions the game world needs to satisfy for the action to be valid
        /// </summary>
        WorldState GetPreconditions();
        /// <summary>
        /// The effects the action will have on the game world if it succeeds
        /// </summary>
        WorldState GetEffects();
        /// <summary>
        /// The cost of the action - the higher the cost, the less likely it will be part of the chose action plan
        /// </summary>
        int Cost { get; }
        /// <summary>
        /// If the action is considered done
        /// </summary>
        bool IsDone();
        /// <summary>
        /// Performs the action
        /// </summary>
        bool Perform();
        /// <summary>
        /// Returns true if the action needs the actor to be in range and it is not
        /// </summary>
        /// <returns></returns>
        bool OutOfRange();
    }
}
