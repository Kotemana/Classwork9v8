using ConsoleApp8;
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

