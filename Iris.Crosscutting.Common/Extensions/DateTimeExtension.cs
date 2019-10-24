using System;
using System.Runtime.InteropServices;

namespace Iris.Crosscutting.Common.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ToBrazilianTimezone(this DateTime dateTime)
        {
            TimeZoneInfo targetTimeZone;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            }
            else
            {
                targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            }

            var targetDatetime = TimeZoneInfo.ConvertTime(dateTime, targetTimeZone);

            return targetDatetime;
        }
    }
}