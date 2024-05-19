using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WordMakerDashboard.Database;
using WordMakerDashboard.Services;

namespace WordMakerDashboard
{
    public partial class frmAdminOperations : Form
    {
        private readonly BindingSource bindingSource;
        private readonly DatabaseOperations dbOperations;
        private readonly CryptographyService cryptographyService;

        public frmAdminOperations()
        {
            InitializeComponent();
            dbOperations = new DatabaseOperations();
            bindingSource = new BindingSource();
            cryptographyService = new CryptographyService();
            btnDelete.Visible = false;
            btnSave.Visible = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure? All text will be cleared.", "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DialogResult = DialogResult.Yes;
                btnDelete.Visible = false;
                btnSave.Visible = false;
                HandleEnableAllTextbox(false);
                ClearTextBoxes();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show("You sure to delete the user '" + txtAdminId.Text + "' from the database ? This action is irreversible.",
               "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                dbOperations.DeleteFromDatabaseTable("tbAdmins", "AdminID", Convert.ToInt32(txtAdminId.Text));
            }

            MessageBox.Show("Entry deleted successfully!");
            ResetAllFields();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show("You sure to update the user '" + txtAdminId.Text + "' from the database ?",
                "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                var newData = new Dictionary<string, string>
                {
                    { "AdminID", txtAdminId.Text },
                    { "AdminFullName", txtFullName.Text },
                    { "AdminCorporateEmail", txtEmail.Text },
                    { "AdminPhoneNumber", txtPhone.Text },
                    { "AdminPrivilegeLevel", dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[4].Value.ToString() },
                    { "AdminPassword", cryptographyService.ConvertToMd5(txtPassword.Text) },
                    { "AdminLogin", txtLogin.Text },
                };

                if (cbPrivilegeLevel.Text != "")
                {
                    newData["PrivilegeLevel"] = cbPrivilegeLevel.Text;
                }

                string updateQuery = @"
                    UPDATE tbAdmins
                    SET AdminFullName = @AdminFullName,
                        AdminCorporateEmail = @AdminCorporateEmail,
                        AdminPhoneNumber = @AdminPhoneNumber,
                        AdminPrivilegeLevel = @AdminPrivilegeLevel,
                        AdminPassword = @AdminPassword,
                        AdminLogin = @AdminLogin
                    WHERE AdminID = @AdminID";

                try
                {
                    dbOperations.UpdateDatabaseEntry(updateQuery, newData);
                    MessageBox.Show("Data altered successfully!");
                    ResetAllFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured while trying to alter the data: " + ex.Message);
                }
            }
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (txtAdminId.Text.Length == 0)
            {
                MessageBox.Show("No data selected. Operation can't proceed.");
                return;
            }
            btnDelete.Visible = true;
            btnSave.Visible = false;
            HandleEnableAllTextbox(false);
        }

        private void tsbUpdate_Click(object sender, EventArgs e)
        {
            if (txtAdminId.Text.Length == 0)
            {
                MessageBox.Show("No data selected. Operation can't proceed.");
                return;
            }
            btnSave.Visible = true;
            btnDelete.Visible = false;
            HandleEnableAllTextbox(true);
        }

        private void tsbExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtFilterWord_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterWord.Text.Trim();

            if (string.IsNullOrEmpty(filterText))
            {
                bindingSource.Filter = null;
            }
            else
            {
                string columnName = "AdminFullName";
                string filterExpression = $"[{columnName}] LIKE '%{filterText}%'";
                bindingSource.Filter = filterExpression;
            }
        }

        private void dgvDados_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtAdminId.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[0].Value.ToString();
            txtFullName.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[1].Value.ToString();
            txtEmail.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[2].Value.ToString();
            txtPhone.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[3].Value.ToString();
            txtLogin.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[5].Value.ToString();
            txtPassword.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[6].Value.ToString();
        }

        private void ClearTextBoxes()
        {
            txtEmail.Clear();
            txtPhone.Clear();
            txtFullName.Clear();
            txtAdminId.Clear();
            txtFilterWord.Clear();
            txtPassword.Clear();
            txtLogin.Clear();
            cbPrivilegeLevel.SelectedIndex = -1;
        }

        private void HandleEnableAllTextbox(bool enable)
        {
            txtEmail.Enabled = enable;
            txtPhone.Enabled = enable;
            txtFullName.Enabled = enable;
            txtPassword.Enabled = enable;
            txtLogin.Enabled = enable;
            cbPrivilegeLevel.Enabled = enable;
        }

        private void LoadDatabaseView()
        {
            bindingSource.DataSource = dbOperations.SelectAllFromDatabase("tbAdmins");
            dgvDados.DataSource = bindingSource;
            dgvDados.Enabled = true;
        }

        private void ResetAllFields()
        {
            ClearTextBoxes();
            HandleEnableAllTextbox(false);
            btnDelete.Visible = false;
            btnSave.Visible = false;
            LoadDatabaseView();
        }

        private void frmAdminOperations_Load(object sender, EventArgs e)
        {
            ClearTextBoxes();
            LoadDatabaseView();
        }
    }
}