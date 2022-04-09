namespace EmployeePartV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Attendance")]
    public partial class Attendance
    {
        public int AttendanceID { get; set; }

        [Column(TypeName = "date")]
        public DateTime AttendanceDate { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int empID { get; set; }

        public bool? registerStart { get; set; }

        public bool? registerLeave { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
