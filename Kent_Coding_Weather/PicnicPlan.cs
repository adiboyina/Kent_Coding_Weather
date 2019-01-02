using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.IO;

namespace PicnicPlan
{
    public class PicnicPlan
    {
        string _filename = ConfigurationSettings.AppSettings["WeatherFilePath"];
        double _highTemperatureInC = Utilities.ConvertFahrenheitToCelsius(85);
        double _lowTemperatureInC = Utilities.ConvertFahrenheitToCelsius(70);
        List<DaysWeather> _bestNdaysWeather = new List<DaysWeather>();

        public List<String> GetPicnicPlan(int? numberOfDays = null)
        {
            List<DaysWeather> normalizedDaysWeather = new List<DaysWeather>();
            List<String> picnicPlanDays = new List<string>();
            DateTime datevalue;
            if (_filename != null && File.Exists(_filename))
            {
                try
                {
                    List<String> lines = File.ReadLines(_filename).ToList();

                    string headerLine = lines.FirstOrDefault();

                    if (headerLine != null && headerLine.Contains(Utilities.dayOfMonth))
                    {
                        List<string> header = headerLine.Split().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                        Utilities.highCelciusIndex = header.IndexOf(Utilities.highCelcius);
                        Utilities.lowCelciusIndex = header.IndexOf(Utilities.lowCelcius);
                        Utilities.chanceOfPrecipitationIndex = header.IndexOf(Utilities.chanceOfPrecipitation);
                        Utilities.dayOfMonthIndex = header.IndexOf(Utilities.dayOfMonth);
                        Utilities.dayOfWeekIndex = header.IndexOf(Utilities.dayOfWeek);

                        foreach (var line in lines.Skip(1))
                        {
                            DaysWeather dayWeather = Utilities.ParseFileDataToWeatherData(line);

                            if ((dayWeather.HighCelcius >= _lowTemperatureInC) && (dayWeather.HighCelcius <= _highTemperatureInC) &&
                                (dayWeather.LowCelsius >= _lowTemperatureInC) && (dayWeather.LowCelsius <= _highTemperatureInC))
                            {
                                normalizedDaysWeather.Add(dayWeather);
                                datevalue = new DateTime(2017, 11, normalizedDaysWeather.FirstOrDefault().DayOfMonth);
                                
                            }
                        }


                        if (normalizedDaysWeather?.Count > 0)
                        {
                            List<DaysWeather> bestDaysWeather = new List<DaysWeather>();
                            if (numberOfDays.HasValue && numberOfDays > 0)
                            {
                                bestDaysWeather = PickBestNDaysWeather(normalizedDaysWeather, numberOfDays.GetValueOrDefault());
                            }
                            else
                            {
                                var groupedDaysWeather = normalizedDaysWeather.OrderBy(x => x.ChanceOfPrecipitation)
                                                                              .GroupBy(x => x.ChanceOfPrecipitation)
                                                                             
                                                                              .Select(grp => grp.ToList())
                                                                              .ToList();
                                bestDaysWeather = groupedDaysWeather.FirstOrDefault();
                            }
                            foreach (DaysWeather dw in bestDaysWeather)
                            {
                                datevalue = new DateTime(2017, 11, dw.DayOfMonth);
                                picnicPlanDays.Add($"{datevalue.ToString("dddd")} the {dw.DayOfMonth}th day of the month is the best day for a picnic.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"File header format is not correct to parse the file : {_filename}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occured while planning the Picnic Plan : {ex.Message} \r\n {ex.InnerException}");
                }
            }
            else
            {
                Console.WriteLine(_filename == null ? "File path is missing in the Config" : $"File does not exist in the path {_filename}");
            }

            return picnicPlanDays;
        }

        public List<DaysWeather> PickBestNDaysWeather(List<DaysWeather> dwOrderedByDays, int nDays)
        {
            Dictionary<string, List<DaysWeather>> bestNdaysWeather = new Dictionary<string, List<DaysWeather>>();
            List<DaysWeather> dwOrderedByPrecipitation = dwOrderedByDays.OrderBy(daysWeather => daysWeather.ChanceOfPrecipitation).ToList();

            foreach (DaysWeather dw in dwOrderedByPrecipitation)
            {
                int indexDw = dwOrderedByDays.FindIndex(x => (x.DayOfMonth == dw.DayOfMonth));
                int daysCount = dwOrderedByDays.Count;

                for (int i = nDays; i > 0; i--)
                {
                    if ((indexDw + i) <= (daysCount - 1) && dwOrderedByDays[indexDw + (i - 1)].DayOfMonth == dwOrderedByDays[indexDw].DayOfMonth + (i - 1))
                    {
                        if (i == nDays)
                        {
                            return dwOrderedByDays.GetRange(indexDw, i);
                        }
                        else
                        {
                            if (!bestNdaysWeather.ContainsKey(dw.ChanceOfPrecipitation.ToString()))
                            {
                                bestNdaysWeather.Add(dw.ChanceOfPrecipitation.ToString(), dwOrderedByDays.GetRange(indexDw, i));
                                break;
                            }
                            else
                            {
                                if (bestNdaysWeather[dw.ChanceOfPrecipitation.ToString()].Count < i)
                                {
                                    bestNdaysWeather.Remove(dw.ChanceOfPrecipitation.ToString());
                                    bestNdaysWeather.Add(dw.ChanceOfPrecipitation.ToString(), dwOrderedByDays.GetRange(indexDw, i));
                                    break;
                                }

                                break;
                            }
                        }
                    }
                }

                if (!bestNdaysWeather.ContainsKey(dw.ChanceOfPrecipitation.ToString()))
                {
                    bestNdaysWeather.Add(dw.ChanceOfPrecipitation.ToString(), dwOrderedByDays.GetRange(indexDw, 1));
                }
            }

            //return the max days not greater than Ndays considering the least amount of precipitation ordering
            return bestNdaysWeather.OrderByDescending(x => x.Value.Count).First().Value;


        }

    }
}
