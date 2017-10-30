namespace MTV.EventDispatcher.Service.Extensions.PosterExtension.UI
{
    partial class PosterUI
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
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.lWidth = new System.Windows.Forms.Label();
            this.lHeight = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numWidth
            // 
            this.numWidth.Location = new System.Drawing.Point(100, 19);
            this.numWidth.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(96, 20);
            this.numWidth.TabIndex = 3;
            this.numWidth.Value = new decimal(new int[] {
            160,
            0,
            0,
            0});
            // 
            // numHeight
            // 
            this.numHeight.Location = new System.Drawing.Point(100, 59);
            this.numHeight.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(96, 20);
            this.numHeight.TabIndex = 4;
            this.numHeight.Value = new decimal(new int[] {
            236,
            0,
            0,
            0});
            // 
            // lWidth
            // 
            this.lWidth.AutoSize = true;
            this.lWidth.Location = new System.Drawing.Point(30, 23);
            this.lWidth.Name = "lWidth";
            this.lWidth.Size = new System.Drawing.Size(41, 13);
            this.lWidth.TabIndex = 5;
            this.lWidth.Text = "Width :";
            // 
            // lHeight
            // 
            this.lHeight.AutoSize = true;
            this.lHeight.Location = new System.Drawing.Point(30, 61);
            this.lHeight.Name = "lHeight";
            this.lHeight.Size = new System.Drawing.Size(44, 13);
            this.lHeight.TabIndex = 6;
            this.lHeight.Text = "Height :";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numWidth);
            this.groupBox1.Controls.Add(this.lHeight);
            this.groupBox1.Controls.Add(this.lWidth);
            this.groupBox1.Controls.Add(this.numHeight);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 87);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Resizing parameter(s)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(455, 14);
            this.label1.TabIndex = 8;
            this.label1.Text = "N.B : These parameters are applied for Automatic Rec and PushVod";
            // 
            // PosterUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "PosterUI";
            this.Size = new System.Drawing.Size(462, 111);
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.Label lWidth;
        private System.Windows.Forms.Label lHeight;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;

    }
}
