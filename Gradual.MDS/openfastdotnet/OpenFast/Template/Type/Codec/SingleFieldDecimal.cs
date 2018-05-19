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

namespace OpenFAST.Template.Type.Codec
{
	[Serializable]
	sealed class SingleFieldDecimal:TypeCodec
	{

		public static ScalarValue DefaultValue
		{
			get
			{
				return new DecimalValue(0.0);
			}
			
		}

	    public override byte[] EncodeValue(ScalarValue v)
		{
			if (v == ScalarValue.NULL)
			{
				return NULL_VALUE_ENCODING;
			}
			
			var buffer = new System.IO.MemoryStream();
			var value_Renamed = (DecimalValue) v;
			
			try
			{
				if (Math.Abs(value_Renamed.exponent) > 63)
				{
					Global.HandleError(Error.FastConstants.R1_LARGE_DECIMAL, "Encountered exponent of size " + value_Renamed.exponent);
				}

			    byte[] temp_byteArray = INTEGER.Encode(new IntegerValue(value_Renamed.exponent));
				buffer.Write(temp_byteArray, 0, temp_byteArray.Length);
			    byte[] temp_byteArray2 = INTEGER.Encode(new LongValue(value_Renamed.mantissa));
				buffer.Write(temp_byteArray2, 0, temp_byteArray2.Length);
			}
			catch (System.IO.IOException e)
			{
				throw new RuntimeException(e);
			}
			
			return buffer.ToArray();
		}
		
		public override ScalarValue Decode(System.IO.Stream in_Renamed)
		{
			int exponent = ((IntegerValue) INTEGER.Decode(in_Renamed)).value_Renamed;
			
			if (Math.Abs(exponent) > 63)
			{
				Global.HandleError(Error.FastConstants.R1_LARGE_DECIMAL, "Encountered exponent of size " + exponent);
			}
			
			long mantissa = INTEGER.Decode(in_Renamed).ToLong();
			var decimalValue = new DecimalValue(mantissa, exponent);
			
			return decimalValue;
		}
		
		public static ScalarValue FromString(string value_Renamed)
		{
			return new DecimalValue(Double.Parse(value_Renamed));
		}
		
		public  override bool Equals(Object obj)
		{
			return obj != null && obj.GetType() == GetType();
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}