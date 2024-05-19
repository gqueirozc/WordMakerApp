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
            this.clientUserDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dictionaryConsultTableStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.movimentoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administratorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newAdminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.arquivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newDictionaryEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sobreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
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
            this.tssData});
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
            // clientUserDataToolStripMenuItem
            // 
            this.clientUserDataToolStripMenuItem.Name = "clientUserDataToolStripMenuItem";
            this.clientUserDataToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.clientUserDataToolStripMenuItem.Text = "Normal Users";
            this.clientUserDataToolStripMenuItem.Click += new System.EventHandler(this.clientUserDataToolStripMenuItem_Click);
            // 
            // dictionaryConsultTableStrip
            // 
            this.dictionaryConsultTableStrip.Name = "dictionaryConsultTableStrip";
            this.dictionaryConsultTableStrip.Size = new System.Drawing.Size(152, 22);
            this.dictionaryConsultTableStrip.Text = "Dictionaries";
            this.dictionaryConsultTableStrip.Click += new System.EventHandler(this.dictionaryConsultTableStrip_Click);
            // 
            // movimentoToolStripMenuItem
            // 
            this.movimentoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dictionaryConsultTableStrip,
            this.clientUserDataToolStripMenuItem,
            this.administratorsToolStripMenuItem});
            this.movimentoToolStripMenuItem.Name = "movimentoToolStripMenuItem";
            this.movimentoToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.movimentoToolStripMenuItem.Text = "Consult/Alter";
            // 
            // administratorsToolStripMenuItem
            // 
            this.administratorsToolStripMenuItem.Name = "administratorsToolStripMenuItem";
            this.administratorsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.administratorsToolStripMenuItem.Text = "Administrators";
            this.administratorsToolStripMenuItem.Click += new System.EventHandler(this.administratorsToolStripMenuItem_Click);
            // 
            // newUserToolStripMenuItem
            // 
            this.newUserToolStripMenuItem.Name = "newUserToolStripMenuItem";
            this.newUserToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newUserToolStripMenuItem.Text = "Client User";
            this.newUserToolStripMenuItem.Click += new System.EventHandler(this.newUserToolStripMenuItem_Click);
            // 
            // newAdminToolStripMenuItem
            // 
            this.newAdminToolStripMenuItem.Name = "newAdminToolStripMenuItem";
            this.newAdminToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newAdminToolStripMenuItem.Text = "Administrator";
            this.newAdminToolStripMenuItem.Click += new System.EventHandler(this.newAdminToolStripMenuItem_Click);
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
            this.newDictionaryEntryToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newDictionaryEntryToolStripMenuItem.Text = "Dictionary Entry";
            this.newDictionaryEntryToolStripMenuItem.Click += new System.EventHandler(this.newDictionaryEntryToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arquivoToolStripMenuItem,
            this.movimentoToolStripMenuItem,
            this.sobreToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // sobreToolStripMenuItem
            // 
            this.sobreToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.sobreToolStripMenuItem.Name = "sobreToolStripMenuItem";
            this.sobreToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.sobreToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(122, 22);
            this.aboutToolStripMenuItem1.Text = "About us";
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
        private System.Windows.Forms.ToolStripMenuItem clientUserDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dictionaryConsultTableStrip;
        private System.Windows.Forms.ToolStripMenuItem movimentoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newAdminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem arquivoToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newDictionaryEntryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sobreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem administratorsToolStripMenuItem;
    }
}