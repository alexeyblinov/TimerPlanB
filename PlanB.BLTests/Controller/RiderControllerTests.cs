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
    public class RiderControllerTests
    {
        [TestMethod()]
        public void SetNewRiderDataTest()
        {
            // Arrange
            var random = new Random();
            int startNumber = random.Next(1, 99);
            var name = Guid.NewGuid().ToString();
            var surname = Guid.NewGuid().ToString();
            var gender = "M";
            var location = Guid.NewGuid().ToString();
            var team = Guid.NewGuid().ToString();
            var classId = "D2";
            var controller = new RiderController(startNumber, classId);

            // Act
            var reController = new RiderController(startNumber, classId);
            reController.SetNewRiderData(name, surname, gender, location, team);

            // Assert
            Assert.AreEqual(startNumber, reController.CurrentRider.RiderId);
            Assert.IsNotNull(reController.CurrentRider.Name);
            Assert.IsNotNull(reController.CurrentRider.Surname);
            Assert.IsNotNull(reController.CurrentRider.Gender);
            Assert.IsNotNull(reController.CurrentRider.Location);
            Assert.IsNotNull(reController.CurrentRider.Team);
            Assert.IsNull(controller.CurrentRider.Name);
        }

        [TestMethod()]
        public void SaveTest()
        {
            // Arrange
            var random = new Random();
            int startNumber = random.Next(1, 99);
            var classId = "C1";

            // Act
            var controller = new RiderController(startNumber, classId);

            // Assert
            Assert.AreEqual(startNumber, controller.CurrentRider.RiderId);
            Assert.IsNull(controller.CurrentRider.Name);

        }
    }
}