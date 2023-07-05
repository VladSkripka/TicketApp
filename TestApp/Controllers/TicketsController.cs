using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using TestApp.Data;
using TestApp.Models;
using ceTe.DynamicPDF.PageElements.BarCoding;
using System.Net;
using Microsoft.Extensions.Hosting.Internal;
using Path = System.IO.Path;

namespace TestApp.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
              return _context.Tickets != null ? 
                          View(await _context.Tickets.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Tickets'  is null.");
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(m => m.ID == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Public_ID,passenger_first,passenger_last")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }
        public IActionResult CreateTicket(Ticket ticket)
        {
            var sql = "INSERT INTO Tickets(Public_ID, passenger_first, passenger_last, FlightPublicID)" +
                " VALUES (@Public_ID, @passenger_first, @passenger_last, @FlightPublicID)";
            var time = DateTime.Now;
            var id = ticket.Public_ID+ticket.Flight.FID+ticket.Flight.sites_left+time.Hour+time.Minute;
            ticket.Public_ID = id;

            _context.Database.ExecuteSqlRaw(sql,
                new SqlParameter("@Public_ID", id),
                new SqlParameter("@passenger_first", ticket.passenger_first),
                new SqlParameter("@passenger_last", ticket.passenger_last),
                new SqlParameter("@FlightPublicID", ticket.Flight.PublicID)
            );
            GeneratePDF(ticket);
            return View(ticket);
        }

        public void GeneratePDF(Ticket ticket)
        {
            Document document = new Document();
            Page page = new Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);
            document.Pages.Add(page);
            Codabar barcode = new Codabar(ticket.Public_ID.ToString(), 50, 0, 48);
            Label text = new Label($"{ticket.Public_ID} \n {ticket.passenger_first} {ticket.passenger_last}", 0, 0, 504, 100, Font.Helvetica, 18, TextAlign.Center);
            page.Elements.Add(barcode);
            page.Elements.Add(text);
            document.Draw($"E:/Влад/Третий курс/Практика/TestApp/TestApp/wwwroot/{ticket.Public_ID}.pdf");
           
        }
        public FileResult DownloadDoc(int id)
        {
            return File($"{id}.pdf", "application/pdf", $"{id}.pdf");
        }
        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Public_ID,passenger_first,passenger_last")] Ticket ticket)
        {
            if (id != ticket.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(m => m.ID == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tickets'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
          return (_context.Tickets?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
