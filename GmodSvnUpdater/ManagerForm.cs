﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using GmodAddonManager.Properties;
using SharpSvn;

namespace GmodAddonManager
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
                    if (Directory.Exists(dir + "\\.svn") || (Directory.Exists(dir + "\\.git") && Environment.ExpandEnvironmentVariables("path").IndexOf("git") != 0))
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
            string answer;
            if (Environment.ExpandEnvironmentVariables("path").IndexOf("git") != 0)
            {
                answer = Microsoft.VisualBasic.Interaction.InputBox("Please give the url to the repository\ngit support enabled",
                                                                    "Add...", "url"); 
            }
            else
            {
                answer = Microsoft.VisualBasic.Interaction.InputBox("Please give the url to the repository\ngit support disabled",
                                                                    "Add...", "url");
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
            if (answer.LastIndexOf("git") != 0)
            {
                var dir = Microsoft.VisualBasic.Interaction.InputBox("Please give name of the folder where to save\nTHIS MUST BE SET IF NOT TOLD OTHERWISE\nUsually the name of the mod", "Folder name...", "");
                Process.Start("git", string.Format("clone {0} {1}", answer, Settings.Default.AddonDir + "\\" + dir)).WaitForExit();
            }
                // ...else checkout a SVN repository
            else
            {
                var url = new Uri(answer);
                using (var svnClient = new SvnClient())
                {
                    var dir = Microsoft.VisualBasic.Interaction.InputBox("Please give name of the folder where to save\nTHIS MUST BE SET IF NOT TOLD OTHERWISE\nUsually the name of the mod", "Folder name...");
                    svnClient.CheckOut(url, Settings.Default.AddonDir + "\\" + dir);
                }
            }
            InitializeStuff();
            // Check if the url is for a git repository
        }

        private void RemoveButtonClick(object sender, EventArgs e)
        {
            // Make sure user wants to remove the addon and remove if so
            DialogResult dlgResult = DialogResult.None;
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
                    DeleteSubFolders(Settings.Default.AddonDir + "\\" + item.Text, ".svn", true);
                    DeleteSubFolders(Settings.Default.AddonDir + "\\" + item.Text, ".git", true);
                    Directory.Delete(Settings.Default.AddonDir + "\\" + item.Text, true);
                }
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