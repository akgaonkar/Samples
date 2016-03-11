namespace Builder
{
    internal interface IPizzaBuilder
    {
        void BuildBase();

        void BuildSize();

        void BuildSause();

        void BuildToppings();

        void BuildExtras();

        public Pizza GetPizza { get; }
    }
}