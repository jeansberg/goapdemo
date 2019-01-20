using Core;
using Core.AI.Goals;
using Core.GameObject;
using System;
using System.Collections.Generic;

namespace Goap.Actions
{
    public class ReadyWeapon : IAction
    {
        private Creature _actor;

        public ReadyWeapon(Creature actor)
        {
            _actor = actor;
        }
        public Creature GetActor()
        {
            throw new NotImplementedException();
        }

        public int Cost => 1;

        public WorldState GetEffects()
        {
            return new WorldState(new Dictionary<ICondition, bool> { { new WeaponReadyCondition(_actor), true } });
        }

        public WorldState GetPreconditions()
        {
            return new WorldState();
        }

        public Creature GetTarget()
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
            Console.WriteLine("Performed load weapon");

            return true;
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
