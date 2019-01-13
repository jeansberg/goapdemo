﻿using Core.GameObject;
using System.Collections.Generic;

namespace Core.AI.Goals
{
    public class TargetEliminatedCondition : ICondition
    {
        private Creature _target;
        public TargetEliminatedCondition(Creature target)
        {
            _target = target;
        }

        public override bool Equals(object obj)
        {
            var condition = obj as TargetEliminatedCondition;
            return condition != null && _target == condition._target;
        }

        public override int GetHashCode()
        {
            return -769192233 + EqualityComparer<Creature>.Default.GetHashCode(_target);
        }
    }
}