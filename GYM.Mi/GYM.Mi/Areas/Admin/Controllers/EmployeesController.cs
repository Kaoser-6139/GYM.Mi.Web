using AutoMapper;
using GYM.Application.Services;
using GYM.Domain.Entities;
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
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmployeesController(IEmployeeService employeeService,
            ILogger<EmployeesController> logger,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment)
        {
            _employeeService = employeeService;
            _logger = logger;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add()
        {
            var model = new AddEmployeeModel();
            return View(model);  
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(AddEmployeeModel model)
        {
            string? savedImageUrl = null;

            if (ModelState.IsValid)
            {
                try
                {
                    var employee = _mapper.Map<Employee>(model);

                    employee.Id = IdentityGenerator.NewSequentialGuid();
                    employee.IsActive = true;

                    savedImageUrl = SaveEmployeeImage(model.ImageFile);

                    if (!string.IsNullOrWhiteSpace(savedImageUrl))
                    {
                        employee.ImageUrl = savedImageUrl;
                    }

                    _employeeService.AddEmployee(employee);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Employee added successfully.",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    DeleteEmployeeImage(savedImageUrl);

                    _logger.LogError(ex, "Failed to add Employee");

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to add Employee",
                        Type = ResponseTypes.Danger
                    });
                }
            }

            return View(model);
        }
        public IActionResult Update(Guid id)
        {
            var employee = _employeeService.GetEmployee(id);

            if (employee == null)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Employee not found.",
                    Type = ResponseTypes.Danger
                });

                return RedirectToAction("Index");
            }

            var model = new UpdateEmployeeModel();
            _mapper.Map(employee, model);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(UpdateEmployeeModel model)
        {
            string? newImageUrl = null;

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEmployee = _employeeService.GetEmployee(model.Id);

                    if (existingEmployee == null)
                    {
                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "Employee not found.",
                            Type = ResponseTypes.Danger
                        });

                        return RedirectToAction("Index");
                    }

                    var oldImageUrl = existingEmployee.ImageUrl;

                    newImageUrl = SaveEmployeeImage(model.ImageFile);

                    if (!string.IsNullOrWhiteSpace(newImageUrl))
                    {
                        model.ImageUrl = newImageUrl;
                    }
                    else
                    {
                        model.ImageUrl = oldImageUrl;
                    }

                    _mapper.Map(model, existingEmployee);

                    existingEmployee.ImageUrl = model.ImageUrl;

                    _employeeService.Update(existingEmployee);

                    if (!string.IsNullOrWhiteSpace(newImageUrl))
                    {
                        DeleteEmployeeImage(oldImageUrl);
                    }

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Employee updated successfully.",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    DeleteEmployeeImage(newImageUrl);

                    _logger.LogError(ex, "Failed to update Employee");

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to update Employee",
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
                var employee = _employeeService.GetEmployee(id);

                if (employee == null)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Employee not found.",
                        Type = ResponseTypes.Danger
                    });

                    return RedirectToAction("Index");
                }

                var imageUrl = employee.ImageUrl;

                _employeeService.DeleteEmployee(id);

                DeleteEmployeeImage(imageUrl);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Employee deleted successfully.",
                    Type = ResponseTypes.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete Employee");

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Failed to delete Employee",
                    Type = ResponseTypes.Danger
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Activate(Guid id)
        {
            try
            {
                var employee = _employeeService.GetEmployee(id);

                if (employee == null)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Employee not found.",
                        Type = ResponseTypes.Danger
                    });

                    return RedirectToAction("Index");
                }

                employee.IsActive = true;
                _employeeService.Update(employee);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Employee activated successfully.",
                    Type = ResponseTypes.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to activate Employee");

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Failed to activate Employee.",
                    Type = ResponseTypes.Danger
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Deactivate(Guid id)
        {
            try
            {
                var employee = _employeeService.GetEmployee(id);

                if (employee == null)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Employee not found.",
                        Type = ResponseTypes.Danger
                    });

                    return RedirectToAction("Index");
                }

                employee.IsActive = false;
                _employeeService.Update(employee);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Employee deactivated successfully.",
                    Type = ResponseTypes.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deactivate Employee");

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Failed to deactivate Employee.",
                    Type = ResponseTypes.Danger
                });
            }

            return RedirectToAction("Index");
        }
        public IActionResult Details(Guid id)
        {
            try
            {
                var employee = _employeeService.GetEmployee(id);

                if (employee == null)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Employee not found.",
                        Type = ResponseTypes.Danger
                    });

                    return RedirectToAction("Index");
                }

                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load Employee details for Id: {EmployeeId}", id);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Failed to load Employee details.",
                    Type = ResponseTypes.Danger
                });

                return RedirectToAction("Index");
            }
        }

        private string? SaveEmployeeImage(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Only JPG, JPEG, PNG and WEBP image files are allowed.");
            }

            var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "employees");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            return $"/uploads/employees/{fileName}";
        }
        private void DeleteEmployeeImage(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return;
            }

            if (!imageUrl.StartsWith("/uploads/employees/", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            try
            {
                var relativePath = imageUrl
                    .TrimStart('/')
                    .Replace("/", Path.DirectorySeparatorChar.ToString());

                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to delete employee image: {ImageUrl}", imageUrl);
            }
        }


        [HttpPost]
        public JsonResult GetEmployeesJsonData([FromBody] EmployeeListModel model)
        {
            try
            {
                var (data, total, totalDisplay) = _employeeService.GetEmployees(
                    model.PageIndex,
                    model.PageSize,
                    model.FormatSortExpression(
                        "FirstName",
                        "PhoneNumber",
                        "Email",
                        "Department",
                        "WorkShift",
                        "Salary",
                        "IsActive",
                        "Id"
                    ),
                    model.Search
                );

                var employees = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                        WebUtility.HtmlEncode($"{record.FirstName} {record.LastName}".Trim()),
                        WebUtility.HtmlEncode(record.PhoneNumber ?? ""),
                        WebUtility.HtmlEncode(record.Email ?? ""),
                        WebUtility.HtmlEncode(record.Department ?? ""),
                        WebUtility.HtmlEncode(record.WorkShift ?? ""),
                        record.Salary.ToString("F2"),
                        record.IsActive ? "Yes" : "No",
                        record.Id.ToString()
                            }).ToArray()
                };

                return Json(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in getting Employees.");
                return Json(DataTables.EmptyResult);
            }
        }


    }
}
