using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MSMQTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageQueue mq;
            if (MessageQueue.Exists(@".\Private$\Test"))
                //creates an instance MessageQueue, which points 
                //to the already existing MyQueue
                mq = new System.Messaging.MessageQueue(@".\Private$\Test");
            else
                //creates a new private queue called MyQueue 
                mq = MessageQueue.Create(@".\Private$\Test");

            System.Messaging.Message mm = new System.Messaging.Message();
            mm.Body = "Hello from Console";
            mm.Label = "test";
            mm.Recoverable = true;
            
            mq.Send(mm);
        }
    }
}
