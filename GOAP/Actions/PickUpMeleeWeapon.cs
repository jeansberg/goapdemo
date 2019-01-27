using Core;
using Core.AI.Goals;
using Core.GameObject;
using System;
using System.Collections.Generic;

namespace GOAP.Actions
{
    public class PickUpMeleeWeapon : IAction
    {
        private Creature _actor;
        private MapItem _target;

        public PickUpMeleeWeapon(Creature actor, MapItem target)
        {
            _actor = actor;
            _target = target;
        }

        public int Cost => 1;

        public Creature GetActor()
        {
            return _actor;
        }

        public WorldState GetEffects()
        {
            return new WorldState(new Dictionary<ICondition, bool> { { new HasMeleeWeaponCondition(_actor), true } });
        }

        public WorldState GetPreconditions()
        {
            return new WorldState(new Dictionary<ICondition, bool> { { new TargetVisibleCondition(_target), true } });
        }

        public GameObject GetTarget()
        {
            return _target;
        }

        public bool IsDone()
        {
            return _actor.Inventory.Contains(_target.InventoryItem);
        }

        public bool IsInRange()
        {
            return _actor.MapComponent.GetPosition().Equals(_target.MapComponent.GetPosition());
        }

        public bool NeedsInRange()
        {
            return true;
        }

        public bool Perform()
        {
            _actor.Inventory.Add(_target.InventoryItem);
            _actor._mapRef.Items.Remove(_target);
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
