using Core.GameObject;

namespace Demo
{
    public class ItemFactory
    {
        public Item CreateMeleeWeapon(Point position)
        {
            var mapComponent = new MapComponent(position, false);

            var graphicsComponent = new GraphicsComponent('/', new RgbColor(200, 100, 100));

            return new Item(mapComponent, graphicsComponent, ItemType.MeleeWeapon);
        }
    }
}