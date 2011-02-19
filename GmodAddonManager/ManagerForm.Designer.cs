namespace GmodAddonManager
{
    partial class ManagerForm
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
            this.listAddonsList = new System.Windows.Forms.ListView();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // updateBut
            // 
            this.updateBut.Location = new System.Drawing.Point(12, 12);
            this.updateBut.Name = "updateBut";
            this.updateBut.Size = new System.Drawing.Size(268, 52);
            this.updateBut.TabIndex = 0;
            this.updateBut.Text = "Update Addons";
            this.updateBut.UseVisualStyleBackColor = true;
            this.updateBut.Click += new System.EventHandler(this.UpdateButClick);
            // 
            // listAddonsList
            // 
            this.listAddonsList.BackColor = System.Drawing.Color.White;
            this.listAddonsList.Location = new System.Drawing.Point(12, 85);
            this.listAddonsList.Name = "listAddonsList";
            this.listAddonsList.Size = new System.Drawing.Size(187, 181);
            this.listAddonsList.TabIndex = 3;
            this.listAddonsList.UseCompatibleStateImageBehavior = false;
            this.listAddonsList.View = System.Windows.Forms.View.List;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(205, 85);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 4;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButtonClick);
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(205, 114);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 5;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.RemoveButtonClick);
            // 
            // ManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(292, 278);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.listAddonsList);
            this.Controls.Add(this.updateBut);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Garry\'s mod Addon Manager";
            this.TransparencyKey = System.Drawing.Color.Maroon;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button updateBut;
        private System.Windows.Forms.ListView listAddonsList;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
    }
}

