using Capstone.Models;
using Capstone.Models.Coins;
using Capstone.Models.CustomExceptions;
using MenuFramework;
using System;
using System.Collections.Generic;
using System.Text;
using static Capstone.Models.Coins.Coin;

namespace Capstone.CLI
{

    class PurchaseMenu : ConsoleMenu
    {
        private VendingMachine machine;

        /// <summary>
        /// Allows the customer to feed money and purchase items.
        /// </summary>
        /// <param name="machine">The vending machine</param>
        public PurchaseMenu(VendingMachine machine)
        {
            this.machine = machine;
            AddOption("Feed Money", FeedMoneyMenu, "F");
            AddOption("Select Product", SelectProduct, "S");
            AddOption("Finish Transaction and receive change", GetChange, "X");
            Configure(cfg =>
            {
                cfg.Title = "Purchase Menu";
            });
        }

        /// <summary>
        /// Shows the user how much credit they currently have.
        /// </summary>
        protected override void OnBeforeShow()
        {
            base.OnBeforeShow();
            Console.WriteLine($"Current Credit: {this.machine.CurrentCredit:c}");
        }

        /// <summary>
        /// Opens a submenu where the user may feed in credits.
        /// </summary>
        private MenuOptionResult FeedMoneyMenu()
        {
            FeedMoneyMenu feedMoney = new FeedMoneyMenu(this.machine);
            feedMoney.Show();
            return MenuOptionResult.DoNotWaitAfterMenuSelection;
        }

        /// <summary>
        /// Prompts the customer to enter their selection
        /// </summary>
        private MenuOptionResult SelectProduct()
        {
            foreach (string itemLine in this.machine.GetInventory())
            {
                Console.WriteLine(itemLine);
            }
            Console.WriteLine();
            string selection = GetString("Please enter your selection: ").ToUpper();
            VendingMachineItem item = null;
            try
            {
                item = this.machine.DispenseItem(selection);
                Console.WriteLine(item.EatMessage);
                Console.WriteLine($"Enjoy your {item.Name}!");
            }
            catch (KeyNotFoundException)//invalid item selected
            {
                Console.WriteLine($"Slot {selection} Does not exist!");
            }
            catch (InvalidOperationException)//item sold out
            {
                Console.WriteLine($"Sorry, slot {selection} is SOLD OUT! Please pick something else.");
            }
            catch (InsufficientFundsException)//not enough money
            {
                Console.WriteLine($"That item costs {machine.Slots[selection].Price:c}. " +
                    $"You only have {this.machine.CurrentCredit:c}. Please insert more money!");
            }

            return MenuOptionResult.WaitAfterMenuSelection;
        }

        /// <summary>
        /// Gets the change.
        /// </summary>
        private MenuOptionResult GetChange()
        {
            //print out the change
            Console.WriteLine("Thank you for shopping with us today! Here is your change:");
            Dictionary<CoinGroup, List<Coin>> change = this.machine.GiveChange();
            foreach (CoinGroup group in change.Keys)
            {
                Console.WriteLine($"Quantity of {group}: {change[group].Count}");
            }
            return MenuOptionResult.WaitThenCloseAfterSelection;
        }
    }
}
