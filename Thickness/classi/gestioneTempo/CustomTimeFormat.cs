using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thickness.classi.gestioneTempo
{
    internal class CustomTimeFormat
    {
        int years;
        int months;
        int days;
        int hours;
        int minutes;
        int seconds;

        public CustomTimeFormat(long totalSeconds)
        {
            if (totalSeconds < 0)
                throw new ArgumentException("totalSeconds cannot be negative.");

            // Assuming 1 year = 12 months and 1 month = 30 days for simplicity  
            years = (int)(totalSeconds / 31104000); // 12 months in seconds  
            totalSeconds %= 31104000;
            months = (int)(totalSeconds / 2592000); // 30 days in seconds  
            totalSeconds %= 2592000;
            days = (int)(totalSeconds / 86400); // 24 hours in seconds  
            totalSeconds %= 86400;
            hours = (int)(totalSeconds / 3600); // 60 minutes in seconds  
            totalSeconds %= 3600;
            minutes = (int)(totalSeconds / 60); // 60 seconds in minutes  
            seconds = (int)(totalSeconds % 60); // Remaining seconds  
        }

        public string GetFormattedTime()
        {
            return $"{years}/{months}/{days}/{hours}/{minutes}/{seconds}";
        }

        public long GetTotalSeconds()
        {
            return ((years * 31104000) + (months * 2592000) + (days * 86400) + (hours * 3600) + (minutes * 60) + seconds);
        }
    }
}
