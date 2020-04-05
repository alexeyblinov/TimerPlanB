using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanB.BL.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanB.BL.Controller.Tests
{
    [TestClass()]
    public class TimemachineControllerTests
    {
        [TestMethod()]
        public void TimemachineControllerTest()
        {
            // Arrange
            var rnd = new Random();
            var min = rnd.Next(1, 59);
            var sec = rnd.Next(1, 59);
            var hun = rnd.Next(1, 99);

            //Act
            var controller = new TimemachineController(min, sec, hun);
            var result = controller.HundredthsValue;

            //Assert
            Assert.AreEqual(min * 6000 + sec * 100 + hun, result);
        }
    }
}