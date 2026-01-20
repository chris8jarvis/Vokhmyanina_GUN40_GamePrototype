using GamePrototype.Dungeon;
using GamePrototype.Items.EconomicItems;
using GamePrototype.Items.EquipItems;
using GamePrototype.Units;
using GamePrototype.Utils;

namespace GamePrototype.Units.Factories
{
    public abstract class UnitFactory
    {
        public abstract Unit CreatePlayer(string name);

        public abstract Unit CreateGoblinEnemy();
        public abstract Unit CreateOrkEnemy();
    }
}
