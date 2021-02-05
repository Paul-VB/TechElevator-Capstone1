using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models.Coins
{

    public class Coin 
    {
        /// <summary>
        /// Represents all the types of coins that exist. The values are in <i>CENTS</i>, not <i>DOLLARS</i> <br></br>
        /// (I.E. Penny = 1, Nickel = 5, etc..) Divide by 100 to get the true dollar amount.
        /// </summary>
        public enum CoinGroup
        {
            Penny = 1,
            Nickel = 5,
            Dime = 10,
            Quarter = 25


        }

        public CoinGroup Group { get; }
        public decimal Value { get { return (int)this.Group / 100; } }

        public Coin(CoinGroup group)
        {
            this.Group = group;
        }
    }
}
