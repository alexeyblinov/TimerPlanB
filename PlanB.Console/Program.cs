using PlanB.BL.Controller;
using PlanB.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanB.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            RiderController riderCo = new RiderController();
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("Enter 'R' to registration or 'S' to start a race:");
                var raceStart = System.Console.ReadLine();
                if (raceStart.Contains("R"))
                {
                    int startNunber;
                    string[] classId = { "A", "B", "C1", "C2", "C3", "D1", "D2", "D3", "D4", "N" };

                    System.Console.Write("Enter rider's Start number: ");
                    int.TryParse(System.Console.ReadLine(), out startNunber);
                    if (startNunber <= 0)
                    {
                        throw new ArgumentException("1 to 99.", nameof(startNunber));
                    }
                    System.Console.Write("Enter rider's Class (A,B,C1,C2,C3,D1,D2,D3,D4,N): ");

                    var stringClass = System.Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(stringClass))
                    {
                        throw new ArgumentNullException("Class cannot be null.", nameof(stringClass));
                    }
                    if (!classId.Contains(stringClass))
                    {
                        throw new ArgumentException("Wrong Class name.", nameof(stringClass));
                    }

                    riderCo = new RiderController(startNunber, stringClass);
                    FullRiderData(riderCo);

                }
                else if (raceStart.Contains("S"))
                {
                    
                    // присвоить всем значения попыток.

                    System.Console.Clear();
                    
                    if(riderCo != null)
                    {
                        for(var i = 0; i < riderCo.Riders.Count; i++)
                        {
                            System.Console.Write("Enter lap 1 time for " + riderCo.Riders[i].ToString() + ": ");
                            int.TryParse(System.Console.ReadLine(), out int lap);
                            RaceController.ChangeRank(riderCo, riderCo.Riders[i], lap, 0);
                            System.Console.Write("Enter lap 2 time for " + riderCo.Riders[i].ToString() + ": ");
                            int.TryParse(System.Console.ReadLine(), out lap);
                            RaceController.ChangeRank(riderCo, riderCo.Riders[i], lap, 0);
                            System.Console.WriteLine();
                        }
                    }
                    
                    RaceController.SetNewPlaces(riderCo);

                    
                    // ищем класс соревнования и время лучшего участника в этом классе.
                    var bestTime = 0;
                    string bestClass = null;

                    if(riderCo != null)
                    {
                        // если не найдёт эталонный класс, вернёт bestClass = null.
                        bestClass = RaceController.FindCompetitionClassId(riderCo, ref bestTime);
                        if (bestClass.Equals(null))
                        {
                            throw new ArgumentNullException("Best class cannot be set.", nameof(bestClass));
                        }
                    }
                    

                    // Рассчёт эталонного времени трассы и установка новых классов по результатам соревнования.
                    RaceController.SetNewClasses(riderCo, bestClass, bestTime);

                    
                }
                else
                {
                    break;
                }
                
            }
            
            // Вывод результатов.
            System.Console.Clear();
            
            foreach (var r in riderCo.Riders)
            {
                System.Console.WriteLine(r.ToString());
            }
                     
            
            System.Console.ReadLine();

            

            void FullRiderData(RiderController riderController)
            {
                if (string.IsNullOrEmpty(riderController.CurrentRider.Name))
                {
                    System.Console.WriteLine("Enter rider's data. ", riderController.CurrentRider.RiderId);
                    System.Console.Write("Name: ");
                    var name = System.Console.ReadLine();
                    System.Console.Write("Surname: ");
                    var surname = System.Console.ReadLine();
                    System.Console.Write("Gender (M|F): ");
                    var gender = System.Console.ReadLine();
                    System.Console.Write("Hometown: ");
                    var location = System.Console.ReadLine();
                    System.Console.Write("Team: ");
                    var team = System.Console.ReadLine();

                    riderController.SetNewRiderData(name, surname, gender, location, team);
                }
            }
        }
    }
}
