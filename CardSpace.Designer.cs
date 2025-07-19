namespace xcom_tactics
{
    partial class CardSpace
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
            components = new System.ComponentModel.Container();
            animationTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // animationTimer
            // 
            animationTimer.Interval = 50;
            // 
            // CardSpace
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            DoubleBuffered = true;
            Margin = new Padding(4, 3, 4, 3);
            Name = "CardSpace";
            Size = new Size(628, 333);
            Paint += painted;
            MouseDown += mousePressed;
            MouseMove += mouseMoved;
            MouseUp += mouseReleased;
            Resize += resized;
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer animationTimer;
    }
}
