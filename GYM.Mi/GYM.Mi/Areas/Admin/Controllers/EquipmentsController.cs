using AutoMapper;
using GYM.Application.Services;
using GYM.Domain.Entities;
using GYM.Domain.Services;
using GYM.Infrastructure;
using GYM.Mi.Areas.Admin.Models;
using GYM.Mi.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace GYM.Mi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin,Manager,Trainer")]
    public class EquipmentsController : Controller
    {
        private readonly ILogger<EquipmentsController> _logger;
        private readonly IEquipmentService _equipmentService;
        private readonly IMapper _mapper;

        public EquipmentsController(ILogger<EquipmentsController> logger,
            IEquipmentService equipmentService,
            IMapper mapper)
        {
            _logger = logger;
            _equipmentService = equipmentService;
            _mapper = mapper;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            var model=new AddEquipmentModel();
            return View(model);

        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(AddEquipmentModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var equipment=_mapper.Map<Equipment>(model);
                    equipment.Id = IdentityGenerator.NewSequentialGuid();
                    _equipmentService.AddEquipment(equipment);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Equipment Added Succesfully.",
                        Type = ResponseTypes.Success
                    });
                   return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to add Equipment");

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to add Equipment",
                        Type = ResponseTypes.Danger
                    });
                }

            }
            return View(model);
        }
        public IActionResult Update(Guid id)
        {
            var model=new UpdateEquipmentModel();
            var equipment=_equipmentService.GetEquipment(id);
            _mapper.Map(equipment, model);
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(UpdateEquipmentModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var equipment= _mapper.Map<Equipment>(model);
                    _equipmentService.Update(equipment);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Equipment updated",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update Equipment");

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to update Equipment",
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
                _equipmentService.DeleteEquipment(id);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Equipment deleted",
                    Type = ResponseTypes.Success

                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete Equipment");

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Failed to delete Equipment",
                    Type = ResponseTypes.Danger
                });
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(Guid id)
        {
            try
            {
                var equipment = _equipmentService.GetEquipment(id);

                if (equipment == null)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Equipment not found.",
                        Type = ResponseTypes.Danger
                    });

                    return RedirectToAction("Index");
                }

                return View(equipment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load Equipment details for Id: {EquipmentId}", id);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Failed to load Equipment details.",
                    Type = ResponseTypes.Danger
                });

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult GetEquipmentsJsonData([FromBody] EquipmentListModel model)
        {
            try
            {
                var (data, total, totalDisplay) = _equipmentService.GetEquipments(
                    model.PageIndex,
                    model.PageSize,
                    model.FormatSortExpression(
                        "Name",
                        "CategoryName",
                        "SerialNumber",
                        "Price",
                        "AvailabilityStatus",
                        "Condition",
                        "Location",
                        "Id"
                    ),
                    model.Search
                );

                var equipments = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                        HttpUtility.HtmlEncode(record.Name ?? ""),
                        HttpUtility.HtmlEncode(record.CategoryName ?? ""),
                        HttpUtility.HtmlEncode(record.SerialNumber ?? ""),
                        record.Price.ToString("F2"),
                        HttpUtility.HtmlEncode(record.AvailabilityStatus ?? ""),
                        HttpUtility.HtmlEncode(record.Condition ?? ""),
                        HttpUtility.HtmlEncode(record.Location ?? ""),
                        record.Id.ToString()
                            }).ToArray()
                };

                return Json(equipments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in getting Equipments.");
                return Json(DataTables.EmptyResult);
            }
        }
    }
}
