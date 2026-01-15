using GamePrototype.Utils;

namespace GamePrototype.Items.EquipItems
{
    public sealed class Weapon : EquipItem
    {
        public Weapon(uint damage, uint durability, string name) : base(durability, name) => Damage = damage; // добавить присвоение прочности

        public uint Damage { get; }

        public override EquipSlot Slot => EquipSlot.Weapon;
    }
}
