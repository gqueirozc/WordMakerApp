using System;
using System.Windows.Forms;
using WordMakerDashboard.Services;

namespace WordMakerDashboard.Forms
{
    public partial class frmLogin : Form
    {
        private DatabaseService dbOperations;
        private CryptographyService cryptographyService;

        public frmLogin()
        {
            InitializeComponent();
            dbOperations = new DatabaseService();
            cryptographyService = new CryptographyService();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var login = txtLogin.Text;
            var password = txtPassword.Text;

            var query = $"SELECT * FROM tbAdmins WHERE AdminLogin = '{login}'";

            var dataTable = dbOperations.SelectAllFromDatabase("tbAdmins", query);

            if (dataTable.Rows.Count == 1)
            {
                var dataBasePassword = dataTable.Rows[0]["AdminPassword"].ToString();
                var privilegeLevel = Convert.ToInt32(dataTable.Rows[0]["AdminPrivilegeLevel"]);
                var fullName = dataTable.Rows[0]["AdminFullName"].ToString();
                var email = dataTable.Rows[0]["AdminCorporateEmail"].ToString();
                var id = dataTable.Rows[0]["AdminId"].ToString();

                if (dataBasePassword == cryptographyService.ConvertToMd5(password))
                {
                    var form = new frmDashboard(privilegeLevel, fullName, email, id, login);
                    this.Hide();
                    form.Show();
                }
                else
                {
                    MessageBox.Show("Invalid user or password!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid user or password!");
                return;
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnLogin_Click(sender, e);
            }
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}