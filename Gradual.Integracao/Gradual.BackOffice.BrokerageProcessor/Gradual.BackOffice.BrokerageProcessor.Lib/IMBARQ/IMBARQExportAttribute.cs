using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.IMBARQ
{
    public enum IMBARQFieldType
    {
        FieldTypeString,
        FieldTypeDateTime,
        FieldTypeDecimal,
        FieldTypeYesOrNo,
        FieldTypeContractType,
        FieldTypeSettlementType,
        FieldTypeSide,
        FieldTypeSubAccountCode,
        FieldTypePayingInstitution,
        FieldTypeLaunch,
        FieldTypeCollateralType,
        FieldTypeCollateralCoverage,
        FieldTypeOptionType
        
    }

    [System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
                AllowMultiple = false,
                   Inherited = true)]  
    public class IMBARQExport : Attribute
    {
        public const int FIELD_NO_ORDER = -1;
        public bool ExportField;
        public int FieldOrder;

        //public IMBARQExport(bool exportField = false)
        //{
        //    this.ExportField = exportField;
        //    this.FieldOrder = FIELD_NO_ORDER;
        //}

        public IMBARQExport(bool exportField, int fieldOrder)
        {
            this.ExportField = exportField;
            this.FieldOrder = fieldOrder;
        }
    }

    [System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
                AllowMultiple = false,
                   Inherited = true)]
    public class IMBARQFieldDescription : Attribute
    {
        public IMBARQFieldType FieldType;
        public string Format;
        public int NumDecimalPlaces;

        public IMBARQFieldDescription(IMBARQFieldType fieldType, string format, int numDecimalPlaces=0)
        {
            this.FieldType = fieldType;
            this.Format = format;
            this.NumDecimalPlaces = numDecimalPlaces;
        }
    }

}
