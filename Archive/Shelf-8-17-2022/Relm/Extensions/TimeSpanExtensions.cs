using System;

namespace Relm.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToTimePlayed(this TimeSpan timeSpan)
        {
            var hours = (int)timeSpan.TotalHours;
            var minutes = timeSpan.Minutes;
            return hours > 99 
                ? $"{hours:D3}h {minutes:D2}m" 
                : $"{hours:D2}h {minutes:D2}m";
        }
    }
}