using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Gradual.OMS.Persistencia
{
    [Serializable]
    [DataContract]
    public class ListarTiposResponse
    {
        [DataMember]
        public List<Type> Resultado { get; set; }

        public ListarTiposResponse()
        {
            this.Resultado = new List<Type>();
        }
    }
}
