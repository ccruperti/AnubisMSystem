using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Infraestructure.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Obtiene la fecha de Inicio de Semana de la Fecha Actual.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetWeekStartDate(this DateTime dateTime)
        {
            CultureInfo cultureInfo = cultureInfo = CultureInfo.CurrentCulture;

            DayOfWeek firstDayofWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            while(dateTime.Date.DayOfWeek != firstDayofWeek)
            {
                dateTime = dateTime.AddDays(-1);
            }
            return dateTime;
        }

        public static DateTime GetWeekStartDate(this DateTime dateTime, CultureInfo cultureInfo)
        {
            DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            while (dateTime.Date.DayOfWeek != firstDayOfWeek)
            {
                dateTime = dateTime.AddDays(-1);
            }
            return dateTime;
        }

        public static DateTime GetWeekStartDate(this DateTime dateTime, DayOfWeek firstDayOfWeek)
        {
            while (dateTime.Date.DayOfWeek != firstDayOfWeek)
            {
                dateTime = dateTime.AddDays(-1);
            }
            return dateTime;
        }

        public static DateTime GetWeekEndDate(this DateTime dateTime)
        {
            return dateTime.GetWeekStartDate().AddDays(6);
        }

        public static DateTime GetWeekEndDate(this DateTime dateTime, CultureInfo cultureInfo)
        {
            return dateTime.GetWeekStartDate(cultureInfo).AddDays(6);
        }

        public static DateTime GetWeekEndDate(this DateTime dateTime, DayOfWeek firstDayOfWeek)
        {
            return dateTime.GetWeekStartDate(firstDayOfWeek).AddDays(6);
        }
    }
}
