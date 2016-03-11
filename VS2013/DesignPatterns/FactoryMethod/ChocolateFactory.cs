using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryMethod
{
    public class ChocolateFactory: Creator 
    {
        public override IIceCream Create()
        {
            return new ChocolateIceCream();
        }
    }
}
