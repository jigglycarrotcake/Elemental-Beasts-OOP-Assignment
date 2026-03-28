using System;
using System.Collections.Generic;
using Elemental_Beasts;

class Program
{
    static void Main(string[] args)
    {
        (Level level, int startingPotions, string levelPotionHint) = BuildLevel(2);

        // Player (healing potions scale with level config)
        Player player = new Player("Hero", 120, 20, 5, startingPotions);

        Console.WriteLine();
        Console.WriteLine($"Welcome to {level.Level_Name}!");
        Console.WriteLine(level.Level_Description);
        Console.WriteLine(levelPotionHint);
        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        Console.WriteLine();

        Console.WriteLine("-- Player Stat Overview --");
        player.DisplayStats();
        Console.WriteLine();

        Console.WriteLine("Choose your starting potion before entering the level:");
        Console.WriteLine("1. Nature's Gift");
        Console.WriteLine("2. Fire Immunity");
        Console.WriteLine("3. Ocean's Breath");
        Console.Write("Enter 1, 2, or 3: ");
        string potionChoice = Console.ReadLine();
        Console.WriteLine();

        switch (potionChoice)
        {
            case "1":
                player.ActivateNatureBuff();
                break;
            case "2":
                player.ActivateFireImmune();
                break;
            case "3":
                player.ActivateWaterBend();
                break;
            default:
                Console.WriteLine("Invalid choice. No potion selected.");
                break;
        }

        Console.WriteLine();
        Console.WriteLine("Potion choice complete. The battle begins!");
        player.DisplayStats();
        Console.WriteLine();

        GameManager gameManager = new GameManager(player.name, level.Level_Number, level.Monsters[0], true);

        for (int i = 0; i < level.Monsters.Length; i++)
        {
            Monster currentMonster = level.Monsters[i];
            Console.WriteLine($"\n--- Encounter {i + 1}: {currentMonster.monsterType} appears! ---");
            // 1 monster at a time.

            while (currentMonster.IsAlive() && player.IsAlive())
            {
                Console.WriteLine("\n================= New Turn =================");
                player.ChooseAction();
                string userInput = Console.ReadLine();
                Console.WriteLine();

                int attackBonus = player.GetAttackBonus(currentMonster);
                int totalDamage = player.attackPower + attackBonus;

                switch (userInput)
                {
                    case "1":
                        if (attackBonus > 0)
                        {
                            Console.WriteLine($"{player.name}'s potion is effective against {currentMonster.monsterType}! +{attackBonus} bonus damage.");
                        }
                        else if (!player.PotionChoice.Equals("None", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"{player.name}'s selected potion has no effect on {currentMonster.monsterType}.");
                        }
                        currentMonster.TakeDamage(totalDamage);
                        break;
                    case "2":
                        player.Heal();
                        break;
                    case "3":
                        player.Defend();
                        break;
                    default:
                        Console.WriteLine("Invalid input, please choose 1, 2, or 3.");
                        continue;
                }

                if (!currentMonster.IsAlive())
                {
                    Console.WriteLine($"\n{currentMonster.monsterType} has been defeated!\n");
                    break;
                }

                int monsterDamage = currentMonster.attackPower;
                int defenseBonus = player.GetDefenseBonus(currentMonster);
                if (defenseBonus > 0)
                {
                    Console.WriteLine($"{player.name}'s potion resists this enemy! -{defenseBonus} damage this turn.");
                    monsterDamage -= defenseBonus;
                }
                if (monsterDamage < 0) monsterDamage = 0;

                Console.WriteLine($"{currentMonster.monsterType} attacks {player.name} for {monsterDamage} damage.");
                player.TakeDamage(monsterDamage);

                if (player.IsAlive())
                {
                    Console.WriteLine("\n=== Level Complete! ===\r\n");
                    Console.WriteLine("====================\r\n");
                    Console.WriteLine("VICTORY! YOU HAVE DEFEATED ALL THE MONSTERS");
                }

                if (!player.IsAlive())
                {
                    Console.WriteLine("\n================================\r\n");
                    Console.WriteLine($"{player.name} has been defeated! GAME OVER.");
                    Console.WriteLine("GAME OVER");
                    Console.WriteLine("================================\r\n");
                    return; // Exit the game
                }
            }
        }

        Console.WriteLine("\n===== Level Complete =====");
    }

    static (Level level, int startingPotions, string levelPotionHint) BuildLevel(int levelNumber)
    {
        if (levelNumber == 2)
        {
            Monster[] level2Monsters = new Monster[]
            {
                new Monster(120, 15, 8, "Ocean Guardian", "A towering guardian of the sea."),
                new Monster(120, 15, 8, "Ocean Guardian", "A second guardian rises from the waves.")
            };

            Level level2 = new Level("Dance at Sea", 2, "A stormy ocean stage with fierce guardians.", level2Monsters);
            return (level2, 3 + level2.Level_Number, "A tide of Ocean Guardians appears! Choose your potion carefully.");
        }

        Monster[] level1Monsters = new Monster[]
        {
            new Monster(80, 15, 5, "Earth Fairies", "A creature emerges from the woods.")
        };
        Level level1 = new Level("Forest Entrance", 1, "A mysterious woods filled with low-level beasts.", level1Monsters);
        return (level1, 3 + level1.Level_Number, "An Earth Fairy appears! Choose your potion carefully.");
    }
}

