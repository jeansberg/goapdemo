using System;

namespace Core.GameObject
{
    public class CombatComponent
    {
        private int _health;

        public CombatComponent(int health = 5)
        {
            _health = health;
        }

        public int Health { get => _health; set => _health = value; }

        internal void TakeDamage(int points)
        {
            _health -= points;
            Console.WriteLine($"Health is now {_health}");
        }
    }
}
