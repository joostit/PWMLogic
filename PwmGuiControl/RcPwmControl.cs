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
using System.Threading;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace BrrrBayBay.PwmGUIControl
{
	public partial class RcPwmControl : UserControl, IPwmGeneratorUserControl
	{

		

		private const int MIN_Frequency = 1;

		private const int MAX_Frequency = 400;

		private volatile Boolean settingsChanged = false;

		private const int applyInterval = 100;

		private LogicPWMConnector connector;

		private Boolean coloredBorders = false;

		private Label frequencyTextLabel;

		private Label frequencyLabel;

		private TextBox frequencyBox;

		private Label hzLabel;

		private Dictionary<int, RcChannelControl> channelControls;

		ToolTip mainTooltip;

		private int pwmFrequency = 50;

		/// <summary>
		/// gets or sets the value that indicates if the true frequency and duty cycle should be shown as tooltips
		/// </summary>
		public Boolean ShowRealValueToolTips { get; set; }


		/// <summary>
		/// Timer that applies the changes
		/// </summary>
		private System.Threading.Timer applyTimer;

		public bool ColoredBorders
		{
			get
			{
				return coloredBorders;
			}
			set
			{
				coloredBorders = value;
			}
		}


		public RcPwmControl()
		{
			InitializeComponent();
			mainTooltip = new ToolTip();

		}

		private void RcPwmControl_Load(object sender, EventArgs e)
		{
			setupGenerator();
			loadChannelControls();



			applyTimer = new System.Threading.Timer(applyTimerCallback, null, 500, applyInterval);
		}

		public void initialize(LogicPWMConnector connector)
		{
			this.connector = connector;
		}


		

		private void loadChannelControls()
		{
			int vPos = 2;

			channelControls = new Dictionary<int, RcChannelControl>();
			RcChannelControl ctl = null;

			for (int i = 0; i < 8; i++)
			{
				ctl = new RcChannelControl();
				ctl.ColoredBorders = coloredBorders;
				ctl.ChannelNumber = i;
				ctl.SettingsChanged += new EventHandler(ctl_SettingsChanged);
				ctl.Location = new System.Drawing.Point(2, vPos);
				ctl.BorderColor = GuiStandards.ChannelColors[i];

				channelControls.Add(i, ctl);

				this.Controls.Add(ctl);
				vPos += ctl.Size.Height + 3;
			}

			vPos = createFrequencyEditor(vPos);

			this.Size = new System.Drawing.Size(ctl.Size.Width + 6, vPos + 2);

		}


		private int createFrequencyEditor(int vPos)
		{
			int vertPos = vPos;

			frequencyTextLabel = new Label();
			frequencyTextLabel.Text = "Frequency:";
			frequencyTextLabel.Location = new Point(2, vPos + 3);
			frequencyTextLabel.Size = new Size(63, 13);
			this.Controls.Add(frequencyTextLabel);

			frequencyLabel = new Label();
			frequencyLabel.DoubleClick += new EventHandler(frequencyLabel_DoubleClick);
			frequencyLabel.Size = new Size(50, 20);
			frequencyLabel.Location = new Point(frequencyTextLabel.Location.X + frequencyTextLabel.Size.Width, vPos);
			frequencyLabel.Text = pwmFrequency.ToString();
			frequencyLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			frequencyLabel.AutoSize = false;
			frequencyLabel.TextAlign = ContentAlignment.MiddleLeft;
			mainTooltip.SetToolTip(frequencyLabel, "Double click to edit");
			frequencyLabel.Visible = true;
			this.Controls.Add(frequencyLabel);

			frequencyBox = new TextBox();
			frequencyBox.Size = new Size(50, 20);
			frequencyBox.Location = new Point(frequencyTextLabel.Location.X + frequencyTextLabel.Size.Width, vPos);
			frequencyBox.LostFocus += new EventHandler(frequencyBox_LostFocus);
			frequencyBox.KeyPress += new KeyPressEventHandler(frequencyBox_KeyPress);		
			mainTooltip.SetToolTip(frequencyBox, "Press [Enter] to apply.");
			frequencyBox.Visible = true;
			this.Controls.Add(frequencyBox);

			hzLabel = new Label();
			hzLabel.Text = "Hz";
			hzLabel.Size = new Size(20, 13);
			hzLabel.Location = new Point(frequencyBox.Location.X + frequencyBox.Size.Width + 2, vPos + 3);
			this.Controls.Add(hzLabel);

			vertPos += frequencyBox.Size.Height + 3;

			return vertPos;
		}

		void frequencyLabel_DoubleClick(object sender, EventArgs e)
		{
			frequencyLabel.Visible = false;
			frequencyBox.Text = pwmFrequency.ToString();
			frequencyBox.Visible = true;
			frequencyBox.Select();
		}

		void frequencyBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char) Keys.Return)
			{
				try
				{
					int val = Convert.ToInt32(frequencyBox.Text);
					if ((val < MIN_Frequency) || (val > MAX_Frequency))
					{
						MessageBox.Show("Please enter a frequency between " + MIN_Frequency.ToString() + " Hz and " + MAX_Frequency.ToString() + " Hz.", "Invalid PWM frequency", MessageBoxButtons.OK, MessageBoxIcon.Error);
						e.Handled = true;
						return;
					}

					pwmFrequency = val;
					settingsChanged = true;

					frequencyBox.Visible = false;
					frequencyLabel.Visible = true;
				}
				catch
				{
					MessageBox.Show("Invalid frequency value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				e.Handled = true;
			}

			if (e.KeyChar == (char)Keys.Escape)
			{
				frequencyBox.Visible = false;
				frequencyLabel.Text = pwmFrequency.ToString();
				frequencyLabel.Visible = true;
				e.Handled = true;
			}
		}

		void frequencyBox_LostFocus(object sender, EventArgs e)
		{
			frequencyBox.Visible = false;
			frequencyLabel.Text = pwmFrequency.ToString();
			frequencyLabel.Visible = true;
		}


		void ctl_SettingsChanged(object sender, EventArgs e)
		{
			settingsChanged = true;
		}


		private void setupGenerator()
		{
			connector.GeneratorMode = PwmGeneratorModes.Synchronous;
			connector.SynchronousFrequency = pwmFrequency;

			for (int i = 0; i < 8; i++)
			{
				connector.channelSettings[i].Enabled = false;
				connector.channelSettings[i].Frequency = pwmFrequency;	// Not nessecary, but for clarity reasons
				connector.channelSettings[i].DutyCycleS = 0.0015;
			}

			connector.applyChannelSettings();
		}


		private void applyTimerCallback(Object obj)
		{
			if (settingsChanged)
			{
				settingsChanged = false;
				applyNewSettings();
			}
		}

		private void applyNewSettings()
		{
			for (int i = 0; i < 8; i++)
			{
				RcChannelControl ctl = channelControls[i];
				connector.channelSettings[i].DutyCycleS = calculateDutyCycle(ctl.Value, ctl.Calibration_center,ctl.Calibration_maxDeviation);
				connector.channelSettings[i].Enabled = ctl.ChannelEnabled;
				connector.channelSettings[i].Frequency = pwmFrequency;
			}
			connector.SynchronousFrequency = pwmFrequency;
			connector.applyChannelSettings();
		}

		/// <summary>
		/// Calculates the duty cycle using the provided parameters
		/// </summary>
		/// <param name="rcValue">The R/C value between -100 and 100</param>
		/// <param name="center">The pwm center duty cycle</param>
		/// <param name="maxDev">The maximum deviation</param>
		/// <returns>The duty cycle</returns>
		private double calculateDutyCycle(int rcValue, double center, double maxDev)
		{
			double retVal = 0;
			double dev = (maxDev * rcValue) / 100;
			retVal = dev + center;	// New calculation
			return retVal;
		}

		public void deInitialize()
		{
			applyTimer.Change(Timeout.Infinite, Timeout.Infinite);
			applyTimer.Dispose();
		}



		public void loadSettings(System.Xml.XmlElement pwmSettings)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(RcSettingsTemplate));

			StringReader stringReader;
			stringReader = new StringReader(pwmSettings.OuterXml);
			XmlTextReader xmlReader = new XmlTextReader(stringReader);
			RcSettingsTemplate sets = (RcSettingsTemplate)serializer.Deserialize(xmlReader);

			for (int i = 0; i < 8; i++)
			{
				channelControls[i].Value = sets.ChannelSettings[i].Value;
				channelControls[i].ChannelEnabled = sets.ChannelSettings[i].ChannelEnabled;
				channelControls[i].Calibration_center = sets.ChannelSettings[i].CalibrationSettings.CenterValue;
				channelControls[i].Calibration_maxDeviation = sets.ChannelSettings[i].CalibrationSettings.MaxDeviation;
			}
			pwmFrequency = sets.PwmFrequency;

			frequencyLabel.Text = pwmFrequency.ToString();
			frequencyBox.Text = pwmFrequency.ToString();

			applyNewSettings();
		}


		public System.Xml.XmlElement saveSettings()
		{
			RcSettingsTemplate sets = createSettingsObject();

			XmlSerializer serializer = new XmlSerializer(typeof(RcSettingsTemplate));
			StringWriter sw = new StringWriter();
			serializer.Serialize(sw, sets);
			XmlDocument xDoc = new XmlDocument();
			xDoc.LoadXml(sw.ToString());

			return xDoc.DocumentElement;
		}

		private RcSettingsTemplate createSettingsObject()
		{
			RcSettingsTemplate sets = new RcSettingsTemplate();
			RcChannelSettingsTemplate chan;
			for (int i = 0; i < 8; i++)
			{
				chan = new RcChannelSettingsTemplate();
				chan.Value = channelControls[i].Value;
				chan.ChannelEnabled = channelControls[i].ChannelEnabled;
				chan.CalibrationSettings.CenterValue = channelControls[i].Calibration_center;
				chan.CalibrationSettings.MaxDeviation = channelControls[i].Calibration_maxDeviation;

				sets.ChannelSettings.Add(chan);
			}
			sets.PwmFrequency = pwmFrequency;
			
			return sets;
		}
	}
}
