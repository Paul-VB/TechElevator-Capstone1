using Capstone.Models;
using Capstone.Models.CustomExceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapstoneTests
{
    [TestClass]
    public class VendingMachineSlot_Tests
    {
        [DataTestMethod]
        [DataRow("Snickers", 4.20, "Candy")]
        [DataRow("Doritos", 0.69, "Chip")]
        [DataRow("Sprite", 2.23, "Drink")]
        [DataRow("Spearmint", 20.00, "Gum")]
        public void Constructor_Test(string itemName, double price, string itemCatagory)
        {
            //arrange
            decimal priceAsDecimal = (decimal)price;
            //act
            VendingMachineSlot slot = new VendingMachineSlot(itemName, priceAsDecimal, itemCatagory);

            //assert
            Assert.AreEqual(priceAsDecimal, slot.Price, "The price of the slot was not set correctly. Check the constructor!");

        }
        [DataTestMethod]
        [ExpectedException(typeof(InvalidTypeException))]
        [DataRow("Snuuckers", 4.20, "Khandie")]
        [DataRow("Splonks", 0.69, "Chyps")]
        [DataRow("Spip", 2.23, "Dronk")]
        [DataRow("Bri'ish Chips", 17.76, "Crisps")]
        [DataRow("StinkyGum", 20.00, "Gummmm")]
        public void InvalidConstructor_Test(string itemName, double price, string itemCatagory)
        {
            //arrange
            decimal priceAsDecimal = (decimal)price;
            //act
            VendingMachineSlot slot = new VendingMachineSlot(itemName, priceAsDecimal, itemCatagory);


        }

        [TestMethod]
        public void ItemName_Test()
        {
            //arrange
            VendingMachineSlot slot = new VendingMachineSlot("Hershey's", 2.00m, "Candy");

            //act

            //assert
            Assert.AreEqual("Hershey's", slot.ItemName);
        }

        [TestMethod]
        public void ItemName_SoldOut_Test()
        {
            //arrange
            VendingMachineSlot slot = new VendingMachineSlot("Hershey's", 2.00m, "Candy");

            //act
            while (slot.Count > 0)
            {
                slot.Pop();//empty the slot
            }

            //assert
            Assert.AreEqual(VendingMachineSlot.SOLDOUTNAME, slot.ItemName);


        }
    }
}
