using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using System;

namespace WhiteLagoon.Web.Controllers {
    public class VillaController : Controller {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webhostEnvironment) {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webhostEnvironment;
        }
        public IActionResult Index() {
            //gets every object of Villas table and converts them to list
            var villas = _unitOfWork.Villa.GetAll();
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

                if(obj.Image is not null) {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"Images\VillaImage");

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\Images\VillaImage\" + fileName;
                    }

                else {
                    obj.ImageUrl = "https://placehold.co/600x400";
                }

                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Created succesfully!";
                return RedirectToAction("Index", "Villa");

            }
            return View(obj);

        }
            
       

        public IActionResult Edit(int? id) {
            Villa? objToUpdate = _unitOfWork.Villa.Get(u => u.Id == id);
            if (objToUpdate == null) {
                return RedirectToAction("Error", "Home");
            }
            
            return View(objToUpdate);
        }
        [HttpPost]
        public IActionResult Edit(Villa obj) {
            if (ModelState.IsValid && obj.Id>0) {

                if (obj.Image != null) {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"Images\VillaImage");

                    if (!string.IsNullOrEmpty(obj.ImageUrl)) {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath)){
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\Images\VillaImage\" + fileName;
                }

                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Edited succesfully!";
                return RedirectToAction("Index", "Villa");
            }
            return View();
        }

        public IActionResult Delete(int? id) {
            Villa? objToDelete = _unitOfWork.Villa.Get(u => u.Id == id);
            if (objToDelete is null) {
                return RedirectToAction("Error", "Home");
            }
            
            return View(objToDelete);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) {
            Villa? objToDelete = _unitOfWork.Villa.Get(u => u.Id == id);
            if (objToDelete is not null) {

                if (!string.IsNullOrEmpty(objToDelete.ImageUrl)) {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objToDelete.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath)) {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.Villa.Remove(objToDelete);
                _unitOfWork.Save();
                TempData["success"] = "Deleted succesfully!";
                return RedirectToAction("Index", "Villa");
            }
            return View();
        }
    }
}
