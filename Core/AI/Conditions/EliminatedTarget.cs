using Core.GameObjects;
using System.Collections.Generic;

namespace Core.AI.Goals
{
    public struct EliminatedTarget : ICondition
    {
        private readonly GameObject _target;
        public EliminatedTarget(GameObject target)
        {
            _target = target;
        }
    }
}