using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WordMakerDashboard.Database;

namespace WordMakerDashboard
{
    public partial class UserOperations : Form
    {
        private readonly BindingSource bindingSource;
        private readonly DatabaseOperations dbOperations;
        public UserOperations()
        {
            InitializeComponent();
            dbOperations = new DatabaseOperations();
            bindingSource = new BindingSource();
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
                dbOperations.DeleteFromDatabaseTable("clientUsers", "UserID", Convert.ToInt32(txtUserId.Text));
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
                var newData = new Dictionary<string, string>
                {
                    { "UserID", txtUserId.Text },
                    { "Username", txtUsername.Text },
                    { "Email", txtEmail.Text },
                    { "PhoneNumber", txtPhone.Text },
                    { "MainLanguage", txtMainLanguage.Text },
                    { "UserLevel", txtUserLevel.Text },
                    { "UserPoints", txtUserPoints.Text },
                };

                string updateQuery = @"
                    UPDATE clientUsers
                    SET Username = @Username,
                        Email = @Email,
                        PhoneNumber = @PhoneNumber,
                        MainLanguage = @MainLanguage,
                        UserLevel = @UserLevel,
                        UserPoints = @UserPoints
                    WHERE UserID = @UserID";

                dbOperations.UpdateDatabaseEntry(updateQuery, newData);
            }

            MessageBox.Show("Data altered successfully!");
            ResetAllFields();
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
            txtEmail.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[2].Value.ToString();
            txtPhone.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[3].Value.ToString();
            txtMainLanguage.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[4].Value.ToString();
            txtUserLevel.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[5].Value.ToString();
            txtUserPoints.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[6].Value.ToString();
        }

        private void ClearTextBoxes()
        {
            txtEmail.Clear();
            txtMainLanguage.Clear();
            txtPhone.Clear();
            txtUserLevel.Clear();
            txtUserPoints.Clear();
            txtUsername.Clear();
            txtUserId.Clear();
            txtFilterWord.Clear();
        }

        private void HandleEnableAllTextbox(bool enable)
        {
            txtEmail.Enabled = enable;
            txtMainLanguage.Enabled = enable;
            txtPhone.Enabled = enable;
            txtUserLevel.Enabled = enable;
            txtUserPoints.Enabled = enable;
            txtUsername.Enabled = enable;
            txtFilterWord.Enabled = enable;
        }

        private void LoadDatabaseView()
        {
            bindingSource.DataSource = dbOperations.SelectAllFromDatabase("clientUsers");
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