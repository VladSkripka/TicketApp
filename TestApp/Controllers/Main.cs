using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
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
                 flight.destination == model.Destination &&
                 flight.sites_left>0);

            foreach (var flight in results)
            {
                var soldTicketsCount = await _context.Tickets.CountAsync(t => t.Flight.PublicID == flight.PublicID);
                flight.sites_left = flight.sites - soldTicketsCount;
                var sqlUpdate = "UPDATE Flights " +
                $"SET sites_left = {flight.sites_left} " +
                $"WHERE PublicID = '{flight.PublicID}';";
                _context.Database.ExecuteSqlRaw(sqlUpdate);
            }
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
