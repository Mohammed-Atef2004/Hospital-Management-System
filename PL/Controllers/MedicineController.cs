using Application.Features.Doctors.Queries.GetAll;
using Application.Features.Medicines.Commands.AddMedicine;
using Application.Features.Medicines.Commands.AdjustMedicineStock;
using Application.Features.Medicines.Commands.DeleteMedicine;
using Application.Features.Medicines.Commands.DispenseMedicine;
using Application.Features.Medicines.Commands.UpdateMedicine;
using Application.Features.Medicines.Queries.GetAll;
using Application.Features.Medicines.Queries.GetById;
using Application.Features.Medicines.Queries.GetExpiredMedicines;
using Application.Features.Medicines.Queries.GetExpiringMedicinesNextMonth;
using Application.Features.Medicines.Queries.GetLowStock;
using Application.Features.Medicines.Queries.GetWithDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/medicines")]
    [ApiController]
    [Authorize(Policy = "Clinical")]
    public class MedicineController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicineController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllMedicinesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetMedicineByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetWithDetails(int id)
        {
            var result = await _mediator.Send(new GetMedicineWithDetailsQuery(id));
            return Ok(result);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock([FromQuery] int threshold = 10)
        {
            var result = await _mediator.Send(new GetMedicinesLowStockQuery(threshold));
            return Ok(result);
        }

        [HttpGet("expired")]
        public async Task<IActionResult> GetExpired()
        {
            var result = await _mediator.Send(new GetExpiredMedicinesQuery());
            return Ok(result);
        }

        [HttpGet("expiring-next-month")]
        public async Task<IActionResult> GetExpiringNextMonth()
        {
            var result = await _mediator.Send(new GetExpiringMedicinesNextMonthQuery());
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Administrative")]
        public async Task<IActionResult> Create([FromBody] AddMedicineCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Administrative")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicineCommand command)
        {
            if (id != command.Id) return BadRequest("ID Mismatch");
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Administrative")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteMedicineCommand(id));
            return NoContent();
        }


        [HttpPost("adjust-stock")]
        [Authorize(Policy = "Administrative")]
        public async Task<IActionResult> AdjustStock([FromBody] MedicineAdjustStockCommand command)
        {
            var newStock = await _mediator.Send(command);
            return Ok(new { CurrentStock = newStock });
        }

        [HttpPost("dispense")]
        public async Task<IActionResult> Dispense([FromBody] DispenseMedicineCommand command)
        {
            var remainingStock = await _mediator.Send(command);
            return Ok(new { RemainingStock = remainingStock });
        }
    }
}
