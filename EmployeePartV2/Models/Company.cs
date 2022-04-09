namespace EmployeePartV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Company")]
    public partial class Company
    {
        public int CompanyID { get; set; }

        [Required]
        [StringLength(50)]
        public string CompanyName { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
    }
}
