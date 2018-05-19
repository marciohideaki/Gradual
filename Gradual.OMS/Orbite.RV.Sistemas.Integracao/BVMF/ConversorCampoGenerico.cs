using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    [Serializable]
    public class ConversorCampoGenerico : ConversorCampoBase
    {
        public ConversorCampoGenerico() 
        {
        }

        public ConversorCampoGenerico(object parametrosDefault) : base(parametrosDefault)
        {
        }

        protected override string OnDeObjetoParaTexto(object obj, object parametros)
        {
            return obj.ToString();
        }

        protected override object OnDeTextoParaObjeto(Type tipo, string texto, object parametros)
        {
            object ret = null;
            try
            {
                ret = Convert.ChangeType(texto, tipo);
                if (tipo == typeof(string))
                    ret = ((string)ret).Trim();
            }
            catch
            {
            }
            return ret;
        }
    }
}
