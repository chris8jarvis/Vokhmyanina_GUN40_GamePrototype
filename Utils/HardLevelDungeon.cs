

using GamePrototype.Dungeon;
using GamePrototype.Items.EconomicItems;
using GamePrototype.Items.EquipItems;

namespace GamePrototype.Utils
{
    public class HardLevelDungeon : DungeonBuilder
    {
        public override DungeonRoom BuildDungeon()
        {
            var enter = CreateEmptyRoom("Enter");
            var monsterGoblinRoom = CreateMonsterRoom("Monster Goblin", UnitFactoryDemo.CreateGoblinEnemy());
            var monsterOrkRoom = CreateMonsterRoom("Monster Ork", UnitFactoryDemo.CreateOrkEnemy());
            var emptyRoom = CreateEmptyRoom("Empty");
            var lootRoom = CreateLootRoom("Loot1", new Gold());
            var lootArmourRoom = CreateLootRoom("Armour", new Armour(30, 30, "Great Armour"));
            var lootStoneRoom = CreateLootRoom("Loot2", new Grindstone("Stone"));
            var finalRoom = CreateLootRoom("Final", new Gold());

            enter.TrySetDirection(Direction.Right, lootArmourRoom);
            enter.TrySetDirection(Direction.Left, monsterGoblinRoom);

            lootArmourRoom.TrySetDirection(Direction.Forward, monsterOrkRoom);
            lootArmourRoom.TrySetDirection(Direction.Left, monsterGoblinRoom);
            monsterGoblinRoom.TrySetDirection(Direction.Forward, lootStoneRoom);

            monsterOrkRoom.TrySetDirection(Direction.Forward, monsterGoblinRoom);
            monsterGoblinRoom.TrySetDirection(Direction.Forward, lootStoneRoom);
            lootStoneRoom.TrySetDirection(Direction.Forward, monsterOrkRoom);

            monsterGoblinRoom.TrySetDirection(Direction.Forward, finalRoom);
            lootStoneRoom.TrySetDirection(Direction.Forward, monsterOrkRoom);
            lootStoneRoom.TrySetDirection(Direction.Forward, monsterOrkRoom);

            monsterOrkRoom.TrySetDirection(Direction.Forward, finalRoom);
            monsterOrkRoom.TrySetDirection(Direction.Forward, finalRoom);

            return enter;
        }
    }
}
