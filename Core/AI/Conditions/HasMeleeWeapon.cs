using Core.GameObject;

namespace Core.AI.Goals
{
    public struct HasMeleeWeapon : ICondition
    {
        private Creature _actor;
        public HasMeleeWeapon(Creature actor)
        {
            _actor = actor;
        }
    }
}
