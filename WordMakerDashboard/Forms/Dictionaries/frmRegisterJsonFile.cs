using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using WordMakerDashboard.Services;

namespace WordMakerDashboard.Forms.Dictionaries
{
    public partial class frmRegisterJsonFile : Form
    {
        private BackgroundWorker progressWorker;
        private DatabaseService dbOperations;

        public frmRegisterJsonFile()
        {
            InitializeComponent();
            dbOperations = new DatabaseService();

            progressBarGlobal.Visible = false;
            label3.Visible = false;

            progressWorker = new BackgroundWorker();
            progressWorker.DoWork += new DoWorkEventHandler(bgWorkerProgressBarGlobal_DoWork);
            progressWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorkerProgressBarGlobal_ProgressChanged);
            progressWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerProgressBarGlobal_RunWorkerCompleted);
            progressWorker.WorkerReportsProgress = true;
            progressWorker.WorkerSupportsCancellation = true;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            var filePath = "";
            var languageName = txtLanguage.Text;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "C:\\",
                Title = "Select a JSON File",
                Filter = "JSON Files (*.json)|*.json",
                Multiselect = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                progressBarGlobal.Visible = true;
                label3.Visible = true;
                filePath = openFileDialog.FileName;
                txtFilePath.Text = filePath;
            }
            else
            {
                return;
            }
            var dictionary = DictionaryService.LoadDictionary(filePath);

            bool languageExists = dbOperations.LanguageExists(languageName);
            if (!languageExists)
            {
                var resp = MessageBox.Show("Language not found. Add new Language?", "Language Not Found!", MessageBoxButtons.YesNo);
                if (resp == DialogResult.Yes)
                {
                    var languageCode = Interaction.InputBox("Please insert the Language Code (e.g. 'en', 'fr'):", "Adding new Language");

                    var query = $@"INSERT INTO tbLanguages (LanguageCode, LanguageName) VALUES
                                ('{languageCode}', '{languageName}')";

                    try
                    {
                        dbOperations.ExecuteQuery(query);
                        MessageBox.Show("Language added successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occured while trying to add the language: " + ex.Message);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Operation Cancelled.");
                    return;
                }
            }

            var respFound = MessageBox.Show("Language dictionary found. Update all words?", "Language Found!", MessageBoxButtons.YesNo);
            if (respFound == DialogResult.Yes)
            {
                try
                {
                    dbOperations.PopulateDatabaseWithNewDictionary(dictionary, languageName, progressWorker);
                    var resp = MessageBox.Show("Operation Complete!");
                    if (resp == DialogResult.OK)
                    {
                        label3.Text = "Operation Complete!";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured while trying to add the language: " + ex.Message);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Operation Cancelled.");
                return;
            }
        }

        private void bgWorkerProgressBarGlobal_DoWork(object sender, DoWorkEventArgs e)
        {
            progressWorker.ReportProgress(100);
        }

        private void bgWorkerProgressBarGlobal_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Process Cancelled.");
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error occurred: " + e.Error.Message);
            }
            else
            {
                MessageBox.Show("Successful Completion. JSON dictionary added/updated to the database.");
            }
        }

        private void bgWorkerProgressBarGlobal_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarGlobal.Value = e.ProgressPercentage;
        }
    }
}