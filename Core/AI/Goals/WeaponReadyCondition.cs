using Core.GameObject;
using System.Collections.Generic;

namespace Core.AI.Goals
{
    public class WeaponReadyCondition : ICondition
    {
        private readonly Creature _creature;

        public WeaponReadyCondition(Creature creature)
        {
            _creature = creature;
        }

        public override bool Equals(object obj)
        {
            var condition = obj as WeaponReadyCondition;
            return condition != null && _creature == condition._creature;
        }
    }
}
