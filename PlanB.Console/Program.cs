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
            RiderController riderPro = new RiderController();
            RiderController riderSportsmens = new RiderController();
            RiderController riderAmateurs = new RiderController();
            RiderController riderNovices = new RiderController();
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("Enter 'R' to registration or S to start a race:");
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

                    switch (stringClass)
                    {
                        case  "A":
                        case  "B":
                        case "C1":
                        case "C2":
                            riderPro = new RiderController(startNunber, stringClass);
                            FullRiderData(riderPro);
                            riderPro.Save();
                            break;
                        case "C3":
                        case "D1":
                            riderSportsmens = new RiderController(startNunber, stringClass);
                            FullRiderData(riderSportsmens);
                            riderSportsmens.Save();
                            break;
                        case "D2":
                        case "D3":
                            riderAmateurs = new RiderController(startNunber, stringClass);
                            FullRiderData(riderAmateurs);
                            riderAmateurs.Save();
                            break;
                        case "D4":
                        case  "N":
                            riderNovices = new RiderController(startNunber, stringClass);
                            FullRiderData(riderNovices);
                            riderNovices.Save();
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
                    foreach (var r in riderPro.Riders)
                    {
                        System.Console.Write("Enter lap 1 time for ", r.ToString(), ": ");
                        int.TryParse(System.Console.ReadLine(), out int lap);
                        RaceController.ChangeRank(riderPro, r, lap, 0);
                        System.Console.Write("Enter lap 2 time for ", r.ToString(), ": ");
                        int.TryParse(System.Console.ReadLine(), out lap);
                        RaceController.ChangeRank(riderPro, r, lap, 0);
                        System.Console.WriteLine();
                    }
                    System.Console.WriteLine();
                    foreach (var r in riderSportsmens.Riders)
                    {
                        System.Console.Write("Enter lap 1 time for ", r.ToString(), ": ");
                        int.TryParse(System.Console.ReadLine(), out int lap);
                        RaceController.ChangeRank(riderPro, r, lap, 0);
                        System.Console.Write("Enter lap 2 time for ", r.ToString(), ": ");
                        int.TryParse(System.Console.ReadLine(), out lap);
                        RaceController.ChangeRank(riderPro, r, lap, 0);
                        System.Console.WriteLine();
                    }
                    System.Console.WriteLine();
                    foreach (var r in riderAmateurs.Riders)
                    {
                        System.Console.Write("Enter lap 1 time for ", r.ToString(), ": ");
                        int.TryParse(System.Console.ReadLine(), out int lap);
                        RaceController.ChangeRank(riderPro, r, lap, 0);
                        System.Console.Write("Enter lap 2 time for ", r.ToString(), ": ");
                        int.TryParse(System.Console.ReadLine(), out lap);
                        RaceController.ChangeRank(riderPro, r, lap, 0);
                        System.Console.WriteLine();
                    }
                    System.Console.WriteLine();
                    foreach (var r in riderNovices.Riders)
                    {
                        System.Console.Write("Enter lap 1 time for ", r.ToString(), ": ");
                        int.TryParse(System.Console.ReadLine(), out int lap);
                        RaceController.ChangeRank(riderPro, r, lap, 0);
                        System.Console.Write("Enter lap 2 time for ", r.ToString(), ": ");
                        int.TryParse(System.Console.ReadLine(), out lap);
                        RaceController.ChangeRank(riderPro, r, lap, 0);
                        System.Console.WriteLine();
                    }

                    // ищем класс соревнования и эталонное время.
                    var Racers = new List<RiderController> { riderPro, riderSportsmens, riderAmateurs, riderNovices };
                    var bestTime = 0;
                    string bestClass = null;
                    for (var i = 0; i < Racers.Count; i++)
                    {
                        if(Racers[i].Riders.Count > 2)
                        {
                            bestClass = RaceController.FindCompetitionClassId(Racers[i], ref bestTime);
                            break;
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
                    System.Console.WriteLine("Enter rider's data.");
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
