using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicnicPlan
{
    public static class Utilities
    {
        public static int highCelciusIndex;
        public static int lowCelciusIndex;
        public static int chanceOfPrecipitationIndex;
        public static int dayOfMonthIndex;
        public static int dayOfWeekIndex;


        public static readonly string highCelcius = "High(celcius)";
        public static readonly string lowCelcius = "Low(Celsius)";
        public static readonly string chanceOfPrecipitation = "ChanceOfPrecipitation";
        public static readonly string dayOfMonth = "DayOfMonth";
        public static readonly string dayOfWeek = "DayOfWeek(FirstLetter)";

        public static double ConvertCelsiusToFahrenheit(double celsiusTemp)
        {
            return ((9.0 / 5.0) * celsiusTemp) + 32;
        }

        public static double ConvertFahrenheitToCelsius(double fahrenheitTemp)
        {
            return (5.0 / 9.0) * (fahrenheitTemp - 32);
        }

        public static DaysWeather ParseFileDataToWeatherData(string weatherFileData)
        {
            DaysWeather weatherData = new DaysWeather();

            List<string> split = weatherFileData.Split().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            weatherData.DayOfMonth = Convert.ToInt16(split[Utilities.dayOfMonthIndex]);
            weatherData.DayOfWeek = split[Utilities.dayOfWeekIndex].Trim();
            weatherData.HighCelcius = Convert.ToDouble(split[Utilities.highCelciusIndex]);
            weatherData.LowCelsius = Convert.ToDouble(split[Utilities.lowCelciusIndex]);
            weatherData.ChanceOfPrecipitation = Convert.ToInt16(split[Utilities.chanceOfPrecipitationIndex]);

            return weatherData;
        }
    }
}