using Core;
using Core.AI.Goals;
using Core.GameObject;
using System;
using System.Collections.Generic;

namespace Goap.Actions
{
    public class AttackTargetMelee : IAction
    {
        private Creature _actor;
        private Creature _target;
        private int _cost;

        public AttackTargetMelee(Creature actor, Creature target, int cost = 1)
        {
            _actor = actor;
            _target = target;
            _cost = cost;
        }

        public Creature GetActor()
        {
            return _actor;
        }

        public int Cost => _cost;

        public WorldState GetEffects()
        {
            return new WorldState(new Dictionary<ICondition, bool> { { new TargetEliminatedCondition(_target), true } });
        }

        public WorldState GetPreconditions()
        {
            return new WorldState(new Dictionary<ICondition, bool> { { new TargetVisibleCondition(_target), true } });
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
