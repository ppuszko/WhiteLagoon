using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers {
    public class VillaController : Controller {
        private readonly IVillaRepository _villaRepo;
        public VillaController(IVillaRepository villaRepo) {
            _villaRepo = villaRepo;
        }
        public IActionResult Index() {
            //gets every object of Villas table and converts them to list
            var villas = _villaRepo.GetAll();
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
                _villaRepo.Add(obj);
                _villaRepo.Save();
                TempData["success"] = "Created succesfully!";
                return RedirectToAction("Index", "Villa");
            }
            return View(obj);  
        }

        public IActionResult Edit(int? id) {
            Villa? objToUpdate = _villaRepo.Get(u => u.Id == id);
            if (objToUpdate == null) {
                return RedirectToAction("Error", "Home");
            }
            
            return View(objToUpdate);
        }
        [HttpPost]
        public IActionResult Edit(Villa obj) {
            if (ModelState.IsValid && obj.Id>0) {
                _villaRepo.Update(obj);
                _villaRepo.Save();
                TempData["success"] = "Edited succesfully!";
                return RedirectToAction("Index", "Villa");
            }
            return View();
        }

        public IActionResult Delete(int? id) {
            Villa? objToDelete = _villaRepo.Get(u => u.Id == id);
            if (objToDelete is null) {
                return RedirectToAction("Error", "Home");
            }
            
            return View(objToDelete);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) {
            Villa? objToDelete = _villaRepo.Get(u => u.Id == id);
            if (objToDelete is not null) {
                _villaRepo.Remove(objToDelete);
                _villaRepo.Save();
                TempData["success"] = "Deleted succesfully!";
                return RedirectToAction("Index", "Villa");
            }
            return View();
        }
    }
}
