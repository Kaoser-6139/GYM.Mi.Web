using AutoMapper;
using GYM.Infrastructure;
using GYM.Mi.Areas.Admin.Models;
using GYM.Mi.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

namespace GYM.Mi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        
       
        
        public RolesController(UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
          
           
        }

        public async Task<IActionResult> Index()
        {   

            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
            var managerUsers = await _userManager.GetUsersInRoleAsync("Manager");
            var trainerUsers = await _userManager.GetUsersInRoleAsync("Trainer");

            var filteredUsers = adminUsers
                                                 .Concat(managerUsers)
                                                 .Concat(trainerUsers)
                                                 .Distinct().ToList(); 

            var userRoleList = new List<UserRoleViewModel>(); 

            foreach (var user in filteredUsers)
            { 
                var roles = await _userManager.GetRolesAsync(user);

                userRoleList.Add(new UserRoleViewModel
                {    
                    Id = user.Id,
                    Name = $"{user.FirstName} {user.LastName}", 
                    Email = user.Email,
                    Roles = roles.ToList() });

            } return View(userRoleList); 
        }


        //public async Task<IActionResult> Index()
        //{
        //    var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
        //    var managerUsers = await _userManager.GetUsersInRoleAsync("Manager");
        //    var hrUsers = await _userManager.GetUsersInRoleAsync("HR");

        //    var filteredUsers = adminUsers.Concat(managerUsers)
        //                                                    .Concat(hrUsers)
        //                                                    .Distinct()
        //                                                    .ToList();

        //    var userRoleList = new List<UserRoleViewModel>();

        //    foreach (var user in filteredUsers)
        //    {
        //        var roles = await _userManager.GetRolesAsync(user);

        //        userRoleList.Add(new UserRoleViewModel
        //        {
        //            //Id = user.Id,
        //            Name = $"{user.FirstName} {user.LastName}",
        //            Email = user.Email,
        //            Roles = roles.ToList()
        //        });
        //    }

        //    return View(userRoleList); 
        //}



        public IActionResult AddRoleAsync()
        {
            var model=new AddRoleModel();
            return View(model);
        }

        //------------------------User To Role++++++++++++++++++++++++(Age User Hisebe Login korbe Then  Role Assign Hobe)

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddRoleAsync(AddRoleModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //            var user = await _userManager.FindByEmailAsync(model.Email);

        //            if (user == null)
        //            {
        //                TempData.Put("ResponseMessage", new ResponseModel
        //                {
        //                    Message = "User not found",
        //                    Type = ResponseTypes.Danger
        //                });
        //            }
        //            else
        //            {
        //                var result = await _userManager.AddToRoleAsync(user, model.Role);
        //                if (result.Succeeded)
        //                {
        //                    TempData.Put("ResponseMessage", new ResponseModel
        //                    {
        //                        Message = "Role added successfully",
        //                        Type = ResponseTypes.Success
        //                    });
        //                }
        //                else
        //                {
        //                    TempData.Put("ResponseMessage", new ResponseModel
        //                    {
        //                        Message = string.Join(", ", result.Errors.Select(e => e.Description)),
        //                        Type = ResponseTypes.Danger
        //                    });
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            TempData.Put("ResponseMessage", new ResponseModel
        //            {
        //                Message = "Role assign failed: " + ex.Message,
        //                Type = ResponseTypes.Danger
        //            });
        //        }
        //    }
        //    return View(model);
        //}

        //+=++++++++++++++++++++++++++ New User To Role++++++++++++++++++++++++++++(User Hisebe Login Na korleo Admin Role Add korte Parbe)
        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddRoleAsync(AddRoleModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var user = CreateUser();

        //            await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
        //            await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);

        //            user.RegistrationDate = DateTime.UtcNow;


        //            var result = await _userManager.CreateAsync(user, model.Password);

        //            await _userManager.AddToRoleAsync(user, model.Role);

        //            TempData.Put("ResponseMessage", new ResponseModel
        //            {
        //                Message = "Role added",
        //                Type = ResponseTypes.Success
        //            });
        //        }
        //        catch (Exception ex)
        //        {
        //            var message = "Role Create Failed";
        //            ModelState.AddModelError("RoleCreateFailed", message);
        //            TempData.Put("ResponseMessage", new ResponseModel
        //            {
        //                Message = message,
        //                Type = ResponseTypes.Danger
        //            });
        //        }
        //    }
        //    return View(model);
        //}
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(AddRoleModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    if (user == null)
                    {
                        user = CreateUser();
                        await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                        await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
                        user.RegistrationDate = DateTime.UtcNow;

                        var createResult = await _userManager.CreateAsync(user, model.Password);
                        if (!createResult.Succeeded)
                        {
                            TempData.Put("ResponseMessage", new ResponseModel
                            {
                                Message = "User creation failed",
                                Type = ResponseTypes.Danger
                            });
                            return RedirectToAction("Index");
                        }
                    }

                    var result = await _userManager.AddToRoleAsync(user, model.Role);

                    if (result.Succeeded)
                    {
                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "Role added successfully",
                            Type = ResponseTypes.Success
                        });
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "Role assignment failed",
                            Type = ResponseTypes.Danger
                        });
                    }
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Role assign failed: " + ex.Message,
                        Type = ResponseTypes.Danger
                    });
                }
            }
            return View(model);
        }



        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteRole(Guid id, string role)
        //{
        //    var user = await _userManager.FindByIdAsync(id.ToString());
        //    if (user == null)
        //    {
        //        TempData["ResponseMessage"] = "User not found!";
        //        return RedirectToAction("Index");
        //    }
        //    if (string.IsNullOrEmpty(role))
        //    {
        //        TempData["ResponseMessage"] = "No role specified!";
        //        return RedirectToAction("Index");
        //    }
        //    var result = await _userManager.RemoveFromRoleAsync(user, role);
        //    TempData["ResponseMessage"] =
        //        result.Succeeded ? "Role removed successfully!"
        //        : "Failed: " + string.Join(", ", result.Errors.Select(e => e.Description));
        //    return RedirectToAction("Index");
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteRole(Guid id, string role)
        //{
        //    var user = await _userManager.FindByIdAsync(id.ToString());
        //    if (user == null || string.IsNullOrEmpty(role))
        //        return RedirectToAction("Index");

        //    await _userManager.RemoveFromRoleAsync(user, role);
        //    return RedirectToAction("Index");
        //}







        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
