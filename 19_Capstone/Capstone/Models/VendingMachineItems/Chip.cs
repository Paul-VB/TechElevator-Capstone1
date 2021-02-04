using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models.VendingMachineItems
{
    public class Chip : VendingMachineItem
    {
        public override string EatMessage { get { return "Crunch Crunch, Yum!"; } }

        public Chip(string name) : base (name)
        {

        }
    }

}
