using Core;
using Core.AI.Goals;
using Core.GameObjects;

namespace Goap.Actions
{
    public abstract class ActionBase : IAction
    {
        protected Creature _actor;
        protected GameObject _target;
        protected int _cost;

        protected ActionBase(Creature actor, GameObject target, int cost)
        {
            Actor = actor;
            Target = target;
            Cost = cost;
        }

        public Creature Actor { get => _actor; private set => _actor = value; }
        public GameObject Target { get => _target; private set => _target = value; }
        public int Cost { get => _cost; private set => _cost = value; }

        public abstract WorldState GetEffects();

        public abstract WorldState GetPreconditions();

        public abstract bool IsDone();

        public abstract bool OutOfRange();

        public abstract bool Perform();
    }
}
