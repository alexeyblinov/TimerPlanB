using PlanB.BL.Controller;
using System; //wtf??

namespace PlanB.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            int startNunber;
            string classId;

            System.Console.WriteLine("Enter rider's Start number:");
            int.TryParse(System.Console.ReadLine(), out startNunber);
            System.Console.WriteLine("Enter rider's Class (B,C,D,N):");
            classId = System.Console.ReadLine();

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
            System.Console.ReadLine();
        }
    }
}
