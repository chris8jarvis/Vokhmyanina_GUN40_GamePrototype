
using GamePrototype.Dungeon;
using GamePrototype.Items.EconomicItems;
using GamePrototype.Items.EquipItems;
using GamePrototype.Units.Factories;

namespace GamePrototype.Utils
{
    public sealed class HardLevelDungeonBuilder : DungeonBuilder
    {
        public override DungeonRoom BuildDungeon(UnitFactory unitFactory)
        {
            var enter = CreateEmptyRoom("Enter");
            var monsterGoblinRoom = CreateMonsterRoom("Monster Goblin", unitFactory.CreateGoblinEnemy());
            var monsterOrkRoom = CreateMonsterRoom("Monster Ork", unitFactory.CreateOrkEnemy());
            var emptyRoom = CreateEmptyRoom("Empty");
            var lootRoom = CreateLootRoom("Loot1", new Gold());
            var lootArmourRoom = CreateLootRoom("Armour", new Armour(30, 30, "Great Armour"));
            var lootStoneRoom = CreateLootRoom("Loot2", new Grindstone("Stone"));
            var finalRoom = CreateLootRoom("Final", new Gold());

            enter.TrySetDirection(Direction.Right, lootArmourRoom);
            enter.TrySetDirection(Direction.Left, monsterGoblinRoom);

            lootArmourRoom.TrySetDirection(Direction.Forward, monsterOrkRoom);
            monsterGoblinRoom.TrySetDirection(Direction.Forward, lootStoneRoom);

            monsterOrkRoom.TrySetDirection(Direction.Forward, finalRoom);
            lootStoneRoom.TrySetDirection(Direction.Forward, finalRoom);

            //monsterGoblinRoom.TrySetDirection(Direction.Forward, finalRoom);
            //monsterOrkRoom.TrySetDirection(Direction.Forward, finalRoom);

            return enter;
        }
    }
}
