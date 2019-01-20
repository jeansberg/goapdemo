using Core.AI.Goals;
using Core.GameObject;
using System.Collections.Generic;

namespace Core.AI.Goals
{
    public struct TargetVisibleCondition : ICondition
    {
        private Creature _target;

        public TargetVisibleCondition(Creature target)
        {
            _target = target;
        }
    }
}