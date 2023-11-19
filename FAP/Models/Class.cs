using System;
using System.Collections.Generic;

namespace FAP.Models
{
    public partial class Class
    {
        public Class()
        {
            ClassSchedules = new HashSet<ClassSchedule>();
            Requests = new HashSet<Request>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? Status { get; set; }
        public string TeacherCode { get; set; } = null!;
        public string SubjectCode { get; set; } = null!;
        public int SemesterId { get; set; }

        public virtual Semester Semester { get; set; } = null!;
        public virtual Subject SubjectCodeNavigation { get; set; } = null!;
        public virtual Account TeacherCodeNavigation { get; set; } = null!;
        public virtual ICollection<ClassSchedule> ClassSchedules { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
