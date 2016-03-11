using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleFactory;
using FactoryMethod;
using DelegateCalculator;

namespace DesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            ////Simple Factory
            //var icecream = IceCreamFactory.Create(IceCreamFlavours.Butterscotch);
            //icecream.Taste();

            ////Factory Method
            //Creator factory = new ChocolateFactory();
            //var icecream2 = factory.Create();
            //icecream2.Taste();

            ////Abstract Factory


            //Delegate
            //Calculator calc = new Calculator();
            //calc.OperandOne = 2;
            //calc.OperandTwo = 3;
            //calc.SetOperation = new ConcreteOperations().Add;
            //Console.WriteLine(calc.Result);


            ////Multicast delegate
            //Calculator mcalc = new Calculator();
            //mcalc.OperandOne = 2;
            //mcalc.OperandTwo = 3;
            //var con = new ConcreteOperations();
            //mcalc.SetOperation = con.PrintAddOnConsole;
            //mcalc.SetOperation = con.ShowAddOnMessageBox;
            //var result = mcalc.Result;
            
            ////Events
            //Calculator ecalc = new Calculator();
            //ecalc.OperandOne = 2;
            //ecalc.OperandTwo = 3;
            //ecalc.SetOperation = new ConcreteOperations().Add;
            //ecalc.ResultGenerated += Console.WriteLine;
            //var result = ecalc.Result;

            ////Multi event
            //Calculator mecalc = new Calculator();
            //mecalc.OperandOne = 2;
            //mecalc.OperandTwo = 3;
            //mecalc.SetOperation = new ConcreteOperations().Add;
            //mecalc.Opr += mecalc_Opr;

            //Console.WriteLine(mecalc.Result.ToString());
            
            //Async delegates
            Calculator ascalc = new Calculator();
            ascalc.OperandOne = 2;
            ascalc.OperandTwo = 3;
            var asCon = new ConcreteOperations();
            ascalc.HTask += asCon.HugeOperation;
            ascalc.HTask.BeginInvoke(Message, ascalc.HTask);
            


            Console.ReadLine();


        }

        static double mecalc_Opr(double one, double two)
        {
            return 100.0;//one + two;
        }
        static void Message(IAsyncResult message)
        {
            var del = (Calculator.HugeTask)message.AsyncState;
            Console.WriteLine(del.EndInvoke(message));
        }
        
    }
}
