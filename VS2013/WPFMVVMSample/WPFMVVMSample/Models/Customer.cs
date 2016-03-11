using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFMVVMSample.Models
{
    class Customer
    {
        public string CustomerName { get; set; }
        public double Amount { get; set; }
        public string Married { get; set; }
        public double Tax { get; private set; }

        public void CalculateTax() 
        {
            Tax = Amount * .1;
        }
    }
}
