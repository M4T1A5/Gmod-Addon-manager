using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using GmodAddonManager.Properties;
using SharpSvn;
using Microsoft.Win32;

namespace GmodAddonManager
{
    public partial class ManagerForm : Form
    {
        public ManagerForm()
        {
            InitializeComponent();
            InitializeStuff();
        }

        private static RegistryKey _reg = Registry.CurrentUser;
        private static string _installDir;

        ///<summary>
        /// Show the user if addon dir is not set and if it is list all addons in the listView
        /// </summary>
        private void InitializeStuff()
        {
            _reg = _reg.OpenSubKey("Software\\Valve\\Steam");
            _installDir = _reg.GetValue("SteamPath") + "\\Steamapps\\";
            foreach (var dir in Directory.GetDirectories(_installDir))
            {
                if (!dir.ToLower().Contains("common") && !dir.ToLower().Contains("sourcemods"))
                {
                    _installDir = string.Empty;
                    _installDir = dir + "\\garrysmod\\garrysmod\\addons";
                }
            }
            if (!Directory.Exists(_installDir))
            {
                MessageBox.Show(Resources.noAddonsDirFoundError);
                Environment.Exit(0);
            }
            UpdateStuff();
        }

        private void UpdateStuff()
        {
            listAddonsList.Items.Clear();
            foreach (var dir in Directory.GetDirectories(_installDir))
            {
                if (Directory.Exists(dir + "\\.svn") || ( Directory.Exists(dir + "\\.git") && Environment.ExpandEnvironmentVariables("path").IndexOf("git") != 0 ))
                {
                    listAddonsList.Items.Add(dir.Substring(dir.LastIndexOf("\\") + 1));
                }
            }
        }

        private void UpdateButtonClick(object sender, EventArgs e)
        {
            UpdateButton.Enabled = false;
            UpdateButton.Text = Resources.updateButtonUpdating;
            // Check if user has selected any addons in the list. if not update all
            if (listAddonsList.SelectedItems.Count != 0)
            {
                foreach (ListViewItem item in listAddonsList.SelectedItems)
                {
                    var addonDir = _installDir + "\\" + item.Text;
                    if (Directory.Exists(addonDir + "\\.svn"))
                    {
                        try
                        {
                            var svnClient = new SvnClient();
                            svnClient.Update(addonDir);
                        }
                        catch (SvnWorkingCopyLockException)
                        {
                            var svnClient = new SvnClient();
                            svnClient.CleanUp(addonDir);
                            svnClient.Update(addonDir);
                            throw;
                        }
                    }
                    else if (Directory.Exists(addonDir + "\\.git") && Environment.ExpandEnvironmentVariables("path").IndexOf("git") != 0)
                    {
                        var processInfo = new ProcessStartInfo("git") {WorkingDirectory = addonDir, Arguments = "fetch"};
                        var process = Process.Start(processInfo);
                        process.WaitForExit();
                        process.StartInfo.Arguments = "merge origin/master";
                        process.Start();
                    }
                }
            }
            else
            {
                // Update all repositories in a nice threaded way
                Parallel.ForEach(Directory.GetDirectories(_installDir), dir =>
                                                                        {
                                                                            // Check if the addon is updated by SVN
                                                                            if (Directory.Exists(dir + "\\.svn"))
                                                                            {
                                                                                try
                                                                                {
                                                                                    var svnClient = new SvnClient();
                                                                                    svnClient.Update(dir);
                                                                                }
                                                                                catch (SvnWorkingCopyLockException)
                                                                                {
                                                                                    var svnClient = new SvnClient();
                                                                                    svnClient.CleanUp(dir);
                                                                                    svnClient.Update(dir);
                                                                                    throw;
                                                                                }
                                                                            }
                                                                                // Check if there are any git addons and if the user has git installed and if both are true then "update" the addons
                                                                            else if (Directory.Exists(dir + "\\.git") && Environment.ExpandEnvironmentVariables("path").IndexOf("git") != 0)
                                                                            {
                                                                                var processInfo = new ProcessStartInfo("git") {WorkingDirectory = dir, Arguments = "fetch"};
                                                                                var process = Process.Start(processInfo);
                                                                                process.WaitForExit();
                                                                                process.StartInfo.Arguments = "merge origin/master";
                                                                                process.Start();
                                                                            }
                                                                        });
                MessageBox.Show(Resources.updateCompleteMessage, Resources.updateCompleteHeader);
            }
            UpdateButton.Text = Resources.updateButDefaultText;
            UpdateButton.Enabled = true;
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            // Get the url to the repository
            string answer;
            if (Environment.ExpandEnvironmentVariables("path").IndexOf("git") != 0)
            {
                answer = Microsoft.VisualBasic.Interaction.InputBox(Resources.addRepoMessageGitEnabled, Resources.addRepoHeader, Resources.addRepoDefaultValue);
            }
            else
            {
                answer = Microsoft.VisualBasic.Interaction.InputBox(Resources.addRepoMessageGitDisabled, Resources.addRepoHeader, Resources.addRepoDefaultValue);
            }
            if (answer == string.Empty)
            {
                return;
            }
            if (answer == "url")
            {
                MessageBox.Show(@"You must give the url", @"No url given");
                return;
            }
            // Check if it is a git repository
            if (answer.LastIndexOf("git") != 0)
            {
                var dir = Microsoft.VisualBasic.Interaction.InputBox(Resources.repoFolderMessage, Resources.repoFolderHeader);
                Process.Start("git", string.Format("clone {0} {1}", answer, _installDir + "\\" + dir)).WaitForExit();
            }
                // ...else checkout a SVN repository
            else
            {
                var url = new Uri(answer);
                using (var svnClient = new SvnClient())
                {
                    var dir = Microsoft.VisualBasic.Interaction.InputBox(Resources.repoFolderMessage, Resources.repoFolderHeader);
                    svnClient.CheckOut(url, _installDir + "\\" + dir);
                }
            }
            UpdateStuff();
        }

        private void DeleteButtonClick(object sender, EventArgs e)
        {
            // Make sure user wants to remove the addon and remove if so
            DialogResult dlgResult;
            if (listAddonsList.SelectedItems.Count == 1)
            {
                dlgResult = MessageBox.Show(string.Format("Do you really want to remove {0}?", listAddonsList.SelectedItems[0].Text), @"Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else
            {
                dlgResult = MessageBox.Show(string.Format("Do you really want to remove all those addons ({0})?", listAddonsList.SelectedItems.Count), @"Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            if (dlgResult == DialogResult.Yes)
            {
                foreach (ListViewItem item in listAddonsList.SelectedItems)
                {
                    DeleteSubFolders(_installDir + "\\" + item.Text, ".svn", true);
                    DeleteSubFolders(_installDir + "\\" + item.Text, ".git", true);
                    Directory.Delete(_installDir + "\\" + item.Text, true);
                }
            }
            UpdateStuff();
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

        private void EditButtonClick(object sender, EventArgs e)
        {
            if (listAddonsList.SelectedItems.Count > 1)
            {
                MessageBox.Show(Resources.editTooManySelected, Resources.editErrorHeader);
                return;
            }
            var addonDir = _installDir + "\\" + listAddonsList.SelectedItems[0].Text;
            var answer = Microsoft.VisualBasic.Interaction.InputBox("Give the new url of the repository");
            Uri newUrl;
            if (string.IsNullOrEmpty(answer))
            {
                return;
            }
            try
            {
                newUrl = new Uri(answer);
            }
            catch (Exception)
            {
                MessageBox.Show(@"Url cannot be resolved");
                return;
            }
            if (!string.IsNullOrEmpty(newUrl.ToString()))
            {
                if (MessageBox.Show(@"Are you sure you want to relocate addon to " + newUrl, @"Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
                if (Directory.Exists(addonDir + "\\.git"))
                {
                    var process = Process.Start("git", "remote rm origin");
                    process.WaitForExit();
                    process.StartInfo.Arguments = "remote add origin " + newUrl;
                    process.Start();
                }
                else
                {
                    var svnClient = new SvnClient();
                    var sourceUrl = svnClient.GetRepositoryRoot(addonDir);
                    svnClient.Relocate(addonDir, sourceUrl, newUrl);
                }
            }
        }
    }
}