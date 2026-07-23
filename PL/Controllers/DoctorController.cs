using Application.Features.Doctors.Commands.Deactivate;
using Application.Features.Doctors.Commands.Reactivate;
using Application.Features.Doctors.Commands.Register;
using Application.Features.Doctors.Commands.RelocateDoctorOffice;
using Application.Features.Doctors.Commands.TransformToDepartment;
using Application.Features.Doctors.Commands.UpdateSpecialization;
using Application.Features.Doctors.Queries.GetAll;
using Application.Features.Doctors.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet("{id}")]
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
            return Ok(doctorId);
            //return CreatedAtAction(nameof(GetProfile), new { id = doctorId }, command);
        }

        [HttpPatch("{id}/relocate")]
        public async Task<IActionResult> Relocate(int id, [FromBody] RelocateRequest request)
        {
            // ندمج الـ ID من المسار مع البيانات من الـ Body
            await _mediator.Send(new RelocateDoctorOfficeCommand(id, request.NewAddress, request.NewCity));
            return NoContent();
        }

        [HttpPatch("{id}/update-specialization")]
        public async Task<IActionResult> UpdateSpecialization(int id, [FromBody] string newSpecialization)
        {
            await _mediator.Send(new UpdateDoctorSpecializationCommand(id, newSpecialization));
            return NoContent();
        }

        [HttpPatch("{id}/transfer-department")]
        public async Task<IActionResult> TransferDepartment(int id, [FromBody] int targetDepartmentId)
        {
            await _mediator.Send(new TransferDoctorToDepartmentCommand(id, targetDepartmentId));
            return NoContent();
        }





        // --- Lifecycle ---

        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _mediator.Send(new DeactivateDoctorCommand(id));
            return NoContent();
        }

        [HttpPost("{id}/reactivate")]
        public async Task<IActionResult> Reactivate(int id)
        {
            await _mediator.Send(new ReactivateDoctorCommand(id));
            return NoContent();
        }
    }

    public record RelocateRequest(string NewAddress, string NewCity);
}