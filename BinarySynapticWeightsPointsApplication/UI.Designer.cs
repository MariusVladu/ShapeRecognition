namespace BinarySynapticWeightsPointsApplication
{
    partial class UI
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
            this.drawingPictureBox = new System.Windows.Forms.PictureBox();
            this.SignificantPointsButton = new System.Windows.Forms.Button();
            this.CenterOfGravityButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.drawingPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // drawingPictureBox
            // 
            this.drawingPictureBox.BackColor = System.Drawing.Color.White;
            this.drawingPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.drawingPictureBox.Location = new System.Drawing.Point(12, 12);
            this.drawingPictureBox.Name = "drawingPictureBox";
            this.drawingPictureBox.Size = new System.Drawing.Size(384, 387);
            this.drawingPictureBox.TabIndex = 3;
            this.drawingPictureBox.TabStop = false;
            this.drawingPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.drawingPictureBox_MouseDown);
            this.drawingPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.drawingPictureBox_MouseMove);
            this.drawingPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.drawingPictureBox_MouseUp);
            // 
            // SignificantPointsButton
            // 
            this.SignificantPointsButton.Location = new System.Drawing.Point(400, 12);
            this.SignificantPointsButton.Name = "SignificantPointsButton";
            this.SignificantPointsButton.Size = new System.Drawing.Size(95, 37);
            this.SignificantPointsButton.TabIndex = 4;
            this.SignificantPointsButton.Text = "Get Significant Points";
            this.SignificantPointsButton.UseVisualStyleBackColor = true;
            this.SignificantPointsButton.Click += new System.EventHandler(this.SignificantPointsButton_Click);
            // 
            // CenterOfGravityButton
            // 
            this.CenterOfGravityButton.Location = new System.Drawing.Point(400, 55);
            this.CenterOfGravityButton.Name = "CenterOfGravityButton";
            this.CenterOfGravityButton.Size = new System.Drawing.Size(95, 37);
            this.CenterOfGravityButton.TabIndex = 5;
            this.CenterOfGravityButton.Text = "Compute Center of Gravity";
            this.CenterOfGravityButton.UseVisualStyleBackColor = true;
            this.CenterOfGravityButton.Click += new System.EventHandler(this.CenterOfGravityButton_Click);
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 411);
            this.Controls.Add(this.CenterOfGravityButton);
            this.Controls.Add(this.SignificantPointsButton);
            this.Controls.Add(this.drawingPictureBox);
            this.Name = "UI";
            this.Text = "Online Shape Recognition";
            ((System.ComponentModel.ISupportInitialize)(this.drawingPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox drawingPictureBox;
        private System.Windows.Forms.Button SignificantPointsButton;
        private System.Windows.Forms.Button CenterOfGravityButton;
    }
}

