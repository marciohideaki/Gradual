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
using TypeCodec = OpenFAST.Template.Type.Codec.TypeCodec;

namespace OpenFAST.Template.Type
{
	[Serializable]
	public sealed class UnsignedIntegerType:IntegerType
	{
	    public UnsignedIntegerType(int numberBits, long maxValue):base("uInt" + numberBits, 0, maxValue, TypeCodec.UINT, TypeCodec.NULLABLE_UNSIGNED_INTEGER)
		{
		}

        public override TypeCodec GetCodec(Operator.Operator operator_Renamed, bool optional)
		{
            if (operator_Renamed.Equals(Operator.Operator.DELTA))
				if (optional)
					return TypeCodec.NULLABLE_INTEGER;
				else
					return TypeCodec.INTEGER;
			return base.GetCodec(operator_Renamed, optional);
		}
	}
}