using Core.GameObject;

namespace Core.AI.Goals
{
    public struct IsHealthy : ICondition
    {
        private Creature _target;
        public IsHealthy(Creature target)
        {
            _target = target;
        }
    }
}
