using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApp.Data;
using TestApp.Models;

namespace TestApp.Controllers
{
    public class Main : Controller
    {
        private readonly ApplicationDbContext _context;

        public Main(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        // GET: Main/Search
        public async Task <IActionResult> Search(SearchViewModel model)
        {
            var results = _context.Flights.Where(flight =>
                 flight.departure_point == model.DeparturePoint&&
                 flight.destination == model.Destination);
            return View(results);
        }

        public async Task<IActionResult> СonfirmationUserDataView(int? ID)
        {
            if (ID == null || _context.Flights == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.PublicID == ID);
            if (flight == null)
            {
                return NotFound();
            }
            Ticket ticket = new Ticket();
            ticket.Flight = flight;
            
            return View(ticket);
        }

        public ActionResult ConfirmTicket(Ticket model)
        {
            return View(model);
        }
    }
}
