namespace YourHeightPlugin
{
    partial class ucProjectSettings
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
            this.lblHeight = new System.Windows.Forms.Label();
            this.trkHeightMultiplier = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trkHeightMultiplier)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(9, 3);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(41, 13);
            this.lblHeight.TabIndex = 49;
            this.lblHeight.Text = "Height:";
            this.lblHeight.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // trkHeightMultiplier
            // 
            this.trkHeightMultiplier.LargeChange = 1;
            this.trkHeightMultiplier.Location = new System.Drawing.Point(61, 3);
            this.trkHeightMultiplier.Name = "trkHeightMultiplier";
            this.trkHeightMultiplier.Size = new System.Drawing.Size(130, 45);
            this.trkHeightMultiplier.TabIndex = 48;
            this.trkHeightMultiplier.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trkHeightMultiplier.Scroll += new System.EventHandler(this.trkHeightMultiplier_Scroll);
            // 
            // ucProjectSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.trkHeightMultiplier);
            this.Name = "ucProjectSettings";
            this.Size = new System.Drawing.Size(342, 170);
            ((System.ComponentModel.ISupportInitialize)(this.trkHeightMultiplier)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.TrackBar trkHeightMultiplier;
    }
}
