using Core.GameObject;
using System.Collections.Generic;

namespace Core.AI.Goals
{
    public struct EliminatedTarget : ICondition
    {
        private Creature _target;
        public EliminatedTarget(Creature target)
        {
            _target = target;
        }
    }
}