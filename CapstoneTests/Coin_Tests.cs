using Capstone.Models.Coins;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapstoneTests
{
    [TestClass]
    public class Coin_Tests
    {
        [TestMethod]
        public void Coin_Penny_Test()
        {
            //arrange
            Coin penny = new Coin(Coin.CoinTypes.Penny);

            //act
            decimal resultValue = penny.Value;

            //assert
            Assert.AreEqual(0.01m, resultValue);

        }

        [TestMethod]
        public void Coin_Nickel_Test()
        {
            //arrange
            Coin nickel = new Coin(Coin.CoinTypes.Nickel);

            //act
            decimal resultValue = nickel.Value;

            //assert
            Assert.AreEqual(0.05m, resultValue);

        }

        [TestMethod]
        public void Coin_Dime_Test()
        {
            //arrange
            Coin dime = new Coin(Coin.CoinTypes.Dime);

            //act
            decimal resultValue = dime.Value;

            //assert
            Assert.AreEqual(0.10m, resultValue);

        }

        [TestMethod]
        public void Coin_Quarter_Test()
        {
            //arrange
            Coin quarter = new Coin(Coin.CoinTypes.Quarter);

            //act
            decimal resultValue = quarter.Value;

            //assert
            Assert.AreEqual(0.25m, resultValue);

        }
    }
}
