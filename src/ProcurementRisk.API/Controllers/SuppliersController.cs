using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProcurementRisk.Application.Suppliers.Commands.CreateSupplier;
using ProcurementRisk.Application.Suppliers.Commands.DeleteSupplier;
using ProcurementRisk.Application.Suppliers.Commands.ScoreSupplier;
using ProcurementRisk.Application.Suppliers.Commands.UpdateSupplier;
using ProcurementRisk.Application.Suppliers.Queries.GetAllSuppliers;

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
        var id = await _mediator.Send(new CreateSupplierCommand(request.Name, request.Country), ct);
        return CreatedAtAction(nameof(GetAll), new { id }, new { id });
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
        try
        {
            await _mediator.Send(new ScoreSupplierCommand(id, request.RiskScore, request.Reasoning), ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public record CreateSupplierRequest(string Name, string Country);
public record UpdateSupplierRequest(string Name, string Country);
public record ScoreRequest(decimal RiskScore, string? Reasoning);
