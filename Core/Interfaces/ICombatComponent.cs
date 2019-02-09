namespace Core.GameObjects
{
    public interface ICombatComponent
    {
        int GetHealth();
        void TakeDamage(int points);
    }
}