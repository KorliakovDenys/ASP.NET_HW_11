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
    public class RoomsController : Controller {
        private readonly DataContext _context;

        public RoomsController(DataContext context) {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index(string? sortParameter, string? searchString) {
            sortParameter ??= string.Empty;
            ViewData["NameSortParameter"] = sortParameter == "name_asc" ? "name_desc" : "name_asc";
            ViewData["DifficultySortParameter"] = sortParameter == "diff_asc" ? "diff_desc" : "diff_asc";
            ViewData["FareRateSortParameter"] = sortParameter == "fr_asc" ? "fr_desc" : "fr_asc";
            ViewData["CurrentFilter"] = searchString;
            
            var rooms = _context.Rooms!.Select(r => r);

            if (!string.IsNullOrEmpty(searchString)) rooms = rooms.Where(r => r.Name!.Contains(searchString));

            return sortParameter.ToLower() switch {
                "name_asc" => View(await rooms.OrderBy(r => r.Name).ToListAsync()),
                "name_desc" => View(await rooms.OrderByDescending(r => r.Name).ToListAsync()),
                "diff_asc" => View(await rooms.OrderBy(r => r.Difficulty).ToListAsync()),
                "diff_desc" => View(await rooms.OrderByDescending(r => r.Difficulty).ToListAsync()),
                "fr_asc" => View(await rooms.OrderBy(r => r.FareRate).ToListAsync()),
                "fr_desc" => View(await rooms.OrderByDescending(r => r.FareRate).ToListAsync()),
                _ => View(await rooms.ToListAsync())
            };
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null || _context.Rooms == null) {
                return NotFound();
            }

            var room = await _context.Rooms.Include(r => r.Sessions)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (room != null) {
                foreach (var session in room.Sessions) {
                    var schedulesForSession = _context.Schedules!
                        .Where(s => s.SessionId == session.Id)
                        .ToList();
                }
            }

            if (room == null) {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Difficulty,FareRate")] Room room) {
            if (ModelState.IsValid) {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null || _context.Rooms == null) {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null) {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Difficulty,FareRate")] Room room) {
            if (id != room.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!RoomExists(room.Id)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null || _context.Rooms == null) {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null) {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            if (_context.Rooms == null) {
                return Problem("Entity set 'DataContext.Rooms'  is null.");
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room != null) {
                _context.Rooms.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id) {
            return (_context.Rooms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}