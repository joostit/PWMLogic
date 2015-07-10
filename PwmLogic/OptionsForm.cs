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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrrrBayBay.PwmLogic
{
	public partial class OptionsForm : Form
	{

		private ToolTip mainTooltip;

		public OptionsForm()
		{
			InitializeComponent();

			mainTooltip = new System.Windows.Forms.ToolTip();
			mainTooltip.SetToolTip(checkForUpdatesBox, "When checked, the application connects to http://brrrbaybay.com/ at startup to see if updates are available.");
			mainTooltip.SetToolTip(highPriorityBox, "When checked, PWM Logic runs at higher system priority. This might improve performance.");
			mainTooltip.SetToolTip(coloredBordersBox, "When checked, the channel borders are painted in the colors used by the Logic.");
			mainTooltip.SetToolTip(realValueBox, "When checked, the real values for frequency and duty cycle are calculated, taking the sample frequency in account.\nThese values are shown as tooltips above the Frequency and Duty Cycle boxes.");

			mainTooltip.SetToolTip(sampleRateBox, "Higher sample rates generally allow higher frequencies and accuracy,\nbut also cause higher CPU usage.");
			mainTooltip.SetToolTip(AsynchronousButton, "Asynchronous mode allows a different frequency for each channel. The channels are not synchronized in any way.\nNote that this mode might cause high CPU usage.");
			mainTooltip.SetToolTip(synchronousButton, "Synchronous mode sets all channels to share the same frequency.\nAll channels are synchronized on the rising edge.");
			mainTooltip.SetToolTip(rcModeButton, "R/C mode provides an easy way to control R/C servo's and motors.\nThe frequency is fixed at 50Hz with duty cycles between 1 and 2 milliseconds.");

		}

		private void OptionsForm_Load(object sender, EventArgs e)
		{
			
				fillBoxes();
			
		}


		private void fillBoxes()
		{
			checkForUpdatesBox.Checked = Properties.Settings.Default.AutoUpdateCheck;
			highPriorityBox.Checked = Properties.Settings.Default.UseHighPriority;
			coloredBordersBox.Checked = Properties.Settings.Default.ColoredChannelBorders;
			realValueBox.Checked = Properties.Settings.Default.ShowRealValueToolTips;

			switch (Properties.Settings.Default.GenerationMode)
			{
				case PwmControlMode.GenericAsync:
					AsynchronousButton.Checked = true;
					break;
				case PwmControlMode.RCControl:
					rcModeButton.Checked = true;
					break;
				case PwmControlMode.GenericSync:
					synchronousButton.Checked = true;
					break;

			}


			fillSampleRateBoxValues();

		}


		private void fillSampleRateBoxValues()
		{

			sampleRateBox.Items.Clear();

			sampleRateBox.Items.Add("25 KHz");
			sampleRateBox.Items.Add("50 KHz");
			sampleRateBox.Items.Add("100 KHz");
			sampleRateBox.Items.Add("200 KHz");
			sampleRateBox.Items.Add("250 KHz");
			sampleRateBox.Items.Add("500 KHz");
			sampleRateBox.Items.Add("1 MHz");
			sampleRateBox.Items.Add("2 MHz");
			sampleRateBox.Items.Add("4 MHz");

			switch (Properties.Settings.Default.SampleRate)
			{
				case BrrrBayBay.LogicPWMLib.LogicSampleRate.f25k:
					sampleRateBox.Text = "25 KHz";
					break;
				case BrrrBayBay.LogicPWMLib.LogicSampleRate.f50k:
					sampleRateBox.Text = "50 KHz";
					break;
				case BrrrBayBay.LogicPWMLib.LogicSampleRate.f100k:
					sampleRateBox.Text = "100 KHz";
					break;
				case BrrrBayBay.LogicPWMLib.LogicSampleRate.f200k:
					sampleRateBox.Text = "200 KHz";
					break;
				case BrrrBayBay.LogicPWMLib.LogicSampleRate.f250k:
					sampleRateBox.Text = "250 KHz";
					break;
				case BrrrBayBay.LogicPWMLib.LogicSampleRate.f500k:
					sampleRateBox.Text = "500 KHz";
					break;
				case BrrrBayBay.LogicPWMLib.LogicSampleRate.f1M:
					sampleRateBox.Text = "1 MHz";
					break;
				case BrrrBayBay.LogicPWMLib.LogicSampleRate.f2M:
					sampleRateBox.Text = "2 MHz";
					break;
				case BrrrBayBay.LogicPWMLib.LogicSampleRate.f4M:
					sampleRateBox.Text = "4 MHz";
					break;
				default:
					sampleRateBox.Text = "200 KHz";
					break;
			}
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			saveSettings();
			this.Close();
		}


		private void saveSettings()
		{
			Properties.Settings.Default.AutoUpdateCheck = checkForUpdatesBox.Checked;
			Properties.Settings.Default.UseHighPriority = highPriorityBox.Checked;
			Properties.Settings.Default.ColoredChannelBorders = coloredBordersBox.Checked;
			Properties.Settings.Default.ShowRealValueToolTips = realValueBox.Checked;

			// Generator mode selection:
			if (AsynchronousButton.Checked) Properties.Settings.Default.GenerationMode = PwmControlMode.GenericAsync;
			if (synchronousButton.Checked) Properties.Settings.Default.GenerationMode = PwmControlMode.GenericSync;
			if (rcModeButton.Checked) Properties.Settings.Default.GenerationMode = PwmControlMode.RCControl;

			switch (sampleRateBox.Text)
			{
				case "25 KHz":
					Properties.Settings.Default.SampleRate = LogicPWMLib.LogicSampleRate.f25k;
					break;
				case "50 KHz":
					Properties.Settings.Default.SampleRate = LogicPWMLib.LogicSampleRate.f50k;
					break;
				case "100 KHz":
					Properties.Settings.Default.SampleRate = LogicPWMLib.LogicSampleRate.f100k;
					break;
				case "200 KHz":
					Properties.Settings.Default.SampleRate = LogicPWMLib.LogicSampleRate.f200k;
					break;
				case "250 KHz":
					Properties.Settings.Default.SampleRate = LogicPWMLib.LogicSampleRate.f250k;
					break;
				case "500 KHz":
					Properties.Settings.Default.SampleRate = LogicPWMLib.LogicSampleRate.f500k;
					break;
				case "1 MHz":
					Properties.Settings.Default.SampleRate = LogicPWMLib.LogicSampleRate.f1M;
					break;
				case "2 MHz":
					Properties.Settings.Default.SampleRate = LogicPWMLib.LogicSampleRate.f2M;
					break;
				case "4 MHz":
					Properties.Settings.Default.SampleRate = LogicPWMLib.LogicSampleRate.f4M;
					break;
				default:
					MessageBox.Show("Unknown samplerate. Defaulting to 250 KHz.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Properties.Settings.Default.SampleRate = LogicPWMLib.LogicSampleRate.f250k;
					break;
			}



		}

	

	}
}
