using Core;
using Core.GameObject;
using System;
using System.Collections.Generic;

namespace Goap.Actions
{
    public class AttackTargetMelee : IAction
    {
        private Creature _actor;
        private Creature _target;

        public AttackTargetMelee(Creature actor, Creature target)
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
            return 1;
        }

        public WorldState GetEffects()
        {
            return new WorldState(new Dictionary<string, bool> { { "targetDamaged", true } });
        }

        public WorldState GetPreconditions()
        {
            return new WorldState();
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
            return _actor.MapComponent.Position.IsAdjacentTo(_target.MapComponent.Position);
        }

        public bool NeedsInRange()
        {
            return true;
        }

        public bool Perform()
        {
            Console.WriteLine("Performed melee attack");
            _target.Damage(1);

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
