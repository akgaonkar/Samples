using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleFactory
{
    class VanillaIceCream : IIceCream
    {
        public void Taste()
        {
            Console.WriteLine("It tastes like vanilla");
        }
    }
}
