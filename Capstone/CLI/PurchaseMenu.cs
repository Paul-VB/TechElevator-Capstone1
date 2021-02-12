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

    /// <summary>
    /// The Purchase sub-menu for the Vending Machine
    /// </summary>
    /// <seealso cref="MenuFramework.ConsoleMenu" />
    class PurchaseMenu : ConsoleMenu
    {
        /// <summary>
        /// The Vending machine we will be working with.
        /// </summary>
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
                cfg.ItemForegroundColor = MainMenu.GlobalItemForegroundColor;
                cfg.SelectedItemForegroundColor = MainMenu.GlobalSelectedItemForegroundColor;
                cfg.Title = "Purchase Menu";
            });
        }

        protected override void OnBeforeShow()
        {
            MainMenu.DisplayLogo();
            this.machine.PrintCredit();
            base.OnBeforeShow();

        }



        /// <summary>
        /// Opens a submenu where the user may feed in credits.
        /// </summary>
        /// <returns></returns>
        private MenuOptionResult FeedMoneyMenu()
        {
            FeedMoneyMenu feedMoney = new FeedMoneyMenu(this.machine);
            feedMoney.Show();
            return MenuOptionResult.DoNotWaitAfterMenuSelection;
        }

        /// <summary>
        /// Prompts the customer to enter their selection
        /// </summary>
        /// <returns></returns>
        private MenuOptionResult SelectProduct()
        {
            //show the user information
            MainMenu.DisplayLogo();
            this.machine.PrintInventory();
            this.machine.PrintCredit();
            Console.WriteLine();

            //the customer's slot selection (i.e. A1, B2, C1 etc...)
            string selection = GetString("Please enter your selection, (leave empty to cancel): ",true).ToUpper();
            if(selection == "")
            {
                return MenuOptionResult.DoNotWaitAfterMenuSelection;
            }

            //the item we will try to return to the customer
            VendingMachineItem item = null;

            //get the current console color so we can restore it when we're done
            ConsoleColor oldForegroundColor = Console.ForegroundColor;
            ConsoleColor oldBackgroundColor = Console.BackgroundColor;

            ConsoleColor errorColor = ConsoleColor.Red;
            try
            {
                //try to purchase the item
                item = this.machine.DispenseItem(selection);
                //if the purchase was successful...
                //...redraw the screen. This will update the customer's credit and the inventory being displayed.
                Console.Clear();
                MainMenu.DisplayLogo();
                //reprint the updated inventory. One of the items will have 1 fewer
                this.machine.PrintInventory();

                //reprint the updated credit that the user has
                this.machine.PrintCredit();
                Console.WriteLine(item.EatMessage);
                Console.WriteLine($"Enjoy your {item.Name}! ");
                Console.WriteLine($"Cost: {this.machine.Slots[selection].Price:c}");
                //Console.WriteLine($"Your Remaining Credit: {this.machine.CurrentCredit:c}");
            }
            catch (KeyNotFoundException)//invalid item selected
            {
                Console.ForegroundColor = errorColor;
                Console.WriteLine("INVALID SELECTION");
                Console.ForegroundColor = oldForegroundColor;
                Console.WriteLine($"Slot {selection} Does not exist!");
            }
            catch (InvalidOperationException)//item sold out
            {
                Console.ForegroundColor = errorColor;
                Console.WriteLine("ITEM SOLD OUT");
                Console.ForegroundColor = oldForegroundColor;
                Console.WriteLine($"Sorry, slot {selection} is SOLD OUT! Please pick something else.");
            }
            catch (InsufficientFundsException)//not enough money
            {
                Console.ForegroundColor = errorColor;
                Console.WriteLine("INSUFFICIENT CREDIT");
                Console.ForegroundColor = oldForegroundColor;
                Console.WriteLine($"That item costs {machine.Slots[selection].Price:c}. " +
                    $"You only have {this.machine.CurrentCredit:c}. Please insert more money!");
                Console.WriteLine("Sorry, Link. I can't give credit. Come back when you're a little... mmmmm... richer!");
            }catch (SalesReportsUnlockedException)//sales reports menu unlocked
            {
                Console.ForegroundColor = errorColor;
                Console.WriteLine("Passcode Accepted. The Sales Reports option has been unlocked in the Main Menu.");
                Console.ForegroundColor = oldForegroundColor;
            }
            finally
            {
                Console.ForegroundColor = oldForegroundColor;
                Console.BackgroundColor = oldBackgroundColor;
            }

            return MenuOptionResult.WaitAfterMenuSelection;
        }

        /// <summary>
        /// Gets the change.
        /// </summary>
        /// <returns></returns>
        private MenuOptionResult GetChange()
        {
            MainMenu.DisplayLogo();
            //print out the change
            Console.WriteLine("Thank you for shopping with us today! Here is your change:");
            Dictionary<CoinTypes, List<Coin>> change = this.machine.GiveChange();
            foreach (CoinTypes group in change.Keys)
            {
                Console.WriteLine($"Quantity of {group}: {change[group].Count}");
            }
            return MenuOptionResult.WaitThenCloseAfterSelection;
        }
    }
}
