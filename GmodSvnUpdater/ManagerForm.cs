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

        ///<summary>
        /// Show the user if addon dir is not set and if it is list all addons in the listView
        /// </summary>
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
            // Ask user for the addon folder
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            // Check if user actually gave a directory and save it to settings
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
            // Check if the addon dir is set and then update every SVN repository
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
            // ...else give user eror that the dir is not set
            else
            {
                MessageBox.Show(Resources.updateAddonDirErrorMessage, Resources.updateErrorHeader);
            }
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            // Warn user if addon directory is not set
            if (Settings.Default.AddonDir == String.Empty)
            {
                MessageBox.Show(Resources.updateAddonDirErrorMessage, Resources.updateErrorHeader);
                return;
            }
            // Get the url to the repository
            var answer = Microsoft.VisualBasic.Interaction.InputBox("Please give the url to the repository",
                                                                    "Add...", "", 100, 100);
            // Make sure the user actually answered something
            if (answer == String.Empty)
            {
                MessageBox.Show(@"You need to give the URL", @"No url...");
                return;
            }
            // Check if the url is for a git repository
            if (answer.LastIndexOf("git") != 0)
            {
                var dir = Microsoft.VisualBasic.Interaction.InputBox("Please give name of the folder where to save\nTHIS MUST BE SET IF NOT TOLD OTHERWISE\nUsually the name of the mod", "Folder name...", "", 100, 100);
                GitSharp.Git.Clone(new GitSharp.Commands.CloneCommand{Source = answer, Directory = Settings.Default.AddonDir + "\\" + dir});
            }
            // ...else checkout a SVN repository
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
            // Make sure user wants to remove the addon and remove if so
            DialogResult dlgResult = MessageBox.Show(string.Format("Do you really want to remove {0}?", listAddonsList.SelectedItems[0].Text), @"Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.Yes)
            {
                DeleteSubFolders(Settings.Default.AddonDir + "\\" + listAddonsList.SelectedItems[0].Text, ".svn", true);
                DeleteSubFolders(Settings.Default.AddonDir + "\\" + listAddonsList.SelectedItems[0].Text, ".git", true);
                Directory.Delete(Settings.Default.AddonDir + "\\" + listAddonsList.SelectedItems[0].Text, true);
            }
            InitializeStuff();
        }

        /// <summary>
        /// Removes read-only flag from .svn and .git files and removes them and the folders they are in
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="wildcardPattern"></param>
        /// <param name="top"></param>
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