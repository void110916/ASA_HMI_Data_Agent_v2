namespace usart
{
    partial class Form1
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
            this.tabSetting = new System.Windows.Forms.TabPage();
            this.RTerminal = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabTerminal.SuspendLayout();
            this.tabSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 38400;
            this.serialPort1.PortName = "COM3";
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // comboPort
            // 
            this.comboPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPort.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.comboPort.FormattingEnabled = true;
            this.comboPort.Location = new System.Drawing.Point(74, 16);
            this.comboPort.Margin = new System.Windows.Forms.Padding(4);
            this.comboPort.Name = "comboPort";
            this.comboPort.Size = new System.Drawing.Size(140, 24);
            this.comboPort.TabIndex = 10;
            this.comboPort.DropDown += new System.EventHandler(this.comboPort_DropDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(21, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.label2.Location = new System.Drawing.Point(350, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "BaudRate :";
            // 
            // textBaud
            // 
            this.textBaud.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.textBaud.Location = new System.Drawing.Point(441, 16);
            this.textBaud.Margin = new System.Windows.Forms.Padding(4);
            this.textBaud.Name = "textBaud";
            this.textBaud.Size = new System.Drawing.Size(116, 23);
            this.textBaud.TabIndex = 4;
            this.textBaud.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBaud_KeyUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.label3.Location = new System.Drawing.Point(589, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "DataBit :";
            // 
            // textBit
            // 
            this.textBit.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.textBit.Location = new System.Drawing.Point(665, 16);
            this.textBit.Margin = new System.Windows.Forms.Padding(4);
            this.textBit.Name = "textBit";
            this.textBit.Size = new System.Drawing.Size(116, 23);
            this.textBit.TabIndex = 5;
            this.textBit.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBit_KeyUp);
            // 
            // Terminal
            // 
            this.Terminal.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.Terminal.Location = new System.Drawing.Point(9, 57);
            this.Terminal.Margin = new System.Windows.Forms.Padding(4);
            this.Terminal.Multiline = true;
            this.Terminal.Name = "Terminal";
            this.Terminal.ReadOnly = true;
            this.Terminal.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Terminal.Size = new System.Drawing.Size(511, 392);
            this.Terminal.TabIndex = 6;
            // 
            // textBinary
            // 
            this.textBinary.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBinary.Location = new System.Drawing.Point(565, 57);
            this.textBinary.Margin = new System.Windows.Forms.Padding(4);
            this.textBinary.Multiline = true;
            this.textBinary.Name = "textBinary";
            this.textBinary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBinary.Size = new System.Drawing.Size(354, 392);
            this.textBinary.TabIndex = 7;
            // 
            // terminalClear
            // 
            this.terminalClear.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.terminalClear.Location = new System.Drawing.Point(448, 471);
            this.terminalClear.Margin = new System.Windows.Forms.Padding(4);
            this.terminalClear.Name = "terminalClear";
            this.terminalClear.Size = new System.Drawing.Size(72, 23);
            this.terminalClear.TabIndex = 2;
            this.terminalClear.Text = "Clear";
            this.terminalClear.UseVisualStyleBackColor = true;
            this.terminalClear.Click += new System.EventHandler(this.button1_Click);
            // 
            // TerminalEnter
            // 
            this.TerminalEnter.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.TerminalEnter.Location = new System.Drawing.Point(367, 471);
            this.TerminalEnter.Margin = new System.Windows.Forms.Padding(4);
            this.TerminalEnter.Name = "TerminalEnter";
            this.TerminalEnter.Size = new System.Drawing.Size(73, 23);
            this.TerminalEnter.TabIndex = 1;
            this.TerminalEnter.Text = "Enter";
            this.TerminalEnter.UseVisualStyleBackColor = true;
            this.TerminalEnter.Click += new System.EventHandler(this.button2_Click);
            // 
            // textWrite
            // 
            this.textWrite.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.textWrite.Location = new System.Drawing.Point(9, 471);
            this.textWrite.Margin = new System.Windows.Forms.Padding(4);
            this.textWrite.Name = "textWrite";
            this.textWrite.Size = new System.Drawing.Size(350, 23);
            this.textWrite.TabIndex = 0;
            this.textWrite.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textWrite_KeyUp);
            // 
            // binaryClear
            // 
            this.binaryClear.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.binaryClear.Location = new System.Drawing.Point(849, 471);
            this.binaryClear.Margin = new System.Windows.Forms.Padding(4);
            this.binaryClear.Name = "binaryClear";
            this.binaryClear.Size = new System.Drawing.Size(70, 23);
            this.binaryClear.TabIndex = 3;
            this.binaryClear.Text = "clear";
            this.binaryClear.UseVisualStyleBackColor = true;
            this.binaryClear.Click += new System.EventHandler(this.binaryClear_Click);
            // 
            // COM
            // 
            this.COM.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.COM.ForeColor = System.Drawing.Color.Red;
            this.COM.Location = new System.Drawing.Point(244, 16);
            this.COM.Margin = new System.Windows.Forms.Padding(4);
            this.COM.Name = "COM";
            this.COM.Size = new System.Drawing.Size(88, 24);
            this.COM.TabIndex = 6;
            this.COM.Text = "OFF";
            this.COM.UseVisualStyleBackColor = true;
            this.COM.Click += new System.EventHandler(this.COM_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTerminal);
            this.tabControl1.Controls.Add(this.tabSetting);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(945, 560);
            this.tabControl1.TabIndex = 11;
            // 
            // tabTerminal
            // 
            this.tabTerminal.Controls.Add(this.comboPort);
            this.tabTerminal.Controls.Add(this.COM);
            this.tabTerminal.Controls.Add(this.label1);
            this.tabTerminal.Controls.Add(this.binaryClear);
            this.tabTerminal.Controls.Add(this.label2);
            this.tabTerminal.Controls.Add(this.textWrite);
            this.tabTerminal.Controls.Add(this.textBaud);
            this.tabTerminal.Controls.Add(this.TerminalEnter);
            this.tabTerminal.Controls.Add(this.label3);
            this.tabTerminal.Controls.Add(this.terminalClear);
            this.tabTerminal.Controls.Add(this.textBit);
            this.tabTerminal.Controls.Add(this.textBinary);
            this.tabTerminal.Controls.Add(this.Terminal);
            this.tabTerminal.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.tabTerminal.Location = new System.Drawing.Point(4, 25);
            this.tabTerminal.Margin = new System.Windows.Forms.Padding(4);
            this.tabTerminal.Name = "tabTerminal";
            this.tabTerminal.Padding = new System.Windows.Forms.Padding(4);
            this.tabTerminal.Size = new System.Drawing.Size(937, 531);
            this.tabTerminal.TabIndex = 0;
            this.tabTerminal.Text = "Terminal";
            this.tabTerminal.UseVisualStyleBackColor = true;
            // 
            // tabSetting
            // 
            this.tabSetting.Controls.Add(this.RTerminal);
            this.tabSetting.Controls.Add(this.label4);
            this.tabSetting.Location = new System.Drawing.Point(4, 25);
            this.tabSetting.Margin = new System.Windows.Forms.Padding(4);
            this.tabSetting.Name = "tabSetting";
            this.tabSetting.Padding = new System.Windows.Forms.Padding(4);
            this.tabSetting.Size = new System.Drawing.Size(937, 531);
            this.tabSetting.TabIndex = 1;
            this.tabSetting.Text = "Setting";
            this.tabSetting.UseVisualStyleBackColor = true;
            this.tabSetting.Enter += new System.EventHandler(this.tabSetting_Enter);
            // 
            // RTerminal
            // 
            this.RTerminal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RTerminal.FormattingEnabled = true;
            this.RTerminal.Items.AddRange(new object[] {
            "ASAHMI",
            "HEX解碼"});
            this.RTerminal.Location = new System.Drawing.Point(123, 26);
            this.RTerminal.Name = "RTerminal";
            this.RTerminal.Size = new System.Drawing.Size(121, 24);
            this.RTerminal.TabIndex = 1;
            this.RTerminal.SelectedIndexChanged += new System.EventHandler(this.RTerminal_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "右側終端顯示 :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 560);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Agent";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabTerminal.ResumeLayout(false);
            this.tabTerminal.PerformLayout();
            this.tabSetting.ResumeLayout(false);
            this.tabSetting.PerformLayout();
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
    }
}

