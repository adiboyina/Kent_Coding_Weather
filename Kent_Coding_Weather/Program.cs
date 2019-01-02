using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace PicnicPlan
{
    class Program
    {
        static void Main(string[] args)
        {
           

            /*
             * This is the steps we follow to satisfy the business requirement. 
             *  => Since Temparature have bounded values (70-85 fahrenheit) we will considered that as First validation
             *  => We will order by consecutive days
             *  => Then we will order by precipitation
             *  
             *  Since all the data in the file is in celcius we will convert the 70 - 85 f to celcius. So that we can avaoid conversion on each line.
             * 
             * */

            PicnicPlan picnicPlan = new PicnicPlan();
            List<string> bestDaysWeather;

            ConsoleKeyInfo cki;
            do
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("**************************************************\r\n");
                Console.WriteLine("\r\n1.Picks the best days for Picnic with least amount of Precipitation and temperature range between 70F - 85F\r\n2.Picks the best days for the Vacation ");
                Console.WriteLine("\r\nSelect option number or press Esc To EXIT\r\n");
                Console.WriteLine("**************************************************\r\n");
                cki = Console.ReadKey(false);
                Console.WriteLine("**************************************************\r\n");

                switch (cki.KeyChar.ToString())
                {
                    case "1":
                        bestDaysWeather = picnicPlan.GetPicnicPlan();
                        Console.WriteLine("**************************************************\r\n");
                        Console.WriteLine("Best Days available for the Picnic considering the temperature range 70F - 85F and with least amount of Precipitation\r\n");
                        Console.WriteLine("**************************************************\r\n");
                        if (bestDaysWeather?.Any() == true)
                        {
                            foreach (string bestDayWeather in bestDaysWeather)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"\r\n{bestDayWeather}\r\n");

                            }
                        }

                        break;
                    case "2":
                        Console.WriteLine("Please enter the number of days for the Vacation\r\n");
                        int numberOfDays = 0;
                        if (int.TryParse(Console.ReadLine().Trim(), out numberOfDays))
                        {
                            bestDaysWeather = picnicPlan.GetPicnicPlan(numberOfDays);

                            Console.WriteLine("**************************************************\r\n");
                            Console.WriteLine($"Best Days, Maximum {bestDaysWeather.Count} days available for the vacation considering the Teamperature and Precipitation\r\n");
                            Console.WriteLine("**************************************************\r\n");
                            if (bestDaysWeather?.Any() == true)
                            {
                                foreach (string bestDayWeather in bestDaysWeather)
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine($"\r\n{bestDayWeather}\r\n");
                                }
                            }

                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("\r\nPlease enter a numeric value for number of days to plan the Picnic\r\n");
                        }
                        break;
                    default:
                        if (cki.Key != ConsoleKey.Escape)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Please choose correct option from 1-2, or hit ESC to exit\r\n", ConsoleColor.Red);
                        }
                        break;
                }


            } while (cki.Key != ConsoleKey.Escape);

        }
    }
}