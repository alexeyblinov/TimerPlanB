using PlanB.BL.Controller;
using PlanB.BL.Model;
using System; //wtf??

namespace PlanB.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            int startNunber;
            RiderClass classId;


            System.Console.WriteLine("Enter rider's Start number:");
            int.TryParse(System.Console.ReadLine(), out startNunber);
            System.Console.WriteLine("Enter rider's Class (A,B,C1,C2,C3,D1,D2,D3,D4,N):");
            switch (System.Console.ReadLine())
            {
                case "A":
                    classId = RiderClass.A;
                    break;
                case "B":
                    classId = RiderClass.B;
                    break;
                case "C1":
                    classId = RiderClass.C1;
                    break;
                case "C2":
                    classId = RiderClass.C2;
                    break;
                case "C3":
                    classId = RiderClass.C3;
                    break;
                case "D1":
                    classId = RiderClass.D1;
                    break;
                case "D2":
                    classId = RiderClass.D2;
                    break;
                case "D3":
                    classId = RiderClass.D3;
                    break;
                case "D4":
                    classId = RiderClass.D4;
                    break;
                case "N":
                    classId = RiderClass.N;
                    break;
                default:
                    throw new ArgumentException("Class ID uot of range.", nameof(classId));
            }



   

            RiderController riderController = new RiderController(startNunber, classId);
            if (string.IsNullOrEmpty(riderController.CurrentRider.Name))
            {
                System.Console.WriteLine("Enter rider data.");
                System.Console.WriteLine("Name: ");
                var name = System.Console.ReadLine();
                System.Console.WriteLine("Surname: ");
                var surname = System.Console.ReadLine();
                System.Console.WriteLine("Gender (M|F): ");
                var gender = System.Console.ReadLine();
                System.Console.WriteLine("Hometown: ");
                var location = System.Console.ReadLine();
                System.Console.WriteLine("Team: ");
                var team = System.Console.ReadLine();

                riderController.SetNewRiderData(name, surname, gender, location, team);
            }


            System.Console.WriteLine(riderController.CurrentRider);
            System.Console.WriteLine();


            // ex
            var rnd = new Random();
            var Race = new RaceController();
            for(var i = 0; i < riderController.Riders.Count; i++)
            {
                
                Race.ChangeRank(riderController, riderController.Riders[i], rnd.Next(1000, 300000), 0, false);
            }
            Race.ChangeRank(riderController, riderController.CurrentRider, rnd.Next(1000, 300000), 0, false);

            System.Console.WriteLine();
            foreach (var r in riderController.Riders)
            {
                System.Console.WriteLine(r);
            }
            // end ex

            System.Console.ReadLine();
        }
    }
}
