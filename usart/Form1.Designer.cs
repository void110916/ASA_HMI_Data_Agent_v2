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
            this.comboPort.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.comboPort.FormattingEnabled = true;
            this.comboPort.Location = new System.Drawing.Point(68, 23);
            this.comboPort.Name = "comboPort";
            this.comboPort.Size = new System.Drawing.Size(121, 25);
            this.comboPort.TabIndex = 10;
            this.comboPort.DropDown += new System.EventHandler(this.comboPort_DropDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.label1.Location = new System.Drawing.Point(22, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.label2.Location = new System.Drawing.Point(304, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "BaudRate :";
            // 
            // textBaud
            // 
            this.textBaud.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.textBaud.Location = new System.Drawing.Point(382, 23);
            this.textBaud.Name = "textBaud";
            this.textBaud.Size = new System.Drawing.Size(100, 25);
            this.textBaud.TabIndex = 4;
            this.textBaud.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBaud_KeyUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.label3.Location = new System.Drawing.Point(509, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "DataBit :";
            // 
            // textBit
            // 
            this.textBit.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.textBit.Location = new System.Drawing.Point(574, 23);
            this.textBit.Name = "textBit";
            this.textBit.Size = new System.Drawing.Size(100, 25);
            this.textBit.TabIndex = 5;
            this.textBit.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBit_KeyUp);
            // 
            // Terminal
            // 
            this.Terminal.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.Terminal.Location = new System.Drawing.Point(12, 78);
            this.Terminal.Multiline = true;
            this.Terminal.Name = "Terminal";
            this.Terminal.ReadOnly = true;
            this.Terminal.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Terminal.Size = new System.Drawing.Size(438, 295);
            this.Terminal.TabIndex = 6;
            // 
            // textBinary
            // 
            this.textBinary.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.textBinary.Location = new System.Drawing.Point(484, 78);
            this.textBinary.Multiline = true;
            this.textBinary.Name = "textBinary";
            this.textBinary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBinary.Size = new System.Drawing.Size(304, 295);
            this.textBinary.TabIndex = 7;
            // 
            // terminalClear
            // 
            this.terminalClear.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.terminalClear.Location = new System.Drawing.Point(375, 396);
            this.terminalClear.Name = "terminalClear";
            this.terminalClear.Size = new System.Drawing.Size(75, 23);
            this.terminalClear.TabIndex = 2;
            this.terminalClear.Text = "Clear";
            this.terminalClear.UseVisualStyleBackColor = true;
            this.terminalClear.Click += new System.EventHandler(this.button1_Click);
            // 
            // TerminalEnter
            // 
            this.TerminalEnter.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.TerminalEnter.Location = new System.Drawing.Point(294, 396);
            this.TerminalEnter.Name = "TerminalEnter";
            this.TerminalEnter.Size = new System.Drawing.Size(75, 23);
            this.TerminalEnter.TabIndex = 1;
            this.TerminalEnter.Text = "Enter";
            this.TerminalEnter.UseVisualStyleBackColor = true;
            this.TerminalEnter.Click += new System.EventHandler(this.button2_Click);
            // 
            // textWrite
            // 
            this.textWrite.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.textWrite.Location = new System.Drawing.Point(13, 394);
            this.textWrite.Name = "textWrite";
            this.textWrite.Size = new System.Drawing.Size(275, 25);
            this.textWrite.TabIndex = 0;
            this.textWrite.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textWrite_KeyUp);
            // 
            // binaryClear
            // 
            this.binaryClear.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.binaryClear.Location = new System.Drawing.Point(713, 396);
            this.binaryClear.Name = "binaryClear";
            this.binaryClear.Size = new System.Drawing.Size(75, 23);
            this.binaryClear.TabIndex = 3;
            this.binaryClear.Text = "clear";
            this.binaryClear.UseVisualStyleBackColor = true;
            this.binaryClear.Click += new System.EventHandler(this.binaryClear_Click);
            // 
            // COM
            // 
            this.COM.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.COM.ForeColor = System.Drawing.Color.Red;
            this.COM.Location = new System.Drawing.Point(213, 25);
            this.COM.Name = "COM";
            this.COM.Size = new System.Drawing.Size(75, 23);
            this.COM.TabIndex = 6;
            this.COM.Text = "OFF";
            this.COM.UseVisualStyleBackColor = true;
            this.COM.Click += new System.EventHandler(this.COM_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.COM);
            this.Controls.Add(this.binaryClear);
            this.Controls.Add(this.textWrite);
            this.Controls.Add(this.TerminalEnter);
            this.Controls.Add(this.terminalClear);
            this.Controls.Add(this.textBinary);
            this.Controls.Add(this.Terminal);
            this.Controls.Add(this.textBit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBaud);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboPort);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}

