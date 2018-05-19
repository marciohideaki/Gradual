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
using FASTType = OpenFAST.Template.Type.FASTType;
using Key = OpenFAST.util.Key;

namespace OpenFAST.Template.Operator
{
    [Serializable]
    public abstract class OperatorCodec
    {
        virtual public Operator Operator
        {
            get
            {
                return operator_Renamed;
            }
			
        }
        private static readonly System.Collections.Generic.Dictionary<Key, OperatorCodec> OPERATOR_MAP = new System.Collections.Generic.Dictionary<Key, OperatorCodec>();
		
        protected internal static readonly OperatorCodec NONE_ALL;
        protected internal static readonly OperatorCodec CONSTANT_ALL;
        protected internal static readonly OperatorCodec DEFAULT_ALL;
        protected internal static readonly OperatorCodec COPY_ALL = new CopyOperatorCodec();
        protected internal static readonly OperatorCodec INCREMENT_INTEGER;
        protected internal static readonly OperatorCodec DELTA_INTEGER;
        protected internal static readonly OperatorCodec DELTA_STRING = new DeltaStringOperatorCodec();
        protected internal static readonly OperatorCodec DELTA_DECIMAL = new DeltaDecimalOperatorCodec();
        protected internal static readonly OperatorCodec TAIL;
        private readonly Operator operator_Renamed;
		
        protected internal OperatorCodec(Operator operator_Renamed, FASTType[] types)
        {
            this.operator_Renamed = operator_Renamed;
            for (int i = 0; i < types.Length; i++)
            {
                var key = new Key(operator_Renamed, types[i]);
				
                if (!OPERATOR_MAP.ContainsKey(key))
                {
                    OPERATOR_MAP[key] = this;
                }
            }
        }
		
        public static OperatorCodec GetCodec(Operator operator_Renamed, FASTType type)
        {
            var key = new Key(operator_Renamed, type);
			
            if (!OPERATOR_MAP.ContainsKey(key))
            {
                Global.HandleError(Error.FastConstants.S2_OPERATOR_TYPE_INCOMP, "The operator \"" + operator_Renamed + "\" is not compatible with type \"" + type + "\"");
                throw new ArgumentException();
            }
			
            return OPERATOR_MAP[key];
        }
		
        public abstract ScalarValue GetValueToEncode(ScalarValue value_Renamed, ScalarValue priorValue, Scalar field);
		
        public abstract ScalarValue DecodeValue(ScalarValue newValue, ScalarValue priorValue, Scalar field);
		
        public virtual bool IsPresenceMapBitSet(byte[] encoding, FieldValue fieldValue)
        {
            return encoding.Length != 0;
        }
		
        public abstract ScalarValue DecodeEmptyValue(ScalarValue previousValue, Scalar field);
		
        public virtual bool UsesPresenceMapBit(bool optional)
        {
            return true;
        }

        public virtual ScalarValue GetValueToEncode(ScalarValue value_Renamed, ScalarValue priorValue, Scalar scalar, BitVectorBuilder presenceMapBuilder)
        {
            var valueToEncode = GetValueToEncode(value_Renamed, priorValue, scalar);
            if (valueToEncode == null)
                presenceMapBuilder.Skip();
            else
                presenceMapBuilder.set_Renamed();
            return valueToEncode;
        }
		
        public virtual bool CanEncode(ScalarValue value_Renamed, Scalar field)
        {
            return true;
        }
		
        public virtual bool ShouldDecodeType()
        {
            return true;
        }

        public override bool Equals(object obj)//POINTP
        {
            return obj != null && obj.GetType() == GetType();
        }
		
        public override string ToString()
        {
            return operator_Renamed.ToString();
        }
        static OperatorCodec()
        {
            NONE_ALL = new NoneOperatorCodec(Operator.NONE, FASTType.ALL_TYPES());
            CONSTANT_ALL = new ConstantOperatorCodec(Operator.CONSTANT, FASTType.ALL_TYPES());
            DEFAULT_ALL = new DefaultOperatorCodec(Operator.DEFAULT, FASTType.ALL_TYPES());
            INCREMENT_INTEGER = new IncrementIntegerOperatorCodec(Operator.INCREMENT, FASTType.INTEGER_TYPES);
            DELTA_INTEGER = new DeltaIntegerOperatorCodec(Operator.DELTA, FASTType.INTEGER_TYPES);
            TAIL = new TailOperatorCodec(Operator.TAIL, new[]{FASTType.ASCII, FASTType.STRING, FASTType.UNICODE, FASTType.BYTE_VECTOR});
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}