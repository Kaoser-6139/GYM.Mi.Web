using GYM.Domain.Services;
using GYM.Infrastructure;
using GYM.Mi.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GYM.Mi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class MembershipsController : Controller
    {
        private readonly IMembershipService _membershipService;
        private readonly ILogger<MembershipsController> _logger;

        public MembershipsController(
            IMembershipService membershipService,
            ILogger<MembershipsController> logger)
        {
            _membershipService = membershipService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var pendingMemberships = _membershipService.GetPendingMemberships();

            return View(pendingMemberships);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Approve(Guid id)
        {
            try
            {
                var approvedBy = User.Identity?.Name ?? "Admin";

                // Membership starts from the day admin approves cash payment
                _membershipService.ApproveMembership(id, DateTime.Today, approvedBy);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Membership approved successfully.",
                    Type = ResponseTypes.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to approve membership.");

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Failed to approve membership.",
                    Type = ResponseTypes.Danger
                });
            }

            return RedirectToAction("Index");
        }
    }
}