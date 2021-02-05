using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Models
{
    public class VendingMachine
    {
        #region Properties
        /// <summary>
        /// a private backing field for Slots
        /// </summary>
        private Dictionary<string, VendingMachineSlot> slots = new Dictionary<string, VendingMachineSlot>();

        /// <summary>
        /// The slots of the vending machine. They Keys will be the labels of the slot ("A1", "A2", "B1" etc.) 
        /// The Keys will be the slots themselves<br></br>
        /// We always get a "clone" of the backing field so nobody can do something like .Clear() on our slots and break the machine
        /// </summary>
        public Dictionary<string, VendingMachineSlot> Slots { get { return new Dictionary<string, VendingMachineSlot>(this.slots); } }

        /// <summary>
        /// The current balance of credit the user has. This will be increased by the TakeMoney() method.
        /// </summary>
        public Decimal CurrentCredit { get; }
        #endregion

        #region Constructors
        public VendingMachine()
        {

        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Reads the lines from a given file path, and returns those lines as a List of strings
        /// </summary>
        /// <param name="pathToStockFile">The path to the input file.</param>
        /// <returns>The lines of the file as a List of strings</returns>
        public List<string> ReadStockFile(string pathToStockFile)
        {
            //the list we will return
            List<string> returnList = new List<string>();

            using (StreamReader rdr = new StreamReader(pathToStockFile))
            {

                while (!rdr.EndOfStream)
                {
                    string line = rdr.ReadLine();

                    returnList.Add(line);
                }

            }
            return returnList;
        }


        /// <summary>
        /// Accepts a list of strings, and builds VendingMachineSlot objects from each string them. Each string should be formatted as follows <br></br>
        /// slotName|itemName|slotPrice|itemCategory
        /// </summary>
        /// <param name="stockLines">a list of strings representing the vendingMachineSlots</param>
        public void Restock(List<string> stockLines)
        {
            //loop though each item in the stockLines
            foreach (string currLine in stockLines)
            {
                /*using currLine, build a vendingMachineSlot object
                 * currLine is a string. its going to look something like this:
                 * A1|Hershey's|2.50|Candy
                 * "A1" represents the slot identifier thing
                 * "Hershey's" is the name of the food item that will be sold in the VendingMachineSlot
                 * 2.50 is the price
                 * "Candy" is the food item's category 
                */

                //currLine holds A1|Hershey's|2.50|Candy

                string[] slotAttributes = currLine.Split("|");
                //slotAttributes will contain ["A1", "Hershey's", 2.50, "Candy"]
                string name = slotAttributes[1];
                decimal price = decimal.Parse(slotAttributes[2]);
                string category = slotAttributes[3];
                //build a new VendingMachineSlot object
                VendingMachineSlot slot = new VendingMachineSlot(name, price, category);

                /* add that object to our dictionary of slots
                 * A1 is the key
                 * the new slot object is the value
                 */
                this.slots.Add(slotAttributes[0], slot);
            }
        }
        #endregion


    }
}
