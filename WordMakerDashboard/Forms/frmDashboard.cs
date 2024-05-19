using System;
using System.Windows.Forms;
using WordMakerDashboard.Forms;

namespace WordMakerDashboard
{
    public partial class frmDashboard : Form
    {
        public frmDashboard(int privilegeLevel, string fullName, string email, string Id, string Login)
        {
            InitializeComponent();
            if (privilegeLevel != 1)
            {
                administratorsToolStripMenuItem.Enabled = false;
                newAdminToolStripMenuItem.Enabled = false;
            }
            lblEmail.Text = "| Email: " + email;
            lblID.Text = "| ID: " + Id;
            lblLogin.Text = "| Login: " + Login;
            lblNome.Text = "| Name: " + fullName;
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

        private void newDictionaryEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new frmRegisterDictionaryOrWord();
            form.MdiParent = this;
            form.Show();
        }

        private void frmDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();
            var form = new frmLogin();
            form.Closed += (s, args) => Close();
            form.Show();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new frmDatabaseGridView("tbUsers");
            form.MdiParent = this.MdiParent;
            form.Show();
        }

        private void administratorsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new frmDatabaseGridView("tbAdmins");
            form.MdiParent = this.MdiParent;
            form.Show();
        }

        private void dictionariesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new frmDatabaseGridView("tblWords", true);
            form.MdiParent = this.MdiParent;
            form.Show();
        }
    }
}