using System;
using System.Collections.Generic;

namespace FAP.Models
{
    public partial class ClassSchedule
    {
        public int ClassId { get; set; }
        public DateTime Date { get; set; }
        public int SlotId { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual Slot Slot { get; set; } = null!;
    }
}
