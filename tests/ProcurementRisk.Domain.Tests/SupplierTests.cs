using ProcurementRisk.Domain.Entities;
using Xunit;

namespace ProcurementRisk.Domain.Tests;

public class SupplierTests
{
    [Fact]
    public void Create_requires_identity_fields()
    {
        Assert.Throws<ArgumentException>(() => Supplier.Create("", "Thailand"));
        Assert.Throws<ArgumentException>(() => Supplier.Create("Supplier", " "));
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(100.1)]
    public void Score_rejects_values_outside_zero_to_one_hundred(decimal score)
    {
        var supplier = Supplier.Create("Verified Supplier", "Thailand");
        Assert.Throws<ArgumentOutOfRangeException>(() => supplier.ApplyScore(score, "evidence"));
    }

    [Fact]
    public void Score_persists_value_and_reasoning()
    {
        var supplier = Supplier.Create("Verified Supplier", "Thailand");
        supplier.ApplyScore(42.5m, "Owner-reviewed public evidence.");
        Assert.Equal(42.5m, supplier.RiskScore);
        Assert.Equal("Owner-reviewed public evidence.", supplier.Reasoning);
    }
}
