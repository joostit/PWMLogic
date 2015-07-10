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
using BrrrBayBay.LogicPWMLib;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;

namespace BrrrBayBay.PwmGUIControl
{
	/// <summary>
	/// The control allowing the user to change asynchronous PWM settings
	/// </summary>
	public class GenericPwmControl : UserControl, IPwmGeneratorUserControl
	{
		/// <summary>
		/// Holds theh synchronous PWM frequency
		/// </summary>
		private int synchronousPwmFrequency = 1000;

		/// <summary>
		/// Holds a value for each channel which indicates if it has been changed
		/// </summary>
		private Dictionary<int, Boolean> changedStates;

		private LogicPWMConnector connector;

		private Dictionary<int, PwmChannelControl> channelControls;

		private Button applyButton;

		public Boolean ColoredBorders { get; set; }

		public Boolean ShowRealValueToolTips { get; set; }

		TextBox frequencyBox;

		Label frequencyLabel;

		ComboBox fUnitsBox;

		/// <summary>
		/// Gets or sets the control mode for the usercontrol
		/// </summary>
		public PwmGeneratorModes ControlMode { get; private set; }


		public GenericPwmControl()
		{
			ShowRealValueToolTips = false;
			ControlMode = PwmGeneratorModes.Asynchronous;
			InitializeComponent();
		}


		public GenericPwmControl(PwmGeneratorModes controlMode)
		{
			this.ControlMode = controlMode;
			InitializeComponent();
		}


		public void deInitialize()
		{
			
		}


		public void initialize(LogicPWMConnector connector)
		{
			this.connector = connector;
		}



		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// AsynchronousPwmControl
			// 
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Name = "AsynchronousPwmControl";
			this.Size = new System.Drawing.Size(227, 102);
			this.Load += new System.EventHandler(this.AsynchronousPwmControl_Load);
			this.ResumeLayout(false);

		}

		private void AsynchronousPwmControl_Load(object sender, EventArgs e)
		{
			int vPos = 2;
			PwmChannelControl ctl = null;
			changedStates = new Dictionary<int, bool>();

			connector.GeneratorMode = ControlMode;


			channelControls = new Dictionary<int, PwmChannelControl>();
			for (int i = 0; i < 8; i++)
			{
				changedStates.Add(i, false);
				ctl = new PwmChannelControl();
				channelControls.Add(i, ctl);
				ctl.channelSettings = connector.channelSettings[i];
				ctl.ChannelNumber = i;
				ctl.sampleRateHz = (int) connector.SampleRate;
				ctl.Location = new System.Drawing.Point(2, vPos);
				ctl.KeyDown += new KeyEventHandler(ctl_KeyDown);
				ctl.ShowRealValueToolTips = ShowRealValueToolTips;
				ctl.Connector = connector;

				ctl.ColoredBorders = ColoredBorders;
				ctl.BorderColor = GuiStandards.ChannelColors[i];
				ctl.EnableFrequencyEdit = (ControlMode == PwmGeneratorModes.Asynchronous);

				this.Controls.Add(ctl);
				vPos += ctl.Size.Height + 3;
				ctl.SettingsChanged += new EventHandler(ctl_SettingsChanged);
			}

			applyButton = new Button();
			applyButton.Text = "Apply";
			applyButton.Click += new EventHandler(applyButton_Click);
			applyButton.Enabled = false;
			applyButton.Size = new System.Drawing.Size(60, 29);
			System.Drawing.Font buttonFont = applyButton.Font;
			applyButton.Font = new System.Drawing.Font(buttonFont.FontFamily, buttonFont.Size + 0.3F, System.Drawing.FontStyle.Bold);

			applyButton.Location = new System.Drawing.Point(ctl.Size.Width - applyButton.Size.Width, vPos);
			this.Controls.Add(applyButton);

			if (ControlMode == PwmGeneratorModes.Synchronous)
			{
				initFrequencyBox(4, vPos + 5);
			}

			vPos += applyButton.Size.Height + 3;

			this.Size = new System.Drawing.Size(ctl.Size.Width + 6, vPos + 2);

			applyChanges(true);
		}


		/// <summary>
		/// Loads the frequency edit controls
		/// </summary>
		/// <param name="xPos">The X position where the controls should be placed</param>
		/// <param name="yPos">The Y position where the controls should be placed</param>
		private void initFrequencyBox(int xPos, int yPos)
		{

			frequencyLabel = new Label();
			frequencyLabel.Font = new System.Drawing.Font(frequencyLabel.Font.FontFamily, frequencyLabel.Font.Size + 0.4f, System.Drawing.FontStyle.Bold);
			frequencyLabel.Text = "frequency:";
			frequencyLabel.Size = new System.Drawing.Size(75, 16);
			frequencyLabel.Location = new Point(5 , yPos + 1);
			frequencyLabel.Update();
			frequencyLabel.AutoSize = false;
						
			frequencyBox = new TextBox();
			frequencyBox.Size = new Size(60, 20);
			frequencyBox.Location = new Point(frequencyLabel.Location.X + frequencyLabel.Size.Width + 3, yPos);
			frequencyBox.Text = synchronousPwmFrequency.ToString();
			frequencyBox.TextChanged += new EventHandler(frequencyChangedHandler);
			frequencyBox.KeyDown += new KeyEventHandler(frequencySettings_KeyDown);
			

			fUnitsBox = new ComboBox();
			fUnitsBox.Size = new Size(45, 21);
			fUnitsBox.Location = new Point(frequencyBox.Location.X + frequencyBox.Size.Width + 3, yPos);
			fUnitsBox.DropDownStyle = ComboBoxStyle.DropDownList;
			fUnitsBox.Items.Add("Hz");
			fUnitsBox.Items.Add("KHz");
			fUnitsBox.Text = "Hz";
			fUnitsBox.SelectedIndexChanged += new EventHandler(frequencyChangedHandler);
			fUnitsBox.KeyDown += new KeyEventHandler(frequencySettings_KeyDown);


			this.Controls.Add(frequencyLabel);
			this.Controls.Add(frequencyBox);
			this.Controls.Add(fUnitsBox);
		}

		void frequencySettings_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
				applyButton_Click(this, new EventArgs());
			}
		}


		private void frequencyChangedHandler(Object sender, EventArgs e)
		{
			float multiplier = 1;
			double freq = 0;
			double? fVal;
			// get the frequency multiplier
			switch (fUnitsBox.Text)
			{
				case "Hz":
					multiplier = 1;
					break;
				case "KHz":
					multiplier = 1000;
					break;
				default:
					MessageBox.Show("Unknown frequency units: " + fUnitsBox.Text);
					return;
			}
			
			// Check if the frequency is a valid number
			fVal = Utils.getDoubleFromString(frequencyBox.Text);
			if (fVal == null)
			{
				frequencyBox.BackColor = Color.Salmon;
				return;
			}
			else
			{
				frequencyBox.BackColor = Color.Empty;
			}

			// Check if the frequency is between 1Hz and 1 MHz
			freq = fVal.Value * multiplier;

			if ((freq < 1) || (freq > 1000000))
			{
				frequencyBox.BackColor = Color.Salmon;
				return;
			}
			else
			{
				frequencyBox.BackColor = Color.Empty;
				synchronousPwmFrequency = (int) freq;
				for (int i = 0; i < 8; i++)
				{
					channelControls[i].setNewFrequency((int) freq, fUnitsBox.Text);
				}
			}
		}


		void ctl_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				e.SuppressKeyPress = true;
				e.Handled = true;
				applyChanges(false);
			}
		}


		private void applyChanges(Boolean forceAll)
		{
			foreach (KeyValuePair<int, PwmChannelControl> kvp in channelControls)
			{
				if ((changedStates[kvp.Key] == true) || forceAll)
				{
					kvp.Value.applyValues();
					changedStates[kvp.Key] = false;
					if (ControlMode == PwmGeneratorModes.Asynchronous)
					{
						connector.applyChannelSettings(kvp.Key);
					}
				}
			}


			if (ControlMode == PwmGeneratorModes.Synchronous)
			{
				connector.SynchronousFrequency = synchronousPwmFrequency;
				connector.applyChannelSettings();
			}

			applyButton.Enabled = false;
		}

		


		void applyButton_Click(object sender, EventArgs e)
		{
			applyChanges(false);
		}


		void ctl_SettingsChanged(object sender, EventArgs e)
		{
			PwmChannelControl apcc = (PwmChannelControl) sender;
			changedStates[apcc.ChannelNumber] = true;
			applyButton.Enabled = true;
		}

		public new void Dispose()
		{
			changedStates.Clear();
			this.Controls.Clear();
			foreach (PwmChannelControl cc in channelControls.Values)
			{
				cc.Dispose();
			}
			channelControls.Clear();
			connector = null;

			base.Dispose();
		}


		public void loadSettings(System.Xml.XmlElement pwmSettings)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(SettingsTemplate));

			StringReader stringReader;
			stringReader = new StringReader(pwmSettings.OuterXml);
			XmlTextReader xmlReader = new XmlTextReader(stringReader);
			SettingsTemplate sets = (SettingsTemplate) serializer.Deserialize(xmlReader);

			for (int i = 0; i < 8; i++)
			{
				channelControls[i].FrequencyUnits = sets.ChannelSettings[i].FrequencyUnits;
				channelControls[i].DcUnits = sets.ChannelSettings[i].DutyCycleUnits;

				channelControls[i].channelSettings.DutyCycleS = sets.ChannelSettings[i].DutyCycle;
				channelControls[i].channelSettings.Enabled = sets.ChannelSettings[i].Enabled;
				channelControls[i].channelSettings.Frequency = sets.ChannelSettings[i].Frequency;

				channelControls[i].reloadChannelSettings();
			}

			if (ControlMode == PwmGeneratorModes.Synchronous)
			{
				
				fUnitsBox.Text = sets.SynchronousFrequencyUnits;
				synchronousPwmFrequency = sets.SynchronousFrequency;
				setFrequencyBoxValue(synchronousPwmFrequency);

				connector.SynchronousFrequency = synchronousPwmFrequency;

				for (int i = 0; i < 8; i++)
				{
					channelControls[i].setNewFrequency(synchronousPwmFrequency, sets.SynchronousFrequencyUnits);
				}
			}

			applyChanges(true);
		}


		private void setFrequencyBoxValue(int freq)
		{
			if (fUnitsBox.Text == "KHz")
			{
				frequencyBox.Text = (synchronousPwmFrequency / 1000).ToString();
			}
			else
			{
				frequencyBox.Text = synchronousPwmFrequency.ToString();
			}
		}

		public System.Xml.XmlElement saveSettings()
		{
			SettingsTemplate sets = createSettingsObject();

			XmlSerializer serializer = new XmlSerializer(typeof(SettingsTemplate));
			StringWriter sw = new StringWriter();
			serializer.Serialize(sw, sets);
			XmlDocument xDoc = new XmlDocument();
			xDoc.LoadXml(sw.ToString());

			return xDoc.DocumentElement;
		}

		private SettingsTemplate createSettingsObject()
		{
			SettingsTemplate sets = new SettingsTemplate();
			ChannelSettingsTemplate chan;
			for (int i = 0; i < 8; i++)
			{
				chan = new ChannelSettingsTemplate();
				chan.DutyCycle = channelControls[i].channelSettings.DutyCycleS;
				chan.Enabled = channelControls[i].channelSettings.Enabled;
				chan.Frequency = channelControls[i].channelSettings.Frequency;
				chan.DutyCycleUnits = channelControls[i].DcUnits;
				chan.FrequencyUnits = channelControls[i].FrequencyUnits;

				sets.ChannelSettings.Add(chan);
			}

			if (ControlMode == PwmGeneratorModes.Synchronous)
			{
				sets.SynchronousFrequency = synchronousPwmFrequency;
				sets.SynchronousFrequencyUnits = channelControls[0].FrequencyUnits;
			}

			return sets;
		}


	}
}
