using Microsoft.AspNetCore.Mvc;
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
    }
}
