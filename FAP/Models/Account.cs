using System;
using System.Collections.Generic;

namespace FAP.Models
{
    public partial class Account
    {
        public Account()
        {
            Classes = new HashSet<Class>();
            Subjects = new HashSet<Subject>();
        }

        public string Code { get; set; } = null!;
        public string? Name { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;

        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
