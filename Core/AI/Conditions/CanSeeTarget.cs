namespace Core.AI.Goals
{
    public struct CanSeeTarget : ICondition
    {
        private GameObjects.GameObject _target;

        public CanSeeTarget(GameObjects.GameObject target)
        {
            _target = target;
        }
    }
}