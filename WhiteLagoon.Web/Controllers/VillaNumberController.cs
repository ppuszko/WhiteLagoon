using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            //gets every object of Villas table and converts them to list
            var villas = _db.VillaNumbers.ToList();
            return View(villas);
        }
        public IActionResult Create() {
            VillaNumberVM villaNumberVM = new() {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            //IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(u => new SelectListItem {
            //    Text = u.Name,
            //    Value = u.Id.ToString()
            //});

                //ViewData["VillaList"] = list;

            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Create(VillaNumber obj) {
            //ModelState.Remove("Villa");
            if (ModelState.IsValid) {
                _db.VillaNumbers.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Created succesfully!";
                return RedirectToAction("Index", "VillaNumber");
            }
            return View(obj);  
        }

        public IActionResult Edit(int? id) {
            VillaNumber? objToUpdate = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == id);
            if (objToUpdate == null) {
                return RedirectToAction("Error", "Home");
            }
            
            return View(objToUpdate);
        }
        [HttpPost]
        public IActionResult Edit(VillaNumber obj) {
            if (ModelState.IsValid && obj.Villa_Number>0) {
                _db.VillaNumbers.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Edited succesfully!";
                return RedirectToAction("Index", "Villa");
            }
            return View();
        }

        public IActionResult Delete(int? id) {
            Villa? objToDelete = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (objToDelete is null) {
                return RedirectToAction("Error", "Home");
            }
            
            return View(objToDelete);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) {
            Villa? objToDelete = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (objToDelete is not null) {
                _db.Villas.Remove(objToDelete);
                _db.SaveChanges();
                TempData["success"] = "Deleted succesfully!";
                return RedirectToAction("Index", "Villa");
            }
            return View();
        }
    }
}
