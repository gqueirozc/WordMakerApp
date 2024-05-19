using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WordMakerDashboard.Database;
using WordMakerDashboard.Services;

namespace WordMakerDashboard
{
    public partial class frmRegisterAdmin : Form
    {
        private readonly DatabaseOperations dbOperations;
        private readonly CryptographyService cryptographyService;

        public frmRegisterAdmin()
        {
            InitializeComponent();
            dbOperations = new DatabaseOperations();
            cryptographyService = new CryptographyService();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show("You sure to add the user '" + txtFullName.Text + "' to the database ?",
                "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                var newData = new Dictionary<string, string>
                {
                    { "AdminFullName", txtFullName.Text },
                    { "AdminCorporateEmail", txtEmail.Text },
                    { "AdminPhoneNumber", txtPhone.Text },
                    { "AdminPrivilegeLevel", cbPrivilegeLevel.Text },
                    { "AdminLogin", txtLogin.Text },
                    { "AdminPassword", cryptographyService.ConvertToMd5(txtPassword.Text)}
                };

                if (cbPrivilegeLevel.Text != "")
                {
                    newData["AdminPrivilegeLevel"] = cbPrivilegeLevel.Text;
                }

                string updateQuery = @"
                    INSERT INTO tbAdmins (AdminFullName, AdminCorporateEmail, AdminPhoneNumber, AdminPrivilegeLevel, AdminLogin, AdminPassword) 
                    VALUES (@AdminFullName, @AdminCorporateEmail, @AdminPhoneNumber, @AdminPrivilegeLevel, @AdminLogin, @AdminPassword)";
             
                try {
                    dbOperations.UpdateDatabaseEntry(updateQuery, newData);
                    MessageBox.Show("Admin added successfully!");
                    ClearTextBoxes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured while trying to add new entry: " + ex.Message);
                }
            }
        }

        private void ClearTextBoxes()
        {
            txtEmail.Clear();
            txtPhone.Clear();
            txtFullName.Clear();
            txtLogin.Clear();
            txtPassword.Text = "12345";
            cbPrivilegeLevel.SelectedIndex = -1;
        }

        private void tsbConsultDB_Click(object sender, EventArgs e)
        {
            var form = new frmDatabaseGridView("tbAdmins");
            form.MdiParent = this.MdiParent;
            form.Show();
        }

        private void tsbExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
