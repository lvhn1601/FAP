using FAP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
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
                List<Class> listClass = new List<Class>();

                string userCode = HttpContext.Session.GetString("UserId");

                if (HttpContext.Session.GetString("UserRole") == "Teacher")
                {
                    classSchedule = _context.ClassSchedules
                    .Where(cs => cs.Class.TeacherCode == userCode && cs.Date.Date >= startDate && cs.Date.Date <= startDate.AddDays(6))
                    .ToList();

                    listClass = _context.Classes
                        .Where(c => c.TeacherCode == userCode)
                        .ToList();
                } else
                {
                    classSchedule = _context.ClassSchedules
                    .Where(cs => cs.ClassId == 1 && cs.Date.Date >= startDate && cs.Date.Date <= startDate.AddDays(6))
                    .ToList();
                }

                ViewBag.listClass = listClass;

                return View(classSchedule);
            }
            else
            {
                return Redirect("Home");
            }
        }

        public IActionResult ChangeSchedule()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangeSchedule(int currentClass, DateTime currentDate, int currentSlot, DateTime moveDate, int moveSlot)
        {
            Request req = new Request()
            {
                ClassId = currentClass,
                FromDate = currentDate,
                ToDate = moveDate,
                FromSlotId = currentSlot,
                ToSlotId = moveSlot,
                SendBy = HttpContext.Session.GetString("UserId")
            };

			_context.Requests.Add(req);
			_context.SaveChanges();

			return RedirectToAction("Index");
        }

        public IActionResult ListRequest()
        {
            if (HttpContext.Session.GetString("UserRole") == "Manager")
            {
                List<Request> listReq = _context.Requests.ToList();
                return View(listReq);
            }
            else
            {
                return Redirect("Home");
            }
        }

        public IActionResult updateStatus(int id, int status)
        {
			if (HttpContext.Session.GetString("UserRole") == "Manager")
			{
				Request req = _context.Requests.FirstOrDefault(r => r.Id == id);

                if (status == 1)
                {
					ClassSchedule schedule = _context.ClassSchedules
					.FirstOrDefault(cs => cs.ClassId == req.ClassId && cs.Date == req.FromDate && cs.SlotId == req.FromSlotId);

					schedule.Date = req.ToDate;
					schedule.SlotId = req.ToSlotId;

					_context.ClassSchedules.Update(schedule);
				}
                
				req.Status = status;

                _context.Requests.Update(req);
                _context.SaveChanges();

                return Redirect("ListRequest");
			}
			else
			{
				return Redirect("Home");
			}
		}
    }
}
