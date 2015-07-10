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

namespace BrrrBayBay.PwmGUIControl
{
	/// <summary>
	/// Defines the interface for usercontrols that control the PWM generator
	/// </summary>
	public interface IPwmGeneratorUserControl : IDisposable
	{
		/// <summary>
		/// Gets or sets a value that indicates if the Channel's borders should be colorder according to the Logic's channel color scheme
		/// </summary>
		Boolean ColoredBorders { get; set; }

		/// <summary>
		/// gets or sets the value that indicates if the true frequency and duty cycle should be shown as tooltips
		/// </summary>
		Boolean ShowRealValueToolTips { get; set; }

		/// <summary>
		/// Initializes the control with the connector
		/// </summary>
		/// <param name="connector">The PWM connector</param>
		void initialize(LogicPWMConnector connector);

		/// <summary>
		/// De-initializes the control
		/// </summary>
		void deInitialize();

		/// <summary>
		/// Loads the control with the specified settings
		/// </summary>
		/// <param name="pwmSettings">PWM Settings element</param>
		void loadSettings(XmlElement pwmSettings);

		/// <summary>
		/// Saves the current settings to a XML Element
		/// </summary>
		/// <returns>The saved settings as an XML Element</returns>
		XmlElement saveSettings();
	}
}
