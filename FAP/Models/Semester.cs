using System;
using System.Collections.Generic;

namespace FAP.Models
{
    public partial class Semester
    {
        public Semester()
        {
            Classes = new HashSet<Class>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
    }
}
