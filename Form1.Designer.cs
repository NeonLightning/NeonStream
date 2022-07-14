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
            this.button1 = new System.Windows.Forms.Button();
            this.Starting = new System.Windows.Forms.Label();
            this.wep1label = new System.Windows.Forms.Label();
            this.wep2label = new System.Windows.Forms.Label();
            this.wep3label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(526, 415);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Starting
            // 
            this.Starting.AutoSize = true;
            this.Starting.Location = new System.Drawing.Point(12, 9);
            this.Starting.Name = "Starting";
            this.Starting.Size = new System.Drawing.Size(0, 15);
            this.Starting.TabIndex = 1;
            // 
            // wep1label
            // 
            this.wep1label.AutoSize = true;
            this.wep1label.Location = new System.Drawing.Point(12, 24);
            this.wep1label.Name = "wep1label";
            this.wep1label.Size = new System.Drawing.Size(0, 15);
            this.wep1label.TabIndex = 2;
            // 
            // wep2label
            // 
            this.wep2label.AutoSize = true;
            this.wep2label.Location = new System.Drawing.Point(12, 39);
            this.wep2label.Name = "wep2label";
            this.wep2label.Size = new System.Drawing.Size(0, 15);
            this.wep2label.TabIndex = 3;
            // 
            // wep3label
            // 
            this.wep3label.AutoSize = true;
            this.wep3label.Location = new System.Drawing.Point(12, 54);
            this.wep3label.Name = "wep3label";
            this.wep3label.Size = new System.Drawing.Size(0, 15);
            this.wep3label.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 450);
            this.Controls.Add(this.wep3label);
            this.Controls.Add(this.wep2label);
            this.Controls.Add(this.wep1label);
            this.Controls.Add(this.Starting);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button1;
        private Label Starting;
        private Label wep1label;
        private Label wep2label;
        private Label wep3label;
    }
}