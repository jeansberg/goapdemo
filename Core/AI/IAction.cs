using Core.AI.Goals;
using Core.GameObject;

namespace Core
{
    public interface IAction
    {
        int Cost { get; }

        void Reset();
        bool IsDone();
        WorldState GetPreconditions();
        WorldState GetEffects();
        Creature GetTarget();
        Creature GetActor();
        bool Perform();
        bool NeedsInRange();
        bool IsInRange();
        void SetInRange();
    }
}
