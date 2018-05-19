using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using System.Globalization;

namespace Gradual.OMS.ServicoA4S
{
    public class JsonOrderTypeConverter : CustomCreationConverter<OrdemTipoEnum> 
    {
        public override OrdemTipoEnum Create(Type objectType)
        {
            return OrdemTipoEnum.Limitada;
        }
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            int retorno = 50;

            if (value is OrdemTipoEnum)
            {
                OrdemTipoEnum tipo = (OrdemTipoEnum)value;

                switch (tipo)
                {
                    case OrdemTipoEnum.Mercado: retorno = 49; break;
                    case OrdemTipoEnum.Limitada: retorno = 50; break;
                    case OrdemTipoEnum.StopLoss: retorno = 51; break;
                    case OrdemTipoEnum.StopLimitada: retorno = 52; break;
                    case OrdemTipoEnum.OnClose: retorno = 65; break;
                    case OrdemTipoEnum.MarketWithLeftOverLimit: retorno = 75; break;
                    case OrdemTipoEnum.Reversao: retorno = 82; break;
                    case OrdemTipoEnum.StopStart: retorno = 83; break;
                    default: retorno = 50; break;
                }
            }

            writer.WriteValue(retorno);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer)
        {
            //            bool nullable = ReflectionUtils.IsNullableType(objectType.GetType().I);
            //Type t = (nullable)
            //  ? Nullable.GetUnderlyingType(objectType)
            //  : objectType;

            if (reader.TokenType == JsonToken.Null)
            {
                //if (!ReflectionUtils.IsNullableType(objectType))
                //    throw new Exception("Cannot convert null value to {0}." + objectType.GetType().ToString());

                return null;
            }

            if (reader.TokenType != JsonToken.String)
                throw new Exception("Unexpected token parsing date. Expected String, got " + reader.TokenType.ToString() + ".");

            int orderType = Convert.ToInt32(reader.Value.ToString());

            //if (string.IsNullOrEmpty(orderTypeText))
            //    return null;

            switch (orderType)
            {
                case 49: return OrdemTipoEnum.Mercado; 
                case 50: return OrdemTipoEnum.Limitada;
                case 51: return OrdemTipoEnum.StopLoss;
                case 52: return OrdemTipoEnum.StopLimitada;
                case 65: return OrdemTipoEnum.OnClose; 
                case 75: return OrdemTipoEnum.MarketWithLeftOverLimit;
                case 82: return OrdemTipoEnum.Reversao;
                case 83: return OrdemTipoEnum.StopStart;
                default: return OrdemTipoEnum.Limitada;
            }

            return OrdemTipoEnum.Limitada;
        }


    }
}
