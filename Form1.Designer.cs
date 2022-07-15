namespace NeonStream
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainDisplayBox = new System.Windows.Forms.GroupBox();
            this.NameOutputlabel1 = new System.Windows.Forms.Label();
            this.MainDisplayBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainDisplayBox
            // 
            this.MainDisplayBox.Controls.Add(this.NameOutputlabel1);
            this.MainDisplayBox.Location = new System.Drawing.Point(2, 3);
            this.MainDisplayBox.Name = "MainDisplayBox";
            this.MainDisplayBox.Size = new System.Drawing.Size(240, 472);
            this.MainDisplayBox.TabIndex = 0;
            this.MainDisplayBox.TabStop = false;
            // 
            // NameOutputlabel1
            // 
            this.NameOutputlabel1.AutoSize = true;
            this.NameOutputlabel1.Location = new System.Drawing.Point(6, 19);
            this.NameOutputlabel1.Name = "NameOutputlabel1";
            this.NameOutputlabel1.Size = new System.Drawing.Size(42, 15);
            this.NameOutputlabel1.TabIndex = 1;
            this.NameOutputlabel1.Text = "Name:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 901);
            this.Controls.Add(this.MainDisplayBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.MainDisplayBox.ResumeLayout(false);
            this.MainDisplayBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox MainDisplayBox;
        private Label NameOutputlabel1;
    }
}