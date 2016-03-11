using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorModel
{
    public class CalculatorModel
    {

        public double OperandOne { get; set; }
        public double OperandTwo { get; set; }
        public double Result { get; private set; }

        public void Add() 
        {
            Result = OperandOne + OperandTwo;
        }
        public void Sub()
        {
            Result = OperandOne - OperandTwo;
        }
        public void Mul()
        {
            Result = OperandOne * OperandTwo;
        }
        public void Div()
        {
            Result = OperandOne / OperandTwo;
        }
    }
}
