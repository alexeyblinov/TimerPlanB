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
            var riderPro = new List<RiderController>();
            var riderSportsmens = new List<RiderController>();
            var riderAmateurs = new List<RiderController>();
            var riderNovices = new List<RiderController>();
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
                    System.Console.Write("Enter rider's Class (A,B,C1,C2,C3,D1,D2,D3,D4,N):");

                    var stringClass = System.Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(stringClass))
                    {
                        throw new ArgumentNullException("Class cannot be null.", nameof(stringClass));
                    }
                    if (!classId.Contains(stringClass))
                    {
                        throw new ArgumentException("Wrong Class name.", nameof(stringClass));
                    }

                    var currentRider = new RiderController(startNunber, stringClass);
                    FullRiderData(currentRider);
                    switch (stringClass)
                    {
                        case  "A":
                        case  "B":
                        case "C1":
                        case "C2":
                            riderPro.Add(currentRider);
                            break;
                        case "C3":
                        case "D1":
                            riderSportsmens.Add(currentRider);
                            break;
                        case "D2":
                        case "D3":
                            riderAmateurs.Add(currentRider);
                            break;
                        case "D4":
                        case  "N":
                            riderNovices.Add(currentRider);
                            break;
                    }


                    //System.Console.WriteLine(riderController.CurrentRider);
                    //System.Console.WriteLine();


                    /* ex
                    var rnd = new Random();
                    for (var i = 0; i < riderController.Riders.Count; i++)
                    {

                        RaceController.ChangeRank(riderController, riderController.Riders[i], rnd.Next(1000, 300000), 0, false);
                    }
                    RaceController.ChangeRank(riderController, riderController.CurrentRider, rnd.Next(1000, 300000), 0, false);

                    System.Console.WriteLine();
                    foreach (var r in riderController.Riders)
                    {
                        System.Console.WriteLine(r);
                    }
                    */
                }
                else if (raceStart.Contains("S"))
                {
                    // TODO присвоить всем значения попыток.
                    System.Console.Clear();
                    if(riderPro != null)
                    {
                        foreach (var r in riderPro)
                        {
                            System.Console.Write("Enter lap 1 time for ", r.ToString(), ": ");
                            int.TryParse(System.Console.ReadLine(), out int lap);
                            RaceController.ChangeRank(r, r.CurrentRider, lap, 0);
                            System.Console.Write("Enter lap 2 time for ", r.ToString(), ": ");
                            int.TryParse(System.Console.ReadLine(), out lap);
                            RaceController.ChangeRank(r, r.CurrentRider, lap, 0);
                            System.Console.WriteLine();
                        }
                    }
                    
                    System.Console.WriteLine();
                    if(riderSportsmens != null)
                    {
                        foreach (var r in riderSportsmens)
                        {
                            System.Console.Write("Enter lap 1 time for ", r.ToString(), ": ");
                            int.TryParse(System.Console.ReadLine(), out int lap);
                            RaceController.ChangeRank(r, r.CurrentRider, lap, 0);
                            System.Console.Write("Enter lap 2 time for ", r.ToString(), ": ");
                            int.TryParse(System.Console.ReadLine(), out lap);
                            RaceController.ChangeRank(r, r.CurrentRider, lap, 0);
                            System.Console.WriteLine();
                        }
                    }
                    
                    System.Console.WriteLine();
                    if(riderAmateurs != null)
                    {
                        foreach (var r in riderAmateurs)
                        {
                            System.Console.Write("Enter lap 1 time for ", r.ToString(), ": ");
                            int.TryParse(System.Console.ReadLine(), out int lap);
                            RaceController.ChangeRank(r, r.CurrentRider, lap, 0);
                            System.Console.Write("Enter lap 2 time for ", r.ToString(), ": ");
                            int.TryParse(System.Console.ReadLine(), out lap);
                            RaceController.ChangeRank(r, r.CurrentRider, lap, 0);
                            System.Console.WriteLine();
                        }
                    }
                    
                    System.Console.WriteLine();
                    if(riderNovices != null)
                    {
                        foreach (var r in riderNovices)
                        {
                            System.Console.Write("Enter lap 1 time for ", r.ToString(), ": ");
                            int.TryParse(System.Console.ReadLine(), out int lap);
                            RaceController.ChangeRank(r, r.CurrentRider, lap, 0);
                            System.Console.Write("Enter lap 2 time for ", r.ToString(), ": ");
                            int.TryParse(System.Console.ReadLine(), out lap);
                            RaceController.ChangeRank(r, r.CurrentRider, lap, 0);
                            System.Console.WriteLine();
                        }
                    }
                    

                    // ищем класс соревнования и эталонное время.
                    var bestTime = 0;
                    string bestClass = null;

                    if(riderPro.Count > 2)
                    {

                    }


                    for (var i = 0; i < Racers.Count; i++)
                    {
                        if(Racers[i].Riders != null)
                        {
                            if (Racers[i].Riders.Count > 2)
                            {
                                bestClass = RaceController.FindCompetitionClassId(Racers[i], ref bestTime);
                                break;
                            }
                        }
                        
                    }
                    foreach(var r in Racers)
                    {
                        RaceController.SetNewClasses(r, bestClass, bestTime);
                    }
                    
                }
                else
                {
                    break;
                }
                
            }

            // Вывод результатов.
            System.Console.Clear();

            foreach(var r in riderPro.Riders)
            {
                r.ToString();
                System.Console.WriteLine();
            }
            System.Console.WriteLine();
            foreach (var r in riderSportsmens.Riders)
            {
                r.ToString();
                System.Console.WriteLine();
            }
            System.Console.WriteLine();
            foreach (var r in riderAmateurs.Riders)
            {
                r.ToString();
                System.Console.WriteLine();
            }
            System.Console.WriteLine();
            foreach (var r in riderNovices.Riders)
            {
                r.ToString();
                System.Console.WriteLine();
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
