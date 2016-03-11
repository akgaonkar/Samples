using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFMVVMSample.Models;
using System.Windows.Input;

namespace WPFMVVMSample.ViewModels
{
    class CustomerViewModel : INotifyPropertyChanged
    {
        private Customer obj = new Customer();
        public RelayCommand ocommand;

        public CustomerViewModel()
        {
            ocommand = new RelayCommand(CalculateTax,Isvalid);
        }

        private bool Isvalid()
        {
            if (obj.Amount < 0) return false;
            else return true;
        }

        //TODO:changed this to a property.
        public ICommand BtnClick 
        {
            get { return ocommand; }
            
        }

        public string TxtCustomerName
        {
            get { return obj.CustomerName; }
            set 
            { 
                obj.CustomerName = value;
               
            }
        }        

        public string TxtAmount
        {
            get { return Convert.ToString(obj.Amount) ; }
            set 
            { 
                obj.Amount = Convert.ToDouble(value);

                Refresh("LblAmountColor");
                ocommand.Refresh();
                
            }
        }

        private void Refresh(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string LblAmountColor
        {
            get 
            {
                if (obj.Amount > 2000)
                {
                    return "Blue";
                }
                else if (obj.Amount > 1500)
                {
                    return "Red";
                }
                return "Yellow";
            }
        }

        public bool IsMarried
        {
            get
            {
                if (obj.Married == "Married")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set 
            {
                if (value)                
                    obj.Married = "Married";                
                else 
                    obj.Married = "NotMarried";
            }

        }

        public string txtTax { get { return obj.Tax.ToString(); } }

        public void CalculateTax() 
        {
            obj.CalculateTax();
            Refresh("txtTax");
        
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    #region | ICommand |
    class RelayCommand : ICommand
    {
        private Action WhatToExecute;
        private Func<bool> WhenItCanBeExected;

        public RelayCommand(Action What, Func<bool> When)
        {
            WhatToExecute = What;
            WhenItCanBeExected = When;
        }
        public bool CanExecute(object parameter)
        {
            return WhenItCanBeExected();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            WhatToExecute();
        }

        public void Refresh() 
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
#endregion
       
}
