using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FactoryMethod
{
    class ChocolateIceCream : IIceCream
    {
        public void Taste()
        {
            Console.WriteLine("It tastes like chocolate");
        }
    }
}
