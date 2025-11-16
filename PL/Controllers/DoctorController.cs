using BLL.Services.Abstraction;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // GET api/Doctor/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var result = await _doctorService.GetDoctorByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // GET api/Doctor
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var result = await _doctorService.GetAllDoctors();
            return Ok(result);
        }

        // POST api/Doctor
        [HttpPost]
        public async Task<IActionResult> AddDoctor([FromBody] Doctor doctor)
        {
            var (_, msg) = await _doctorService.AddDoctorAsync(doctor);
            return Ok(msg);
        }

        // PUT api/Doctor
        [HttpPut]
        public async Task<IActionResult> UpdateDoctor([FromBody] Doctor doctor)
        {
            var (_, msg) = await _doctorService.UpdateDoctorAsync(doctor);
            return Ok(msg);
        }

        // DELETE api/Doctor/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var (_, msg) = await _doctorService.DeleteDoctorAsync(id);
            return Ok(msg);
        }

        
    }
}
