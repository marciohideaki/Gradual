using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    [Serializable]
    public class ConversorCampoData : ConversorCampoBase
    {
        private int posAno = -1;
        private int qtdeAno = -1;
        private int posMes = -1;
        private int posDia = -1;
        private int posHora = -1;
        private int posMinuto = -1;
        private int posSegundo = -1;
        private int posMilissegundo = -1;

        public ConversorCampoData() 
        {
            this.ParametrosDefault = new ConversorCampoDataParametro();
            inicializaParametros();
        }

        public ConversorCampoData(object parametros) : base(parametros)
        {
            inicializaParametros();
        }

        public ConversorCampoDataParametro Parametros
        {
            get { return (ConversorCampoDataParametro)this.ParametrosDefault; }
        }

        private void inicializaParametros()
        {
            // Pega a string de formato
            string formato = this.Parametros.FormatoData.ToLower();

            // Troca a por y caso exista
            formato = formato.Replace('a', 'y');

            // Descobre quantas digitos tem o ano
            if (formato.Contains("yyyy"))
                qtdeAno = 4;
            else if (formato.Contains("yy"))
                qtdeAno = 2;

            // Acha o inicio de cada um
            posAno = formato.IndexOf('y');
            posMes = formato.IndexOf('m');
            posDia = formato.IndexOf('d');
            posHora = formato.IndexOf('h');
            posMinuto = formato.IndexOf('n');
            posSegundo = formato.IndexOf('s');
            posMilissegundo = formato.IndexOf('l');
        }

        protected override string OnDeObjetoParaTexto(object obj, object parametros)
        {
            return base.OnDeObjetoParaTexto(obj, parametros);
        }

        protected override object OnDeTextoParaObjeto(Type tipo, string texto, object parametros)
        {
            if (texto.Trim() != "" && texto.Replace("0", "").Trim() != "")
            {
                int ano = 0;
                int mes = 0;
                int dia = 0;
                int hora = 0;
                int minuto = 0;
                int segundo = 0;
                int milissegundo = 0;

                if (posAno != -1)
                {
                    ano = int.Parse(texto.Substring(posAno, qtdeAno));
                    if (ano < 100 && ano >= 30)
                        ano += 1900;
                    else if (ano < 30)
                        ano += 2000;
                }
                if (posMes != -1)
                    mes = int.Parse(texto.Substring(posMes, 2));
                if (posDia != -1)
                    dia = int.Parse(texto.Substring(posDia, 2));
                if (posHora != -1)
                    hora = int.Parse(texto.Substring(posHora, 2));
                if (posMinuto != -1)
                    minuto = int.Parse(texto.Substring(posMinuto, 2));
                if (posSegundo != -1)
                    segundo = int.Parse(texto.Substring(posSegundo, 2));
                if (posMilissegundo != -1)
                    milissegundo = int.Parse(texto.Substring(posMilissegundo));

                return Convert.ChangeType(new DateTime(ano, mes, dia, hora, minuto, segundo, milissegundo), tipo);
            }
            else
            {
                return null;
            }
        }
    }
}
