using Capstone.Models;
using Capstone.Models.VendingMachineItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapstoneTests
{
    [TestClass]
    public class VendingMachineItem_Tests
    {
        [TestMethod]
        public void EatMessage_Chip_Test()
        {
            //arrange
            VendingMachineItem testItem = new Chip("Doritos");

            //act
            string eatMessage = testItem.EatMessage;

            //assert
            Assert.AreEqual("Crunch Crunch, Yum!", eatMessage);
        }
        [TestMethod]
        public void EatMessage_Candy_Test()
        {
            //arrange
            VendingMachineItem testItem = new Candy("Snickers");

            //act
            string eatMessage = testItem.EatMessage;

            //assert
            Assert.AreEqual("Munch Munch, Yum!", eatMessage);
        }
        [TestMethod]
        public void EatMessage_Drink_Test()
        {
            //arrange
            VendingMachineItem testItem = new Drink("Sprite");

            //act
            string eatMessage = testItem.EatMessage;

            //assert
            Assert.AreEqual("Glug Glug, Yum!", eatMessage);
        }
        [TestMethod]
        public void EatMessage_Gum_Test()
        {
            //arrange
            VendingMachineItem testItem = new Gum("Spearmint");

            //act
            string eatMessage = testItem.EatMessage;

            //assert
            Assert.AreEqual("Chew Chew, Yum!", eatMessage);
        }
    }
}
