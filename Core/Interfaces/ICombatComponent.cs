namespace Core.GameObject
{
    public interface ICombatComponent
    {
        int GetHealth();
        void TakeDamage(int points);
    }
}