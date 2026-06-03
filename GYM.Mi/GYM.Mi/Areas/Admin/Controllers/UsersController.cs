using AutoMapper;
using GYM.Domain.Dtos;
using GYM.Domain.Entities;
using GYM.Domain.Services;
using GYM.Infrastructure;
using GYM.Mi.Areas.Admin.Models;
using GYM.Mi.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Identity;
using NuGet.DependencyResolver;
using System.Data;
using System.Web;

namespace GYM.Mi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin,Manager,Trainer,User")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly IGeminiService _gemini;
        private readonly IMembershipService _membershipService;
        public UsersController(ILogger<UsersController> logger,
            IUserService userService,
            IMapper mapper,
            IGeminiService service,
            IEmployeeService employeeService,
            IMembershipService membershipService)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
            _gemini = service;
            _employeeService = employeeService;
            _membershipService = membershipService;
        }

        public IActionResult Welcome(Guid id)
        {
            var user = _userService.GetUser(id);
            return View(user);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add()
        {
            var model = new AddUserModel();
            return View(model);
        }



        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(AddUserModel model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    var user = _mapper.Map<User>(model);
                    // user.Id = GYM.Mi.Domain.IdentityGenerator.NewSequentialGuid();

                    _userService.AddUser(user);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Your Information Added Succesfully.",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Profile", new { id = user.Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to add Information");

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to add Information",
                        Type = ResponseTypes.Danger
                    });
                }
            }

            return View(model);

        }

        public IActionResult Update(Guid id)
        {
            var model = new UpdateUserModel();
            var user = _userService.GetUser(id);
            _mapper.Map(user, model);
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(UpdateUserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _mapper.Map<User>(model);

                    user.BMI = model.HeightCm > 0 && model.WeightKg > 0
                        ? Math.Round(model.WeightKg / Math.Pow(model.HeightCm / 100.0, 2), 1)
                        : null;

                    _userService.Update(user);

                    if (!string.IsNullOrWhiteSpace(model.PlanName) &&
                        model.Amount > 0 &&
                        model.DurationMonths > 0)
                    {
                        var membership = new Membership
                        {
                            UserId = model.Id,
                            PlanName = model.PlanName,
                            Amount = model.Amount,
                            DurationMonths = model.DurationMonths,
                            PaymentStatus = "Pending",
                            CreatedAt = DateTime.UtcNow
                        };

                        _membershipService.CreateMembership(membership);
                    }

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "User updated. Membership payment is pending for admin approval.",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Profile", new { id = model.Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update user");

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to update user",
                        Type = ResponseTypes.Danger
                    });
                }
            }

            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _userService.DeleteUser(id);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "User deleted",
                    Type = ResponseTypes.Success

                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete User");

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Failed to delete User",
                    Type = ResponseTypes.Danger
                });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetUsersJsonData([FromBody] UserListModel model)
        {
            try
            {
                var (data, total, totalDisplay) = _userService.GetUsers(
                    model.PageIndex,
                    model.PageSize,
                    model.FormatSortExpression(
                        "FullName",
                        "PhoneNumber",
                        "Age",
                        "Gender",
                        "PrimaryGoal",
                        "EntryDate",
                        "Id"
                    ),
                    model.Search
                );

                string GetMembershipText(Guid userId)
                {
                    var membershipHistory = _membershipService.GetMembershipHistory(userId);

                    var latestMembership = membershipHistory?
                        .OrderByDescending(x => x.CreatedAt)
                        .FirstOrDefault();

                    if (latestMembership == null)
                    {
                        return "No Membership";
                    }

                    var status = latestMembership.PaymentStatus;

                    if (latestMembership.PaymentStatus == "Pending")
                    {
                        status = "Pending";
                    }
                    else if (latestMembership.PaymentStatus == "Active" &&
                             latestMembership.ExpiryDate.HasValue &&
                             latestMembership.ExpiryDate.Value >= DateTime.UtcNow)
                    {
                        status = "Active";
                    }
                    else if (latestMembership.PaymentStatus == "Active")
                    {
                        status = "Expired";
                    }
                    else if (latestMembership.PaymentStatus == "Expired")
                    {
                        status = "Expired";
                    }

                    return $"{latestMembership.PlanName} {status}";
                }

                var users = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                        HttpUtility.HtmlEncode(record.FullName ?? ""),
                        HttpUtility.HtmlEncode(record.PhoneNumber ?? ""),
                        record.Age.ToString(),
                        HttpUtility.HtmlEncode(record.Gender ?? ""),
                        HttpUtility.HtmlEncode(record.PrimaryGoal ?? ""),
                        HttpUtility.HtmlEncode(GetMembershipText(record.Id)),
                        record.EntryDate.ToShortDateString(),
                        record.Id.ToString()
                            }).ToArray()
                };

                return Json(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in getting Users.");
                return Json(DataTables.EmptyResult);
            }
        }

        //Advanced Search

        [HttpPost]
        public async Task<JsonResult> GetUsersAdvancedJsonData([FromBody] UserListModel model)
        {
            try
            {
                model.SearchItem ??= new UserSearchModel();

                var searchDto = _mapper.Map<UserSearchDto>(model.SearchItem);

                var (data, total, totalDisplay) = await _userService.GetUsersSP(
                    model.PageIndex,
                    model.PageSize,
                    model.FormatSortExpression(
                        "FullName",
                        "PhoneNumber",
                        "Age",
                        "Gender",
                        "PrimaryGoal",
                        "MembershipText",
                        "EntryDate",
                        "Id"
                    ),
                    model.Search,
                    searchDto
                );

                var users = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                        HttpUtility.HtmlEncode(record.FullName ?? ""),
                        HttpUtility.HtmlEncode(record.PhoneNumber ?? ""),
                        record.Age.ToString(),
                        HttpUtility.HtmlEncode(record.Gender ?? ""),
                        HttpUtility.HtmlEncode(record.PrimaryGoal ?? ""),
                        HttpUtility.HtmlEncode(record.MembershipText ?? "No Membership"),
                        record.EntryDate.ToShortDateString(),
                        record.Id.ToString()
                            }).ToArray()
                };

                return Json(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in advanced user search.");
                return Json(DataTables.EmptyResult);
            }
        }

        public IActionResult Profile(Guid id)
        {
            var user = _userService.GetUser(id);

            if (user == null)
                return NotFound();

            Employee? trainer = null;

            if (user.TrainerEmployeeId != null)
            {
                trainer = _employeeService.GetEmployee(user.TrainerEmployeeId.Value);
            }

            var activeMembership = _membershipService.GetActiveMembership(id);
            var membershipHistory = _membershipService.GetMembershipHistory(id);

            var model = new UserProfileForTrainerViewModel
            {
                User = user,

                TrainerName = trainer != null
                    ? trainer.FirstName + " " + trainer.LastName
                    : "No Trainer Assigned",

                TrainerPhone = trainer?.PhoneNumber ?? "N/A",

                Membership = new MembershipViewModel
                {
                    ActiveMembership = activeMembership,
                    MembershipHistory = membershipHistory
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<string> AskGemini(Guid userId, string message)
        {
            if (string.IsNullOrEmpty(message))
                return "Please ask a question about your fitness, health, or workout.";

            try
            {
                
                var user = _userService.GetUser(userId);
                if (user == null)
                    return "User profile not found.";

                
                string userContext = $"Member Name: {user.FullName}\nAge: {user.Age}, Gender: {user.Gender}, Height: {user.HeightCm}cm, Weight: {user.WeightKg}kg, Goal: {user.PrimaryGoal}, Workout Preference: {user.WorkoutPreference}, Time Preference: {user.WorkoutTimePreference}, Medical: {user.MedicalConditions}, Injury: {user.InjuryNotes}.";

                
                string fullPrompt = $"{userContext}\n\nMember question: {message}\n\nPlease provide a personalized response (workout, nutrition, fitness advice etc.) addressing above member's context.";

                
                string geminiResponse = await _gemini.GetResponseAsync(fullPrompt);
                return geminiResponse;
            }
            catch (Exception ex)
            {
                return "Sorry, there was a problem connecting to your AI coach. Please try again later.";
            }
        }

      
    }
}
