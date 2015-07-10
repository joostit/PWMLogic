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
namespace BrrrBayBay.PwmGUIControl
{
	partial class PwmChannelControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.channelNumberLabel = new System.Windows.Forms.Label();
			this.frequencyBox = new System.Windows.Forms.TextBox();
			this.enabledBox = new System.Windows.Forms.CheckBox();
			this.fLabel = new System.Windows.Forms.Label();
			this.dcLabel = new System.Windows.Forms.Label();
			this.dutyCycleBox = new System.Windows.Forms.TextBox();
			this.dutyCycleBar = new System.Windows.Forms.TrackBar();
			this.dcUnitsBox = new System.Windows.Forms.ComboBox();
			this.fUnitsBox = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.dutyCycleBar)).BeginInit();
			this.SuspendLayout();
			// 
			// channelNumberLabel
			// 
			this.channelNumberLabel.AutoSize = true;
			this.channelNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.channelNumberLabel.Location = new System.Drawing.Point(3, 9);
			this.channelNumberLabel.Name = "channelNumberLabel";
			this.channelNumberLabel.Size = new System.Drawing.Size(16, 13);
			this.channelNumberLabel.TabIndex = 0;
			this.channelNumberLabel.Text = "N";
			// 
			// frequencyBox
			// 
			this.frequencyBox.Location = new System.Drawing.Point(73, 5);
			this.frequencyBox.Name = "frequencyBox";
			this.frequencyBox.Size = new System.Drawing.Size(61, 20);
			this.frequencyBox.TabIndex = 2;
			this.frequencyBox.TextChanged += new System.EventHandler(this.frequencyBox_TextChanged);
			// 
			// enabledBox
			// 
			this.enabledBox.AutoSize = true;
			this.enabledBox.Location = new System.Drawing.Point(24, 9);
			this.enabledBox.Name = "enabledBox";
			this.enabledBox.Size = new System.Drawing.Size(15, 14);
			this.enabledBox.TabIndex = 1;
			this.enabledBox.UseVisualStyleBackColor = true;
			this.enabledBox.CheckedChanged += new System.EventHandler(this.enabledBox_CheckedChanged);
			// 
			// fLabel
			// 
			this.fLabel.AutoSize = true;
			this.fLabel.Location = new System.Drawing.Point(54, 9);
			this.fLabel.Name = "fLabel";
			this.fLabel.Size = new System.Drawing.Size(13, 13);
			this.fLabel.TabIndex = 3;
			this.fLabel.Text = "f:";
			// 
			// dcLabel
			// 
			this.dcLabel.AutoSize = true;
			this.dcLabel.Location = new System.Drawing.Point(214, 9);
			this.dcLabel.Name = "dcLabel";
			this.dcLabel.Size = new System.Drawing.Size(22, 13);
			this.dcLabel.TabIndex = 5;
			this.dcLabel.Text = "dc:";
			// 
			// dutyCycleBox
			// 
			this.dutyCycleBox.Location = new System.Drawing.Point(242, 5);
			this.dutyCycleBox.Name = "dutyCycleBox";
			this.dutyCycleBox.Size = new System.Drawing.Size(61, 20);
			this.dutyCycleBox.TabIndex = 4;
			this.dutyCycleBox.TextChanged += new System.EventHandler(this.dutyCycleBox_TextChanged);
			// 
			// dutyCycleBar
			// 
			this.dutyCycleBar.AutoSize = false;
			this.dutyCycleBar.LargeChange = 50;
			this.dutyCycleBar.Location = new System.Drawing.Point(351, 5);
			this.dutyCycleBar.Maximum = 100;
			this.dutyCycleBar.Name = "dutyCycleBar";
			this.dutyCycleBar.Size = new System.Drawing.Size(130, 20);
			this.dutyCycleBar.TabIndex = 6;
			this.dutyCycleBar.TickFrequency = 10;
			this.dutyCycleBar.TickStyle = System.Windows.Forms.TickStyle.None;
			this.dutyCycleBar.Value = 50;
			this.dutyCycleBar.Scroll += new System.EventHandler(this.dutyCycleBar_Scroll);
			this.dutyCycleBar.ValueChanged += new System.EventHandler(this.dutyCycleBar_ValueChanged);
			// 
			// dcUnitsBox
			// 
			this.dcUnitsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.dcUnitsBox.FormattingEnabled = true;
			this.dcUnitsBox.Items.AddRange(new object[] {
            "%",
            "ms",
            "us"});
			this.dcUnitsBox.Location = new System.Drawing.Point(309, 5);
			this.dcUnitsBox.Name = "dcUnitsBox";
			this.dcUnitsBox.Size = new System.Drawing.Size(36, 21);
			this.dcUnitsBox.TabIndex = 5;
			this.dcUnitsBox.TextChanged += new System.EventHandler(this.dutyCycleBox_TextChanged);
			// 
			// fUnitsBox
			// 
			this.fUnitsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.fUnitsBox.FormattingEnabled = true;
			this.fUnitsBox.Items.AddRange(new object[] {
            "Hz",
            "KHz"});
			this.fUnitsBox.Location = new System.Drawing.Point(140, 5);
			this.fUnitsBox.Name = "fUnitsBox";
			this.fUnitsBox.Size = new System.Drawing.Size(43, 21);
			this.fUnitsBox.TabIndex = 3;
			this.fUnitsBox.TextChanged += new System.EventHandler(this.frequencyBox_TextChanged);
			// 
			// PwmChannelControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.fUnitsBox);
			this.Controls.Add(this.dcUnitsBox);
			this.Controls.Add(this.dutyCycleBar);
			this.Controls.Add(this.dutyCycleBox);
			this.Controls.Add(this.dcLabel);
			this.Controls.Add(this.fLabel);
			this.Controls.Add(this.enabledBox);
			this.Controls.Add(this.frequencyBox);
			this.Controls.Add(this.channelNumberLabel);
			this.Name = "PwmChannelControl";
			this.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);
			this.Size = new System.Drawing.Size(484, 31);
			this.Load += new System.EventHandler(this.AsyncPwmChannelControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.dutyCycleBar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label channelNumberLabel;
		private System.Windows.Forms.TextBox frequencyBox;
		private System.Windows.Forms.CheckBox enabledBox;
		private System.Windows.Forms.Label fLabel;
		private System.Windows.Forms.Label dcLabel;
		private System.Windows.Forms.TextBox dutyCycleBox;
		private System.Windows.Forms.TrackBar dutyCycleBar;
		private System.Windows.Forms.ComboBox dcUnitsBox;
		private System.Windows.Forms.ComboBox fUnitsBox;
	}
}
