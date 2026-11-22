using Application.Features.Doctors.Commands.Deactivate;
using Application.Features.Doctors.Commands.Reactivate;
using Application.Features.Doctors.Commands.Register;
using Application.Features.Doctors.Commands.RelocateDoctorOffice;
using Application.Features.Doctors.Commands.CorrectName;
using Application.Features.Doctors.Commands.TransformToDepartment;
using Application.Features.Doctors.Commands.UpdateContactDetails;
using Application.Features.Doctors.Commands.UpdateSpecialization;
using Application.Features.Doctors.Queries.GetAll;
using Application.Features.Doctors.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    [Authorize(Policy = "Administrative")]
    public class DoctorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DoctorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // --- Queries (Read Side) ---

        [HttpGet]
        public async Task<IActionResult> GetDoctors([FromQuery] GetAllDoctorsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            var result = await _mediator.Send(new GetDoctorByIdQuery(id));
            return result != null ? Ok(result) : NotFound();
        }


        // --- Commands (Write Side - Tasks) ---

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterNewDoctorCommand command)
        {
            var doctorId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDoctor), new { id = doctorId }, new { id = doctorId });
        }

        [HttpPatch("{id:int}/relocate")]
        public async Task<IActionResult> Relocate(int id, [FromBody] RelocateRequest request)
        {
            // ندمج الـ ID من المسار مع البيانات من الـ Body
            await _mediator.Send(new RelocateDoctorOfficeCommand(id, request.NewAddress, request.NewCity));
            return NoContent();
        }

        [HttpPatch("{id:int}/update-specialization")]
        public async Task<IActionResult> UpdateSpecialization(int id, [FromBody] string newSpecialization)
        {
            await _mediator.Send(new UpdateDoctorSpecializationCommand(id, newSpecialization));
            return NoContent();
        }

        [HttpPatch("{id:int}/name")]
        public async Task<IActionResult> CorrectName(int id, [FromBody] CorrectDoctorNameRequest request)
        {
            await _mediator.Send(new CorrectDoctorNameCommand(id, request.FirstName, request.LastName));
            return NoContent();
        }

        [HttpPatch("{id:int}/contact")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] UpdateDoctorContactRequest request)
        {
            await _mediator.Send(new UpdateDoctorContactDetailsCommand(id, request.PhoneNumber));
            return NoContent();
        }

        [HttpPatch("{id:int}/transfer-department")]
        public async Task<IActionResult> TransferDepartment(int id, [FromBody] int targetDepartmentId)
        {
            await _mediator.Send(new TransferDoctorToDepartmentCommand(id, targetDepartmentId));
            return NoContent();
        }





        // --- Lifecycle ---

        [HttpPost("{id:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _mediator.Send(new DeactivateDoctorCommand(id));
            return NoContent();
        }

        [HttpPost("{id:int}/reactivate")]
        public async Task<IActionResult> Reactivate(int id)
        {
            await _mediator.Send(new ReactivateDoctorCommand(id));
            return NoContent();
        }
    }

    public record RelocateRequest(string NewAddress, string NewCity);
    public record CorrectDoctorNameRequest(string FirstName, string LastName);
    public record UpdateDoctorContactRequest(string? PhoneNumber);
}
