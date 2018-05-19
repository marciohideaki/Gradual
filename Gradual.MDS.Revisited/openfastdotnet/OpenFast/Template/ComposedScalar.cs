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
using System.Text;

namespace OpenFAST.Template
{
	
	[Serializable]
	public class ComposedScalar:Field
	{
		override public string TypeName
		{
			get
			{
				return type.Name;
			}
			
		}
		override public System.Type ValueType
		{
			get
			{
				return ScalarValueType;
			}
			
		}
		virtual public FASTType Type
		{
			get
			{
				return type;
			}
			
		}
		virtual public Scalar[] Fields
		{
			get
			{
				return fields;
			}
			
		}

	    private const System.Type ScalarValueType = null;
		private readonly Scalar[] fields;
		private readonly ComposedValueConverter valueConverter;
		private readonly FASTType type;
		
		public ComposedScalar(string name, FASTType type, Scalar[] fields, bool optional, ComposedValueConverter valueConverter):this(new QName(name), type, fields, optional, valueConverter)
		{
		}
		
		public ComposedScalar(QName name, FASTType type, Scalar[] fields, bool optional, ComposedValueConverter valueConverter):base(name, optional)
		{
			this.fields = fields;
			this.valueConverter = valueConverter;
			this.type = type;
		}
		
		public override FieldValue CreateValue(string value_Renamed)
		{
			return type.GetValue(value_Renamed);
		}
		
		public override FieldValue Decode(System.IO.Stream in_Renamed, Group decodeTemplate, Context context, BitVectorReader presenceMapReader)
		{
			var values = new FieldValue[fields.Length];
			for (var i = 0; i < fields.Length; i++)
			{
				values[i] = fields[i].Decode(in_Renamed, decodeTemplate, context, presenceMapReader);
				if (i == 0 && values[0] == null)
					return null;
			}
			return valueConverter.Compose(values);
		}
		
		public override byte[] Encode(FieldValue value_Renamed, Group encodeTemplate, Context context, BitVectorBuilder presenceMapBuilder)
		{
			if (value_Renamed == null)
			{
				// Only encode null in the first field.
				return fields[0].Encode(null, encodeTemplate, context, presenceMapBuilder);
			}
		    var buffer = new System.IO.MemoryStream(fields.Length * 8);
		    var values = valueConverter.Split(value_Renamed);
		    for (int i = 0; i < fields.Length; i++)
		    {
		        try
		        {
		            var temp_byteArray = fields[i].Encode(values[i], encodeTemplate, context, presenceMapBuilder);
		            buffer.Write(temp_byteArray, 0, temp_byteArray.Length);
		        }
		        catch (System.IO.IOException e)
		        {
		            throw new RuntimeException(e);
		        }
		    }
		    return buffer.ToArray();
		}
		
		public override bool IsPresenceMapBitSet(byte[] encoding, FieldValue fieldValue)
		{
			return false;
		}
		
		public override bool UsesPresenceMapBit()
		{
			return optional;
		}
		
		public  override bool Equals(Object obj)
		{
			if (obj == this)
				return true;
			if (obj == null || !obj.GetType().Equals(typeof(ComposedScalar)))
				return false;
			var other = (ComposedScalar) obj;
			if (other.fields.Length != fields.Length)
				return false;
			if (!other.Name.Equals(Name))
				return false;
			for (int i = 0; i < fields.Length; i++)
			{
				if (!other.fields[i].Type.Equals(fields[i].Type))
					return false;
				if (!other.fields[i].TypeCodec.Equals(fields[i].TypeCodec))
					return false;
				if (!other.fields[i].Operator.Equals(fields[i].Operator))
					return false;
				if (!other.fields[i].OperatorCodec.Equals(fields[i].OperatorCodec))
					return false;
				if (!other.fields[i].DefaultValue.Equals(fields[i].DefaultValue))
					return false;
				if (!other.fields[i].Dictionary.Equals(fields[i].Dictionary))
					return false;
			}
			return true;
		}
		
		public override int GetHashCode()
		{
			return name.GetHashCode();
		}
		
		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append("Composed {");
			for (int i = 0; i < fields.Length; i++)
			{
				builder.Append(fields[i].ToString()).Append(", ");
			}
			builder.Remove(builder.Length - 2, builder.Length);
			return builder.Append("}").ToString();
		}
	}
}