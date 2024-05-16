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
using WordMakerDashboard.Services;
using static System.Resources.ResXFileRef;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WordMakerDashboard
{
    public partial class Form1 : Form
    {
        BackgroundWorker progressWorker;
        string selectedLanguageCode;

        public Form1()
        {
            InitializeComponent();
            progressWorker = new BackgroundWorker();
            progressWorker.DoWork += new DoWorkEventHandler(bgWorkerProgressBarGlobal_DoWork);
            progressWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorkerProgressBarGlobal_ProgressChanged);
            progressWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerProgressBarGlobal_RunWorkerCompleted);
            progressWorker.WorkerReportsProgress = true;
            progressWorker.WorkerSupportsCancellation = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

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
                MessageBox.Show("Successful Completion.");
            }
        }

        private void bgWorkerProgressBarGlobal_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarGlobal.Value = e.ProgressPercentage;
        }


        private void PopulateFullDB()
        {
            string language = comboBox_Languages.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(language))
            {
                MessageBox.Show("Please select a language.");
                return;
            }

            try
            {
                progressBarGlobal.Visible = true;
                selectedLanguageCode = language;
                progressWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load language dictionary: {ex.Message}");
            }
        }


        private void PopulateFullDB2()
        {
            var dictionary = DictionaryService.LoadDictionary(selectedLanguageCode);
            DatabaseConnect.PopulateDatabaseWithNewDictionary(dictionary, progressWorker);
        }
    }
}
