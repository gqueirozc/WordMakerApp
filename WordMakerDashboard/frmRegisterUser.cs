using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WordMakerDashboard.Database;
using WordMakerDashboard.Services;

namespace WordMakerDashboard
{
    public partial class frmRegisterUser : Form
    {
        private readonly DatabaseOperations dbOperations;
        private readonly CryptographyService cryptographyService;

        public frmRegisterUser()
        {
            InitializeComponent();
            dbOperations = new DatabaseOperations();
            cryptographyService = new CryptographyService();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show("You sure to add the user '" + txtUsername.Text + "' to the database ?",
              "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                var newData = new Dictionary<string, string>
                {
                    { "UserName", txtUsername.Text },
                    { "UserPassword", cryptographyService.ConvertToMd5(txtPassword.Text) },
                    { "UserEmail", txtEmail.Text },
                    { "UserLevel", txtStartingLevel.Text },
                    { "UserPoints", txtStartingPoints.Text }
                };

                string updateQuery = @"
                    INSERT INTO tbUsers (UserName, UserPassword, UserEmail, UserLevel, UserPoints) 
                    VALUES (@UserName, @UserPassword, @UserEmail, @UserLevel, @UserPoints)";
                try
                {
                    dbOperations.UpdateDatabaseEntry(updateQuery, newData);
                    MessageBox.Show("User added successfully!");
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
            txtUsername.Clear();
            txtStartingLevel.Text = "1";
            txtStartingPoints.Text = "0";
            txtPassword.Text = "12345";
        }

        private void tsbConsultDB_Click(object sender, EventArgs e)
        {
            var form = new frmDatabaseGridView("tbUsers", "UserName");
            form.MdiParent = this.MdiParent;
            form.Show();
        }

        private void tsbExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
