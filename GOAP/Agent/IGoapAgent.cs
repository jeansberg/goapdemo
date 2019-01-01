﻿using Core;
using Core.GameObject;
using System.Collections.Generic;

namespace Goap
{
    /// <summary>
    /// Represents the interface between the action planning system and the NPC / Monster
    /// </summary>
    public interface IGoapAgent
    {

        void Start(Creature owner);
        void Update();
        bool HasActionPlan();
        IAction NextAction();
        void SetActionPlan(List<IAction> actions);
        WorldState GetWorldState();
        List<WorldState> CreateGoalStates();
        List<IAction> AvailableActions();
        IAgentState MoveToState();
        IAgentState ActState();
        IAgentState IdleState();
        void MoveToward(IAction action);
    }
}