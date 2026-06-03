using GYM.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Runtime.Intrinsics.Arm;

namespace GYM.Mi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin,Manager,Trainer")]
    public class DashboardController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEquipmentService _equipmentService;
        private readonly IEmployeeService _employeeService;
        private readonly IMembershipService _membershipService;

        public DashboardController(
            IUserService userService,
            IEquipmentService equipmentService,
            IEmployeeService employeeService,
            IMembershipService membershipService)
        {
            _userService = userService;
            _equipmentService = equipmentService;
            _employeeService = employeeService;
            _membershipService = membershipService;
        }

        public IActionResult Index()
        {
            int totalUsers = _userService.GetTotalUsersCount();
            int totalEquipments = _equipmentService.GetTotalEquipmentCount();
            int totalEmployees = _employeeService.GetTotalEmployeeCount();
            decimal totalRevenue = _membershipService.GetTotalRevenue();

            ViewData["TotalUsers"] = totalUsers;
            ViewData["totalEquipments"] = totalEquipments;
            ViewData["totalEmployees"] = totalEmployees;
            ViewData["totalRevenue"] = totalRevenue;

            return View();
        }
    }
}
