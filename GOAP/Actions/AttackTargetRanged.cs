//using Core;
//using Core.AI.Goals;
//using Core.GameObjects;
//using System;
//using System.Collections.Generic;

//namespace Goap.Actions
//{
//    public class AttackTargetRanged : ActionBase
//    {
//        public AttackTargetRanged(GameObject actor, GameObject target, int cost = 1) : base(actor, target, cost)
//        {
//        }

//        public override WorldState GetEffects()
//        {
//            return new WorldState(new Dictionary<ICondition, bool> {
//                { new EliminatedTarget(_target), true },
//                { new HasReadiedWeapon(_actor), false} });
//        }

//        public override WorldState GetPreconditions()
//        {
//            return new WorldState(new Dictionary<ICondition, bool> {
//                { new HasMeleeWeapon (_actor), true },
//                { new HasReadiedWeapon(_actor), true },
//                { new CanSeeTarget(_target), true }});
//        }

//        public override bool IsDone() => !_target.IsAlive();

//        public override bool Perform()
//        {
//            _actor.Attack(_target);

//            return IsDone();
//        }

//        public override bool OutOfRange()
//        {
//            return !actor.MapComponent
//                .GetPosition()
//                .IsWithin(5, _target.MapComponent.GetPosition());
//        }

//        public override string ToString() => $"Ranged attack {_target}";
//    }
//}
