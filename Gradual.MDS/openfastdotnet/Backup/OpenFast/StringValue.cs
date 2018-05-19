/*

The contents of this file are subject to the Mozilla Public License
Version 1.1 (the "License"); you may not use this file except in
compliance with the License. You may obtain a copy of the License at
http://www.mozilla.org/MPL/

Software distributed under the License is distributed on an "AS IS"
basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
License for the specific language governing rights and limitations
under the License.

The Original Code is OpenFAST.

The Initial Developer of the Original Code is The LaSalle Technology
Group, LLC.  Portions created by Shariq Muhammad
are Copyright (C) Shariq Muhammad. All Rights Reserved.

Contributor(s): Shariq Muhammad <shariq.muhammad@gmail.com>

*/
using System;

namespace OpenFAST
{
	
	[Serializable]
	public class StringValue:ScalarValue
	{
		override public byte[] Bytes
		{
			get
			{
				return System.Text.Encoding.UTF8.GetBytes(value_Renamed);
			}
			
		}

	    public string value_Renamed;
		
		public StringValue(string value_Renamed)
		{
			if (value_Renamed == null)
				throw new NullReferenceException();
			this.value_Renamed = value_Renamed;
		}
		public override byte ToByte()
		{
			var i = ToInt();
			if (i > SByte.MaxValue || i < SByte.MinValue)
			{
				Global.HandleError(Error.FastConstants.R4_NUMERIC_VALUE_TOO_LARGE, "The value \"" + i + "\" is too large to fit into a byte.");
				return 0;
			}
			return (byte) i;
		}
		public override short ToShort()
		{
			int i = ToInt();
			if (i > Int16.MaxValue || i < Int16.MinValue)
			{
				Global.HandleError(Error.FastConstants.R4_NUMERIC_VALUE_TOO_LARGE, "The value \"" + i + "\" is too large to fit into a short.");
				return 0;
			}
			return (short) i;
		}
		public override int ToInt()
		{
			try
			{
				return Int32.Parse(value_Renamed);
			}
			catch (Exception e)
			{
				Global.HandleError(Error.FastConstants.R4_NUMERIC_VALUE_TOO_LARGE, "The value \"" + value_Renamed + "\" is too large to fit into an int.", e);
				return 0;
			}
		}
		public override long ToLong()
		{
			try
			{
				return Int64.Parse(value_Renamed);
			}
			catch (FormatException e)
			{
				Global.HandleError(Error.FastConstants.R4_NUMERIC_VALUE_TOO_LARGE, "The value \"" + value_Renamed + "\" is too large to fit into a long.", e);
				return 0;
			}
		}
		public override double ToDouble()
		{
			try
			{
				return Double.Parse(value_Renamed);
			}
			catch (FormatException e)
			{
				Global.HandleError(Error.FastConstants.R4_NUMERIC_VALUE_TOO_LARGE, "The value\"" + value_Renamed + "\" is too large to fit into a double.", e);
				return 0.0;
			}
		}
		public override Decimal ToBigDecimal()
		{
			return Decimal.Parse(value_Renamed, System.Globalization.NumberStyles.Any);
		}
		public override string ToString()
		{
			return value_Renamed;
		}
		public  override bool Equals(Object obj)
		{
			if ((obj == null) || !(obj is StringValue))
			{
				return false;
			}
			return Equals((StringValue) obj);
		}
		internal bool Equals(StringValue otherValue)
		{
			return value_Renamed.Equals(otherValue.value_Renamed);
		}
		public override int GetHashCode()
		{
			return value_Renamed.GetHashCode();
		}
		public override bool EqualsValue(string defaultValue)
		{
			return value_Renamed.Equals(defaultValue);
		}
	}
}