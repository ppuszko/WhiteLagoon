using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers {
    //allows to access these methods only for logged in admin users
    [Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller {
        private readonly IUnitOfWork _unitOfWork;
        public AmenityController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index() {
            var amenities = _unitOfWork.Amenity.GetAll(includeProperties:"Villa");
            return View(amenities);
        }

        public IActionResult Create() {
            AmenityVM amenityVm = new() {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
                
            return View(amenityVm);
        }
        [HttpPost]
        public IActionResult Create(AmenityVM obj) {
            if (ModelState.IsValid) {
                _unitOfWork.Amenity.Add(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "Amenity created succesfully";
                return RedirectToAction("Index", "Amenity");
            }
            TempData["error"] = "Amenity wasn't created. Please check if data is valid";
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        } 


        public IActionResult Edit(int id) {
            if (id == null | id == 0) {
                return RedirectToAction("Error", "Home");
            }
            AmenityVM amenityVM = new() {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == id)
            };
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Edit(AmenityVM amenityVM) {
            if (ModelState.IsValid) {
                _unitOfWork.Amenity.Update(amenityVM.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "Amenity updated succesfully!";
                return RedirectToAction("Index", "Amenity");
            }
            amenityVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            TempData["error"] = "Amenity wasn't updated. Please check if data is valid";
            return View(amenityVM);
        }
        public IActionResult Delete(int id) {
            if(id==null || id == 0) {
                return RedirectToAction("Error", "Home");
            }
            AmenityVM objToDelete = new() {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u=>u.Id == id)
            };
            return View(objToDelete);
        }
        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM) {
            Amenity? objToDelete = _unitOfWork.Amenity.Get(u => u.Id == amenityVM.Amenity.Id);
            if(objToDelete != null) {

                _unitOfWork.Amenity.Remove(objToDelete);
                _unitOfWork.Save();
                TempData["success"] = "Amenity deleted succesfully!";
                return RedirectToAction("Index", "Amenity");
            }
            TempData["error"] = "Something went wrong, try again later";
            return View();

        }

    }
}
//if (id == 0 || id == null) {
//    return RedirectToAction("Error", "Home");
//}