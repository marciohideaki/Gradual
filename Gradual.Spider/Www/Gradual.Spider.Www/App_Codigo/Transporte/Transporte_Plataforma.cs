using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Spider.Lib.Dados;

namespace Gradual.Spider.Www.App_Codigo.Transporte
{
    public class Transporte_Plataforma
    {
        #region Propriedades
        
        public string CodigoPlataforma          { get; set; }

        public string NomePlataforma            { get; set; }

        public string CodigoPlataformaSessaoCliente { get; set; }

        public string CodigoSessaoCliente       { get; set; }

        public string NomeSessaoCliente         { get; set; }

        public string CodigoPlataformaSessaoRepassador { get; set; }

        public string CodigoSessaoRepassador    { get; set; }

        public string NomeSessaoRepassador      { get; set; }

        public string CodigoPlataformaSessaoMesa { get; set; }

        public string CodigoSessaoMesa          { get; set; }

        public string NomeSessaoMesa            { get; set; }
        
        public string ValorSessaoCliente        { get; set; }
        
        public string ValorSessaoRepassador     { get; set; }
        
        public string ValorSessaoMesa           { get; set; }

        public string AtivoSessaoCliente        { get; set; }

        public string AtivoSessaoRepassador     { get; set; }

        public string AtivoSessaoMesa           { get; set; }

        public string DataAtualizacao           { get; set; }

        #endregion

        #region Métodos

        public List<Transporte_Plataforma> TraduzirLista(List<PlataformaSessaoInfo> pLista )
        {
            var lRetorno = new List<Transporte_Plataforma>();

            var lListaAgrupada = from a in pLista group a by new { a.CodigoPlataforma } into NewList select  NewList ;

            foreach (var a in lListaAgrupada)
            {
                var lTrans = new Transporte_Plataforma();

                foreach (var p in a)
                {
                    lTrans.CodigoPlataforma = p.CodigoPlataforma.ToString();
                    lTrans.DataAtualizacao  = p.DataUltimoEvento.ToString("dd/MM/yyyy");
                    lTrans.NomePlataforma   = p.NomePlataforma;

                    if (p.CodigoAcesso == 1)      // --> Cliente
                    {
                        lTrans.CodigoSessaoCliente = p.CodigoSessao.ToString();
                        lTrans.ValorSessaoCliente = p.ValorPlataforma.ToString("N2");
                        lTrans.AtivoSessaoCliente = p.StAtivo.ToString();
                        lTrans.NomeSessaoCliente = p.NomeSessao;
                        lTrans.CodigoPlataformaSessaoCliente = p.CodigoPlataformaSessao.ToString();
                    }
                    else if (p.CodigoAcesso == 2) //--> Assessor
                    {
                        lTrans.CodigoSessaoMesa = p.CodigoSessao.ToString();
                        lTrans.ValorSessaoMesa = p.ValorPlataforma.ToString("N2");
                        lTrans.AtivoSessaoMesa = p.StAtivo.ToString();
                        lTrans.NomeSessaoMesa = p.NomeSessao;
                        lTrans.CodigoPlataformaSessaoMesa = p.CodigoPlataformaSessao.ToString();
                    }
                    else if (p.CodigoAcesso == 3)//--> Operador
                    {
                        lTrans.CodigoSessaoRepassador = p.CodigoSessao.ToString();
                        lTrans.ValorSessaoRepassador = p.ValorPlataforma.ToString("N2");
                        lTrans.AtivoSessaoRepassador = p.StAtivo.ToString();
                        lTrans.NomeSessaoRepassador = p.NomeSessao;
                        lTrans.CodigoPlataformaSessaoRepassador = p.CodigoPlataformaSessao.ToString();
                    }

                }

                lRetorno.Add(lTrans);
            }

            //int lUltimoId = 0;

            //var lTrans = new Transporte_Plataforma();

            //pLista.ForEach(p=> 
            //{
            //    lTrans.CodigoPlataforma       = p.CodigoPlataforma.ToString();
            //    lTrans.DataAtualizacao        = p.DataUltimoEvento.ToString("dd/MM/yyyy");
            //    lTrans.NomePlataforma         = p.NomePlataforma;

            //    lUltimoId = lUltimoId == 0 ? p.CodigoPlataforma : lUltimoId;

            //    if (p.CodigoAcesso == 1)      // --> Cliente
            //    {
            //        lTrans.CodigoSessaoCliente           = p.CodigoSessao.ToString();
            //        lTrans.ValorSessaoCliente            = p.ValorPlataforma.ToString("N2");
            //        lTrans.AtivoSessaoCliente            = p.StAtivo.ToString();
            //        lTrans.NomeSessaoCliente             = p.NomeSessao;
            //        lTrans.CodigoPlataformaSessaoCliente = p.CodigoPlataformaSessao.ToString();
            //    }
            //    else if (p.CodigoAcesso == 2) //--> Assessor
            //    {
            //        lTrans.CodigoSessaoMesa           = p.CodigoSessao.ToString();
            //        lTrans.ValorSessaoMesa            = p.ValorPlataforma.ToString("N2");
            //        lTrans.AtivoSessaoMesa            = p.StAtivo.ToString();
            //        lTrans.NomeSessaoMesa             = p.NomeSessao;
            //        lTrans.CodigoPlataformaSessaoMesa = p.CodigoPlataformaSessao.ToString();
            //    }
            //    else if (p.CodigoAcesso == 3)//--> Operador
            //    {
            //        lTrans.CodigoSessaoRepassador           = p.CodigoSessao.ToString();
            //        lTrans.ValorSessaoRepassador            = p.ValorPlataforma.ToString("N2");
            //        lTrans.AtivoSessaoRepassador            = p.StAtivo.ToString();
            //        lTrans.NomeSessaoRepassador             = p.NomeSessao;
            //        lTrans.CodigoPlataformaSessaoRepassador = p.CodigoPlataformaSessao.ToString();
            //    }

            //    if (lUltimoId == p.CodigoPlataforma)
            //    {
            //        lRetorno.Remove(lTrans);
            //        lRetorno.Add(lTrans);
            //    }
            //    else
            //    {
            //        lRetorno.Add(lTrans);
            //        lTrans = new Transporte_Plataforma();
            //    }

            //    lUltimoId = p.CodigoPlataforma;
            //});

            return lRetorno;
        }

        public List<PlataformaSessaoInfo> ToPlataformaSessaoInfo( Transporte_Plataforma pPlataforma)
        {
            var lRetorno = new List<PlataformaSessaoInfo>();

            var p = pPlataforma;
            
            var lTransCliente = new PlataformaSessaoInfo();

            lTransCliente.CodigoPlataforma       = int.Parse(p.CodigoPlataforma);
            lTransCliente.CodigoSessao           = p.CodigoSessaoCliente != "" ? int.Parse(p.CodigoSessaoCliente) : 0;
            lTransCliente.ValorPlataforma        = p.ValorSessaoCliente != "" ? decimal.Parse(p.ValorSessaoCliente) : 0;
            lTransCliente.StAtivo                = bool.Parse(p.AtivoSessaoCliente);
            lTransCliente.DataUltimoEvento       = DateTime.Now;
            lTransCliente.CodigoAcesso           = 1;

            if (p.CodigoPlataformaSessaoCliente != string.Empty)
            {
                lTransCliente.CodigoPlataformaSessao = int.Parse(p.CodigoPlataformaSessaoCliente);
            }

            lRetorno.Add(lTransCliente);

            var lTransRepasse = new PlataformaSessaoInfo();

            lTransRepasse.CodigoPlataforma       = int.Parse(p.CodigoPlataforma);
            lTransRepasse.CodigoSessao           = p.CodigoSessaoRepassador != "" ? int.Parse(p.CodigoSessaoRepassador) : 0 ;
            lTransRepasse.ValorPlataforma        = p.ValorSessaoRepassador != "" ? decimal.Parse(p.ValorSessaoRepassador) : 0;
            lTransRepasse.StAtivo                = bool.Parse(p.AtivoSessaoRepassador);
            lTransRepasse.DataUltimoEvento       = DateTime.Now;
            lTransRepasse.CodigoAcesso           = 3;

            if (p.CodigoPlataformaSessaoRepassador != string.Empty)
            {
                lTransRepasse.CodigoPlataformaSessao = int.Parse(p.CodigoPlataformaSessaoRepassador);
            }
            
            lRetorno.Add(lTransRepasse);

            var lTransMesa = new PlataformaSessaoInfo();

            lTransMesa.CodigoPlataforma       = int.Parse(p.CodigoPlataforma);
            lTransMesa.CodigoSessao           = p.CodigoSessaoMesa != "" ?  int.Parse(p.CodigoSessaoMesa) : 0 ;
            lTransMesa.ValorPlataforma        = p.ValorSessaoMesa != "" ? decimal.Parse(p.ValorSessaoMesa) : 0;
            lTransMesa.StAtivo                = bool.Parse(p.AtivoSessaoMesa);
            lTransMesa.DataUltimoEvento       = DateTime.Now;
            lTransMesa.CodigoAcesso           = 2;

            if (p.CodigoPlataformaSessaoMesa != string.Empty)
            {
                lTransMesa.CodigoPlataformaSessao = int.Parse(p.CodigoPlataformaSessaoMesa);
            }

            lRetorno.Add(lTransMesa);

            return lRetorno;
        }

        public Transporte_Plataforma TraduzirLog(List<Transporte_Plataforma> pLista)
        {
            var lRetorno = new Transporte_Plataforma();

            for (int i = 0; i < pLista.Count; i++)
            {

            }

            return lRetorno;
        }

        public Transporte_Plataforma Traduzir(PlataformaSessaoInfo pParametros)
        {
            var lRetorno = new Transporte_Plataforma();

            return lRetorno;
        }
        #endregion
    }
}