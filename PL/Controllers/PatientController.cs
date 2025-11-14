using BLL.Services.Abstraction;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET api/Patient/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var result = await _patientService.GetPatientByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // GET api/Patient
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var result = await _patientService.GetAllPatients();
            return Ok(result);
        }

        // POST api/Patient
        [HttpPost]
        public async Task<IActionResult> AddPatient([FromBody] Patient patient)
        {
            var (_, msg) = await _patientService.AddPatientAsync(patient);
            return Ok(msg);
        }

        // PUT api/Patient
        [HttpPut]
        public async Task<IActionResult> UpdatePatient([FromBody] Patient patient)
        {
            var (_, msg) = await _patientService.UpdatePatientAsync(patient);
            return Ok(msg);
        }

        // DELETE api/Patient/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var (_, msg) = await _patientService.DeletePatientAsync(id);
            return Ok(msg);
        }

        // POST api/Patient/5/appointments
        [HttpPost("{id:int}/appointments")]
        public async Task<IActionResult> AddAppointmentToPatient(int id, [FromBody] Appointment appointment)
        {
            var (_, msg) = await _patientService.AddAppointmentToPatient(appointment, id);
            return Ok(msg);
        }

        // GET api/Patient/5/appointments
        [HttpGet("{id:int}/appointments")]
        public async Task<IActionResult> GetPatientAppointments(int id)
        {
            var result = await _patientService.GetPatientAppointments(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
