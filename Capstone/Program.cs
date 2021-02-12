using Capstone.CLI;
using Capstone.Models;
using Capstone.Models.Coins;
using Capstone.Models.VendingMachineItems;
using System;
using System.Collections.Generic;
using static Capstone.Models.Coins.Coin;

namespace Capstone
{
    class Program
    {

        /****************************************************************************************
         * Notes on this Capstone solution:
         *      This solution contains both a project for the Vending Machine program (Capstone)
         *      and a project for tests (CapstoneTests). The Test project already references the
         *      Capstone project, so all you need to do is add Test Classes and Test Methods.
         *      
         *      ConsoleMenuFramework has been added via Nuget, so the project is ready to derive
         *      new menus. There is already a sample menu in the CLI folder. You can rename this 
         *      one, or create a new one to get started.
         * 
         * *************************************************************************************/
        static void Main(string[] args)
        {
            // You may want to create some objects to get the whole process started here...
            VendingMachine machine = new VendingMachine();
            List<string> stockFileLines = machine.ReadStockFile();
            machine.Restock(stockFileLines);//restock the machine
            // Some objects could be passed into the menu constructor, so that the menu has something to 
            // perform its actions against....
            MainMenu mainMenu = new MainMenu(machine);
            mainMenu.Show();
        }
    }
}
