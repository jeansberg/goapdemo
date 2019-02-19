﻿using Core.GameObjects;

namespace Core.AI.Goals
{
    public struct HasRangedWeapon : ICondition
    {
        private Creature _actor;
        public HasRangedWeapon(Creature actor)
        {
            _actor = actor;
        }
    }
}
