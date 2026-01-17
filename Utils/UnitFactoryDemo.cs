using GamePrototype.Items.EconomicItems;
using GamePrototype.Items.EquipItems;
using GamePrototype.Units;

namespace GamePrototype.Utils
{
    public class UnitFactoryDemo
    {
        public static Unit CreatePlayer(string name)
        {
            var player = new Player(name, 30, 30, 6);
            player.AddItemToInventory(new Weapon(10, 15, "Sword"));
            player.AddItemToInventory(new Weapon(100, 25, "Great sword"));
            player.AddItemToInventory(new RangeWeapon(5, 15, "Bow"));
            player.AddItemToInventory(new Armour(10, 15, "Simple armour"));
            player.AddItemToInventory(new Helmet(10, 15, "Dirty hat"));
           // player.AddItemToInventory(new Helmet(55, 25, "Silver helmet"));
            player.AddItemToInventory(new HealthPotion("Potion"));
            player.AddItemToInventory(new Grindstone("Grindstone")); //для теста нужно подобрать grindstone в промежуточной комнате
            return player;
        }

        public static Unit CreateGoblinEnemy() => new Goblin(GameConstants.Goblin, 18, 18, 2);
    }
}
