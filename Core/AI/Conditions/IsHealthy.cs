using Core.GameObjects;

namespace Core.AI.Goals
{
    public struct IsHealthy : ICondition
    {
        private readonly GameObject _target;
        public IsHealthy(GameObject target)
        {
            _target = target;
        }
    }
}
