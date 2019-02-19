namespace Core.Interfaces
{
    public enum ItemType
    {
        MeleeWeapon,
        RangedWeapon,
        Loot
    }

    public struct InventoryItem
    {
        public ItemType Type;
        public string Name;

        public InventoryItem(ItemType type, string name)
        {
            Type = type;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}