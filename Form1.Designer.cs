﻿namespace xcom_tactics
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
            cardSpace = new UnitCardSpace();
            SuspendLayout();
            // 
            // cardSpace
            // 
            cardSpace.Dock = DockStyle.Fill;
            cardSpace.Location = new Point(0, 0);
            cardSpace.Margin = new Padding(4, 3, 4, 3);
            cardSpace.Name = "cardSpace";
            cardSpace.Size = new Size(784, 561);
            cardSpace.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(cardSpace);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private UnitCardSpace cardSpace;
    }
}
