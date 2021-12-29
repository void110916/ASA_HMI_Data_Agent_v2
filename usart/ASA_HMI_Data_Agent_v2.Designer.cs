namespace usart
{
    partial class ASA_HMI_Data_Agent_v2
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (serialPort1.IsOpen)
                serialPort1.Close();
            serialPort1.Dispose();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ASA_HMI_Data_Agent_v2));
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.comboPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBaud = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBit = new System.Windows.Forms.TextBox();
            this.Terminal = new System.Windows.Forms.TextBox();
            this.textBinary = new System.Windows.Forms.TextBox();
            this.terminalClear = new System.Windows.Forms.Button();
            this.TerminalEnter = new System.Windows.Forms.Button();
            this.textWrite = new System.Windows.Forms.TextBox();
            this.binaryClear = new System.Windows.Forms.Button();
            this.COM = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTerminal = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pacSend = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabProgram = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.progMessage = new System.Windows.Forms.Label();
            this.progButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.hexFile = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.device = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ProgPort = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabSetting = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.RTerminal = new System.Windows.Forms.ComboBox();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabTerminal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabProgram.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 38400;
            this.serialPort1.DtrEnable = true;
            this.serialPort1.PortName = "COM3";
            this.serialPort1.RtsEnable = true;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // comboPort
            // 
            this.comboPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPort.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.comboPort.FormattingEnabled = true;
            this.comboPort.Location = new System.Drawing.Point(46, 23);
            this.comboPort.Margin = new System.Windows.Forms.Padding(2, 5, 20, 5);
            this.comboPort.Name = "comboPort";
            this.comboPort.Size = new System.Drawing.Size(114, 24);
            this.comboPort.TabIndex = 10;
            this.comboPort.DropDown += new System.EventHandler(this.comboPort_DropDown);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(5, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 8, 2, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port :";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.label2.Location = new System.Drawing.Point(314, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(20, 8, 2, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "BaudRate :";
            // 
            // textBaud
            // 
            this.textBaud.Font = new System.Drawing.Font("微軟正黑體", 9.25F);
            this.textBaud.Location = new System.Drawing.Point(387, 23);
            this.textBaud.Margin = new System.Windows.Forms.Padding(2, 5, 20, 5);
            this.textBaud.Name = "textBaud";
            this.textBaud.Size = new System.Drawing.Size(96, 24);
            this.textBaud.TabIndex = 4;
            this.textBaud.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBaud_KeyUp);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.label3.Location = new System.Drawing.Point(523, 27);
            this.label3.Margin = new System.Windows.Forms.Padding(20, 8, 2, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "DataBit :";
            // 
            // textBit
            // 
            this.textBit.Font = new System.Drawing.Font("微軟正黑體", 9.25F);
            this.textBit.Location = new System.Drawing.Point(582, 23);
            this.textBit.Margin = new System.Windows.Forms.Padding(2, 5, 2, 5);
            this.textBit.Name = "textBit";
            this.textBit.Size = new System.Drawing.Size(90, 24);
            this.textBit.TabIndex = 5;
            this.textBit.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBit_KeyUp);
            // 
            // Terminal
            // 
            this.Terminal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Terminal.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.Terminal.Location = new System.Drawing.Point(4, 21);
            this.Terminal.Margin = new System.Windows.Forms.Padding(1, 1, 1, 5);
            this.Terminal.Multiline = true;
            this.Terminal.Name = "Terminal";
            this.Terminal.ReadOnly = true;
            this.Terminal.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Terminal.Size = new System.Drawing.Size(508, 383);
            this.Terminal.TabIndex = 6;
            // 
            // textBinary
            // 
            this.textBinary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBinary.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBinary.Location = new System.Drawing.Point(4, 18);
            this.textBinary.Margin = new System.Windows.Forms.Padding(1, 1, 1, 5);
            this.textBinary.Multiline = true;
            this.textBinary.Name = "textBinary";
            this.textBinary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBinary.Size = new System.Drawing.Size(371, 385);
            this.textBinary.TabIndex = 7;
            this.textBinary.Text = "i16_4 :\r\n    { 0, 1, 2, 3 }\r\n\r\ni16_1x4 :\r\n{\r\n    { 1, 2, 3 ,4 }\r\n}\r\n\r\ni16_4 , i16" +
    "_4 :\r\n{\r\n    i16_4 :\r\n        { 2, 3,4, 5 }\r\n    i16_4 :\r\n        { 4, 5, 6, 7 }" +
    "\r\n}\r\n\r\ni16_4 , i16_4 :\r\n{\r\n  \r\n}\r\n";
            // 
            // terminalClear
            // 
            this.terminalClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.terminalClear.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.terminalClear.Location = new System.Drawing.Point(439, 409);
            this.terminalClear.Margin = new System.Windows.Forms.Padding(10, 1, 1, 1);
            this.terminalClear.MinimumSize = new System.Drawing.Size(73, 24);
            this.terminalClear.Name = "terminalClear";
            this.terminalClear.Size = new System.Drawing.Size(73, 24);
            this.terminalClear.TabIndex = 2;
            this.terminalClear.Text = "Clear";
            this.terminalClear.UseVisualStyleBackColor = true;
            this.terminalClear.Click += new System.EventHandler(this.termina_clear_Click);
            // 
            // TerminalEnter
            // 
            this.TerminalEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TerminalEnter.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.TerminalEnter.Location = new System.Drawing.Point(351, 409);
            this.TerminalEnter.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.TerminalEnter.MinimumSize = new System.Drawing.Size(73, 24);
            this.TerminalEnter.Name = "TerminalEnter";
            this.TerminalEnter.Size = new System.Drawing.Size(73, 24);
            this.TerminalEnter.TabIndex = 1;
            this.TerminalEnter.Text = "Enter";
            this.TerminalEnter.UseVisualStyleBackColor = true;
            this.TerminalEnter.Click += new System.EventHandler(this.terminal_enter_Click);
            // 
            // textWrite
            // 
            this.textWrite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textWrite.Font = new System.Drawing.Font("微軟正黑體", 9.25F);
            this.textWrite.Location = new System.Drawing.Point(4, 409);
            this.textWrite.Margin = new System.Windows.Forms.Padding(1, 1, 5, 1);
            this.textWrite.MinimumSize = new System.Drawing.Size(150, 24);
            this.textWrite.Name = "textWrite";
            this.textWrite.Size = new System.Drawing.Size(337, 24);
            this.textWrite.TabIndex = 0;
            this.textWrite.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textWrite_KeyUp);
            // 
            // binaryClear
            // 
            this.binaryClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.binaryClear.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.binaryClear.Location = new System.Drawing.Point(302, 408);
            this.binaryClear.Margin = new System.Windows.Forms.Padding(2);
            this.binaryClear.MinimumSize = new System.Drawing.Size(73, 24);
            this.binaryClear.Name = "binaryClear";
            this.binaryClear.Size = new System.Drawing.Size(73, 24);
            this.binaryClear.TabIndex = 3;
            this.binaryClear.Text = "clear";
            this.binaryClear.UseVisualStyleBackColor = true;
            this.binaryClear.Click += new System.EventHandler(this.binaryClear_Click);
            // 
            // COM
            // 
            this.COM.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.COM.ForeColor = System.Drawing.Color.Red;
            this.COM.Location = new System.Drawing.Point(200, 21);
            this.COM.Margin = new System.Windows.Forms.Padding(20, 2, 20, 2);
            this.COM.Name = "COM";
            this.COM.Size = new System.Drawing.Size(74, 27);
            this.COM.TabIndex = 6;
            this.COM.Text = "OFF";
            this.COM.UseVisualStyleBackColor = true;
            this.COM.Click += new System.EventHandler(this.COM_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDrop = true;
            this.tabControl1.Controls.Add(this.tabTerminal);
            this.tabControl1.Controls.Add(this.tabProgram);
            this.tabControl1.Controls.Add(this.tabSetting);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(10, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(920, 552);
            this.tabControl1.TabIndex = 11;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabControl1.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControl1_DragDrop);
            // 
            // tabTerminal
            // 
            this.tabTerminal.Controls.Add(this.splitContainer1);
            this.tabTerminal.Controls.Add(this.groupBox2);
            this.tabTerminal.Controls.Add(this.flowLayoutPanel1);
            this.tabTerminal.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.tabTerminal.Location = new System.Drawing.Point(4, 25);
            this.tabTerminal.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.tabTerminal.Name = "tabTerminal";
            this.tabTerminal.Size = new System.Drawing.Size(912, 523);
            this.tabTerminal.TabIndex = 0;
            this.tabTerminal.Text = "Terminal";
            this.tabTerminal.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(7, 74);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(899, 443);
            this.splitContainer1.SplitterDistance = 516;
            this.splitContainer1.TabIndex = 14;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Terminal);
            this.groupBox3.Controls.Add(this.terminalClear);
            this.groupBox3.Controls.Add(this.TerminalEnter);
            this.groupBox3.Controls.Add(this.textWrite);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(516, 443);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Terminal";
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.pacSend);
            this.groupBox1.Controls.Add(this.binaryClear);
            this.groupBox1.Controls.Add(this.textBinary);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(379, 443);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ASAHMI";
            // 
            // pacSend
            // 
            this.pacSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pacSend.Location = new System.Drawing.Point(6, 409);
            this.pacSend.Name = "pacSend";
            this.pacSend.Size = new System.Drawing.Size(75, 23);
            this.pacSend.TabIndex = 8;
            this.pacSend.Text = "send";
            this.pacSend.UseVisualStyleBackColor = true;
            this.pacSend.Click += new System.EventHandler(this.pacSend_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBit);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBaud);
            this.groupBox2.Controls.Add(this.comboPort);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.COM);
            this.groupBox2.Location = new System.Drawing.Point(7, 5);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 5, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.groupBox2.Size = new System.Drawing.Size(680, 64);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Serial Port Setting";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(0, 0);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // tabProgram
            // 
            this.tabProgram.AllowDrop = true;
            this.tabProgram.Controls.Add(this.groupBox6);
            this.tabProgram.Controls.Add(this.groupBox5);
            this.tabProgram.Location = new System.Drawing.Point(4, 25);
            this.tabProgram.Name = "tabProgram";
            this.tabProgram.Padding = new System.Windows.Forms.Padding(3);
            this.tabProgram.Size = new System.Drawing.Size(912, 523);
            this.tabProgram.TabIndex = 2;
            this.tabProgram.Text = "Program";
            this.tabProgram.UseVisualStyleBackColor = true;
            this.tabProgram.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControl1_DragDrop);
            this.tabProgram.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabProgram_DragEnter);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.progMessage);
            this.groupBox6.Controls.Add(this.progButton);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.button2);
            this.groupBox6.Controls.Add(this.hexFile);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.progressBar1);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox6.Location = new System.Drawing.Point(3, 136);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(906, 384);
            this.groupBox6.TabIndex = 14;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "programming usual application";
            // 
            // progMessage
            // 
            this.progMessage.AutoSize = true;
            this.progMessage.Location = new System.Drawing.Point(62, 149);
            this.progMessage.Name = "progMessage";
            this.progMessage.Size = new System.Drawing.Size(0, 16);
            this.progMessage.TabIndex = 6;
            // 
            // progButton
            // 
            this.progButton.Location = new System.Drawing.Point(65, 63);
            this.progButton.Name = "progButton";
            this.progButton.Size = new System.Drawing.Size(835, 40);
            this.progButton.TabIndex = 5;
            this.progButton.Text = "start programming";
            this.progButton.UseVisualStyleBackColor = true;
            this.progButton.Click += new System.EventHandler(this.progButtonClick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 16);
            this.label9.TabIndex = 4;
            this.label9.Text = "Hex file :";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(817, 34);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(83, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "select file";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // hexFile
            // 
            this.hexFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hexFile.Location = new System.Drawing.Point(65, 34);
            this.hexFile.Name = "hexFile";
            this.hexFile.Size = new System.Drawing.Size(746, 23);
            this.hexFile.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "bar :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(65, 109);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(835, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.device);
            this.groupBox5.Controls.Add(this.textBox1);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.ProgPort);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2, 5, 2, 2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.groupBox5.Size = new System.Drawing.Size(906, 122);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Serial Port Setting";
            // 
            // device
            // 
            this.device.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.device.FormattingEnabled = true;
            this.device.Items.AddRange(new object[] {
            "auto",
            "asa_m128_v1",
            "asa_m128_v2",
            "asa_m128_v3",
            "asa_m3_v1",
            "asa_m4_v1"});
            this.device.Location = new System.Drawing.Point(65, 56);
            this.device.Margin = new System.Windows.Forms.Padding(2, 5, 20, 5);
            this.device.Name = "device";
            this.device.Size = new System.Drawing.Size(114, 24);
            this.device.TabIndex = 11;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("微軟正黑體", 9.25F);
            this.textBox1.Location = new System.Drawing.Point(65, 87);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 5, 2, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(90, 24);
            this.textBox1.TabIndex = 5;
            this.textBox1.Visible = false;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(24, 27);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 8, 2, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 16);
            this.label6.TabIndex = 1;
            this.label6.Text = "Port :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.label7.Location = new System.Drawing.Point(6, 91);
            this.label7.Margin = new System.Windows.Forms.Padding(20, 8, 2, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 16);
            this.label7.TabIndex = 4;
            this.label7.Text = "DataBit :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label7.Visible = false;
            // 
            // ProgPort
            // 
            this.ProgPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProgPort.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.ProgPort.FormattingEnabled = true;
            this.ProgPort.Location = new System.Drawing.Point(65, 23);
            this.ProgPort.Margin = new System.Windows.Forms.Padding(2, 5, 20, 5);
            this.ProgPort.Name = "ProgPort";
            this.ProgPort.Size = new System.Drawing.Size(114, 24);
            this.ProgPort.TabIndex = 10;
            this.ProgPort.DropDown += new System.EventHandler(this.ProgPort_DropDown);
            this.ProgPort.DropDownClosed += new System.EventHandler(this.ProgPort_DropDownClosed);
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.label8.Location = new System.Drawing.Point(9, 59);
            this.label8.Margin = new System.Windows.Forms.Padding(20, 8, 2, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 16);
            this.label8.TabIndex = 2;
            this.label8.Text = "Device :";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabSetting
            // 
            this.tabSetting.Controls.Add(this.pictureBox1);
            this.tabSetting.Controls.Add(this.groupBox4);
            this.tabSetting.Location = new System.Drawing.Point(4, 25);
            this.tabSetting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabSetting.Name = "tabSetting";
            this.tabSetting.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabSetting.Size = new System.Drawing.Size(912, 523);
            this.tabSetting.TabIndex = 1;
            this.tabSetting.Text = "Setting";
            this.tabSetting.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(882, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.helpProvider1.SetShowHelp(this.pictureBox1, true);
            this.pictureBox1.Size = new System.Drawing.Size(30, 30);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.RTerminal);
            this.groupBox4.Location = new System.Drawing.Point(8, 7);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(8);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox4.Size = new System.Drawing.Size(367, 233);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Terminal setting";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "右側終端顯示 :";
            // 
            // RTerminal
            // 
            this.RTerminal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RTerminal.FormattingEnabled = true;
            this.RTerminal.Items.AddRange(new object[] {
            "ASAHMI",
            "HEX解碼"});
            this.RTerminal.Location = new System.Drawing.Point(100, 24);
            this.RTerminal.Name = "RTerminal";
            this.RTerminal.Size = new System.Drawing.Size(121, 24);
            this.RTerminal.TabIndex = 1;
            this.RTerminal.SelectedIndexChanged += new System.EventHandler(this.RTerminal_SelectedIndexChanged);
            // 
            // ASA_HMI_Data_Agent_v2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 552);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(745, 380);
            this.Name = "ASA_HMI_Data_Agent_v2";
            this.helpProvider1.SetShowHelp(this, true);
            this.Text = "ASA HMI Data Agent v2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabTerminal.ResumeLayout(false);
            this.tabTerminal.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabProgram.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.ComboBox comboPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBaud;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBit;
        private System.Windows.Forms.TextBox Terminal;
        private System.Windows.Forms.TextBox textBinary;
        private System.Windows.Forms.Button terminalClear;
        private System.Windows.Forms.Button TerminalEnter;
        private System.Windows.Forms.TextBox textWrite;
        private System.Windows.Forms.Button binaryClear;
        private System.Windows.Forms.Button COM;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTerminal;
        private System.Windows.Forms.TabPage tabSetting;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox RTerminal;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button pacSend;
        private System.Windows.Forms.TabPage tabProgram;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox device;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ProgPort;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox hexFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button progButton;
        private System.Windows.Forms.Label progMessage;
    }
}

