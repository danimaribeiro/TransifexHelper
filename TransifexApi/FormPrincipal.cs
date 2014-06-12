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
    }
}
