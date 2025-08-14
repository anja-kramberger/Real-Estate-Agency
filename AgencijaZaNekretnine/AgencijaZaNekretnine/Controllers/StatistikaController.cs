using AgencijaZaNekretnine.Data;
using AgencijaZaNekretnine.Models.EFRepository;
using AgencijaZaNekretnine.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using System.Linq;
using System.Drawing;

namespace AgencijaZaNekretnine.Controllers
{
    [Authorize(Roles = "Menadzer")]
    public class StatistikaController : Controller
    {
        private AgencijaNekretninaContext agencijaContext = new AgencijaNekretninaContext();

        
        private NekretninaRepository _nekretninaRepository;
        private UgovorRepository _ugovorRepository;

        private readonly UserManager<IdentityUser> _userManager;
        ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public StatistikaController(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = dbContext;
            _roleManager = roleManager;
            _nekretninaRepository = new NekretninaRepository();
            _ugovorRepository = new UgovorRepository();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult StatistikaAgencije()
        {
            List<object> list = GetSalesData();
            List<float> lista = (List<float>)list[1];
            

            int prodateNekretnine = _nekretninaRepository.GetAll().Where(n => n.Status.Id == 2).Count();

            int nekretnine = _nekretninaRepository.GetAll().Count();

            var agenti = _userManager.GetUsersInRoleAsync("Agent").Result.Count();

            int prodato = _nekretninaRepository.GetAll().Where(n => n.Status.Naziv == "Nedostupno").Count();

            double procenat = (double)prodato / (double)nekretnine;

            //double suma = list.Sum(x => Convert.ToDouble(x));

            double zarada = lista.Sum();

            List<object> listaa = GetSalesDataa();
            List<float> listaNekretnina = (List<float>)listaa[1];
            float stan = listaNekretnina[0];
            float kuca = listaNekretnina[1];
            float plac = listaNekretnina[2];
            float posProstor = listaNekretnina[3];
            double procenatStan = (double)stan / (double)prodateNekretnine;
            double procenatKuca = (double)kuca / (double)prodateNekretnine;
            double procenatPlac = (double)plac / (double)prodateNekretnine;
            double procenatPProsto = (double)posProstor / (double)prodateNekretnine;

            ViewBag.Prodato = Math.Round(procenat*100);
            ViewBag.Agenti = agenti;
            ViewBag.Nekretnine = nekretnine;
            ViewBag.Total = zarada.ToString();
            ViewBag.Stan = Math.Round((procenatStan*100),1);
            ViewBag.Kuca = Math.Round((procenatKuca * 100), 1);
            ViewBag.Plac = Math.Round((procenatPlac * 100), 1);
            ViewBag.PProstor = Math.Round((procenatPProsto * 100), 1);
            return View();
        }
        [HttpPost]
        public List<object> GetSalesData()
        {
            List<object> data = new List<object>();

            List<string> labels = new List<string> {"Januar", "Februar", "Mart", "April", "Maj", "Jun",
                                                    "Jul", "Avgust", "Septembar", "Oktobar", "Novembar", "Decembar"};
            data.Add(labels);

            /*for(int i=0; i<labels.Count(); i++)
            {

                float labels[i] = _ugovorRepository.GetAll().Where(x => x.Datum.Month == i).Select(x => x.Iznos).Sum();
            }*/
            
                float januar = _ugovorRepository.GetAll().Where(x => x.Datum.Month == 1).Select(x => x.Iznos).Sum();
                float februar = _ugovorRepository.GetAll().Where(x => x.Datum.Month == 2).Select(x => x.Iznos).Sum();
                float mart = _ugovorRepository.GetAll().Where(x => x.Datum.Month == 3).Select(x => x.Iznos).Sum();
                float april = _ugovorRepository.GetAll().Where(x => x.Datum.Month == 4).Select(x => x.Iznos).Sum();
                float maj = _ugovorRepository.GetAll().Where(x => x.Datum.Month == 5).Select(x => x.Iznos).Sum();
                float jun = _ugovorRepository.GetAll().Where(x => x.Datum.Month == 6).Select(x => x.Iznos).Sum();
                float jul = _ugovorRepository.GetAll().Where(x => x.Datum.Month == 7).Select(x => x.Iznos).Sum();
                float avgust = _ugovorRepository.GetAll().Where(x => x.Datum.Month == 8).Select(x => x.Iznos).Sum();
                float septembar = _ugovorRepository.GetAll().Where(x => x.Datum.Month == 9).Select(x => x.Iznos).Sum();
                float oktobar = _ugovorRepository.GetAll().Where(x => x.Datum.Month == 10).Select(x => x.Iznos).Sum();
                float novembar = _ugovorRepository.GetAll().Where(x => x.Datum.Month == 11).Select(x => x.Iznos).Sum();
                float decembar = _ugovorRepository.GetAll().Where(x => x.Datum.Month == 12).Select(x => x.Iznos).Sum();

                List<float> SalesNumber = new List<float> {januar, februar, mart, april, maj, jun, jul, avgust, septembar, oktobar,
                                                    novembar, decembar};

            data.Add(SalesNumber);

            return data;
        }
        [HttpPost]
        public IActionResult AgentPoMesecima(string Id) //List<object>
        {
            List<object> data = new List<object>();

            List<string> labels = new List<string> {"Januar", "Februar", "Mart", "April", "Maj", "Jun",
                                                    "Jul", "Avgust", "Septembar", "Oktobar", "Novembar", "Decembar"};
            data.Add(labels);

            
            float januar = _ugovorRepository.GetAllByAgent(Id).Where(x => x.Datum.Month == 1).Select(x => x.Iznos).Sum();
            float februar = _ugovorRepository.GetAllByAgent(Id).Where(x => x.Datum.Month == 2).Select(x => x.Iznos).Sum();
            float mart = _ugovorRepository.GetAllByAgent(Id).Where(x => x.Datum.Month == 3).Select(x => x.Iznos).Sum();
            float april = _ugovorRepository.GetAllByAgent(Id).Where(x => x.Datum.Month == 4).Select(x => x.Iznos).Sum();
            float maj = _ugovorRepository.GetAllByAgent(Id).Where(x => x.Datum.Month == 5).Select(x => x.Iznos).Sum();
            float jun = _ugovorRepository.GetAllByAgent(Id).Where(x => x.Datum.Month == 6).Select(x => x.Iznos).Sum();
            float jul = _ugovorRepository.GetAllByAgent(Id).Where(x =>  x.Datum.Month == 7).Select(x => x.Iznos).Sum();
            float avgust = _ugovorRepository.GetAllByAgent(Id).Where(x => x.Datum.Month == 8).Select(x => x.Iznos).Sum();
            float septembar = _ugovorRepository.GetAllByAgent(Id).Where(x => x.Datum.Month == 9 ).Select(x => x.Iznos).Sum();
            float oktobar = _ugovorRepository.GetAllByAgent(Id).Where(x => x.Datum.Month == 10).Select(x => x.Iznos).Sum();
            float novembar = _ugovorRepository.GetAllByAgent(Id).Where(x => x.Datum.Month == 11).Select(x => x.Iznos).Sum();
            float decembar = _ugovorRepository.GetAllByAgent(Id).Where(x => x.Datum.Month == 12).Select(x => x.Iznos).Sum();

            List<float> SalesNumber = new List<float> {januar, februar, mart, april, maj, jun, jul, avgust, septembar, oktobar,
                                                novembar, decembar};
            
            data.Add(SalesNumber);

            var zarada = _ugovorRepository.GetAllByAgent(Id).Select(x => x.Iznos).Sum(); 
            var brNekretnina = _ugovorRepository.GetAllByAgent(Id).Count();


            data.Add(zarada);
            data.Add(brNekretnina);

            return Json(data);
        }

        public List<object> GetSalesDataa()
        {
            List<object> data = new List<object>();
            List<string> tipNekretnine = agencijaContext.Tips.Select(n => n.NazivTipa).ToList();
            data.Add(tipNekretnine);



            float kuca = _ugovorRepository.GetAll().Where(x => x.Nekretnina.Tip.Id == 2).Count();
            float stan = _ugovorRepository.GetAll().Where(x => x.Nekretnina.Tip.Id == 1).Count();
            float plac = _ugovorRepository.GetAll().Where(x => x.Nekretnina.Tip.Id == 3).Count();
            float poslovniProstor = _ugovorRepository.GetAll().Where(x => x.Nekretnina.Tip.Id == 4).Count();
            
            List<float> SalesNumber = new List<float> {stan, kuca, plac, poslovniProstor };
            data.Add(SalesNumber);

            return data;
        }

        public IActionResult StatistikaSvihAgenata()
        {
            var usersWithPermission = _userManager.GetUsersInRoleAsync("Agent").Result;
            var idsWithPermission = usersWithPermission.Select(s => s.Id);
            var useri = _context.UserBOs.Where(u => idsWithPermission.Contains(u.Id) && u.isApproved == true).ToList();
            
            
            
            
            
            return View(useri);
        }
        public List<object> ZaradaPoAgentu()
        {
            List<object> data=new List<object>();

            var usersWithPermission = _userManager.GetUsersInRoleAsync("Agent").Result;
            var idsWithPermission = usersWithPermission.Select(s => s.Id);
            var useri = _context.UserBOs.Where(u => idsWithPermission.Contains(u.Id) && u.isApproved == true).ToList();

            List<string> agenti = new List<string>();
            foreach(var a in useri)
            {
                string agent = a.FirstName + " " + a.LastName;
                agenti.Add(agent);
            }

            
            data.Add(agenti);

            List<float> ukupnaZarada = new List<float>();
            foreach(var a in useri)
            {
                ukupnaZarada.Add(_ugovorRepository.GetAll().Where(n=>n.Nekretnina.Agent.UserName == a.UserName).Select(x => x.Iznos).Sum());
            }

            data.Add(ukupnaZarada);

            Random r = new Random();
            
            List<string> colors = new List<string>();


            for (int i=0; i<useri.Count(); i++)
            {
                KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
                KnownColor randomColor = names[r.Next(names.Length)];
                string boja = randomColor.ToString();
                colors.Add(boja);
            }
            data.Add(colors);
            return data;
        }
        public List<object> NekretninePoAgentu()
        {
            List<object> data = new List<object>();

            var usersWithPermission = _userManager.GetUsersInRoleAsync("Agent").Result;
            var idsWithPermission = usersWithPermission.Select(s => s.Id);
            var useri = _context.UserBOs.Where(u => idsWithPermission.Contains(u.Id) && u.isApproved == true).ToList();

            List<string> agenti = new List<string>();
            foreach (var a in useri)
            {
                string agent = a.FirstName + " " + a.LastName;
                agenti.Add(agent);
            }

            data.Add(agenti);

            List<float> ukupnaZarada = new List<float>();
            foreach (var a in useri)
            {
                ukupnaZarada.Add(_nekretninaRepository.GetAll().Where(n=>n.Agent.UserName == a.UserName && n.Status.Id == 2).Count());
            }

            data.Add(ukupnaZarada);

            Random r = new Random();

            List<string> colors = new List<string>();

            for (int i = 0; i < useri.Count(); i++)
            {
                KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
                KnownColor randomColor = names[r.Next(names.Length)];
                string boja = randomColor.ToString();
                colors.Add(boja);
            }
            data.Add(colors);
            return data;
        }

    }
}
