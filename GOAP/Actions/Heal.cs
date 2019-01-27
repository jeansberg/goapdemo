using Core;
using Core.AI.Goals;
using Core.GameObject;
using System;
using System.Collections.Generic;

namespace Goap.Actions
{
    public class Heal : IAction
    {
        private readonly Creature _target;

        public Heal(Creature target)
        {
            _target = target;
        }

        public Creature GetActor()
        {
            throw new NotImplementedException();
        }

        public int Cost => 1;

        public WorldState GetEffects()
        {
            return new WorldState(new Dictionary<ICondition, bool> { { new HealthyCondition(_target), true } });
        }

        public WorldState GetPreconditions()
        {
            return new WorldState();
        }

        public GameObject GetTarget()
        {
            throw new NotImplementedException();
        }

        public bool IsDone()
        {
            throw new NotImplementedException();
        }

        public bool IsInRange()
        {
            throw new NotImplementedException();
        }

        public bool NeedsInRange()
        {
            return false;
        }

        public bool Perform()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            return;
        }

        public void SetInRange()
        {
            throw new NotImplementedException();
        }
    }
}
