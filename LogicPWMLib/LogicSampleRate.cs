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

namespace BrrrBayBay.LogicPWMLib
{
	/// <summary>
	/// defines samples that can be used by the Logic Device
	/// </summary>
	public enum LogicSampleRate : uint
	{
		f25k = 25000,

		f50k = 50000,

		f100k = 100000,

		f200k = 200000,

		f250k = 250000,

		f500k = 500000,

		f1M = 1000000,

		f2M = 2000000,

		f4M = 4000000,

		f8M = 8000000,

		f12M = 12000000,

		f16M = 16000000,

		f24M = 24000000
		
	}
}
