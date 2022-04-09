namespace EmployeePartV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("generalSettingOfficialHoliday")]
    public partial class generalSettingOfficialHoliday
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int gsID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int officialHolidayID { get; set; }

        [StringLength(1)]
        public string notes { get; set; }

        public virtual GeneralSetting GeneralSetting { get; set; }

        public virtual officialHoliday officialHoliday { get; set; }
    }
}
