using System;
using System.Collections.Generic;
using Elemental_Beasts;

public partial class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. New Game (progress through all levels)");
            Console.WriteLine("2. Select Level");
            Console.WriteLine("3. Exit");
            Console.Write("Enter 1, 2, or 3: ");
            string Gamechoice = Console.ReadLine();
            Console.WriteLine();

            if (Gamechoice == "1")
            {
                for (int i = 1; i <= 3; i++)
                {
                    (Level level, int startingPotions, string levelPotionHint) = BuildLevel(i);
                    Player player = new Player("Hero", 120, 20, 5, startingPotions);
                    GameManager gameManager = new GameManager(player.name, level.Level_Number, level.Monsters[0], true);
                    gameManager.RunLevel(player, level, i);
                }

                Console.WriteLine("New Game run complete. Returning to main menu...\n");
                continue;
            }

            if (Gamechoice == "2")
            {
                string LevelChoice;
                do
                {
                    Console.WriteLine("--- Level Selection ---");
                    Console.WriteLine("1. The Forest");
                    Console.WriteLine("2. A Dance at Sea");
                    Console.WriteLine("3. The Volcano");
                    Console.Write("Enter 1, 2, or 3: ");
                    LevelChoice = Console.ReadLine();

                    if (LevelChoice != "1" && LevelChoice != "2" && LevelChoice != "3")
                    {
                        Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.\n");
                    }
                } while (LevelChoice != "1" && LevelChoice != "2" && LevelChoice != "3");

                int levelNumber = int.Parse(LevelChoice);
                (Level level, int startingPotions, string levelDescription) = BuildLevel(levelNumber);
                Player player = new Player("Hero", 120, 20, 5, startingPotions);
                GameManager gameManager = new GameManager(player.name, level.Level_Number, level.Monsters[0], true);
                gameManager.RunLevel(player, level, levelNumber);

                Console.WriteLine("Level complete. Returning to main menu...\n");
                continue;
            }

            if (Gamechoice == "3")
            {
                Console.WriteLine("Exiting the game...");
                Console.WriteLine("Thank You for Playing! Goodbye!");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                break;
            }

            Console.WriteLine("Invalid option. Please enter 1, 2, or 3.\n");
        }
    }


    // monster data
    private static (Level level, int startingPotions, string levelPotionHint) BuildLevel(int levelNumber)
    {
        if (levelNumber == 3)
        {
            Monster[] level3Monsters = new Monster[]
            {
                new Monster(150, 15, 8, "Fire Goblin", "A blazing goblin."),
                new Monster(150, 15, 8, "Fire Goblin", "Another fiery goblin."),
                new Boss(200, 25, 10, "Fire Lord", "A blazing ball of flame emerges from the volcano.")
            };

            Level level3 = new Level("Volcano Summit", 3, "The final fiery stage.", level3Monsters);
            return (level3, 3 + level3.Level_Number, "The Fire Lord awaits! Choose wisely.");
        }

        if (levelNumber == 2)
        {
            Monster[] level2Monsters = new Monster[]
            {
                new Monster(120, 10, 8, "Ocean Guardian", "A towering guardian of the sea."),
                new Monster(120, 10, 8, "Ocean Guardian", "A second guardian rises from the waves.")
            };

            Level level2 = new Level("Dance at Sea", 2, "A stormy ocean stage with fierce guardians.", level2Monsters);
            return (level2, 3 + level2.Level_Number, "A tide of Ocean Guardians appears! Choose your potion carefully.");
        }

        Monster[] level1Monsters = new Monster[]
        {
            new Monster(80, 5, 5, "Earth Fairies", "A creature emerges from the woods.")
        };
        Level level1 = new Level("Forest Entrance", 1, "A mysterious woods filled with low-level beasts.", level1Monsters);
        return (level1, 3 + level1.Level_Number, "An Earth Fairy appears! Choose your potion carefully.");
    }
}
