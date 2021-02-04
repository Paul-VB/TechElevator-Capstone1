using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models.VendingMachineItems
{
    class Drink : VendingMachineItem
    {
        public override string EatMessage { get { return "Glug Glug, Yum!"; } }

        public Drink(string name) : base(name)
        {

        }

    }
}
