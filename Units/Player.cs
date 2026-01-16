using GamePrototype.Items.EconomicItems;
using GamePrototype.Items.EquipItems;
using GamePrototype.Utils;
using System.Text;

namespace GamePrototype.Units
{
    public sealed class Player : Unit
    {
        private const int DELTA_ARMOUR_REDUCE = 1;
        private const int WEAPON_DURABILITY_REDUCE = 1;
        private readonly Dictionary<EquipSlot, EquipItem> _equipment = new();

        public Player(string name, uint health, uint maxHealth, uint baseDamage) : base(name, health, maxHealth, baseDamage)
        {            
        }

        public override uint GetUnitDamage()
        {
            if (_equipment.TryGetValue(EquipSlot.Weapon, out var item) && item is Weapon weapon) 
            {
                weapon.ReduceDurability(WEAPON_DURABILITY_REDUCE);
                Console.WriteLine($"Weapon durability reduced. Weapon: {weapon.Durability}");
                return BaseDamage + weapon.Damage;
            }
            return BaseDamage;
        }

        public override void HandleCombatComplete()
        {
            var items = Inventory.Items;
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i] is EconomicItem economicItem)
                {
                    UseEconomicItem(economicItem); // переделать в TryUseEconomicItem. 
                    Inventory.TryRemove(items[i]); // проверять условие если действительно было использовано, только тогда удалять из инвентаря
                    // так же подозрение что тут ошибка, что золото например будет всегда удаляться
                }
            }

        }

        public override void AddItemToInventory(Item item)
        {
            if (item is EquipItem equipItem && _equipment.TryAdd(equipItem.Slot, equipItem)) 
            {
                // Item was equipped
                return;
            }
            base.AddItemToInventory(item);
        }

        private void UseEconomicItem(EconomicItem economicItem)
        {
            if (economicItem is HealthPotion healthPotion) 
            {
                Health += healthPotion.HealthRestore;
                Console.WriteLine($"Health potion used. Health: {Health}");
            }

            if (economicItem is Grindstone grindstone)
            {
                if (_equipment.TryGetValue(EquipSlot.Weapon, out var item) && item is Weapon weapon)
                {
                    weapon.Repair(grindstone.DurabilityRestore);
                    Console.WriteLine($"Grindstone used. Weapon: {weapon.Durability}");
                }
            }
        }

        protected override void DamageReceiveHandler()
        {
            Console.WriteLine("DamageReceiveHandler is working");
            if (_equipment.TryGetValue(EquipSlot.Armour, out var item) && item is Armour armour)
            {
                Console.WriteLine("Armour reduce is working");
                armour.ReduceDurability(DELTA_ARMOUR_REDUCE);
                Console.WriteLine($"Armour reduced. Armour: {armour.Durability}");
            }
            if (_equipment.TryGetValue(EquipSlot.Helmet, out var bufItemForHelmet) && bufItemForHelmet is Helmet helmet)
            {
                Console.WriteLine("Helmet reduce is working");
                helmet.ReduceDurability(DELTA_ARMOUR_REDUCE);
                Console.WriteLine($"Helmet reduced. Helmet: {helmet.Durability}");
            }
        }

        protected override uint CalculateAppliedDamage(uint damage)
        {
            if (_equipment.TryGetValue(EquipSlot.Armour, out var item) && item is Armour armour) 
            {
                damage -= (uint)(damage * (armour.Defence / 100f));
            }
            if (_equipment.TryGetValue(EquipSlot.Helmet, out var bufItemForHelmet) && bufItemForHelmet is Helmet helmet)
            {
                damage -= (uint)(damage * (helmet.Defence / 100f));
            }

            return damage;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(Name);
            builder.AppendLine($"Health {Health}/{MaxHealth}");
            builder.AppendLine("Loot:");
            var items = Inventory.Items;
            for (int i = 0; i < items.Count; i++) 
            {
                builder.AppendLine($"[{items[i].Name}] : {items[i].Amount}");
            }
            return builder.ToString();
        }
    }
}
