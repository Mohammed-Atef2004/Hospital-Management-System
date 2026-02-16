using Application.Features.Departments.Commands.Activate;
using Application.Features.Departments.Commands.Change_Discription;
using Application.Features.Departments.Commands.Create;
using Application.Features.Departments.Commands.Deactivate;
using Application.Features.Departments.Commands.RenameDepartment;
using Application.Features.Departments.Queries.GetAll;
using Application.Features.Departments.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateDepartmentCommand cmd)
            => Ok(await _mediator.Send(cmd));

        [HttpPut("{id}/rename")]
        public async Task<IActionResult> Rename(int id, [FromBody] string name)
        {
            RenameDepartmentCommand cmd = new(id, name);
            return Ok(await _mediator.Send(cmd));
        }

        [HttpPut("{id}/change-description")]
        public async Task<IActionResult> ChangeDescription(int id,[FromBody] string discription)
        {
            ChangeDepartmentDescriptionCommand cmd = new(id, discription);
            return Ok(await _mediator.Send(cmd));
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
            => Ok(await _mediator.Send(new ActivateDepartmentCommand(id)));

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
            => Ok(await _mediator.Send(new DeactivateDepartmentCommand(id)));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
            => Ok(await _mediator.Send(new GetDepartmentByIdQuery(id)));

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _mediator.Send(new GetAllDepartmentsQuery()));
    }

}
