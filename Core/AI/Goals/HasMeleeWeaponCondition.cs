using Core.GameObject;

namespace Core.AI.Goals
{
    public struct HasMeleeWeaponCondition : ICondition
    {
        private Creature _actor;
        public HasMeleeWeaponCondition(Creature actor)
        {
            _actor = actor;
        }
    }
}
