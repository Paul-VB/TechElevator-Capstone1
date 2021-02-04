using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class VendingMachine
    {
        /// <summary>
        /// The slots of the vending machine. They Keys will be the labels of the slot ("A1", "A2", "B1" etc.) 
        /// The Keys will be the slots themselves
        /// </summary>
        private Dictionary<string, VendingMachineSlot> slots = new Dictionary<string, VendingMachineSlot>();

        /// <summary>
        /// The current balance of credit the user has. This will be increased by the TakeMoney() method.
        /// </summary>
        public Decimal CurrentCredit { get; }


        /// <summary>
        /// Reads the lines from a given file path, and returns those lines as a List of strings
        /// </summary>
        /// <param name="pathToStockFile">The path to the input file.</param>
        /// <returns>The lines of the file as a List of strings</returns>
        public List<string> ReadStockFile(string pathToStockFile)
        {
            //the list we will return
            List<string> returnList = new List<string>();

            //do things

            return returnList;
        }


    }
}
