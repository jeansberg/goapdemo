using Core.GameObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AI.Goals
{
    public class HealthyCondition : ICondition
    {
        private Creature _target;
        public HealthyCondition(Creature target)
        {
            _target = target;
        }

        public override bool Equals(object obj)
        {
            var condition = obj as HealthyCondition;
            return condition != null && _target == condition._target;
        }
    }
}
