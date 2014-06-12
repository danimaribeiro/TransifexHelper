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
    public partial class ConfigurationForm : Form
    {
        public ConfigurationForm()
        {
            InitializeComponent();
        }

        private void ConfigurationForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadConfiguration();
            }
            catch (Exception ex)
            {

            }
        }

        private void LoadConfiguration()
        {
            var configuration = Base.Configuration.GetDefault();
            if (configuration != null)
            {
                usernameText.Text = configuration.Username;
                passwordText.Text = configuration.Password;
                timeNumeric.Value = configuration.TimeBetweenTranslation;
                var api = new Api.ClientApi(configuration);
                var languages = api.Languages();

                comboLanguage.DataSource = languages;
                comboLanguage.DisplayMember = "name";
                comboLanguage.ValueMember = "code";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TransifexApi.Properties.Settings.Default.Username = usernameText.Text;
            TransifexApi.Properties.Settings.Default.Password = passwordText.Text;
            TransifexApi.Properties.Settings.Default.LanguageCode = comboLanguage.SelectedValue.ToString();
            var project = (Api.Projects)listProjects.SelectedItem;
            TransifexApi.Properties.Settings.Default.ActiveProject = project.slug;
            TransifexApi.Properties.Settings.Default.TimeBetweenNotification = (int)timeNumeric.Value;
            TransifexApi.Properties.Settings.Default.Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var api = new Api.ClientApi(new Base.Configuration() { Username = usernameText.Text, Password = passwordText.Text });
                var languages = api.Languages();

                comboLanguage.DataSource = languages;
                comboLanguage.DisplayMember = "name";
                comboLanguage.ValueMember = "code";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Try again. It's a network problem.", "Sorry!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var api = new Api.ClientApi(new Base.Configuration() { Username = usernameText.Text, Password = passwordText.Text });
                var languages = api.Projects();

                listProjects.DataSource = languages;
                listProjects.DisplayMember = "description";                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Try again. It's a network problem.", "Sorry!");
            }
        }

    }
}
