using System;
using System.Collections.Generic;

namespace FAP.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Classes = new HashSet<Class>();
        }

        public string Code { get; set; } = null!;
        public string? Name { get; set; }
        public bool? Status { get; set; }
        public string ManagerCode { get; set; } = null!;

        public virtual Account ManagerCodeNavigation { get; set; } = null!;
        public virtual ICollection<Class> Classes { get; set; }
    }
}
