using Microsoft.AspNetCore.Mvc;
using TechMove_Global_Logistic_Management_System.Models.DTOs;
using TechMove_Global_Logistic_Management_System.Services;
using TechMove_Global_Logistic_Management_System.ViewModels;

namespace TechMove_Global_Logistic_Management_System.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly ServiceRequestService _serviceRequestService;

        public ServiceRequestController(
            ServiceRequestService serviceRequestService)
        {
            _serviceRequestService = serviceRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> ServiceRequestTabView()
        {
            var requests =
                await _serviceRequestService
                .GetAllServiceRequestsAsync();

            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult>
            AddServiceRequest(CreateServiceRequestDto dto)
        {
            var success =
                await _serviceRequestService
                .AddServiceRequestAsync(dto);

            if (!success)
            {
                TempData["Error"] =
                    "Failed to create service request";
            }
            else
            {
                TempData["Success"] =
                    "Service request created successfully";
            }

            return RedirectToAction(
                nameof(ServiceRequestTabView));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateServiceRequest(UpdateServiceRequestDto dto)
        {
            var success = await _serviceRequestService.UpdateServiceRequestAsync(dto);

            if (!success)
                TempData["Error"] = "Failed to update service request";
            else
                TempData["Success"] = "Service request updated successfully";

            return RedirectToAction(nameof(ServiceRequestTabView));
        }

        [HttpPost]
        public async Task<IActionResult>
            DeleteServiceRequest(int id)
        {
            var success =
                await _serviceRequestService
                .DeleteServiceRequestAsync(id);

            if (!success)
            {
                TempData["Error"] =
                    "Failed to delete service request";
            }
            else
            {
                TempData["Success"] =
                    "Service request deleted successfully";
            }

            return RedirectToAction(
                nameof(ServiceRequestTabView));
        }
    }
}