namespace AAI_Final_Assignment_WinForms
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
            this.Screen = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // Screen
            // 
            this.Screen.BackColor = System.Drawing.SystemColors.HotTrack;
            this.Screen.Location = new System.Drawing.Point(1, 1);
            this.Screen.Name = "Screen";
            this.Screen.Size = new System.Drawing.Size(800, 451);
            this.Screen.TabIndex = 0;
            this.Screen.Paint += new System.Windows.Forms.PaintEventHandler(this.Screen_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Screen);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Panel Screen;
    }
}