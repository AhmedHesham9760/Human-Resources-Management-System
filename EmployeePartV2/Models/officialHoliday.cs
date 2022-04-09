namespace EmployeePartV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class officialHoliday
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public officialHoliday()
        {
            generalSettingOfficialHolidays = new HashSet<generalSettingOfficialHoliday>();
        }

        [Required]
        [StringLength(50)]
        public string officialHolidayName { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime startDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}" , ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        public DateTime endDate { get; set; }

        public int officialHolidayID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<generalSettingOfficialHoliday> generalSettingOfficialHolidays { get; set; }
    }
}
