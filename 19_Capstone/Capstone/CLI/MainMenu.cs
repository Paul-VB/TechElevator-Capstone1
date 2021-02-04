using MenuFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.CLI
{
    public class MainMenu : ConsoleMenu
    {
        /*******************************************************************************
         * Private data:
         * Usually, a menu has to hold a reference to some type of "business objects",
         * on which all of the actions requested by the user are performed. A common 
         * technique would be to declare those private fields here, and then pass them
         * in through the constructor of the menu.
         * ****************************************************************************/

        // NOTE: This constructor could be changed to accept arguments needed by the menu
        public MainMenu()
        {
            // Add Sample menu options
            AddOption("Greeting", Greeting, "G");
            AddOption("Show the Time", GetTime, "T");
            AddOption("Quit", Close, "Q");

            Configure(cfg =>
           {
               cfg.ItemForegroundColor = ConsoleColor.Cyan;
               cfg.MenuSelectionMode = MenuSelectionMode.KeyString; // KeyString: User types a key, Arrow: User selects with arrow
               cfg.KeyStringTextSeparator = ": ";
               cfg.Title = "Main Menu";
           });
        }

        private MenuOptionResult GetTime()
        {
            Console.WriteLine($"The time is {DateTime.Now}");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult Greeting()
        {
            string name = GetString("What is your name? ");
            Console.WriteLine($"Hello, {name}!");
            return MenuOptionResult.WaitAfterMenuSelection;
        }
    }
}
