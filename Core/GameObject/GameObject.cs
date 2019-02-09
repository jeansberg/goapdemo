namespace Core.GameObjects
{
    public abstract class GameObject
    {
        protected MapComponent _mapComponent;
        protected CombatComponent _combatComponent;
        protected GraphicsComponent _graphicsComponent;
        public Map.Map _mapRef;

        protected GameObject(MapComponent mapComponent, CombatComponent combatComponent, GraphicsComponent graphicsComponent, Map.Map mapRef)
        {
            _mapComponent = mapComponent;
            _combatComponent = combatComponent;
            _graphicsComponent = graphicsComponent;
            _mapRef = mapRef;
        }

        public MapComponent MapComponent { get => _mapComponent; set => _mapComponent = value; }
        public CombatComponent CombatComponent { get => _combatComponent; set => _combatComponent = value; }
        public GraphicsComponent GraphicsComponent { get => _graphicsComponent; set => _graphicsComponent = value; }

        public void TakeDamage(int points)
        {
            _combatComponent.TakeDamage(points);
        }
        public bool IsAlive() { return _combatComponent.GetHealth() > 0; }
    }
}
