using Capstone.Models;
using MenuFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.CLI
{
    public class MainMenu : ConsoleMenu
    {
        private VendingMachine machine;
        /*******************************************************************************
         * Private data:
         * Usually, a menu has to hold a reference to some type of "business objects",
         * on which all of the actions requested by the user are performed. A common 
         * technique would be to declare those private fields here, and then pass them
         * in through the constructor of the menu.
         * ****************************************************************************/

        // NOTE: This constructor could be changed to accept arguments needed by the menu
        public MainMenu(VendingMachine machine)
        {
            this.machine = machine;//initialize the vending machine


            // Add Sample menu options
            AddOption("Display Vending Machine Items", DisplayItems, "D");
            AddOption("Purchase Items", PurchaseMenu, "P");
            AddOption("Quit", Close, "Q");

            Configure(cfg =>
           {
               cfg.ItemForegroundColor = ConsoleColor.Cyan;
               cfg.SelectedItemForegroundColor = ConsoleColor.Yellow;
               cfg.MenuSelectionMode = MenuSelectionMode.Arrow; // KeyString: User types a key, Arrow: User selects with arrow
               cfg.KeyStringTextSeparator = ": ";
               cfg.Title = "Main Menu";
           });
        }



        private MenuOptionResult DisplayItems()
        {
            foreach(string itemLine in this.machine.GetInventory())
            {
                Console.WriteLine(itemLine);
            }
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult PurchaseMenu()
        {
            PurchaseMenu purchaseMenu = new PurchaseMenu(this.machine);
            purchaseMenu.Show();
            return MenuOptionResult.DoNotWaitAfterMenuSelection;
        }
    }
}
