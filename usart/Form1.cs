﻿using System;
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
using System.Text.RegularExpressions;
using programmer;
namespace usart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // serialPort1.Encoding = Encoding.UTF8;
            string[] coms = portSearch();
            // comboPort_DropDown(this, new EventArgs());
            foreach (string com in coms)
            {
                if (portOn(com, 38400, 8))
                {
                    break;
                }
            }
            textBaud.Text = serialPort1.BaudRate.ToString();
            textBit.Text = serialPort1.DataBits.ToString();
            RTerminal.SelectedIndex = 0;
        }
        private string[] portSearch()
        {
            comboPort.Items.Clear();
            ProgPort.Items.Clear();
            comboPort.Enabled = true;
            string[] coms = SerialPort.GetPortNames();
            if (coms.Length != 0)
            {
                comboPort.Items.AddRange(coms);
                comboPort.SelectedItem = coms[0];
                ProgPort.Items.AddRange(coms);
                ProgPort.SelectedItem = coms[0];
            }
            return coms;
        }
        private bool portOn(string portName, int baud, int databit)
        {

            string[] coms = portSearch();
            if (coms.Contains<string>(portName))
            {
                serialPort1.PortName = portName;
                serialPort1.BaudRate = baud;
                serialPort1.DataBits = databit;
                serialPort1.Encoding = Encoding.UTF8;
                //Console.WriteLine(serialPort1.Encoding);
                try
                {
                    serialPort1.Open();
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
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
                    ProgPort.SelectedItem = portName;
                    comboPort.SelectedItem = portName;

                    COM.Text = "ON";
                    COM.ForeColor = Color.Green;

                    ProgPort.Enabled = false;
                    comboPort.Enabled = false;
                    textBaud.Enabled = false;
                    textBit.Enabled = false;
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
                ProgPort.SelectedItem = portName;
                comboPort.SelectedItem = portName;
                COM.Text = "OFF";
                COM.ForeColor = Color.Red;

                ProgPort.Enabled = true;
                comboPort.Enabled = true;
                textBaud.Enabled = true;
                textBit.Enabled = true;
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
            if (e.KeyCode == Keys.Enter)
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

        private void terminal_enter_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(textWrite.Text);
            }
            Terminal.Text += "<<" + textWrite.Text + "\r\n";
            textWrite.Clear();

        }

        private void termina_clear_Click(object sender, EventArgs e)
        {
            Terminal.Clear();
            textWrite.Clear();
        }

        private void binaryClear_Click(object sender, EventArgs e)
        {
            textBinary.Clear();

        }
        private void COM_Click(object sender, EventArgs e)
        {
            if (comboPort.SelectedItem != null)
            {
                if (COM.Text == "OFF")
                {
                    portOn(comboPort.SelectedItem.ToString(), int.Parse(textBaud.Text), int.Parse(textBit.Text));
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
                Terminal.Text += "<<" + textWrite.Text + "\r\n";

                textWrite.Clear();
            }
        }
        enum right_terminal_state
        {
            ASAHMI = 0,
            HEX = 1
        }
        ASADecode decode = new ASADecode();
        ASAEncode encode = new ASAEncode();
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Regex rx = new Regex(@"~G[AMS],");  //檢查是否有HMI sync format封包
            Thread.Sleep(15);  //（毫秒）等待一定時間，確保資料的完整性 int len
            if (!serialPort1.IsOpen)
            {
                return;
            }
            byte[] buffs;
            int len = serialPort1.BytesToRead;

            if (len != 0)
            {
                buffs = new byte[len];
                serialPort1.Read(buffs, 0, len);
                List<byte> terminal_buffs = new List<byte>(buffs.Length);
                int i = 0;
                int rTerminalIndex = 0;
                if (RTerminal.InvokeRequired)
                {
                    Action updateMethod = new Action(() => rTerminalIndex = RTerminal.SelectedIndex);
                    Terminal.Invoke(updateMethod);
                }
                else
                {
                    rTerminalIndex = RTerminal.SelectedIndex;
                }
                foreach (var buff in buffs)
                {
                    var terminal_buff = decode.put(buff);
                    if (terminal_buff != 0)
                    {
                        terminal_buffs.Add(terminal_buff);
                        i++;
                    }

                    if (decode.putEnable && rTerminalIndex == (int)right_terminal_state.ASAHMI)
                    {
                        string text = decode.get();

                        if (textBinary.InvokeRequired)
                        {
                            Action updateMethod = new Action(() => textBinary.AppendText(text));
                            textBinary.Invoke(updateMethod);
                        }
                        else
                        {
                            Terminal.AppendText(text);
                        }
                    }
                }


                string receivedata = Encoding.UTF8.GetString(terminal_buffs.ToArray());
                var mt = rx.Match(receivedata);
                if (mt.Success)
                {
                    serialPort1.WriteLine("~ACK");
                }

                if (Terminal.InvokeRequired)
                {
                    Action updateMethod = new Action(() =>
                    {
                        Terminal.AppendText(receivedata);
                        if (mt.Success)
                        {
                            var idx = Terminal.Text.LastIndexOf("\n");
                            Terminal.Text = Terminal.Text.Insert(idx + 1, "<< ~ACK\r\n");
                        }
                    });
                    Terminal.Invoke(updateMethod);
                }
                else
                {
                    Terminal.AppendText(receivedata);
                    if (mt.Success)
                    {
                        var idx = Terminal.Text.LastIndexOf("\n");
                        Terminal.Text = Terminal.Text.Insert(idx + 1, "<< ~ACK\r\n");
                    }
                }

                if (rTerminalIndex == (int)right_terminal_state.HEX)
                {
                    string s = "";

                    foreach (byte buff in buffs)
                    {
                        s += buff.ToString("X2") + " ";
                    }
                    if (textBinary.InvokeRequired)
                    {
                        Action updateMethod = new Action(() => textBinary.AppendText(s));
                        textBinary.Invoke(updateMethod);
                    }
                    else
                    {
                        textBinary.AppendText(s);
                    }
                }
            }

        }

        private void pacSend_Click(object sender, EventArgs e)
        {
            var formats = encode.split(textBinary.Text);
            if (formats.Length > 1)
            {
                var resault = MessageBox.Show("multi-format detected, do you want to send all at ones?", "HMI warning", MessageBoxButtons.YesNoCancel);
                if (resault == DialogResult.Cancel)
                    return;
                else if (resault == DialogResult.No)
                {
                    var is_ready = encode.put(formats[0]);
                    if (!is_ready)
                    {
                        MessageBox.Show("HMI format incorrect!!", "HMI warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

            }
            //var is_ready = encode.put(textBinary.Text);
            //if (!is_ready)
            //{
            //    MessageBox.Show("HMI format incorrect!!", "HMI warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    //Terminal.Text += "( HMI format error!! )\r\n";
            //    return;
            //}
            var package = encode.get();
            if (Terminal.Text.EndsWith(""))
            {
                serialPort1.Write("~ACK");
            }
            serialPort1.Write(package);

            //int[] a = new int[] { 0, 0, 0 };
            ////int[] a = { 0, 0, 0 };
            //var str=Console.ReadLine();
            //var val = str.Split(' ');
            //for (int i=0;i<val.Length;i++)
            //{
            //    a[i]=int.Parse(val[i]);
            //}
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            serialPort1.Dispose();

        }


        private void RTerminal_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox1.Text = RTerminal.SelectedItem.ToString();
            textBinary.Text = "";
            pacSend.Visible = RTerminal.SelectedItem.ToString() == "ASAHMI";
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (tabControl1.SelectedIndex == 0)
            {
               serialPort1.DataReceived+= new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
                if (!serialPort1.IsOpen)
                {
                    serialPort1.Encoding = Encoding.UTF8;
                    portOn((string)comboPort.SelectedItem, int.Parse(textBaud.Text), int.Parse(textBit.Text));
                }

                textBaud.Text = serialPort1.BaudRate.ToString();
                textBit.Text = serialPort1.DataBits.ToString();
                RTerminal.SelectedIndex = 0;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                serialPort1.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
                device.SelectedIndex = 0;
                serialPort1.Close();
                serialPort1.Encoding = CMD.encoder;
            }
        }

        private void ProgPort_DropDown(object sender, EventArgs e)
        {
            var portname = portSearch();
            ProgPort.Items.Clear();
            ProgPort.Enabled = true;

            if (portname.Length != 0)
            {
                ProgPort.Items.AddRange(portname);
                ProgPort.SelectedItem = portname[0];
            }
        }


        private void tabControl1_DragDrop(object sender, DragEventArgs e)
        {
            string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (file[0].EndsWith(".hex"))
                hexFile.Text = file[0];
            else
                MessageBox.Show($"File {file} is not an ihex file.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void tabProgram_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        async private void progButtonClick(object sender, EventArgs e)
        {
            progMessage.Text = "";
            portOn(ProgPort.SelectedItem.ToString(), int.Parse(textBaud.Text), int.Parse(textBit.Text));
            if (serialPort1.IsOpen)
            {
                var loader = new Loader(serialPort1, device.SelectedIndex, true, flash_file: hexFile.Text);
                var progress = new Progress<int>(percent => {; progressBar1.Value = percent; });
                
                try
                {
                    await loader.prepare(progress);
                    await loader.loading(progress);
                }
                catch (Exception err)
                {
                    progMessage.Text = err.Message;
                }
            }
            else
            {
                MessageBox.Show("Cannot communicate with device", "connect error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            portOff(ProgPort.SelectedItem.ToString());
            progMessage.Text = "Done!";
        }

        private void ProgPort_DropDownClosed(object sender, EventArgs e)
        {
            serialPort1.PortName = ProgPort.SelectedItem.ToString();
        }
    }
}
