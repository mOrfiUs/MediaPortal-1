namespace WatchDog
{
  partial class MPWatchDog
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.settingsGroup = new System.Windows.Forms.GroupBox();
      this.btnZipFileReset = new System.Windows.Forms.Button();
      this.btnZipFile = new System.Windows.Forms.Button();
      this.tbZipFile = new System.Windows.Forms.TextBox();
      this.logDirLabel = new System.Windows.Forms.Label();
      this.ExportLogsRadioButton = new System.Windows.Forms.RadioButton();
      this.label1 = new System.Windows.Forms.Label();
      this.SafeModeRadioButton = new System.Windows.Forms.RadioButton();
      this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
      this.menuItem1 = new System.Windows.Forms.MenuItem();
      this.menuItem2 = new System.Windows.Forms.MenuItem();
      this.menuItem3 = new System.Windows.Forms.MenuItem();
      this.menuItem4 = new System.Windows.Forms.MenuItem();
      this.menuItem8 = new System.Windows.Forms.MenuItem();
      this.menuItem5 = new System.Windows.Forms.MenuItem();
      this.menuItem13 = new System.Windows.Forms.MenuItem();
      this.menuItem14 = new System.Windows.Forms.MenuItem();
      this.menuItemStartTVserver = new System.Windows.Forms.MenuItem();
      this.menuItemStopTVserver = new System.Windows.Forms.MenuItem();
      this.menuItemClearWEventLogOnTVserver = new System.Windows.Forms.MenuItem();
      this.menuItem9 = new System.Windows.Forms.MenuItem();
      this.menuItemClearEventLogs = new System.Windows.Forms.MenuItem();
      this.menuItemClearMPlogs = new System.Windows.Forms.MenuItem();
      this.menuItem6 = new System.Windows.Forms.MenuItem();
      this.menuItem7 = new System.Windows.Forms.MenuItem();
      this.statusBar = new System.Windows.Forms.StatusBar();
      this.tmrUnAttended = new System.Windows.Forms.Timer(this.components);
      this.tmrMPWatcher = new System.Windows.Forms.Timer(this.components);
      this.tmrWatchdog = new System.Windows.Forms.Timer(this.components);
      this.NormalModeRadioButton = new System.Windows.Forms.RadioButton();
      this.ProceedButton = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.menuItemClearTVserverLogs = new System.Windows.Forms.MenuItem();
      this.settingsGroup.SuspendLayout();
      this.SuspendLayout();
      // 
      // settingsGroup
      // 
      this.settingsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.settingsGroup.Controls.Add(this.btnZipFileReset);
      this.settingsGroup.Controls.Add(this.btnZipFile);
      this.settingsGroup.Controls.Add(this.tbZipFile);
      this.settingsGroup.Controls.Add(this.logDirLabel);
      this.settingsGroup.Location = new System.Drawing.Point(12, 12);
      this.settingsGroup.Name = "settingsGroup";
      this.settingsGroup.Size = new System.Drawing.Size(410, 86);
      this.settingsGroup.TabIndex = 2;
      this.settingsGroup.TabStop = false;
      this.settingsGroup.Text = "Settings";
      // 
      // btnZipFileReset
      // 
      this.btnZipFileReset.Location = new System.Drawing.Point(333, 57);
      this.btnZipFileReset.Name = "btnZipFileReset";
      this.btnZipFileReset.Size = new System.Drawing.Size(64, 23);
      this.btnZipFileReset.TabIndex = 4;
      this.btnZipFileReset.Text = "Reset";
      this.btnZipFileReset.UseVisualStyleBackColor = true;
      this.btnZipFileReset.Click += new System.EventHandler(this.btnZipFileReset_Click);
      // 
      // btnZipFile
      // 
      this.btnZipFile.Location = new System.Drawing.Point(333, 32);
      this.btnZipFile.Name = "btnZipFile";
      this.btnZipFile.Size = new System.Drawing.Size(64, 23);
      this.btnZipFile.TabIndex = 3;
      this.btnZipFile.Text = "Browse";
      this.btnZipFile.Click += new System.EventHandler(this.btnZipFile_Click);
      // 
      // tbZipFile
      // 
      this.tbZipFile.Location = new System.Drawing.Point(6, 34);
      this.tbZipFile.Name = "tbZipFile";
      this.tbZipFile.Size = new System.Drawing.Size(321, 20);
      this.tbZipFile.TabIndex = 2;
      this.tbZipFile.TextChanged += new System.EventHandler(this.tbZipFile_TextChanged);
      // 
      // logDirLabel
      // 
      this.logDirLabel.Location = new System.Drawing.Point(6, 16);
      this.logDirLabel.Name = "logDirLabel";
      this.logDirLabel.Size = new System.Drawing.Size(152, 15);
      this.logDirLabel.TabIndex = 2;
      this.logDirLabel.Text = "Resulting ZIP of logs";
      // 
      // ExportLogsRadioButton
      // 
      this.ExportLogsRadioButton.AutoSize = true;
      this.ExportLogsRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
      this.ExportLogsRadioButton.Location = new System.Drawing.Point(12, 268);
      this.ExportLogsRadioButton.Name = "ExportLogsRadioButton";
      this.ExportLogsRadioButton.Size = new System.Drawing.Size(389, 17);
      this.ExportLogsRadioButton.TabIndex = 8;
      this.ExportLogsRadioButton.TabStop = true;
      this.ExportLogsRadioButton.Text = "Export all currently present logs from MediaPortal and TV Server";
      this.ExportLogsRadioButton.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.Location = new System.Drawing.Point(12, 124);
      this.label1.Name = "label1";
      this.label1.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
      this.label1.Size = new System.Drawing.Size(410, 39);
      this.label1.TabIndex = 1;
      this.label1.Text = "This will start MediaPortal using the default skin, and only plugins which were p" +
    "art of the release version you installed. No extensions will be loaded.";
      // 
      // SafeModeRadioButton
      // 
      this.SafeModeRadioButton.AutoSize = true;
      this.SafeModeRadioButton.Checked = true;
      this.SafeModeRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
      this.SafeModeRadioButton.Location = new System.Drawing.Point(12, 104);
      this.SafeModeRadioButton.Name = "SafeModeRadioButton";
      this.SafeModeRadioButton.Size = new System.Drawing.Size(221, 17);
      this.SafeModeRadioButton.TabIndex = 0;
      this.SafeModeRadioButton.TabStop = true;
      this.SafeModeRadioButton.Text = "Report a Bug to Team MediaPortal";
      this.SafeModeRadioButton.UseVisualStyleBackColor = true;
      // 
      // mainMenu
      // 
      this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem3,
            this.menuItem13,
            this.menuItem6});
      // 
      // menuItem1
      // 
      this.menuItem1.Index = 0;
      this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2});
      this.menuItem1.Text = "File";
      // 
      // menuItem2
      // 
      this.menuItem2.Checked = true;
      this.menuItem2.Index = 0;
      this.menuItem2.Text = "Exit";
      this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
      // 
      // menuItem3
      // 
      this.menuItem3.Index = 1;
      this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem4,
            this.menuItem8,
            this.menuItem5});
      this.menuItem3.Text = "Action";
      // 
      // menuItem4
      // 
      this.menuItem4.Index = 0;
      this.menuItem4.Text = "1. Perform pre-test actions";
      // 
      // menuItem8
      // 
      this.menuItem8.Index = 1;
      this.menuItem8.Text = "2. Launch MediaPortal";
      // 
      // menuItem5
      // 
      this.menuItem5.Index = 2;
      this.menuItem5.Text = "3. Perform post-test actions";
      // 
      // menuItem13
      // 
      this.menuItem13.Index = 2;
      this.menuItem13.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem14,
            this.menuItem9});
      this.menuItem13.Text = "Tools";
      // 
      // menuItem14
      // 
      this.menuItem14.Index = 0;
      this.menuItem14.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemStartTVserver,
            this.menuItemStopTVserver,
            this.menuItemClearWEventLogOnTVserver,
            this.menuItemClearTVserverLogs});
      this.menuItem14.Text = "Manage TV server";
      // 
      // menuItemStartTVserver
      // 
      this.menuItemStartTVserver.Index = 0;
      this.menuItemStartTVserver.Text = "Start TV Server";
      this.menuItemStartTVserver.Click += new System.EventHandler(this.menuItemStartTVserver_Click);
      // 
      // menuItemStopTVserver
      // 
      this.menuItemStopTVserver.Index = 1;
      this.menuItemStopTVserver.Text = "Stop TV Server";
      this.menuItemStopTVserver.Click += new System.EventHandler(this.menuItemStopTVserver_Click);
      // 
      // menuItemClearWEventLogOnTVserver
      // 
      this.menuItemClearWEventLogOnTVserver.Index = 2;
      this.menuItemClearWEventLogOnTVserver.Text = "Clear Windows EventLog";
      this.menuItemClearWEventLogOnTVserver.Click += new System.EventHandler(this.menuItemClearWEventLogOnTVserver_Click);
      // 
      // menuItem9
      // 
      this.menuItem9.Index = 1;
      this.menuItem9.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemClearEventLogs,
            this.menuItemClearMPlogs});
      this.menuItem9.Text = "Manage MediaPortal Client";
      // 
      // menuItemClearEventLogs
      // 
      this.menuItemClearEventLogs.Index = 0;
      this.menuItemClearEventLogs.Text = "Clear Windows EventLogs";
      this.menuItemClearEventLogs.Click += new System.EventHandler(this.menuItemClearEventLogs_Click);
      // 
      // menuItemClearMPlogs
      // 
      this.menuItemClearMPlogs.Index = 1;
      this.menuItemClearMPlogs.Text = "Clear MediaPortal logs";
      this.menuItemClearMPlogs.Click += new System.EventHandler(this.menuItemClearMPlogs_Click);
      // 
      // menuItem6
      // 
      this.menuItem6.Index = 3;
      this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem7});
      this.menuItem6.Text = "Help";
      // 
      // menuItem7
      // 
      this.menuItem7.Index = 0;
      this.menuItem7.Text = "About";
      this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
      // 
      // statusBar
      // 
      this.statusBar.Location = new System.Drawing.Point(0, 344);
      this.statusBar.Name = "statusBar";
      this.statusBar.Size = new System.Drawing.Size(434, 20);
      this.statusBar.TabIndex = 6;
      this.statusBar.Text = "Status: Idle";
      // 
      // tmrUnAttended
      // 
      this.tmrUnAttended.Interval = 1000;
      this.tmrUnAttended.Tick += new System.EventHandler(this.tmrUnAttended_Tick);
      // 
      // tmrMPWatcher
      // 
      this.tmrMPWatcher.Interval = 5000;
      this.tmrMPWatcher.Tick += new System.EventHandler(this.tmrMPWatcher_Tick);
      // 
      // tmrWatchdog
      // 
      this.tmrWatchdog.Interval = 1000;
      this.tmrWatchdog.Tick += new System.EventHandler(this.tmrWatchdog_Tick);
      // 
      // NormalModeRadioButton
      // 
      this.NormalModeRadioButton.AutoSize = true;
      this.NormalModeRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
      this.NormalModeRadioButton.Location = new System.Drawing.Point(12, 186);
      this.NormalModeRadioButton.Name = "NormalModeRadioButton";
      this.NormalModeRadioButton.Size = new System.Drawing.Size(325, 17);
      this.NormalModeRadioButton.TabIndex = 1;
      this.NormalModeRadioButton.TabStop = true;
      this.NormalModeRadioButton.Text = "Report a Bug to a Plugin Developer or Skin Designer";
      this.NormalModeRadioButton.UseVisualStyleBackColor = true;
      // 
      // ProceedButton
      // 
      this.ProceedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.ProceedButton.Location = new System.Drawing.Point(334, 315);
      this.ProceedButton.Name = "ProceedButton";
      this.ProceedButton.Size = new System.Drawing.Size(75, 23);
      this.ProceedButton.TabIndex = 8;
      this.ProceedButton.Text = "Proceed";
      this.ProceedButton.UseVisualStyleBackColor = true;
      this.ProceedButton.Click += new System.EventHandler(this.ProceedButton_Click);
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.Location = new System.Drawing.Point(12, 206);
      this.label2.Name = "label2";
      this.label2.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
      this.label2.Size = new System.Drawing.Size(398, 27);
      this.label2.TabIndex = 2;
      this.label2.Text = "Besides setting the log level to \"debug\", this option will start MediaPortal as c" +
    "onfigured, using all extensions you have installed.";
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label3.Location = new System.Drawing.Point(12, 288);
      this.label3.Name = "label3";
      this.label3.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
      this.label3.Size = new System.Drawing.Size(410, 24);
      this.label3.TabIndex = 9;
      this.label3.Text = "If MediaPortal crashes unexpectedly, or if you can not reproduce an issue nicely," +
    " then this option will simply export all the currently available log files.";
      // 
      // menuItemClearTVserverLogs
      // 
      this.menuItemClearTVserverLogs.Index = 3;
      this.menuItemClearTVserverLogs.Text = "Clear TV Server logs";
      this.menuItemClearTVserverLogs.Click += new System.EventHandler(this.menuItemClearTVserverLogs_Click);
      // 
      // MPWatchDog
      // 
      this.AcceptButton = this.ProceedButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(434, 364);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.ExportLogsRadioButton);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.SafeModeRadioButton);
      this.Controls.Add(this.NormalModeRadioButton);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.ProceedButton);
      this.Controls.Add(this.statusBar);
      this.Controls.Add(this.settingsGroup);
      this.Menu = this.mainMenu;
      this.Name = "MPWatchDog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "MediaPortal Watchdog";
      this.settingsGroup.ResumeLayout(false);
      this.settingsGroup.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox settingsGroup;
    private System.Windows.Forms.Button btnZipFile;
    private System.Windows.Forms.TextBox tbZipFile;
    private System.Windows.Forms.Label logDirLabel;
    private System.Windows.Forms.MainMenu mainMenu;
    private System.Windows.Forms.MenuItem menuItem1;
    private System.Windows.Forms.MenuItem menuItem2;
    private System.Windows.Forms.MenuItem menuItem3;
    private System.Windows.Forms.MenuItem menuItem4;
    private System.Windows.Forms.MenuItem menuItem5;
    private System.Windows.Forms.MenuItem menuItem6;
    private System.Windows.Forms.MenuItem menuItem7;
    private System.Windows.Forms.StatusBar statusBar;
    private System.Windows.Forms.MenuItem menuItem8;
    private System.Windows.Forms.Timer tmrUnAttended;
    private System.Windows.Forms.Timer tmrMPWatcher;
    private System.Windows.Forms.Timer tmrWatchdog;
    private System.Windows.Forms.RadioButton SafeModeRadioButton;
    private System.Windows.Forms.RadioButton ExportLogsRadioButton;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.RadioButton NormalModeRadioButton;
    private System.Windows.Forms.Button ProceedButton;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnZipFileReset;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.MenuItem menuItem13;
    private System.Windows.Forms.MenuItem menuItem14;
    private System.Windows.Forms.MenuItem menuItemStartTVserver;
    private System.Windows.Forms.MenuItem menuItemStopTVserver;
    private System.Windows.Forms.MenuItem menuItem9;
    private System.Windows.Forms.MenuItem menuItemClearEventLogs;
    private System.Windows.Forms.MenuItem menuItemClearMPlogs;
    private System.Windows.Forms.MenuItem menuItemClearWEventLogOnTVserver;
    private System.Windows.Forms.MenuItem menuItemClearTVserverLogs;
  }
}