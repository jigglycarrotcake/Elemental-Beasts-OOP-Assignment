using System;
namespace Elemental_Beasts;

public class Entity
{
    private string Name;
    private int MaxHealth; //maxhealth = 120, DEFAULT AD = 20
    private int CurrentHealth; // tracks current health
    private int AttackPower; 
    private int DefensePower; //default = 5, can increase with potions
    protected bool isDefending;

    public string name
    {
        get { return Name; }
        set { Name = value; }
    }

    public int maxHealth
    {
        get { return MaxHealth; }
        set { MaxHealth = value; }
    }

    public int currentHealth
    {
        get { return CurrentHealth; }
        set { CurrentHealth = value; }
    }

    public int attackPower
    {
        get { return AttackPower; }
        set { AttackPower = value; }
    }

    public int defensePower
    {
        get { return DefensePower; }
        set { DefensePower = value; }
    }

    public object monsterType { get; internal set; }

    protected Entity(string givenName, int givenMaxHealth, int givenAttackPower, int givenDefensePower)
    {
        Name = givenName;
        MaxHealth = givenMaxHealth;
        CurrentHealth = givenMaxHealth;
        AttackPower = givenAttackPower;
        DefensePower = givenDefensePower;
        isDefending = false;
    }

    public virtual void TakeDamage(int damage)
    {
        int totalDamage = damage;

        if (isDefending)
        {
            totalDamage = damage - DefensePower;
            Console.WriteLine($"{Name} defended the attack!");
            isDefending = false;
        }

        CurrentHealth -= totalDamage;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
        Console.WriteLine($"{Name} took {totalDamage} damage. Health: {CurrentHealth}/{MaxHealth}");
    }

    public virtual void AttackEntity(Entity target)
    {
        if (IsAlive())
        {   
            Console.WriteLine();
            Console.WriteLine($"{Name} attacks {target.name} for {AttackPower} damage!");
            target.TakeDamage(AttackPower);
            Console.WriteLine("___________________________________________________________________");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine($"{Name} is already defeated.");
        }
    }

    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }

    public void Defend()
    {
        isDefending = true;
        Console.WriteLine($"{Name} is defending and will take reduced damage on the next attack!");
    }

    public void ResetDefense()
    {
        isDefending = false;
        Console.WriteLine($"{Name} is no longer defending.");
    }
    
    public virtual void DisplayStats()
    {
        // Basically like an indicator above the enitities to show their details like HP, skills & stuff (e.g. Hilichurls In Genshin)
        Console.WriteLine("\n---- Stats ----");
        Console.WriteLine("Name: " + name);
        Console.WriteLine("HP: " + currentHealth + "/" + maxHealth);
        Console.WriteLine("ATK: " + attackPower);
        Console.WriteLine("DEF: " + defensePower);
        Console.WriteLine("------------------");
    }
}


public class Monster : Entity
{
    private string MonsterType;
    private string Description;

    public string monsterType
    {
        get { return MonsterType; }
        set { MonsterType = value; }
    }

    public string description
    {
        get { return Description; }
        set { Description = value; }
    }

    public Monster(int givenMaxHealth, int givenAttackPower, int givenDefensePower, string givenMonsterType, string givenDescription)
        : base("Monster", givenMaxHealth, givenAttackPower, givenDefensePower)
    {
        MonsterType = givenMonsterType;
        Description = givenDescription;
    }

    public override void AttackEntity(Entity target)
    {
        if (IsAlive())
        {
            Console.WriteLine($"{MonsterType} attacks {target.name} for {attackPower} damage!");
            target.TakeDamage(attackPower);
            Console.WriteLine("___________________________________________________________________");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine($"{MonsterType} has been defeated.");
            Console.WriteLine("___________________________________________________________________");
            Console.WriteLine();
        }
    }

    public void RandomAction(Player target) //remove sebab tak guna pun
    {
        if (IsAlive())
        {
            Random random = new Random();
            int action = random.Next(0, 11);

            if (action <= 8)
            {
                Console.WriteLine($"{MonsterType} prepares to attack!");
                AttackEntity(target);
                Console.WriteLine($"Monsters Health: {currentHealth}/{maxHealth}");
                Console.WriteLine("___________________________________________________________________");
                Console.WriteLine();
                // BLEGHH
            }
            else
            {   
                Console.WriteLine($"{MonsterType} is defending!");
                Console.WriteLine($"Monsters Health: {currentHealth}/{maxHealth}");
                Console.WriteLine("___________________________________________________________________");
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        int totalDamage = damage;

        if (isDefending)
            {
                totalDamage = damage - defensePower;
                Console.WriteLine($"{MonsterType} defended the attack!");
                isDefending = false;
                Console.WriteLine("___________________________________________________________________");
                Console.WriteLine();
            }
        else
            {
            currentHealth -= totalDamage;
            }
                if (currentHealth < 0)
                {
                    currentHealth = 0; // makes health 0 doesn't go negative
                }
                Console.WriteLine($"{MonsterType} took {totalDamage} damage. Health: {currentHealth}/{maxHealth}");
                Console.WriteLine("___________________________________________________________________");
                Console.WriteLine();
    }

    public override void DisplayStats()
    {
        Console.WriteLine("\n---- Monster Stats ----");
        Console.WriteLine("Type: " + monsterType);
        Console.WriteLine("Description: " + description);
        Console.WriteLine("HP: " + currentHealth + "/" + maxHealth);
        Console.WriteLine("ATK: " + attackPower);
        Console.WriteLine("DEF: " + defensePower);
        Console.WriteLine("------------------");
    }
}


public class Boss : Monster
{
    public Boss(int givenMaxHealth, int givenAttackPower, int givenDefensePower,
                string givenMonsterType, string givenDescription)
        : base(givenMaxHealth, givenAttackPower, givenDefensePower, givenMonsterType, givenDescription)
    {
    }

    public void setDescription(string newDescription)
    {
        description = newDescription;
    }
}


public class Player : Entity
{
    private int PotionHeals;
    private bool FireImmune;
    private bool WaterBend;
    private bool NatureBuff;
    private string SelectedPotion;
    private int attackBonus;

    public int Healing_Potions
    {
        get { return PotionHeals; }
        set { PotionHeals = value; }
    }

    public bool Fire_Immunity
    {
        get { return FireImmune; }
        set { FireImmune = value; }
    }

    public bool Water_Power
    {
        get { return WaterBend; }
        set { WaterBend = value; }
    }

    public bool Buffer
    {
        get { return NatureBuff; }
        set { NatureBuff = value; }
    }

    public string PotionChoice
    {
        get { return SelectedPotion; }
    }

    public int AttackBonus
    {
        get { return attackBonus; }
        set { attackBonus = value; }
    }

    public Player(string givenName, int givenMaxHealth, int givenAttackPower, int givenDefensePower, int initialHealingPotions = 3)
        : base(givenName, givenMaxHealth, givenAttackPower, givenDefensePower)
    {
        PotionHeals = initialHealingPotions;
        FireImmune = false; // By default
        WaterBend = false;  // By default
        NatureBuff = false; // By default
        SelectedPotion = "None";
    }

    public void AddHealingPotions(int amount)
    {
        PotionHeals += amount;
        Console.WriteLine($"{name} received {amount} extra healing potions. Total: {PotionHeals}.");
        Console.WriteLine("___________________________________________________________________");
        Console.WriteLine();

    }

    public void ChooseAction()
    {
        Console.WriteLine("Player's turn:");
        Console.WriteLine("--------------------------------");
        Console.WriteLine("|    1. Attack                  |");
        Console.WriteLine("|    2. Heal                    |");
        Console.WriteLine("|    3. Defend                  |");
        Console.WriteLine("--------------------------------\n");
    }

    public void Heal()
    {
        if (Healing_Potions > 0)
        {
            if (currentHealth >= maxHealth)
            {
                Console.WriteLine("Health is already full!");
                Console.WriteLine("___________________________________________________________________");
                Console.WriteLine();
            }
            else
            {
                int healedAmount = 50;
                int newHealth = currentHealth + healedAmount;
                if (newHealth > maxHealth)
                {
                    newHealth = maxHealth;
                }

                currentHealth = newHealth;
                PotionHeals--;
                Console.WriteLine($"{name} used a healing potion! Health: {currentHealth}/{maxHealth}");
                Console.WriteLine("___________________________________________________________________");
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("No healing potions left!");
            Console.WriteLine("___________________________________________________________________");
            Console.WriteLine();
        }
    }

    public override void AttackEntity(Entity target)
    {
        if (IsAlive())
        {
            Console.WriteLine($"{name} attacks {target.monsterType} for {attackPower} damage!");
            target.TakeDamage(attackPower);
            Console.WriteLine($"Hero's Health: {currentHealth}/{maxHealth}");
            Console.WriteLine("___________________________________________________________________");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine($"{name} has been defeated.");
            Console.WriteLine($"Hero's Health: {currentHealth}/{maxHealth}");
            Console.WriteLine("___________________________________________________________________");
            Console.WriteLine();
        }
    }

    public override void TakeDamage(int damage)
    {
        int totalDamage = damage;

        if (isDefending)
        {
            totalDamage = damage - defensePower;
            Console.WriteLine($"{monsterType} defended the attack!");
            isDefending = false;
            Console.WriteLine("___________________________________________________________________");
            Console.WriteLine();
        }

        if (totalDamage < 0)
        {
            totalDamage = 0;
        }

        currentHealth -= totalDamage;

        if (currentHealth < 0)
        {
            currentHealth = 0; // makes health 0 doesn't go negative
        }

        Console.WriteLine($"{monsterType} took {totalDamage} damage. Health: {currentHealth}/{maxHealth}");
        Console.WriteLine("___________________________________________________________________");
        Console.WriteLine();
    }

    public void ActivateFireImmune()
    {
        Fire_Immunity = true;
        SelectedPotion = "Fire Immunity";
        Console.WriteLine($"{name} selected Fire Immunity. This works only against Fire Goblins and Bosses.");
    }

    public void ActivateWaterBend()
    {
        Water_Power = true;
        SelectedPotion = "Water Potion";
        Console.WriteLine($"{name} selected Water Potion. This works only against Ocean Guardians.");
    }

    public void ActivateNatureBuff()
    {
        NatureBuff = true;
        SelectedPotion = "Nature's Gift";
        Console.WriteLine($"{name} selected Nature's Gift. This works only against Earth Fairies.");
    }

    public bool IsPotionEffectiveAgainst(Monster target) //makes sure the potion works for their designated monster
    {
        if (Buffer && target.monsterType.Equals("Earth Fairies", StringComparison.OrdinalIgnoreCase))
            return true;
        if (Water_Power && (target.monsterType.Equals("Ocean Guardian", StringComparison.OrdinalIgnoreCase) || target.monsterType.Equals("Ocean Guardians", StringComparison.OrdinalIgnoreCase)))
            return true;
        if (Fire_Immunity && (target.monsterType.Equals("Fire Goblin", StringComparison.OrdinalIgnoreCase) || target is Boss))
            return true;
        return false;
    }

    public int GetAttackBonus(Monster target) //attackbonus for designated monster
    {
        if (!IsPotionEffectiveAgainst(target))
            return 0;
            // to ensure potions work against corresponding mosnters only
        if (Buffer && target.monsterType.Equals("Earth Fairies", StringComparison.OrdinalIgnoreCase))
            return 10;
        if (Water_Power && (target.monsterType.Equals("Ocean Guardian", StringComparison.OrdinalIgnoreCase) || target.monsterType.Equals("Ocean Guardians", StringComparison.OrdinalIgnoreCase)))
            return 15;
        if (Fire_Immunity && (target.monsterType.Equals("Fire Goblin", StringComparison.OrdinalIgnoreCase) || target is Boss))
            return 30;

        return 0;
    }

    public int GetDefenseBonus(Monster target) //defense bonus for designated monster
    {
        if (!IsPotionEffectiveAgainst(target))
            return 0;

        if (Buffer && target.monsterType.Equals("Earth Fairies", StringComparison.OrdinalIgnoreCase))
            return 10;
        if (Water_Power && (target.monsterType.Equals("Ocean Guardian", StringComparison.OrdinalIgnoreCase) || target.monsterType.Equals("Ocean Guardians", StringComparison.OrdinalIgnoreCase)))
            return 15;
        if (Fire_Immunity && (target.monsterType.Equals("Fire Goblin", StringComparison.OrdinalIgnoreCase) || target is Boss))
            return 20;

        return 0;
    }

    public void SelectPotion(Player player){
        string potionChoice;

        do
        {
            Console.WriteLine("Choose your starting potion before entering the level:");
            Console.WriteLine("1. Nature's Gift");
            Console.WriteLine("2. Fire Immunity");
            Console.WriteLine("3. Ocean's Breath");
            Console.Write("Enter 1, 2, or 3: ");
            potionChoice = Console.ReadLine();
            Console.WriteLine("___________________________________________________________________");
            Console.WriteLine();

            switch (potionChoice)
            {
                case "1":
                    player.ActivateNatureBuff(); //activates buff for earth fairies
                    break;
                case "2":
                    player.ActivateFireImmune(); // activates buff for fire lord and goblin
                    break;
                case "3":
                    player.ActivateWaterBend(); // activates buff for sea guardian
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.\n");
                    continue;
            }

            break;
        } while (true);

        Console.WriteLine();
        Console.WriteLine("Potion choice complete. The battle begins!");
        Console.WriteLine("___________________________________________________________________");
        Console.WriteLine();
    }

    public override void DisplayStats()
    {
        Console.WriteLine("\n---- Player Stats ----");
        Console.WriteLine("Name: " + name);
        Console.WriteLine("HP: " + currentHealth + "/" + maxHealth);
        Console.WriteLine("ATK: " + attackPower);
        Console.WriteLine("DEF: " + defensePower);
        Console.WriteLine("------------------");
    }

}


public class Level //for level info to build and load
{
    private string LevelName;
    private int LevelNumber;
    private string Description;
    private Monster[] MonstersInLevel;
   
    public string Level_Name
    {
        get { return LevelName; }
        set { LevelName = value; }
    }

    public int Level_Number
    {
        get { return LevelNumber; }
        set { LevelNumber = value; }
    }

    public string Level_Description
    {
        get { return Description; }
        set { Description = value; }
    }

    public Monster[] Monsters
    {
        get { return MonstersInLevel; }
        set { MonstersInLevel = value; }
    }

    public Level(string givenLevelName, int givenLevelNumber, string givenDescription, Monster[] givenMonstersInLevel)
    {
        LevelName = givenLevelName;
        LevelNumber = givenLevelNumber;
        Description = givenDescription;
        MonstersInLevel = givenMonstersInLevel;
    }
}

   public class GameManager
    {
        private string Playername;
        private int LevelNumber;
        private Monster currentMonster;
        public bool isRunning; //so that program can access

        public GameManager(string givenPlayername, int givenLevel, Monster currentMonster, bool isRunning)
        {
            this.Playername = givenPlayername;
            this.LevelNumber = givenLevel;
            this.currentMonster = currentMonster;
            this.isRunning = isRunning;
        }

        public string Player
    {
        get { return Playername; }
        set { Playername = value; }
    }

        public int Level
        {
            get { return LevelNumber; }
            set { LevelNumber = value; }


        }

        public Monster CurrentMonster
        {
            get { return currentMonster; }
            set { currentMonster = value; }
        }

        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
        }

    public void RunLevel(Player player,Level Level, int LevelNumber){ //to load levels in level selection and progression
            Console.WriteLine();
            Console.WriteLine($"Welcome to {Level.Level_Name}!");
            Console.WriteLine(Level.Level_Description);
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine();

            player.SelectPotion(player); // Once per level
            player.DisplayStats();
            currentMonster.DisplayStats();
            Console.WriteLine();

            Random rng = new Random();

            for (int i = 0; i < Level.Monsters.Length; i++)
                {
                    Monster currentMonster = Level.Monsters[i];

                    Console.WriteLine($"\n--- Encounter {i + 1}: {currentMonster.monsterType} appears! ---");
                    // 1 monster at a time.

                    while (currentMonster.IsAlive() && player.IsAlive())
                    {
                        Console.WriteLine("\n================= New Turn =================");

                        string userInput;
                        while (true)
                        {
                            player.ChooseAction();
                            userInput = Console.ReadLine();
                            Console.WriteLine();

                            if (userInput == "1" || userInput == "2" || userInput == "3")
                            {
                                break;
                            }

                            Console.WriteLine("Invalid input, please choose 1, 2, or 3.\n");
                        }

                        int attackBonus = player.GetAttackBonus(currentMonster);
                        int totalDamage = player.attackPower + attackBonus;

                        switch (userInput)
                        {
                            case "1":
                                if (attackBonus > 0)
                                {
                                    Console.WriteLine($"{player.name}'s potion is effective against {currentMonster.monsterType}! +{attackBonus} bonus damage.");
                                    currentMonster.TakeDamage(attackBonus); // to apply the bonus damage after the attack
                                }
                                else if (!player.PotionChoice.Equals("None", StringComparison.OrdinalIgnoreCase))
                                {
                                    Console.WriteLine($"{player.name}'s selected potion has no effect on {currentMonster.monsterType}.");
                                }
                                player.AttackEntity(currentMonster);

                                Console.WriteLine();
                                break;
                            case "2":
                                player.Heal();
                                Console.WriteLine();
                                break;
                            case "3":
                                player.Defend();
                                Console.WriteLine();
                                break;
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

                        bool monsterAttacks = rng.Next(0, 11) <= 8;
                        if (monsterAttacks)
                        {
                            Console.WriteLine($"{currentMonster.monsterType} prepares to attack!");
                            player.TakeDamage(monsterDamage);
                            Console.WriteLine($"Monsters Health: {currentMonster.currentHealth}/{currentMonster.maxHealth}");
                            Console.WriteLine("___________________________________________________________________");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine($"{currentMonster.monsterType} is defending!");
                            currentMonster.Defend();
                            Console.WriteLine($"Monsters Health: {currentMonster.currentHealth}/{currentMonster.maxHealth}");
                            Console.WriteLine("___________________________________________________________________");
                            Console.WriteLine();
                        }

                        if (!player.IsAlive())
                        {
                            Console.WriteLine("\n================================\r\n");
                            Console.WriteLine($"{player.name} has been defeated! GAME OVER.");
                            Console.WriteLine("GAME OVER");
                            Console.WriteLine("================================\r\n");
                            return;
                        }
                    }
                }

                    Console.WriteLine("\n=== Level Complete! ===\r\n");
                    Console.WriteLine("====================\r\n");
                    Console.WriteLine("VICTORY! YOU HAVE DEFEATED ALL THE MONSTERS");
    }
}

