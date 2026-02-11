namespace ConsoleApp8;

public class Fighter
{
    private static readonly Random _random = new();

    public string Name { get; }
    public int MaxHealth { get; }
    public int CurrentHealth { get; private set; }
    public int Attack { get; }
    public int Strength { get; }
    public bool IsAlive => CurrentHealth > 0;

    public Fighter(string name)
    {
        Name = name;
        MaxHealth = 50 + _random.Next(1, 31); // 1 to 30 inclusive
        CurrentHealth = MaxHealth;
        Attack = 6 + _random.Next(1, 7); // 1 to 6 inclusive
        Strength = 1 + _random.Next(0, 5); // 0 to 4 inclusive
    }

    public int CalculateDamage()
    {
        return _random.Next(1, Attack + 1) + Strength;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
    }

    public string GetInfo()
    {
        return $"{Name} (HP: {CurrentHealth}/{MaxHealth}, ATK: {Attack}, STR: {Strength}) - {(IsAlive ? "Alive" : "Dead")}";
    }
}
