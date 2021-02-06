using Capstone.Models.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    /// <summary>
    /// A slot in the vending machine. This holds a stack of items to sell
    /// </summary>
    /// <seealso cref="System.Collections.Generic.Stack{Capstone.Models.VendingMachineItem}" />
    public class VendingMachineSlot : Stack<VendingMachineItem>
    {
        public const string DISPLAY_QUANTITY_SOLD_OUT = "SOLD OUT";
        #region Properties
        public int Capacity { get; } = 5;

        public int QuantitySold { get { return this.Capacity - this.Count; } }


        /// <summary>
        /// Gets a string that represents the quantity of items remaining in this slot. If there are no more items, returns "SOLD OUT"
        /// </summary>
        public string QuantityRemainingDisplayString
        {
            get
            {
                if (this.Count == 0)
                {
                    return DISPLAY_QUANTITY_SOLD_OUT;
                }
                return this.Count.ToString();
            }
        }

        /// <summary>
        /// The Price of the item that is being sold in this slot.
        /// </summary>
        public decimal Price { get; }

        /// <summary>
        /// The name of the item that was initially loaded into this slot
        /// </summary>
        public string ItemName { get; } = "";

        /// <summary>
        /// Gets the category of the VendingMachineItem held inside this slot (i.e. Gum, Drink, Candy etc...) or null if the slot is empty
        /// </summary>
        public string ItemCategory
        {
            get
            {
                if (this.Count == 0)
                {
                    return null;
                }
                else
                {
                    return this.Peek().GetType().Name;
                }
            }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new VendingMachineSlot Object 
        /// </summary>
        /// <param name="itemName">The Name of the item (i.e. Hershey's, Snickers, etc.) </param>
        /// <param name="price">The price.</param>
        /// <param name="itemCategory">The item category (i.e "Candy", "Gum" etc.).</param>
        public VendingMachineSlot(string itemName, decimal price, string itemCategory)
        {
            #region invalid data checking
            //first, make sure that a subclass of type itemCatergory exists via spooky arcane type reflection voodoo
            Type itemType = Type.GetType("Capstone.Models.VendingMachineItems." + itemCategory);//try to get the className of itemCategory
            if (itemType == null || !itemType.IsSubclassOf(typeof(VendingMachineItem)))
            {
                throw new InvalidTypeException($"Invalid Item Category! {itemCategory} is not a subclass of VendingMachineItem");
            }
            #endregion

            //lets populate this vendingMachineSlot with items!
            for (int i = 0; i < Capacity; i++)
            {
                //create a new item (Hershey bar, snickers, sprite etc... to add to this instance of vendingMachineSlot
                VendingMachineItem newItem = (VendingMachineItem)Activator.CreateInstance(itemType, itemName);
                this.Push(newItem);
            }

            //set the price
            this.Price = price;

            //set the true item name
            this.ItemName = itemName;
        }

        #endregion

        public override string ToString()
        {
            return $"{this.QuantityRemainingDisplayString}|{this.Price:c}|{this.ItemName}";
        }
    }
}
