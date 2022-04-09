using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace EmployeePartV2.Models
{
    public partial class HRdbContext : DbContext
    {
        public HRdbContext()
            : base("name=HRdbContext2")
        {
        }

        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Day> Days { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<GeneralSetting> GeneralSettings { get; set; }
        public virtual DbSet<generalSettingOfficialHoliday> generalSettingOfficialHolidays { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<ModulePermission> ModulePermissions { get; set; }
        public virtual DbSet<officialHoliday> officialHolidays { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Statue> Statues { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>()
                .Property(e => e.StartTime)
                .HasPrecision(0);

            modelBuilder.Entity<Attendance>()
                .Property(e => e.EndTime)
                .HasPrecision(0);

            modelBuilder.Entity<Company>()
                .Property(e => e.CompanyName)
                .IsUnicode(false);

            modelBuilder.Entity<Day>()
                .Property(e => e.DayName)
                .IsUnicode(false);

            modelBuilder.Entity<Day>()
                .HasMany(e => e.GeneralSettings)
                .WithMany(e => e.Days)
                .Map(m => m.ToTable("DaysWeekEnd").MapLeftKey("DayID").MapRightKey("GSID"));

            modelBuilder.Entity<Department>()
                .Property(e => e.DeptName)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.Employees)
                .WithRequired(e => e.Department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>()
                .HasOptional(e => e.GeneralSetting)
                .WithRequired(e => e.Department);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Nationality)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.SSN)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.SSNFile)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.StartTime)
                .HasPrecision(0);

            modelBuilder.Entity<Employee>()
                .Property(e => e.EndTime)
                .HasPrecision(0);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Attendances)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Statues)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GeneralSetting>()
                .HasMany(e => e.generalSettingOfficialHolidays)
                .WithRequired(e => e.GeneralSetting)
                .HasForeignKey(e => e.gsID);

            modelBuilder.Entity<generalSettingOfficialHoliday>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Module>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Module>()
                .HasMany(e => e.ModulePermissions)
                .WithRequired(e => e.Module)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ModulePermission>()
                .Property(e => e.degree)
                .IsUnicode(false);

            modelBuilder.Entity<officialHoliday>()
                .Property(e => e.officialHolidayName)
                .IsUnicode(false);

            modelBuilder.Entity<Permission>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Permission>()
                .HasMany(e => e.ModulePermissions)
                .WithRequired(e => e.Permission)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Statue>()
                .Property(e => e.ExtraTime)
                .HasPrecision(0);

            modelBuilder.Entity<Statue>()
                .Property(e => e.LateTime)
                .HasPrecision(0);

            modelBuilder.Entity<User>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.userName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);
        }
    }
}
