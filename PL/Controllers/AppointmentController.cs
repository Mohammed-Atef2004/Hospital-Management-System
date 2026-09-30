
using Application.Features.Appointments.Commands.Cancel;
using Application.Features.Appointments.Commands;
using Application.Features.Appointments.Commands.Create;
using Application.Features.Appointments.Queries.GetAllAppointments;
using Application.Features.Appointments.Queries.GetAppointmentById;
using Application.Features.Appointments.Queries.GetCancelledAppointments;
using Application.Features.Appointments.Queries.GetDoctorAppointments;
using Application.Features.Appointments.Queries.GetPatientAppointments;
using Application.Features.Appointments.Queries.GetUpcomingAppointments;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Application.Features.Appointments.DTOs;
using Application.Features.Appointments.Commands.UpdateDoctor;
using Application.Features.Appointments.Commands.UpdateAppointmentDate;
using Application.Features.Appointments.Commands.UpdateDescription;
using Application.Features.Appointments.Commands.UpdatePatient;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ============================================================
    // Queries
    // ============================================================

    // GET: api/Appointments/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppointmentResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var query = new GetAppointmentByIdQuery(id);

        var appointment = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(appointment);
    }

    // GET: api/Appointments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var query = new GetAllAppointmentsQuery();

        var appointments = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(appointments);
    }

    // GET: api/Appointments/doctor/5
    [HttpGet("doctor/{doctorId:int}")]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetByDoctor(
        int doctorId,
        CancellationToken cancellationToken)
    {
        var query = new GetDoctorAppointmentsQuery(doctorId);

        var appointments = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(appointments);
    }

    // GET: api/Appointments/patient/5
    [HttpGet("patient/{patientId:int}")]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetByPatient(
        int patientId,
        CancellationToken cancellationToken)
    {
        var query = new GetPatientAppointmentsQuery(patientId);

        var appointments = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(appointments);
    }

    // GET: api/Appointments/upcoming
    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetUpcoming(
        CancellationToken cancellationToken)
    {
        var query = new GetUpcomingAppointmentsQuery();

        var appointments = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(appointments);
    }

    // GET: api/Appointments/cancelled
    [HttpGet("cancelled")]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetCancelled(
        CancellationToken cancellationToken)
    {
        var query = new GetCancelledAppointmentsQuery();

        var appointments = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(appointments);
    }


    // ============================================================
    // Commands
    // ============================================================

    // POST: api/Appointments
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAppointmentCommand command,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            command,
            cancellationToken);

        return Ok();
    }

    // PUT: api/Appointments/5/date
    [HttpPut("{id:int}/date")]
    public async Task<IActionResult> UpdateDate(
        int id,
         UpdateAppointmentDateCommand command,
        CancellationToken cancellationToken)
    {
       

        await _mediator.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    // PUT: api/Appointments/5/description
    [HttpPut("{id:int}/description")]
    public async Task<IActionResult> UpdateDescription(
        int id,
        UpdateDesciptionCommand command,
        CancellationToken cancellationToken)
    {
        

        await _mediator.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    // PUT: api/Appointments/5/doctor
    [HttpPut("{id:int}/doctor")]
    public async Task<IActionResult> ChangeDoctor(
        UpdateDoctorCommand command,
        CancellationToken cancellationToken)
    {
       

        await _mediator.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    // PUT: api/Appointments/5/patient
    [HttpPut("{id:int}/patient")]
    public async Task<IActionResult> ChangePatient(
        int id,
        UpdatePatientCommand command,
        CancellationToken cancellationToken)
    {
        

        await _mediator.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    // DELETE: api/Appointments/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Cancel(
        int id,
        CancellationToken cancellationToken)
    {
        var command = new CancelAppointmentCommand(id);

        await _mediator.Send(
            command,
            cancellationToken);

        return NoContent();
    }
}
