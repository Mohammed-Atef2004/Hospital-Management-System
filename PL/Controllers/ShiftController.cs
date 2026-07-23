using Application.Features.Shifts.Commands.CreateShift;
using Application.Features.Shifts.Commands.RemoveShift;
using Application.Features.Shifts.Commands.UpdateShift;
using Application.Features.Shifts.DTOs;
using Application.Features.Shifts.Queries.GetAvailableShifts;
using Application.Features.Shifts.Queries.GetShiftById;
using Application.Features.Shifts.Queries.GetStaffShifts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/Shifts")]
    public class ShiftController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShiftController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateShiftCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPut("{shiftId}")]
        public async Task<ActionResult> Update([FromRoute] int shiftId, [FromBody] UpdateShiftCommand command)
        {
            if (shiftId != command.shiftId)
            {
                return BadRequest("Shift ID mismatch.");
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{shiftId}/staff/{staffId}")]
        public async Task<ActionResult> Remove([FromRoute] int shiftId, [FromRoute] int staffId)
        {
            var command = new RemoveShiftCommand(shiftId, staffId);
            await _mediator.Send(command);
            return NoContent();
        }


        [HttpGet("{shiftId}")]
        public async Task<ActionResult<ShiftDTO>> GetById([FromRoute] int shiftId)
        {
            var query = new GetShiftByIdQuery(shiftId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("staff/{staffId}")]
        public async Task<ActionResult<List<ShiftDTO>>> GetStaffShifts([FromRoute] int staffId)
        {
            var query = new GetStaffShiftsQuery(staffId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("available")]
        public async Task<ActionResult<List<ShiftDTO>>> GetAvailableShifts([FromQuery] GetAvailableShiftsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
