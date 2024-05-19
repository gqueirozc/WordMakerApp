namespace WordMakerDashboard
{
    partial class frmRegisterDictionaryOrWord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRegisterDictionaryOrWord));
            this.tsbExit = new System.Windows.Forms.ToolStripButton();
            this.tsbConsult = new System.Windows.Forms.ToolStripButton();
            this.grbFicha = new System.Windows.Forms.GroupBox();
            this.txtLanguage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDefinition = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtExample = new System.Windows.Forms.TextBox();
            this.txtWord = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbAddJson = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSave = new System.Windows.Forms.Button();
            this.grbFicha.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsbExit
            // 
            this.tsbExit.Image = ((System.Drawing.Image)(resources.GetObject("tsbExit.Image")));
            this.tsbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExit.Name = "tsbExit";
            this.tsbExit.Size = new System.Drawing.Size(50, 24);
            this.tsbExit.Text = "Exit";
            this.tsbExit.Click += new System.EventHandler(this.tsbExit_Click);
            // 
            // tsbConsult
            // 
            this.tsbConsult.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbConsult.Name = "tsbConsult";
            this.tsbConsult.Size = new System.Drawing.Size(103, 24);
            this.tsbConsult.Text = "Consult Database";
            this.tsbConsult.Click += new System.EventHandler(this.tsbConsult_Click);
            // 
            // grbFicha
            // 
            this.grbFicha.Controls.Add(this.txtLanguage);
            this.grbFicha.Controls.Add(this.label1);
            this.grbFicha.Controls.Add(this.txtDefinition);
            this.grbFicha.Controls.Add(this.label6);
            this.grbFicha.Controls.Add(this.txtExample);
            this.grbFicha.Controls.Add(this.txtWord);
            this.grbFicha.Controls.Add(this.label3);
            this.grbFicha.Controls.Add(this.label2);
            this.grbFicha.Location = new System.Drawing.Point(13, 30);
            this.grbFicha.Name = "grbFicha";
            this.grbFicha.Size = new System.Drawing.Size(355, 240);
            this.grbFicha.TabIndex = 1;
            this.grbFicha.TabStop = false;
            this.grbFicha.Text = "Register Word";
            // 
            // txtLanguage
            // 
            this.txtLanguage.Location = new System.Drawing.Point(69, 19);
            this.txtLanguage.Name = "txtLanguage";
            this.txtLanguage.Size = new System.Drawing.Size(186, 20);
            this.txtLanguage.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Language:";
            // 
            // txtDefinition
            // 
            this.txtDefinition.Location = new System.Drawing.Point(69, 71);
            this.txtDefinition.Multiline = true;
            this.txtDefinition.Name = "txtDefinition";
            this.txtDefinition.Size = new System.Drawing.Size(270, 68);
            this.txtDefinition.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Definition:";
            // 
            // txtExample
            // 
            this.txtExample.Location = new System.Drawing.Point(69, 145);
            this.txtExample.Multiline = true;
            this.txtExample.Name = "txtExample";
            this.txtExample.Size = new System.Drawing.Size(270, 78);
            this.txtExample.TabIndex = 3;
            // 
            // txtWord
            // 
            this.txtWord.Location = new System.Drawing.Point(69, 45);
            this.txtWord.Name = "txtWord";
            this.txtWord.Size = new System.Drawing.Size(186, 20);
            this.txtWord.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Example:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Word:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbConsult,
            this.tsbAddJson,
            this.toolStripSeparator1,
            this.tsbExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(380, 27);
            this.toolStrip1.TabIndex = 23;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbAddJson
            // 
            this.tsbAddJson.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddJson.Name = "tsbAddJson";
            this.tsbAddJson.Size = new System.Drawing.Size(82, 24);
            this.tsbAddJson.Text = "Add via JSON";
            this.tsbAddJson.Click += new System.EventHandler(this.tsbAddJson_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(25, 276);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(327, 33);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save Word";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmRegisterDictionaryOrWord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 322);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grbFicha);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmRegisterDictionaryOrWord";
            this.Text = "Register Dictionary";
            this.grbFicha.ResumeLayout(false);
            this.grbFicha.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripButton tsbExit;
        private System.Windows.Forms.ToolStripButton tsbConsult;
        private System.Windows.Forms.GroupBox grbFicha;
        private System.Windows.Forms.TextBox txtDefinition;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtExample;
        private System.Windows.Forms.TextBox txtWord;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TextBox txtLanguage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolStripButton tsbAddJson;
    }
}