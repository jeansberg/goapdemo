using Core;
using Core.GameObject;
using System;
using System.Collections.Generic;

namespace GOAP.Actions
{
    public class LoadWeapon : IAction
    {
        public Creature GetActor()
        {
            throw new NotImplementedException();
        }

        public int GetCost()
        {
            return 1;
        }

        public WorldState GetEffects()
        {
            return new WorldState(new Dictionary<string, bool> { { "weaponLoaded", true } });
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
