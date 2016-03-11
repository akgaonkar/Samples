using System;

namespace Builder
{
    internal class ItalianPizzaBuilder : IPizzaBuilder
    {
        public ItalianPizzaBuilder()
        {
        }

        public void BuildBase()
        {
            throw new NotImplementedException();
        }

        public void BuildSize()
        {
            throw new NotImplementedException();
        }

        public void BuildSause()
        {
            throw new NotImplementedException();
        }

        public void BuildToppings()
        {
            throw new NotImplementedException();
        }

        public void BuildExtras()
        {
            throw new NotImplementedException();
        }

        public Pizza GetPizza
        {
            get;
            private set;
        }
    }
}