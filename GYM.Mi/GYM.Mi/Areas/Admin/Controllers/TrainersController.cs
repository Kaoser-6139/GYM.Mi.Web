using AutoMapper;
using GYM.Domain.Services;
using GYM.Infrastructure;
using GYM.Mi.Areas.Admin.Models;
using GYM.Mi.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GYM.Mi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin,Manager")]
    public class TrainersController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IUserService _userService;
        private readonly ILogger<TrainersController> _logger;
        private readonly IMapper _mapper;
        private readonly IMembershipService _membershipService;
        public TrainersController(IEmployeeService employeeService,
            IUserService userService,
            ILogger<TrainersController> logger,
            IMapper mapper,
            IMembershipService membershipService)
        {
            _employeeService = employeeService;
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
            _membershipService = membershipService;
        }
        public IActionResult Index()
        {
            return View();
        }



        public IActionResult ManageStudents(Guid id)
        {
            var trainer = _employeeService.GetEmployee(id);

            if (trainer == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<ManageStudentsForTrainerModel>(trainer);
            model.Id = id;

            // Assigned users: TrainerEmployeeId == trainer.Id
            var assignedUsers = _userService.GetAssignedUsers(model.Id);

            model.AssignedStudents = assignedUsers.Select(u =>
            {
                var membershipInfo = GetStudentMembershipDisplay(u.Id);

                return new ManageStudentsForAvailableStudentsModel
                {
                    Id = u.Id,
                    Name = u.FullName ?? "",
                    MembershipText = membershipInfo.text,
                    MembershipBadgeClass = membershipInfo.badgeClass
                };
            }).ToList();

            // Available users: unassigned users only
            var (data, total, totalDisplay) = _userService.GetAvailableUsers(
                pageIndex: 1,
                pageSize: 500,
                order: "FullName asc",
                search: new DataTablesSearch { Value = "" }
            );

            model.AvailableStudents = data.Select(u =>
            {
                var membershipInfo = GetStudentMembershipDisplay(u.Id);

                return new ManageStudentsForAvailableStudentsModel
                {
                    Id = u.Id,
                    Name = u.FullName ?? "",
                    MembershipText = membershipInfo.text,
                    MembershipBadgeClass = membershipInfo.badgeClass
                };
            }).ToList();

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddStudentToTrainer(Guid trainerId, Guid userId)
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            try
            {
                var trainer = _employeeService.GetEmployee(trainerId);

                if (trainer == null)
                {
                    throw new Exception("Trainer not found.");
                }

                _userService.AssignToTrainer(userId, trainerId);

                if (isAjax)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Student assigned successfully."
                    });
                }

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Student assigned successfully.",
                    Type = ResponseTypes.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to assign student to trainer");

                if (isAjax)
                {
                    return Json(new
                    {
                        success = false,
                        message = ex.Message
                    });
                }

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = ex.Message,
                    Type = ResponseTypes.Danger
                });
            }

            return RedirectToAction(nameof(ManageStudents), new { id = trainerId });
        }
        //--------------------
      

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult RemoveStudentFromTrainer(Guid trainerId, Guid userId)
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            try
            {
                var trainer = _employeeService.GetEmployee(trainerId);

                if (trainer == null)
                {
                    throw new Exception("Trainer not found.");
                }

                _userService.UnassignFromTrainer(userId);

                if (isAjax)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Student removed successfully."
                    });
                }

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Student removed successfully.",
                    Type = ResponseTypes.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove student from trainer");

                if (isAjax)
                {
                    return Json(new
                    {
                        success = false,
                        message = ex.Message
                    });
                }

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = ex.Message,
                    Type = ResponseTypes.Danger
                });
            }

            return RedirectToAction(nameof(ManageStudents), new { id = trainerId });
        }
        //--------------
        
        [HttpPost]
        public JsonResult GetTrainersJsonData([FromBody] EmployeeListModel model)
        {
            try
            {
                var order = model.FormatSortExpression(
                    "FirstName", "LastName", "PhoneNumber", "Email", "Id"
                ) ?? "FirstName asc";

                var (data, total, totalDisplay) = _employeeService.GetEmployees(
                    model.PageIndex,
                    model.PageSize,
                    order,
                    model.Search,
                    departmentFilter: "Trainer",
                    isActiveFilter: true
                );

                var trainers = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = data.Select(t =>
                    {
                        var studentCount = _userService.GetAssignedUsers(t.Id).Count;

                        return new string[]
                        {
                    WebUtility.HtmlEncode($"{t.FirstName} {t.LastName}"),
                    WebUtility.HtmlEncode(t.PhoneNumber ?? ""),
                    WebUtility.HtmlEncode(t.Email ?? ""),
                    studentCount.ToString(),
                    t.Id.ToString()
                        };
                    }).ToArray()
                };

                return Json(trainers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load trainers");
                return Json(DataTables.EmptyResult);
            }
        }

        private (string text, string badgeClass) GetStudentMembershipDisplay(Guid userId)
        {
            var activeMembership = _membershipService.GetActiveMembership(userId);

            if (activeMembership != null)
            {
                var planName = activeMembership.PlanName ?? "Membership";

                if (planName.Equals("Premium", StringComparison.OrdinalIgnoreCase))
                {
                    return ($"{planName} Active", "bg-success");
                }

                if (planName.Equals("Standard", StringComparison.OrdinalIgnoreCase))
                {
                    return ($"{planName} Active", "bg-primary");
                }

                if (planName.Equals("Basic", StringComparison.OrdinalIgnoreCase))
                {
                    return ($"{planName} Active", "bg-info text-dark");
                }

                return ($"{planName} Active", "bg-success");
            }

            var latestMembership = _membershipService
                .GetMembershipHistory(userId)
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefault();

            if (latestMembership == null)
            {
                return ("No Membership", "bg-secondary");
            }

            var latestPlanName = latestMembership.PlanName ?? "Membership";
            var paymentStatus = latestMembership.PaymentStatus ?? "Unknown";

            if (paymentStatus.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            {
                return ($"{latestPlanName} Pending", "bg-warning text-dark");
            }

            if (paymentStatus.Equals("Active", StringComparison.OrdinalIgnoreCase))
            {
                return ($"{latestPlanName} Expired", "bg-danger");
            }

            return ($"{latestPlanName} {paymentStatus}", "bg-secondary");
        }

    }
}
