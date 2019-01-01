using Core;
using Core.GameObject;
using System;
using System.Collections.Generic;

namespace GOAP.Actions
{
    public class AttackTargetRanged : IAction
    {
        public Creature GetActor()
        {
            throw new NotImplementedException();
        }

        public int GetCost()
        {
            return 2;
        }

        public WorldState GetEffects()
        {
            return new WorldState(new Dictionary<string, bool> { { "targetDamaged", true } });
        }

        public WorldState GetPreconditions()
        {
            return new WorldState(new Dictionary<string, bool> { { "weaponLoaded", true } });
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
