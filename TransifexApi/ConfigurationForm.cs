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
        private bool isProjectRight;

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

                comboLanguage.SelectedValue = configuration.LanguadeCode;
                projectNameTextbox.Text = configuration.ActiveProject;
                isProjectRight = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isProjectRight)
            {
                TransifexApi.Properties.Settings.Default.Username = usernameText.Text;
                TransifexApi.Properties.Settings.Default.Password = passwordText.Text;
                TransifexApi.Properties.Settings.Default.LanguageCode = comboLanguage.SelectedValue.ToString();
                TransifexApi.Properties.Settings.Default.ActiveProject = projectNameTextbox.Text;
                TransifexApi.Properties.Settings.Default.TimeBetweenNotification = (int)timeNumeric.Value;
                TransifexApi.Properties.Settings.Default.Save();
                this.Close();
            }
            else
                MessageBox.Show("Check if the project exists and all the other values are filled.", "Check the values");
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                var api = new Api.ClientApi(new Base.Configuration() { Username = usernameText.Text, Password = passwordText.Text });
                var projectInfo = api.InfoProject(projectNameTextbox.Text);
                if (projectInfo != null)
                {
                    labelProject.Text = projectInfo.description;
                    labelSourceLanguage.Text = projectInfo.source_language_code;
                    isProjectRight = true;
                }
                else
                    MessageBox.Show("The project doesn't exist. Check again the name.", "Nothing");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Try again. It's a network problem.", "Sorry!");
            }
        }

    }
}
