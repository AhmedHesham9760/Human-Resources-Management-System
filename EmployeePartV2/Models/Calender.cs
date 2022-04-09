using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeePartV2.Models;

namespace EmployeePartV2.Models
{
    public class calender
    {
        List<object> months;
        public static List<object> Months
        {
            get
            {
                return new List<object>()
                {
                    new { month = 0, monthName = "Choose"},
                    new { month = 1, monthName = "January" },
                    new { month = 2, monthName = "February" },
                    new { month = 3, monthName = "March" },
                    new { month = 4, monthName = "April" },
                    new { month = 5, monthName = "May" },
                    new { month = 6, monthName = "June" },
                    new { month = 7, monthName = "July" },
                    new { month = 8, monthName = "August" },
                    new { month = 9, monthName = "September" },
                    new { month = 10, monthName = "October" },
                    new { month = 11, monthName = "November" },
                    new { month = 12, monthName = "December" },
                };
            }
        }
        public static List<object> getMonths(DateTime startDate)
        {
            List<object> months = new List<object>();
            months.Add(new { month = 0, monthName = "Choose" });
            for (DateTime i = startDate; (i.Month <= 12) && (i.Year == startDate.Year); i = i.AddMonths(1))
            {
                months.Add(new { month = i.Month, monthName = i.ToString("MMMM") });
            }
            return months;
        }
        public static List<object> getYears(DateTime startDate)
        {
            List<object> years = new List<object>();
            years.Add(new { yearValue = 0, displayYear = "Choose" });
            for (int i = startDate.Year; i <= DateTime.Today.Year; i++)
            {
                years.Add(new { yearValue = i, displayYear = i });
            }
            return years;
        }
    }
}


