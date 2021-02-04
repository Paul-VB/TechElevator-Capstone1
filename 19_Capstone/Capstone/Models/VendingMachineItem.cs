using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    /// <summary>
    /// The parent class to all items we will sell in the vending machine
    /// </summary>
    public abstract class VendingMachineItem
    {
        /// <summary>
        /// The name of the item (i.e. Hershey's, Sprite, Snickers etc).
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// The Message the user should see after purchasing this item.
        /// </summary>
        public abstract string EatMessage { get; }

        public VendingMachineItem(string name)
        {
            this.Name = name;
        }
    }

}
