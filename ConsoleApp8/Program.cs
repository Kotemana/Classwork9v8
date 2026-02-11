using ConsoleApp8;

Console.WriteLine("=== Battle Simulation System ===\n");

var battle = new Battle();

battle.LoadNamesFromFile("names.txt");

Console.Write("Enter number of squads per faction: ");
int squadCount = int.Parse(Console.ReadLine() ?? "2");

Console.Write("Enter number of fighters per squad: ");
int fightersPerSquad = int.Parse(Console.ReadLine() ?? "10");

battle.InitializeFactions(squadCount, fightersPerSquad);

battle.ShowFactionsReport();

while (true)
{
    Console.WriteLine("\n=== BATTLE MENU ===");
    Console.WriteLine("1. PvP Battle (1 vs 1)");
    Console.WriteLine("2. Squad vs Squad");
    Console.WriteLine("3. Faction War (Coming soon)");
    Console.WriteLine("0. Exit");
    Console.Write("\nSelect option: ");

    string? input = Console.ReadLine();
    
    switch (input)
    {
        case "1":
            battle.PvPBattle();
            break;
        case "2":
            battle.SquadVsSquad();
            break;
        case "3":
            Console.WriteLine("\nFaction War feature is coming in next stages!\n");
            break;
        case "0":
            Console.WriteLine("\nExiting...");
            return;
        default:
            Console.WriteLine("\nInvalid option. Please try again.\n");
            break;
    }
}

