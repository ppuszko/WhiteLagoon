using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers {
    public class VillaNumberController : Controller {
        private readonly ApplicationDbContext _db;
        public VillaNumberController(ApplicationDbContext db) {
            _db = db;
        }
        public IActionResult Index() {
            
            //.Include() populates Villa navigation property in VillaNumber, which allows for accessing Villa's properties
            //EF also allows for multiple inclusions by using another .Include and for layered inclusions by using .ThenInclude
            var villaNumbers = _db.VillaNumbers.Include(u => u.Villa).ToList();


            return View(villaNumbers);
        }
        public IActionResult Create() {
            VillaNumberVM villaNumberVM = new() {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Create(VillaNumberVM obj) {
            //ModelState.Remove("Villa");
            bool roomNumberExists = _db.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists) {
                _db.VillaNumbers.Add(obj.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "Created succesfully!";
                return RedirectToAction("Index", "VillaNumber");
            }
            if (roomNumberExists) {
                TempData["error"] = "that room number already exists";
            }
            obj.VillaList = _db.Villas.ToList().Select(u => new SelectListItem {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);  
        }

        public IActionResult Edit(int villaNumberId) {
            VillaNumberVM villaNumberVM = new() {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u=>u.Villa_Number == villaNumberId)
            };
            if(villaNumberVM.VillaNumber == null) {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Edit(VillaNumberVM villaNumberVM) {
            

            if (ModelState.IsValid) {
                _db.VillaNumbers.Update(villaNumberVM.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "Edited succesfully!";
                return RedirectToAction(/*solves problem of using magic string, works only when redirecting to action inside this controller
                                         */nameof(Index), "VillaNumber");
            }
            
            villaNumberVM.VillaList = _db.Villas.ToList().Select(u => new SelectListItem {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(villaNumberVM);
        }

        public IActionResult Delete(int villaNumberId) {
            VillaNumberVM villaNumberVM = new() {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()

                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber is null) {
                return RedirectToAction("Error", "Home");
            }
            
            return View(villaNumberVM);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(VillaNumberVM villaNumberVm) {
            VillaNumber? objToDelete = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberVm.VillaNumber.Villa_Number);
            if (objToDelete is not null) {
                _db.VillaNumbers.Remove(objToDelete);
                _db.SaveChanges();
                TempData["success"] = "Deleted succesfully!";
                return RedirectToAction(nameof(Index), "VillaNumber");
            }
            TempData["error"] = "Villa number could not be deleted!";
            return View();
        }
    }
}
