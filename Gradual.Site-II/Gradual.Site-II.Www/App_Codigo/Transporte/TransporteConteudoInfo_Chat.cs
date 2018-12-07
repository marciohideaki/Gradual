using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace Gradual.Site.Www
{
    public class TransporteConteudoInfo_Chat
    {
        #region Propriedades

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public string Palestrante { get; set; }

        public string DataCadastro { get; set; }

        public string HoraInicio { get; set; }

        public string HoraFinal { get; set; }

        public string DiaSemana { get; set; }

        public string FlagPublicado { get; set; }

        public string Tag { get; set; }

        public TimeSpan HoraFormataInicio { get; set; }

        public TimeSpan HoraFormataFinal { get; set; }

        public string DiasSemanasFormatado { get; set; }

        public string HoraFormatada { get; set; }

        #endregion

        #region Métodos Públicos

        public bool EstaDisponivelAgora()
        {
            string lDiasDaSemana = this.DiaSemana;
            
            //ATENÇÃO!! no DateTime.Now.DayOfWeek, domingo é ZERO e segunda 1; no cadastro, domingo NÃO PODE SER 7!, tem que ser zero também. os outros dias não dão problema.

            if (lDiasDaSemana.Contains(Convert.ToInt32(DateTime.Now.DayOfWeek).ToString()))
            {
                //tem no dia de hoje, vendo a hora...
                DateTime lDataInicial = DateTime.ParseExact(string.Format("{0} {1}", DateTime.Now.ToString("dd/MM/yyyy"), this.HoraInicio), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime lDataFinal   = DateTime.ParseExact(string.Format("{0} {1}", DateTime.Now.ToString("dd/MM/yyyy"), this.HoraFinal),  "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                if (lDataInicial <= DateTime.Now && DateTime.Now <= lDataFinal)
                    return true;
            }

            return false;
        }

        #endregion

        /*

        public List<TransporteConteudoInfo_Chat> TraduzirLista(List<TransporteConteudoInfo_Chat> pParametro)
        {
            var lRetorno = new List<TransporteConteudoInfo_Chat>();


            if (null != pParametro && pParametro.Count > 0)
                pParametro.ForEach(lTransporte =>
                {
                    var TransporteChat = new TransporteConteudoInfo_Chat();

                    TransporteChat.Titulo               =   lTransporte.Titulo;

                    TransporteChat.Descricao            =   lTransporte.Descricao;

                    TransporteChat.Palestrante          =   lTransporte.Palestrante;

                    TransporteChat.DataCadastro         =   lTransporte.DataCadastro;

                    TransporteChat.DiaSemana            =   lTransporte.DiaSemana;

                    TransporteChat.FlagPublicado        =   lTransporte.FlagPublicado;

                    TransporteChat.Tag                  =   lTransporte.Tag;

                    TransporteChat.HoraFormataInicio    =   new TimeSpan(lTransporte.HoraInicio.Split(':')[0].DBToInt32(), lTransporte.HoraInicio.Split(':')[1].DBToInt32(), 0);

                    TransporteChat.HoraFormataFinal     =   new TimeSpan(lTransporte.HoraFinal.Split(':')[0].DBToInt32(), lTransporte.HoraFinal.Split(':')[1].DBToInt32(), 0);

                    TransporteChat.HoraFinal            = lTransporte.HoraFinal;

                    TransporteChat.HoraInicio           = lTransporte.HoraInicio;
                    
                    TransporteChat.HoraFormatada        = lTransporte.HoraInicio + " às " + lTransporte.HoraFinal;

                    TransporteChat.DiasSemanasFormatado = this.FormatarDiasSemana(lTransporte.DiaSemana);


                    lRetorno.Add(TransporteChat);


                });

            return lRetorno;
        }

        private string FormatarDiasSemana(string pDiasSemanasChat)
        {
            string lRetorno = "";
            string[] dias = pDiasSemanasChat.Split(',');

            foreach (var item in dias)
            {
                if (lRetorno != string.Empty)
                    lRetorno = lRetorno + " , " + this.RetornarDiaSemanaEmString(item.DBToInt32());
                else
                    lRetorno = this.RetornarDiaSemanaEmString(item.DBToInt32());
            }

            return lRetorno;
        }

        private string RetornarDiaSemanaEmString(int pDia)
        {
            string lRetorno = "";

            switch (pDia)
            {
                case 1:
                    lRetorno = "Segunda-Feira";
                    break;

                case 2:
                    lRetorno = "Terça-Feira";
                    break;

                case 3:
                    lRetorno = "Quarta-Feira";
                    break;

                case 4:
                    lRetorno = "Quinta-Feira";
                    break;

                case 5:
                    lRetorno = "Sexta-Feira";
                    break;

                case 6:
                    lRetorno = "Sabado";
                    break;

                case 7:
                    lRetorno = "Domingo";
                    break;

               
            }

            return lRetorno;
        }*/
    }
}