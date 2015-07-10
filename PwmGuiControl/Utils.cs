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

namespace BrrrBayBay.PwmGUIControl
{
	/// <summary>
	/// Contains all sorts of functionality
	/// </summary>
	public static class Utils
	{




		/// <summary>
		/// Returns the int value contained in the string. Returns null if the int was invalid
		/// </summary>
		/// <param name="val">The String value representing an int</param>
		/// <returns>The int value or null</returns>
		public static int? getIntFromString(String val)
		{
			int? retVal = null;

			try
			{
				retVal = Convert.ToInt32(val);
			}
			catch
			{
				retVal = null;
			}

			return retVal;
		}





		/// <summary>
		/// Converts a string to a double. Returns null if the string value is invalid
		/// </summary>
		/// <param name="val">The string representing a double</param>
		/// <returns>The double value or null</returns>
		public static double? getDoubleFromString(String val)
		{
			double? retVal = null;

			try
			{
				retVal = Convert.ToDouble(val);
			}
			catch
			{
				retVal = null;
			}
			return retVal;
		}

	}
}
