using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProcurementRisk.Application.Suppliers.Commands.CreateSupplier;
using ProcurementRisk.Application.Suppliers.Commands.DeleteSupplier;
using ProcurementRisk.Application.Suppliers.Commands.ScoreSupplier;
using ProcurementRisk.Application.Suppliers.Commands.UpdateSupplier;
using ProcurementRisk.Application.Suppliers.Queries.GetAllSuppliers;
using ProcurementRisk.Application.Evidence;
using ProcurementRisk.Domain.Entities;

namespace ProcurementRisk.API.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllSuppliersQuery(), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSupplierRequest request, CancellationToken ct)
    {
        try
        {
            var id = await _mediator.Send(new CreateSupplierCommand(request.Name, request.Country), ct);
            return CreatedAtAction(nameof(GetAll), new { id }, new { id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSupplierRequest request, CancellationToken ct)
    {
        try
        {
            await _mediator.Send(new UpdateSupplierCommand(id, request.Name, request.Country), ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try
        {
            await _mediator.Send(new DeleteSupplierCommand(id), ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{id:guid}/score")]
    public async Task<IActionResult> Score(Guid id, [FromBody] ScoreRequest request, CancellationToken ct)
    {
        await Task.CompletedTask;
        return Conflict(new
        {
            error = "MANUAL_SCORE_DISABLED_USE_EVIDENCE",
            message = "M2 scores are calculated from complete, fresh structured evidence. Record evidence instead."
        });
    }

    [HttpGet("{id:guid}/evidence")]
    public async Task<IActionResult> GetEvidence(Guid id, CancellationToken ct)
    {
        try
        {
            var result = await _mediator.Send(new GetSupplierEvidenceQuery(id), ct);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{id:guid}/evidence")]
    public async Task<IActionResult> AddEvidence(Guid id, [FromBody] AddEvidenceRequest request, CancellationToken ct)
    {
        if (!Enum.TryParse<RiskFactor>(request.Factor, true, out var factor))
            return BadRequest(new { error = "RISK_FACTOR_INVALID", allowed = Enum.GetNames<RiskFactor>() });

        try
        {
            var evidenceId = await _mediator.Send(new AddSupplierEvidenceCommand(
                id, factor, request.SourceType, request.SourceReference, request.ObservedAtUtc,
                request.Reviewer, request.Confidence, request.RiskValue, request.Summary), ct);
            return Created($"/api/suppliers/{id}/evidence/{evidenceId}", new { id = evidenceId });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id:guid}/risk-summary")]
    public async Task<IActionResult> GetRiskSummary(Guid id, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new GetRiskSummaryQuery(id), ct));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

public record CreateSupplierRequest(string Name, string Country);
public record UpdateSupplierRequest(string Name, string Country);
public record ScoreRequest(decimal RiskScore, string? Reasoning);
public record AddEvidenceRequest(
    string Factor,
    string SourceType,
    string SourceReference,
    DateTime ObservedAtUtc,
    string Reviewer,
    decimal Confidence,
    decimal RiskValue,
    string Summary);
