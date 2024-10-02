using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
            var ActiveUID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null || ActiveUID == null) return RedirectToAction(nameof(Index));

            var schedule = await _context.Schedules
                .Include(s => s.UserSchedules).ThenInclude(s => s.TimeSlots)
                .Include(s => s.UserSchedules).ThenInclude(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (schedule == null) return RedirectToAction(nameof(Index));

            if (schedule.UserSchedules.FirstOrDefault(u => u.User.Id == ActiveUID) == null)
            {
                var activeUser = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == ActiveUID);
                if (activeUser == null) RedirectToAction(nameof(Index));

                var userSchedule = new UserSchedule
                    {
                        Schedule = schedule,
                        User = activeUser,
                    };
                schedule.UserSchedules.Add(userSchedule);
                await _context.SaveChangesAsync();
            }

            return View(schedule);
        }

        // POST: ScheduleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, [FromBody] long[] serializedData)
        {
            _logger.LogInformation($"\n######\nBeginning Edit Post route for id {id}\n########");
            if (serializedData == null) _logger.LogInformation("null");
            else _logger.LogInformation($"time slots: [{String.Join(' ',serializedData)}");
            
            //change to error responses
            if (id == null) return RedirectToAction(nameof(Index));
            if (serializedData == null) return RedirectToAction(nameof(Index));

            
            var ActiveUID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ActiveUID == null) return RedirectToAction(nameof(Index));
            _logger.LogInformation($"got activeuserid from claims: {ActiveUID} for ");

            var userSchedule = await _context.UserSchedules
                .Include(u => u.TimeSlots)
                .FirstOrDefaultAsync(s => s.User.Id == ActiveUID && s.ScheduleId == id);
            
            if (userSchedule == null) return RedirectToAction(nameof(Index));
            //{
                //_logger.LogInformation($"failed to find UserSchedule with UserId: {ActiveUID} and ScheduleID: {id}");

                //var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == id);
                //var activeUser = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == ActiveUID);
                //_logger.LogInformation($"schedule: {schedule} activeUser: {activeUser}, activeusername: {ActiveUID}");   
                
                //if (schedule == null || activeUser == null) RedirectToAction(nameof(Index));
                //else
                //{
                //    userSchedule = new UserSchedule
                //    {
                //        Schedule = schedule,
                //        User = activeUser,
                //    };
                //    schedule.UserSchedules.Add(userSchedule);
                //}
            //}

            _logger.LogInformation($"got timeslots: {String.Join(" ",serializedData)}");

            userSchedule.TimeSlots = new List<TimeSlot>();
            foreach(var unixTimestamp in serializedData)
            {
                try
                {
                    userSchedule.TimeSlots.Add(new TimeSlot
                    {
                        UserSchedule = userSchedule,
                        DateTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTimestamp).UtcDateTime
                    });
                }
                catch (ArgumentOutOfRangeException e) {
                    _logger.LogError(e.Message);
                };
            }  

            _logger.LogInformation($"schedule slots:  \n{string.Join("\n", userSchedule.TimeSlots.Select(t => t.DateTime.ToString()))}");

            await _context.SaveChangesAsync();
            return Ok(serializedData);
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
