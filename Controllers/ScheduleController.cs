using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP_NET_HW_11.Data;
using ASP_NET_HW_11.Models.Database;

namespace ASP_NET_HW_11.Controllers {
    public class ScheduleController : Controller {
        private readonly DataContext _context;

        public ScheduleController(DataContext context) {
            _context = context;
        }

        // GET: Schedule
        public async Task<IActionResult> Index() {
            var dataContext = _context.Schedules!.Include(s => s.Customer).Include(s => s.Session);
            return View(await dataContext.ToListAsync());
        }

        // GET: Schedule/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null || _context.Schedules == null) {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Customer)
                .Include(s => s.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null) {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedule/Create
        public async Task<IActionResult> Create(int id) {
            var room = await _context.Rooms!.FirstOrDefaultAsync(r =>
                r.Id == _context.Sessions!.FirstOrDefault(s => s.Id == id)!.RoomId);
            if (room is null) return RedirectToAction("Index", "Rooms");
            
            ViewData["RoomName"] = room.Name;
            ViewData["SessionId"] = new SelectList(_context.Sessions!.Where(s => s.RoomId == room.Id).ToList(), "Id",
                "StartDateTime", id);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            return View();
        }

        // POST: Schedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SessionId, CustomerId")] Schedule schedule) {
            if (ModelState.IsValid) {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Rooms");
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", schedule.CustomerId);
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "StartDateTime", schedule.SessionId);
            return View(schedule);
        }

        // GET: Schedule/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null || _context.Schedules == null) {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null) {
                return NotFound();
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", schedule.CustomerId);
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", schedule.SessionId);
            return View(schedule);
        }

        // POST: Schedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SessionId,CustomerId")] Schedule schedule) {
            if (id != schedule.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!ScheduleExists(schedule.Id)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", schedule.CustomerId);
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", schedule.SessionId);
            return View(schedule);
        }

        // GET: Schedule/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null || _context.Schedules == null) {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Customer)
                .Include(s => s.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null) {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            if (_context.Schedules == null) {
                return Problem("Entity set 'DataContext.Schedules'  is null.");
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null) {
                _context.Schedules.Remove(schedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id) {
            return (_context.Schedules?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}