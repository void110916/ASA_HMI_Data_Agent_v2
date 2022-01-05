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
using System.Text.RegularExpressions;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using programmer;
namespace usart
{

    public partial class ASA_HMI_Data_Agent_v2 : Form
    {
        bool PortReceivable = false;
        public ASA_HMI_Data_Agent_v2()
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
                DataReceived();
            }
            textBaud.Text = serialPort1.BaudRate.ToString();
            textBit.Text = serialPort1.DataBits.ToString();
            RTerminal.SelectedIndex = 0;
        }

        /// call function start
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
                serialPort1.WriteTimeout = 1000;
                serialPort1.ReadTimeout = 1000;
                serialPort1.DtrEnable = true;
                serialPort1.RtsEnable = true;

                try
                {
                    serialPort1.Open();
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
                    PortReceivable = true;

                    return true;
                }
            }
            return false;
        }
        private bool portOff(string portName)
        {
            if (serialPort1.IsOpen)
            {

                //serialPort1.DiscardOutBuffer();
                PortReceivable = false;

                Thread.Sleep(100);
                try
                {
                    serialPort1.DtrEnable = false;
                    serialPort1.RtsEnable = false;
                    serialPort1.DiscardInBuffer();
                    serialPort1.Close();
                }
                catch
                { }
            }

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
                //COM.Enabled = false;
                //Thread.Sleep(500);
                //COM.Enabled = true;
                return true;
            }
            return false;
        }

        enum right_terminal_state
        {
            ASAHMI = 0,
            HEX = 1
        }
        ASADecode decode = new ASADecode();
        ASAEncode encode = new ASAEncode();
        /// call function end

        /// terminal tab start
        // terminal setting
        private void comboPort_DropDown(object sender, EventArgs e)
        {
            portSearch();
        }

        private void textBaud_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                serialPort1.BaudRate = int.Parse(textBaud.Text);
                Terminal.Text += "(system) BaudRate is changed\r\n";
            }
        }

        private void textBit_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                serialPort1.DataBits = int.Parse(textBit.Text);
                Terminal.Text += "(system) DataBit is changed\r\n";
            }
        }

        private void COM_Click(object sender, EventArgs e)
        {
            if (comboPort.SelectedItem != null)
            {
                if (COM.Text == "OFF")
                {
                    if (portOn(comboPort.SelectedItem.ToString(), int.Parse(textBaud.Text), int.Parse(textBit.Text)))
                        DataReceived();
                }
                else if (COM.Text == "ON")
                {
                    portOff(comboPort.SelectedItem.ToString());
                }
            }
        }

        // left terminal side 
        private void terminal_enter_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                var dat = Encoding.ASCII.GetBytes(textWrite.Text);
                serialPort1.BaseStream.Write(dat, 0, dat.Length);
            }
            Terminal.Text += "<<" + textWrite.Text + "\r\n";
            textWrite.Clear();

        }

        private void terminal_clear_Click(object sender, EventArgs e)
        {
            Terminal.Clear();
            textWrite.Clear();
        }

        private void textWrite_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (serialPort1.IsOpen)
                {
                    var dat = Encoding.ASCII.GetBytes(textWrite.Text);
                    serialPort1.BaseStream.Write(dat, 0, dat.Length);
                }
                Terminal.Text += "<<" + textWrite.Text + "\r\n";

                textWrite.Clear();
            }
        }
        // right binary/HMI view
        private void binaryClear_Click(object sender, EventArgs e)
        {
            textBinary.Clear();
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
            var package = encode.get();
            byte[] dat;
            if (Terminal.Text.EndsWith(""))
            {
                dat = Encoding.ASCII.GetBytes("~ACK");
                serialPort1.BaseStream.Write(dat, 0, dat.Length);
            }
            dat = Encoding.ASCII.GetBytes(package);
            serialPort1.BaseStream.Write(dat, 0, dat.Length);
        }
        // serial received event    

        private void DataReceived()
        {
            const int blockLimit = 1024;
            byte[] buffer = new byte[blockLimit];

            Action kickoffRead = null;
            kickoffRead = delegate
            {
                //Thread.Sleep(1);
                //if ((!serialPort1.IsOpen) || (!PortReceivable)) return;
                //try
                //{

                //    int actualLength = await serialPort1.BaseStream.ReadAsync(buffer, 0, buffer.Length);
                //    byte[] received = new byte[actualLength];
                //    Buffer.BlockCopy(buffer, 0, received, 0, actualLength);
                //    raiseAppSerialDataEvent(received);
                //}
                //catch (IOException exc)
                //{
                //    portOff(serialPort1.PortName);
                //    //MessageBox.Show($"serial port error with:\r\n{exc.Message}", "serial error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    //handleAppSerialError(exc);
                //}
                //kickoffRead();

                serialPort1.BaseStream.BeginRead(buffer, 0, buffer.Length, delegate (IAsyncResult ar)
               {
                   Thread.Sleep(10);
                   if ((!serialPort1.IsOpen) || (!PortReceivable)) return;

                   try
                   {
                       int actualLength = serialPort1.BaseStream.EndRead(ar);
                       byte[] received = new byte[actualLength];
                       Buffer.BlockCopy(buffer, 0, received, 0, actualLength);
                       raiseAppSerialDataEvent(received);
                   }
                   catch //(IOException exc)
                   {
                       portOff(serialPort1.PortName);
                       //MessageBox.Show($"serial port error with:\r\n{exc.Message}", "serial error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                       //handleAppSerialError(exc);
                   }
                   kickoffRead();
               }, null);
            };
            //while (serialPort1.IsOpen&&PortReceivable)
            kickoffRead();
        }
        private void raiseAppSerialDataEvent(byte[] data)
        {
            Regex rx = new Regex(@"~G[AMS],");  //檢查是否有HMI sync format封包
            List<byte> terminal_buffs = new List<byte>(data.Length);
            int i = 0;
            int rTerminalIndex = 0;
            if (RTerminal.InvokeRequired)
            {
                Action updateMethod = new Action(() => rTerminalIndex = RTerminal.SelectedIndex);
                Terminal.BeginInvoke(updateMethod);
            }
            else
            {
                rTerminalIndex = RTerminal.SelectedIndex;
            }
            foreach (var buff in data)
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
                        textBinary.BeginInvoke(updateMethod);
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
                var dat = Encoding.ASCII.GetBytes("~ACK\n");
                serialPort1.BaseStream.Write(dat, 0, dat.Length);
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
                Terminal.BeginInvoke(updateMethod);
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
                foreach (byte buff in data)
                {
                    s += buff.ToString("X2") + " ";
                }
                if (textBinary.InvokeRequired)
                {
                    Action updateMethod = new Action(() => textBinary.AppendText(s));
                    textBinary.BeginInvoke(updateMethod);
                }
                else
                {
                    textBinary.AppendText(s);
                }
            }
        }

        private void serialPort1_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            if (e.EventType == SerialPinChange.CDChanged)
                if (!serialPort1.CDHolding)
                    portOff(comboPort.SelectedItem.ToString());
        }
        /// terminal tab end

        /// setting tab start
        private void RTerminal_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox1.Text = RTerminal.SelectedItem.ToString();
            textBinary.Text = "";
            pacSend.Visible = RTerminal.SelectedItem.ToString() == "ASAHMI";
        }
        /// settting tab end

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                if (!serialPort1.IsOpen)
                {
                    serialPort1.Encoding = Encoding.UTF8;
                    portOn((string)comboPort.SelectedItem, int.Parse(textBaud.Text), int.Parse(textBit.Text));
                    DataReceived();
                }
                textBaud.Text = serialPort1.BaudRate.ToString();
                textBit.Text = serialPort1.DataBits.ToString();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                portOff((string)comboPort.SelectedItem);

                device.SelectedIndex = 0;
                serialPort1.Encoding = CMD.encoder;
            }
        }
        /// program tab start
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
            Loader loader;
            if (!File.Exists(hexFile.Text))
                return;
            if (portOn(ProgPort.SelectedItem.ToString(), int.Parse(textBaud.Text), int.Parse(textBit.Text)))
            {

                loader = new Loader(serialPort1, device.SelectedIndex, true, flash_file: hexFile.Text);
                var progress = new Progress<int>(percent => { progMessage.Text = loader.status.ToString(); progressBar1.Value = percent; });
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
            if (loader.status == Loader.Stage.END)
                progMessage.Text = "Done!";
            else
                progMessage.Text = "Fail Programming";
        }

        private void ProgPort_DropDownClosed(object sender, EventArgs e)
        {
            if (ProgPort.SelectedItem != null)
                serialPort1.PortName = ProgPort.SelectedItem.ToString();
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            if (openIHexFileDialog.ShowDialog() == DialogResult.OK)
            {
                hexFile.Text = openIHexFileDialog.FileName;
            }
        }
        /// program tab end

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1 != null)
                serialPort1.Dispose();
        }


    }
}
