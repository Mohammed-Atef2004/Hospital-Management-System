using Application.Features.Departments.Commands.Activate;
using Application.Features.Departments.Commands.Change_Discription;
using Application.Features.Departments.Commands.Create;
using Application.Features.Departments.Commands.Deactivate;
using Application.Features.Departments.Commands.RenameDepartment;
using Application.Features.Departments.Queries.GetAll;
using Application.Features.Departments.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/departments")]
[Authorize(Policy = "Administrative")]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    [HttpPut("{id:int}/rename")]
    public async Task<IActionResult> Rename(int id, [FromBody] string name, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RenameDepartmentCommand(id, name), cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/change-description")]
    public async Task<IActionResult> ChangeDescription(int id, [FromBody] string description, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ChangeDepartmentDescriptionCommand(id, description), cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/activate")]
    public async Task<IActionResult> Activate(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ActivateDepartmentCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/deactivate")]
    public async Task<IActionResult> Deactivate(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeactivateDepartmentCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetDepartmentByIdQuery(id), cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetAllDepartmentsQuery(), cancellationToken));
}
