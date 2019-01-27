namespace Core.AI.Goals
{
    public struct TargetVisibleCondition : ICondition
    {
        private GameObject.GameObject _target;

        public TargetVisibleCondition(GameObject.GameObject target)
        {
            _target = target;
        }
    }
}