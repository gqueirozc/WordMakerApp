using System;
using System.Windows.Forms;

namespace WordMakerDashboard
{
    public partial class frmDashboard : Form
    {
        public frmDashboard(int privilegeLevel)
        {
            InitializeComponent();
            if(privilegeLevel != 1)
            {
                administratorsToolStripMenuItem.Enabled = false;
                newAdminToolStripMenuItem.Enabled = false;
            }
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            tssMensagem.Text = Application.ProductName + "  Versão: " + Application.ProductVersion;
            tssData.Text = DateTime.Today.ToString();
            this.WindowState = FormWindowState.Maximized;
        }

        private void dictionaryConsultTableStrip_Click(object sender, EventArgs e)
        {
            var form = new frmDictionaryOperations();
            form.MdiParent = this;
            form.Show();
        }

        private void clientUserDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new UserOperations();
            form.MdiParent = this;
            form.Show();
        }

        private void administratorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new frmAdminOperations();
            form.MdiParent = this;
            form.Show();
        }

        private void newAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new frmRegisterAdmin();
            form.MdiParent = this;
            form.Show();
        }

        private void newUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new frmRegisterUser();
            form.MdiParent = this;
            form.Show();
        }
    }
}
