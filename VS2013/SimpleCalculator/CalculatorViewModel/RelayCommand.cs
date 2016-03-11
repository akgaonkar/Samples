using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calculator
{

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
