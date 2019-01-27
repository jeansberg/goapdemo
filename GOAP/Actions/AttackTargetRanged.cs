using Core;
using Core.AI.Goals;
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

        public int Cost => 2;

        public WorldState GetEffects()
        {
            return new WorldState(new Dictionary<ICondition, bool> {
                { new EliminatedTarget(_target), true },
                { new HasReadiedWeapon(_actor), false} });
        }

        public WorldState GetPreconditions()
        {
            return new WorldState(new Dictionary<ICondition, bool> {
                { new HasReadiedWeapon(_actor), true },
                { new CanSeeTarget(_target), true }});
        }

        public GameObject GetTarget()
        {
            return _target;
        }

        public bool IsDone()
        {
            return !_target.IsAlive();
        }

        public bool IsInRange()
        {
            return _actor.MapComponent.GetPosition().IsWithin(5, _target.MapComponent.GetPosition());
        }

        public bool NeedsInRange()
        {
            return true;
        }

        public bool Perform()
        {
            Console.WriteLine("Performed ranged attack");
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
