using ProcurementRisk.Domain.Entities;
using ProcurementRisk.Domain.Services;
using Xunit;

namespace ProcurementRisk.Domain.Tests;

public class SupplierEvidenceTests
{
    private static readonly DateTime Now = new(2026, 9, 6, 3, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Evidence_requires_provenance_and_bounded_values()
    {
        var supplierId = Guid.NewGuid();
        Assert.Throws<ArgumentException>(() => SupplierEvidence.Record(supplierId, RiskFactor.Identity, "", "ref", Now, "checker", 80, 20, "summary", Now));
        Assert.Throws<ArgumentException>(() => SupplierEvidence.Record(supplierId, RiskFactor.Identity, "URL", "", Now, "checker", 80, 20, "summary", Now));
        Assert.Throws<ArgumentOutOfRangeException>(() => SupplierEvidence.Record(supplierId, RiskFactor.Identity, "URL", "ref", Now, "checker", 101, 20, "summary", Now));
        Assert.Throws<ArgumentOutOfRangeException>(() => SupplierEvidence.Record(supplierId, RiskFactor.Identity, "URL", "ref", Now, "checker", 80, -1, "summary", Now));
    }

    [Fact]
    public void Missing_evidence_keeps_score_unknown()
    {
        var evidence = new[] { Create(RiskFactor.Identity, 30, Now) };
        var result = RiskAssessmentCalculator.Calculate(evidence, Now);
        Assert.Equal(20m, result.EvidenceCoverage);
        Assert.Null(result.RiskScore);
        Assert.Equal("COLLECT_EVIDENCE", result.Recommendation);
        Assert.Contains(result.Factors, factor => factor.Factor == RiskFactor.FinancialStability && factor.EvidenceStatus == "NO_DATA");
    }

    [Fact]
    public void Stale_evidence_does_not_contribute_to_coverage()
    {
        var result = RiskAssessmentCalculator.Calculate(new[] { Create(RiskFactor.Identity, 30, Now.AddDays(-91)) }, Now);
        Assert.Equal(0m, result.EvidenceCoverage);
        Assert.Equal("STALE", result.Factors.Single(factor => factor.Factor == RiskFactor.Identity).EvidenceStatus);
    }

    [Fact]
    public void Complete_fresh_evidence_produces_deterministic_weighted_score()
    {
        var evidence = new[]
        {
            Create(RiskFactor.Identity, 10, Now),
            Create(RiskFactor.FinancialStability, 20, Now),
            Create(RiskFactor.OperationalCapacity, 30, Now),
            Create(RiskFactor.Compliance, 40, Now),
            Create(RiskFactor.SupplyContinuity, 50, Now)
        };
        var result = RiskAssessmentCalculator.Calculate(evidence, Now);
        Assert.Equal(100m, result.EvidenceCoverage);
        Assert.Equal(28.5m, result.RiskScore);
        Assert.Equal("OWNER_REVIEW", result.Recommendation);
    }

    private static SupplierEvidence Create(RiskFactor factor, decimal riskValue, DateTime observedAt) =>
        SupplierEvidence.Record(Guid.NewGuid(), factor, "PUBLIC_URL", "https://example.invalid/evidence", observedAt, "reviewer-01", 80, riskValue, "Test evidence", Now);
}
