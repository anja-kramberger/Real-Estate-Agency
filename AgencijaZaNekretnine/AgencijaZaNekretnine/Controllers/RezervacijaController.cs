using AgencijaZaNekretnine.Models.EFRepository;
using AgencijaZaNekretnine.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AgencijaZaNekretnine.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace AgencijaZaNekretnine.Controllers
{
    [Authorize(Roles = "Agent")]
    [Route("api/[controller]")]
    [ApiController]
    public class RezervacijaController : Controller
    {        
        AgencijaNekretninaContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public RezervacijaController(AgencijaNekretninaContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Route("/Rezervacija")]
        public IActionResult Index() //string fileName
        {
            return View();
        }
       
        public IActionResult GetUrl()
        {
            ViewBag.Dan = DateTime.Now.Day.ToString();
            ViewBag.Mesec = DateTime.Now.Month.ToString();
            ViewBag.Godina = DateTime.Now.Year.ToString();
            //return Redirect("~/Views/Rezervacija/Rezervacija.html");
            return RedirectToPage("~/wwwroot/index.html");
        }

        [HttpGet]
        public async Task<IEnumerable<WebApiEvent>> GetAsync()  
        {
            var user = await _userManager.GetUserAsync(User);
            var id = user.Id;
            var pom = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            return _context.Rezervacijas
                .ToList()
                .Select(e => (WebApiEvent)e)
                .Where(e => e.agent_id == id);
        }

        [HttpGet("{id}")]
        public Rezervacija? Get(int id)
        {
            return _context
                .Rezervacijas
                .Find(id);
        }

        [HttpPost]
        public async Task<ObjectResult> PostAsync([FromForm] WebApiEvent apiEvent)
        {
            var newEvent = (Rezervacija)apiEvent;


            var user = await _userManager.GetUserAsync(User);
            var id = user.Id;

            newEvent.IdAgenta = id;

            _context.Rezervacijas.Add(newEvent);
            _context.SaveChanges();

            return Ok(new
            {
                tid = newEvent.Id,
                action = "inserted"
            });
        }

        [HttpPut("{id}")]
        public ObjectResult? Put(int id, [FromForm] WebApiEvent apiEvent)
        {
            var updatedEvent = (Rezervacija)apiEvent;
            var dbEvent = _context.Rezervacijas.Find(id);
            if (dbEvent == null)
            {
                return null;
            }
            dbEvent.Opis = updatedEvent.Opis;
            dbEvent.DatumOd = updatedEvent.DatumOd;
            dbEvent.DatumDo = updatedEvent.DatumDo;

            _context.SaveChanges();

            return Ok(new
            {
                action = "updated"
            });
        }

        [HttpDelete("{id}")]
        public ObjectResult DeleteEvent(int id)
        {
            var e = _context.Rezervacijas.Find(id);
            if (e != null)
            {
                _context.Rezervacijas.Remove(e);
                _context.SaveChanges();
            }

            return Ok(new
            {
                action = "deleted"
            });
        }
    }
}
