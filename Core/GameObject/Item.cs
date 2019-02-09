using Core.Interfaces;

namespace Core.GameObjects
{
    public class MapItem : GameObject
    {
        private InventoryItem _item;
        public MapItem(MapComponent mapComponent, GraphicsComponent graphicsComponent, InventoryItem item) : base(mapComponent, null, graphicsComponent, null)
        {
            InventoryItem = item;
        }

        public InventoryItem InventoryItem { get => _item; set => _item = value; }
    }
}
