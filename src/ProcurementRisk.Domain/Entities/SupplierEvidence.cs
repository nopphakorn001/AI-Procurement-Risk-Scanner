namespace ProcurementRisk.Domain.Entities;

public enum RiskFactor
{
    Identity,
    FinancialStability,
    OperationalCapacity,
    Compliance,
    SupplyContinuity
}

public class SupplierEvidence
{
    public Guid Id { get; private set; }
    public Guid SupplierId { get; private set; }
    public RiskFactor Factor { get; private set; }
    public string SourceType { get; private set; } = string.Empty;
    public string SourceReference { get; private set; } = string.Empty;
    public DateTime ObservedAtUtc { get; private set; }
    public string Reviewer { get; private set; } = string.Empty;
    public decimal Confidence { get; private set; }
    public decimal RiskValue { get; private set; }
    public string Summary { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }

    private SupplierEvidence() { }

    public static SupplierEvidence Record(
        Guid supplierId,
        RiskFactor factor,
        string sourceType,
        string sourceReference,
        DateTime observedAtUtc,
        string reviewer,
        decimal confidence,
        decimal riskValue,
        string summary,
        DateTime? nowUtc = null)
    {
        if (supplierId == Guid.Empty) throw new ArgumentException("Supplier is required.", nameof(supplierId));
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceType);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceReference);
        ArgumentException.ThrowIfNullOrWhiteSpace(reviewer);
        ArgumentException.ThrowIfNullOrWhiteSpace(summary);
        if (confidence is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(confidence));
        if (riskValue is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(riskValue));

        var now = nowUtc ?? DateTime.UtcNow;
        var observed = DateTime.SpecifyKind(observedAtUtc, DateTimeKind.Utc);
        if (observed > now.AddMinutes(5))
            throw new ArgumentOutOfRangeException(nameof(observedAtUtc), "Observation time cannot be in the future.");

        return new SupplierEvidence
        {
            Id = Guid.NewGuid(),
            SupplierId = supplierId,
            Factor = factor,
            SourceType = sourceType.Trim(),
            SourceReference = sourceReference.Trim(),
            ObservedAtUtc = observed,
            Reviewer = reviewer.Trim(),
            Confidence = confidence,
            RiskValue = riskValue,
            Summary = summary.Trim(),
            CreatedAtUtc = now
        };
    }

    public bool IsStale(DateTime nowUtc, int freshnessDays = 90) =>
        ObservedAtUtc < nowUtc.AddDays(-freshnessDays);
}
