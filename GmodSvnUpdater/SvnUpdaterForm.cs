using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using GmodSvnUpdater.Properties;
using Microsoft.VisualBasic;
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
        
        void AddButtonClick(object sender, EventArgs e)
        {
        	if (Properties.Settings.Default.AddonDir == String.Empty) 
        	{
        		MessageBox.Show(Resources.updateAddonDirErrorMessage, Resources.updateErrorHeader);
        		return;
        	}
			var answer = Microsoft.VisualBasic.Interaction.InputBox("Please give the url to the repository",
        	                                                        "Add...", "", 100, 100);
        	if (answer != String.Empty)
        	{
        		Uri url = new Uri(answer);
	        	using(SvnClient svnClient = new SvnClient())
	        	{
	        		var dir = Microsoft.VisualBasic.Interaction.InputBox("Please give name of the folder where to save\nTHIS MUST BE SET IF NOT TOLD OTHERWISE\nUsually the name of the mod", "Folder name...", "", 100, 100);
	        		svnClient.CheckOut(url, Properties.Settings.Default.AddonDir + "\\" + dir);
	        	}
	        	InitializeSvnStuff();
        	}
        }
        
        void RemoveButtonClick(object sender, EventArgs e)
        {
        	DialogResult dlgResult = MessageBox.Show("Do you really want to remove "+ listAddonsList.SelectedItems[0].Text + "?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dlgResult == DialogResult.Yes)
			{
				DeleteSubFolders(Properties.Settings.Default.AddonDir + "\\" + listAddonsList.SelectedItems[0].Text, ".svn", true);
				Directory.Delete(Properties.Settings.Default.AddonDir + "\\" + listAddonsList.SelectedItems[0].Text, true);
			}
			InitializeSvnStuff();
        }
		public static void DeleteSubFolders(string folderPath, string wildcardPattern, bool top)
		{
		String[] list;
		
		//If we are at the top level, get all directory names that match the pattern
		if (top)
		list = Directory.GetDirectories(folderPath, wildcardPattern, SearchOption.AllDirectories);
		else //Get directories and files for matching sub directories
		list = Directory.GetFileSystemEntries(folderPath, wildcardPattern);
		
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
		if ((fileAtts & FileAttributes.ReadOnly) != 0)
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