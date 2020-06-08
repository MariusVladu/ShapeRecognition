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
            this.ResetCanvasButton = new System.Windows.Forms.Button();
            this.CenterOfGravityButton = new System.Windows.Forms.Button();
            this.showRaysButton = new System.Windows.Forms.Button();
            this.ExtractFeaturesButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.straightAnglesLabel = new System.Windows.Forms.Label();
            this.wideAnglesLabel = new System.Windows.Forms.Label();
            this.rightAnglesLabel = new System.Windows.Forms.Label();
            this.acuteAnglesLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.drawingPictureBox)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            // ResetCanvasButton
            // 
            this.ResetCanvasButton.Location = new System.Drawing.Point(400, 360);
            this.ResetCanvasButton.Name = "ResetCanvasButton";
            this.ResetCanvasButton.Size = new System.Drawing.Size(88, 39);
            this.ResetCanvasButton.TabIndex = 5;
            this.ResetCanvasButton.Text = "Reset Canvas";
            this.ResetCanvasButton.UseVisualStyleBackColor = true;
            this.ResetCanvasButton.Click += new System.EventHandler(this.ResetCanvasButton_Click);
            // 
            // CenterOfGravityButton
            // 
            this.CenterOfGravityButton.Location = new System.Drawing.Point(400, 12);
            this.CenterOfGravityButton.Name = "CenterOfGravityButton";
            this.CenterOfGravityButton.Size = new System.Drawing.Size(95, 37);
            this.CenterOfGravityButton.TabIndex = 5;
            this.CenterOfGravityButton.Text = "Compute Center of Gravity";
            this.CenterOfGravityButton.UseVisualStyleBackColor = true;
            this.CenterOfGravityButton.Click += new System.EventHandler(this.CenterOfGravityButton_Click);
            // 
            // showRaysButton
            // 
            this.showRaysButton.Location = new System.Drawing.Point(400, 55);
            this.showRaysButton.Name = "showRaysButton";
            this.showRaysButton.Size = new System.Drawing.Size(95, 25);
            this.showRaysButton.TabIndex = 6;
            this.showRaysButton.Text = "Show rays";
            this.showRaysButton.UseVisualStyleBackColor = true;
            this.showRaysButton.Click += new System.EventHandler(this.showRaysButton_Click);
            // 
            // ExtractFeaturesButton
            // 
            this.ExtractFeaturesButton.Location = new System.Drawing.Point(400, 86);
            this.ExtractFeaturesButton.Name = "ExtractFeaturesButton";
            this.ExtractFeaturesButton.Size = new System.Drawing.Size(95, 26);
            this.ExtractFeaturesButton.TabIndex = 7;
            this.ExtractFeaturesButton.Text = "Extract Features";
            this.ExtractFeaturesButton.UseVisualStyleBackColor = true;
            this.ExtractFeaturesButton.Click += new System.EventHandler(this.ExtractFeaturesButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.straightAnglesLabel);
            this.groupBox1.Controls.Add(this.wideAnglesLabel);
            this.groupBox1.Controls.Add(this.rightAnglesLabel);
            this.groupBox1.Controls.Add(this.acuteAnglesLabel);
            this.groupBox1.Location = new System.Drawing.Point(400, 132);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(141, 84);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Extracted Features";
            // 
            // straightAnglesLabel
            // 
            this.straightAnglesLabel.AutoSize = true;
            this.straightAnglesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.straightAnglesLabel.Location = new System.Drawing.Point(6, 61);
            this.straightAnglesLabel.Name = "straightAnglesLabel";
            this.straightAnglesLabel.Size = new System.Drawing.Size(112, 15);
            this.straightAnglesLabel.TabIndex = 3;
            this.straightAnglesLabel.Text = "# Straight angles = ";
            // 
            // wideAnglesLabel
            // 
            this.wideAnglesLabel.AutoSize = true;
            this.wideAnglesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wideAnglesLabel.Location = new System.Drawing.Point(6, 46);
            this.wideAnglesLabel.Name = "wideAnglesLabel";
            this.wideAnglesLabel.Size = new System.Drawing.Size(98, 15);
            this.wideAnglesLabel.TabIndex = 2;
            this.wideAnglesLabel.Text = "# Wide angles = ";
            // 
            // rightAnglesLabel
            // 
            this.rightAnglesLabel.AutoSize = true;
            this.rightAnglesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rightAnglesLabel.Location = new System.Drawing.Point(6, 31);
            this.rightAnglesLabel.Name = "rightAnglesLabel";
            this.rightAnglesLabel.Size = new System.Drawing.Size(99, 15);
            this.rightAnglesLabel.TabIndex = 1;
            this.rightAnglesLabel.Text = "# Right angles = ";
            // 
            // acuteAnglesLabel
            // 
            this.acuteAnglesLabel.AutoSize = true;
            this.acuteAnglesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acuteAnglesLabel.Location = new System.Drawing.Point(6, 16);
            this.acuteAnglesLabel.Name = "acuteAnglesLabel";
            this.acuteAnglesLabel.Size = new System.Drawing.Size(100, 15);
            this.acuteAnglesLabel.TabIndex = 0;
            this.acuteAnglesLabel.Text = "# Acute angles = ";
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 444);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ExtractFeaturesButton);
            this.Controls.Add(this.showRaysButton);
            this.Controls.Add(this.CenterOfGravityButton);
            this.Controls.Add(this.ResetCanvasButton);
            this.Controls.Add(this.drawingPictureBox);
            this.Name = "UI";
            this.Text = "Online Shape Recognition";
            ((System.ComponentModel.ISupportInitialize)(this.drawingPictureBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox drawingPictureBox;
        private System.Windows.Forms.Button ResetCanvasButton;
        private System.Windows.Forms.Button CenterOfGravityButton;
        private System.Windows.Forms.Button showRaysButton;
        private System.Windows.Forms.Button ExtractFeaturesButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label acuteAnglesLabel;
        private System.Windows.Forms.Label straightAnglesLabel;
        private System.Windows.Forms.Label wideAnglesLabel;
        private System.Windows.Forms.Label rightAnglesLabel;
    }
}

