namespace CMESchedulerApp.UI
{
    partial class EventList
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lvwPrograms = new System.Windows.Forms.ListView();
            this.columnOriginalTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEstimatedStart = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEstimatedStop = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnchannel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnRealDuration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnElapsedDuration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnCreatedDateTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderContentID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.popUpContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabSegmentsLogs = new System.Windows.Forms.TabControl();
            this.tabSegments = new System.Windows.Forms.TabPage();
            this.lvwSegments = new System.Windows.Forms.ListView();
            this.columnSegmentCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTITLE = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTIME = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDURATION = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSegmentState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTotalDuration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnReRuns = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabLog = new System.Windows.Forms.TabPage();
            this.richLog = new System.Windows.Forms.RichTextBox();
            this.logContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageNotification = new System.Windows.Forms.TabPage();
            this.richTxtNotification = new System.Windows.Forms.RichTextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTxtFilesNotification = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.popUpContextMenu.SuspendLayout();
            this.tabSegmentsLogs.SuspendLayout();
            this.tabSegments.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.logContextMenu.SuspendLayout();
            this.tabPageNotification.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lvwPrograms);
            this.splitContainer2.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer2_Panel1_Paint);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabSegmentsLogs);
            this.splitContainer2.Size = new System.Drawing.Size(722, 436);
            this.splitContainer2.SplitterDistance = 211;
            this.splitContainer2.TabIndex = 16;
            // 
            // lvwPrograms
            // 
            this.lvwPrograms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnOriginalTitle,
            this.columnEstimatedStart,
            this.columnEstimatedStop,
            this.columnchannel,
            this.columnRealDuration,
            this.columnElapsedDuration,
            this.columnState,
            this.columnCreatedDateTime,
            this.columnHeaderContentID});
            this.lvwPrograms.ContextMenuStrip = this.popUpContextMenu;
            this.lvwPrograms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwPrograms.FullRowSelect = true;
            this.lvwPrograms.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwPrograms.HideSelection = false;
            this.lvwPrograms.Location = new System.Drawing.Point(0, 0);
            this.lvwPrograms.Name = "lvwPrograms";
            this.lvwPrograms.ShowGroups = false;
            this.lvwPrograms.ShowItemToolTips = true;
            this.lvwPrograms.Size = new System.Drawing.Size(722, 211);
            this.lvwPrograms.TabIndex = 2;
            this.lvwPrograms.UseCompatibleStateImageBehavior = false;
            this.lvwPrograms.View = System.Windows.Forms.View.Details;
            this.lvwPrograms.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvwPrograms_ItemSelectionChanged);
            this.lvwPrograms.SelectedIndexChanged += new System.EventHandler(this.lvwPrograms_SelectedIndexChanged);
            // 
            // columnOriginalTitle
            // 
            this.columnOriginalTitle.Text = "TITLE";
            // 
            // columnEstimatedStart
            // 
            this.columnEstimatedStart.Text = "ESTIMATED START";
            this.columnEstimatedStart.Width = 99;
            // 
            // columnEstimatedStop
            // 
            this.columnEstimatedStop.Text = "ESTIMATED STOP";
            this.columnEstimatedStop.Width = 148;
            // 
            // columnchannel
            // 
            this.columnchannel.Text = "CHANNEL";
            this.columnchannel.Width = 96;
            // 
            // columnRealDuration
            // 
            this.columnRealDuration.Text = "REAL DURATION";
            // 
            // columnElapsedDuration
            // 
            this.columnElapsedDuration.Text = "ELAPSED DURATION";
            // 
            // columnState
            // 
            this.columnState.Text = "STATE";
            // 
            // columnCreatedDateTime
            // 
            this.columnCreatedDateTime.Text = "CREATED ON";
            // 
            // columnHeaderContentID
            // 
            this.columnHeaderContentID.Text = "ContentID";
            // 
            // popUpContextMenu
            // 
            this.popUpContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.popUpContextMenu.Name = "contextMenuStrip1";
            this.popUpContextMenu.Size = new System.Drawing.Size(168, 26);
            this.popUpContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.popUpContextMenu_Opening);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.removeToolStripMenuItem.Text = "Remove Completed";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // tabSegmentsLogs
            // 
            this.tabSegmentsLogs.Controls.Add(this.tabSegments);
            this.tabSegmentsLogs.Controls.Add(this.tabLog);
            this.tabSegmentsLogs.Controls.Add(this.tabPageNotification);
            this.tabSegmentsLogs.Controls.Add(this.tabPage1);
            this.tabSegmentsLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSegmentsLogs.Location = new System.Drawing.Point(0, 0);
            this.tabSegmentsLogs.Name = "tabSegmentsLogs";
            this.tabSegmentsLogs.SelectedIndex = 0;
            this.tabSegmentsLogs.Size = new System.Drawing.Size(722, 221);
            this.tabSegmentsLogs.TabIndex = 1;
            // 
            // tabSegments
            // 
            this.tabSegments.Controls.Add(this.lvwSegments);
            this.tabSegments.Location = new System.Drawing.Point(4, 22);
            this.tabSegments.Name = "tabSegments";
            this.tabSegments.Padding = new System.Windows.Forms.Padding(3);
            this.tabSegments.Size = new System.Drawing.Size(714, 195);
            this.tabSegments.TabIndex = 0;
            this.tabSegments.Text = "Segments";
            this.tabSegments.UseVisualStyleBackColor = true;
            // 
            // lvwSegments
            // 
            this.lvwSegments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwSegments.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnSegmentCode,
            this.columnTITLE,
            this.columnTIME,
            this.columnDURATION,
            this.columnSegmentState,
            this.columnTotalDuration,
            this.columnReRuns});
            this.lvwSegments.FullRowSelect = true;
            this.lvwSegments.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwSegments.HideSelection = false;
            this.lvwSegments.Location = new System.Drawing.Point(3, 30);
            this.lvwSegments.Name = "lvwSegments";
            this.lvwSegments.ShowGroups = false;
            this.lvwSegments.ShowItemToolTips = true;
            this.lvwSegments.Size = new System.Drawing.Size(708, 162);
            this.lvwSegments.TabIndex = 1;
            this.lvwSegments.UseCompatibleStateImageBehavior = false;
            this.lvwSegments.View = System.Windows.Forms.View.Details;
            this.lvwSegments.SelectedIndexChanged += new System.EventHandler(this.lvwSegments_SelectedIndexChanged);
            // 
            // columnSegmentCode
            // 
            this.columnSegmentCode.Text = "SEGMENT ID";
            this.columnSegmentCode.Width = 121;
            // 
            // columnTITLE
            // 
            this.columnTITLE.Text = "TITLE";
            // 
            // columnTIME
            // 
            this.columnTIME.Text = "TIME";
            // 
            // columnDURATION
            // 
            this.columnDURATION.Text = "DURATION";
            // 
            // columnSegmentState
            // 
            this.columnSegmentState.Text = "STATE";
            // 
            // columnTotalDuration
            // 
            this.columnTotalDuration.Text = "TOTAL DURATION";
            // 
            // columnReRuns
            // 
            this.columnReRuns.Text = "RE-RUNS";
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.richLog);
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(714, 195);
            this.tabLog.TabIndex = 1;
            this.tabLog.Text = "STAR Notification";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // richLog
            // 
            this.richLog.BackColor = System.Drawing.Color.White;
            this.richLog.ContextMenuStrip = this.logContextMenu;
            this.richLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richLog.Location = new System.Drawing.Point(3, 3);
            this.richLog.Name = "richLog";
            this.richLog.ReadOnly = true;
            this.richLog.Size = new System.Drawing.Size(708, 189);
            this.richLog.TabIndex = 0;
            this.richLog.Text = "";
            this.richLog.WordWrap = false;
            this.richLog.TextChanged += new System.EventHandler(this.richLog_TextChanged);
            // 
            // logContextMenu
            // 
            this.logContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearLogToolStripMenuItem});
            this.logContextMenu.Name = "logContextMenu";
            this.logContextMenu.Size = new System.Drawing.Size(120, 26);
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.clearLogToolStripMenuItem.Text = "Clear Log";
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // tabPageNotification
            // 
            this.tabPageNotification.BackColor = System.Drawing.Color.White;
            this.tabPageNotification.Controls.Add(this.richTxtNotification);
            this.tabPageNotification.Location = new System.Drawing.Point(4, 22);
            this.tabPageNotification.Name = "tabPageNotification";
            this.tabPageNotification.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNotification.Size = new System.Drawing.Size(714, 195);
            this.tabPageNotification.TabIndex = 2;
            this.tabPageNotification.Text = "PUSH Notification";
            // 
            // richTxtNotification
            // 
            this.richTxtNotification.BackColor = System.Drawing.Color.White;
            this.richTxtNotification.ContextMenuStrip = this.logContextMenu;
            this.richTxtNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTxtNotification.Location = new System.Drawing.Point(3, 3);
            this.richTxtNotification.Name = "richTxtNotification";
            this.richTxtNotification.ReadOnly = true;
            this.richTxtNotification.Size = new System.Drawing.Size(708, 189);
            this.richTxtNotification.TabIndex = 1;
            this.richTxtNotification.Text = "";
            this.richTxtNotification.WordWrap = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTxtFilesNotification);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(714, 195);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "Files Notification";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTxtFilesNotification
            // 
            this.richTxtFilesNotification.AcceptsTab = true;
            this.richTxtFilesNotification.BackColor = System.Drawing.Color.White;
            this.richTxtFilesNotification.ContextMenuStrip = this.logContextMenu;
            this.richTxtFilesNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTxtFilesNotification.Location = new System.Drawing.Point(3, 3);
            this.richTxtFilesNotification.Name = "richTxtFilesNotification";
            this.richTxtFilesNotification.ReadOnly = true;
            this.richTxtFilesNotification.Size = new System.Drawing.Size(708, 189);
            this.richTxtFilesNotification.TabIndex = 2;
            this.richTxtFilesNotification.Text = "";
            this.richTxtFilesNotification.WordWrap = false;
            // 
            // EventList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer2);
            this.Name = "EventList";
            this.Size = new System.Drawing.Size(722, 436);
            this.Load += new System.EventHandler(this.EventList_Load);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.popUpContextMenu.ResumeLayout(false);
            this.tabSegmentsLogs.ResumeLayout(false);
            this.tabSegments.ResumeLayout(false);
            this.tabLog.ResumeLayout(false);
            this.logContextMenu.ResumeLayout(false);
            this.tabPageNotification.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabSegmentsLogs;
        private System.Windows.Forms.TabPage tabSegments;
        private System.Windows.Forms.ListView lvwSegments;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.RichTextBox richLog;
        private System.Windows.Forms.ListView lvwPrograms;
        private System.Windows.Forms.ContextMenuStrip logContextMenu;
        private System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnEstimatedStart;
        private System.Windows.Forms.ColumnHeader columnEstimatedStop;
        private System.Windows.Forms.ColumnHeader columnchannel;
        private System.Windows.Forms.ColumnHeader columnSegmentCode;
        private System.Windows.Forms.ColumnHeader columnRealDuration;
        private System.Windows.Forms.ColumnHeader columnElapsedDuration;
        private System.Windows.Forms.ColumnHeader columnOriginalTitle;
        private System.Windows.Forms.ColumnHeader columnState;
        private System.Windows.Forms.ColumnHeader columnCreatedDateTime;
        private System.Windows.Forms.ColumnHeader columnTITLE;
        private System.Windows.Forms.ColumnHeader columnTIME;
        private System.Windows.Forms.ColumnHeader columnDURATION;
        private System.Windows.Forms.ColumnHeader columnSegmentState;
        private System.Windows.Forms.ColumnHeader columnTotalDuration;
        private System.Windows.Forms.ColumnHeader columnReRuns;
        private System.Windows.Forms.ColumnHeader columnHeaderContentID;
        private System.Windows.Forms.ContextMenuStrip popUpContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPageNotification;
        private System.Windows.Forms.RichTextBox richTxtNotification;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox richTxtFilesNotification;

    }
}
