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
using SaleaeDeviceSdkDotNet;
using System.Threading;

namespace BrrrBayBay.LogicPWMLib
{
	public class LogicPWMConnector : IDisposable
	{

		/// <summary>
		/// If true, an error has occured and the connector is repairing the connection.
		/// </summary>
		private volatile Boolean repairing = false;

		/// <summary>
		/// Gets a value that indicates if the PWM generator is running
		/// </summary>
		public Boolean Running
		{
			get
			{
				return running;
			}
		}

		/// <summary>
		/// Gets a value that indicates if the Logic device is connected (True if connected)
		/// </summary>
		public Boolean Connected
		{
			get
			{
				return (logicDevice != null);
			}
		}

		private ulong devId = 0;

		private LogicSampleRate sampleRate = LogicSampleRate.f4M;

		/// <summary>
		/// Gets or sets the sample rate. Thios should only be applied when the PWM generator isn't running
		/// </summary>
		public LogicSampleRate SampleRate
		{
			get
			{
				return sampleRate;
			}
			set
			{
				sampleRate = value;
				pwmGenerator.sampleRate = value;
				if (logicDevice != null)
				{
					logicDevice.SampleRateHz = (uint)value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the PWM frequency when synchronous mode is used
		/// </summary>
		public int SynchronousFrequency
		{
			get
			{
				return pwmGenerator.SynchronousPwmFrequency;
			}
			set
			{
				pwmGenerator.SynchronousPwmFrequency = value;
			}
		}

		/// <summary>
		/// Gets the array which holds the channel settings ( 0 to 7 )
		/// </summary>
		public PwmChannelSettings[] channelSettings
		{
			get
			{
				return pwmGenerator.ChannelSettings;
			}
		}


		/// <summary>
		/// gets or sets the PWM generator mode
		/// </summary>
		public PwmGeneratorModes GeneratorMode
		{
			get
			{
				return pwmGenerator.GeneratorMode;
			}
			set
			{
				pwmGenerator.GeneratorMode = value;
			}
		}

		/// <summary>
		/// Gets raised when the Running state changes
		/// </summary>
		public event EventHandler RunningStateChanged;

		/// <summary>
		/// Gets raised when a Logic device is connected
		/// </summary>
		public event EventHandler OnLogicConnect;

		/// <summary>
		/// Gets raised when the nlogic device disconnects
		/// </summary>
		public event EventHandler OnLogicDisconnect;

		/// <summary>
		/// Gets raised when an error occurs
		/// </summary>
		public event EventHandler OnError;

		private LogicPwmDataGenerator pwmGenerator;

		private MLogic logicDevice = null;

		private MSaleaeDevices deviceConnector;

		private Boolean running = false;

		/// <summary>
		/// Constructor
		/// </summary>
		public LogicPWMConnector()
		{
			deviceConnector = new MSaleaeDevices();
			deviceConnector.OnLogicConnect += new MSaleaeDevices.OnLogicConnectDelegate(devices_OnConnect);
			deviceConnector.OnDisconnect += new MSaleaeDevices.OnDisconnectDelegate(devices_OnDisconnect);
			pwmGenerator = new LogicPwmDataGenerator();
			pwmGenerator.sampleRate = sampleRate;
			deviceConnector.BeginConnect();
		}


		/// <summary>
		/// Calculates the real frequency when using the current sample rate
		/// </summary>
		/// <param name="desiredFrequency">The desired frequency in Hz</param>
		/// <returns>The real frequency in Hz</returns>
		public double calculateRealFrequency(int desiredFrequency)
		{
			return pwmGenerator.calculateRealFrequency(desiredFrequency);
		}

		/// <summary>
		/// Calculates the real duty cycle when using the current sample rate
		/// </summary>
		/// <param name="desiredFrequency">The desired duty cycle in seconds</param>
		/// <returns>The real duty cycle in seconds</returns>
		public double calculateRealDutyCycle(double desiredDutyCycle)
		{
			return pwmGenerator.calculateRealDutyCycle(desiredDutyCycle);
		}


		/// <summary>
		/// Applies the channel settings for a single channel in asynchronous mode
		/// </summary>
		/// <param name="channel">The channel number</param>
		public void applyChannelSettings(int channel)
		{
			pwmGenerator.applyChannelSettings(channel);
		}

		/// <summary>
		/// Applies all channel settings
		/// </summary>
		public void applyChannelSettings()
		{
			pwmGenerator.applyChannelSettings();
		}


		public void Disconnect()
		{
			if (logicDevice != null)
			{
				logicDevice.Stop();
				logicDevice = null;
			}

			raiseRunningStateChanged();
		}

		/// <summary>
		/// Starts the PWM generator
		/// </summary>
		public void Start()
		{
			logicDevice.WriteStart();
			running = true;
			raiseRunningStateChanged();
		}

		/// <summary>
		/// Stops the PWM generator.
		/// </summary>
		public void Stop()
		{
			try
			{
				running = false;
				Thread.Sleep(50);	
				logicDevice.Stop();
				Thread.Sleep(200);				// Allow the generator the complete its running tasks
				logicDevice.SetOutput(0x00);	// Set all outputs to Low
				running = false;
				raiseRunningStateChanged();
			}
			catch { }
		}

		/// <summary>
		/// Raises the RunningStateChanged event
		/// </summary>
		private void raiseRunningStateChanged()
		{
			if (RunningStateChanged != null)
			{
				RunningStateChanged(this, new EventArgs());
			}
		}



		private void devices_OnDisconnect(ulong device_id)
		{
			running = false;
			logicDevice = null;
			device_id = 0;
			if ((OnLogicDisconnect != null)  && !repairing)
			{
				OnLogicDisconnect.Invoke(this, new EventArgs());
			}
		}



		private void devices_OnConnect(ulong device_id, MLogic logic)
		{
			
			running = false;
			if (logic != null)
			{
				logicDevice = logic;
				logicDevice.SampleRateHz = (uint)(sampleRate);
				this.devId = device_id;

				
				logicDevice.SetOutput(0x00);	
				logicDevice.OnError += new MLogic.OnErrorDelegate(logicDevice_OnError);
				logicDevice.OnReadData += new MLogic.OnReadDataDelegate(logicDevice_OnReadData);
				logicDevice.OnWriteData += new MLogic.OnWriteDataDelegate(logicDevice_OnWriteData);

				if (repairing)
				{
					Start();
					repairing = false;
				}
				else
				{
					if (OnLogicConnect != null)
					{
						OnLogicConnect.Invoke(this, new EventArgs());
					}
				}
			}
		}

		/// <summary>
		/// Callback when the Logic device requests data to be written
		/// </summary>
		/// <param name="device_id">The requesting device ID</param>
		/// <param name="data">The array that needs to be filled</param>
		private void logicDevice_OnWriteData(ulong device_id, byte[] data)
		{
			pwmGenerator.fillBufferWithPwmStream(data);
		}

		/// <summary>
		/// Callback when the Logic device has data to be read
		/// </summary>
		/// <remarks>This callback isn't used</remarks>
		/// <param name="device_id">The device ID from which data is available</param>
		/// <param name="data">The array which contains the data</param>
		private void logicDevice_OnReadData(ulong device_id, byte[] data)
		{
			// This doesn't do anything since we're only writing to the device
		}

		/// <summary>
		/// Callback for when an error occured
		/// </summary>
		/// <param name="device_id">The device ID which created the error</param>
		private void logicDevice_OnError(ulong device_id)
		{
			// When we're running, restart operations
			if (running)
			{
				// ToDo: Restarting does not work. SDK Bug?!?
				logicDevice.Stop();
				repairing = true;
				ThreadPool.QueueUserWorkItem(new WaitCallback(raiseOnError));
			}
		}


		/// <summary>
		/// Raises the OnError event. This method is called on a threadpool thread
		/// </summary>
		/// <param name="state">N/A</param>
		private void raiseOnError(Object state)
		{
			logicDevice.WriteStart();

			if (OnError != null)
			{
				OnError(this, new EventArgs());
			}
		}


		public void Dispose()
		{
			this.Disconnect();
			Thread.Sleep(100);	// Allow the PWM generator to fully stop
			pwmGenerator.Dispose();
	
			pwmGenerator = null;			
			logicDevice = null;
			deviceConnector = null;
			running = false;

			GC.Collect();
		}

		public void resetChannelSettings()
		{
			pwmGenerator.resetChannelSettings();
		}
	}
}
