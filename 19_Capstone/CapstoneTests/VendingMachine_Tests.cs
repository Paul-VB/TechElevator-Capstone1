using Capstone.Models;
using Capstone.Models.Coins;
using Capstone.Models.CustomExceptions;
using Capstone.Models.VendingMachineItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using static Capstone.Models.Coins.Coin;

namespace CapstoneTests
{
    [TestClass]
    public class VendingMachine_Tests
    {
        //create an arbitrary list of strings
        private List<string> sampleStockFileLines = new List<string>()
            {
                "A1|M&Ms|3.05|Candy",
                "A2|Doritos|4.20|Chip",
                "B1|Sprite|2.75|Drink",
                "B2|Big Chew|3.65|Gum"
            };
        [TestMethod]
        public void Restock_Test_GoodData()
        {
            //arrange
            List<string> stockLines = new List<string>(sampleStockFileLines);
            VendingMachine testMachine = new VendingMachine();

            //act
            testMachine.Restock(stockLines);//restock from the arbitrary list

            //assert
            Assert.AreEqual(stockLines.Count, testMachine.Slots.Count, "Not all stockLines were added as slots!");
            Assert.AreEqual("M&Ms", testMachine.Slots["A1"].Peek().Name, "The item held in slot A1 is not called M&Ms!");
            Assert.AreEqual(3.05m, testMachine.Slots["A1"].Price, "The price of slot A1 is not set correctly");
            Assert.IsTrue(testMachine.Slots["A1"].Peek() is Candy, "The item held in slot A1 is not Candy!");

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
        public void Restock_Test_MalformedStocklines()
        {
            //arrange
            List<string> stockLines = new List<string>()
            {
                "A1|M&Ms|3.05|Candy",//good
                "A2|Doritos|4.20|Chip",//good
                "B1|Sprite|2.75|Drink",//good
                "B2|Spearmint|0.95|gum",//good! the item type is not case sensitive!
                "B3|Big Chew|3.65q|Gum",//bad from here down
                "C1|Snickers||1.69|Candy",
                "D1Pepsi|3.00|Drink",
                "D2|Lays|2.50|CChip",
                "break damnit!",
                "",
                "||||||||||",
                null


            };
            VendingMachine testMachine = new VendingMachine();

            //act
            testMachine.Restock(stockLines);

            //assert
            Assert.AreEqual(3, testMachine.Slots.Count, "All the correctly spelled stock lines were not added, OR the misspelled stock lines were added");


        }

        [TestMethod]
        public void GetInventory_Test()
        {
            List<string> expectedInventory = new List<string>()
            {
                "A1|3|$3.05|M&Ms",
                "A2|4|$4.20|Doritos",
                $"B1|{VendingMachineSlot.DISPLAY_QUANTITY_SOLD_OUT}|$2.75|Sprite",
                "B2|5|$3.65|Big Chew"
            };
            //arrange
            VendingMachine testMachine = new VendingMachine();
            testMachine.Restock(sampleStockFileLines);
            testMachine.TakeMoney(40.00m);

            //sell 2 M&Ms
            testMachine.DispenseItem("A1");
            testMachine.DispenseItem("A1");
            //sell 1 Doritos
            testMachine.DispenseItem("A2");
            //sell all sprite
            while (testMachine.Slots["B1"].Count > 0)
            {
                testMachine.DispenseItem("B1");
            }

            //act
            List<string> resultInventory = testMachine.GetInventory();

            //assert
            CollectionAssert.AreEquivalent(expectedInventory, resultInventory);




        }

        [TestMethod]
        public void DispenseItem_Test()
        {
            //arrange
            string slotTag = "B1";
            VendingMachine testMachine = new VendingMachine();
            testMachine.Restock(new List<string>(sampleStockFileLines));//restock from arbitrary list

            testMachine.TakeMoney(100.00m);

            //act
            VendingMachineItem soda = testMachine.DispenseItem(slotTag);

            //assert
            Assert.IsTrue(soda is Drink, "The machine dispensed the wrong item!");
            Assert.AreEqual("Sprite", soda.Name, "The Machine dispensed a drink, but it was not the right drink!");
            Assert.AreEqual(97.25m, testMachine.CurrentCredit, "The price of the soda was not deducted from the current Credit!");
            Assert.AreEqual(testMachine.Slots[slotTag].Capacity - 1, testMachine.Slots[slotTag].Count,
                $"The quantity of items in slot {slotTag} didn't change after we tried to dispense");

        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void DispenseItem_Test_SlotNotFound()
        {
            //arrange
            string slotTag = "z9";
            VendingMachine testMachine = new VendingMachine();
            testMachine.Restock(new List<string>(sampleStockFileLines));
            testMachine.TakeMoney(100.00m);

            //act
            VendingMachineItem soda = testMachine.DispenseItem(slotTag);

        }

        [TestMethod]
        [ExpectedException(typeof(SalesReportsUnlockedException))]
        public void DispenseItem_Test_SalesMenuUnlocked()
        {
            //arrange
            string slotTag = VendingMachine.SALES_REPORTS_PASSCODE;
            VendingMachine testMachine = new VendingMachine();
            testMachine.Restock(new List<string>(sampleStockFileLines));
            testMachine.TakeMoney(100.00m);

            //act
            VendingMachineItem soda = testMachine.DispenseItem(slotTag);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientFundsException))]
        public void DispenseItem_Test_InsufficientFunds()
        {
            //arrange
            string slotTag = "B2";
            VendingMachine testMachine = new VendingMachine();
            testMachine.Restock(new List<string>(sampleStockFileLines));

            //act
            VendingMachineItem soda = testMachine.DispenseItem(slotTag);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DispenseItem_Test_ItemSoldOut()
        {
            //arrange
            string slotTag = "B1";
            VendingMachine testMachine = new VendingMachine();
            testMachine.Restock(new List<string>(sampleStockFileLines));
            testMachine.TakeMoney(40.00m);
            //act
            while (testMachine.Slots[slotTag].Count > 0)
            {
                testMachine.DispenseItem(slotTag);
            }

            //act
            VendingMachineItem soda = testMachine.DispenseItem(slotTag);

        }

        [TestMethod]
        public void GenerateSalesReport_Test()
        {
            //arrange
            VendingMachine testMachine = new VendingMachine();
            testMachine.Restock(new List<string>(sampleStockFileLines));
            testMachine.TakeMoney(50.00m);
            //sell 2 M&Ms
            testMachine.DispenseItem("A1");
            testMachine.DispenseItem("A1");
            //sell 1 Doritos
            testMachine.DispenseItem("A2");
            //sell 5 sprites, thirsty boys
            testMachine.DispenseItem("B1");
            testMachine.DispenseItem("B1");
            testMachine.DispenseItem("B1");
            testMachine.DispenseItem("B1");
            testMachine.DispenseItem("B1");
            //sell no gum

            List<string> expectedSalesReportLines = new List<string>()
            {
                "M&Ms|2",
                "Doritos|1",
                "Sprite|5",
                "Big Chew|0",
                "",
                "$24.05"
            };
            //act
            List<string> resultSalesReportLines = testMachine.GenerateSalesReport();

            //assert
            CollectionAssert.AreEquivalent(expectedSalesReportLines, resultSalesReportLines);
        }

        [TestMethod]
        //"Data rows"
        public void GiveChange_Test_Series()
        {
            GiveChange_TestHelper(
                0.41m,
                new Dictionary<CoinGroup, int>()
                {
                    { CoinGroup.Penny,1 },
                    { CoinGroup.Nickel,1 },
                    { CoinGroup.Dime,1 },
                    { CoinGroup.Quarter,1 }
                },
                DisplayName: "One of each coin");

            GiveChange_TestHelper(
                0.57m,
                new Dictionary<CoinGroup, int>()
                {
                    { CoinGroup.Penny,2 },
                    { CoinGroup.Nickel,1 },
                    { CoinGroup.Dime,0 },
                    { CoinGroup.Quarter,2 }
                },
                DisplayName: "2P, 1N, 0D, 2Q");

            GiveChange_TestHelper(
                0.69m,
                new Dictionary<CoinGroup, int>()
                {
                    { CoinGroup.Penny,4 },
                    { CoinGroup.Nickel,1 },
                    { CoinGroup.Dime,1 },
                    { CoinGroup.Quarter,2 }
                },
                DisplayName: "4P, 1N, 1D, 2Q");

            GiveChange_TestHelper(
                4.20m,
                new Dictionary<CoinGroup, int>()
                {
                    { CoinGroup.Penny,0 },
                    { CoinGroup.Nickel,0 },
                    { CoinGroup.Dime,2 },
                    { CoinGroup.Quarter,16 }
                },
                    DisplayName: "0P, 0N, 2D, 16Q");//reverse coinstar


        }

        private void GiveChange_TestHelper(decimal startingCredit, Dictionary<CoinGroup, int> expectedChange, string DisplayName = "")
        {
            //arrange
            VendingMachine testMachine = new VendingMachine();
            testMachine.TakeMoney(startingCredit);

            //act
            Dictionary<CoinGroup, List<Coin>> resultChange = testMachine.GiveChange();

            //assert
            CollectionAssert.AreEquivalent(expectedChange.Keys, resultChange.Keys,
                $"{DisplayName} test failed!: The resulting change does not have all the correct Coin Types!");
            foreach (CoinGroup group in expectedChange.Keys)
            {
                Assert.AreEqual(expectedChange[group], resultChange[group].Count, $"{DisplayName} test failed!: Quantity of {group} is incorrect!");
            }
            Assert.AreEqual(0, testMachine.CurrentCredit, "There is still credit left in the machine! the customer didn't get all their change back!");

        }
    }
}
