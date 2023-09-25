using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using static ELog.Core.PMMSEnums;

namespace ELog.Core.Helper
{
    public static class PMMSDateTimeHelper
    {
        public static List<DateTime> GetDatesMonth(DateTime currentDate)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(currentDate.Year, currentDate.Month))  // Days: 1, 2 ... 31 etc.
                                .Select(day => new DateTime(currentDate.Year, currentDate.Month, day)) // Map each day to a date
                                .ToList(); // Load dates into a list
        }

        public static List<DateTime> GetDatesWithoutWeekendsForMonth(DateTime currentDate)
        {
            var dates = new List<DateTime>();

            // Loop from the first day of the month until we hit the next month, moving forward a day at a time
            for (var date = new DateTime(currentDate.Year, currentDate.Month, 1); date.Month == currentDate.Month; date = date.AddDays(1))
            {
                if (!(date.DayOfWeek == DayOfWeek.Saturday && date.DayOfWeek == DayOfWeek.Sunday))
                {
                    dates.Add(date);
                }
            }

            return dates;
        }

        public static List<DateTime> GetDatesForWeeks(DateTime currentDate)
        {
            DateTime startOfWeek = DateTime.Today.AddDays((int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)currentDate.DayOfWeek);

            return Enumerable.Range(0, 7).Select(i => startOfWeek.AddDays(i)).ToList();
        }

        public static WeighingMachineFrequencyType GetDropDownToBindFrequencyTypes(List<DateTime> calibratedDates, List<int?> lstAvailableFrequencyType)
        {
            if (lstAvailableFrequencyType != null)
            {
                var currentDate = DateTime.UtcNow;
                if (lstAvailableFrequencyType.Any(x => x == (int)WeighingMachineFrequencyType.Monthly) && (!calibratedDates.Any()))
                {
                    return WeighingMachineFrequencyType.Monthly;
                }
                else if (lstAvailableFrequencyType.Any(x => x == (int)WeighingMachineFrequencyType.Weekly) && (!calibratedDates.Select(x => x.Date).Intersect(GetDatesForWeeks(currentDate).Select(x => x.Date)).Any()))
                {
                    return WeighingMachineFrequencyType.Weekly;
                }
                else if (lstAvailableFrequencyType.Any(x => x == (int)WeighingMachineFrequencyType.Daily) && (!calibratedDates.Select(x => x.Date).Any(x => x == currentDate.Date)))
                {
                    return WeighingMachineFrequencyType.Daily;
                }
            }

            return default;
        }
    }
}