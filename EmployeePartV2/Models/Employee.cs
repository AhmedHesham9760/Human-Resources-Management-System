namespace EmployeePartV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Employee")]
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            Attendances = new HashSet<Attendance>();
            Statues = new HashSet<Statue>();
        }

        [Key]
        public int EmpID { get; set; }


        [StringLength(50)]
        [Display(Name = "FullName")]
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }


        [StringLength(250)]
        [Required(ErrorMessage = "*")]
        public string Address { get; set; }

        [StringLength(1)]
        [Required(ErrorMessage = "*")]
        public string Gender { get; set; }


        [StringLength(50)]
        [Required(ErrorMessage = "*")]
        public string Email { get; set; }


        [StringLength(50)]
        [Required(ErrorMessage = "*")]
        public string Nationality { get; set; }

        [StringLength(14)]
        [Required(ErrorMessage = "*")]
        public string SSN { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "*")]
        public string Notes { get; set; }

        [StringLength(150)]
        [Required(ErrorMessage = "*")]
        public string SSNFile { get; set; }

        [StringLength(11)]
        [Required(ErrorMessage = "*")]
        public string Phone { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "*")]
        public DateTime Birthdate { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "*")]
        public DateTime contractStartDate { get; set; }
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "*")]
        public TimeSpan StartTime { get; set; }
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "*")]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "*")]
        public int Salary { get; set; }

        [Display(Name = "Departments")]
        public int DeptID { get; set; }

        public virtual Department Department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Statue> Statues { get; set; }
    }
}
