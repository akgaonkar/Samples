using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DelegateCalculator
{
    public class Calculator
    {
        public double OperandOne { get; set; }
        public double OperandTwo { get; set; }

        public delegate double Operation(double one, double two);
        private Operation _operation;
        public event Operation Opr;
        public  delegate void _result(string str);
        public event _result ResultGenerated;
        public delegate string HugeTask();
        public HugeTask HTask;
        public Operation SetOperation
        {
            private get { return _operation; }
            set { _operation += value; }
        }
      
        public double Result {
            get
            {
                var ret = SetOperation.Invoke(OperandOne, OperandTwo);              
                if (Opr != null)
                {
                    ret = Opr(OperandOne, OperandTwo);
                }
                if (ResultGenerated != null) 
                    ResultGenerated(ret.ToString());
                return ret;
            }
        }

    }
    public class ConcreteOperations
    {
        public double Add(double a, double b)
        {
            return a - b;
        }

        public double PrintAddOnConsole(double a, double b) 
        {
            Console.WriteLine(Add(a, b));
            return 0.0;
        }
        public double ShowAddOnMessageBox(double a, double b)
        {
            MessageBox.Show((Add(a, b).ToString()));
            return 0.0;
        }
        public string HugeOperation()
        {
            Thread.Sleep(3000);
            return "huge task finished";
        }
    }
}
