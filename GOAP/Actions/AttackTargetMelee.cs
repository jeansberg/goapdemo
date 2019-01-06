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
            if(_target.GetType() == typeof(Player))
            {
                return new WorldState(new Dictionary<string, bool> { { "playerDamaged", true } });
            }
            return new WorldState(new Dictionary<string, bool> { { "npcDamaged", true } });
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
            _actor.Attack(_target);

            return IsDone();
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
