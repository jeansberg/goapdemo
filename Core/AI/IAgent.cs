using Core;
using Core.GameObject;
using System.Collections.Generic;

namespace Goap
{
    /// <summary>
    /// Represents the interface between the action planning system and the NPC / Monster
    /// </summary>
    public interface IAgent
    {

        void Start(Creature owner, WorldState worldState);
        void Update();
        bool HasActionPlan();
        IAction NextAction();
        void SetActionPlan(List<IAction> actions);
        WorldState GetWorldState();
        Creature GetOwner();
        void UpdateWorldState(WorldState changes);
        List<WorldState> CreateGoalStates();
        List<IAction> AvailableActions();
        IAgentState MoveToState();
        IAgentState ActState();
        IAgentState IdleState();
        bool MoveToward(IAction action);
    }
}
