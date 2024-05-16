using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WordMakerDashboard.Database;

namespace WordMakerDashboard
{
    public partial class frmRegisterAdmin : Form
    {
        private readonly DatabaseOperations dbOperations;

        public frmRegisterAdmin()
        {
            InitializeComponent();
            dbOperations = new DatabaseOperations();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show("You sure to add the user '" + txtFullName.Text + "' to the database ?",
                "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                var newData = new Dictionary<string, string>
                {
                    { "FullName", txtFullName.Text },
                    { "CorporateEmail", txtEmail.Text },
                    { "PhoneNumber", txtPhone.Text },
                    { "PrivilegeLevel", cbPrivilegeLevel.Text }
                };

                if (cbPrivilegeLevel.Text != "")
                {
                    newData["PrivilegeLevel"] = cbPrivilegeLevel.Text;
                }

                string updateQuery = @"
                    INSERT INTO adminUsers (FullName, CorporateEmail, PhoneNumber, PrivilegeLevel) 
                    VALUES (@FullName, @CorporateEmail, @PhoneNumber, @PrivilegeLevel)";
                try {
                    dbOperations.UpdateDatabaseEntry(updateQuery, newData);
                } catch (Exception ex)
                {
                    MessageBox.Show("An error occured while trying to add new entry: " + ex.Message);
                }
            }

            MessageBox.Show("Admin added successfully!");
            ClearTextBoxes();
        }

        private void ClearTextBoxes()
        {
            txtEmail.Clear();
            txtPhone.Clear();
            txtFullName.Clear();
            cbPrivilegeLevel.SelectedIndex = -1;
        }
    }
}
