using Core.GameObject;

namespace Core.AI.Goals
{
    public class TargetEliminatedCondition : ICondition
    {
        private Creature _target;
        public TargetEliminatedCondition(Creature target)
        {
            _target = target;
        }

        public override bool Equals(object obj)
        {
            var condition = obj as TargetEliminatedCondition;
            return condition != null && _target == condition._target;
        }
    }
}