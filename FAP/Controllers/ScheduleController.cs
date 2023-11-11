using FAP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace FAP.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ProjectPRNContext _context;

        public ScheduleController(ProjectPRNContext context)
        {
            _context = context;
        }

        private DateTime GetStartDateOfWeek(int weekNumber, int year)
        {
            DateTime startDate = new DateTime(year, 1, 1);
            int daysToFirstDay = (int)startDate.DayOfWeek - (int)DayOfWeek.Monday;
            startDate = startDate.AddDays(-daysToFirstDay);

            int daysToAdd = (weekNumber - 1) * 7;
            DateTime startDateOfWeek = startDate.AddDays(daysToAdd);

            return startDateOfWeek;
        }

        private int getWeekNum(DateTime date)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

        public IActionResult Index(int week, int year)
        {
            if (HttpContext.Session.GetString("UserId") !=  null)
            {
                if (year == 0)
                    year = DateTime.Now.Year;

                if (week == 0)
                {
                    if (year == DateTime.Now.Year)
                        week = getWeekNum(DateTime.Now);
                    else
                        week = 1;
                }

                var slots = _context.Slots.ToList();
                ViewBag.slots = slots;

                DateTime startDate = GetStartDateOfWeek(week, year);

                ViewBag.weekNumber = week;
                ViewBag.year = year;
                ViewBag.startDate = startDate;

                List<ClassSchedule> classSchedule;

                string userCode = HttpContext.Session.GetString("UserId");

                if (HttpContext.Session.GetString("UserRole") == "Teacher")
                {
                    classSchedule = _context.ClassSchedules
                    .Where(cs => cs.Class.TeacherCode == userCode && cs.Date.Date >= startDate && cs.Date.Date <= startDate.AddDays(6))
                    .ToList();
                } else
                {
                    classSchedule = _context.ClassSchedules
                    .Where(cs => cs.ClassId == 1 && cs.Date.Date >= startDate && cs.Date.Date <= startDate.AddDays(6))
                    .ToList();
                }

                return View(classSchedule);
            }
            else
            {
                return Redirect("Home");
            }
        }
    }
}
