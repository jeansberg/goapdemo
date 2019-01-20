using Core.GameObject;
using System.Collections.Generic;

namespace Core.AI.Goals
{
    public struct WeaponReadyCondition : ICondition
    {
        private readonly Creature _creature;

        public WeaponReadyCondition(Creature creature)
        {
            _creature = creature;
        }
    }
}
