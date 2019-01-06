using Core;
using Core.GameObject;
using System;
using System.Collections.Generic;

namespace Goap.Actions
{
    public class AttackTargetRanged : IAction
    {
        private Creature _actor;
        private Creature _target;

        public AttackTargetRanged(Creature actor, Creature target)
        {
            _actor = actor;
            _target = target;
        }

        public Creature GetActor()
        {
            return _actor;
        }

        public int GetCost()
        {
            return 2;
        }

        public WorldState GetEffects()
        {
            return new WorldState(new Dictionary<string, bool> { { "playerDamaged", true } });
        }

        public WorldState GetPreconditions()
        {
            return new WorldState(new Dictionary<string, bool> { { "weaponLoaded", true } });
        }

        public Creature GetTarget()
        {
            return _target;
        }

        public bool IsDone()
        {
            return !_target.IsAlive();
        }

        public bool IsInRange()
        {
            return _actor.MapComponent.Position.IsWithin(5, _target.MapComponent.Position);
        }

        public bool NeedsInRange()
        {
            return true;
        }

        public bool Perform()
        {
            Console.WriteLine("Performed ranged attack");
            _actor.Attack(_target);

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
