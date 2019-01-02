using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicnicPlan
{
    public class DaysWeather
    {
        public int DayOfMonth { get; set; }
        public string DayOfWeek { get; set; }
        public double HighCelcius { get; set; }
        public double LowCelsius { get; set; }
        public int ChanceOfPrecipitation { get; set; }

    }
}