using Core.GameObjects;

namespace Core.AI.Goals
{
    public struct IsRich : ICondition
    {
        private Creature _target;
        public IsRich(Creature target)
        {
            _target = target;
        }
    }
}
