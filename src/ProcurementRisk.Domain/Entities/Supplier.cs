namespace ProcurementRisk.Domain.Entities;

public class Supplier
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public decimal? RiskScore { get; private set; }
    public string? Reasoning { get; private set; }

    private Supplier() { }

    public static Supplier Create(string name, string country)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(country);

        return new Supplier
        {
            Id = Guid.NewGuid(),
            Name = name,
            Country = country,
            RiskScore = null,
            Reasoning = null
        };
    }

    public void Update(string name, string country)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(country);
        Name = name;
        Country = country;
    }

    public void ApplyScore(decimal score, string? reasoning)
    {
        if (score < 0 || score > 100)
            throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 100.");
        RiskScore = score;
        Reasoning = reasoning;
    }
}
