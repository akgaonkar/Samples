using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFCounter
{
    public class MyCounter : INotifyPropertyChanged
    {
        public int Count { get; set; }

        public void Increment() 
        {
            Count++;
        }

        public ICommand MyCommand { get; set; }

        public MyCounter()
        {
            MyCommand = new MyCommand(this);
            PropertyChanged(this, new PropertyChangedEventArgs("Count"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
