using Capstone.Models;
using MenuFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.CLI
{
    class FeedMoneyMenu : ConsoleMenu
    {
        private readonly VendingMachine machine;
        /// <summary>
        /// This menu lets the customer feed in money.
        /// </summary>
        /// <param name="machine">The Vending Machine currently being used</param>
        public FeedMoneyMenu(VendingMachine machine)
        {
            this.machine = machine;
            List<decimal> validDollarAmounts = new List<decimal>()
            {
                1.00m, 2.00m, 5.00m, 10.00m
            };
            AddOptionRange<decimal>(validDollarAmounts, FeedMoney);
            AddOption("Go Back", Close, "C");
            Configure(cfg =>
            {
                cfg.Title = "Please Insert Money";
            });
        }

        protected override void OnBeforeShow()
        {
            base.OnBeforeShow();
            Console.WriteLine($"Current Credit: {this.machine.CurrentCredit:c}");
        }

        /// <summary>
        /// Feeds a given amount of money into the Vending Machine
        /// </summary>
        private MenuOptionResult FeedMoney(decimal amountToFeed)
        {
            this.machine.TakeMoney(amountToFeed);
            return MenuOptionResult.DoNotWaitAfterMenuSelection;
        }


    }
}
