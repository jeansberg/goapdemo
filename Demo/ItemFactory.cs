using System;
using Core.GameObject;
using Core.Interfaces;

namespace Demo
{
    public class ItemFactory
    {
        public MapItem CreateSword(Point position)
        {
            var mapComponent = new MapComponent(position, false);

            var graphicsComponent = new GraphicsComponent('/', new RgbColor(200, 100, 100));

            return new MapItem(mapComponent, graphicsComponent, new InventoryItem(ItemType.MeleeWeapon, "Sword"));
        }

        internal MapItem CreateLoot(Point position)
        {
            var mapComponent = new MapComponent(position, false);

            var graphicsComponent = new GraphicsComponent('*', new RgbColor(200, 200, 50));

            return new MapItem(mapComponent, graphicsComponent, new InventoryItem(ItemType.Loot, "Loot"));
        }
    }
}