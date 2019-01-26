namespace Core.GameObject
{
    public enum ItemType
    {
        MeleeWeapon
    }

    public class Item : GameObject
    {
        private ItemType _type;
        public Item(MapComponent mapComponent, GraphicsComponent graphicsComponent, ItemType type) : base(mapComponent, null, graphicsComponent)
        {
            _type = type;
        }
    }
}
