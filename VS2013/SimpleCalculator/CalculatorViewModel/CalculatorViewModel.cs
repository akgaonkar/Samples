using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculatorModel;
using System.Windows.Input;


namespace Calculator.ViewModel
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private CalculatorModel.CalculatorModel model;// = new CalculatorModel.CalculatorModel();

        public event PropertyChangedEventHandler PropertyChanged;

        public string txtOperandOne 
        {
            get { return Convert.ToString(model.OperandOne); }
            set
            {
                model.OperandOne = Convert.ToDouble(value);
                Refresh("lblResult");
                ((RelayCommand)AddCommand).Refresh();
            }
        }
        public string txtOperandTwo 
        {
            get { return Convert.ToString(model.OperandTwo); }
            set
            {
                model.OperandTwo = Convert.ToDouble(value);
                Refresh("lblResult");
                ((RelayCommand)AddCommand).Refresh();
            }
        }
        public string lblResult 
        {
            get 
            {
                return Convert.ToString(model.Result);
            }            
        }

        public ICommand AddCommand { get; private set; }
        public ICommand SubCommand { get; private set; }
        public ICommand MulCommand { get; private set; }
        public ICommand DivCommand { get; private set; }


        public CalculatorViewModel()
        {
            model = new CalculatorModel.CalculatorModel();

            AddCommand = new RelayCommand(Add, Validate);
            SubCommand = new RelayCommand(Sub, Validate);
            MulCommand = new RelayCommand(Mul, Validate);
            DivCommand = new RelayCommand(Div, ValidateForDiv);
        }

        #region Commands
        public void Add() { model.Add(); Refresh("lblResult"); }
        public bool Validate()
        {
            if (txtOperandOne == "" || txtOperandTwo == "") return false;
            else return true;
        }
        public void Sub() { model.Sub(); Refresh("lblResult"); }
        public void Mul() { model.Mul(); Refresh("lblResult"); }
        public void Div() { model.Div(); Refresh("lblResult"); }
        public bool ValidateForDiv()
        {
            Validate();
            if ( Convert.ToDouble(txtOperandTwo) == 0.0) return false;
            else return true;
        }

        private void Refresh(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        

        #endregion
    }
}
