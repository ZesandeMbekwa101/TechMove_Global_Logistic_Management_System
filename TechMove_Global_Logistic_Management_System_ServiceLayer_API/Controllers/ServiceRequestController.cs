using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Data;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.DTOs;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Models;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Services;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly FastForexService _fastForex;

        public ServiceRequestController(
            ApplicationDbContext context,
            FastForexService fastForex)
        {
            _context = context;
            _fastForex = fastForex;
        }

        // =======================
        // GET ALL
        // =======================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.ServiceRequests
                .OrderByDescending(x => x.Service_Request_Id)
                .Select(x => new ServiceRequestDto
                {
                    Service_Request_Id = x.Service_Request_Id,
                    Contract_Id = x.Contract_Id,
                    Service_Description = x.Service_Description,
                    ZAR_Amount = x.ZAR_Amount,
                    Target_Currency = x.Currency_Code,
                    Converted_Amount = x.USD_Amount,
                    Exchange_Rate = x.Exchange_Rate,
                    Status = x.Status,
                    Created_On = x.Created_On
                })
                .ToListAsync();

            return Ok(data);
        }

        // =======================
        // GET BY ID
        // =======================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var x = await _context.ServiceRequests.FindAsync(id);

            if (x == null)
                return NotFound();

            return Ok(new ServiceRequestDto
            {
                Service_Request_Id = x.Service_Request_Id,
                Contract_Id = x.Contract_Id,
                Service_Description = x.Service_Description,
                ZAR_Amount = x.ZAR_Amount,
                Target_Currency = x.Currency_Code,
                Converted_Amount = x.USD_Amount,
                Exchange_Rate = x.Exchange_Rate,
                Status = x.Status,
                Created_On = x.Created_On
            });
        }

        // =======================
        // CREATE (ZAR → USD/EUR/etc)
        // =======================
        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceRequestDto dto)
        {
            var contract = await _context.Contracts.FindAsync(dto.Contract_Id);

            if (contract == null)
                return NotFound("Contract not found");

            // 🔥 CALL FASTFOREX (ZAR → TARGET)
            var conversion = await _fastForex
                .ConvertZarToCurrencyAsync(dto.ZAR_Amount, dto.Target_Currency);

            if (conversion == null)
                return BadRequest("Conversion failed");

            var entity = new ServiceRequestModel
            {
                Contract_Id = dto.Contract_Id,
                Service_Description = dto.Service_Description,

                // BASE INPUT
                ZAR_Amount = dto.ZAR_Amount,

                // TARGET CURRENCY
                Currency_Code = dto.Target_Currency,

                // CONVERTED VALUES
                USD_Amount = conversion.ConvertedAmount,
                Exchange_Rate = conversion.Rate,

                Status = dto.Status,
                Created_On = DateTime.UtcNow
            };

            _context.ServiceRequests.Add(entity);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Service request created successfully",

                baseCurrency = conversion.Base,
                targetCurrency = conversion.Target,

                amountZAR = conversion.Amount,
                convertedAmount = conversion.ConvertedAmount,

                rate = conversion.Rate
            });
        }

        // =======================
        // UPDATE (ZAR → USD/EUR/etc)
        // =======================
        [HttpPut("UpdateServiceRequest")]
        public async Task<IActionResult> Update(UpdateServiceRequestDto dto)
        {
            var entity = await _context.ServiceRequests.FindAsync(dto.Service_Request_Id);

            if (entity == null)
                return NotFound("Service request not found");

            var contract = await _context.Contracts.FindAsync(dto.Contract_Id);

            if (contract == null)
                return NotFound("Contract not found");

            // 🔥 CALL FASTFOREX (same as CREATE)
            var conversion = await _fastForex
                .ConvertZarToCurrencyAsync(dto.ZAR_Amount, dto.Target_Currency);

            if (conversion == null)
                return BadRequest("Conversion failed");

            // =========================
            // UPDATE ENTITY (clean + consistent with Create)
            // =========================
            entity.Contract_Id = dto.Contract_Id;
            entity.Service_Description = dto.Service_Description;

            // BASE INPUT
            entity.ZAR_Amount = dto.ZAR_Amount;

            // TARGET CURRENCY
            entity.Currency_Code = dto.Target_Currency;

            // CONVERTED VALUES (FROM FASTFOREX — SAME AS CREATE)
            entity.USD_Amount = conversion.ConvertedAmount;
            entity.Exchange_Rate = conversion.Rate;

            entity.Status = dto.Status;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Service request updated successfully",

                baseCurrency = conversion.Base,
                targetCurrency = conversion.Target,

                amountZAR = conversion.Amount,
                convertedAmount = conversion.ConvertedAmount,

                rate = conversion.Rate
            });
        }

        // =======================
        // DELETE
        // =======================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var x = await _context.ServiceRequests.FindAsync(id);

            if (x == null)
                return NotFound();

            _context.ServiceRequests.Remove(x);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Deleted successfully" });
        }
    }
}