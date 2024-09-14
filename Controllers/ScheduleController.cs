using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using time_trace.Data;
using time_trace.Models;

namespace time_trace.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ScheduleController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public ScheduleController(ApplicationDbContext context, ILogger<ScheduleController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: ScheduleController
        public async Task<ActionResult> Index()
        {
            var schedules = await _context.Schedules.ToListAsync();
            return View(schedules);
        }

        public async Task<ActionResult> Edit(int? id)
        {

            if (id == null) return RedirectToAction(nameof(Index));

            var schedule = await _context.Schedules
                .Include(s => s.Users)
                .Include(s => s.UserSchedules).ThenInclude(s => s.TimeSlots)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (schedule == null) return RedirectToAction(nameof(Index));

            return View(schedule);
        }

        // POST: ScheduleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, List<TimeSlot> TimeSlots)
        {
            _logger.LogInformation("\n######\nBeginning Edit Post route\n########");
            if (id == null) return RedirectToAction(nameof(Index));

            var activeUserName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (activeUserName == null) throw new Exception("No User logged in");
            _logger.LogInformation($"got activeuserid from claims: {activeUserName}");

            var UserSchedule = await _context.UserSchedules
                .Include(u => u.TimeSlots)
                .FirstOrDefaultAsync(s => s.User.UserName == activeUserName && s.ScheduleId == id);
            if (UserSchedule == null)
            {
                _logger.LogInformation($"failed to find UserSchedule with UserId: {activeUserName} and ScheduleID: {id}");

                var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == id);
                var activeUser = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == activeUserName);
                _logger.LogInformation($"schedule: {schedule} activeUser: {activeUser}, activeusername: {activeUserName}");
                if (schedule == null || activeUser == null) throw new Exception("Could not Retrieve schedule and Argument");

                schedule.UserSchedules.Add(new UserSchedule
                {
                    Schedule = schedule,
                    User = activeUser,
                });
            }
            //UserSchedule.TimeSlots = TimeSlots;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Edit),id);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
