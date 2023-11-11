using System;
using System.Collections.Generic;

namespace FAP.Models
{
    public partial class Slot
    {
        public Slot()
        {
            ClassSchedules = new HashSet<ClassSchedule>();
        }

        public int Id { get; set; }
        public TimeSpan? Start { get; set; }
        public TimeSpan? End { get; set; }

        public virtual ICollection<ClassSchedule> ClassSchedules { get; set; }
    }
}
