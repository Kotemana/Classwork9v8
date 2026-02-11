namespace ConsoleApp8;

public class Squad
{
    private static readonly Random _random = new();

    public string Name { get; }
    public List<Fighter> Fighters { get; }

    public Squad(string name)
    {
        Name = name;
        Fighters = new List<Fighter>();
    }

    public void AddFighter(Fighter fighter)
    {
        Fighters.Add(fighter);
    }

    public List<Fighter> GetAliveFighters()
    {
        return Fighters.Where(f => f.IsAlive).ToList();
    }

    public Fighter? GetRandomAliveFighter()
    {
        var aliveFighters = GetAliveFighters();
        if (aliveFighters.Count == 0)
        {
            return null;
        }
        return aliveFighters[_random.Next(aliveFighters.Count)];
    }

    public bool HasAliveFighters()
    {
        return Fighters.Any(f => f.IsAlive);
    }

    public string GetReport()
    {
        var report = $"  Squad: {Name}\n";
        report += $"  Fighters ({Fighters.Count}):\n";
        foreach (var fighter in Fighters)
        {
            report += $"    - {fighter.GetInfo()}\n";
        }
        return report;
    }
}
