namespace ConsoleApp8;

public class Battle
{
    private static readonly Random _random = new();
    private List<string> _names = new();
    private List<Faction> _factions = new();

    public List<string> LoadNamesFromFile(string filePath)
    {
        try
        {
            _names = File.ReadAllLines(filePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
            Console.WriteLine($"Loaded {_names.Count} names from file.");
            return _names;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading names from file: {ex.Message}");
            return new List<string>();
        }
    }

    public void InitializeFactions(int squadCount, int fightersPerSquad)
    {
        if (_names.Count == 0)
        {
            Console.WriteLine("No names loaded. Cannot initialize factions.");
            return;
        }

        _factions.Clear();

        string[] factionNames = { "Orcs", "Elves" };

        foreach (var factionName in factionNames)
        {
            var faction = new Faction(factionName);

            for (int squadIndex = 0; squadIndex < squadCount; squadIndex++)
            {
                var squad = new Squad($"{factionName} Squad {squadIndex + 1}");

                for (int fighterIndex = 0; fighterIndex < fightersPerSquad; fighterIndex++)
                {
                    string randomName = _names[_random.Next(_names.Count)];
                    var fighter = new Fighter(randomName);
                    squad.AddFighter(fighter);
                }

                faction.AddSquad(squad);
            }

            _factions.Add(faction);
        }

        Console.WriteLine($"\nFactions initialized: {_factions.Count} factions created.");
        Console.WriteLine($"Each faction has {squadCount} squads with {fightersPerSquad} fighters each.\n");
    }

    public void ShowFactionsReport()
    {
        if (_factions.Count == 0)
        {
            Console.WriteLine("No factions to report.");
            return;
        }

        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("FACTIONS REPORT");
        Console.WriteLine(new string('=', 60));

        foreach (var faction in _factions)
        {
            Console.WriteLine(faction.GetFullReport());
        }

        Console.WriteLine(new string('=', 60));
    }

    public List<Faction> GetFactions()
    {
        return _factions;
    }

    public void PvPBattle()
    {
        if (_factions.Count < 2)
        {
            Console.WriteLine("Need at least 2 factions for PvP battle.");
            return;
        }

        var faction1 = _factions[0];
        var faction2 = _factions[1];

        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("PVP BATTLE (1 vs 1)");
        Console.WriteLine(new string('=', 60) + "\n");

        var squad1 = faction1.GetRandomSquad();
        var squad2 = faction2.GetRandomSquad();

        if (squad1 == null || squad2 == null)
        {
            Console.WriteLine("Cannot select squads for battle.");
            return;
        }

        var fighter1 = squad1.GetRandomAliveFighter();
        var fighter2 = squad2.GetRandomAliveFighter();

        if (fighter1 == null || fighter2 == null)
        {
            Console.WriteLine("Cannot select fighters for battle.");
            return;
        }

        Console.WriteLine($"Fighter 1: {fighter1.Name} from {faction1.Name} - {squad1.Name}");
        Console.WriteLine($"  Stats: {fighter1.GetInfo()}");
        Console.WriteLine($"\nFighter 2: {fighter2.Name} from {faction2.Name} - {squad2.Name}");
        Console.WriteLine($"  Stats: {fighter2.GetInfo()}");
        Console.WriteLine("\n" + new string('-', 60) + "\n");

        Fighter attacker = _random.Next(2) == 0 ? fighter1 : fighter2;
        Fighter defender = attacker == fighter1 ? fighter2 : fighter1;

        int round = 1;
        while (fighter1.IsAlive && fighter2.IsAlive)
        {
            Console.WriteLine($"Round {round}:");
            
            int damage = attacker.CalculateDamage();
            defender.TakeDamage(damage);

            Console.WriteLine($"  {attacker.Name} deals {damage} damage to {defender.Name}. HP remaining: {defender.CurrentHealth}/{defender.MaxHealth}");

            if (!defender.IsAlive)
            {
                break;
            }

            (attacker, defender) = (defender, attacker);
            round++;
        }

        Console.WriteLine("\n" + new string('-', 60));
        Console.WriteLine("BATTLE RESULT:");
        if (fighter1.IsAlive)
        {
            Console.WriteLine($"WINNER: {fighter1.Name} ({faction1.Name})");
            Console.WriteLine($"LOSER: {fighter2.Name} ({faction2.Name})");
        }
        else
        {
            Console.WriteLine($"WINNER: {fighter2.Name} ({faction2.Name})");
            Console.WriteLine($"LOSER: {fighter1.Name} ({faction1.Name})");
        }
        Console.WriteLine(new string('=', 60) + "\n");
    }

    public void SquadVsSquad()
    {
        if (_factions.Count < 2)
        {
            Console.WriteLine("Need at least 2 factions for Squad vs Squad battle.");
            return;
        }

        var faction1 = _factions[0];
        var faction2 = _factions[1];

        var squad1 = faction1.GetRandomSquadWithAliveFighters();
        var squad2 = faction2.GetRandomSquadWithAliveFighters();

        if (squad1 == null || squad2 == null)
        {
            Console.WriteLine("Cannot select squads with alive fighters for battle.");
            return;
        }

        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("SQUAD VS SQUAD BATTLE");
        Console.WriteLine(new string('=', 60) + "\n");
        Console.WriteLine($"Squad 1: {squad1.Name} from {faction1.Name} ({squad1.GetAliveFighters().Count} fighters)");
        Console.WriteLine($"Squad 2: {squad2.Name} from {faction2.Name} ({squad2.GetAliveFighters().Count} fighters)");
        Console.WriteLine("\n" + new string('-', 60) + "\n");

        var fighters1 = squad1.Fighters;
        var fighters2 = squad2.Fighters;

        int index1 = 0;
        int index2 = 0;
        int round = 1;

        while (squad1.HasAliveFighters() && squad2.HasAliveFighters())
        {
            Console.WriteLine($"Round {round}:");

            Fighter? attacker1 = GetNextAliveFighter(fighters1, ref index1);
            if (attacker1 != null)
            {
                Fighter? target2 = GetFighterAtIndex(fighters2, index2);
                if (target2 != null)
                {
                    int damage = attacker1.CalculateDamage();
                    target2.TakeDamage(damage);
                    Console.WriteLine($"  [{squad1.Name}] {attacker1.Name} attacks {target2.Name} for {damage} damage. HP: {target2.CurrentHealth}/{target2.MaxHealth} {(target2.IsAlive ? "" : "- DEAD")}");
                }
            }

            if (!squad2.HasAliveFighters())
            {
                break;
            }

            Fighter? attacker2 = GetNextAliveFighter(fighters2, ref index2);
            if (attacker2 != null)
            {
                int targetIndex1 = index1 - 1;
                if (targetIndex1 < 0) targetIndex1 = fighters1.Count - 1;
                
                Fighter? target1 = GetFighterAtIndex(fighters1, targetIndex1);
                if (target1 != null)
                {
                    int damage = attacker2.CalculateDamage();
                    target1.TakeDamage(damage);
                    Console.WriteLine($"  [{squad2.Name}] {attacker2.Name} attacks {target1.Name} for {damage} damage. HP: {target1.CurrentHealth}/{target1.MaxHealth} {(target1.IsAlive ? "" : "- DEAD")}");
                }
            }

            Console.WriteLine();
            round++;
        }

        Console.WriteLine(new string('-', 60));
        Console.WriteLine("BATTLE RESULT:");

        Squad winnerSquad;
        Faction winnerFaction;
        Squad loserSquad;
        Faction loserFaction;

        if (squad1.HasAliveFighters())
        {
            winnerSquad = squad1;
            winnerFaction = faction1;
            loserSquad = squad2;
            loserFaction = faction2;
        }
        else
        {
            winnerSquad = squad2;
            winnerFaction = faction2;
            loserSquad = squad1;
            loserFaction = faction1;
        }

        Console.WriteLine($"WINNER: {winnerSquad.Name} ({winnerFaction.Name})");
        Console.WriteLine($"LOSER: {loserSquad.Name} ({loserFaction.Name})");
        Console.WriteLine("\nHEROES (Survivors):");

        var survivors = winnerSquad.GetAliveFighters();
        foreach (var hero in survivors)
        {
            Console.WriteLine($"  - {hero.Name} (HP: {hero.CurrentHealth}/{hero.MaxHealth})");
        }

        Console.WriteLine(new string('=', 60) + "\n");
    }

    private Fighter? GetNextAliveFighter(List<Fighter> fighters, ref int index)
    {
        int startIndex = index;
        int attempts = 0;

        while (attempts < fighters.Count)
        {
            if (fighters[index].IsAlive)
            {
                Fighter result = fighters[index];
                index = (index + 1) % fighters.Count;
                return result;
            }

            index = (index + 1) % fighters.Count;
            attempts++;
        }

        return null;
    }

    private Fighter? GetFighterAtIndex(List<Fighter> fighters, int index)
    {
        if (index < 0 || index >= fighters.Count)
        {
            return null;
        }

        if (fighters[index].IsAlive)
        {
            return fighters[index];
        }

        for (int i = 0; i < fighters.Count; i++)
        {
            int checkIndex = (index + i) % fighters.Count;
            if (fighters[checkIndex].IsAlive)
            {
                return fighters[checkIndex];
            }
        }

        return null;
    }
}
