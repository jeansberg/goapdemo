using Core.GameObject;
using System.Collections.Generic;

namespace Core.AI.Goals
{
    public struct TargetEliminatedCondition : ICondition
    {
        private Creature _target;
        public TargetEliminatedCondition(Creature target)
        {
            _target = target;
        }
    }
}