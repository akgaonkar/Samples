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
        public int Count { get; private set; }

        public void Increment() 
        {
            Count++;
            PropertyChanged(this, new PropertyChangedEventArgs("Count"));
        }

        private MyCommand commandCounter;

        public MyCommand myCommand 
        {
            get 
            {
                return commandCounter;
            }
            set 
            {
                commandCounter = value;

            }
        }

        public MyCounter()
        {
            myCommand = new MyCommand(this);

            
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
