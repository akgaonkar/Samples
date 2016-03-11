using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SmsClient;

namespace SMS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SendSms sms = new SendSms();
            string status = sms.send(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
            if (status == "1")
            {
                MessageBox.Show("Message Send");
            }
            else if (status == "2")
            {
                MessageBox.Show("No Internet Connection");
            }
            else
            {
                MessageBox.Show("Invalid Login Or No Internet Connection");
            }
        }
    }
}
