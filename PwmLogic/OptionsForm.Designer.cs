/*
 * Copyright 2012 Joost Haverkort (BrrrBayBay)
 * http://brrrbaybay.com
 * 
 * 
 * This file is part of PwmLogic.
 * 
 * PwmLogic is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 * PwmLogic is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even 
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along with PwmLogic. If not, see http://www.gnu.org/licenses/.
 * 
*/
namespace BrrrBayBay.PwmLogic
{
	partial class OptionsForm
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
			this.mainTabControl = new System.Windows.Forms.TabControl();
			this.generalTab = new System.Windows.Forms.TabPage();
			this.coloredBordersBox = new System.Windows.Forms.CheckBox();
			this.highPriorityBox = new System.Windows.Forms.CheckBox();
			this.checkForUpdatesBox = new System.Windows.Forms.CheckBox();
			this.generatorTab = new System.Windows.Forms.TabPage();
			this.label2 = new System.Windows.Forms.Label();
			this.generatorTypeGroup = new System.Windows.Forms.GroupBox();
			this.rcModeButton = new System.Windows.Forms.RadioButton();
			this.synchronousButton = new System.Windows.Forms.RadioButton();
			this.AsynchronousButton = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.sampleRateBox = new System.Windows.Forms.ComboBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.realValueBox = new System.Windows.Forms.CheckBox();
			this.mainTabControl.SuspendLayout();
			this.generalTab.SuspendLayout();
			this.generatorTab.SuspendLayout();
			this.generatorTypeGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTabControl
			// 
			this.mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.mainTabControl.Controls.Add(this.generalTab);
			this.mainTabControl.Controls.Add(this.generatorTab);
			this.mainTabControl.Location = new System.Drawing.Point(0, 1);
			this.mainTabControl.Name = "mainTabControl";
			this.mainTabControl.SelectedIndex = 0;
			this.mainTabControl.Size = new System.Drawing.Size(398, 217);
			this.mainTabControl.TabIndex = 0;
			// 
			// generalTab
			// 
			this.generalTab.Controls.Add(this.realValueBox);
			this.generalTab.Controls.Add(this.coloredBordersBox);
			this.generalTab.Controls.Add(this.highPriorityBox);
			this.generalTab.Controls.Add(this.checkForUpdatesBox);
			this.generalTab.Location = new System.Drawing.Point(4, 22);
			this.generalTab.Name = "generalTab";
			this.generalTab.Padding = new System.Windows.Forms.Padding(3);
			this.generalTab.Size = new System.Drawing.Size(390, 191);
			this.generalTab.TabIndex = 0;
			this.generalTab.Text = "General";
			this.generalTab.UseVisualStyleBackColor = true;
			// 
			// coloredBordersBox
			// 
			this.coloredBordersBox.AutoSize = true;
			this.coloredBordersBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.coloredBordersBox.Location = new System.Drawing.Point(63, 69);
			this.coloredBordersBox.Name = "coloredBordersBox";
			this.coloredBordersBox.Size = new System.Drawing.Size(128, 17);
			this.coloredBordersBox.TabIndex = 5;
			this.coloredBordersBox.Text = "Show channel colors:";
			this.coloredBordersBox.UseVisualStyleBackColor = true;
			// 
			// highPriorityBox
			// 
			this.highPriorityBox.AutoSize = true;
			this.highPriorityBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.highPriorityBox.Location = new System.Drawing.Point(35, 29);
			this.highPriorityBox.Name = "highPriorityBox";
			this.highPriorityBox.Size = new System.Drawing.Size(156, 17);
			this.highPriorityBox.TabIndex = 4;
			this.highPriorityBox.Text = "Use higher process priority: ";
			this.highPriorityBox.UseVisualStyleBackColor = true;
			// 
			// checkForUpdatesBox
			// 
			this.checkForUpdatesBox.AutoSize = true;
			this.checkForUpdatesBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkForUpdatesBox.Location = new System.Drawing.Point(25, 6);
			this.checkForUpdatesBox.Name = "checkForUpdatesBox";
			this.checkForUpdatesBox.Size = new System.Drawing.Size(166, 17);
			this.checkForUpdatesBox.TabIndex = 2;
			this.checkForUpdatesBox.Text = "Check for updates on startup:";
			this.checkForUpdatesBox.UseVisualStyleBackColor = true;
			// 
			// generatorTab
			// 
			this.generatorTab.Controls.Add(this.label2);
			this.generatorTab.Controls.Add(this.generatorTypeGroup);
			this.generatorTab.Controls.Add(this.label1);
			this.generatorTab.Controls.Add(this.sampleRateBox);
			this.generatorTab.Location = new System.Drawing.Point(4, 22);
			this.generatorTab.Name = "generatorTab";
			this.generatorTab.Padding = new System.Windows.Forms.Padding(3);
			this.generatorTab.Size = new System.Drawing.Size(390, 191);
			this.generatorTab.TabIndex = 1;
			this.generatorTab.Text = "Generator";
			this.generatorTab.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(31, 54);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Generator type:";
			// 
			// generatorTypeGroup
			// 
			this.generatorTypeGroup.Controls.Add(this.rcModeButton);
			this.generatorTypeGroup.Controls.Add(this.synchronousButton);
			this.generatorTypeGroup.Controls.Add(this.AsynchronousButton);
			this.generatorTypeGroup.Location = new System.Drawing.Point(117, 33);
			this.generatorTypeGroup.Name = "generatorTypeGroup";
			this.generatorTypeGroup.Size = new System.Drawing.Size(121, 98);
			this.generatorTypeGroup.TabIndex = 4;
			this.generatorTypeGroup.TabStop = false;
			// 
			// rcModeButton
			// 
			this.rcModeButton.AutoSize = true;
			this.rcModeButton.Location = new System.Drawing.Point(6, 65);
			this.rcModeButton.Name = "rcModeButton";
			this.rcModeButton.Size = new System.Drawing.Size(75, 17);
			this.rcModeButton.TabIndex = 2;
			this.rcModeButton.TabStop = true;
			this.rcModeButton.Text = "R/C Mode";
			this.rcModeButton.UseVisualStyleBackColor = true;
			// 
			// synchronousButton
			// 
			this.synchronousButton.AutoSize = true;
			this.synchronousButton.Location = new System.Drawing.Point(6, 42);
			this.synchronousButton.Name = "synchronousButton";
			this.synchronousButton.Size = new System.Drawing.Size(87, 17);
			this.synchronousButton.TabIndex = 1;
			this.synchronousButton.TabStop = true;
			this.synchronousButton.Text = "Synchronous";
			this.synchronousButton.UseVisualStyleBackColor = true;
			// 
			// AsynchronousButton
			// 
			this.AsynchronousButton.AutoSize = true;
			this.AsynchronousButton.Location = new System.Drawing.Point(6, 19);
			this.AsynchronousButton.Name = "AsynchronousButton";
			this.AsynchronousButton.Size = new System.Drawing.Size(92, 17);
			this.AsynchronousButton.TabIndex = 0;
			this.AsynchronousButton.TabStop = true;
			this.AsynchronousButton.Text = "Asynchronous";
			this.AsynchronousButton.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(45, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(66, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Sample rate:";
			// 
			// sampleRateBox
			// 
			this.sampleRateBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.sampleRateBox.FormattingEnabled = true;
			this.sampleRateBox.Location = new System.Drawing.Point(117, 6);
			this.sampleRateBox.Name = "sampleRateBox";
			this.sampleRateBox.Size = new System.Drawing.Size(121, 21);
			this.sampleRateBox.TabIndex = 2;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(231, 224);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(312, 224);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 2;
			this.okButton.Text = "&Ok";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// realValueBox
			// 
			this.realValueBox.AutoSize = true;
			this.realValueBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.realValueBox.Location = new System.Drawing.Point(50, 92);
			this.realValueBox.Name = "realValueBox";
			this.realValueBox.Size = new System.Drawing.Size(141, 17);
			this.realValueBox.TabIndex = 6;
			this.realValueBox.Text = "Show real value tooltips:";
			this.realValueBox.UseVisualStyleBackColor = true;
			// 
			// OptionsForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(399, 259);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.mainTabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			this.Load += new System.EventHandler(this.OptionsForm_Load);
			this.mainTabControl.ResumeLayout(false);
			this.generalTab.ResumeLayout(false);
			this.generalTab.PerformLayout();
			this.generatorTab.ResumeLayout(false);
			this.generatorTab.PerformLayout();
			this.generatorTypeGroup.ResumeLayout(false);
			this.generatorTypeGroup.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl mainTabControl;
		private System.Windows.Forms.TabPage generalTab;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.CheckBox highPriorityBox;
		private System.Windows.Forms.CheckBox checkForUpdatesBox;
		private System.Windows.Forms.TabPage generatorTab;
		private System.Windows.Forms.GroupBox generatorTypeGroup;
		private System.Windows.Forms.RadioButton rcModeButton;
		private System.Windows.Forms.RadioButton synchronousButton;
		private System.Windows.Forms.RadioButton AsynchronousButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox sampleRateBox;
		private System.Windows.Forms.CheckBox coloredBordersBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox realValueBox;
	}
}