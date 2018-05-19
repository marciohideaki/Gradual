using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    [Serializable]
    public class ConversorCampoNumero : ConversorCampoBase
    {
        public ConversorCampoNumeroParametro Parametros
        {
            get { return (ConversorCampoNumeroParametro)this.ParametrosDefault; }
        }

        public ConversorCampoNumero() 
        {
            this.ParametrosDefault = new ConversorCampoNumeroParametro();
        }

        public ConversorCampoNumero(object parametrosDefault) : base(parametrosDefault)
        {
        }

        protected override string OnDeObjetoParaTexto(object obj, object parametros)
        {
            return base.OnDeObjetoParaTexto(obj, parametros);
        }

        protected override object OnDeTextoParaObjeto(Type tipo, string texto, object parametros)
        {
            // Se tiver separador, apenas certifica-se que é virgula
            if (this.Parametros.Separador == null || this.Parametros.Separador == "")
            {
                // Coloca a virgula
                texto = texto.Substring(0, texto.Length - this.Parametros.NumeroDecimais) + "," + texto.Substring(texto.Length - this.Parametros.NumeroDecimais, this.Parametros.NumeroDecimais);
            }
            else
            {
                if (this.Parametros.Separador != ",")
                    texto = texto.Replace(this.Parametros.Separador, ",");
            }

            // Transforma
            object ret = Convert.ChangeType(texto, tipo, System.Globalization.CultureInfo.GetCultureInfo("pt-br"));

            // Retorna
            return ret;
        }
    }
}
