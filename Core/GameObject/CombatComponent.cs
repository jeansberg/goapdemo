using System;

namespace Core.GameObject
{
    public class CombatComponent : ICombatComponent
    {
        private int _health;

        public CombatComponent(int health = 5)
        {
            _health = health;
        }

        public int GetHealth()
        {
            return _health;
        }


        public void TakeDamage(int points)
        {
            _health -= points;
            Console.WriteLine($"Health is now {_health}");
        }
    }
}
