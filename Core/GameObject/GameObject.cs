namespace Core.GameObject
{
    public abstract class GameObject
    {
        protected MapComponent _mapLocation;

        protected GameObject(MapComponent mapComponent)
        {
            MapComponent = mapComponent;
        }

        public MapComponent MapComponent { get => _mapLocation; set => _mapLocation = value; }
    }
}
