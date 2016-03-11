using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFCounter
{
    public class MyCommand : ICommand
    {
        MyCounter counter;

        public MyCommand(MyCounter _counter)
        {
            counter = _counter;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            counter.Increment();
        }
    }
        
}
