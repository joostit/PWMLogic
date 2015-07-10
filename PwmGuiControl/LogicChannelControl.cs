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
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace BrrrBayBay.PwmGUIControl
{
	/// <summary>
	/// Base class for a PWM Channel Usercontrol
	/// </summary>
	public class LogicChannelControl : UserControl
	{
		// PInvoke declaration
		[DllImport("User32.dll", EntryPoint = "GetDCEx")]
		internal static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hRgn, int flags);

		private int channelNumber = 0;

		/// <summary>
		/// Gets or sets the channel number for this control
		/// </summary>
		public int ChannelNumber
		{
			get
			{
				return channelNumber;
			}
			set
			{
				channelNumber = value;
				if (ChannelNumberChanged != null)
				{
					ChannelNumberChanged.Invoke(this, new EventArgs());
				}
			}
		}


		/// <summary>
		/// Is set to true when the control should raise SettingsChanged events
		/// </summary>
		private Boolean raiseSettingsChangedAllowed = false;

		/// <summary>
		/// Gets or sets a value that indicates if the settingschangedevent may be raised
		/// </summary>
		protected Boolean RaiseSettingsChangedAllowed
		{
			get { return raiseSettingsChangedAllowed; }
			set { raiseSettingsChangedAllowed = value; }
		}


		private Color borderColor = Color.Black;

		public Color BorderColor
		{
			get
			{
				return borderColor;
			}
			set
			{
				borderColor = value;
				this.Refresh();
			}

		}

		
		private Boolean coloredBorders = true;

		/// <summary>
		/// Gets or sets the a value if colored borders should be used
		/// </summary>
		public Boolean ColoredBorders
		{
			get
			{
				return coloredBorders;
			}
			set
			{
				coloredBorders = value;
				if (coloredBorders == true)
				{
					this.BorderStyle = System.Windows.Forms.BorderStyle.None;
				}
				else
				{
					this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				}
				this.Refresh();
			}
		}

		/// <summary>
		/// gets raised if a setting has been changed by the user
		/// </summary>
		public event EventHandler SettingsChanged;

		/// <summary>
		/// Gets raised when the channel number has been changed
		/// </summary>
		public event EventHandler ChannelNumberChanged;

		public LogicChannelControl()
		{
			ColoredBorders = coloredBorders;
		}
		

		/// <summary>
		/// Raises the settingschangedevent
		/// </summary>
		protected void raiseSettingsChangedEvent()
		{
			if (raiseSettingsChangedAllowed)
			{
				if (SettingsChanged != null)
				{
					SettingsChanged.Invoke(this, new EventArgs());
				}
			}
		}

		

		// Event Override
		protected override void OnPaint(PaintEventArgs e)
		{
			if (this.coloredBorders)
			{

				e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

				IntPtr hWnd = this.Handle;
				IntPtr hRgn = IntPtr.Zero;
				IntPtr hdc = GetDCEx(hWnd, hRgn, 1027);

				using (Graphics grfx = Graphics.FromHdc(hdc))
				{
					Rectangle thicRect = new Rectangle(1, 1, this.Width - 3, this.Height - 3);
					Rectangle thinRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
					Pen thickPen = new Pen(borderColor, 2F);
					Pen thinPen = new Pen(Color.Black, 1F);
					//grfx.DrawRectangle(pen, rect)
					drawRoundRect(grfx, thickPen, thicRect, 3);
					drawRoundRect(grfx, thinPen, thinRect, 3);

				}
			}

			base.OnPaint(e);
		}



		/// <summary>
		/// Draws a rounded rectangle on the graphics object
		/// </summary>
		/// <param name="g">The graphics object</param>
		/// <param name="p">The pen</param>
		/// <param name="rect">The rectangle</param>
		/// <param name="radius">The corner radius</param>
		public void drawRoundRect(Graphics g, Pen p, Rectangle rect, float radius)
		{
			GraphicsPath gp = new GraphicsPath();
			float x = rect.X;
			float y = rect.Y;
			float width = rect.Width;
			float height = rect.Height;
			g.SmoothingMode = SmoothingMode.HighQuality;

			gp.AddLine(x + radius, y, x + width - (radius * 2), y); // Line
			gp.AddArc(x + width - (radius * 2), y, radius * 2, radius * 2, 270, 90); // Corner
			gp.AddLine(x + width, y + radius, x + width, y + height - (radius * 2)); // Line
			gp.AddArc(x + width - (radius * 2), y + height - (radius * 2), radius * 2, radius * 2, 0, 90); // Corner
			gp.AddLine(x + width - (radius * 2), y + height, x + radius, y + height); // Line
			gp.AddArc(x, y + height - (radius * 2), radius * 2, radius * 2, 90, 90); // Corner
			gp.AddLine(x, y + height - (radius * 2), x, y + radius); // Line
			gp.AddArc(x, y, radius * 2, radius * 2, 180, 90); // Corner
			gp.CloseFigure();

			g.DrawPath(p, gp);
			gp.Dispose();
		}
		


	}
}
