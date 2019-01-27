namespace Core.AI.Goals
{
    public struct CanSeeTarget : ICondition
    {
        private GameObject.GameObject _target;

        public CanSeeTarget(GameObject.GameObject target)
        {
            _target = target;
        }
    }
}