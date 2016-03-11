using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryMethod
{
    public class ButterscotchFactory : Creator
    {
        public override IIceCream Create()
        {
            return new ButterscotchIceCream();
        }
    }
}
