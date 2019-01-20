using Core.GameObject;

namespace Core.AI.Goals
{
    public struct HealthyCondition : ICondition
    {
        private Creature _target;
        public HealthyCondition(Creature target)
        {
            _target = target;
        }
    }
}
