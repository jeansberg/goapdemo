﻿using Core.AI.Goals;
using Core.GameObjects;
using Goap.Actions;
using System;
using System.Collections.Generic;

namespace GOAP.Actions
{
    public class PickUpLoot : ActionBase
    {
        public PickUpLoot(Creature actor, MapItem target, int cost = 1) : base(actor, target, cost)
        {
        }

        public override WorldState GetEffects()
        {
            return new WorldState(new Dictionary<ICondition, bool> { { new IsRich(_actor), true } });
        }

        public override WorldState GetPreconditions()
        {
            return new WorldState(new Dictionary<ICondition, bool> { { new CanSeeTarget(_target), true } });
        }

        public override bool IsDone()
        {
            return _actor.Inventory.Contains(((MapItem)_target).InventoryItem);
        }

        public override bool OutOfRange()
        {
            return !_actor.MapComponent
                .GetPosition()
                .Equals(_target.MapComponent.GetPosition());
        }

        public override bool Perform()
        {
            _actor.Inventory.Add(((MapItem)_target).InventoryItem);
            _actor._mapRef.Items.Remove((MapItem)_target);
            return true;
        }

        public override string ToString()
        {
            return $"Pick up {_target}";
        }
    }
}
