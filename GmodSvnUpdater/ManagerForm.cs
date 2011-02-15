using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using GmodSvnUpdater.Properties;
using SharpSvn;

namespace GmodSvnUpdater
{
    public partial class ManagerForm : Form
    {
        public ManagerForm()
        {
            InitializeComponent();
            InitializeStuff();
        }

        public void InitializeStuff()
        {
            if (Settings.Default.AddonDir == String.Empty)
            {
                dirSetLabel.Text = Resources.notSetString;
            }
            if (!string.IsNullOrEmpty(Settings.Default.AddonDir))
            {
                dirSetLabel.Text = String.Empty;
                listAddonsList.Items.Clear();
                foreach (var dir in Directory.GetDirectories(Settings.Default.AddonDir))
                {
                    if (Directory.Exists(dir + "\\.svn") || Directory.Exists(dir + "\\.git"))
                    {
                        listAddonsList.Items.Add(dir.Substring(dir.LastIndexOf("\\") + 1));
                    }
                }
            }
        }

        private void SetDirButClick(object sender, EventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            if (folderBrowserDialog.SelectedPath != String.Empty)
            {
                if (folderBrowserDialog.SelectedPath.ToLower().IndexOf("addons") != 0)
                {
                    Settings.Default.AddonDir = folderBrowserDialog.SelectedPath;
                    Settings.Default.Save();
                    InitializeStuff();
                }
            }
        }

        private void UpdateButClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Settings.Default.AddonDir))
            {
                Parallel.ForEach(Directory.GetDirectories(Settings.Default.AddonDir), dir =>
                                                                                                 {
                                                                                                     if (Directory.Exists(dir + "\\.svn"))
                                                                                                     {
                                                                                                         var svnClient = new SvnClient();
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

        private void AddButtonClick(object sender, EventArgs e)
        {
            if (Settings.Default.AddonDir == String.Empty)
            {
                MessageBox.Show(Resources.updateAddonDirErrorMessage, Resources.updateErrorHeader);
                return;
            }
            var answer = Microsoft.VisualBasic.Interaction.InputBox("Please give the url to the repository",
                                                                    "Add...", "", 100, 100);
            if (answer == String.Empty)
            {
                MessageBox.Show(@"You need to give the URL", @"No url...");
                return;
            }
            if (answer.LastIndexOf("git") != 0)
            {
                var dir = Microsoft.VisualBasic.Interaction.InputBox("Please give name of the folder where to save\nTHIS MUST BE SET IF NOT TOLD OTHERWISE\nUsually the name of the mod", "Folder name...", "", 100, 100);
                GitSharp.Git.Clone(new GitSharp.Commands.CloneCommand{Source = answer, Directory = Settings.Default.AddonDir + "\\" + dir});
            }
            else
            {
                var url = new Uri(answer);
                using (var svnClient = new SvnClient())
                {
                    var dir = Microsoft.VisualBasic.Interaction.InputBox("Please give name of the folder where to save\nTHIS MUST BE SET IF NOT TOLD OTHERWISE\nUsually the name of the mod", "Folder name...", "", 100, 100);
                    svnClient.CheckOut(url, Settings.Default.AddonDir + "\\" + dir);
                } 
            }
            InitializeStuff();
        }

        private void RemoveButtonClick(object sender, EventArgs e)
        {
            DialogResult dlgResult = MessageBox.Show(string.Format("Do you really want to remove {0}?", listAddonsList.SelectedItems[0].Text), @"Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.Yes)
            {
                DeleteSubFolders(Settings.Default.AddonDir + "\\" + listAddonsList.SelectedItems[0].Text, ".svn", true);
                DeleteSubFolders(Settings.Default.AddonDir + "\\" + listAddonsList.SelectedItems[0].Text, ".git", true);
                Directory.Delete(Settings.Default.AddonDir + "\\" + listAddonsList.SelectedItems[0].Text, true);
            }
            InitializeStuff();
        }

        public static void DeleteSubFolders(string folderPath, string wildcardPattern, bool top)
        {
            String[] list;

            //If we are at the top level, get all directory names that match the pattern
            if (top)
            {
                list = Directory.GetDirectories(folderPath, wildcardPattern, SearchOption.AllDirectories);
            }
            else //Get directories and files for matching sub directories
            {
                list = Directory.GetFileSystemEntries(folderPath, wildcardPattern);
            }

            foreach (string item in list)
            {
                //Sub directories
                if (Directory.Exists(item))
                {
                    //Match all sub directories
                    DeleteSubFolders(item, "*", false);
                }
                else // Files in directory
                {
                    //Get the attribute for the file
                    FileAttributes fileAtts = File.GetAttributes(item);
                    //If it is read only make it writable
                    if (( fileAtts & FileAttributes.ReadOnly ) != 0)
                    {
                        File.SetAttributes(item, fileAtts & ~FileAttributes.ReadOnly);
                    }
                    File.Delete(item);
                }
            }
            //Delete the matching folder that we are in
            if (top == false)
            {
                Directory.Delete(folderPath);
            }
        }
    }
}