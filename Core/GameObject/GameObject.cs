namespace Core.GameObject
{
    public abstract class GameObject
    {
        protected MapComponent _mapComponent;
        protected CombatComponent _combatComponent;
        protected GraphicsComponent _graphicsComponent;

        protected GameObject(MapComponent mapComponent, CombatComponent combatComponent, GraphicsComponent graphicsComponent)
        {
            _mapComponent = mapComponent;
            _combatComponent = combatComponent;
            _graphicsComponent = graphicsComponent;
        }

        public MapComponent MapComponent { get => _mapComponent; set => _mapComponent = value; }
        public CombatComponent CombatComponent { get => _combatComponent; set => _combatComponent = value; }
        public GraphicsComponent GraphicsComponent { get => _graphicsComponent; set => _graphicsComponent = value; }
    }
}
