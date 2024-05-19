using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using WordMakerDashboard.Database;

namespace WordMakerDashboard
{
    public partial class frmDictionaryOperations : Form
    {
        private DatabaseOperations dbOperations;
        private BindingSource bindingSource;

        public frmDictionaryOperations()
        {
            InitializeComponent();
            bindingSource = new BindingSource();
            dbOperations = new DatabaseOperations();
            btnDelete.Visible = false;
            btnSave.Visible = false;
        }

        private void frmDictionaryOperations_Load(object sender, EventArgs e)
        {
            ClearTextBoxes();
            LoadDatabaseView();
            PopulateComboBoxWithLanguages();
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

        private void comboBox_Languages_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadDatabaseView();
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
                string columnName = "Word";
                string filterExpression = $"[{columnName}] LIKE '%{filterText}%'";
                bindingSource.Filter = filterExpression;
            }
        }

        private void dgvDados_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtWordId.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[0].Value.ToString();
            txtWord.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[1].Value.ToString();
            txtDefinition.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[2].Value.ToString();
            txtExample.Text = dgvDados.Rows[dgvDados.CurrentRow.Index].Cells[3].Value.ToString();
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (txtWordId.Text.Length == 0)
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
            if (txtWordId.Text.Length == 0)
            {
                MessageBox.Show("No data selected. Operation can't proceed.");
                return;
            }
            btnSave.Visible = true;
            btnDelete.Visible = false;
            HandleEnableAllTextbox(true);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtWordId.Text.Length == 0)
            {
                MessageBox.Show("No data selected. Operation can't proceed.");
                return;
            }

            var resp = MessageBox.Show("You sure to delete the word '" + txtWord.Text + "' from the database ? This action is irreversible.",
               "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                dbOperations.DeleteFromDatabaseTable("tbDefinitions", "WordID", Convert.ToInt32(txtWordId.Text));
                dbOperations.DeleteFromDatabaseTable("tbWords", "WordID", Convert.ToInt32(txtWordId.Text));
            }

            MessageBox.Show("Entry deleted successfully!");
            ResetAllFields();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtWordId.Text.Length == 0)
            {
                MessageBox.Show("No data selected. Operation can't proceed.");
                return;
            }

            var resp = MessageBox.Show("You sure to update the word '" + txtWord.Text + "' from the database ?",
                "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                var newData = new Dictionary<string, string>
                {
                    { "Word", txtWord.Text },
                    { "WordDefinition", txtDefinition.Text },
                    { "WordExample", txtExample.Text },
                    { "WordId", txtWordId.Text },
                    { "LanguageName", comboBox_Languages.Text },
                };

                string updateQuery = @"UPDATE tblWords
                                           SET 
                                               Word = @Word,
                                               WordDefinition = @WordDefinition,
                                               WordExample = @WordExample
                                           WHERE WordId = @WordId 
                                           AND LanguageId = (SELECT LanguageId FROM tbLanguages WHERE LanguageName = @LanguageName);";

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

        private void LoadDatabaseView()
        {
            var selectString = $@"SELECT WordId, Word, WordDefinition, WordExample, LanguageName, LanguageCode 
                                  FROM tblWords w JOIN tbLanguages l ON w.LanguageId = l.LanguageId 
                                  WHERE l.LanguageName =  N'{comboBox_Languages.Text}'";

            bindingSource.DataSource = dbOperations.SelectAllFromDatabase("tblWords", selectString);
            dgvDados.DataSource = bindingSource;
            dgvDados.Enabled = true;
        }

        private void ClearTextBoxes()
        {
            txtExample.Clear();
            txtDefinition.Clear();
            txtWord.Clear();
            txtWordId.Clear();
            txtFilterWord.Clear();
            comboBox_Languages.SelectedIndex = -1;
        }

        private void HandleEnableAllTextbox(bool enable)
        {
            txtWord.Enabled = enable;
            txtDefinition.Enabled = enable;
            txtExample.Enabled = enable;
        }

        private void tsbExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ResetAllFields()
        {
            ClearTextBoxes();
            HandleEnableAllTextbox(false);
            btnDelete.Visible = false;
            btnSave.Visible = false;
            LoadDatabaseView();
        }

        private void PopulateComboBoxWithLanguages()
        {
            var selectString = $"SELECT LanguageName FROM tbLanguages";
            var languagesTable = dbOperations.SelectAllFromDatabase("tbLanguages", selectString);
            if (languagesTable.Rows.Count > 0)
            {
                foreach (DataRow row in languagesTable.Rows)
                {
                    comboBox_Languages.Items.Add(row["LanguageName"].ToString());
                }
            }
            else
            {
                MessageBox.Show("No languages found in the database.");
            }
        }
    }
}
