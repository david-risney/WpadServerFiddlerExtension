using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Net;
using System.Text;
using System.Windows.Forms;
using WpadDhcpServerLibrary;

namespace WpadFiddlerExtension
{
    public partial class ExtensionSettingsDialog : UserControl
    {
        private bool mCloseButtonVisible = true;

        public void SaveAndClose()
        {
            VisibleChanged -= ExtensionSettingsDialog_VisibleChanged;
            Log.Get().OnNewEntry -= ExtensionSettingsDialog_OnNewEntry;
            Settings.Get().Server.OnException = null;
            Settings.Get().Server.OnRunningChanged = null;

            Settings.Get().SaveSettings();
        }

        public ExtensionSettingsDialog()
        {
            mCloseButtonVisible = Parent is Form;

            InitializeComponent();
            ScrewInitializeComponents();

            VisibleChanged += ExtensionSettingsDialog_VisibleChanged;
            Log.Get().OnNewEntry += ExtensionSettingsDialog_OnNewEntry;
            Settings.Get().Server.OnException = Server_OnException;
            Settings.Get().Server.OnRunningChanged = Server_OnRunningChanged;
        }

        void Server_OnException(Exception exception)
        {
            UpdateServerStatus();
        }

        void Server_OnRunningChanged(bool running)
        {
            UpdateServerStatus();
        }

        public void ScrewInitializeComponents()
        {
            if (Settings.Get().LocalPacFile != null)
            {
                localPacFileTextBox.Text = Settings.Get().LocalPacFile.FullName;
            }

            if (Settings.Get().RemotePacUrl != null)
            {
                remotePacUrlTextBox.Text = Settings.Get().RemotePacUrl.ToString();
            }

            localFiddlerRadioButton.Checked = false;
            localPacFileRadioButton.Checked = false;
            remotePacUrlRadioButton.Checked = false;

            switch (Settings.Get().PacSetting)
            {
                case Settings.PacSettingValues.LocalFiddler:
                    localFiddlerRadioButton.Checked = true;
                    break;

                case Settings.PacSettingValues.LocalPacFile:
                    localPacFileRadioButton.Checked = true;
                    break;

                case Settings.PacSettingValues.RemotePacUrl:
                    remotePacUrlRadioButton.Checked = true;
                    break;
            }
            LocalFiddlerRadioButtonChanged();
            LocalPacFileRadioButtonChanged();
            RemotePacFileRadioButtonChanged();

            UpdateServerStatus();

            logListView.HeaderStyle = ColumnHeaderStyle.Clickable; 
            logListView.Columns.Clear();
            logListView.Columns.Add("Time", -2, HorizontalAlignment.Left);
            logListView.Columns.Add("Message", -2, HorizontalAlignment.Left);
            logListView.ShowItemToolTips = true;
            logListView.KeyDown += new KeyEventHandler(logListView_KeyDown);
            logListView.ContextMenu = new ContextMenu();

            _copyMenuItem = new MenuItem("Copy", new EventHandler(logListView_CtxtMenu_Copy));
            _addAllowIpAddrMenuItem = new MenuItem("Add to Allow List", new EventHandler(logListView_CtxtMenu_AddAllow));
            _addDenyIpAddrMenuItem = new MenuItem("Add to Deny List", new EventHandler(logListView_CtxtMenu_AddDeny));
            logListView.ContextMenu.Popup += new EventHandler(LogListViewContextMenu_OnPopup);

            logListView.ContextMenu.MenuItems.Add(_copyMenuItem);
            logListView.ContextMenu.MenuItems.Add(new MenuItem("-"));
            logListView.ContextMenu.MenuItems.Add(_addAllowIpAddrMenuItem);
            logListView.ContextMenu.MenuItems.Add(_addDenyIpAddrMenuItem);

            noFilteringRadioButton.CheckedChanged += new EventHandler(noFilteringRadioButton_CheckedChanged);
            allowListFilteringRadioButton.CheckedChanged += new EventHandler(allowListFilteringRadioButton_CheckedChanged);
            denyListFilteringRadioButton.CheckedChanged += new EventHandler(denyListFilteringRadioButton_CheckedChanged);

            switch (Settings.Get().IPList)
            {
                case Settings.IPListValues.NoList:
                    noFilteringRadioButton.Checked = true;
                    allowListFilteringRadioButton.Checked = false;
                    denyListFilteringRadioButton.Checked = false;
                    ipAddrListView.Enabled = false;
                    addIpAddrButton.Enabled = false;
                    removeIpAddrButton.Enabled = false;
                    break;

                case Settings.IPListValues.AllowList:
                    noFilteringRadioButton.Checked = false;
                    allowListFilteringRadioButton.Checked = true;
                    denyListFilteringRadioButton.Checked = false;
                    ipAddrListView.Enabled = true;
                    addIpAddrButton.Enabled = true;
                    removeIpAddrButton.Enabled = true;
                    break;

                case Settings.IPListValues.DenyList:
                    noFilteringRadioButton.Checked = false;
                    allowListFilteringRadioButton.Checked = false;
                    denyListFilteringRadioButton.Checked = true;
                    ipAddrListView.Enabled = true;
                    addIpAddrButton.Enabled = true;
                    removeIpAddrButton.Enabled = true;
                    break;
            }
            IpAddrListUpdate();

            if (!mCloseButtonVisible)
            {
                this.Cancel.Visible = mCloseButtonVisible;
                this.Cancel.Hide();
            }

            ResumeLayout(false);
            PerformLayout();
        }

        void LogListViewContextMenu_OnPopup(object sender, EventArgs e)
        {
            bool enableCopy = logListView.SelectedIndices.Count > 0;
            bool enableAddIp = logListView.SelectedIndices.Count == 1
                && Log.Get().Logs[logListView.SelectedIndices[0]].Context is IPAddress;

            _copyMenuItem.Enabled = enableCopy;
            _addAllowIpAddrMenuItem.Enabled = enableAddIp;
            _addDenyIpAddrMenuItem.Enabled = enableAddIp;
        }

        MenuItem _addAllowIpAddrMenuItem;
        MenuItem _addDenyIpAddrMenuItem;
        MenuItem _copyMenuItem;

        void IpAddrListUpdate()
        {
            ipAddrListView.Items.Clear();
            List<IPAddress> ipList = null;

            switch (Settings.Get().IPList)
            {
                case Settings.IPListValues.NoList:
                    ipList = null;
                    break;

                case Settings.IPListValues.AllowList:
                    ipList = Settings.Get().AllowIPList;
                    break;

                case Settings.IPListValues.DenyList:
                    ipList = Settings.Get().DenyIPList;
                    break;
            }

            if (ipList != null)
            {
                foreach (IPAddress ipAddr in ipList)
                {
                    ipAddrListView.Items.Add(new ListViewItem(ipAddr.ToString()));
                }
            }
        }

        void logListView_CtxtMenu_AddAllow(object sender, EventArgs eventArgs)
        {
            IPAddress ipAddr = (IPAddress)Log.Get().Logs[logListView.SelectedIndices[0]].Context;
            Settings.Get().AllowIPList.Add(ipAddr);
            IpAddrListUpdate();
        }

        void logListView_CtxtMenu_AddDeny(object sender, EventArgs eventArgs)
        {
            IPAddress ipAddr = (IPAddress)Log.Get().Logs[logListView.SelectedIndices[0]].Context;
            Settings.Get().DenyIPList.Add(ipAddr);
            IpAddrListUpdate();
        }

        void logListView_CtxtMenu_Copy(object sender, EventArgs eventArgs)
        {
            logListView_Copy();
        }

        void logListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                logListView_Copy();
            }
        }

        private void logListView_Copy()
        {
            if (logListView.SelectedItems.Count > 0)
            {
                string result = "";
                foreach (ListViewItem item in logListView.SelectedItems)
                {
                    for (int subIdx = 0; subIdx < item.SubItems.Count; ++subIdx)
                    {
                        result += item.SubItems[subIdx].Text + "\r\n";
                    }

                    result += "\r\n";
                }

                Clipboard.SetText(result);
            }
        }

        private void ExtensionSettingsDialog_VisibleChanged(object sender, EventArgs e)
        {
            /*
            if (Visible)
            {
                foreach (Log.Entry entry in Log.Get().Logs)
                {
                    AddLogEntry(entry);
                }
            }
            else
            {
                ClearLogEntries();
            }
             */
        }

        private void AddLogEntry(Log.Entry entry)
        {
            ListViewItem item = new ListViewItem(entry.Time.ToString(), 0);
            item.SubItems.Add(entry.Text);
            item.ToolTipText = entry.Text;
            logListView.Items.Add(item);
            logListView.Update();
        }

        private void ClearLogEntries()
        {
            logListView.Items.Clear();
        }

        private void  ExtensionSettingsDialog_OnNewEntry(Log.Entry entry)
        {
            AddLogEntry(entry);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            if (Parent is Form)
            {
                ((Form)(Parent)).Close();
                SaveAndClose();
            }
        }

        private void startStopServerButton_Click(object sender, EventArgs e)
        {
            Settings.Get().Running = !Settings.Get().Running;
            UpdateServerStatus();
        }

        private void UpdateServerStatus()
        {
            //WpadDhcpServer server = Settings.Get().Server;
            //if (server.Running)
            if (Settings.Get().Running)
            {
                serverStatusTextBox.Text = "Running";
                startStopServerButton.Text = "Stop Server";
            }
            else
            {
                serverStatusTextBox.Text = "Stopped";
                startStopServerButton.Text = "Start Server";
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            LocalFiddlerRadioButtonChanged();
        }

        private void LocalFiddlerRadioButtonChanged()
        {
            if (localFiddlerRadioButton.Checked)
            {
                localPacFileRadioButton.Checked = false;
                remotePacUrlRadioButton.Checked = false;
                LocalPacFileRadioButtonChanged();
                RemotePacFileRadioButtonChanged();

                Settings.Get().PacSetting = Settings.PacSettingValues.LocalFiddler;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            LocalPacFileRadioButtonChanged();
        }

        private void LocalPacFileRadioButtonChanged()
        {
            if (localPacFileRadioButton.Checked)
            {
                localPacFileTextBox.Enabled = true;
                button2.Enabled = true;
                if (Settings.Get().LocalPacFile == null)
                {
                    button2.PerformClick();
                }
                else
                {
                    localFiddlerRadioButton.Checked = false;
                    remotePacUrlRadioButton.Checked = false;
                    LocalFiddlerRadioButtonChanged();
                    RemotePacFileRadioButtonChanged();

                    Settings.Get().PacSetting = Settings.PacSettingValues.LocalPacFile;
                }
            }
            else
            {
                button2.Enabled = false;
                localPacFileTextBox.Enabled = false;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            localFiddlerRadioButton.Checked = false;
            remotePacUrlRadioButton.Checked = false;
            LocalFiddlerRadioButtonChanged();
            RemotePacFileRadioButtonChanged();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Title = "Select PAC File";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Get().LocalPacFile = new System.IO.FileInfo(openFileDialog.FileName);
                localPacFileTextBox.Text = Settings.Get().LocalPacFile.FullName;
                Settings.Get().PacSetting = Settings.PacSettingValues.LocalPacFile;
            }
            else
            {
                localFiddlerRadioButton.Checked = true;
                LocalFiddlerRadioButtonChanged();
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            RemotePacFileRadioButtonChanged();
        }

        private void RemotePacFileRadioButtonChanged()
        {
            if (remotePacUrlRadioButton.Checked)
            {
                remotePacUrlTextBox.Enabled = true;
                if (Settings.Get().RemotePacUrl == null)
                {
                    if (!remotePacUrlTextBox.Focused)
                        remotePacUrlTextBox.Focus();
                }
                else
                {
                    localFiddlerRadioButton.Checked = false;
                    localPacFileRadioButton.Checked = false;
                    LocalFiddlerRadioButtonChanged();
                    LocalPacFileRadioButtonChanged();

                    Settings.Get().PacSetting = Settings.PacSettingValues.RemotePacUrl;
                }
            }
            else
            {
                remotePacUrlTextBox.Enabled = false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            localFiddlerRadioButton.Checked = false;
            localPacFileRadioButton.Checked = false;
            LocalFiddlerRadioButtonChanged();
            LocalPacFileRadioButtonChanged();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            try
            {
                Settings.Get().RemotePacUrl = new Uri(remotePacUrlTextBox.Text);
                Settings.Get().PacSetting = Settings.PacSettingValues.RemotePacUrl;
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Remote PAC URL");
                localFiddlerRadioButton.Checked = true;
                LocalFiddlerRadioButtonChanged();
            }
        }

        private void noFilteringRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (noFilteringRadioButton.Checked)
            {
                Settings.Get().IPList = Settings.IPListValues.NoList;
                allowListFilteringRadioButton.Checked = false;
                denyListFilteringRadioButton.Checked = false;
                ipAddrListView.Enabled = false;
                addIpAddrButton.Enabled = false;
                removeIpAddrButton.Enabled = false;
                IpAddrListUpdate();
            }
        }

        void denyListFilteringRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (denyListFilteringRadioButton.Checked)
            {
                Settings.Get().IPList = Settings.IPListValues.DenyList;
                noFilteringRadioButton.Checked = false;
                allowListFilteringRadioButton.Checked = false;
                ipAddrListView.Enabled = true;
                addIpAddrButton.Enabled = true;
                removeIpAddrButton.Enabled = true;
                IpAddrListUpdate();
            }
        }

        private void allowListFilteringRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (allowListFilteringRadioButton.Checked)
            {
                Settings.Get().IPList = Settings.IPListValues.AllowList;
                noFilteringRadioButton.Checked = false;
                denyListFilteringRadioButton.Checked = false;
                ipAddrListView.Enabled = true;
                addIpAddrButton.Enabled = true;
                removeIpAddrButton.Enabled = true;
                IpAddrListUpdate();
            }
        }

        private void removeIpAddrButton_Click(object sender, EventArgs e)
        {
            if (ipAddrListView.SelectedIndices.Count > 0)
            {
                List<IPAddress> ipList = null;

                switch (Settings.Get().IPList)
                {
                    case Settings.IPListValues.NoList:
                        ipList = null;
                        break;

                    case Settings.IPListValues.AllowList:
                        ipList = Settings.Get().AllowIPList;
                        break;

                    case Settings.IPListValues.DenyList:
                        ipList = Settings.Get().DenyIPList;
                        break;
                }

                if (ipList != null)
                {
                    for (int idx = ipAddrListView.SelectedIndices.Count; idx > 0; --idx)
                    {
                        ipList.RemoveAt(ipAddrListView.SelectedIndices[idx - 1]);
                    }
                }

                IpAddrListUpdate();
            }
        }

        private void ipAddrListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void addIpAddrButton_Click(object sender, EventArgs e)
        {
            TextBoxWindow textBoxWindow = new TextBoxWindow("Add IP Address", "IP Address", "", true, true);
            if (textBoxWindow.DialogResult == DialogResult.OK)
            {
                try
                {
                    switch (Settings.Get().IPList)
                    {
                        case Settings.IPListValues.NoList:
                            break;

                        case Settings.IPListValues.AllowList:
                            Settings.Get().AllowIPList.Add(IPAddress.Parse(textBoxWindow.Content));
                            break;

                        case Settings.IPListValues.DenyList:
                            Settings.Get().DenyIPList.Add(IPAddress.Parse(textBoxWindow.Content));
                            break;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Invalid IP Address", "Error", MessageBoxButtons.OK);
                }

                IpAddrListUpdate();
            }
        }

        private void ExtensionSettingsDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
