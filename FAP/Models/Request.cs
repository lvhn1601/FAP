using System;
using System.Collections.Generic;

namespace FAP.Models
{
    public partial class Request
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int FromSlotId { get; set; }
        public int ToSlotId { get; set; }
        public string? SendBy { get; set; }
        public int? Status { get; set; }

        public virtual Class? Class { get; set; }
    }
}
