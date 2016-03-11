using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFactory
{
	public class IceCreamFactory
	{
		public static IIceCream Create(IceCreamFlavours flavour)
		{
			switch (flavour)
			{
				case IceCreamFlavours.Choclate:
					return new ChocolateIceCream();
				case IceCreamFlavours.Vanilla:
					return new VanillaIceCream();
				case IceCreamFlavours.Butterscotch:
					return new ButterscotchIceCream();
			}
			return null;
		}
	}
}
