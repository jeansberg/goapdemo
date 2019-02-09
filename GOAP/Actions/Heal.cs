using Core;
using Core.AI.Goals;
using Core.GameObjects;
using System;
using System.Collections.Generic;

namespace Goap.Actions
{
    public class Heal : ActionBase
    {
        public Heal(Creature actor, GameObject target, int cost = 0) : base(actor, target, cost)
        {
            _target = target;
        }

        public override WorldState GetEffects()
        {
            return new WorldState(new Dictionary<ICondition, bool> { { new IsHealthy(_target), true } });
        }

        public override WorldState GetPreconditions()
        {
            return new WorldState();
        }

        public override bool IsDone()
        {
            throw new NotImplementedException();
        }

        public override bool OutOfRange()
        {
            throw new NotImplementedException();
        }

        public override bool Perform()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            return;
        }
    }
}
