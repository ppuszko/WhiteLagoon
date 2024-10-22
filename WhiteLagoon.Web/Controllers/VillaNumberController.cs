using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers {
    public class VillaNumberController : Controller {
        private readonly IUnitOfWork _unitOfWork;
        public VillaNumberController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index() {
            
            //.Include() populates Villa navigation property in VillaNumber, which allows for accessing Villa's properties
            //EF also allows for multiple inclusions by using another .Include and for layered inclusions by using .ThenInclude
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }
        public IActionResult Create() {
            VillaNumberVM villaNumberVM = new() {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Create(VillaNumberVM obj) {
            //ModelState.Remove("Villa");
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists) {
                _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "Created succesfully!";
                return RedirectToAction("Index", "VillaNumber");
            }
            if (roomNumberExists) {
                TempData["error"] = "that room number already exists";
            }
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);  
        }

        public IActionResult Edit(int villaNumberId) {
            VillaNumberVM villaNumberVM = new() {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u=>u.Villa_Number == villaNumberId)
            };
            if(villaNumberVM.VillaNumber == null) {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Edit(VillaNumberVM villaNumberVM) {
            

            if (ModelState.IsValid) {
                _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "Edited succesfully!";
                return RedirectToAction(/*solves problem of using magic string, works only when redirecting to action inside this controller
                                         */nameof(Index), "VillaNumber");
            }
            
            villaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(villaNumberVM);
        }

        public IActionResult Delete(int villaNumberId) {
            VillaNumberVM villaNumberVM = new() {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()

                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber is null) {
                return RedirectToAction("Error", "Home");
            }
            
            return View(villaNumberVM);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(VillaNumberVM villaNumberVm) {
            VillaNumber? objToDelete = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberVm.VillaNumber.Villa_Number);
            if (objToDelete is not null) {
                _unitOfWork.VillaNumber.Remove(objToDelete);
                _unitOfWork.Save();
                TempData["success"] = "Deleted succesfully!";
                return RedirectToAction(nameof(Index), "VillaNumber");
            }
            TempData["error"] = "Villa number could not be deleted!";
            return View();
        }

    }
}
