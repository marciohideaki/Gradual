using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;
using Gradual.Site.DbLib.Persistencias.MinhaConta.Fundos;
using Gradual.Site.DbLib.Dados.MinhaConta;

namespace Gradual.Site.Www.Transporte
{
    [Serializable]
    public class Transporte_PosicaoCotista
    {
        public string IdFundo { get; set; }

        public string NomeFundo { get; set; }

        public string Risco { get; set; }

        public string ValorCota { get; set; }

        public string QtdCotas { get; set; }

        public string ValorBruto { get; set; }

        public string IR { get; set; }

        public string ValorLiquido { get; set; }

        public string DataAtualizacao { get; set; }

        public string CodigoAnbima { get; set; }

        public string Cota { get; set; }

        public string Quantidade { get; set; }

        public string IOF { get; set; }

        public string DataLiquidacao { get; set; }

        public List<Transporte_PosicaoCotista> TraduzirLista(PosicaoCotista.PosicaoCotistaViewModel[] pLista)
        {
            var lRetorno = new List<Transporte_PosicaoCotista>();

            PosicaoCotista.PosicaoCotistaViewModel[] lListaTemp = pLista.OrderByDescending(posicao => posicao.DataConversao).ToArray();

            for (int i = 0; i < lListaTemp.Count(); i++)
            {
                var lTrans = new Transporte_PosicaoCotista();

                if (lListaTemp[i].ValorBruto == 0) continue;

                if (lListaTemp[i].CodigoAnbima == string.Empty) continue;

                IntegracaoFundosInfo lFundo = new IntegracaoFundosDbLib().GetNomeRiscoFundo(lListaTemp[i].CodigoAnbima, 0);

                if (!lFundo.CodigoFundoItau.Equals(string.Empty)) continue;

                lTrans.DataAtualizacao = lListaTemp[i].DataConversao.ToString("dd/MM/yyyy");
                lTrans.NomeFundo       = lListaTemp[i].CodigoAnbima == "" ? "Sem Código ANBIMA" : lFundo.NomeProduto;
                lTrans.IR              = lListaTemp[i].ValorIR.ToString("N2");
                lTrans.QtdCotas        = lListaTemp[i].Quantidade.ToString("N8");
                lTrans.Risco           = lFundo.Risco;
                lTrans.ValorBruto      = lListaTemp[i].ValorBruto.ToString("N2");
                lTrans.ValorCota       = lListaTemp[i].CotaDia.ToString("N8");
                lTrans.ValorLiquido    = lListaTemp[i].ValorLiquido.ToString("N2");
                lTrans.CodigoAnbima    = lListaTemp[i].CodigoAnbima;
                lTrans.IdFundo         = lFundo.IdProduto.ToString();
                lTrans.IOF             = lListaTemp[i].ValorIOF.ToString("N2");

                lRetorno.Add(lTrans);

            }

            return lRetorno;
        }

        public List<Transporte_PosicaoCotista> TraduzirListaSumarizada(PosicaoCotista.PosicaoCotistaViewModel[] pLista)
        {
            var lRetorno = new List<Transporte_PosicaoCotista>();

            PosicaoCotista.PosicaoCotistaViewModel[] lListaOrdenada = pLista.OrderByDescending(posicao => posicao.DataConversao).ToArray();

            var lListaTemp = new List<PosicaoCotista.PosicaoCotistaViewModel>();

            foreach (PosicaoCotista.PosicaoCotistaViewModel posicao in lListaOrdenada)
            {
                var posicaoTemp = lListaTemp.Find(pos => { return pos.CodigoAnbima == posicao.CodigoAnbima; });

                if (posicaoTemp == null)
                {
                    lListaTemp.Add(posicao);
                }
                else
                {
                    var lPosicao = posicaoTemp;
                    
                    lListaTemp.Remove(posicaoTemp);

                    lPosicao.ValorBruto   += posicao.ValorBruto;
                    lPosicao.ValorIR      += posicao.ValorIR;
                    lPosicao.ValorIOF     += posicao.ValorIOF;
                    lPosicao.Quantidade   += posicao.Quantidade;
                    lPosicao.ValorLiquido += posicao.ValorLiquido;

                    lListaTemp.Add(lPosicao);
                }
            }

            for (int i = 0; i < lListaTemp.Count(); i++)
            {
                var lTrans = new Transporte_PosicaoCotista();

                if (lListaTemp[i].ValorBruto == 0) continue;

                if (lListaTemp[i].CodigoAnbima == string.Empty) continue;

                IntegracaoFundosInfo lFundo = new IntegracaoFundosDbLib().GetNomeRiscoFundo(lListaTemp[i].CodigoAnbima, 0);

                if (!string.IsNullOrEmpty(lFundo.CodigoFundoItau)) continue;

                lTrans.DataAtualizacao = lListaTemp[i].DataConversao.ToString("dd/MM/yyyy");
                lTrans.NomeFundo       = lListaTemp[i].CodigoAnbima == "" ? "Sem Código ANBIMA" : lFundo.NomeProduto;
                lTrans.IR              = lListaTemp[i].ValorIR.ToString("N2");
                lTrans.QtdCotas        = lListaTemp[i].Quantidade.ToString("N8");
                lTrans.Risco           = lFundo.Risco;
                lTrans.ValorBruto      = lListaTemp[i].ValorBruto.ToString("N2");
                lTrans.ValorCota       = lListaTemp[i].CotaDia.ToString("N8");
                lTrans.ValorLiquido    = lListaTemp[i].ValorLiquido.ToString("N2");
                lTrans.CodigoAnbima    = lListaTemp[i].CodigoAnbima;
                lTrans.IdFundo         = lFundo.IdProduto.ToString();
                lTrans.IOF             = lListaTemp[i].ValorIOF.ToString("N2");

                lRetorno.Add(lTrans);

            }

            return lRetorno;
        }

        public List<FundoInfo> TraduzirListaFinancial(PosicaoCotista.PosicaoCotistaViewModel[] pLista)
        {
            var lRetorno = new List<FundoInfo>();

            PosicaoCotista.PosicaoCotistaViewModel[] lListaTemp = pLista.OrderByDescending(posicao => posicao.DataConversao).ToArray();

            for (int i = 0; i < lListaTemp.Count(); i++)
            {
                var lTrans = new FundoInfo();

                if (lListaTemp[i].ValorBruto == 0) continue;

                if (lListaTemp[i].CodigoAnbima == string.Empty) continue;

                IntegracaoFundosInfo lFundo = new IntegracaoFundosDbLib().GetNomeRiscoFundo(lListaTemp[i].CodigoAnbima, 0);

                Nullable<int> lCodigoFundoItau = new IntegracaoFundosDbLib().VerificaExistenciaFundoItau(lListaTemp[i].CodigoAnbima);

                if (lCodigoFundoItau != null) continue;

                lTrans.DataAtualizacao   = lListaTemp[i].DataConversao;
                lTrans.NomeFundo         = lListaTemp[i].CodigoAnbima == "" ? "Sem Código ANBIMA" : lFundo.NomeProduto;
                lTrans.IR                = lListaTemp[i].ValorIR;
                lTrans.Quantidade        = lListaTemp[i].Quantidade;
                //lTrans.Risco           = lFundo.Risco;
                lTrans.ValorBruto        = lListaTemp[i].ValorBruto;
                lTrans.Cota              = lListaTemp[i].CotaDia;
                lTrans.ValorLiquido      = lListaTemp[i].ValorLiquido;
                //lTrans.CodigoAnbima    = pLista[i].CodigoAnbima;
                lTrans.IOF               = lListaTemp[i].ValorIOF;
                lTrans.CodigoFundo       = lFundo.IdProduto;
                lRetorno.Add(lTrans);

            }

            return lRetorno;
        }

        public List<Transporte_PosicaoCotista> TrauzirListaItau(List<FundoInfo> pLista)
        {
            var lRetorno = new List<Transporte_PosicaoCotista>();

            List<FundoInfo> lListaTemp = pLista.OrderByDescending(posicao => posicao.DataAtualizacao).ToList();

            var lTrans = new Transporte_PosicaoCotista();

            for (int i = 0; i < lListaTemp.Count; i++)
            {
                lTrans = new Transporte_PosicaoCotista();

                //lTrans.NomeFundo        = pLista[i].Cod.CodigoAnbima == "" ? "Sem Código ANBIMA" : lFundo.NomeProduto;
                IntegracaoFundosInfo lFundo = new IntegracaoFundosDbLib().GetNomeRiscoFundo(lListaTemp[i].CodigoFundoItau.DBToInt32());

                Nullable<int> lCodigoFundoItau = new IntegracaoFundosDbLib().VerificaExistenciaFundoItau(lFundo.IdCodigoAnbima);

                if (lCodigoFundoItau == null) continue;

                lTrans.DataAtualizacao = "-";//lListaTemp[i].DataAtualizacao.Value.ToString("dd/MM/yyyy");
                lTrans.NomeFundo       = lListaTemp[i].NomeFundo;
                lTrans.Risco           = lFundo.Risco;
                lTrans.IR              = lListaTemp[i].IR.Value.ToString("N2");
                lTrans.QtdCotas        = lListaTemp[i].Quantidade.Value.ToString("N8");
                lTrans.ValorBruto      = lListaTemp[i].ValorBruto.Value.ToString("N8");
                lTrans.ValorCota       = lListaTemp[i].Cota.Value.ToString("N8");
                lTrans.ValorLiquido    = lListaTemp[i].ValorLiquido.Value.ToString("N8");
                lTrans.IOF             = lListaTemp[i].IOF.Value.ToString("N2");
                lTrans.IdFundo         = lFundo.IdProduto.ToString();
                lTrans.CodigoAnbima    = lFundo.IdCodigoAnbima;
                lTrans.Risco           = lFundo.Risco;

                lRetorno.Add(lTrans);
            }
            
            return lRetorno;
        }

        public List<FundoInfo> TraduzirLista(List<Transporte_PosicaoCotista> pLista)
        {
            var lRetorno = new List<FundoInfo>();

            List<Transporte_PosicaoCotista> lListaTemp = pLista.OrderByDescending(posicao => posicao.DataAtualizacao).ToList();

            Transporte_PosicaoCotista lTrans = null;

            for (int i = 0; i < lListaTemp.Count; i++)
            {
                lTrans = lListaTemp[i]; 

                var lFundo = new FundoInfo();

                lFundo.DataAtualizacao = lTrans.DataAtualizacao.DBToDateTime();
                lFundo.NomeFundo       = lTrans.NomeFundo;
                lFundo.IR              = lTrans.IR.DBToDecimal();
                lFundo.Quantidade      = lTrans.QtdCotas.DBToDecimal();
                lFundo.ValorBruto      = lTrans.ValorBruto.DBToDecimal();
                lFundo.Cota            = lTrans.ValorCota.DBToDecimal();
                lFundo.ValorLiquido    = lTrans.ValorLiquido.DBToDecimal();
                lFundo.IOF             = lTrans.IOF.DBToDecimal();
                lFundo.CodigoFundo     = lTrans.IdFundo.DBToInt32();
                lFundo.CodigoAnbima    = lTrans.CodigoAnbima;

                lRetorno.Add(lFundo);
            }

            return lRetorno;
        }
    }
}