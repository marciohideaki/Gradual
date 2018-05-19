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
	sealed class ByteVectorType:TypeCodec
	{
	    public override byte[] Encode(ScalarValue value_Renamed)
		{
			byte[] bytes = value_Renamed.Bytes;
			int lengthSize = IntegerCodec.GetUnsignedIntegerSize(bytes.Length);
			var encoding = new byte[bytes.Length + lengthSize];
			byte[] length = UINT.Encode(new IntegerValue(bytes.Length));
			Array.Copy(length, 0, encoding, 0, lengthSize);
			Array.Copy(bytes, 0, encoding, lengthSize, bytes.Length);
			return encoding;
		}
		
		public override ScalarValue Decode(System.IO.Stream in_Renamed)
		{
			int length = ((IntegerValue) UINT.Decode(in_Renamed)).value_Renamed;
			var encoding = new byte[length];
			for (int i = 0; i < length; i++)
				try
				{
					encoding[i] = (byte) in_Renamed.ReadByte();
				}
				catch (System.IO.IOException e)
				{
					throw new RuntimeException(e);
				}
			return new ByteVectorValue(encoding);
		}
		public override byte[] EncodeValue(ScalarValue value_Renamed)
		{
			throw new NotSupportedException();
		}
		
		public static ScalarValue FromString(string value_Renamed)
		{
			return new ByteVectorValue(System.Text.Encoding.UTF8.GetBytes(value_Renamed));
		}
		public  override bool Equals(Object obj)
		{
			return obj != null && obj.GetType() == GetType();//POINTP
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}