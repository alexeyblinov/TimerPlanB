using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanB.BL.Controller;
using PlanB.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanB.BL.Controller.Tests
{
    [TestClass()]
    public class RaceControllerTests
    {
        static Random rnd = new Random();
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
            RiderController controller1 = new RiderController();
            RiderController controller2 = new RiderController();
            var bestTime1 = 0;
            var bestTime2 = 0;
            int bestResult = Rider.MAXTIME;
            string compClass2;

            //Act
            for (var i = 0; i < 2; i++)
            {
                startNumber = rnd.Next(1, 99);
                controller1 = new RiderController(startNumber, "C2");

                name = Guid.NewGuid().ToString();
                surname = Guid.NewGuid().ToString();
                location = Guid.NewGuid().ToString();
                team = Guid.NewGuid().ToString();
                controller1.SetNewRiderData(name, surname, gender, location, team);

                lapTime = rnd.Next(0, Rider.MAXTIME - 4);
                penalty = rnd.Next(0, 3);
                RaceController.ChangeRank(controller1, controller1.CurrentRider, lapTime, penalty);
                lapTime = rnd.Next(0, Rider.MAXTIME - 4);
                penalty = rnd.Next(0, 3);
                RaceController.ChangeRank(controller1, controller1.CurrentRider, lapTime, penalty);
            }

            for (var i = 0; i < 3; i++)
            {
                startNumber = rnd.Next(1, 99);
                controller2 = new RiderController(startNumber, "D4");

                name = Guid.NewGuid().ToString();
                surname = Guid.NewGuid().ToString();
                location = Guid.NewGuid().ToString();
                team = Guid.NewGuid().ToString();
                controller2.SetNewRiderData(name, surname, gender, location, team);

                lapTime = rnd.Next(0, Rider.MAXTIME - 4);
                penalty = rnd.Next(0, 3);
                RaceController.ChangeRank(controller2, controller2.CurrentRider, lapTime, penalty);
                lapTime = rnd.Next(0, Rider.MAXTIME - 4);
                penalty = rnd.Next(0, 3);
                RaceController.ChangeRank(controller2, controller2.CurrentRider, lapTime, penalty);
            }

            RaceController.SetNewPlaces(controller1);
            RaceController.SetNewPlaces(controller2);

            compClass2 = RaceController.FindCompetitionClassId(controller2, ref bestTime2);

            foreach (var r in controller2.Riders)
            {
                if (r.BestResult < bestResult)
                {
                    bestResult = r.BestResult;
                }
            }

            //Assert
            Assert.IsNull(RaceController.FindCompetitionClassId(controller1, ref bestTime1));
            Assert.AreEqual(bestTime1, 0);

            Assert.AreEqual(compClass2, "D4");
            Assert.AreEqual(bestTime2, bestResult);
        }

        [TestMethod()]
        public void SetNewClassesTest()
        {
            //Arrange
            var controller = new RiderController(1, "N");
            string bestClass;
            var bestTime = 0;

            //Act
            controller.SetNewRiderData(name, surname, gender, location, team);

            for(int i = 0; i < 3; i++)
            {
                controller = new RiderController(i+2, "C3");
                name = Guid.NewGuid().ToString();
                surname = Guid.NewGuid().ToString();
                location = Guid.NewGuid().ToString();
                team = Guid.NewGuid().ToString();
                controller.SetNewRiderData(name, surname, gender, location, team);
            }
            RaceController.ChangeRank(controller, controller.Riders[0], 6147, 0);
            RaceController.ChangeRank(controller, controller.Riders[1], 5792, 0);
            RaceController.ChangeRank(controller, controller.Riders[2], 5830, 0);
            RaceController.ChangeRank(controller, controller.Riders[3], 5000, 0);
            RaceController.SetNewPlaces(controller);
            
            bestClass = RaceController.FindCompetitionClassId(controller, ref bestTime);
            RaceController.SetNewClasses(controller, bestClass, bestTime);

            //Assert
            Assert.AreEqual(controller, controller.Riders[3].ResultClassId, "D3");
        }
    }
}