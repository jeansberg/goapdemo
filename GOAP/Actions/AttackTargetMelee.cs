using Core.AI.Goals;
using Core.GameObjects;
using System.Collections.Generic;

namespace Goap.Actions
{
    public class AttackTargetMelee : ActionBase
    {
        public AttackTargetMelee(Creature actor, GameObject target, int cost = 1) : base(actor, target, cost)
        {
        }

        public override WorldState GetEffects()
        {
            return new WorldState(new Dictionary<ICondition, bool> {
                { new EliminatedTarget(_target), true } });
        }

        public override WorldState GetPreconditions()
        {
            return new WorldState(new Dictionary<ICondition, bool> {
                { new CanSeeTarget(_target), true },
                { new HasMeleeWeapon (Actor), true }});
        }

        public override bool IsDone() => !_target.IsAlive();

        public override bool OutOfRange() => !Actor.MapComponent
                .GetPosition()
                .IsAdjacentTo(_target.MapComponent.GetPosition());

        public override bool Perform()
        {
            Actor.Attack(_target);

            return IsDone();
        }

        public override string ToString() => $"Melee attack {_target}";
    }
}
