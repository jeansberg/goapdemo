namespace Core.GameObject
{
    public class GameObject
    {
        protected MapLocation _mapLocation;

        public GameObject(MapLocation mapComponent)
        {
            MapComponent = mapComponent;
        }

        public MapLocation MapComponent { get => _mapLocation; set => _mapLocation = value; }
    }
}
