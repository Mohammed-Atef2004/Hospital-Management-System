using BLL.Services.Abstraction;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET api/Department/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var result = await _departmentService.GetDepartmentByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // GET api/Department
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var result = await _departmentService.GetAllDepartments();
            return Ok(result);
        }

        // POST api/Department
        [HttpPost]
        public async Task<IActionResult> AddDepartment([FromBody] Department department)
        {
            var (_, msg) = await _departmentService.AddDepartmentAsync(department);
            return Ok(msg);
        }

        // PUT api/Department
        [HttpPut]
        public async Task<IActionResult> UpdateDepartment([FromBody] Department department)
        {
            var (_, msg) = await _departmentService.UpdateDepartmentAsync(department);
            return Ok(msg);
        }

        // DELETE api/Department/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var (_, msg) = await _departmentService.DeleteDepartmentAsync(id);
            return Ok(msg);
        }

        
    }
}
