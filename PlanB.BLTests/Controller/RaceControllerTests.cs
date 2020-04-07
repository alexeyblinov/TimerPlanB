using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanB.BL.Model;
using System;
using System.Linq;


namespace PlanB.BL.Controller.Tests
{
    [TestClass()]
    public class RaceControllerTests
    {
        static readonly Random rnd = new Random();
        int startNumber = rnd.Next(1, 99);
        string name = Guid.NewGuid().ToString();
        string surname = Guid.NewGuid().ToString();
        string gender = "M";
        string location = Guid.NewGuid().ToString();
        string team = Guid.NewGuid().ToString();
        int lapTime = rnd.Next(0, Rider.MAXTIME - 4);
        int penalty = rnd.Next(0, 3);

        [TestMethod()]
        public void ChangeRankTest()
        {
            //Arrange
            var controller = new RiderController(startNumber, "D2");

            // Act
            controller.SetNewRiderData(name, surname, gender, location, team);

            RaceController.ChangeRank(controller, controller.CurrentRider, lapTime, penalty);
            lapTime = rnd.Next(0, Rider.MAXTIME - 4);
            penalty = rnd.Next(0, 3);
            RaceController.ChangeRank(controller, controller.CurrentRider, lapTime, penalty);

            //Assert
            if (controller.CurrentRider.TryFirst <= controller.CurrentRider.TrySecond)
            {
                Assert.AreEqual(controller.CurrentRider.TryFirst, controller.CurrentRider.BestResult);
            }
            else
            {
                Assert.AreEqual(controller.CurrentRider.TrySecond, controller.CurrentRider.BestResult);
            }
        }
        
        [TestMethod()]
        public void SetNewPlacesTest()
        {
            //Arrange
            var controller = new RiderController(startNumber, "D2");
            
            // Act
            controller.SetNewRiderData(name, surname, gender, location, team);

            startNumber = rnd.Next(1, 99);
            controller = new RiderController(startNumber, "D3");
            name = Guid.NewGuid().ToString();
            surname = Guid.NewGuid().ToString();
            location = Guid.NewGuid().ToString();
            team = Guid.NewGuid().ToString();
            controller.SetNewRiderData(name, surname, gender, location, team);

            RaceController.ChangeRank(controller, controller.Riders[0], lapTime, penalty);
            lapTime = rnd.Next(0, Rider.MAXTIME - 4);
            penalty = rnd.Next(0, 3);
            RaceController.ChangeRank(controller, controller.Riders[0], lapTime, penalty);
            lapTime = rnd.Next(0, Rider.MAXTIME - 4);
            penalty = rnd.Next(0, 3);
            RaceController.ChangeRank(controller, controller.Riders[1], lapTime, penalty);
            lapTime = rnd.Next(0, Rider.MAXTIME - 4);
            penalty = rnd.Next(0, 3);
            RaceController.ChangeRank(controller, controller.Riders[1], lapTime, penalty);

            RaceController.SetNewPlaces(controller);
            var best = controller.Riders[0].BestResult <= controller.Riders[1].BestResult;
            var rank = controller.Riders[0].Rank < controller.Riders[1].Rank;

            //Assert
            Assert.AreEqual(best, rank);
        }
        
        [TestMethod()]
        public void FindCompetitionClassIdTest()
        {
            //Arrange
            var controller = new RiderController();

            // Act
            for(int i = 0; i<3; i++)
            {
                startNumber = rnd.Next(1, 99);
                controller = new RiderController(startNumber, "D3");
                name = Guid.NewGuid().ToString();
                surname = Guid.NewGuid().ToString();
                location = Guid.NewGuid().ToString();
                team = Guid.NewGuid().ToString();
                controller.SetNewRiderData(name, surname, gender, location, team);
            }

            startNumber = rnd.Next(1, 99);
            controller = new RiderController(startNumber, "C1");
            name = Guid.NewGuid().ToString();
            surname = Guid.NewGuid().ToString();
            location = Guid.NewGuid().ToString();
            team = Guid.NewGuid().ToString();
            controller.SetNewRiderData(name, surname, gender, location, team);

            startNumber = rnd.Next(1, 99);
            controller = new RiderController(startNumber, "N");
            name = Guid.NewGuid().ToString();
            surname = Guid.NewGuid().ToString();
            location = Guid.NewGuid().ToString();
            team = Guid.NewGuid().ToString();
            controller.SetNewRiderData(name, surname, gender, location, team);

            foreach(var r in controller.Riders)
            {
                RaceController.ChangeRank(controller, r, rnd.Next(1, Rider.MAXTIME - 4), rnd.Next(1, 3));
                RaceController.ChangeRank(controller, r, rnd.Next(1, Rider.MAXTIME - 4), rnd.Next(1, 3));
            }
            RaceController.SetNewPlaces(controller);

            var bestTime = 0;
            var bestClass = RaceController.FindCompetitionClassId(controller, ref bestTime);
            var firstRiderInD3 = controller.Riders.FirstOrDefault(r => r.PreviousClassId == "D3").BestResult;

            //Assert
            Assert.AreEqual("D3", bestClass);
            Assert.AreEqual(firstRiderInD3, bestTime);
        }

        [TestMethod()]
        public void SetNewClassesTest()
        {
            //Arrange
            var controller = new RiderController(1, "N");
            var bestTime = 4348;

            //Act
            controller.SetNewRiderData(name, surname, gender, location, team);
            controller.CurrentRider.BestResult = 6147;

            RaceController.SetNewClasses(controller, "C3", bestTime);

            //Assert
            Assert.AreEqual("D3", controller.CurrentRider.ResultClassId);
        }
    }
}