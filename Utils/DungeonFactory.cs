
using GamePrototype.Dungeon;
using GamePrototype.Items.EconomicItems;
using GamePrototype.Units;

namespace GamePrototype.Utils
{
    public abstract class DungeonFactory
    {
        public abstract DungeonRoom BuildDungeon();

        protected virtual DungeonRoom CreateEmptyRoom(string name)
        {
            return new DungeonRoom(name);
        }
        protected virtual DungeonRoom CreateMonsterRoom(string name, Unit enemy)
        {
            return new DungeonRoom(name, enemy);
        }
        protected virtual DungeonRoom CreateLootRoom(string name, Item item)
        {
            return new DungeonRoom(name, item);
        }
    }
}
