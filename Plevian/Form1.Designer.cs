namespace Plevian
{
    partial class Form1
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
            this.sfmlPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // sfmlPanel
            // 
            this.sfmlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sfmlPanel.Location = new System.Drawing.Point(0, 0);
            this.sfmlPanel.Margin = new System.Windows.Forms.Padding(0);
            this.sfmlPanel.Name = "sfmlPanel";
            this.sfmlPanel.Size = new System.Drawing.Size(888, 461);
            this.sfmlPanel.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 461);
            this.Controls.Add(this.sfmlPanel);
            this.Name = "Form1";
            this.Text = "Plevian";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel sfmlPanel;
    }
}

