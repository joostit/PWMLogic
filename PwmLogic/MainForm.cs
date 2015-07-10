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
using BrrrBayBay.LogicPWMLib;
using BrrrBayBay.PwmGUIControl;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using System.Xml;
using System.Diagnostics;


/*
 * 
 *  ToDo:
 *  -- Sort of "Do you want to apply first" before opening options screen
 *  -- validate PWM functionality on scope
 * 
 * 
 * 
 * */


namespace BrrrBayBay.PwmLogic
{
	public partial class MainForm : Form
	{
		/// <summary>
		/// Holds the current version of PwmLogic
		/// </summary>
		private static Version CURRENT_VERSION = new Version("1.0.1.3");

		// Several status message strings
		private String notConnectedString = "Not connected";
		private String connectedString = "Connected with Logic device";
		private String runningStatusString = "PWM Generator running...";
		private String notRunningStatusString = "Not running.";
		private String ApplicationTitle = "PWM Logic";

		private PwmControlMode lastMode = PwmControlMode.GenericAsync;

		private PwmControlMode controlMode = PwmControlMode.GenericAsync;

		private IPwmGeneratorUserControl mainControl = null;

		private PwmSettingsTemplate currentSettings = null;

		private String fileToLoad = null;

		private int jitterCount = 0;

		/// <summary>
		/// Holds the Logic connector
		/// </summary>
		private volatile LogicPWMConnector connector;

		public MainForm(String[] args)
		{
			runInit();
			if (args.Length == 1)
			{
				fileToLoad = args[0];
			}
		}

		public MainForm()
		{
			runInit();
		}

		private void runInit()
		{
			InitializeComponent();

			this.runStateLabel.Text = notRunningStatusString;
			this.connectionStateLabel.Text = notConnectedString;
		}


		private void setJitterCount()
		{
			jitterLabel.Text = "Buffer underruns: " + jitterCount.ToString();
		}


		private void Form1_Load(object sender, EventArgs e)
		{
			jitterLabel.Visible = false;
			jitterCount = 0;
			setJitterCount();

			connector = new LogicPWMLib.LogicPWMConnector();
			connector.OnLogicConnect += new EventHandler(connector_OnLogicConnect);
			connector.OnLogicDisconnect += new EventHandler(connector_OnLogicDisconnect);
			connector.RunningStateChanged += new EventHandler(connector_RunningStateChanged);
			connector.OnError += new EventHandler(connector_OnError);

			this.Text = ApplicationTitle;

			if (fileToLoad != null)
			{
				try
				{
					loadFile(fileToLoad);
				}
				catch (Exception exc)
				{
					MessageBox.Show("Could not open file.\n" + exc.Message, "File open error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}


			startActiveMode();

			//loadControlComponent();
		}

		void connector_OnError(object sender, EventArgs e)
		{
			try
			{
				this.Invoke(new MethodInvoker(delegate()
				{
					jitterCount++;
					setJitterCount();
				}));
			}
			catch
			{

			}
		}



		void connector_RunningStateChanged(object sender, EventArgs e)
		{
			try
			{
				this.Invoke((MethodInvoker)delegate
				{
					updateRunningState();
				});
			}
			catch
			{

			}
		}


		private void startActiveMode()
		{
			if (Properties.Settings.Default.UseHighPriority)
			{
				Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
			}
			else
			{
				Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
			}

			startPwmButton.Enabled = false;
			stopPwmButton.Enabled = false;

			loadControlComponent();
		}


		private void endActiveMode()
		{
			startPwmButton.Enabled = false;
			stopPwmButton.Enabled = false;
			
			if (connector.Running)
			{
				connector.Stop();
			}

			mainControl.deInitialize();
			mainControlPanel.Controls.Clear();
			
			mainControl.Dispose();
			mainControl = null;

			this.Text = ApplicationTitle;

			jitterLabel.Visible = false;
			jitterCount = 0;

			GC.Collect();		// Just to make sure
		}


		private void loadControlComponent()
		{
			connector.SampleRate = Properties.Settings.Default.SampleRate;
			controlMode = Properties.Settings.Default.GenerationMode;

			switch (controlMode)
			{
				case PwmControlMode.GenericAsync:
					mainControl = new GenericPwmControl( PwmGeneratorModes.Asynchronous);
					this.Text = ApplicationTitle + "  [ASynchronous mode]";
					break;
				case PwmControlMode.GenericSync:
					mainControl = new GenericPwmControl( PwmGeneratorModes.Synchronous);
					this.Text = ApplicationTitle + "  [Synchronous mode]";
					break;
				case PwmControlMode.RCControl:
					mainControl = new RcPwmControl();
					this.Text = ApplicationTitle + "  [R/C control mode]";
					break;
				default:
					MessageBox.Show("This mode is not supported yet");
					endActiveMode();
					return;
			}


			mainControl.ShowRealValueToolTips = Properties.Settings.Default.ShowRealValueToolTips;
			mainControl.ColoredBorders = Properties.Settings.Default.ColoredChannelBorders;
			mainControl.initialize(connector);
			mainControlPanel.Controls.Add((Control)mainControl);
			
			this.Size = new System.Drawing.Size(((Control)mainControl).Size.Width + 10, ((Control)mainControl).Size.Height + 100);
			
			startPwmButton.Enabled = connector.Connected;
			

			// If there are settings to be loaded
			if (currentSettings != null)
			{
				if (Properties.Settings.Default.GenerationMode == currentSettings.ControlMode)
				{
					mainControl.loadSettings(currentSettings.ControllerSettings);
				}
				else
				{
					currentSettings = null;
				}
			}

		}


		void connector_OnLogicDisconnect(object sender, EventArgs e)
		{
			this.Invoke((MethodInvoker)delegate
			{
				updateConnectionState();
				
			});
		}


		void connector_OnLogicConnect(object sender, EventArgs e)
		{
			this.Invoke((MethodInvoker)delegate
			{
				updateConnectionState();
			});
		}

		/// <summary>
		/// Updates the Form with the current running state
		/// </summary>
		private void updateRunningState()
		{
			if (connector.Running)
			{
				startPwmButton.Enabled = false;
				stopPwmButton.Enabled = true;
				this.runStateLabel.Text = runningStatusString;

				jitterLabel.Visible = true;
				jitterCount = 0;
				setJitterCount();
			}
			else
			{
				if (mainControl != null)
				{
					startPwmButton.Enabled = true;
					stopPwmButton.Enabled = false;
				}
				else
				{
					startPwmButton.Enabled = false;
					stopPwmButton.Enabled = false;
				}
				this.runStateLabel.Text = notRunningStatusString;
				jitterLabel.Visible = false;
				jitterCount = 0;
			}
		}

		/// <summary>
		/// Updates the Form with the current connected state
		/// </summary>
		private void updateConnectionState()
		{
			if (connector.Connected)
			{
				if (mainControl != null)
				{
					startPwmButton.Enabled = true;
					stopPwmButton.Enabled = false;
				}
				else
				{
					startPwmButton.Enabled = false;
					stopPwmButton.Enabled = false;
				}
				this.connectionStateLabel.Text = connectedString;
			}
			else
			{
				startPwmButton.Enabled = false;
				stopPwmButton.Enabled = false;
				this.connectionStateLabel.Text = notConnectedString;
			}
		}

		private void startPwmButton_Click(object sender, EventArgs e)
		{
			connector.applyChannelSettings();
			connector.Start();
		}

		private void stopPwmButton_Click(object sender, EventArgs e)
		{
			connector.Stop();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutForm frm = new AboutForm();
			frm.Owner = this;
			frm.appVersion = CURRENT_VERSION;
			frm.ShowDialog();
			frm.Dispose();
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			connector.Stop();
			currentSettings = saveSettingsToObject();
			lastMode = currentSettings.ControlMode;
			endActiveMode();

			OptionsForm frm = new OptionsForm();
			//frm.Owner = this;
			frm.ShowDialog(this);
			frm.Dispose();

			// If the generationmode was changed, don't use the old settings
			if (lastMode != Properties.Settings.Default.GenerationMode)
			{
				connector.resetChannelSettings();
			}

			if (currentSettings != null)
			{
				currentSettings.sampleRate = Properties.Settings.Default.SampleRate;
			}

			startActiveMode();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			connector.Disconnect();
			endActiveMode();
			connector.Dispose();
			connector = null;
			Properties.Settings.Default.Save();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog diag = new SaveFileDialog();
			diag.FileName = "PWM_Presets.pwm";
			diag.Filter = "PWM preset files (*.pwm)|*.pwm|All files (*.*)|*.*";
			diag.OverwritePrompt = true;
			DialogResult choice = diag.ShowDialog();
			if (choice == System.Windows.Forms.DialogResult.OK)
			{
				PwmSettingsTemplate template = saveSettingsToObject();
				TextWriter textWriter = new StreamWriter(diag.FileName);
				XmlSerializer serializer = new XmlSerializer(typeof(PwmSettingsTemplate));
				serializer.Serialize(textWriter, template);
				textWriter.Close();
			}


		}


		private PwmSettingsTemplate saveSettingsToObject()
		{
			XmlElement sets = mainControl.saveSettings();
			PwmSettingsTemplate template = new PwmSettingsTemplate();
			template.ControlMode = Properties.Settings.Default.GenerationMode;
			template.sampleRate = Properties.Settings.Default.SampleRate;
			template.ControllerSettings = sets;

			return template;
		}
		

		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{

			OpenFileDialog diag = new OpenFileDialog();
			try
			{			
				diag.Multiselect = false;
				diag.CheckFileExists = true;
				diag.Filter = "PWM preset files (*.pwm)|*.pwm|All files (*.*)|*.*";
				DialogResult choice = diag.ShowDialog();
				
				endActiveMode();
				if (choice == System.Windows.Forms.DialogResult.OK)
				{
					
					loadFile(diag.FileName);
				}

				//diag.Dispose();
				startActiveMode();
			}
			catch (Exception exc)
			{
				MessageBox.Show(this, "Incompatible PWM settings file.\n\nError:\n" + exc.Message, "Could not load file", MessageBoxButtons.OK, MessageBoxIcon.Error);
				endActiveMode();
				currentSettings = null;
				startActiveMode();
			}
			finally
			{
				diag.Dispose();
			}

		}


		private void loadFile(String path)
		{

			XmlSerializer serializer = new XmlSerializer(typeof(PwmSettingsTemplate));
			TextReader textreader = new StreamReader(path);
			PwmSettingsTemplate loadedSettings = (PwmSettingsTemplate)serializer.Deserialize(textreader);

			Properties.Settings.Default.SampleRate = loadedSettings.sampleRate;
			Properties.Settings.Default.GenerationMode = loadedSettings.ControlMode;
			currentSettings = loadedSettings;
			textreader.Close();
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			endActiveMode();
			currentSettings = null;
			startActiveMode();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void clearSettingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			endActiveMode();
			currentSettings = null;
			connector.resetChannelSettings();
			startActiveMode();
		}


	

	}
}
