using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models.VendingMachineItems
{
    public class Gum : VendingMachineItem
    {
        public override string EatMessage { get { return "Chew Chew, Yum!"; } }

        public Gum(string name) : base(name)
        {

        }
    }
}
