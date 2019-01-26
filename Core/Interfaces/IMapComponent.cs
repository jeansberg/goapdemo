namespace Core.GameObject
{
    public interface IMapComponent
    {
        Point GetPosition();
        bool IsBlocking();
        void SetPosition(Point value);
    }
}