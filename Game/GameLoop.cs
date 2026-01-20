using GamePrototype.Combat;
using GamePrototype.Dungeon;
using GamePrototype.Items.EconomicItems;
using GamePrototype.Items.EquipItems;
using GamePrototype.Units;
using GamePrototype.Units.Factories;
using GamePrototype.Utils;

namespace GamePrototype.Game
{
    public sealed class GameLoop
    {
        private Unit _player;
        private DungeonRoom _dungeon;
        private readonly CombatManager _combatManager = new CombatManager();
        
        public void StartGame() 
        {
            Initialize();
            Console.WriteLine("Entering the dungeon");
            StartGameLoop();
        }

        #region Game Loop

        private void Initialize()
        {
            Console.WriteLine("Welcome, player!");
            bool isHardLevel = false;
            while (true)
            {
                Console.WriteLine("Write 'e' - for easy level, 'h' - for hard level. [e/h]: ");
                string userChoice = Console.ReadLine();
                if (userChoice == "e")
                {
                    isHardLevel = false;
                    break;
                }
                else if (userChoice == "h")
                {
                    isHardLevel = true;
                    break;
                }
                else
                {
                    Console.WriteLine("Wrong direction");
                }
            }

            UnitFactory unitFactory;
            DungeonBuilder dungeonBuilder;
            if (isHardLevel)
            {
                unitFactory = new HardUnitFactory();
                dungeonBuilder = new HardLevelDungeonBuilder();
            }
            else
            {
                unitFactory = new EasyUnitFactory();
                dungeonBuilder = new EasyLevelDungeonBuilder();
            }

            _dungeon = dungeonBuilder.BuildDungeon(unitFactory);

            Console.WriteLine("Enter your name");
            _player = unitFactory.CreatePlayer(Console.ReadLine());
            Console.WriteLine($"Hello {_player.Name}");
        }

        private void StartGameLoop()
        {
            var currentRoom = _dungeon;

            while (currentRoom.IsFinal == false)
            { 
                StartRoomEncounter(currentRoom, out var success);
                if (!success) 
                {
                    Console.WriteLine("Game over!");
                    return;
                }
                while (true)
                {
                    DisplayActionChoice();
                    string playerCommand = Console.ReadLine();
                    if (playerCommand == "1")
                    {
                        _player.InventoryUsage();
                    }
                    else if (playerCommand == "2")
                    {
                        currentRoom = HandleMoveCommand(currentRoom);
                        break;
                    } 
                    else
                    {
                        Console.WriteLine("Wrong command");
                    }
                    
                }
            }
            Console.WriteLine($"Congratulations, {_player.Name}");
            Console.WriteLine("Result: ");
            Console.WriteLine(_player.ToString());
        }

        private void StartRoomEncounter(DungeonRoom currentRoom, out bool success)
        {
            success = true;
            if (currentRoom.Loot != null) 
            {
                _player.AddItemToInventory(currentRoom.Loot);
            }
            if (currentRoom.Enemy != null) 
            {
                if (_combatManager.StartCombat(_player, currentRoom.Enemy) == _player)
                {
                    _player.HandleCombatComplete();
                    LootEnemy(currentRoom.Enemy);
                }
                else 
                {
                    success = false;
                }
            }

            void LootEnemy(Unit enemy)
            {
                _player.AddItemsFromUnitToInventory(enemy);
            }
        }

        private void DisplayRouteOptions(DungeonRoom currentRoom)
        {
            Console.WriteLine("Where to go?");
            foreach (var room in currentRoom.Rooms)
            {
                Console.Write($"{room.Key}: {(int) room.Key}\t");
            }
        }

        private void DisplayActionChoice()
        {
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1 - inventory check, 2 - move");
        }
        private DungeonRoom HandleMoveCommand(DungeonRoom currentRoom)
        {
            while (true)
            {
                DisplayRouteOptions(currentRoom);
                if (Enum.TryParse<Direction>(Console.ReadLine(), out var direction) &&
                    Enum.IsDefined(typeof(Direction), direction) &&
                    currentRoom.Rooms.TryGetValue(direction, out var nextRoom))
                {
                    return nextRoom;
                }
                else
                {
                    Console.WriteLine("Wrong direction!");
                }
            }
        }
        
        

        #endregion
    }
}
