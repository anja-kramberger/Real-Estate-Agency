using AgencijaZaNekretnine.Data;
using AgencijaZaNekretnine.Models;
using AgencijaZaNekretnine.Models.EFRepository;
using AgencijaZaNekretnine.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using System.Linq;
using X.PagedList;

namespace AgencijaZaNekretnine.Controllers
{
    [Authorize(Roles = "Agent,Menadzer")]
    public class NekretninaController : Controller
    {
        private AgencijaNekretninaContext agencijaContext = new AgencijaNekretninaContext();

        private readonly UserManager<IdentityUser> _userManager;
        ApplicationDbContext _context;
        private NekretninaRepository _nekretninaRepository;
        private UgovorRepository _ugovorRepository;

        private readonly IWebHostEnvironment _HostEnvironment;
        public NekretninaController(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext,
            IWebHostEnvironment hostEnvironment)
        {
            _nekretninaRepository = new NekretninaRepository();
            _userManager = userManager;
            _context = dbContext;
            _ugovorRepository = new UgovorRepository();
            _HostEnvironment = hostEnvironment;
        }

        public IActionResult Index(string searchString, string currentFilter, int? Tipovi, int? Statusi, int currentTip, int currentStatus,
            string cenaOd, string cenaDo, string currentCenaOd, string currentCenaDo, int? page)
        {
            var pageNumber = page ?? 1;
            ViewBag.Tipovi = _nekretninaRepository.GetAllTipovi();
            ViewBag.Statusi = _nekretninaRepository.GetAllStatusi();

            var lista = _nekretninaRepository.GetAll();
            string id = _userManager.GetUserId(User);
            string ime = User.Identity.Name;

            int? tips = Tipovi;
            int? statuses = Statusi;

            //if (searchString != null || cenaOd > 0 || (cenaDo > 0 && cenaDo > cenaOd))
            if (searchString != null || cenaOd != null || cenaDo != null || Tipovi != null || Statusi != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
                cenaOd = currentCenaOd;
                cenaDo = currentCenaDo;
                tips = currentTip; 
                statuses = currentStatus;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.CenaOd = cenaOd;
            ViewBag.CenaDo = cenaDo;
            ViewBag.CurrentTip = tips;
            ViewBag.CurrentStatus = statuses;

            if (!String.IsNullOrEmpty(searchString) || !String.IsNullOrEmpty(cenaOd) || !String.IsNullOrEmpty(cenaDo)
                || (tips != 0 && tips != null) || (statuses != 0 && statuses != null) ) // !String.IsNullOrEmpty(tips.ToString())  !String.IsNullOrEmpty(statuses.ToString()
            {
                if(!String.IsNullOrEmpty(cenaOd))
                {
                    int intCenaOd = Int32.Parse(cenaOd);
                    lista = lista.Where(s => s.Cena >= intCenaOd).ToList();
                }
                if (!String.IsNullOrEmpty(cenaDo))
                {
                    int intCenaDo = Int32.Parse(cenaDo);
                    lista = lista.Where(s => s.Cena <= intCenaDo).ToList();
                }
                if (tips != 0 && tips != null)
                {
                    lista = lista.Where(s => s.Tip.Id == tips);
                }
                if (statuses != 0 && statuses != null)
                {
                    lista = lista.Where(s => s.Status.Id == statuses);
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    lista = lista.Where(s => s.Adresa.Contains(searchString)).ToList();

                }

            }

            if (User.IsInRole("Agent"))
            {
                lista = lista.Where(n => n.Agent.UserName.Equals(ime));
            }
            var listaN = lista.ToPagedList(pageNumber, 5);

            return View(listaN);
        }


        public IActionResult Create()
        {
            
            var agents = _userManager.GetUsersInRoleAsync("Agent").Result;
            var idsWithPermission = agents.Select(s => s.Id);
            var users = _context.UserBOs.Where(u => idsWithPermission.Contains(u.Id) && u.isApproved == true).ToList();

            ViewBag.Tipovi = _nekretninaRepository.GetAllTipovi();
            ViewBag.Statusi = _nekretninaRepository.GetAllStatusi();
            ViewBag.Agenti = users;
            ViewBag.Preduzimaci = _nekretninaRepository.GetAllPreduzimaci();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NekretninaBO nekretnina)    
        {
            if (nekretnina.Agent.Id == "Odaberi Agenta")
            {

            }
            ModelState.Remove("Preduzimac.Naziv");
            ModelState.Remove("PreduzimacBO.Naziv");
            ModelState.Remove("Agent.Discriminator");
            

            if (!ModelState.IsValid)
            {
                ViewBag.Tipovi = _nekretninaRepository.GetAllTipovi();
                ViewBag.Statusi = _nekretninaRepository.GetAllStatusi();
                var agents = _userManager.GetUsersInRoleAsync("Agent").Result;
                var idsWithPermission = agents.Select(s => s.Id);
                var users = _context.UserBOs.Where(u => idsWithPermission.Contains(u.Id) && u.isApproved == true).ToList();
                ViewBag.Agenti = users;
                ViewBag.Preduzimaci = _nekretninaRepository.GetAllPreduzimaci();

                return View(nekretnina);
            }
            else
            {
                if (nekretnina.File != null)
                {
                    string path = UploadFiles(nekretnina.File);
                    nekretnina.Slika = path;
                }
                _nekretninaRepository.Add(nekretnina);


                int NekrID = _nekretninaRepository.GetIdByAddress(nekretnina.Adresa);
                int PomId = _nekretninaRepository.GetAll().Last().Id;
                int idnekr = nekretnina.Id;
                if (nekretnina.Images != null)
                {
                    foreach (var image in nekretnina.Images)
                    {
                        string tempFileName = image.FileName;
                        string stringFileName = UploadFiles(image);



                        SlikeNekretnineBO sliki = new SlikeNekretnineBO();
                        sliki.UrlSlike = stringFileName;
                        sliki.Nekretnina = new NekretninaBO()
                        {
                            Id = PomId,
                            Adresa = nekretnina.Adresa,

                        };
                        _nekretninaRepository.AddSlike(sliki);
                    }
                }
                return RedirectToAction("Index");
            }

        }

        private string UploadFiles(IFormFile image)
        {
            string fileName = null;
            if (image != null)
            {
                string uploadDirLocation = Path.Combine(_HostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadDirLocation, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            return fileName;
        }

        public IActionResult Edit(int id)
        {

            NekretninaBO nekretnina = _nekretninaRepository.GetById(id);
            ViewBag.Tipovi = _nekretninaRepository.GetAllTipovi();
            ViewBag.Statusi = _nekretninaRepository.GetAllStatusi();
            ViewBag.Agenti = _userManager.GetUsersInRoleAsync("Agent").Result;
            ViewBag.Preduzimaci = _nekretninaRepository.GetAllPreduzimaci();
            nekretnina.ImgUrls = (ICollection<SlikeNekretnineBO>?)_nekretninaRepository.GetAllSlike(id);
            return View(nekretnina);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit(NekretninaBO nekretnina)
        {
            string pomSlika = _nekretninaRepository.VratiSliku(nekretnina);

            ModelState.Remove("Preduzimac.Naziv");
            ModelState.Remove("PreduzimacBO.Naziv");
            ModelState.Remove("Agent.Discriminator");
            if (!ModelState.IsValid)
            {
                ViewBag.Tipovi = _nekretninaRepository.GetAllTipovi();
                ViewBag.Statusi = _nekretninaRepository.GetAllStatusi();
                ViewBag.Agenti = _userManager.GetUsersInRoleAsync("Agent").Result;
                ViewBag.Preduzimaci = _nekretninaRepository.GetAllPreduzimaci();
                nekretnina.ImgUrls = (ICollection<SlikeNekretnineBO>?)_nekretninaRepository.GetAllSlike(nekretnina.Id);
                ViewBag.Slike = _nekretninaRepository.GetAllSlike(nekretnina.Id);


                return View(nekretnina);
            }
            else
            {
                if (nekretnina.File != null)
                {
                    string imageUrl = "Images\\" + pomSlika;
                    var toDeleteImageFromFolder = Path.Combine(_HostEnvironment.WebRootPath, imageUrl.TrimStart('\\'));
                    DeleteAImage(toDeleteImageFromFolder);
                    string pathh = UploadFiles(nekretnina.File);
                    nekretnina.Slika = pathh;

                }
                else
                {
                    nekretnina.Slika = pomSlika;
                }


                _nekretninaRepository.Update(nekretnina);

                int NekrID = _nekretninaRepository.GetIdByAddress(nekretnina.Adresa);
                int idnekr = nekretnina.Id;
                if (nekretnina.Images != null)
                {
                    foreach (var image in nekretnina.Images)
                    {
                        string tempFileName = image.FileName;
                        string stringFileName = UploadFiles(image);



                        SlikeNekretnineBO sliki = new SlikeNekretnineBO();
                        sliki.UrlSlike = stringFileName;
                        sliki.Nekretnina = new NekretninaBO()
                        {
                            Id = idnekr,
                            Adresa = nekretnina.Adresa,

                        };

                        _nekretninaRepository.AddSlike(sliki);


                    }
                }
                return RedirectToAction("Index");
            }

        }
        [HttpDelete]
        public JsonResult Delete(int Id)
        {
            NekretninaBO nekretninaBo = _nekretninaRepository.GetById(Id);
            string url = nekretninaBo.Slika;
            string pomSlika = _nekretninaRepository.VratiSliku(nekretninaBo);
            int idnekr = nekretninaBo.Id;
            
            if(Id != null)
            {
                if (_nekretninaRepository.GetAllSlike(idnekr) != null)
                {
                    foreach (SlikeNekretnineBO slika in _nekretninaRepository.GetAllSlike(idnekr))
                    {
                        string imageUrl = "Images\\" + slika.UrlSlike;
                        var toDeleteImageFromFolder = Path.Combine(_HostEnvironment.WebRootPath, imageUrl.TrimStart('\\'));
                        //_nekretninaRepository.DeleteImage(nekretninaBo.Id);
                        _nekretninaRepository.DeleteImage(slika.Id);
                        DeleteAImage(toDeleteImageFromFolder);
                        
                    }
                }
                if (pomSlika != null)
                {
                    string imageUrl = "Images\\" + pomSlika;
                    var toDeleteImageFromFolder = Path.Combine(_HostEnvironment.WebRootPath, imageUrl.TrimStart('\\'));
                    DeleteAImage(toDeleteImageFromFolder);

                }
                _nekretninaRepository.Delete(nekretninaBo);
            }
            
            else
            {
                return Json(new { success = false, message = "Došlo je do greške" });
            }
            return Json(new { success = true, message = "Nekretnina je obrisana." });
            //return RedirectToAction("Index");
        }

        //[HttpDelete]
        public IActionResult DeleteHomeImg(int Id)
        {
            Nekretnina nekretnina = agencijaContext.Nekretninas.FirstOrDefault(n => n.Id == Id);
            int id = nekretnina.Id;
            if (Id != null)
            {
                
                string pomSlika = nekretnina.Slika;
                string imageUrl = "Images\\" + pomSlika;
                var toDeleteImageFromFolder = Path.Combine(_HostEnvironment.WebRootPath, imageUrl.TrimStart('\\'));
                DeleteAImage(toDeleteImageFromFolder);
                nekretnina.Slika = null;
                agencijaContext.SaveChanges();
            }
            return RedirectToAction("Edit", new { id = id, refresh = 1 });


        }

        //[HttpDelete]
        public IActionResult DeleteImgs(int Id)
        {
            SlikeNekretnine slikeNekr = agencijaContext.SlikeNekretnines.FirstOrDefault(s => s.Id == Id);
            int id = slikeNekr.IdNekretnine;
            if (Id != null)
            {
                
                string imageUrl = "Images\\" + slikeNekr.UrlSlike;
                var toDeleteImageFromFolder = Path.Combine(_HostEnvironment.WebRootPath, imageUrl.TrimStart('\\'));
                DeleteAImage(toDeleteImageFromFolder);
                int slikaId = slikeNekr.Id;
                _nekretninaRepository.DeleteImage(slikaId);

            }
            
           
            return RedirectToAction("Edit", new { id = id});


        }
        private void DeleteAImage(string toDeleteImageFromFOlder)
        {
            if(System.IO.File.Exists(toDeleteImageFromFOlder))
            {
                System.IO.File.Delete(toDeleteImageFromFOlder);
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            NekretninaBO nekretnina = _nekretninaRepository.GetById(id);
            ViewBag.Tipovi = _nekretninaRepository.GetAllTipovi();
            ViewBag.Statusi = _nekretninaRepository.GetAllStatusi();
            ViewBag.Agenti = _userManager.GetUsersInRoleAsync("Agent").Result;
            ViewBag.Preduzimaci = _nekretninaRepository.GetAllPreduzimaci();
            nekretnina.ImgUrls = (ICollection<SlikeNekretnineBO>?)_nekretninaRepository.GetAllSlike(id);
            if (nekretnina.ImgUrls.Count() > 0)
            {
                ViewBag.PrvaSlika = nekretnina.ImgUrls.First().UrlSlike;
            }
            return View(nekretnina);
        }

    }
}
