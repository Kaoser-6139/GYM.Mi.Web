using AutoMapper;

using GYM.Domain.Entities;
using GYM.Domain.Services;
using GYM.Infrastructure;
using GYM.Mi.Areas.Admin.Models;
using GYM.Mi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Identity;
using System.Data;
using System.Web;

namespace GYM.Mi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UsersController(ILogger<UsersController> logger, IUserService userService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }

        public IActionResult Welcome()
        {
            return View();
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

                    _userService.AddUser(user);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Your Information Added Succesfully.",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
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
            var model = new UpdateUserMode();
             var user=_userService.GetUser(id);
            _mapper.Map(user, model);   
            return View(model);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult  Update(UpdateUserMode model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var author = _mapper.Map<User>(model);

                    _userService.Update(author);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "User updated",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
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
        [HttpPost,ValidateAntiForgeryToken]
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
                        "FullName", "Age", "Gender",
                        "PhoneNumber", "EntryDate", "HeightCm",
                        "WeightKg", "BMI", "MedicalConditions",
                         "InjuryNotes", "PrimaryGoal", "WorkoutPreference",
                         "WorkoutTimePreference", "Id"),
                    model.Search
                );

                var users = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                        HttpUtility.HtmlEncode(record.FullName),
                        record.Age.ToString(),
                        HttpUtility.HtmlEncode(record.Gender),
                        record.PhoneNumber ?? "",
                        record.EntryDate.ToShortDateString(),
                        record.HeightCm.ToString(),
                        record.WeightKg.ToString(),
                        record.BMI.ToString(),
                        HttpUtility.HtmlEncode(record.MedicalConditions ?? ""),
                        HttpUtility.HtmlEncode(record.InjuryNotes ?? ""),
                        HttpUtility.HtmlEncode(record.PrimaryGoal ?? ""),
                        HttpUtility.HtmlEncode(record.WorkoutPreference ?? ""),
                        HttpUtility.HtmlEncode(record.WorkoutTimePreference ?? ""),
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


    }
}
