using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Threading;

namespace usart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string[] coms = portSearch();
            foreach (string com in coms)
            {
                if (portOn(com))
                {
                    break;
                }
            }
            textBaud.Text = serialPort1.BaudRate.ToString();
            textBit.Text = serialPort1.DataBits.ToString();
        }
        private string[] portSearch()
        {
            comboPort.Items.Clear();
            comboPort.Enabled = true;
            string[] coms = SerialPort.GetPortNames();
            if (coms.Length != 0)
            {
                comboPort.Items.AddRange(coms);
                comboPort.SelectedItem = coms[0];
            }
            return coms;
        }
        private bool portOn(string portName)
        {

            string[] coms = portSearch();
            if (coms.Contains<string>(portName))
            {
                serialPort1.PortName = portName;
                serialPort1.BaudRate = 38400;
                //Console.WriteLine(serialPort1.Encoding);
                try
                {
                    serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    serialPort1.DiscardOutBuffer();
                }
                catch
                {
                    portOff(portName);
                    return false;
                }
                serialPort1.DiscardInBuffer();
                serialPort1.DiscardOutBuffer();
                if (serialPort1.IsOpen)
                {
                    comboPort.SelectedItem = portName;
                    COM.Text = "ON";
                    COM.ForeColor = Color.Green;

                    comboPort.Enabled = false;
                    
                    return true;
                }
            }
            return false;
        }
        private bool portOff(string portName)
        {

            serialPort1.Close();
            if (!serialPort1.IsOpen)
            {
                comboPort.SelectedItem = portName;
                COM.Text = "OFF";
                COM.ForeColor = Color.Red;
                comboPort.Enabled = true;
                return true;
            }
            return false;
        }

        private void comboPort_DropDown(object sender, EventArgs e)
        {
            portSearch();
        }


        private void textBaud_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                serialPort1.BaudRate = int.Parse(textBaud.Text);
                Terminal.Text += "(system) BaudRate is changed\n";
            }
        }

        private void textBit_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                serialPort1.DataBits = int.Parse(textBit.Text);
                Terminal.Text += "(system) DataBit is changed\n";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                serialPort1.Write(textWrite.Text);
            }
            Terminal.Text += "<<"+textWrite.Text+"\n";
            textWrite.Clear();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Terminal.Clear();
            textWrite.Clear();
        }

        private void binaryClear_Click(object sender, EventArgs e)
        {
            textBinary.Clear();
;        }
        private void COM_Click(object sender, EventArgs e)
        {
            if (comboPort.SelectedItem != null)
            {
                if (COM.Text == "OFF")
                {
                    portOn(comboPort.SelectedItem.ToString());
                }
                else if (COM.Text == "ON")
                {
                    portOff(comboPort.SelectedItem.ToString());
                }
            }
        }
        private void textWrite_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write(textWrite.Text);
                }
                Terminal.Text += "<<"+textWrite.Text+"\n";

                textWrite.Clear();
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(5);  //（毫秒）等待一定時間，確保資料的完整性 int len        
            int len = serialPort1.BytesToRead;
            string receivedata = string.Empty;
            byte[] buffs;
            if (len != 0)
            {
                buffs = new byte[len];
                serialPort1.Read(buffs, 0, len);
                receivedata = Encoding.UTF8.GetString(buffs);
                if (Terminal.InvokeRequired)
                {
                    //如果需要invoke
                    //step 1. 建立一個delegate方法
                    Action updateMethod = new Action(() => Terminal.AppendText(receivedata));

                    //step 2. 交給元件Invoke去執行delegate方法
                    Terminal.Invoke(updateMethod);
                }
                else
                {
                    //如果不需要，那就直接更新元件吧
                    Terminal.AppendText(receivedata);
                }

                StringBuilder s = new StringBuilder();
                foreach (byte i in buffs)
                {
                    s.Append(i.ToString("X2")).Append(" ");
                }
                
                if (textBinary.InvokeRequired)
                {
                    //如果需要invoke
                    //step 1. 建立一個delegate方法
                    Action updateMethod = new Action(() => textBinary.AppendText(s.ToString()));

                    //step 2. 交給元件Invoke去執行delegate方法
                    textBinary.Invoke(updateMethod);
                }
                else
                {
                    //如果不需要，那就直接更新元件吧
                    textBinary.AppendText(receivedata);
                }
            }
            

            //Console.WriteLine(receivedata);


            //serialPort1.DiscardInbuffser();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                serialPort1.Dispose();
            }
        }
    }
}
