using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models.Coins
{
    /// <summary>
    /// Each instance of coin represents a literal, physical coin
    /// </summary>
    public class Coin 
    {
        /// <summary>
        /// Represents all the types of coins that exist. The values are in <i>CENTS</i>, not <i>DOLLARS</i><br></br>
        /// (I.E. Penny = 1, Nickel = 5, etc..) Divide by 100 to get the true dollar amount.
        /// </summary>
        public enum CoinTypes
        {

            Penny = 1,
            Nickel = 5,
            Dime = 10,
            Quarter = 25
            //HalfDollar = 50
        }

        /// <summary>
        /// What type of coin is this specific instance of Coin? (Penny, Nickel, Dime, etc...).
        /// </summary>
        public CoinTypes CoinType { get; }
        /// <summary>
        /// The Dollar Value of this Coin Object. (i.e. Penny = 0.01, Nickel = 0.05, Dime = 0.10 etc.)
        /// </summary>
        public decimal Value { get { return (int)this.CoinType / 100.0m; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Coin" /> class.
        /// </summary>
        /// <param name="coinType">The type of coin.</param>

        public Coin(CoinTypes coinType)
        {
            this.CoinType = coinType;
        }
    }
}
