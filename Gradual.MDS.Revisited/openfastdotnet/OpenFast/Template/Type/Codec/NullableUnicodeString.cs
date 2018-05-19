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
	public sealed class NullableUnicodeString:NotStopBitEncodedTypeCodec
	{
		public static ScalarValue DefaultValue
		{
			get
			{
				return new StringValue("");
			}
			
		}

	    internal NullableUnicodeString()
		{
		}
		
		public override byte[] EncodeValue(ScalarValue value_Renamed)
		{
			if (value_Renamed.Null)
				return NULLABLE_BYTE_VECTOR_TYPE.EncodeValue(ScalarValue.NULL);

            byte[] utf8encoding = System.Text.Encoding.UTF8.GetBytes(((StringValue) value_Renamed).value_Renamed);
			return NULLABLE_BYTE_VECTOR_TYPE.Encode(new ByteVectorValue(utf8encoding));

		}
		
		public override ScalarValue Decode(System.IO.Stream in_Renamed)
		{
			ScalarValue decodedValue = NULLABLE_BYTE_VECTOR_TYPE.Decode(in_Renamed);
			if (decodedValue == null)
				return null;
			var value_Renamed = (ByteVectorValue) decodedValue;
			return new StringValue(System.Text.Encoding.UTF8.GetString(value_Renamed.value_Renamed));

		}

		public static ScalarValue FromString(string value_Renamed)
		{
			return new StringValue(value_Renamed);
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