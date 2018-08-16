using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados.Fundos;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.Intranet.Contratos.Dados;
using System.Data;

namespace Gradual.Intranet.Www.App_Codigo
{
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

        public string DataAplicacao { get; set; }

        //public string DataAtualizacao { get; set; }

        public List<Transporte_PosicaoCotista> TraduzirLista(PosicaoCotista.PosicaoCotistaViewModel[] pLista)
        {
            var lRetorno = new List<Transporte_PosicaoCotista>();

            for (int i = 0; i < pLista.Count(); i++)
            {
                var lTrans = new Transporte_PosicaoCotista();

                if (pLista[i].ValorBruto == 0) continue;

                if (pLista[i].CodigoAnbima == string.Empty) continue;

                IntegracaoFundosInfo lFundo = ClienteDbLib.GetNomeRiscoFundo(pLista[i].CodigoAnbima, 0);

                if (!string.IsNullOrEmpty(lFundo.CodigoFundoItau)) continue;

                lTrans.DataAtualizacao = pLista[i].DataConversao.ToString("dd/MM/yyyy");
                lTrans.NomeFundo       = pLista[i].CodigoAnbima == "" ? "Sem Código ANBIMA" : lFundo.NomeProduto;
                lTrans.IR              = pLista[i].ValorIR.ToString("N2");
                lTrans.QtdCotas        = pLista[i].Quantidade.ToString("N8");
                lTrans.Risco           = lFundo.Risco;
                lTrans.ValorBruto      = pLista[i].ValorBruto.ToString("N2");
                lTrans.ValorCota       = pLista[i].CotaDia.ToString("N8");
                lTrans.ValorLiquido    = pLista[i].ValorLiquido.ToString("N2");
                lTrans.CodigoAnbima    = pLista[i].CodigoAnbima;
                lTrans.IdFundo         = lFundo.IdProduto.ToString();
                lTrans.IOF             = pLista[i].ValorIOF.ToString("N2");
                lTrans.DataAplicacao   = pLista[i].DataAplicacao.ToString("dd/MM/yyyy");
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

                    lPosicao.ValorBruto += posicao.ValorBruto;
                    lPosicao.ValorIR += posicao.ValorIR;
                    lPosicao.ValorIOF += posicao.ValorIOF;
                    lPosicao.Quantidade += posicao.Quantidade;
                    lPosicao.ValorLiquido += posicao.ValorLiquido;

                    lListaTemp.Add(lPosicao);
                }
            }

            for (int i = 0; i < lListaTemp.Count(); i++)
            {
                var lTrans = new Transporte_PosicaoCotista();

                if (lListaTemp[i].ValorBruto == 0) continue;

                if (lListaTemp[i].CodigoAnbima == string.Empty) continue;

                IntegracaoFundosInfo lFundo =  ClienteDbLib.GetNomeRiscoFundo(lListaTemp[i].CodigoAnbima, 0);

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

        public List<Transporte_PosicaoCotista> TraduzirListaSumarizada(System.Data.DataTable pTable)
        {
            var lRetorno = new List<Transporte_PosicaoCotista>();
            DataTable lTable;

            var Rows = (from row in pTable.AsEnumerable()
                        orderby row["DataConversao"] descending
                        select row);
            lTable = Rows.AsDataView().ToTable();

            var lListaTemp = new List<AtlasPosicaoCotista>();
            
            foreach (DataRow lRow in lTable.Rows)
            {
                PosicaoFundo lPosicao = new PosicaoFundo(lRow);

                var posicaoTemp = lListaTemp.Find(pos => { return pos.IdCarteira == lPosicao.IdCarteira; });

                if (posicaoTemp == null)
                {
                    AtlasPosicaoCotista lPosicaoCotista = new AtlasPosicaoCotista();

                    lPosicaoCotista.IdPosicao               = lPosicao.IdPosicao               ;
                    lPosicaoCotista.IdOperacao              = lPosicao.IdOperacao              ;
                    lPosicaoCotista.IdCotista               = lPosicao.IdCotista               ;
                    lPosicaoCotista.IdCarteira              = lPosicao.IdCarteira              ;
                    lPosicaoCotista.Nome                    = lPosicao.NomeFundo               ;
                    lPosicaoCotista.ValorAplicacao          = lPosicao.ValorAplicacao          ;
                    lPosicaoCotista.DataAplicacao           = lPosicao.DataAplicacao           ;
                    lPosicaoCotista.DataConversao           = lPosicao.DataConversao           ;
                    lPosicaoCotista.CotaAplicacao           = lPosicao.CotaAplicacao           ;
                    lPosicaoCotista.CotaDia                 = lPosicao.CotaDia                 ;
                    lPosicaoCotista.ValorBruto              = lPosicao.ValorBruto              ;
                    lPosicaoCotista.ValorLiquido            = lPosicao.ValorLiquido            ;
                    lPosicaoCotista.QuantidadeInicial       = lPosicao.QuantidadeInicial       ;
                    lPosicaoCotista.Quantidade              = lPosicao.Quantidade              ;
                    lPosicaoCotista.QuantidadeBloqueada     = lPosicao.QuantidadeBloqueada     ;
                    lPosicaoCotista.DataUltimaCobrancaIR    = lPosicao.DataUltimaCobrancaIR    ;
                    lPosicaoCotista.ValorIR                 = lPosicao.ValorIR                 ;
                    lPosicaoCotista.ValorIOF                = lPosicao.ValorIOF                ;
                    lPosicaoCotista.ValorPerformance        = lPosicao.ValorPerformance        ;
                    lPosicaoCotista.ValorIOFVirtual         = lPosicao.ValorIOFVirtual         ;
                    lPosicaoCotista.QuantidadeAntesCortes   = lPosicao.QuantidadeAntesCortes   ;
                    lPosicaoCotista.ValorRendimento         = lPosicao.ValorRendimento         ;
                    lPosicaoCotista.PosicaoIncorporada      = lPosicao.PosicaoIncorporada      ;
                    lPosicaoCotista.CodigoAnbima            = lPosicao.CodigoAnbima            ;
                    lPosicaoCotista.DataUltimoCortePfee     = lPosicao.DataUltimoCortePfee     ;

                    lListaTemp.Add(lPosicaoCotista);
                }
                else
                {
                    AtlasPosicaoCotista lPosicaoCotista = new AtlasPosicaoCotista();

                    lListaTemp.Remove(posicaoTemp);

                    posicaoTemp.ValorBruto      += lPosicao.ValorBruto;
                    posicaoTemp.ValorIR         += lPosicao.ValorIR;
                    posicaoTemp.ValorIOF        += lPosicao.ValorIOF;
                    posicaoTemp.Quantidade      += lPosicao.Quantidade;
                    posicaoTemp.ValorLiquido    += lPosicao.ValorLiquido;

                    lListaTemp.Add(posicaoTemp);
                }
            }

            for (int i = 0; i < lListaTemp.Count(); i++)
            {
                var lTrans = new Transporte_PosicaoCotista();

                if (lListaTemp[i].ValorBruto == 0) continue;

                lTrans.DataAtualizacao  = lListaTemp[i].DataConversao.ToString("dd/MM/yyyy");
                lTrans.NomeFundo        = lListaTemp[i].Nome;
                lTrans.IR               = lListaTemp[i].ValorIR.ToString("N2");
                lTrans.QtdCotas         = lListaTemp[i].Quantidade.ToString("N8");
                //lTrans.Risco            = lFundo.Risco;
                lTrans.ValorBruto       = lListaTemp[i].ValorBruto.ToString("N2");
                lTrans.ValorCota        = lListaTemp[i].CotaDia.ToString("N8");
                lTrans.ValorLiquido     = lListaTemp[i].ValorLiquido.ToString("N2");
                lTrans.CodigoAnbima     = lListaTemp[i].CodigoAnbima;
                //lTrans.IdFundo          = lFundo.IdProduto.ToString();
                lTrans.IOF              = lListaTemp[i].ValorIOF.ToString("N2");

                lRetorno.Add(lTrans);

            }

            return lRetorno;
        }

        /// <summary>
        ///     Implementação parcial e simplificada da tradução da lista para eventual necessidade
        /// </summary>
        /// <param name="pLista"></param>
        /// <returns></returns>
        public List<FundosInfo> TraduzirListaFinancial(AtlasPosicaoCotista[] pLista)
        {
            var lRetorno = new List<FundosInfo>();

            for (int i = 0; i < pLista.Count(); i++)
            {
                var lTrans = new FundosInfo();

                if (pLista[i].ValorBruto == 0) continue;

                lTrans.DataAtualizacao  = pLista[i].DataConversao;
                lTrans.NomeFundo        = pLista[i].Nome;
                lTrans.IR               = pLista[i].ValorIR;
                lTrans.Quantidade       = pLista[i].Quantidade;
                lTrans.ValorBruto       = pLista[i].ValorBruto;
                lTrans.Cota             = pLista[i].CotaDia;
                lTrans.ValorLiquido     = pLista[i].ValorLiquido;
                lTrans.IOF              = pLista[i].ValorIOF;
                //lTrans.CodigoFundo = lFundo.IdProduto;

                lRetorno.Add(lTrans);

            }

            return lRetorno;
        }

        public List<Transporte_PosicaoCotista> TrauzirListaItau(List<ClienteFundosInfo> pLista)
        {
            var lRetorno = new List<Transporte_PosicaoCotista>();

            var lTrans = new Transporte_PosicaoCotista();

            for (int i = 0; i < pLista.Count; i++)
            {
                lTrans = new Transporte_PosicaoCotista();

                IntegracaoFundosInfo lFundo = ClienteDbLib.GetNomeRiscoFundo(pLista[i].CodigoFundoItau.DBToInt32());

                Nullable<int> lCodigoFundoItau =  ClienteDbLib.VerificaExistenciaFundoItau(lFundo.IdCodigoAnbima);

                if (lCodigoFundoItau == null) continue;

                lTrans.DataAtualizacao = pLista[i].DataAtualizacao.Value.ToString("dd/MM/yyyy");
                lTrans.NomeFundo       = pLista[i].NomeFundo;
                lTrans.IR              = pLista[i].IR.Value.ToString("N2");
                lTrans.QtdCotas        = pLista[i].Quantidade.Value.ToString("N2");
                lTrans.ValorBruto      = pLista[i].ValorBruto.Value.ToString("N2");
                lTrans.ValorCota       = pLista[i].Cota.Value.ToString("N2");
                lTrans.ValorLiquido    = pLista[i].ValorLiquido.Value.ToString("N2");
                lTrans.IOF             = pLista[i].IOF.Value.ToString("N2");
                lTrans.IdFundo         = lFundo.IdProduto.ToString();
                lTrans.CodigoAnbima    = lFundo.IdCodigoAnbima;
                lTrans.Risco           = lFundo.Risco;
                
                lRetorno.Add(lTrans);
            }

            return lRetorno;
        }
    }

    public class AtlasPosicaoCotista : PosicaoCotista.PosicaoCotistaViewModel
    {
        public string Nome { get; set; }
    }
}