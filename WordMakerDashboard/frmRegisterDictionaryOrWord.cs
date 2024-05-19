using System;
using System.Windows.Forms;
using WordMakerDashboard.Database;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace WordMakerDashboard
{
    public partial class frmRegisterDictionaryOrWord : Form
    {
        private DatabaseOperations dbOperations;

        public frmRegisterDictionaryOrWord()
        {
            InitializeComponent();
            dbOperations = new DatabaseOperations();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var languageName = txtLanguage.Text;
            var word = txtWord.Text;
            var definition = txtDefinition.Text;
            var example = txtExample.Text;

            if (!dbOperations.LanguageExists(languageName))
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

            if (dbOperations.WordExists(word, languageName, out var wordId))
            {
                if (PromptForUpdate(word))
                {
                    UpdateWord(wordId);
                }
                else
                {
                    ClearTextBoxes();
                    return;
                }
            }
            else
            {
                var query = $@"INSERT INTO tblWords (Word, LanguageId, WordDefinition, WordExample) 
                                VALUES ('{word}', (SELECT LanguageId FROM tbLanguages WHERE LanguageName = '{languageName}'), '{definition}', '{example}')";
                try { 
                    dbOperations.ExecuteQuery(query);
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

        public bool PromptForUpdate(string word)
        {
            return MessageBox.Show($"The word '{word}' already exists for the language. Do you want to update its values?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }


        private void UpdateWord(int wordId)
        {
            var newData = new Dictionary<string, string>
                {
                    { "Word", txtWord.Text },
                    { "WordDefinition", txtDefinition.Text },
                    { "WordExample", txtExample.Text },
                    { "LanguageName", txtLanguage.Text },
                    { "WordId", wordId.ToString()}
                };

            string updateQuery = $@"UPDATE tblWords
                                           SET Word = @Word,
                                               WordDefinition = @WordDefinition,
                                               WordExample = @WordExample
                                           WHERE WordId = @WordId 
                                           AND LanguageId = (SELECT LanguageId FROM tbLanguages WHERE LanguageName = @LanguageName);";
            try
            {
                dbOperations.UpdateDatabaseEntry(updateQuery, newData);
                MessageBox.Show("Data altered successfully!");
                ClearTextBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while trying to alter the data: " + ex.Message);
                return;
            }
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
            form.MdiParent = this.MdiParent;
            form.Show();
        }
    }
}
