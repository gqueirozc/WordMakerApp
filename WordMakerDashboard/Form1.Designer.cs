namespace WordMakerDashboard
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
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox_Languages = new System.Windows.Forms.ComboBox();
            this.progressBarGlobal = new System.Windows.Forms.ProgressBar();
            this.bgWorkerProgressBarGlobal = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(290, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Enviar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox_Languages
            // 
            this.comboBox_Languages.DisplayMember = "English";
            this.comboBox_Languages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Languages.FormattingEnabled = true;
            this.comboBox_Languages.Items.AddRange(new object[] {
            "English",
            "Portuguese"});
            this.comboBox_Languages.Location = new System.Drawing.Point(131, 39);
            this.comboBox_Languages.Name = "comboBox_Languages";
            this.comboBox_Languages.Size = new System.Drawing.Size(121, 21);
            this.comboBox_Languages.Sorted = true;
            this.comboBox_Languages.TabIndex = 1;
            // 
            // progressBarGlobal
            // 
            this.progressBarGlobal.Location = new System.Drawing.Point(12, 415);
            this.progressBarGlobal.Name = "progressBarGlobal";
            this.progressBarGlobal.Size = new System.Drawing.Size(776, 23);
            this.progressBarGlobal.TabIndex = 2;
            this.progressBarGlobal.Visible = false;
            // 
            // bgWorkerProgressBarGlobal
            // 
            this.bgWorkerProgressBarGlobal.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerProgressBarGlobal_DoWork);
            this.bgWorkerProgressBarGlobal.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorkerProgressBarGlobal_ProgressChanged);
            this.bgWorkerProgressBarGlobal.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerProgressBarGlobal_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.progressBarGlobal);
            this.Controls.Add(this.comboBox_Languages);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox_Languages;
        private System.Windows.Forms.ProgressBar progressBarGlobal;
        private System.ComponentModel.BackgroundWorker bgWorkerProgressBarGlobal;
    }
}

