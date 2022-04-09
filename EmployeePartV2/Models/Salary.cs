using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeePartV2.Models;

namespace EmployeePartV2.Models
{
    public class Salary
    {
        Statue employeeStatue;
        double DailyEmployeeSalary;
        double hourlyEmployeeSalary;

        DateTime date;
        public DateTime Date
        {
            get
            {
                date = employeeStatue.month;
                return date;
            }
        }
        int employeeStatusId;
        public int EmployeeStatusId
        {
            get
            {
                //employeeStatusId = employeeStatue.StatusID;
                return employeeStatusId;
            }
        }

        string employeeName;
        public string EmployeeName
        {
            get
            {
                employeeName = employeeStatue.Employee.Name;
                return employeeName;
            }
        }
        string address;
        public string Address
        {
            get
            {
                address = employeeStatue.Employee.Address;
                return address;
            }
        }

        string phone;
        public string Phone
        {
            get
            {
                phone = employeeStatue.Employee.Phone;
                return phone;
            }
        }

        string employeeDepartment;
        public string EmployeeDepartment
        {
            get
            {
                employeeDepartment = employeeStatue.Employee.Department.DeptName;
                return employeeDepartment;
            }
        }

        double basicSalary;
        public double BasicSalary
        {
            get
            {
                basicSalary = employeeStatue.Employee.Salary;
                return basicSalary;
            }
        }

        int attendanceDays;
        public int AttendanceDays
        {
            get
            {
                attendanceDays = Convert.ToInt32(employeeStatue.AttendanceDays);
                return attendanceDays;
            }
        }

        int bonusDays;
        public int BonusDays
        {
            get
            {
                bonusDays = Convert.ToInt32(employeeStatue.BonusDays);
                return bonusDays;
            }
        }

        int absentDays;
        public int AbsentDays
        {
            get
            {
                absentDays = Convert.ToInt32(DateTime.DaysInMonth(employeeStatue.month.Year, employeeStatue.month.Month) - employeeStatue.WeekEndHolidaysNumber - RestDays.officialHolidayCounter(employeeStatue) - employeeStatue.AttendanceDays);
                return absentDays;
            }
        }

        double extraHoursCost;
        public double ExtraHoursCost
        {
            get
            {
                extraHoursCost = Convert.ToDouble(TimeSpan.Parse(employeeStatue.ExtraTime.ToString()).TotalHours) * employeeStatue.Employee.Department.GeneralSetting.ExtraPerHour * hourlyEmployeeSalary;
                return Math.Round(extraHoursCost, 2);

            }
        }

        double discountHoursCost;
        public double DiscountHoursCost
        {
            get
            {
                discountHoursCost = Convert.ToDouble(TimeSpan.Parse(employeeStatue.LateTime.ToString()).TotalHours) * employeeStatue.Employee.Department.GeneralSetting.DiscountPerHour * hourlyEmployeeSalary;
                return Math.Round(discountHoursCost, 2);

            }
        }

        double totalExtraCost;
        public double TotalExtraCost
        {
            get
            {
                totalExtraCost = ExtraHoursCost + (BonusDays * DailyEmployeeSalary);
                if (totalExtraCost > BasicSalary)
                {
                    return BasicSalary;
                }
                else
                {
                    return Math.Round(totalExtraCost, 2);

                }
                //return Math.Round(totalExtraCost, 2);
            }
        }

        double totalDiscountCost;
        public double TotalDiscountCost
        {
            get
            {
                totalDiscountCost = DiscountHoursCost + (AbsentDays * DailyEmployeeSalary);
                if (totalDiscountCost > BasicSalary)
                {
                    return BasicSalary;
                }
                else
                {
                    return Math.Round(totalDiscountCost, 2);

                }
                //return Math.Round(totalDiscountCost, 2);
            }
        }

        double netSalary;
        public double NetSalary
        {
            get
            {
                netSalary = employeeStatue.Employee.Salary + TotalExtraCost - TotalDiscountCost;
                if (netSalary < 0)
                {
                    return BasicSalary;
                }
                return Math.Round(netSalary, 2);
                //return Math.Round(netSalary, 2);
            }
        }



        public Salary(Statue employeeStatue)
        {
            this.employeeStatue = employeeStatue;
            this.employeeStatusId = employeeStatue.StatusID;
            //this.DailyEmployeeSalary =Convert.ToDouble(employeeStatue.Employee.Salary)  / (30 - (employeeStatue.Employee.Department.GeneralSetting.Days.Count()*4));
            this.DailyEmployeeSalary = Math.Round((Convert.ToDouble(employeeStatue.Employee.Salary)) / 22, 2);
            this.hourlyEmployeeSalary = Math.Round((DailyEmployeeSalary / 8), 2);
        }

    }
}