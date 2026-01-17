using GamePrototype.Items.EconomicItems;
using GamePrototype.Items.EquipItems;
using GamePrototype.Utils;
using System.Collections.Generic;
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
            else if (_equipment.TryGetValue(EquipSlot.RangeWeapon, out var item1) && item1 is RangeWeapon rangeWeapon)
            {
                rangeWeapon.ReduceDurability(WEAPON_DURABILITY_REDUCE);
                Console.WriteLine($"Bow durability reduced. Bow: {rangeWeapon.Durability}");
                return BaseDamage + rangeWeapon.Damage;
            }
            return BaseDamage;
        }

        public override void HandleCombatComplete()
        {
            // переделано в использование из инвентаря, чтобы бездумно не пылесосился инвентарь
            // см. InventoryUsage()

            //var items = Inventory.Items;
            //for (int i = items.Count - 1; i >= 0; i--)
            //{
            //    if (items[i] is EconomicItem economicItem)
            //    {
            //        UseEconomicItem(economicItem); // переделать в TryUseEconomicItem. 
            //        Inventory.TryRemove(items[i]); // проверять условие если действительно было использовано, только тогда удалять из инвентаря
            //        // так же подозрение что тут ошибка, что золото например будет всегда удаляться
            //    }
            //}
        }

        public void UseEconomicItem(EconomicItem economicItem)
        {
            if (economicItem is HealthPotion healthPotion) 
            {
                Heal(healthPotion.HealthRestore);
                Console.WriteLine($"Health potion used. Health: {Health}");
            }

            if (economicItem is Grindstone grindstone)
            {
                if (_equipment.Count > 0)
                {
                    UseGrindstone(grindstone.DurabilityRestore);
                }
            }
        }

        private void Heal(uint healthRestore)
        {
            if (Health + healthRestore > MaxHealth)
            {
                Health = MaxHealth;
            }
            else
            {
                Health += healthRestore;
            }
        }
        
        private string ShowEquipment()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Equipment:");

            int i = 0;
            foreach (var slot in Enum.GetValues<EquipSlot>())
            {
                if (_equipment.TryGetValue(slot, out var item))
                {
                    builder.AppendLine($"{i}. [{slot}] {item.Name} - Durability: {item.Durability}/{item.MaxDurability}");
                }
                else
                {
                    builder.AppendLine($"{i}. [{slot}] - Empty");
                }
                i++;
            }
            return builder.ToString();
        }

        private void UseGrindstone(uint durabilityRestore)
        {
            var slots = Enum.GetValues<EquipSlot>();
            while (true)
            {
                Console.WriteLine(ShowEquipment());
                Console.WriteLine("Text number for item to fix with Grindstone or 'e' for exit");
                string playerCommand = Console.ReadLine();
                if (playerCommand == "e")
                {
                    break;
                }
                if (int.TryParse(playerCommand, out int index) &&
                    index >= 0 && 
                    index < slots.Length)
                {
                    EquipSlot selectedSlot = slots[index];

                    if (_equipment.TryGetValue(selectedSlot, out var equipItem))
                    {
                        equipItem.Repair(durabilityRestore);
                        Console.WriteLine($"{equipItem.Name} is repaired: {equipItem.Durability}/{equipItem.MaxDurability}");
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Slot {selectedSlot} is empty!");
                    }
                }
                else
                {
                    Console.WriteLine("Wrong command");
                }
            }
        }

        public override void AddItemToInventory(Item item)
        {
            if (item is EquipItem newItem)
            {
                if (_equipment.TryGetValue(newItem.Slot, out var equippedItem)) // если слот занят
                {
                    Console.WriteLine($"Do you want to exchange {equippedItem.Name} to {newItem.Name}? [y/n]");
                    if (Console.ReadLine() == "y")
                    {
                        EquipItemsExchange(equippedItem, newItem);
                        return;
                    }
                    base.AddItemToInventory(newItem);
                    return;
                }
                _equipment.TryAdd(newItem.Slot, newItem);
                Console.WriteLine($"{newItem.Slot} equipped with {newItem.Name}");
                return;
            }

            base.AddItemToInventory(item);
        }

        private void EquipItemsExchange(EquipItem equippedItem, EquipItem newItem)
        {
            _equipment.Remove(equippedItem.Slot);
            base.AddItemToInventory(equippedItem);

            _equipment.TryAdd(newItem.Slot, newItem); // мы тут не обрабатываем ошибку, хотя с другой стороны, что может пойти не так?
            Console.WriteLine($"{newItem.Slot} equipped with {newItem.Name}");
        }

        protected override void DamageReceiveHandler()
        {
            if (_equipment.TryGetValue(EquipSlot.Armour, out var item) && item is Armour armour)
            {
                armour.ReduceDurability(DELTA_ARMOUR_REDUCE);
                Console.WriteLine($"Armour reduced. Armour: {armour.Durability}");
            }
            if (_equipment.TryGetValue(EquipSlot.Helmet, out var bufItemForHelmet) && bufItemForHelmet is Helmet helmet)
            {
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

        public override string ShowInventory()
        {
            var items = Inventory.Items;
            if (items.Count == 0)
            {
                return "Inventory is empty";
            }

            var builder = new StringBuilder();
            builder.AppendLine("Inventory");
            for (int i = 0; i < items.Count; i++)
            {
                builder.AppendLine($"{i} - {items[i].Name}");
            }
            return builder.ToString();
        }

        public override void InventoryUsage()
        {
            while (true)
            {
                Console.Write(ShowInventory());
                Console.WriteLine("Text number for use or 'e' for exit inventory");
                string playerCommand = Console.ReadLine();
                if (playerCommand == "e")
                {
                    break;
                }
                else if (int.TryParse(playerCommand, out int index) &&
                         index >= 0 &&
                         index < Inventory.Items.Count)
                {
                    var item = Inventory.Items[index];
                    if (item is EquipItem equipItem)
                    {
                        Inventory.TryRemove(equipItem);
                        AddItemToInventory(equipItem);
                    }
                    else if (item is EconomicItem economicItem)
                    {
                        UseEconomicItem(economicItem);
                        Inventory.TryRemove(economicItem);
                    }
                }
                else
                {
                    Console.WriteLine($"Wrong command");
                }
            }
        }
    }
}
