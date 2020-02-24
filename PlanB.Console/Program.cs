using PlanB.BL.Model;

namespace PlanB.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Enter rider data.");
            System.Console.WriteLine("Start number: ");
            var number = System.Console.ReadLine();
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

            int startNumber;
            int.TryParse(number, out startNumber);

            Gender riderGender = new Gender(gender);
            

            Rider rider = new Rider(startNumber, name, surname, riderGender, hometown, team);

        }
    }
}
