namespace WordMakerDashboard
{
    partial class frmDashboard
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
            this.tssData = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssMensagem = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblID = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLogin = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblNome = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblEmail = new System.Windows.Forms.ToolStripStatusLabel();
            this.arquivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newDictionaryEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newAdminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.movimentoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dictionaryConsultTableStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.clientUserDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administratorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.consultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administratorsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dictionariesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tssData
            // 
            this.tssData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.tssData.Name = "tssData";
            this.tssData.Size = new System.Drawing.Size(16, 17);
            this.tssData.Text = "...";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssMensagem,
            this.tssData,
            this.lblID,
            this.lblLogin,
            this.lblNome,
            this.lblEmail});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssMensagem
            // 
            this.tssMensagem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tssMensagem.Name = "tssMensagem";
            this.tssMensagem.Size = new System.Drawing.Size(16, 17);
            this.tssMensagem.Text = "...";
            // 
            // lblID
            // 
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(18, 17);
            this.lblID.Text = "ID";
            // 
            // lblLogin
            // 
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(37, 17);
            this.lblLogin.Text = "Login";
            // 
            // lblNome
            // 
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(40, 17);
            this.lblNome.Text = "Nome";
            // 
            // lblEmail
            // 
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(36, 17);
            this.lblEmail.Text = "Email";
            // 
            // arquivoToolStripMenuItem
            // 
            this.arquivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDictionaryEntryToolStripMenuItem,
            this.newUserToolStripMenuItem,
            this.newAdminToolStripMenuItem});
            this.arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            this.arquivoToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.arquivoToolStripMenuItem.Text = "New Register";
            // 
            // newDictionaryEntryToolStripMenuItem
            // 
            this.newDictionaryEntryToolStripMenuItem.Name = "newDictionaryEntryToolStripMenuItem";
            this.newDictionaryEntryToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.newDictionaryEntryToolStripMenuItem.Text = "Dictionary Entry";
            this.newDictionaryEntryToolStripMenuItem.Click += new System.EventHandler(this.newDictionaryEntryToolStripMenuItem_Click);
            // 
            // newUserToolStripMenuItem
            // 
            this.newUserToolStripMenuItem.Name = "newUserToolStripMenuItem";
            this.newUserToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.newUserToolStripMenuItem.Text = "Client User";
            this.newUserToolStripMenuItem.Click += new System.EventHandler(this.newUserToolStripMenuItem_Click);
            // 
            // newAdminToolStripMenuItem
            // 
            this.newAdminToolStripMenuItem.Name = "newAdminToolStripMenuItem";
            this.newAdminToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.newAdminToolStripMenuItem.Text = "Administrator";
            this.newAdminToolStripMenuItem.Click += new System.EventHandler(this.newAdminToolStripMenuItem_Click);
            // 
            // movimentoToolStripMenuItem
            // 
            this.movimentoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dictionaryConsultTableStrip,
            this.clientUserDataToolStripMenuItem,
            this.administratorsToolStripMenuItem});
            this.movimentoToolStripMenuItem.Name = "movimentoToolStripMenuItem";
            this.movimentoToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.movimentoToolStripMenuItem.Text = "Update/Delete";
            // 
            // dictionaryConsultTableStrip
            // 
            this.dictionaryConsultTableStrip.Name = "dictionaryConsultTableStrip";
            this.dictionaryConsultTableStrip.Size = new System.Drawing.Size(180, 22);
            this.dictionaryConsultTableStrip.Text = "Dictionaries";
            this.dictionaryConsultTableStrip.Click += new System.EventHandler(this.dictionaryConsultTableStrip_Click);
            // 
            // clientUserDataToolStripMenuItem
            // 
            this.clientUserDataToolStripMenuItem.Name = "clientUserDataToolStripMenuItem";
            this.clientUserDataToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.clientUserDataToolStripMenuItem.Text = "Normal Users";
            this.clientUserDataToolStripMenuItem.Click += new System.EventHandler(this.clientUserDataToolStripMenuItem_Click);
            // 
            // administratorsToolStripMenuItem
            // 
            this.administratorsToolStripMenuItem.Name = "administratorsToolStripMenuItem";
            this.administratorsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.administratorsToolStripMenuItem.Text = "Administrators";
            this.administratorsToolStripMenuItem.Click += new System.EventHandler(this.administratorsToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arquivoToolStripMenuItem,
            this.movimentoToolStripMenuItem,
            this.consultToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // consultToolStripMenuItem
            // 
            this.consultToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dictionariesToolStripMenuItem,
            this.administratorsToolStripMenuItem1,
            this.usersToolStripMenuItem});
            this.consultToolStripMenuItem.Name = "consultToolStripMenuItem";
            this.consultToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.consultToolStripMenuItem.Text = "Consult";
            // 
            // administratorsToolStripMenuItem1
            // 
            this.administratorsToolStripMenuItem1.Name = "administratorsToolStripMenuItem1";
            this.administratorsToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.administratorsToolStripMenuItem1.Text = "Administrators";
            this.administratorsToolStripMenuItem1.Click += new System.EventHandler(this.administratorsToolStripMenuItem1_Click);
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.usersToolStripMenuItem.Text = "Users";
            this.usersToolStripMenuItem.Click += new System.EventHandler(this.usersToolStripMenuItem_Click);
            // 
            // dictionariesToolStripMenuItem
            // 
            this.dictionariesToolStripMenuItem.Name = "dictionariesToolStripMenuItem";
            this.dictionariesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dictionariesToolStripMenuItem.Text = "Dictionaries";
            this.dictionariesToolStripMenuItem.Click += new System.EventHandler(this.dictionariesToolStripMenuItem_Click);
            // 
            // frmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.Name = "frmDashboard";
            this.Text = "Dashboard";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDashboard_FormClosed);
            this.Load += new System.EventHandler(this.frmDashboard_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel tssData;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssMensagem;
        private System.Windows.Forms.ToolStripStatusLabel lblNome;
        private System.Windows.Forms.ToolStripStatusLabel lblEmail;
        private System.Windows.Forms.ToolStripMenuItem arquivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newDictionaryEntryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newAdminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem movimentoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dictionaryConsultTableStrip;
        private System.Windows.Forms.ToolStripMenuItem clientUserDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem administratorsToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblID;
        private System.Windows.Forms.ToolStripStatusLabel lblLogin;
        private System.Windows.Forms.ToolStripMenuItem consultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dictionariesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem administratorsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
    }
}