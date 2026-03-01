using Application.Features.Nurses.Commands.AddNurse;
using Application.Features.Nurses.Commands.CreateShift;
using Application.Features.Nurses.Commands.DeleteNurse;
using Application.Features.Nurses.Commands.DeleteShift;
using Application.Features.Nurses.Commands.UpdateNurse;
using Application.Features.Nurses.Commands.UpdateShift;
using Application.Features.Nurses.Queries.GetAllNurses;
using Application.Features.Nurses.Queries.GetAllShifts;
using Application.Features.Nurses.Queries.GetNurseById;
using Application.Features.Nurses.Queries.GetNursePatients;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/nurses")]
    public class NurseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NurseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetNurses([FromQuery] GetAllNursesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{nurseId}/shifts")]
        public async Task<IActionResult> GetShifts(int nurseId)
        {
            var result = await _mediator.Send(new GetNurseShiftsQuery(nurseId));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNurseById(int id)
        {
            var result = await _mediator.Send(new GetNurseByIdQuery(id));
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("{id}/patients")]
        public async Task<IActionResult> GetNursePatients(int id)
        {
            var result = await _mediator.Send(new GetNursePatientsQuery(id));
            return Ok(result);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(AddNurseCommand command)
        {
            var nurseId = await _mediator.Send(command);
            return Ok(nurseId);

        }
        [HttpPost("shift")]
        public async Task<IActionResult> CreateShift(CreateShiftCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);

        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateNurse(int id, [FromBody] UpdateNurseCommand command)
        {
            var updatedCommand = command with { NurseId = id };
            await _mediator.Send(updatedCommand);
            return NoContent();
        }

        [HttpPut("{nurseId}/update/shifts/{shiftId}")]
        public async Task<IActionResult> UpdateShift(
            int nurseId,
            int shiftId,
            [FromBody] UpdateShiftCommand command)
        {
            if (nurseId != command.NurseId || shiftId != command.ShiftId)
                return BadRequest("Route ids mismatch");
            var updatedCommand = command with { NurseId = nurseId, ShiftId = shiftId };
            await _mediator.Send(updatedCommand);
            return NoContent();
        }


        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteNurse(int id)
        {
            await _mediator.Send(new DeleteNurseCommand(id));
            return NoContent();
        }

        [HttpDelete("{nurseId}/shifts/{Shiftid}")]
        public async Task<IActionResult> DeleteShift(int nurseId, int Shiftid)
        {
            await _mediator.Send(new DeleteShiftCommand(nurseId, Shiftid));
            return NoContent();
        }
    }
}
