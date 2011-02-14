using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using GmodSvnUpdater.Properties;
using SharpSvn;

namespace GmodSvnUpdater
{
    public partial class SvnUpdaterForm : Form
    {
        public SvnUpdaterForm()
        {
            InitializeComponent();
            InitializeSvnStuff();
        }

        public void InitializeSvnStuff()
        {
            if (Properties.Settings.Default.AddonDir == String.Empty)
            {
                dirSetLabel.Text = Resources.notSetString;
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.AddonDir))
            {
                dirSetLabel.Text = String.Empty;
                listAddonsList.Items.Clear();
                foreach (var dir in Directory.GetDirectories(Properties.Settings.Default.AddonDir))
                {
                    if (Directory.Exists(dir + "\\.svn"))
                    {
                        listAddonsList.Items.Add(dir.Substring(dir.LastIndexOf("\\") + 1));
                    }
                }
            }
        }

        private void setDirBut_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            if (folderBrowserDialog.SelectedPath != String.Empty)
            {
                if (folderBrowserDialog.SelectedPath.ToLower().IndexOf("addons") != 0)
                {
                    Properties.Settings.Default.AddonDir = folderBrowserDialog.SelectedPath;
                    Properties.Settings.Default.Save();
                    InitializeSvnStuff();
                }
            }
        }

        private void updateBut_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.AddonDir))
            {
                Parallel.ForEach(Directory.GetDirectories(Properties.Settings.Default.AddonDir), dir =>
                                                                                                 {
                                                                                                     if (Directory.Exists(dir +"\\.svn"))
                                                                                                     {
                                                                                                         SvnClient svnClient = new SvnClient();
                                                                                                         svnClient.Update(dir);
                                                                                                     }
                                                                                                 });
                MessageBox.Show(Resources.updateCompleteMessage, Resources.updateCompleteHeader);
            }
            else
            {
                MessageBox.Show(Resources.updateAddonDirErrorMessage, Resources.updateErrorHeader);
            }
        }
    }
}