using GamePrototype.Dungeon;
using GamePrototype.Items.EconomicItems;

namespace GamePrototype.Utils
{
    //public static class DungeonBuilder
    public class EasyLevelDungeon : DungeonBuilder //заменить на builder. не использовать слово factory у dungeon
    {
        //public static DungeonRoom BuildDungeon()
        public override DungeonRoom BuildDungeon()
        {
            var enter = CreateEmptyRoom("Enter");
            var monsterRoom = CreateMonsterRoom("Monster", UnitFactoryDemo.CreateGoblinEnemy());
            var emptyRoom = CreateEmptyRoom("Empty");
            var lootRoom = CreateLootRoom("Loot1", new Gold());
            var lootStoneRoom = CreateLootRoom("Loot2", new Grindstone("Stone"));
            var finalRoom = CreateLootRoom("Final", new Grindstone("Stone1"));

            enter.TrySetDirection(Direction.Right, monsterRoom);
            enter.TrySetDirection(Direction.Left, emptyRoom);

            monsterRoom.TrySetDirection(Direction.Forward, lootRoom);
            monsterRoom.TrySetDirection(Direction.Left, emptyRoom);

            emptyRoom.TrySetDirection(Direction.Forward, lootStoneRoom);

            lootRoom.TrySetDirection(Direction.Forward, finalRoom);
            lootStoneRoom.TrySetDirection(Direction.Forward, finalRoom);

            return enter;
        }
    }
}
