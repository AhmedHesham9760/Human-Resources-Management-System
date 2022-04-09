namespace EmployeePartV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Statue")]
    public partial class Statue
    {
        [Key]
        public int StatusID { get; set; }

        public TimeSpan? ExtraTime { get; set; }

        public TimeSpan? LateTime { get; set; }

        public byte? AttendanceDays { get; set; }

        public byte? BonusDays { get; set; }

        [Column(TypeName = "date")]
        public DateTime month { get; set; }

        public int empID { get; set; }

        public byte? WeekEndHolidaysNumber { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
