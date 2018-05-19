using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    [Serializable]
    public class ConversorCampoBase
    {
        public ConversorCampoBase()
        {
        }

        public ConversorCampoBase(object parametrosDefault)
        {
            this.ParametrosDefault = parametrosDefault;
        }

        public object ParametrosDefault { get; set; }

        public string DeObjetoParaTexto(object obj)
        {
            return OnDeObjetoParaTexto(obj, this.ParametrosDefault);
        }

        public string DeObjetoParaTexto(object obj, object parametros)
        {
            return OnDeObjetoParaTexto(obj, parametros);
        }

        public object DeTextoParaObjeto(Type tipo, string texto)
        {
            return OnDeTextoParaObjeto(tipo, texto, this.ParametrosDefault);
        }

        public object DeTextoParaObjeto(Type tipo, string texto, object parametros)
        {
            return OnDeTextoParaObjeto(tipo, texto, parametros);
        }

        protected virtual string OnDeObjetoParaTexto(object obj, object parametros)
        {
            return null;
        }

        protected virtual object OnDeTextoParaObjeto(Type tipo, string texto, object parametros)
        {
            return null;
        }
    }
}
