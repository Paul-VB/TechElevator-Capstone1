using Capstone.Models;
using Capstone.Models.CustomExceptions;
using Capstone.Models.VendingMachineItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapstoneTests
{
    [TestClass]
    public class VendingMachineTests
    {
        [TestMethod]
        public void Restock_Test_GoodData()
        {
            //arrange
            List<string> stockLines = new List<string>()
            {
                "A1|M&Ms|3.05|Candy",
                "A2|Doritos|4.20|Chip",
                "B1|Sprite|2.75|Drink",
                "B2|Big Chew|3.65|Gum"
            };
            VendingMachine testMachine = new VendingMachine();

            //act
            testMachine.Restock(stockLines);

            //assert
            Assert.AreEqual(stockLines.Count, testMachine.Slots.Count,"Not all stockLines were added as slots!");
            Assert.AreEqual("M&Ms", testMachine.Slots["A1"].Peek().Name,"The item held in slot A1 is not called M&Ms!");
            Assert.AreEqual(3.05m, testMachine.Slots["A1"].Price, "The price of slot A1 is not set correctly");
            Assert.IsTrue(testMachine.Slots["A1"].Peek() is Candy,"The item held in slot A1 is not Candy!");

            Assert.AreEqual("Doritos", testMachine.Slots["A2"].Peek().Name, "The item held in slot A2 is not called Doritos!");
            Assert.AreEqual(4.20m, testMachine.Slots["A2"].Price, "The price of slot A2 is not set correctly");
            Assert.IsTrue(testMachine.Slots["A2"].Peek() is Chip, "The item held in slot A2 is not Chip!");

            Assert.AreEqual("Sprite", testMachine.Slots["B1"].Peek().Name, "The item held in slot B1 is not called Sprite! LeBron's gonna be mad");
            Assert.AreEqual(2.75m, testMachine.Slots["B1"].Price, "The price of slot B1 is not set correctly");
            Assert.IsTrue(testMachine.Slots["B1"].Peek() is Drink, "The item held in slot B1 is not a Drink!");

            Assert.AreEqual("Big Chew", testMachine.Slots["B2"].Peek().Name, "The item held in slot B2 is not called Big Chew!");
            Assert.AreEqual(3.65m, testMachine.Slots["B2"].Price, "The price of slot B2 is not set correctly");
            Assert.IsTrue(testMachine.Slots["B2"].Peek() is Gum, "The item held in slot B2 is not Gum!");

        }

        [TestMethod]
        public void Restock_Test_TypoInStockLine()
        {
            //arrange
            List<string> stockLines = new List<string>()
            {

                "A2|Doritos|4.20|CChips",
            };

            //act
            VendingMachine testMachine = new VendingMachine();

            //assert
            Assert.ThrowsException<InvalidTypeException>(() => testMachine.Restock(stockLines));//lambdas are weird, yo

        }
    }
}
