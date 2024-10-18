using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers {
    public class VillaController : Controller {
        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext db) {
            _db = db;
        }
        public IActionResult Index() {
            //gets every object of Villas table and converts them to list
            var villas = _db.Villas.ToList();
            return View(villas);
        }
        public IActionResult Create() {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa obj) {
            if (obj.Name == obj.Description) {
                ModelState.AddModelError("Name", "The description can not exactly match the name");
                
            }
            if (ModelState.IsValid) {
                _db.Villas.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Created succesfully!";
                return RedirectToAction("Index", "Villa");
            }
            return View(obj);  
        }

        public IActionResult Edit(int? id) {
            Villa? objToUpdate = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (objToUpdate == null) {
                return RedirectToAction("Error", "Home");
            }
            
            return View(objToUpdate);
        }
        [HttpPost]
        public IActionResult Edit(Villa obj) {
            if (ModelState.IsValid && obj.Id>0) {
                _db.Villas.Update(obj);
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
