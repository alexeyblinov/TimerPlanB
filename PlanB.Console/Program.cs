using PlanB.BL.Controller;
using PlanB.BL.Model;
using System; 
using System.Linq;

namespace PlanB.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            int startNunber;
            string[] classId = { "A", "B", "C1", "C2", "C3", "D1", "D2", "D3", "D4", "N" };

            System.Console.WriteLine("Enter rider's Start number:");
            int.TryParse(System.Console.ReadLine(), out startNunber);
            if(startNunber <= 0)
            {
                throw new ArgumentException("1 to 99.", nameof(startNunber));
            }
            System.Console.WriteLine("Enter rider's Class (A,B,C1,C2,C3,D1,D2,D3,D4,N):");

            var stringClass = System.Console.ReadLine();
            if (string.IsNullOrWhiteSpace(stringClass))
            {
                throw new ArgumentNullException("Class cannot be null.", nameof(stringClass));
            }
            if (!classId.Contains(stringClass))
            {
                throw new ArgumentException("Wrong Class name.", nameof(stringClass));
            }


            RiderController riderController = new RiderController(startNunber, stringClass);
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
