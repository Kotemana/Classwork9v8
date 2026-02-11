namespace ConsoleApp8;

public class Faction
{
    private static readonly Random _random = new();

    public string Name { get; }
    public List<Squad> Squads { get; }

    public Faction(string name)
    {
        Name = name;
        Squads = new List<Squad>();
    }

    public void AddSquad(Squad squad)
    {
        Squads.Add(squad);
    }

    public Squad? GetRandomSquad()
    {
        if (Squads.Count == 0)
        {
            return null;
        }
        return Squads[_random.Next(Squads.Count)];
    }

    public Squad? GetRandomSquadWithAliveFighters()
    {
        var squadsWithAliveFighters = Squads.Where(s => s.HasAliveFighters()).ToList();
        if (squadsWithAliveFighters.Count == 0)
        {
            return null;
        }
        return squadsWithAliveFighters[_random.Next(squadsWithAliveFighters.Count)];
    }

    public string GetFullReport()
    {
        var report = $"\n=== Faction: {Name} ===\n";
        report += $"Total Squads: {Squads.Count}\n\n";
        
        foreach (var squad in Squads)
        {
            report += squad.GetReport();
            report += "\n";
        }
        
        return report;
    }

    public bool HasSquadsWithAliveFighters()
    {
        return Squads.Any(s => s.HasAliveFighters());
    }
}
