namespace MTV.Scheduler.App.UI
{
    partial class MainGuiUserControl
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
            this.popUpContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
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
            this.columnStopperSegment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnMPEG7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnVODCMD = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnARExtended = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSTARTNotification = new System.Windows.Forms.TabPage();
            this.ListViewSegment = new System.Windows.Forms.ListView();
            this.columnSegmentCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTITLE = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTIME = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDURATION = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSegmentState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTotalDuration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnReRuns = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnStart_tc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnMaterialType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPlayoutReply = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPlayoutReplyTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPageDuplexPushNotifcation = new System.Windows.Forms.TabPage();
            this.richTextBoxDuplexPushNotification = new System.Windows.Forms.RichTextBox();
            this.tabPageFilesNotification = new System.Windows.Forms.TabPage();
            this.richTextBoxFilesNotification = new System.Windows.Forms.RichTextBox();
            this.popUpContextMenu.SuspendLayout();
            this.logContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageSTARTNotification.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPageDuplexPushNotifcation.SuspendLayout();
            this.tabPageFilesNotification.SuspendLayout();
            this.SuspendLayout();
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
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
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
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer1.Size = new System.Drawing.Size(1000, 800);
            this.splitContainer1.SplitterDistance = 500;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer2.Size = new System.Drawing.Size(1000, 500);
            this.splitContainer2.SplitterDistance = 285;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage2);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(1000, 285);
            this.tabControl3.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ListViewAutorec);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(992, 259);
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
            this.columnStopperSegment,
            this.columnMPEG7,
            this.columnVODCMD,
            this.columnARExtended});
            this.ListViewAutorec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListViewAutorec.FullRowSelect = true;
            this.ListViewAutorec.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ListViewAutorec.HideSelection = false;
            this.ListViewAutorec.Location = new System.Drawing.Point(3, 3);
            this.ListViewAutorec.Name = "ListViewAutorec";
            this.ListViewAutorec.ShowGroups = false;
            this.ListViewAutorec.ShowItemToolTips = true;
            this.ListViewAutorec.Size = new System.Drawing.Size(986, 253);
            this.ListViewAutorec.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.ListViewAutorec.TabIndex = 4;
            this.ListViewAutorec.UseCompatibleStateImageBehavior = false;
            this.ListViewAutorec.View = System.Windows.Forms.View.Details;
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
            // columnStopperSegment
            // 
            this.columnStopperSegment.Text = "Next Segment";
            this.columnStopperSegment.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // columnARExtended
            // 
            this.columnARExtended.Text = "Extended Cmd";
            this.columnARExtended.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSTARTNotification);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1000, 211);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.Visible = false;
            // 
            // tabPageSTARTNotification
            // 
            this.tabPageSTARTNotification.Controls.Add(this.ListViewSegment);
            this.tabPageSTARTNotification.Location = new System.Drawing.Point(4, 22);
            this.tabPageSTARTNotification.Name = "tabPageSTARTNotification";
            this.tabPageSTARTNotification.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSTARTNotification.Size = new System.Drawing.Size(992, 185);
            this.tabPageSTARTNotification.TabIndex = 1;
            this.tabPageSTARTNotification.Text = "Episode(s) Viewer";
            this.tabPageSTARTNotification.UseVisualStyleBackColor = true;
            // 
            // ListViewSegment
            // 
            this.ListViewSegment.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnSegmentCode,
            this.columnTITLE,
            this.columnTIME,
            this.columnDURATION,
            this.columnSegmentState,
            this.columnTotalDuration,
            this.columnReRuns,
            this.columnStart_tc,
            this.columnMaterialType,
            this.columnPlayoutReply,
            this.columnPlayoutReplyTime});
            this.ListViewSegment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListViewSegment.FullRowSelect = true;
            this.ListViewSegment.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ListViewSegment.HideSelection = false;
            this.ListViewSegment.Location = new System.Drawing.Point(3, 3);
            this.ListViewSegment.Name = "ListViewSegment";
            this.ListViewSegment.ShowGroups = false;
            this.ListViewSegment.ShowItemToolTips = true;
            this.ListViewSegment.Size = new System.Drawing.Size(986, 179);
            this.ListViewSegment.TabIndex = 3;
            this.ListViewSegment.UseCompatibleStateImageBehavior = false;
            this.ListViewSegment.View = System.Windows.Forms.View.Details;
            // 
            // columnSegmentCode
            // 
            this.columnSegmentCode.Text = "Episode ID";
            this.columnSegmentCode.Width = 121;
            // 
            // columnTITLE
            // 
            this.columnTITLE.Text = "Title";
            this.columnTITLE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnTITLE.Width = 78;
            // 
            // columnTIME
            // 
            this.columnTIME.Text = "T Start";
            this.columnTIME.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnTIME.Width = 79;
            // 
            // columnDURATION
            // 
            this.columnDURATION.Text = "Duration";
            this.columnDURATION.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnDURATION.Width = 138;
            // 
            // columnSegmentState
            // 
            this.columnSegmentState.Text = "Status";
            this.columnSegmentState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnSegmentState.Width = 108;
            // 
            // columnTotalDuration
            // 
            this.columnTotalDuration.Text = "T.Duration";
            this.columnTotalDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnTotalDuration.Width = 127;
            // 
            // columnReRuns
            // 
            this.columnReRuns.Text = "Re-Runs";
            this.columnReRuns.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnReRuns.Width = 96;
            // 
            // columnStart_tc
            // 
            this.columnStart_tc.Text = "Start_TC";
            this.columnStart_tc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnStart_tc.Width = 101;
            // 
            // columnMaterialType
            // 
            this.columnMaterialType.Text = "Material Type";
            this.columnMaterialType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnMaterialType.Width = 116;
            // 
            // columnPlayoutReply
            // 
            this.columnPlayoutReply.Text = "Playout Reply";
            this.columnPlayoutReply.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnPlayoutReply.Width = 50;
            // 
            // columnPlayoutReplyTime
            // 
            this.columnPlayoutReplyTime.Text = "T Playout Reply";
            this.columnPlayoutReplyTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnPlayoutReplyTime.Width = 100;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPageDuplexPushNotifcation);
            this.tabControl2.Controls.Add(this.tabPageFilesNotification);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1000, 296);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPageDuplexPushNotifcation
            // 
            this.tabPageDuplexPushNotifcation.Controls.Add(this.richTextBoxDuplexPushNotification);
            this.tabPageDuplexPushNotifcation.Location = new System.Drawing.Point(4, 22);
            this.tabPageDuplexPushNotifcation.Name = "tabPageDuplexPushNotifcation";
            this.tabPageDuplexPushNotifcation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDuplexPushNotifcation.Size = new System.Drawing.Size(992, 270);
            this.tabPageDuplexPushNotifcation.TabIndex = 2;
            this.tabPageDuplexPushNotifcation.Text = "MTV.EventDequeuer.Service";
            this.tabPageDuplexPushNotifcation.UseVisualStyleBackColor = true;
            // 
            // richTextBoxDuplexPushNotification
            // 
            this.richTextBoxDuplexPushNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxDuplexPushNotification.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxDuplexPushNotification.Name = "richTextBoxDuplexPushNotification";
            this.richTextBoxDuplexPushNotification.Size = new System.Drawing.Size(986, 264);
            this.richTextBoxDuplexPushNotification.TabIndex = 1;
            this.richTextBoxDuplexPushNotification.Text = "";
            // 
            // tabPageFilesNotification
            // 
            this.tabPageFilesNotification.Controls.Add(this.richTextBoxFilesNotification);
            this.tabPageFilesNotification.Location = new System.Drawing.Point(4, 22);
            this.tabPageFilesNotification.Name = "tabPageFilesNotification";
            this.tabPageFilesNotification.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFilesNotification.Size = new System.Drawing.Size(992, 270);
            this.tabPageFilesNotification.TabIndex = 3;
            this.tabPageFilesNotification.Text = "Files Notification ";
            this.tabPageFilesNotification.UseVisualStyleBackColor = true;
            // 
            // richTextBoxFilesNotification
            // 
            this.richTextBoxFilesNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxFilesNotification.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxFilesNotification.Name = "richTextBoxFilesNotification";
            this.richTextBoxFilesNotification.Size = new System.Drawing.Size(986, 264);
            this.richTextBoxFilesNotification.TabIndex = 1;
            this.richTextBoxFilesNotification.Text = "";
            // 
            // MainGuiUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainGuiUserControl";
            this.Size = new System.Drawing.Size(1000, 800);
            this.popUpContextMenu.ResumeLayout(false);
            this.logContextMenu.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageSTARTNotification.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPageDuplexPushNotifcation.ResumeLayout(false);
            this.tabPageFilesNotification.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip popUpContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip logContextMenu;
        private System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSTARTNotification;
        private System.Windows.Forms.ListView ListViewSegment;
        private System.Windows.Forms.ColumnHeader columnSegmentCode;
        private System.Windows.Forms.ColumnHeader columnTITLE;
        private System.Windows.Forms.ColumnHeader columnTIME;
        private System.Windows.Forms.ColumnHeader columnDURATION;
        private System.Windows.Forms.ColumnHeader columnSegmentState;
        private System.Windows.Forms.ColumnHeader columnTotalDuration;
        private System.Windows.Forms.ColumnHeader columnReRuns;
        private System.Windows.Forms.ColumnHeader columnStart_tc;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPageDuplexPushNotifcation;
        private System.Windows.Forms.RichTextBox richTextBoxDuplexPushNotification;
        private System.Windows.Forms.TabPage tabPageFilesNotification;
        private System.Windows.Forms.RichTextBox richTextBoxFilesNotification;
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
        private System.Windows.Forms.ColumnHeader columnMaterialType;
        private System.Windows.Forms.ColumnHeader columnTriggerType;
        private System.Windows.Forms.ColumnHeader columnPlayoutReply;
        private System.Windows.Forms.ColumnHeader columnPlayoutReplyTime;
        private System.Windows.Forms.ColumnHeader columnPosterState;
        private System.Windows.Forms.ColumnHeader columnDummyCommandStatus;
        private System.Windows.Forms.ColumnHeader columnStopperSegment;
        private System.Windows.Forms.ColumnHeader columnMPEG7;
        private System.Windows.Forms.ColumnHeader columnVODCMD;
        private System.Windows.Forms.ColumnHeader columnARExtended;
    }
}
