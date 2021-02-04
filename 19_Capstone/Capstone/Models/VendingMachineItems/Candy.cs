using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models.VendingMachineItems
{
    class Candy : VendingMachineItem
    {
        public override string EatMessage { get { return "Munch Munch, Yum!"; } }

        public Candy(string name) : base(name)
        {

        }
    }
}
