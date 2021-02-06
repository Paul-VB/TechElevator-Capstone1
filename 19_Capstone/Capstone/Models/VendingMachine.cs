﻿using Capstone.Models.Coins;
using Capstone.Models.CustomExceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Capstone.Models.Coins.Coin;

namespace Capstone.Models
{
    public class VendingMachine
    {
        //the path to the CSV file that holds the info on what items and prices to restock the machine with
        const string STOCKFILEPATH = @"..\..\..\..\vendingmachine.csv";

        //the path to the file where all events are logged
        const string AUDITFILEPATH = @"..\..\..\..\Log.txt";
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
        /// The current balance of credit the user has. 
        /// This will be increased by the TakeMoney() method, 
        /// and should be decreased when they purchase something
        /// </summary>
        public Decimal CurrentCredit { get; private set; }

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
        public List<string> ReadStockFile(string pathToStockFile = STOCKFILEPATH)
        {
            decimal startCredit = CurrentCredit;
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

            LogToAuditFile("READ STOCK FILE", startCredit, CurrentCredit);
            return returnList;
        }


        /// <summary>
        /// Accepts a list of strings, and builds VendingMachineSlot objects from each string them. Each string should be formatted as follows <br></br>
        /// slotName|itemName|slotPrice|itemCategory <br></br>
        /// This method should commonly be used in conjunction with the ReadStockFile method
        /// </summary>
        /// <param name="stockLines">a list of strings representing the vendingMachineSlots</param>
        public void Restock(List<string> stockLines)
        {
            decimal startCredit = CurrentCredit;
            //loop though each item in the stockLines
            foreach (string currLine in stockLines)
            {
                /* Using currLine, build a vendingMachineSlot object
                 * currLine is a string. its going to look something like this:
                 * A1|Hershey's|2.50|Candy
                 * "A1" represents the slot identifier thing
                 * "Hershey's" is the name of the food item that will be sold in the VendingMachineSlot
                 * 2.50 is the price
                 * "Candy" is the food item's category 
                */

                string[] slotAttributes;
                //slotAttributes will contain something like ["A1", "Hershey's", 2.50, "Candy"]
                string name;
                decimal price;
                string category;
                VendingMachineSlot slot;
                try
                {
                    slotAttributes = currLine.Split("|");
                    name = slotAttributes[1];
                    price = decimal.Parse(slotAttributes[2]);
                    category = slotAttributes[3];
                    //build a new VendingMachineSlot object
                    slot = new VendingMachineSlot(name, price, category);
                }
                catch (InvalidTypeException)
                {
                    LogToAuditFile("BAD STOCK FILE LINE: the category in the stock file may have been misspelled", startCredit, CurrentCredit);
                    continue;
                }
                catch (NullReferenceException)
                {
                    LogToAuditFile("BAD STOCK FILE LINE: (this really shouldn't be possible, but the stock file line was null)", startCredit, CurrentCredit);
                    continue;
                }
                catch (FormatException)
                {
                    LogToAuditFile("BAD STOCK FILE LINE: Is the price an actual number?", startCredit, CurrentCredit);
                    continue;
                }
                catch (IndexOutOfRangeException)
                {
                    LogToAuditFile("BAD STOCK FILE LINE: Is there the correct number of vertical bars in the line?", startCredit, CurrentCredit);
                    continue;
                }


                /* add that object to our dictionary of slots
                 * A1 is the key
                 * the new slot object is the value
                 */
                this.slots.Add(slotAttributes[0], slot);
            }
        }


        /// <summary>
        /// Gets the current inventory of the vending machine as a list of strings
        /// </summary>
        /// <returns>A list of strings representing each Vending machine slot</returns>
        public List<string> GetInventory()
        {
            List<string> returnlist = new List<string>();

            foreach (KeyValuePair<string, VendingMachineSlot> kvp in this.slots)
            {
                returnlist.Add($"{kvp.Key}|{kvp.Value.ToString()}");
            }
            return returnlist;
        }




        public void TakeMoney(decimal moneyToTake)
        {
            decimal startCredit = CurrentCredit;
            this.CurrentCredit += moneyToTake;
            LogToAuditFile($"FEED MONEY:", startCredit, CurrentCredit);
        }

        /// <summary>
        /// Tries to dispense a vending machine item out of the vendingMachineSlot indicated
        /// </summary>
        /// <param name="slotIdentifier">The name of the VendingMachineSlot to take from (i.e. "A1", "B1", "C3" etc..)</param>
        /// <returns></returns>
        public VendingMachineItem DispenseItem(string slotIdentifier)
        {
            decimal startCredit = CurrentCredit;
            slotIdentifier = slotIdentifier.ToUpper();
            try
            {
                VendingMachineSlot slot = this.slots[slotIdentifier];
                //check if they have enough money
                if (this.CurrentCredit < slot.Price)
                {
                    throw (new InsufficientFundsException(message: "The customer does not have enough credit to purchase this item"));
                }
                VendingMachineItem itemToDispense = slot.Pop();//try to get the item
                this.CurrentCredit -= slot.Price;
                LogToAuditFile($"SUCCESSFUL PURCHASE: {itemToDispense.Name} {slotIdentifier}", startCredit, CurrentCredit);

                return itemToDispense;
            }
            catch (KeyNotFoundException e)//thrown if the slot not exists
            {
                LogToAuditFile($"SLOT NOT FOUND: {slotIdentifier}", startCredit, CurrentCredit);
                throw e;
            }
            catch (InvalidOperationException e)//thrown if the slot is empty
            {
                LogToAuditFile($"ITEM SOLD OUT: {slotIdentifier}", startCredit, CurrentCredit);
                throw e;
            }
            catch (InsufficientFundsException e)//thrown if not enough money
            {
                LogToAuditFile($"INSUFFICIENT FUNDS: {this.slots[slotIdentifier].Peek().Name} {slotIdentifier}", startCredit, CurrentCredit);
                throw e;
            }
        }

        /// <summary>
        /// Gives the customer their change using the fewest number of coins possible
        /// </summary>
        /// <returns>A Dictionary of coinNames and coins</returns>
        public Dictionary<CoinGroup, List<Coin>> GiveChange()
        {
            decimal startCredit = CurrentCredit;
            //the dictionary of coinGroups and Coins we will return.
            Dictionary<CoinGroup, List<Coin>> change = new Dictionary<CoinGroup, List<Coin>>();

            //get a sorted list of all coinGroups that exist. We need largest to smallest.
            List<CoinGroup> sortedGroups = new List<CoinGroup>((CoinGroup[])Enum.GetValues(typeof(CoinGroup)));
            sortedGroups.Sort();
            sortedGroups.Reverse();


            //for each type of coin that exists (i.e. Pennies, Nickels, Dimes etc...
            foreach (CoinGroup currCoinGroup in sortedGroups)
            {
                //get the true dollar amount for the current CoinGroup
                decimal trueValue = (decimal)(int)currCoinGroup / 100; //Start right, start here

                //initialize the coinGroups list
                change[currCoinGroup] = new List<Coin>();

                //calculate the number of coins we will need
                int numbOfCoins = (int)(this.CurrentCredit / trueValue);//yo dawg i heard you like coins

                //keep adding coins of the current type until we have reached numbOfCoins
                for (int i = 0; i < numbOfCoins; i++)
                {
                    change[currCoinGroup].Add(new Coin(currCoinGroup));
                    this.CurrentCredit -= trueValue;
                }
            }
            LogToAuditFile($"GIVE CHANGE:", startCredit, CurrentCredit);
            return change;
        }

        /// <summary>
        /// Logs events to the audit file
        /// </summary>
        /// <param name="eventDescription">The event description.</param>
        /// <param name="startCredit">The start credit.</param>
        /// <param name="endCredit">The end credit.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for 
        private void LogToAuditFile(string eventDescription, decimal startCredit, decimal endCredit)
        {
            using (StreamWriter writer = new StreamWriter(AUDITFILEPATH, true))
            {
                string logEvent = $"{DateTime.Now} \t {eventDescription} \t {startCredit:c} \t {endCredit}";
                writer.WriteLine(logEvent);
            }
        }


        #endregion

        #region UI Direct print methods
        /// <summary>
        /// Prints the vending machine's inventory to the console with fancy formatting and pretty colors
        /// </summary>
        public void PrintInventory()
        {
            //get the current color so we can restore it when we're done
            ConsoleColor oldColor = Console.ForegroundColor;

            //the color of "normal" text we want to show to users
            ConsoleColor normalColor = ConsoleColor.Gray;

            //the color of text of items that are sold out.
            ConsoleColor soldOutColor = ConsoleColor.Red;

            //the color of text of items that are almost sold out. aka "buy now while you still can" to get people to spend more money
            ConsoleColor lowStockColor = ConsoleColor.Yellow;


            Console.ForegroundColor = normalColor;
            Console.WriteLine("--------------------------------------");
            Console.WriteLine(" Slot | Remaining |  Cost  | Item Name");
            Console.WriteLine("------+-----------+--------+----------");
            foreach (KeyValuePair<string, VendingMachineSlot> kvp in this.slots)
            {
                string slotKey = kvp.Key;
                int remaining = kvp.Value.Count;
                decimal cost = kvp.Value.Price;
                string itemName = kvp.Value.ItemName;
                //Prints each line piece by piece with fancy formatting
                //print slot tag, (A1, B2 etc...)
                Console.ForegroundColor = normalColor;
                if (remaining == 0) { Console.ForegroundColor = soldOutColor; }
                else if (remaining == 1) { Console.ForegroundColor = lowStockColor; }
                Console.Write(string.Format("{0,4}  ", slotKey));
                Console.ForegroundColor = normalColor;
                Console.Write("|");

                //print quantity remaining
                if (remaining == 0) { Console.ForegroundColor = soldOutColor; }
                else if (remaining == 1) { Console.ForegroundColor = lowStockColor; }
                Console.Write(string.Format(" {0,9} ", remaining));
                Console.ForegroundColor = normalColor;
                Console.Write("|");

                //print price
                if (remaining == 0) { Console.ForegroundColor = soldOutColor; }
                else if (remaining == 1) { Console.ForegroundColor = lowStockColor; }
                Console.Write(string.Format(" {0,6:c} ", cost));
                Console.ForegroundColor = normalColor;
                Console.Write("|");

                //print item name
                if (remaining == 0) { Console.ForegroundColor = soldOutColor; }
                else if (remaining == 1) { Console.ForegroundColor = lowStockColor; }
                Console.Write(string.Format(" {0,-15}", itemName));
                Console.ForegroundColor = normalColor;

                //write new line
                Console.Write("\n");

                //restore the previous color
                Console.ForegroundColor = oldColor;
            }
            Console.WriteLine("------+-----------+--------+----------");
            Console.WriteLine(" Slot | Remaining |  Cost  | Item Name");
            Console.WriteLine("--------------------------------------");
        }

        /// <summary>
        /// Prints the customer's current credit to the console with fancy formatting and pretty colors.
        /// </summary>
        public void PrintCredit()
        {
            ConsoleColor veryLowCreditColor = ConsoleColor.Red;
            ConsoleColor lowCreditColor = ConsoleColor.Yellow;
            ConsoleColor goodCreditColor = ConsoleColor.Green;
            decimal veryLowCreditLevel = 1.00m;
            decimal lowCreditLevel = 2.00m;
            //get the current color so we can restore it when we're done
            ConsoleColor oldForegroundColor = Console.ForegroundColor;
            ConsoleColor oldBackgroundColor = Console.BackgroundColor;
            Console.Write("Your Remaining Credit: ");
            if (this.CurrentCredit < veryLowCreditLevel)
            {
                Console.ForegroundColor = veryLowCreditColor;
            }
            else if (this.CurrentCredit < lowCreditLevel)
            {
                Console.ForegroundColor = lowCreditColor;
            }
            else
            {
                Console.ForegroundColor = goodCreditColor;
            }


            Console.WriteLine($"{this.CurrentCredit:c}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.ForegroundColor = oldForegroundColor;
            Console.BackgroundColor = oldBackgroundColor;
        }
        #endregion



    }
}
