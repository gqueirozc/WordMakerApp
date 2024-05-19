using System;
using System.Windows.Forms;
using WordMakerDashboard.Database;

namespace WordMakerDashboard
{
    public partial class frmDatabaseGridView : Form
    {

        private readonly BindingSource bindingSource;
        private readonly DatabaseOperations dbOperations;
        private readonly string TableName;
        private readonly string SearchColumn;

        public frmDatabaseGridView(string tableName, string searchColumn)
        {
            InitializeComponent();
            dbOperations = new DatabaseOperations();
            bindingSource = new BindingSource();
            TableName = tableName;
            SearchColumn = searchColumn;
        }

        private void frmDatabaseGridView_Load(object sender, EventArgs e)
        {
            LoadDatabaseView();
            Text = "Database Consult View - " + TableName;
        }

        private void LoadDatabaseView()
        {   
            bindingSource.DataSource = dbOperations.SelectAllFromDatabase(TableName);
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
                string columnName = SearchColumn;
                string filterExpression = $"[{columnName}] LIKE '%{filterText}%'";
                bindingSource.Filter = filterExpression;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDatabaseView();
        }
    }
}
