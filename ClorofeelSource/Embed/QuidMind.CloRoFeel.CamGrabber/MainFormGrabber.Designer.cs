namespace QuidMind.CloRoFeel.CamGrabber
{
    partial class MainFormGrabber
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
            this.components = new System.ComponentModel.Container();
            this.lblLastGrab = new System.Windows.Forms.Label();
            this.tmrGrabPic = new System.Windows.Forms.Timer(this.components);
            this.lblLastError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblLastGrab
            // 
            this.lblLastGrab.AutoSize = true;
            this.lblLastGrab.Location = new System.Drawing.Point(4, 10);
            this.lblLastGrab.Name = "lblLastGrab";
            this.lblLastGrab.Size = new System.Drawing.Size(31, 13);
            this.lblLastGrab.TabIndex = 0;
            this.lblLastGrab.Text = "none";
            // 
            // tmrGrabPic
            // 
            this.tmrGrabPic.Interval = 2000;
            this.tmrGrabPic.Tick += new System.EventHandler(this.tmrGrabPic_Tick);
            // 
            // lblLastError
            // 
            this.lblLastError.AutoSize = true;
            this.lblLastError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblLastError.Location = new System.Drawing.Point(7, 42);
            this.lblLastError.Name = "lblLastError";
            this.lblLastError.Size = new System.Drawing.Size(0, 13);
            this.lblLastError.TabIndex = 1;
            // 
            // MainFormGrabber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 67);
            this.Controls.Add(this.lblLastError);
            this.Controls.Add(this.lblLastGrab);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainFormGrabber";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CloroFeel - WebCam grabber";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLastGrab;
        private System.Windows.Forms.Timer tmrGrabPic;
        private System.Windows.Forms.Label lblLastError;
    }
}

