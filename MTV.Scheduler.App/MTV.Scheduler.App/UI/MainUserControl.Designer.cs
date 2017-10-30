namespace MTV.Scheduler.App.UI
{
    partial class MainUserControl
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ListViewAutorec = new System.Windows.Forms.ListView();
            this.columnOriginalTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEstimatedStart = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEstimatedStop = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnchannel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnRealDuration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnElapsedDuration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnCreatedDateTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderContentID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderMediaType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTriggerType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPosterState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDummyCommandStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnMPEG7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnVODCMD = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPageDuplexPushNotifcation = new System.Windows.Forms.TabPage();
            this.richTextBoxDuplexPushNotification = new System.Windows.Forms.RichTextBox();
            this.tabPageFilesNotification = new System.Windows.Forms.TabPage();
            this.richTextBoxFilesNotification = new System.Windows.Forms.RichTextBox();
            this.tabPageIncomingContents = new System.Windows.Forms.TabPage();
            this.richTextBoxMonitroingSystem = new System.Windows.Forms.RichTextBox();
            this.popUpContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPageDuplexPushNotifcation.SuspendLayout();
            this.tabPageFilesNotification.SuspendLayout();
            this.tabPageIncomingContents.SuspendLayout();
            this.popUpContextMenu.SuspendLayout();
            this.logContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer1.Size = new System.Drawing.Size(1000, 800);
            this.splitContainer1.SplitterDistance = 333;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage2);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(1000, 333);
            this.tabControl3.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ListViewAutorec);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(992, 307);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Schedule(s) Viewer";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ListViewAutorec
            // 
            this.ListViewAutorec.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnOriginalTitle,
            this.columnEstimatedStart,
            this.columnEstimatedStop,
            this.columnchannel,
            this.columnRealDuration,
            this.columnElapsedDuration,
            this.columnState,
            this.columnCreatedDateTime,
            this.columnHeaderContentID,
            this.columnHeaderMediaType,
            this.columnTriggerType,
            this.columnPosterState,
            this.columnDummyCommandStatus,
            this.columnMPEG7,
            this.columnVODCMD});
            this.ListViewAutorec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListViewAutorec.FullRowSelect = true;
            this.ListViewAutorec.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ListViewAutorec.HideSelection = false;
            this.ListViewAutorec.Location = new System.Drawing.Point(3, 3);
            this.ListViewAutorec.Name = "ListViewAutorec";
            this.ListViewAutorec.ShowGroups = false;
            this.ListViewAutorec.ShowItemToolTips = true;
            this.ListViewAutorec.Size = new System.Drawing.Size(986, 301);
            this.ListViewAutorec.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.ListViewAutorec.TabIndex = 4;
            this.ListViewAutorec.UseCompatibleStateImageBehavior = false;
            this.ListViewAutorec.View = System.Windows.Forms.View.Details;
            this.ListViewAutorec.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListViewAutorec_ItemSelectionChanged);
            // 
            // columnOriginalTitle
            // 
            this.columnOriginalTitle.Text = "Title";
            // 
            // columnEstimatedStart
            // 
            this.columnEstimatedStart.Text = "T Start";
            this.columnEstimatedStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnEstimatedStart.Width = 137;
            // 
            // columnEstimatedStop
            // 
            this.columnEstimatedStop.Text = "T End";
            this.columnEstimatedStop.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnEstimatedStop.Width = 148;
            // 
            // columnchannel
            // 
            this.columnchannel.Text = "Channel";
            this.columnchannel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnchannel.Width = 96;
            // 
            // columnRealDuration
            // 
            this.columnRealDuration.Text = "R.Duration";
            this.columnRealDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnRealDuration.Width = 126;
            // 
            // columnElapsedDuration
            // 
            this.columnElapsedDuration.Text = "E.Duration";
            this.columnElapsedDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnElapsedDuration.Width = 135;
            // 
            // columnState
            // 
            this.columnState.Text = "Status";
            this.columnState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnCreatedDateTime
            // 
            this.columnCreatedDateTime.Text = "T Creation";
            this.columnCreatedDateTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeaderContentID
            // 
            this.columnHeaderContentID.Text = "Content ID";
            this.columnHeaderContentID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeaderMediaType
            // 
            this.columnHeaderMediaType.Text = "Media Type";
            this.columnHeaderMediaType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnTriggerType
            // 
            this.columnTriggerType.Text = "Trigger Type";
            this.columnTriggerType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnPosterState
            // 
            this.columnPosterState.Text = "Poster";
            this.columnPosterState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnDummyCommandStatus
            // 
            this.columnDummyCommandStatus.Text = "Dummy Cmd";
            this.columnDummyCommandStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnMPEG7
            // 
            this.columnMPEG7.Text = "MPEG7";
            this.columnMPEG7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnVODCMD
            // 
            this.columnVODCMD.Text = "DC Cmd";
            this.columnVODCMD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPageDuplexPushNotifcation);
            this.tabControl2.Controls.Add(this.tabPageFilesNotification);
            this.tabControl2.Controls.Add(this.tabPageIncomingContents);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1000, 463);
            this.tabControl2.TabIndex = 2;
            // 
            // tabPageDuplexPushNotifcation
            // 
            this.tabPageDuplexPushNotifcation.Controls.Add(this.richTextBoxDuplexPushNotification);
            this.tabPageDuplexPushNotifcation.Location = new System.Drawing.Point(4, 22);
            this.tabPageDuplexPushNotifcation.Name = "tabPageDuplexPushNotifcation";
            this.tabPageDuplexPushNotifcation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDuplexPushNotifcation.Size = new System.Drawing.Size(992, 437);
            this.tabPageDuplexPushNotifcation.TabIndex = 2;
            this.tabPageDuplexPushNotifcation.Text = "Realtime tracking Scheduled/Modified Contents";
            this.tabPageDuplexPushNotifcation.UseVisualStyleBackColor = true;
            // 
            // richTextBoxDuplexPushNotification
            // 
            this.richTextBoxDuplexPushNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxDuplexPushNotification.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxDuplexPushNotification.Name = "richTextBoxDuplexPushNotification";
            this.richTextBoxDuplexPushNotification.Size = new System.Drawing.Size(986, 431);
            this.richTextBoxDuplexPushNotification.TabIndex = 1;
            this.richTextBoxDuplexPushNotification.Text = "";
            // 
            // tabPageFilesNotification
            // 
            this.tabPageFilesNotification.Controls.Add(this.richTextBoxFilesNotification);
            this.tabPageFilesNotification.Location = new System.Drawing.Point(4, 22);
            this.tabPageFilesNotification.Name = "tabPageFilesNotification";
            this.tabPageFilesNotification.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFilesNotification.Size = new System.Drawing.Size(992, 437);
            this.tabPageFilesNotification.TabIndex = 3;
            this.tabPageFilesNotification.Text = "I/O Monitoring";
            this.tabPageFilesNotification.UseVisualStyleBackColor = true;
            // 
            // richTextBoxFilesNotification
            // 
            this.richTextBoxFilesNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxFilesNotification.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxFilesNotification.Name = "richTextBoxFilesNotification";
            this.richTextBoxFilesNotification.Size = new System.Drawing.Size(986, 431);
            this.richTextBoxFilesNotification.TabIndex = 1;
            this.richTextBoxFilesNotification.Text = "";
            // 
            // tabPageIncomingContents
            // 
            this.tabPageIncomingContents.Controls.Add(this.richTextBoxMonitroingSystem);
            this.tabPageIncomingContents.Location = new System.Drawing.Point(4, 22);
            this.tabPageIncomingContents.Name = "tabPageIncomingContents";
            this.tabPageIncomingContents.Size = new System.Drawing.Size(992, 437);
            this.tabPageIncomingContents.TabIndex = 4;
            this.tabPageIncomingContents.Text = "Internal Service Monitoring";
            this.tabPageIncomingContents.UseVisualStyleBackColor = true;
            // 
            // richTextBoxMonitroingSystem
            // 
            this.richTextBoxMonitroingSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMonitroingSystem.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxMonitroingSystem.Name = "richTextBoxMonitroingSystem";
            this.richTextBoxMonitroingSystem.Size = new System.Drawing.Size(992, 437);
            this.richTextBoxMonitroingSystem.TabIndex = 2;
            this.richTextBoxMonitroingSystem.Text = "";
            // 
            // popUpContextMenu
            // 
            this.popUpContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.popUpContextMenu.Name = "contextMenuStrip1";
            this.popUpContextMenu.Size = new System.Drawing.Size(180, 26);
            this.popUpContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.popUpContextMenu_Opening);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.removeToolStripMenuItem.Text = "Remove Completed";
            // 
            // logContextMenu
            // 
            this.logContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearLogToolStripMenuItem});
            this.logContextMenu.Name = "logContextMenu";
            this.logContextMenu.Size = new System.Drawing.Size(125, 26);
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.clearLogToolStripMenuItem.Text = "Clear Log";
            // 
            // MainUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainUserControl";
            this.Size = new System.Drawing.Size(1000, 800);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPageDuplexPushNotifcation.ResumeLayout(false);
            this.tabPageFilesNotification.ResumeLayout(false);
            this.tabPageIncomingContents.ResumeLayout(false);
            this.popUpContextMenu.ResumeLayout(false);
            this.logContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView ListViewAutorec;
        private System.Windows.Forms.ColumnHeader columnOriginalTitle;
        private System.Windows.Forms.ColumnHeader columnEstimatedStart;
        private System.Windows.Forms.ColumnHeader columnEstimatedStop;
        private System.Windows.Forms.ColumnHeader columnchannel;
        private System.Windows.Forms.ColumnHeader columnRealDuration;
        private System.Windows.Forms.ColumnHeader columnElapsedDuration;
        private System.Windows.Forms.ColumnHeader columnState;
        private System.Windows.Forms.ColumnHeader columnCreatedDateTime;
        private System.Windows.Forms.ColumnHeader columnHeaderContentID;
        private System.Windows.Forms.ColumnHeader columnHeaderMediaType;
        private System.Windows.Forms.ColumnHeader columnTriggerType;
        private System.Windows.Forms.ColumnHeader columnPosterState;
        private System.Windows.Forms.ColumnHeader columnDummyCommandStatus;
        private System.Windows.Forms.ColumnHeader columnMPEG7;
        private System.Windows.Forms.ColumnHeader columnVODCMD;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPageDuplexPushNotifcation;
        private System.Windows.Forms.RichTextBox richTextBoxDuplexPushNotification;
        private System.Windows.Forms.TabPage tabPageFilesNotification;
        private System.Windows.Forms.RichTextBox richTextBoxFilesNotification;
        private System.Windows.Forms.ContextMenuStrip popUpContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip logContextMenu;
        private System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPageIncomingContents;
        private System.Windows.Forms.RichTextBox richTextBoxMonitroingSystem;
    }
}
