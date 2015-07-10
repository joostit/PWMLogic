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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrrrBayBay.LogicPWMLib;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace BrrrBayBay.PwmGUIControl
{
	public partial class PwmChannelControl : LogicChannelControl
	{

		public Boolean ShowRealValueToolTips { get; set; }

		/// <summary>
		/// Gets or sets the channel settings for this control
		/// </summary>
		public PwmChannelSettings channelSettings { get; set; }

		private double tmpFrequency = 0;

		private double tmpDc = 0;

		private Boolean tmpEnabled = true;

		public int sampleRateHz { get; set; }

		private String dcUnits = "%";

		ToolTip mainTooltip;

		public LogicPWMConnector Connector { get; set; }

		private Boolean enableFrequencyEdit = false;

		public Boolean EnableFrequencyEdit
		{
			get { return enableFrequencyEdit; }
			set
			{
				enableFrequencyEdit = value;
				fUnitsBox.Enabled = value;
				frequencyBox.Enabled = value;
			}
		}
		
		
		/// <summary>
		/// Gets or sets the Duty cycle units (us / ms / % )
		/// </summary>
		public String DcUnits
		{
			get
			{
				return dcUnits;
			}
			set
			{
				dcUnitsBox.Text = value;
				dcUnits = value;
			}
		}


		private String frequencyUnits = "Hz";

		/// <summary>
		/// Gets or sets the frequency units (Hz / KHz)
		/// </summary>
		public String FrequencyUnits {
			get
			{
				return frequencyUnits;
			}
			set
			{
				fUnitsBox.Text = value;
				frequencyUnits = value;
			}
		}

		

		/// <summary>
		/// Is true when a duty cycle change is caused programmatically. When true, the change is caused by the user
		/// </summary>
		private volatile Boolean sliderProgrammaticallyChanged = false;

		


		public PwmChannelControl()
		{
			ShowRealValueToolTips = false;
			base.ChannelNumberChanged += new EventHandler(AsyncPwmChannelControl_ChannelNumberChanged);
			sampleRateHz = 0;
			ChannelNumber = 0;
			InitializeComponent();
			mainTooltip = new ToolTip();

			mainTooltip.SetToolTip(dcLabel, "Duty cycle");
			mainTooltip.SetToolTip(fLabel, "Frequency");
			mainTooltip.SetToolTip(enabledBox, "Enables or disables the channel");
			mainTooltip.SetToolTip(dutyCycleBar, "Relative duty cycle: " + dutyCycleBar.Value.ToString() + "%");
		}

		void AsyncPwmChannelControl_ChannelNumberChanged(object sender, EventArgs e)
		{
			if (channelNumberLabel != null)
			{
				channelNumberLabel.Text = ChannelNumber.ToString();
			}
		}


		/// <summary>
		/// Fills in the frequency and frequencyUnit boxes
		/// </summary>
		/// <param name="frequency">The new frequency</param>
		/// <param name="fUnits">The new frequency units</param>
		public void setNewFrequency(int frequency, String fUnits)
		{
			fUnitsBox.Text = fUnits;
			if (fUnits == "KHz")
			{
				frequency = frequency / 1000;
			}
			frequencyBox.Text = frequency.ToString();
		}

		/// <summary>
		/// Applies the user settings to the channelsettings object. This is only performed if the user settings are correct
		/// </summary>
		/// <returns>true if successfull</returns>
		public Boolean applyValues()
		{
			Boolean retVal = false;

			frequencyUnits = fUnitsBox.Text;
			dcUnits = dcUnitsBox.Text;

			RaiseSettingsChangedAllowed = false;

			// Check if both the frequency and returnvalues are correct
			retVal = recalculateFrequency();
			if (retVal) retVal = recalculateDutyCycle();

			if (retVal == true)
			{
				channelSettings.DutyCycleS = tmpDc;
				channelSettings.Enabled = tmpEnabled;
				channelSettings.Frequency = (int)tmpFrequency;
			}

			RaiseSettingsChangedAllowed = true;
			return retVal;
		}


		/// <summary>
		/// Sets the tmpFrequency value and raises the SettingsChangedEvent
		/// </summary>
		/// <param name="tmpFrequency"></param>
		private void setTmpFrequency(double tmpFrequency)
		{
			
			if (RaiseSettingsChangedAllowed)
			{
				this.tmpFrequency = tmpFrequency;
				raiseSettingsChangedEvent();
			}
		}


		private void AsyncPwmChannelControl_Load(object sender, EventArgs e)
		{
			DcUnits = "%";
			FrequencyUnits = "Hz";
			reloadChannelSettings();

			foreach (Control ct in this.Controls)
			{
				ct.KeyDown += new KeyEventHandler(ct_KeyDown);
				ct.KeyPress += new KeyPressEventHandler(ct_KeyPress);
			}
		}

		void ct_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char) Keys.Return)
			{
				e.Handled = true;
			}
		}

		void ct_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				OnKeyDown(new KeyEventArgs(Keys.Return));
			}
		}


		public void reloadChannelSettings()
		{
			double dcValue = 0;
			RaiseSettingsChangedAllowed = false;

			tmpDc = channelSettings.DutyCycleS;
			tmpFrequency = channelSettings.Frequency;
			tmpEnabled = channelSettings.Enabled;

			enabledBox.Checked = channelSettings.Enabled;
			switch (frequencyUnits)
			{
				case "Hz":
					frequencyBox.Text = channelSettings.Frequency.ToString();
					break;
				case "KHz":
					frequencyBox.Text = (channelSettings.Frequency / 1000).ToString();
					break;
			}

			switch (dcUnits)
			{
				case "%":
					dcValue = (tmpDc / (1f / tmpFrequency)) * 100;
					break;
				case "ms":
					dcValue = tmpDc * 1000;
					break;
				case "us":
					dcValue = tmpDc * 1000000;
					break;
			}
			
			setDcBoxVal(dcValue);
			RaiseSettingsChangedAllowed = true;
		}


		private void frequencyBox_TextChanged(object sender, EventArgs e)
		{
			recalculateFrequency();
			recalculateDutyCycle();

			if (ShowRealValueToolTips)
			{
				calculateRealValues();
			}

		}

		private void calculateRealValues()
		{
			double realF = Connector.calculateRealFrequency((int)tmpFrequency);
			double realFval = 0;
			String fUnitString = "";

			switch (fUnitsBox.Text)
			{
				case "Hz":
					realFval = realF;
					fUnitString = " Hz";
					break;
				case "KHz":
					realFval = realF / 1000;
				fUnitString = " KHz";
					break;
				default:	// This happens when the control isn't initialized yet
					return;
			}

			realFval = Math.Round(realFval, 3);

			mainTooltip.SetToolTip(frequencyBox, "Real frequency: " + realFval.ToString() + fUnitString);

			// Calculate Duty cycle
			double realDc = Connector.calculateRealDutyCycle(tmpDc);
			double realDcVal = 0;
			String dcUnitString = "";
			switch (dcUnitsBox.Text)
			{
				case "%":
					realDcVal = (realDc / (1 / realF)) * 100;
					dcUnitString = " %";
					break;

				case "us":
					realDcVal = realDc * 1000000;
					dcUnitString = "  us";
					break;

				case "ms":
					realDcVal = realDc * 1000;
					dcUnitString = " ms";
					break;
				default:	// This happens when the control isn't initialized yet
					return;

			}
			realDcVal = Math.Round(realDcVal, 3);
			mainTooltip.SetToolTip(dutyCycleBox, "Real duty cycle: " + realDcVal.ToString() + dcUnitString);
		}

		/// <summary>
		/// Recalculates the frequency value and returns true if it is correct
		/// </summary>
		/// <returns></returns>
		private Boolean recalculateFrequency()
		{
			Boolean retVal = false;
			int? rawVal = Utils.getIntFromString(frequencyBox.Text);
			if (rawVal != null)
			{
				if (rawVal.Value > 0)
				{
					frequencyBox.BackColor = Color.Empty;
					switch (fUnitsBox.Text)
					{
						case "Hz":
							setTmpFrequency(rawVal.Value);
							retVal = true;
							break;
						case "KHz":
							setTmpFrequency(rawVal.Value * 1000);
							retVal = true;
							break;
						default:
							throw new Exception("Unknown unit selected for frequency");
					}
				}
				else
				{
					frequencyBox.BackColor = Color.Salmon;
				}
			}
			else
			{
				frequencyBox.BackColor = Color.Salmon;
			}
			return retVal;
		}

		/// <summary>
		/// Updates the slider position with the current duty cycle value
		/// </summary>
		private void updateSliderPosition()
		{
			try
			{
				sliderProgrammaticallyChanged = true;
				double period = 1 / tmpFrequency;
				double percentage = (tmpDc / period) * 100;
				dutyCycleBar.Value = (int)Math.Round(percentage);
			}
			finally
			{
				sliderProgrammaticallyChanged = false;
			}
		}

		/// <summary>
		/// Recalculates and checks the dutycycle value
		/// </summary>
		private Boolean recalculateDutyCycle()
		{
			Boolean retVal = false;
			double? rawVal = Utils.getDoubleFromString(dutyCycleBox.Text);
			double maxDc = 0;
			double tmpVal = 0 ;

			if (rawVal != null)
			{
				if (rawVal.Value >= 0)
				{
					

					switch (dcUnitsBox.Text)
					{
						case "%":
							if (rawVal.Value <= 100)
							{
								tmpVal = ((1f / tmpFrequency) / 100f) * rawVal.Value;
								dutyCycleBox.BackColor = Color.Empty;
								retVal = true;
							}
							else
							{
								dutyCycleBox.BackColor = Color.Salmon;
							}
							break;

						case "us":
							tmpVal = rawVal.Value / 1000000;
							dutyCycleBox.BackColor = Color.Empty;
							break;

						case "ms":
							tmpVal = rawVal.Value / 1000;
							dutyCycleBox.BackColor = Color.Empty;
							
							break;
						default:
							throw new Exception("Unknown unit selected for dutycycle");
					}


					//Check if the dutycycle doesn't exeed the frequency period time
					maxDc = 1f / tmpFrequency;
					if (tmpVal > maxDc)
					{
						dutyCycleBox.BackColor = Color.Salmon;
						retVal = false;
					}
					else
					{
						retVal = true;
						setTmpDc(tmpVal);
						updateSliderPosition();
					}

				}
				else
				{
					dutyCycleBox.BackColor = Color.Salmon;
				}
			}
			else
			{
				dutyCycleBox.BackColor = Color.Salmon;
			}

			return retVal;
		}


		/// <summary>
		/// Sets the tmpDc value and raises the settingschanged event
		/// </summary>
		/// <param name="value"></param>
		private void setTmpDc(double value)
		{
			if (RaiseSettingsChangedAllowed)
			{
				tmpDc = value;
				raiseSettingsChangedEvent();
			}
		}


		private void dutyCycleBox_TextChanged(object sender, EventArgs e)
		{

			recalculateFrequency();
			recalculateDutyCycle();
			calculateRealValues();
		}

		private void setDcBoxVal(double value)
		{
			dutyCycleBox.Text = Math.Round(value, 4).ToString();
		}

		private void dutyCycleBar_Scroll(object sender, EventArgs e)
		{
			if (!sliderProgrammaticallyChanged)
			{
				int newDc = dutyCycleBar.Value;
				double newDcS = ((1f / tmpFrequency) / 100f) * newDc;

				switch (dcUnitsBox.Text)
				{
					case "%":
						setDcBoxVal( newDc);
						break;
					case "us":
						setDcBoxVal(newDcS * 1000000);
						break;
					case "ms":
						setDcBoxVal(newDcS * 1000);

						break;
					default:
						throw new Exception("Unknown unit selected for dutycycle");
				}


			}
		}

		private void enabledBox_CheckedChanged(object sender, EventArgs e)
		{
			if (RaiseSettingsChangedAllowed)
			{
				tmpEnabled = enabledBox.Checked;
				raiseSettingsChangedEvent();
			}
		}


		public new void Dispose()
		{
			channelSettings = null;
			base.Dispose();
		}

		private void dutyCycleBar_ValueChanged(object sender, EventArgs e)
		{
			mainTooltip.SetToolTip(dutyCycleBar, "Relative duty cycle: " + dutyCycleBar.Value.ToString() + "%");
		}

	}
}
