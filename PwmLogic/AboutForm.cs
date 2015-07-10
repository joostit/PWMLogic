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
	public partial class AboutForm : Form
	{
		/// <summary>
		/// Gets or sets the application version
		/// </summary>
		public Version appVersion { get; set; }

		public AboutForm()
		{
			appVersion = new Version("0.0.0.0");
			InitializeComponent();
		}

		private void AboutForm_Load(object sender, EventArgs e)
		{
			versionLabel.Text = appVersion.ToString();
		}
	}
}
