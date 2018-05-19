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
            // Ajusta os parametros
            ConversorCampoNumeroParametro parametros2 = 
                (ConversorCampoNumeroParametro)(parametros == null ? this.Parametros : parametros);

            // Se tiver separador, apenas certifica-se que é virgula
            if (parametros2.Separador == null || parametros2.Separador == "")
            {
                // Coloca a virgula
                texto = texto.Substring(0, texto.Length - parametros2.NumeroDecimais) + "," + texto.Substring(texto.Length - parametros2.NumeroDecimais, parametros2.NumeroDecimais);
            }
            else
            {
                if (parametros2.Separador != ",")
                    texto = texto.Replace(this.Parametros.Separador, ",");
            }

            // Transforma
            object ret = Convert.ChangeType(texto, tipo, System.Globalization.CultureInfo.GetCultureInfo("pt-br"));

            // Retorna
            return ret;
        }
    }
}
