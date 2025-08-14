using AgencijaZaNekretnine.Models.EFRepository;
using AgencijaZaNekretnine.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.RenderTree;
using GemBox.Document;
using GemBox.Pdf.Content;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;
using System.Text.RegularExpressions;
using AgencijaZaNekretnine.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.FileManager;
using SkiaSharp;

namespace AgencijaZaNekretnine.Controllers
{
    [Authorize(Roles = "Agent")]
    public class UgovorController : Controller
    {
        private UgovorRepository _ugovorRepository;
        private readonly UserManager<IdentityUser> _userManager;
        ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _HostEnvironment;
        public UgovorController(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext,
            RoleManager<IdentityRole> roleManager, IWebHostEnvironment hostEnvironment)
        {
            _ugovorRepository = new UgovorRepository();
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;
            _userManager = userManager;
            _context = dbContext;
            _roleManager = roleManager;
            _HostEnvironment = hostEnvironment;
        }
        public async Task<IActionResult> Index(string searchString, string currentFilter, int? page)
        {

            var pageNumber = page ?? 1;

            string ime = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(ime);
            var userId = user.Id;
            var lista = _ugovorRepository.GetAllByAgent(userId);

            if (searchString != null)
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
                lista = lista.Where(s => s.PunoIme.Contains(searchString) || s.Nekretnina.Adresa.Contains(searchString));
            }

            var list = lista.ToPagedList(pageNumber, 6);
            return View(list);
        }

        public async Task<ActionResult> CreateAsync()
        {
            string ime = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(ime);
            var userId = user.Id;
            ViewBag.Nekretnine = _ugovorRepository.GetAllNekretnine(userId);//.Where(t => t.Status.Naziv.Equals("Dostupno"));
            ViewBag.Placanje = _ugovorRepository.GetAllPlacanja();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(UgovorBO ugovor, int NekretninaId)
        {
            ugovor.Datum = DateTime.Now;


            ModelState.Remove("Nekretnina.Agent");
            ModelState.Remove("Nekretnina.Adresa");
            ModelState.Remove("Nekretnina.BrojSoba");
            ModelState.Remove("Nekretnina.Preduzimac");
            ModelState.Remove("Nekretnina.Tip");
            if (!ModelState.IsValid)
            {
                string ime = User.Identity.Name;
                var user = await _userManager.FindByNameAsync(ime);
                var userId = user.Id;
                ViewBag.Nekretnine = _ugovorRepository.GetAllNekretnine(userId);
                ViewBag.Placanje = _ugovorRepository.GetAllPlacanja();
                return View(ugovor);
            }
            else
            {
                if (ugovor.Pdf != null)
                {
                    string folder = "pdf/";
                    ugovor.PdfUrl = await UploadFile(folder, ugovor.Pdf);
                }
                _ugovorRepository.Add(ugovor);
                _ugovorRepository.UpdateNekretnina(ugovor.Nekretnina);
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        private async Task<string> UploadFile(string folderPath, IFormFile file)
        {
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;
            string serverFolder = Path.Combine(_HostEnvironment.WebRootPath, folderPath);
            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            return "/" + folderPath;
        }
        
        public async Task<IActionResult> NaknadniUnosPdfAsync(IFormFile Pdf,int ugovorId)
        {
            UgovorBO ugovor = _ugovorRepository.GetByID(ugovorId);
            if (Pdf != null)
            {
                if (ugovor.Pdf == null)
                {
                    string folder = "pdf/";
                    ugovor.PdfUrl = await UploadFile(folder, Pdf);
                    _ugovorRepository.Update(ugovor);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public FileStreamResult Download(UgovorBO ugovor)
        {
            
            string ime = ugovor.PunoIme; 
            string fon = ugovor.BrojTelefona;
            string jmbg = ugovor.Jmbg.ToString();
            string cena = ugovor.Iznos.ToString();

            var path = ("C:\\Users\\PC\\Desktop\\AgencijaZaNekretnine\\Pdf\\Ugovor.docx"); //C:\\Users\\PC\\Desktop\\Pdf\\Ugovor.docx
            var document = DocumentModel.Load(path);
            
            
                var nekretnina = _ugovorRepository.getNekretninaById(ugovor.Nekretnina.Id);
                string adresa = nekretnina.Adresa;
                string tip = nekretnina.Tip.Naziv;
                string kvadratura = nekretnina.Kvadratura.ToString();
                string datum = DateTime.Now.ToString("dd/MM/yyyy");
                document.Content.Replace("#Adresa", adresa);
                document.Content.Replace("#Datum", datum);
            
            
            document.Content.Replace("#PunoIme", ime);
            document.Content.Replace("#JMBG", jmbg);
            document.Content.Replace("#Cena", cena);
            document.Content.Replace("#Kvadratura", kvadratura);



            var stream = new MemoryStream();
            document.Save(stream, SaveOptions.PdfDefault);
            return File(stream, "application/pdf", ime + ".pdf");
        }


        [HttpPost]
        public IActionResult getNekretnina(int NekretninaID)
        {
            var result = _ugovorRepository.getNekretninaById(NekretninaID);
            var cena = result.Cena;

            return Json(cena);
        }

        [HttpPost]
        public ActionResult getPercent(int NekretninaID, int percent)
        {
            List<object> data = new List<object>();
            if (NekretninaID != 0)
            {
                var nekretnina = _ugovorRepository.getNekretninaById(NekretninaID);
                int cena = (int)nekretnina.Cena;
                int procenat = percent;
                
                if (percent > 0)
                {
                    double procenat1 = ((double)procenat / 100);
                    var procenat2 = 1 + procenat1;
                    var iznos = procenat2 * cena;
                    data.Add(iznos);
                    data.Add(NekretninaID);
                    return Json(data);
                }
                else
                {
                    data.Add(cena);
                    data.Add(NekretninaID);
                    return Json(data);
                }
            }
            else
            {
                return View();
            }
        }
        
    }
  
}
