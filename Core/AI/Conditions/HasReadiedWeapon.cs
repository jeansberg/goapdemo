using Core.GameObjects;
using System.Collections.Generic;

namespace Core.AI.Goals
{
    public struct HasReadiedWeapon : ICondition
    {
        private readonly Creature _creature;

        public HasReadiedWeapon(Creature creature)
        {
            _creature = creature;
        }
    }
}
