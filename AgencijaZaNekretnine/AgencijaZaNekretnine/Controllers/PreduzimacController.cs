using AgencijaZaNekretnine.Models;
using AgencijaZaNekretnine.Models.EFRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace AgencijaZaNekretnine.Controllers
{
    [Authorize(Roles ="Admin")]
    public class PreduzimacController : Controller
    {
        public PreduzimacRepository preduzimacRepository;
        private readonly IConfiguration Configuration;
        private readonly AgencijaNekretninaContext _context;
        public PreduzimacController(IConfiguration configuration, AgencijaNekretninaContext context)
        {
            preduzimacRepository = new PreduzimacRepository();
            Configuration = configuration;
            _context = context;
        }

        
        public ViewResult Index(string searchString, string currentFilter, int? page)
        {
            var pageNumber = page ?? 1;
            var data = preduzimacRepository.GetAll();
            ViewBag.Preduzimaci = data;

            foreach(var item in data)
            {
                var countPred = data.GroupBy(u => u.IDPreduzimaca).Count();
            }
            


            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            

            if (!String.IsNullOrEmpty(searchString))
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    data = data.Where(s => s.Naziv.Contains(searchString));
                }
                
            }
            var lista = data.ToPagedList(pageNumber, 4);
            //return View(PaginatedList<Preduzimac>.Create(_context.Preduzimacs.ToList(), pageNumber ?? 1, pageSize));

            return View(lista);
        }

        public IActionResult Create()
        {

            return View();
        }


        [HttpPost]
        public IActionResult Create(PreduzimacBO preduzimacBO)
        {

            if (!ModelState.IsValid)
            {
                return View(preduzimacBO);
            }
            else
            {
                preduzimacRepository.Add(preduzimacBO);
                return RedirectToAction("Index");
            }
        }
        
        [HttpDelete]
        public JsonResult Delete(int Id)
        {
            PreduzimacBO preduzimacBO = preduzimacRepository.GetById(Id);
            var pom = preduzimacRepository.GetAllNekretnine(Id).Count();
            if (pom == 0)
            {
                preduzimacRepository.Delete(preduzimacBO);
            }
            else
            {
                return Json(new { success = false, message = "Nije moguće obrisati, morate obrisati nekretnine sa datim preduzimačem!" });
            }
            return Json(new { success = true, message = "Preduzimač je obrisan." });
            //return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            PreduzimacBO preduzimacBO = preduzimacRepository.GetById(id);
            return View(preduzimacBO);
        }

        [HttpPost]
        public IActionResult Edit(PreduzimacBO preduzimacBO)
        {
            if (!ModelState.IsValid)
            {
                return View(preduzimacBO);
            }
            else
            {
                preduzimacRepository.Update(preduzimacBO); ;
                return RedirectToAction("Index");
            }
            
            
        }
    }
}
