using ProcurementRisk.Domain.Entities;

namespace ProcurementRisk.Domain.Services;

public record RiskFactorResult(
    RiskFactor Factor,
    decimal Weight,
    string EvidenceStatus,
    decimal? RiskValue,
    decimal? Confidence,
    Guid? EvidenceId,
    DateTime? ObservedAtUtc,
    decimal? Contribution);

public record RiskAssessmentResult(
    decimal EvidenceCoverage,
    decimal? RiskScore,
    string Recommendation,
    IReadOnlyList<RiskFactorResult> Factors,
    string CalculationPolicy);

public static class RiskAssessmentCalculator
{
    private static readonly IReadOnlyDictionary<RiskFactor, decimal> Weights =
        new Dictionary<RiskFactor, decimal>
        {
            [RiskFactor.Identity] = 20m,
            [RiskFactor.FinancialStability] = 25m,
            [RiskFactor.OperationalCapacity] = 20m,
            [RiskFactor.Compliance] = 20m,
            [RiskFactor.SupplyContinuity] = 15m
        };

    public static RiskAssessmentResult Calculate(
        IEnumerable<SupplierEvidence> evidence,
        DateTime nowUtc,
        int freshnessDays = 90)
    {
        var activeByFactor = evidence
            .Where(item => !item.IsStale(nowUtc, freshnessDays))
            .GroupBy(item => item.Factor)
            .ToDictionary(group => group.Key, group => group.OrderByDescending(item => item.ObservedAtUtc).First());

        var results = new List<RiskFactorResult>();
        foreach (var pair in Weights)
        {
            if (!activeByFactor.TryGetValue(pair.Key, out var item))
            {
                var hasStale = evidence.Any(candidate => candidate.Factor == pair.Key);
                results.Add(new RiskFactorResult(pair.Key, pair.Value, hasStale ? "STALE" : "NO_DATA", null, null, null, null, null));
                continue;
            }

            results.Add(new RiskFactorResult(
                pair.Key,
                pair.Value,
                "RECORDED",
                item.RiskValue,
                item.Confidence,
                item.Id,
                item.ObservedAtUtc,
                item.RiskValue * pair.Value / 100m));
        }

        var coverage = results.Where(item => item.RiskValue.HasValue).Sum(item => item.Weight);
        decimal? score = coverage == 100m ? results.Sum(item => item.Contribution ?? 0m) : null;
        return new RiskAssessmentResult(
            coverage,
            score,
            score.HasValue ? "OWNER_REVIEW" : "COLLECT_EVIDENCE",
            results,
            "Score remains UNKNOWN until fresh evidence exists for 100% of weighted factors.");
    }
}
