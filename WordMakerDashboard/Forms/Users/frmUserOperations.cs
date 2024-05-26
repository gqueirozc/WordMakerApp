using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WordMakerDashboard.Services;

namespace WordMakerDashboard
{
    public partial class UserOperations : Form
    {
        private readonly BindingSource bindingSource;
        private readonly CryptographyService cryptographyService;

        public UserOperations()
        {
            InitializeComponent();
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
            var resp = MessageBox.Show("You sure to delete the user '" + txtUserId.Text + "' from the database ? This action is irreversible.",
               "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                DatabaseService.DeleteFromDatabaseTable("clientUsers", "UserID", Convert.ToInt32(txtUserId.Text));
            }

            MessageBox.Show("Entry deleted successfully!");
            ResetAllFields();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show("You sure to update the user '" + txtUserId.Text + "' from the database ?",
                "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                var newData = new Dictionary<string, object>
                {
                    { "UserID", Convert.ToInt32(txtUserId.Text) },
                    { "UserName", txtUsername.Text },
                    { "UserEmail", txtEmail.Text },
                    { "UserLevel", txtUserLevel.Text },
                    { "UserPoints", txtUserPoints.Text },
                    { "UserPassword", cryptographyService.ConvertToMd5(txtPassword.Text) },
                };

                string updateQuery = @"
                    UPDATE tbUsers
                    SET UserName = @UserName,
                        UserEmail = @UserEmail,
                        UserLevel = @UserLevel,
                        UserPoints = @UserPoints,
                        UserPassword = @UserPassword
                    WHERE UserID = @UserID";

                try
                {
                    DatabaseService.UpdateDatabaseEntry(updateQuery, newData);
                    MessageBox.Show("Data altered successfully!");
                    ResetAllFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured while trying to alter the data: " + ex.Message);
                }
            }
        }

        private void tsbAlterar_Click(object sender, EventArgs e)
        {
            btnSave.Visible = true;
            btnDelete.Visible = false;
            HandleEnableAllTextbox(true);
        }

        private void tsbExcluir_Click(object sender, EventArgs e)
        {
            btnDelete.Visible = true;
            btnSave.Visible = false;
            HandleEnableAllTextbox(false);
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
                string columnName = "Username";
                string filterExpression = $"[{columnName}] LIKE '%{filterText}%'";
                bindingSource.Filter = filterExpression;
            }
        }

        private void dgvDados_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtUserId.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[0].Value.ToString();
            txtUsername.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[1].Value.ToString();
            txtPassword.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[2].Value.ToString();
            txtEmail.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[3].Value.ToString();
            txtUserLevel.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[4].Value.ToString();
            txtUserPoints.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[5].Value.ToString();
        }

        private void ClearTextBoxes()
        {
            txtEmail.Clear();
            txtPassword.Clear();
            txtUserLevel.Clear();
            txtUserPoints.Clear();
            txtUsername.Clear();
            txtUserId.Clear();
            txtFilterWord.Clear();
        }

        private void HandleEnableAllTextbox(bool enable)
        {
            txtEmail.Enabled = enable;
            txtPassword.Enabled = enable;
            txtUserLevel.Enabled = enable;
            txtUserPoints.Enabled = enable;
            txtUsername.Enabled = enable;
        }

        private void LoadDatabaseView()
        {
            bindingSource.DataSource = DatabaseService.SelectAllFromDatabase("tbUsers");
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

        private void UserOperations_Load(object sender, EventArgs e)
        {
            ClearTextBoxes();
            LoadDatabaseView();
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (txtUserId.Text.Length == 0)
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
            if (txtUserId.Text.Length == 0)
            {
                MessageBox.Show("No data selected. Operation can't proceed.");
                return;
            }
            btnSave.Visible = true;
            btnDelete.Visible = false;
            HandleEnableAllTextbox(true);
        }
    }
}