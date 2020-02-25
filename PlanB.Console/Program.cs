using PlanB.BL.Controller;
using System; //wtf??

namespace PlanB.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            int startNunber;
            System.Console.WriteLine("Enter rider data.");
            System.Console.WriteLine("Start number: ");
            int.TryParse(System.Console.ReadLine(), out startNunber);
            System.Console.WriteLine("Name: ");
            var name = System.Console.ReadLine();
            System.Console.WriteLine("Surname: ");
            var surname = System.Console.ReadLine();
            System.Console.WriteLine("Gender (M|F): ");
            var gender = System.Console.ReadLine();
            System.Console.WriteLine("Hometown: ");
            var hometown = System.Console.ReadLine();
            System.Console.WriteLine("Team: ");
            var team = System.Console.ReadLine();
            
            RiderController riderController = new RiderController(startNunber, name, surname, gender, hometown, team);
            riderController.Save(riderController);
            System.Console.ReadLine();
        }
    }
}
