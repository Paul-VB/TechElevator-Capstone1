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
        #region Properties
        /// <summary>
        /// The string that will be displayed for the vending machine's quantity remaining when there are no items left
        /// </summary>
        public const string DISPLAY_QUANTITY_SOLD_OUT = "SOLD OUT";

        /// <summary>
        /// The max number of <see cref="VendingMachineItem"/>s that this vending machine slot can hold.
        /// </summary>
        public int Capacity { get; } = 5;

        /// <summary>
        /// Gets the quantity of items sold from this Vending Machine Slot
        /// </summary>
        public int QuantitySold { get { return this.Capacity - this.Count; } }

        /// <summary>
        /// Gets a string that represents the quantity of items remaining in this slot. If there are no more items, returns "SOLD OUT"
        /// </summary>
        /// <value>
        /// The quantity remaining display string.
        /// </value>
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
        /// <value>
        /// The price.
        /// </value>
        public decimal Price { get; }

        /// <summary>
        /// The name of the item that was initially loaded into this slot
        /// (i.e. Hershey's, Snickers, Sprite, etc.)
        /// </summary>
        /// <value>
        /// The name of the item.
        /// </value>
        public string ItemName { get; } = "";

        /// <summary>
        /// Gets the category of the VendingMachineItem held inside this slot (i.e. Gum, Drink, Candy etc...) or null if the slot is empty
        /// </summary>
        /// <value>
        /// The item category.
        /// </value>
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
        /// <param name="itemName">The Name of the item (i.e. Hershey's, Snickers, etc.)</param>
        /// <param name="price">The price.</param>
        /// <param name="itemCategory">The item category (i.e "Candy", "Gum" etc.).</param>
        /// <exception cref="InvalidTypeException">Invalid Item Category! {itemCategory} is not a subclass of VendingMachineItem</exception>
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

        /// <summary>
        /// Gets a string representation of this VendingMachineSlot
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.QuantityRemainingDisplayString}|{this.Price:c}|{this.ItemName}";
        }
    }
}
