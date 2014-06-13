using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TransifexApi
{
    public partial class FormPrincipal : Form
    {
        private Base.Configuration _configuration;
        private DateTime _lastShow;
        private List<Api.Resources> _resources;
        private List<Api.Translation> _translations;

        private int index_resource = 0;
        private int index_translation = 0;
        private bool isDirty;

        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            try
            {
                _lastShow = DateTime.Now;
                LoadConfiguration();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Sorry! Inform this to the developer.");
            }
        }

        private void LoadConfiguration()
        {
            _configuration = Base.Configuration.GetDefault();
            if (_configuration != null)
            {
                this.Text = string.Format("Transifex - Translator Helper - {0}", _configuration.ActiveProject);
                this.statusLabel.Text = string.Format("Project: {0} - Username: {1} - {2}", _configuration.ActiveProject, _configuration.Username, _configuration.LanguadeCode);

                var api = new Api.ClientApi(_configuration);
                _resources = api.ProjectResources(_configuration.ActiveProject);
                LoadTranslations();
                LoadNextSentence();
            }
        }

        private void LoadTranslations()
        {
            if (index_resource < _resources.Count)
            {
                var api = new Api.ClientApi(_configuration);
                _translations = api.Translations(_configuration.ActiveProject, _resources[index_resource].slug);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                _lastShow = DateTime.Now;
            }
            else if (_configuration != null)
            {
                var minutes = _configuration.TimeBetweenTranslation;
                if ((DateTime.Now - _lastShow) > TimeSpan.FromMinutes(minutes))
                {
                    this.notification.ShowBalloonTip(1000);
                }
            }
        }

        private void notification_BalloonTipClicked(object sender, EventArgs e)
        {
            this.Show();
        }

        private void minimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ConfigurationForm().ShowDialog();
            LoadConfiguration();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isDirty)
            {
                if (MessageBox.Show("You have typed a translation, do you like to send it before proceed?", "Atention", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    UpdateTranslation();
                    isDirty = false;
                }
            }
            index_translation++;
            LoadNextSentence();
        }

        private void LoadNextSentence()
        {
            while (true)
            {
                if (index_resource < _resources.Count)
                {
                    if (index_translation < _translations.Count)
                    {
                        var translation = _translations[index_translation];
                        if (!translation.reviewed && string.IsNullOrWhiteSpace(translation.translation))
                        {
                            CheckTranslation(translation);
                            break;
                        }
                        else
                            index_translation++;
                    }
                    else
                    {
                        index_resource++;
                        LoadTranslations();
                    }
                }
                else
                {
                    MessageBox.Show("Congratulations there is no string for translate anymore!", "Nothing");
                    break;
                }
            }
        }

        private void CheckTranslation(Api.Translation translation)
        {
            var api = new Api.ClientApi(_configuration);
            translation = api.GetTranslation(_configuration.ActiveProject, _resources[index_resource].slug, translation);
            if (!translation.reviewed && string.IsNullOrWhiteSpace(translation.translation))
            {
                _translations[index_translation] = translation;
                textToBeTranslated.Text = translation.source_string;
            }
            else
                index_translation++;
        }

        private bool UpdateTranslation()
        {
            if (index_resource < _resources.Count)
            {
                if (index_translation < _translations.Count)
                {
                    _translations[index_translation].translation = richTextBox2.Text;
                    var api = new Api.ClientApi(_configuration);
                    return api.UpdateTranslation(_configuration.ActiveProject, _resources[index_resource].slug,
                        _translations[index_translation]);
                   
                }
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (UpdateTranslation())
            {
                isDirty = false;
                LoadNextSentence();
            }
            else
                MessageBox.Show("It wasn't possible to save the translation. Try again!", "Sorry!");
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            isDirty = true;
        }
      
    }
}
