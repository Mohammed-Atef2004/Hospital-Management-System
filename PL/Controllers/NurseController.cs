using Application.Features.Nurses.Commands.AddNurse;
using Application.Features.Nurses.Commands.AssignPatientToNurse;
using Application.Features.Nurses.Commands.ChangeHeadNurseStatus;
using Application.Features.Nurses.Commands.ChangeNurseDepartment;
using Application.Features.Nurses.Commands.DeleteNurse;
using Application.Features.Nurses.Commands.EndPatientAssignment;
using Application.Features.Nurses.Commands.RecordVitalSigns;
using Application.Features.Nurses.Commands.TransferPatientToAnotherNurse;
using Application.Features.Nurses.Commands.UpdateNurse;
using Application.Features.Nurses.Queries.GetAllNurses;
using Application.Features.Nurses.Queries.GetNurseById;
using Application.Features.Nurses.Queries.GetShiftAssignedPatients;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/nurses")]
    [Authorize(Policy = "Clinical")]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNurseById(int id)
        {
            var result = await _mediator.Send(new GetNurseByIdQuery(id));
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("{shiftId}/nurses/{nurseId}/patients")]
        public async Task<ActionResult> GetShiftAssignedPatients(
            [FromRoute] int shiftId,
            [FromRoute] int nurseId,
            CancellationToken cancellationToken)
        {
            var query = new GetShiftAssignedPatientsQuery(shiftId, nurseId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(AddNurseCommand command)
        {
            var nurseId = await _mediator.Send(command);
            return Ok(nurseId);

        }


        [HttpPut("/update")]
        public async Task<IActionResult> UpdateNurse([FromBody] UpdateNurseCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }



        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteNurse(int id)
        {
            await _mediator.Send(new DeleteNurseCommand(id));
            return NoContent();
        }


        [HttpPost("assign")]
        public async Task<IActionResult> AssignPatientToNurse(
    [FromBody] AssignPatientToNurseCommand command,
    CancellationToken cancellationToken)
        {
            var assignmentId = await _mediator.Send(command, cancellationToken);

            return Ok(assignmentId);
        }

        [HttpPost("transfer-assignment")]
        public async Task<IActionResult> TransferPatientToAnotherNurse(
        TransferPatientToAnotherNurseCommand command,
        CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPost("end-assignment")]
        public async Task<IActionResult> EndPatientAssignment(
            EndPatientAssignmentCommand command,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPost("record-vital-signs")]
        public async Task<IActionResult> RecordVitalSigns(
    RecordVitalSignsCommand command,
    CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpPut("{nurseId}/department/{departmentId}")]
        public async Task<IActionResult> ChangeDepartment(int nurseId,
          int departmentId,
         CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new ChangeNurseDepartmentCommand(nurseId, departmentId),
                cancellationToken);

            return NoContent();
        }



        [HttpPut("{nurseId}/head-nurse-status")]
        public async Task<IActionResult> ChangeHeadNurseStatus(int nurseId,
         [FromBody] bool isHeadNurse,
          CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new ChangeHeadNurseStatusCommand(
                    nurseId,
                    isHeadNurse),
                cancellationToken);

            return NoContent();
        }
    }
}
