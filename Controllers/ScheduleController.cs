using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using time_trace.Data;
using time_trace.Models;

namespace time_trace.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ScheduleController> _logger;
        public ScheduleController(ApplicationDbContext context, ILogger<ScheduleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ScheduleController
        public async Task<ActionResult> Index()
        {
            var schedules = await _context.Schedules.ToListAsync();
            return View(schedules);
        }

        public async Task<ActionResult> Edit(int? id)
        {

            if (id == null) return RedirectToAction("Index");

            var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == id);
            
            return View(schedule);
        }

        // GET: ScheduleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScheduleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: ScheduleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ScheduleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ScheduleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
