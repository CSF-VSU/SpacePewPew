namespace SpacePewPew
{
    partial class MainForm
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
            this.OGL = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.SuspendLayout();
            // 
            // OGL
            // 
            this.OGL.AccumBits = ((byte)(0));
            this.OGL.AutoCheckErrors = false;
            this.OGL.AutoFinish = false;
            this.OGL.AutoMakeCurrent = true;
            this.OGL.AutoSwapBuffers = true;
            this.OGL.BackColor = System.Drawing.Color.Black;
            this.OGL.ColorBits = ((byte)(32));
            this.OGL.DepthBits = ((byte)(16));
            this.OGL.Location = new System.Drawing.Point(12, 12);
            this.OGL.Name = "OGL";
            this.OGL.Size = new System.Drawing.Size(374, 273);
            this.OGL.StencilBits = ((byte)(0));
            this.OGL.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 297);
            this.Controls.Add(this.OGL);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Tao.Platform.Windows.SimpleOpenGlControl OGL;
    }
}

