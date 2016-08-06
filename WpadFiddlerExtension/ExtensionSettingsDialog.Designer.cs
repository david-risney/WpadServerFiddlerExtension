namespace WpadFiddlerExtension
{
    partial class ExtensionSettingsDialog
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Cancel = new System.Windows.Forms.Button();
            this.startStopServerButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.logListView = new System.Windows.Forms.ListView();
            this.Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Message = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.localPacFileTextBox = new System.Windows.Forms.TextBox();
            this.remotePacUrlTextBox = new System.Windows.Forms.TextBox();
            this.remotePacUrlRadioButton = new System.Windows.Forms.RadioButton();
            this.localPacFileRadioButton = new System.Windows.Forms.RadioButton();
            this.localFiddlerRadioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.serverStatusTextBox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.removeIpAddrButton = new System.Windows.Forms.Button();
            this.addIpAddrButton = new System.Windows.Forms.Button();
            this.ipAddrListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.denyListFilteringRadioButton = new System.Windows.Forms.RadioButton();
            this.allowListFilteringRadioButton = new System.Windows.Forms.RadioButton();
            this.noFilteringRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.Location = new System.Drawing.Point(520, 593);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Close";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // startStopServerButton
            // 
            this.startStopServerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startStopServerButton.Location = new System.Drawing.Point(502, 3);
            this.startStopServerButton.Name = "startStopServerButton";
            this.startStopServerButton.Size = new System.Drawing.Size(93, 23);
            this.startStopServerButton.TabIndex = 3;
            this.startStopServerButton.Text = "Start/Stop Server";
            this.startStopServerButton.UseVisualStyleBackColor = true;
            this.startStopServerButton.Click += new System.EventHandler(this.startStopServerButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.logListView);
            this.groupBox1.Location = new System.Drawing.Point(3, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(592, 227);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Log";
            // 
            // logListView
            // 
            this.logListView.AllowColumnReorder = true;
            this.logListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.logListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Time,
            this.Message});
            this.logListView.FullRowSelect = true;
            this.logListView.GridLines = true;
            this.logListView.Location = new System.Drawing.Point(6, 19);
            this.logListView.Name = "logListView";
            this.logListView.Size = new System.Drawing.Size(580, 202);
            this.logListView.TabIndex = 5;
            this.logListView.UseCompatibleStateImageBehavior = false;
            this.logListView.View = System.Windows.Forms.View.Details;
            // 
            // Time
            // 
            this.Time.Name = "Time";
            this.Time.Text = "Time";
            this.Time.Width = 72;
            // 
            // Message
            // 
            this.Message.Name = "Message";
            this.Message.Text = "Message";
            this.Message.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Message.Width = 310;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.localPacFileTextBox);
            this.groupBox2.Controls.Add(this.remotePacUrlTextBox);
            this.groupBox2.Controls.Add(this.remotePacUrlRadioButton);
            this.groupBox2.Controls.Add(this.localPacFileRadioButton);
            this.groupBox2.Controls.Add(this.localFiddlerRadioButton);
            this.groupBox2.Location = new System.Drawing.Point(3, 501);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(592, 90);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PAC Settings";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(511, 39);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Select File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // localPacFileTextBox
            // 
            this.localPacFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.localPacFileTextBox.Location = new System.Drawing.Point(123, 41);
            this.localPacFileTextBox.Name = "localPacFileTextBox";
            this.localPacFileTextBox.ReadOnly = true;
            this.localPacFileTextBox.Size = new System.Drawing.Size(382, 20);
            this.localPacFileTextBox.TabIndex = 5;
            this.localPacFileTextBox.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // remotePacUrlTextBox
            // 
            this.remotePacUrlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.remotePacUrlTextBox.Location = new System.Drawing.Point(123, 64);
            this.remotePacUrlTextBox.Name = "remotePacUrlTextBox";
            this.remotePacUrlTextBox.Size = new System.Drawing.Size(463, 20);
            this.remotePacUrlTextBox.TabIndex = 3;
            this.remotePacUrlTextBox.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            this.remotePacUrlTextBox.Enter += new System.EventHandler(this.textBox2_Enter);
            this.remotePacUrlTextBox.Leave += new System.EventHandler(this.textBox2_Leave);
            // 
            // remotePacUrlRadioButton
            // 
            this.remotePacUrlRadioButton.AutoSize = true;
            this.remotePacUrlRadioButton.Location = new System.Drawing.Point(6, 65);
            this.remotePacUrlRadioButton.Name = "remotePacUrlRadioButton";
            this.remotePacUrlRadioButton.Size = new System.Drawing.Size(111, 17);
            this.remotePacUrlRadioButton.TabIndex = 2;
            this.remotePacUrlRadioButton.TabStop = true;
            this.remotePacUrlRadioButton.Text = "Remote PAC URL";
            this.remotePacUrlRadioButton.UseVisualStyleBackColor = true;
            this.remotePacUrlRadioButton.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // localPacFileRadioButton
            // 
            this.localPacFileRadioButton.AutoSize = true;
            this.localPacFileRadioButton.Location = new System.Drawing.Point(6, 42);
            this.localPacFileRadioButton.Name = "localPacFileRadioButton";
            this.localPacFileRadioButton.Size = new System.Drawing.Size(91, 17);
            this.localPacFileRadioButton.TabIndex = 1;
            this.localPacFileRadioButton.TabStop = true;
            this.localPacFileRadioButton.Text = "Local PAC file";
            this.localPacFileRadioButton.UseVisualStyleBackColor = true;
            this.localPacFileRadioButton.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // localFiddlerRadioButton
            // 
            this.localFiddlerRadioButton.AutoSize = true;
            this.localFiddlerRadioButton.Location = new System.Drawing.Point(6, 19);
            this.localFiddlerRadioButton.Name = "localFiddlerRadioButton";
            this.localFiddlerRadioButton.Size = new System.Drawing.Size(246, 17);
            this.localFiddlerRadioButton.TabIndex = 0;
            this.localFiddlerRadioButton.TabStop = true;
            this.localFiddlerRadioButton.Text = "Auto-generate PAC script for local Fiddler proxy";
            this.localFiddlerRadioButton.UseVisualStyleBackColor = true;
            this.localFiddlerRadioButton.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Server Status:";
            // 
            // serverStatusTextBox
            // 
            this.serverStatusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.serverStatusTextBox.Location = new System.Drawing.Point(86, 5);
            this.serverStatusTextBox.Name = "serverStatusTextBox";
            this.serverStatusTextBox.Size = new System.Drawing.Size(410, 20);
            this.serverStatusTextBox.TabIndex = 6;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.AutoSize = true;
            this.groupBox3.Controls.Add(this.removeIpAddrButton);
            this.groupBox3.Controls.Add(this.addIpAddrButton);
            this.groupBox3.Controls.Add(this.ipAddrListView);
            this.groupBox3.Controls.Add(this.denyListFilteringRadioButton);
            this.groupBox3.Controls.Add(this.allowListFilteringRadioButton);
            this.groupBox3.Controls.Add(this.noFilteringRadioButton);
            this.groupBox3.Location = new System.Drawing.Point(3, 262);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(592, 237);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Response Filtering";
            // 
            // removeIpAddrButton
            // 
            this.removeIpAddrButton.Location = new System.Drawing.Point(163, 195);
            this.removeIpAddrButton.Name = "removeIpAddrButton";
            this.removeIpAddrButton.Size = new System.Drawing.Size(75, 23);
            this.removeIpAddrButton.TabIndex = 5;
            this.removeIpAddrButton.Text = "Remove";
            this.removeIpAddrButton.UseVisualStyleBackColor = true;
            this.removeIpAddrButton.Click += new System.EventHandler(this.removeIpAddrButton_Click);
            // 
            // addIpAddrButton
            // 
            this.addIpAddrButton.Location = new System.Drawing.Point(82, 195);
            this.addIpAddrButton.Name = "addIpAddrButton";
            this.addIpAddrButton.Size = new System.Drawing.Size(75, 23);
            this.addIpAddrButton.TabIndex = 4;
            this.addIpAddrButton.Text = "Add";
            this.addIpAddrButton.UseVisualStyleBackColor = true;
            this.addIpAddrButton.Click += new System.EventHandler(this.addIpAddrButton_Click);
            // 
            // ipAddrListView
            // 
            this.ipAddrListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ipAddrListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ipAddrListView.FullRowSelect = true;
            this.ipAddrListView.GridLines = true;
            this.ipAddrListView.Location = new System.Drawing.Point(83, 19);
            this.ipAddrListView.MultiSelect = false;
            this.ipAddrListView.Name = "ipAddrListView";
            this.ipAddrListView.Size = new System.Drawing.Size(503, 170);
            this.ipAddrListView.TabIndex = 3;
            this.ipAddrListView.UseCompatibleStateImageBehavior = false;
            this.ipAddrListView.View = System.Windows.Forms.View.Details;
            this.ipAddrListView.SelectedIndexChanged += new System.EventHandler(this.ipAddrListView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "IP Address";
            this.columnHeader1.Width = 305;
            // 
            // denyListFilteringRadioButton
            // 
            this.denyListFilteringRadioButton.AutoSize = true;
            this.denyListFilteringRadioButton.Location = new System.Drawing.Point(6, 65);
            this.denyListFilteringRadioButton.Name = "denyListFilteringRadioButton";
            this.denyListFilteringRadioButton.Size = new System.Drawing.Size(50, 17);
            this.denyListFilteringRadioButton.TabIndex = 2;
            this.denyListFilteringRadioButton.TabStop = true;
            this.denyListFilteringRadioButton.Text = "Deny";
            this.denyListFilteringRadioButton.UseVisualStyleBackColor = true;
            // 
            // allowListFilteringRadioButton
            // 
            this.allowListFilteringRadioButton.AutoSize = true;
            this.allowListFilteringRadioButton.Location = new System.Drawing.Point(6, 42);
            this.allowListFilteringRadioButton.Name = "allowListFilteringRadioButton";
            this.allowListFilteringRadioButton.Size = new System.Drawing.Size(50, 17);
            this.allowListFilteringRadioButton.TabIndex = 1;
            this.allowListFilteringRadioButton.TabStop = true;
            this.allowListFilteringRadioButton.Text = "Allow";
            this.allowListFilteringRadioButton.UseVisualStyleBackColor = true;
            this.allowListFilteringRadioButton.CheckedChanged += new System.EventHandler(this.allowListFilteringRadioButton_CheckedChanged);
            // 
            // noFilteringRadioButton
            // 
            this.noFilteringRadioButton.AutoSize = true;
            this.noFilteringRadioButton.Location = new System.Drawing.Point(6, 19);
            this.noFilteringRadioButton.Name = "noFilteringRadioButton";
            this.noFilteringRadioButton.Size = new System.Drawing.Size(75, 17);
            this.noFilteringRadioButton.TabIndex = 0;
            this.noFilteringRadioButton.TabStop = true;
            this.noFilteringRadioButton.Text = "No filtering";
            this.noFilteringRadioButton.UseVisualStyleBackColor = true;
            this.noFilteringRadioButton.CheckedChanged += new System.EventHandler(this.noFilteringRadioButton_CheckedChanged);
            // 
            // ExtensionSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.serverStatusTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.startStopServerButton);
            this.Controls.Add(this.groupBox1);
            this.Name = "ExtensionSettingsDialog";
            this.Size = new System.Drawing.Size(598, 621);
            this.Load += new System.EventHandler(this.ExtensionSettingsDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button startStopServerButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox remotePacUrlTextBox;
        private System.Windows.Forms.RadioButton remotePacUrlRadioButton;
        private System.Windows.Forms.RadioButton localPacFileRadioButton;
        private System.Windows.Forms.RadioButton localFiddlerRadioButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox localPacFileTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox serverStatusTextBox;
        private System.Windows.Forms.ListView logListView;
        private System.Windows.Forms.ColumnHeader Time;
        private System.Windows.Forms.ColumnHeader Message;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton denyListFilteringRadioButton;
        private System.Windows.Forms.RadioButton allowListFilteringRadioButton;
        private System.Windows.Forms.RadioButton noFilteringRadioButton;
        private System.Windows.Forms.Button addIpAddrButton;
        private System.Windows.Forms.ListView ipAddrListView;
        private System.Windows.Forms.Button removeIpAddrButton;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}
