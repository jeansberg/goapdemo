using Core.AI.Goals;
using Core.GameObject;
using System.Collections.Generic;

namespace Goap.Actions
{
    public class TargetVisibleCondition : ICondition
    {
        private Creature _target;

        public TargetVisibleCondition(Creature target)
        {
            _target = target;
        }

        public override bool Equals(object obj)
        {
            var condition = obj as TargetVisibleCondition;
            return condition != null && _target == condition._target;
        }

        public override int GetHashCode()
        {
            return -769192233 + EqualityComparer<Creature>.Default.GetHashCode(_target);
        }
    }
}