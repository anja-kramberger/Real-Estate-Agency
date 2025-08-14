using AgencijaZaNekretnine.Data;
using AgencijaZaNekretnine.Models;
using AgencijaZaNekretnine.Models.EFRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;
using System.Security.Claims;

namespace AgencijaZaNekretnine.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        ApplicationDbContext _context;
        public NekretninaRepository _nekretninaRepository;
        public UgovorRepository _ugovorRepository;
        public PreduzimacRepository _preduzimacRepository;

        public DashboardController( ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {

            _context = context;
            _userManager = userManager;
            _preduzimacRepository = new PreduzimacRepository();
            _nekretninaRepository = new NekretninaRepository(); ;
            _ugovorRepository = new UgovorRepository(); ;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ulogovan = await _userManager.GetUserAsync(HttpContext.User);
            UserBO user = _context.UserBOs.FirstOrDefault(u => u.Id == ulogovan.Id);
            string ime = user.FirstName + " " + user.LastName;
            ViewBag.Ime = ime;
            ViewBag.Confirm = _context.UserBOs.Where(u => u.isApproved == false).Count();

            IEnumerable<UserBO> zaposleni = _context.UserBOs.Where(u => u.isApproved == true).ToList();

            ViewBag.Zaposleni =  _context.UserBOs.Where(u => u.isApproved == true).ToList().Count();
            ViewBag.Cekanje = _context.UserBOs.Where(u => u.isApproved == false).ToList().Count();

            NekretninaBO nekretnina = new NekretninaBO();
            UgovorBO ugovor = new UgovorBO();
            UserBO korisnik = new UserBO();
            var agenti = _userManager.GetUsersInRoleAsync("Agent").Result;
            var idsAgent = agenti.Select(s => s.Id);
            ViewBag.Agenti = agenti.Count();
            var countUgovora = 0;
            
            var id = "";
            /*for (int i = 0; i < agenti.Count(); i++)
            {
                string IdPrvi = agenti[i].Id;
                for (int j = i+1; j<agenti.Count(); j++)
                {
                    
                    string IdDrugi = agenti[j].Id;
                    if (_ugovorRepository.GetAll().Where(u => u.Nekretnina.Agent.Id == IdPrvi).Count() > _ugovorRepository.GetAll().Where(u => u.Nekretnina.Agent.Id == IdDrugi).Count())
                    {
                        id = agenti[i].Id;
                    }
                    else
                    {
                        id = agenti[j].Id;
                    }
                }
                
            }*/
            int maximum = 0;
            for (int i = 0; i < agenti.Count(); i++)
            {
                var agent = _ugovorRepository.GetAll().Where(u => u.Nekretnina.Agent.Id == agenti[i].Id).Count();
                if( agent > maximum)
                {
                    maximum = agent;
                    id = agenti[i].Id;
                }
            }

            var najagent = _context.UserBOs.FirstOrDefault(a => a.Id == id);
            ViewBag.NajAgent = najagent.FirstName +" "+ najagent.LastName;

            ViewBag.Nekretnine = _nekretninaRepository.GetAll().Count();
            ViewBag.NekretnineProdate = _nekretninaRepository.GetAll().Where(n => n.Status.Naziv == "Nedostupno").Count();
            ViewBag.NekretninaAgent = _nekretninaRepository.GetAll().Where(n => n.Agent.Id == userId).Count();

            ViewBag.Preduzimaci = _preduzimacRepository.GetAll().Count(); 
            ViewBag.PreduzimaciNedovrseni = _preduzimacRepository.GetAll().Where(p => p.Count < p.BrojNekretnina).Count();

            ViewBag.Ugovor = _ugovorRepository.GetAll().Count();
            ViewBag.UgovoriNedovrseni = _ugovorRepository.GetAll().Where(u => u.Nekretnina.Agent.Id == userId && u.PdfUrl == null).Count();

            return View();
        }

        public IActionResult AccessDeniedPage()
        {
            return View();
        }
    }
}
