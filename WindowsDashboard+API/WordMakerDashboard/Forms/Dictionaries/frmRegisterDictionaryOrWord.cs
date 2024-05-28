using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WordMakerDashboard.Forms.Dictionaries;
using WordMakerDashboard.Services;

namespace WordMakerDashboard
{
    public partial class frmRegisterDictionaryOrWord : Form
    {
        public frmRegisterDictionaryOrWord()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var languageName = txtLanguage.Text;
            var word = txtWord.Text;
            var newData = new Dictionary<string, object>
            {
                { "Word", txtWord.Text },
                { "WordDefinition", txtDefinition.Text },
                { "WordExample", txtExample.Text },
                { "LanguageName", txtLanguage.Text },
            };

            if (!DatabaseService.LanguageExists(languageName))
            {   
                var resp = MessageBox.Show("Language not found. Add new Language?", "Language Not Found!", MessageBoxButtons.YesNo);
                if (resp == DialogResult.Yes)
                {
                    var languageCode = Interaction.InputBox("Please insert the Language Code (e.g. 'en', 'fr'):", "Adding new Language");

                    var query = $@"INSERT INTO tbLanguages (LanguageCode, LanguageName) VALUES
                                ('{languageCode}', '{languageName}')";

                    try
                    {
                        DatabaseService.ExecuteQuery(query);
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

            if (DatabaseService.WordExists(word, languageName, out var wordId))
            {
                newData.Add("WordId", wordId);
                if (PromptForUpdate(word))
                {
                    try
                    {
                        DatabaseService.UpdateWord(newData);
                        MessageBox.Show("Word updated successfully!");
                        ClearTextBoxes();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occured while trying to add the data: " + ex.Message);
                        return;
                    }
                }
                else
                {
                    ClearTextBoxes();
                    return;
                }
            }
            else
            {
                try
                {
                    DatabaseService.InsertWord(newData);
                    MessageBox.Show("Word added successfully!");
                    ClearTextBoxes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured while trying to add the data: " + ex.Message);
                    return;
                }
            }
        }

        private bool PromptForUpdate(string word)
        {
            return MessageBox.Show($"The word '{word}' already exists for the language. Do you want to update its values?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        private void ClearTextBoxes()
        {
            txtWord.Clear();
            txtDefinition.Clear();
            txtExample.Clear();
            txtLanguage.Clear();
        }

        private void tsbConsult_Click(object sender, EventArgs e)
        {
            var form = new frmDatabaseGridView("tblWords", true);
            form.MdiParent = MdiParent;
            form.Show();
        }

        private void tsbExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsbAddJson_Click(object sender, EventArgs e)
        {
            var form = new frmRegisterJsonFile();
            form.MdiParent = MdiParent;
            form.Show();
        }
    }
}