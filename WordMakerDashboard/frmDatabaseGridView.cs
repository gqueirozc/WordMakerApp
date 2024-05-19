using System;
using System.Data;
using System.Windows.Forms;
using WordMakerDashboard.Database;

namespace WordMakerDashboard
{
    public partial class frmDatabaseGridView : Form
    {
        private readonly BindingSource bindingSource;
        private readonly DatabaseOperations dbOperations;
        private readonly string TableName;
        private readonly bool IsDictionary;

        public frmDatabaseGridView(string tableName, bool isDictionary = false)
        {
            InitializeComponent();
            dbOperations = new DatabaseOperations();
            bindingSource = new BindingSource();
            TableName = tableName;
            IsDictionary = isDictionary;
        }

        private void frmDatabaseGridView_Load(object sender, EventArgs e)
        {
            Text = "Database Consult View - " + TableName;

            if (!IsDictionary)
            {
                cbLanguages.Visible = false;
                label1.Visible = false;
                LoadDatabaseView();
                return;
            }
            else
            {
                cbLanguages.Visible = true;
                label1.Visible = true;
                PopulateComboBoxWithLanguages();
            }
        }

        private void LoadDatabaseView(string query = "")
        {
            bindingSource.DataSource = dbOperations.SelectAllFromDatabase(TableName, query);        
            dgvDisplay.DataSource = bindingSource;

            var i = 0;
            foreach (var column in dgvDisplay.Columns)
            {
                cbFilters.Items.Add(dgvDisplay.Columns[i].DataPropertyName);
                i += 1;
            }

            dgvDisplay.Enabled = true;
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
                string columnName = cbFilters.Text;
                string filterExpression = $"[{columnName}] LIKE '%{filterText}%'";
                bindingSource.Filter = filterExpression;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!IsDictionary)
            {
                cbLanguages.Visible = false;
                label1.Visible = false;
                LoadDatabaseView();
                return;
            }
        }

        private void PopulateComboBoxWithLanguages()
        {
            var selectString = $"SELECT LanguageName FROM tbLanguages";
            var languagesTable = dbOperations.SelectAllFromDatabase("tbLanguages", selectString);
            if (languagesTable.Rows.Count > 0)
            {
                foreach (DataRow row in languagesTable.Rows)
                {
                    cbLanguages.Items.Add(row["LanguageName"].ToString());
                }
            }
            else
            {
                MessageBox.Show("No languages found in the database.");
            }
        }

        private void cbLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectString = $@"SELECT * FROM tblWords w JOIN tbLanguages l ON w.LanguageId = l.LanguageId 
                                  WHERE l.LanguageName =  N'{cbLanguages.Text}'";
            LoadDatabaseView(selectString);
        }
    }
}
