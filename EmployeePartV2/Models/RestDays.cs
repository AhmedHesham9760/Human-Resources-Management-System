using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeePartV2.Models;

namespace EmployeePartV2.Models
{
    public class RestDays
    {
        HRdbContext db = new HRdbContext();
        //calculate number of week ends in the month 
        public static byte weekEndCounter(DateTime startDate, List<string> weekEndDays)
        {
            byte weekEndDaysInMonthCount = 0;
            DateTime endDate = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
            if (weekEndDays.Count() != 0)
            {
                while (startDate <= endDate)
                {
                    foreach (var item in weekEndDays)
                    {
                        if (startDate.ToString("dddd") == item)
                        {
                            weekEndDaysInMonthCount++;
                        }
                    }
                    startDate = startDate.AddDays(1);
                }
                return weekEndDaysInMonthCount;
            }
            else
            {
                return 0;
            }

        }

        //calculate number of official Holiday days in Month 
        public static int officialHolidayCounter(Statue employeeStatue)
        {

            List<generalSettingOfficialHoliday> officialHolidays = employeeStatue.Employee.Department.GeneralSetting.generalSettingOfficialHolidays.ToList();
            if (officialHolidays.Count() == 0)
            {
                return 0;
            }
            else
            {
                List<string> weekEndDays = employeeStatue.Employee.Department.GeneralSetting.Days.Select(n => n.DayName).ToList();
                List<DateTime> netOfficialHolidays = new List<DateTime>();
                foreach (var item in officialHolidays)
                {
                    DateTime startCounter = new DateTime(item.officialHoliday.startDate.Year, item.officialHoliday.startDate.Month, item.officialHoliday.startDate.Day);
                    DateTime endCounter;
                    if (employeeStatue.month.Month == DateTime.Today.Month && employeeStatue.month.Year == DateTime.Today.Year)
                    {
                        endCounter = DateTime.Today;
                    }
                    else
                    {
                        endCounter = new DateTime(employeeStatue.month.Year, employeeStatue.month.Month, DateTime.DaysInMonth(employeeStatue.month.Year, employeeStatue.month.Month));
                    }

                    while ((startCounter <= item.officialHoliday.endDate) && (startCounter <= endCounter))
                    {
                        netOfficialHolidays.Add(startCounter);
                        startCounter = startCounter.AddDays(1);
                    }
                }
                return netOfficialHolidays.Distinct().Select(n => n.ToString("dddd")).Except(weekEndDays).Count();
            }
        }
    }
}