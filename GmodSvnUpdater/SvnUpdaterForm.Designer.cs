namespace GmodSvnUpdater
{
    partial class SvnUpdaterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	this.updateBut = new System.Windows.Forms.Button();
        	this.setDirBut = new System.Windows.Forms.Button();
        	this.dirSetLabel = new System.Windows.Forms.Label();
        	this.listAddonsList = new System.Windows.Forms.ListView();
        	this.addButton = new System.Windows.Forms.Button();
        	this.removeButton = new System.Windows.Forms.Button();
        	this.editButton = new System.Windows.Forms.Button();
        	this.SuspendLayout();
        	// 
        	// updateBut
        	// 
        	this.updateBut.Location = new System.Drawing.Point(12, 12);
        	this.updateBut.Name = "updateBut";
        	this.updateBut.Size = new System.Drawing.Size(100, 23);
        	this.updateBut.TabIndex = 0;
        	this.updateBut.Text = "Update Addons";
        	this.updateBut.UseVisualStyleBackColor = true;
        	this.updateBut.Click += new System.EventHandler(this.updateBut_Click);
        	// 
        	// setDirBut
        	// 
        	this.setDirBut.Location = new System.Drawing.Point(172, 12);
        	this.setDirBut.Name = "setDirBut";
        	this.setDirBut.Size = new System.Drawing.Size(100, 23);
        	this.setDirBut.TabIndex = 1;
        	this.setDirBut.Text = "Set Addons Dir";
        	this.setDirBut.UseVisualStyleBackColor = true;
        	this.setDirBut.Click += new System.EventHandler(this.setDirBut_Click);
        	// 
        	// dirSetLabel
        	// 
        	this.dirSetLabel.AutoSize = true;
        	this.dirSetLabel.Location = new System.Drawing.Point(163, 38);
        	this.dirSetLabel.Name = "dirSetLabel";
        	this.dirSetLabel.Size = new System.Drawing.Size(83, 13);
        	this.dirSetLabel.TabIndex = 2;
        	this.dirSetLabel.Text = "Placeholder text";
        	// 
        	// listAddonsList
        	// 
        	this.listAddonsList.Location = new System.Drawing.Point(12, 85);
        	this.listAddonsList.MultiSelect = false;
        	this.listAddonsList.Name = "listAddonsList";
        	this.listAddonsList.Size = new System.Drawing.Size(165, 165);
        	this.listAddonsList.TabIndex = 3;
        	this.listAddonsList.UseCompatibleStateImageBehavior = false;
        	this.listAddonsList.View = System.Windows.Forms.View.List;
        	// 
        	// addButton
        	// 
        	this.addButton.Location = new System.Drawing.Point(183, 85);
        	this.addButton.Name = "addButton";
        	this.addButton.Size = new System.Drawing.Size(75, 23);
        	this.addButton.TabIndex = 4;
        	this.addButton.Text = "Add";
        	this.addButton.UseVisualStyleBackColor = true;
        	this.addButton.Click += new System.EventHandler(this.AddButtonClick);
        	// 
        	// removeButton
        	// 
        	this.removeButton.Location = new System.Drawing.Point(183, 114);
        	this.removeButton.Name = "removeButton";
        	this.removeButton.Size = new System.Drawing.Size(75, 23);
        	this.removeButton.TabIndex = 5;
        	this.removeButton.Text = "Remove";
        	this.removeButton.UseVisualStyleBackColor = true;
        	this.removeButton.Click += new System.EventHandler(this.RemoveButtonClick);
        	// 
        	// editButton
        	// 
        	this.editButton.Location = new System.Drawing.Point(183, 143);
        	this.editButton.Name = "editButton";
        	this.editButton.Size = new System.Drawing.Size(75, 23);
        	this.editButton.TabIndex = 6;
        	this.editButton.Text = "Edit";
        	this.editButton.UseVisualStyleBackColor = true;
        	// 
        	// SvnUpdaterForm
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(284, 262);
        	this.Controls.Add(this.editButton);
        	this.Controls.Add(this.removeButton);
        	this.Controls.Add(this.addButton);
        	this.Controls.Add(this.listAddonsList);
        	this.Controls.Add(this.dirSetLabel);
        	this.Controls.Add(this.setDirBut);
        	this.Controls.Add(this.updateBut);
        	this.DoubleBuffered = true;
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        	this.MaximizeBox = false;
        	this.Name = "SvnUpdaterForm";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Garry\'s mod SVN manager";
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button updateBut;
        private System.Windows.Forms.Button setDirBut;
        private System.Windows.Forms.Label dirSetLabel;
        private System.Windows.Forms.ListView listAddonsList;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button editButton;
    }
}

