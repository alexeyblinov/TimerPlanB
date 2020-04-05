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
        [TestMethod()]
        public void ChangeRankTest()
        {
            //Arrange
            var rnd = new Random();
            int startNumber = rnd.Next(1, 99);
            string name = Guid.NewGuid().ToString();
            string surname = Guid.NewGuid().ToString();
            var gender = "M";
            string location = Guid.NewGuid().ToString();
            string team = Guid.NewGuid().ToString();
            var lapTime = rnd.Next(0, Rider.MAXTIME - 4);
            var penalty = rnd.Next(0, 3);

            // Act
            var controller1 = new RiderController(startNumber, "D2");
            startNumber = rnd.Next(1, 99);
            var controller2 = new RiderController(startNumber, "D3");

            controller1.SetNewRiderData(name, surname, gender, location, team);

            name = Guid.NewGuid().ToString();
            surname = Guid.NewGuid().ToString();
            location = Guid.NewGuid().ToString();
            team = Guid.NewGuid().ToString();
            controller2.SetNewRiderData(name, surname, gender, location, team);

            RaceController.ChangeRank(controller1, controller1.CurrentRider, lapTime, penalty);
            lapTime = rnd.Next(0, Rider.MAXTIME - 4);
            penalty = rnd.Next(0, 3);
            RaceController.ChangeRank(controller2, controller2.CurrentRider, lapTime, penalty);

            var best = controller1.CurrentRider.BestResult <= controller2.CurrentRider.BestResult;
            var rank = controller1.CurrentRider.Rank < controller2.CurrentRider.Rank;

            //Assert
            Assert.AreEqual(best, rank);
        }

        [TestMethod()]
        public void SetNewPlacesTest()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod()]
        public void FindCompetitionClassIdTest()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod()]
        public void SetNewClassesTest()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}