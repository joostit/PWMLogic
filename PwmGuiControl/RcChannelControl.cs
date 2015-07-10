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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrrrBayBay.PwmGUIControl
{
	public class RcChannelControl : LogicChannelControl
	{

		private double calibration_center = 0.0015F;

		public double Calibration_center
		{
			get
			{
				return calibration_center;
			}
			set
			{
				calibration_center = value;
				updateCalibration(centerBox.Visible);
			}
		}

		private double calibration_maxDeviation = 0.0005F;

		public double Calibration_maxDeviation
		{
			get
			{
				return calibration_maxDeviation;
			}
			set
			{
				calibration_maxDeviation = value;
				updateCalibration(deviationBox.Visible);
			}
		}

		private System.Windows.Forms.CheckBox enabledBox;
		private System.Windows.Forms.TrackBar dutyCycleBar;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label valueLabel;
		private System.Windows.Forms.Label channelNumberLabel;

		private volatile Boolean channelEnabled = false;
		private Label calibrationLabel;
		private TextBox centerBox;
		private TextBox deviationBox;
		private Label centerTextLabel;
		private Label centerMsLabel;
		private Label deviationMsLabel;
		private Label deviationTextLabel;

		private volatile int rcValue = 0;

		private ToolTip mainTooltip;

		/// <summary>
		/// Gets or sets the value indicated by the track bar
		/// </summary>
		public int Value
		{
			get
			{
				return rcValue;
			}
			set
			{
				rcValue = value;
				if (this.InvokeRequired)
				{
					this.Invoke(new MethodInvoker(delegate()
				{
					this.dutyCycleBar.Value = rcValue;
				}));

				}
				else
				{
					this.dutyCycleBar.Value = rcValue;
				}
			}
		}
	


		/// <summary>
		/// Gets or sets if this channel is enabled
		/// </summary>
		public Boolean ChannelEnabled
		{
			get
			{
				return channelEnabled;
			}
			set
			{
				channelEnabled = value;
				if (this.InvokeRequired)
				{
					this.Invoke(new MethodInvoker(delegate()
					{
						enabledBox.Checked = channelEnabled;
					}));

				}
				else
				{
					enabledBox.Checked = channelEnabled;
				}
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public RcChannelControl()
		{
			InitializeComponent();
			updateCalibration(false);

			mainTooltip = new ToolTip();
			mainTooltip.SetToolTip(calibrationLabel, "Calibration settings.\nDouble-click to edit");
			mainTooltip.SetToolTip(centerBox, "Calibration: Center PWM duty cycle.\nPress [Enter] to save the new values. Press [Escape] to cancel]");
			mainTooltip.SetToolTip(deviationBox, "Calibration: Maximum PWM duty cycle deviation.\nPress [Enter] to save the new values. Press [Escape] to cancel]");
		}

		private void InitializeComponent()
		{
			this.enabledBox = new System.Windows.Forms.CheckBox();
			this.channelNumberLabel = new System.Windows.Forms.Label();
			this.dutyCycleBar = new System.Windows.Forms.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.valueLabel = new System.Windows.Forms.Label();
			this.calibrationLabel = new System.Windows.Forms.Label();
			this.centerBox = new System.Windows.Forms.TextBox();
			this.deviationBox = new System.Windows.Forms.TextBox();
			this.centerTextLabel = new System.Windows.Forms.Label();
			this.centerMsLabel = new System.Windows.Forms.Label();
			this.deviationMsLabel = new System.Windows.Forms.Label();
			this.deviationTextLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dutyCycleBar)).BeginInit();
			this.SuspendLayout();
			// 
			// enabledBox
			// 
			this.enabledBox.AutoSize = true;
			this.enabledBox.Location = new System.Drawing.Point(24, 19);
			this.enabledBox.Name = "enabledBox";
			this.enabledBox.Size = new System.Drawing.Size(15, 14);
			this.enabledBox.TabIndex = 3;
			this.enabledBox.UseVisualStyleBackColor = true;
			this.enabledBox.CheckedChanged += new System.EventHandler(this.enabledBox_CheckedChanged);
			// 
			// channelNumberLabel
			// 
			this.channelNumberLabel.AutoSize = true;
			this.channelNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.channelNumberLabel.Location = new System.Drawing.Point(3, 19);
			this.channelNumberLabel.Name = "channelNumberLabel";
			this.channelNumberLabel.Size = new System.Drawing.Size(16, 13);
			this.channelNumberLabel.TabIndex = 2;
			this.channelNumberLabel.Text = "N";
			// 
			// dutyCycleBar
			// 
			this.dutyCycleBar.AutoSize = false;
			this.dutyCycleBar.LargeChange = 20;
			this.dutyCycleBar.Location = new System.Drawing.Point(45, 5);
			this.dutyCycleBar.Maximum = 100;
			this.dutyCycleBar.Minimum = -100;
			this.dutyCycleBar.Name = "dutyCycleBar";
			this.dutyCycleBar.Size = new System.Drawing.Size(378, 38);
			this.dutyCycleBar.TabIndex = 7;
			this.dutyCycleBar.TickFrequency = 25;
			this.dutyCycleBar.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.dutyCycleBar.ValueChanged += new System.EventHandler(this.dutyCycleBar_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(228, 42);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(13, 13);
			this.label1.TabIndex = 8;
			this.label1.Text = "0";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(42, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "- 100 %";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(398, 42);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(36, 13);
			this.label3.TabIndex = 10;
			this.label3.Text = "100 %";
			// 
			// valueLabel
			// 
			this.valueLabel.AutoSize = true;
			this.valueLabel.Location = new System.Drawing.Point(429, 19);
			this.valueLabel.Name = "valueLabel";
			this.valueLabel.Size = new System.Drawing.Size(13, 13);
			this.valueLabel.TabIndex = 11;
			this.valueLabel.Text = "0";
			// 
			// calibrationLabel
			// 
			this.calibrationLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.calibrationLabel.ForeColor = System.Drawing.Color.Gray;
			this.calibrationLabel.Location = new System.Drawing.Point(497, 14);
			this.calibrationLabel.Name = "calibrationLabel";
			this.calibrationLabel.Size = new System.Drawing.Size(98, 34);
			this.calibrationLabel.TabIndex = 13;
			this.calibrationLabel.Text = "1.42 ms \r\n+/- 0.563 ms";
			this.calibrationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.calibrationLabel.DoubleClick += new System.EventHandler(this.calibrationLabel_DoubleClick);
			// 
			// centerBox
			// 
			this.centerBox.Location = new System.Drawing.Point(526, 9);
			this.centerBox.Name = "centerBox";
			this.centerBox.Size = new System.Drawing.Size(50, 20);
			this.centerBox.TabIndex = 14;
			this.centerBox.Text = "1,123";
			this.centerBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.calBox_KeyPress);
			// 
			// deviationBox
			// 
			this.deviationBox.Location = new System.Drawing.Point(526, 35);
			this.deviationBox.Name = "deviationBox";
			this.deviationBox.Size = new System.Drawing.Size(50, 20);
			this.deviationBox.TabIndex = 15;
			this.deviationBox.Text = "1,123";
			this.deviationBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.calBox_KeyPress);
			// 
			// centerTextLabel
			// 
			this.centerTextLabel.AutoSize = true;
			this.centerTextLabel.Location = new System.Drawing.Point(479, 12);
			this.centerTextLabel.Name = "centerTextLabel";
			this.centerTextLabel.Size = new System.Drawing.Size(41, 13);
			this.centerTextLabel.TabIndex = 16;
			this.centerTextLabel.Text = "Center:";
			// 
			// centerMsLabel
			// 
			this.centerMsLabel.AutoSize = true;
			this.centerMsLabel.Location = new System.Drawing.Point(582, 12);
			this.centerMsLabel.Name = "centerMsLabel";
			this.centerMsLabel.Size = new System.Drawing.Size(23, 13);
			this.centerMsLabel.TabIndex = 17;
			this.centerMsLabel.Text = "ms.";
			// 
			// deviationMsLabel
			// 
			this.deviationMsLabel.AutoSize = true;
			this.deviationMsLabel.Location = new System.Drawing.Point(582, 38);
			this.deviationMsLabel.Name = "deviationMsLabel";
			this.deviationMsLabel.Size = new System.Drawing.Size(23, 13);
			this.deviationMsLabel.TabIndex = 18;
			this.deviationMsLabel.Text = "ms.";
			// 
			// deviationTextLabel
			// 
			this.deviationTextLabel.AutoSize = true;
			this.deviationTextLabel.Location = new System.Drawing.Point(441, 38);
			this.deviationTextLabel.Name = "deviationTextLabel";
			this.deviationTextLabel.Size = new System.Drawing.Size(79, 13);
			this.deviationTextLabel.TabIndex = 19;
			this.deviationTextLabel.Text = "Max. deviation:";
			// 
			// RcChannelControl
			// 
			this.Controls.Add(this.deviationTextLabel);
			this.Controls.Add(this.deviationMsLabel);
			this.Controls.Add(this.centerMsLabel);
			this.Controls.Add(this.centerTextLabel);
			this.Controls.Add(this.deviationBox);
			this.Controls.Add(this.centerBox);
			this.Controls.Add(this.calibrationLabel);
			this.Controls.Add(this.valueLabel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dutyCycleBar);
			this.Controls.Add(this.enabledBox);
			this.Controls.Add(this.channelNumberLabel);
			this.Name = "RcChannelControl";
			this.Size = new System.Drawing.Size(616, 61);
			this.ChannelNumberChanged += new System.EventHandler(this.RcChannelControl_ChannelNumberChanged);
			this.Load += new System.EventHandler(this.RcChannelControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.dutyCycleBar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		void RcChannelControl_ChannelNumberChanged(object sender, EventArgs e)
		{
			channelNumberLabel.Text = this.ChannelNumber.ToString();
		}

		private void dutyCycleBar_ValueChanged(object sender, EventArgs e)
		{
			rcValue = dutyCycleBar.Value;
			valueLabel.Text = rcValue.ToString();
			raiseSettingsChangedEvent();
		}

		private void RcChannelControl_Load(object sender, EventArgs e)
		{
			RaiseSettingsChangedAllowed = true;
		}

		private void enabledBox_CheckedChanged(object sender, EventArgs e)
		{
			channelEnabled = enabledBox.Checked;
			raiseSettingsChangedEvent();
		}

		private void calibrationLabel_DoubleClick(object sender, EventArgs e)
		{
			updateCalibration(true);
		}

		/// <summary>
		/// Enables calibration value editing
		/// </summary>
		/// <param name="enableEdit">True if editing should be enabled, else false</param>
		private void updateCalibration(Boolean enableEdit)
		{
			centerBox.Visible = enableEdit;
			centerMsLabel.Visible = enableEdit;
			centerTextLabel.Visible = enableEdit;

			deviationBox.Visible = enableEdit;
			deviationMsLabel.Visible = enableEdit;
			deviationTextLabel.Visible = enableEdit;

			double centerVal = Math.Round(calibration_center * 1000, 3);
			double devVal =  Math.Round(calibration_maxDeviation * 1000, 3);

			calibrationLabel.Text = centerVal.ToString() + " ms\n+/- " + devVal.ToString() + " ms";

			centerBox.Text = centerVal.ToString();
			deviationBox.Text = devVal.ToString();

			calibrationLabel.Visible = !enableEdit;

			if (enableEdit)
			{
				centerBox.Select();
			}

		}

		private void calBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				try
				{
					double centerVal = Convert.ToDouble(centerBox.Text);
					double deviationVal = Convert.ToDouble(deviationBox.Text);

					// Convert to seconds
					centerVal = centerVal / 1000;
					deviationVal = deviationVal / 1000;

					if ((centerVal <= 0) || (deviationVal <= 0))
					{
						MessageBox.Show("Please enter valid values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						if (centerVal - deviationVal < 0)
						{
							MessageBox.Show("Invalid Center / Deviation combination.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						else
						{
							Calibration_center = centerVal;
							calibration_maxDeviation = deviationVal;
							updateCalibration(false);
							raiseSettingsChangedEvent();
						}
					}

				}
				catch
				{
					MessageBox.Show("Please fill in correct values for the center duty cycle and maximum deviation.", "Input error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				e.Handled = true;
			}

			if (e.KeyChar == (char)Keys.Escape)
			{
				e.Handled = true;
				updateCalibration(false);
			}
		}
	}
}
