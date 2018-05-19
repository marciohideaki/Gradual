using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.PoupeDirect.Lib.Dados;
using System.Collections;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Solicitacoes
{
    public class TransporteAplicacaoResgate
    {
        #region | Propriedades

        public string CodigoCliente { get; set; }

        public string CodigoTabela { get; set; }

        public string CodigoProduto { get; set; }

        public string Descricao { get; set; }

        public string CodigoStatus { get; set; }

        public string DescricaoProduto { get; set; }

        public string DescricaoStatus { get; set; }

        public IEnumerable Itens { get; set; }

        public IEnumerable Itens2 { get; set; }

        public string DtEfetivacao { get; set; }

        public string DtSolicitacao { get; set; }

        public string ValorSolicitado { get; set; }

        #endregion

        #region | Construtores

        public TransporteAplicacaoResgate() { }

        #endregion


        public TransporteAplicacaoResgate(IEnumerable pItens, IEnumerable pItens2)
            : this()
        {
            this.Itens = pItens;

            this.Itens2 = pItens2;
        }

        

        public List<TransporteAplicacaoResgate> ToListTransporteResultadoAplicacaoResgate(List<AplicacaoInfo> ListaAplicacao)
        {
            var lRetorno = new List<TransporteAplicacaoResgate>();

            ListaAplicacao.ForEach(delegate(AplicacaoInfo aplicacaoInfo)
            {
                lRetorno.Add(new TransporteAplicacaoResgate()
                {
                    CodigoCliente = aplicacaoInfo.CodigoCliente.ToString(),
                    CodigoTabela = aplicacaoInfo.CodigoAplicacao.ToString(),
                    Descricao = aplicacaoInfo.DescricaoAplicacao,
                    CodigoProduto = aplicacaoInfo.CodigoProduto.ToString(),
                    DescricaoProduto = aplicacaoInfo.DescricaoProduto,
                    CodigoStatus = aplicacaoInfo.CodigoStatus.Value.ToString(),
                    DescricaoStatus =  aplicacaoInfo.DescricaoStatus,
                    DtEfetivacao = aplicacaoInfo.DtEfetivacao.Value.ToString("dd/MM/yyyy"),
                    DtSolicitacao =aplicacaoInfo.DtSolicitacao.Value.ToString("dd/MM/yyyy"),
                    ValorSolicitado = aplicacaoInfo.ValorSolicitado.ToString("N2"),
                    
                });
            });

            return lRetorno;
        }


        public List<TransporteAplicacaoResgate> ToListTransporteResultadoAplicacaoResgate(List<ResgateInfo> ListaAplicacao)
        {
            var lRetorno = new List<TransporteAplicacaoResgate>();

            ListaAplicacao.ForEach(delegate(ResgateInfo aplicacaoInfo)
            {
                lRetorno.Add(new TransporteAplicacaoResgate()
                {
                    CodigoCliente = aplicacaoInfo.CodigoCliente.ToString(),
                    CodigoTabela = aplicacaoInfo.CodigoResgate.ToString(),
                    Descricao = aplicacaoInfo.DescricaoResgate,
                    CodigoProduto = aplicacaoInfo.CodigoProduto.ToString(),
                    DescricaoProduto = aplicacaoInfo.DescricaoProduto,
                    CodigoStatus = aplicacaoInfo.CodigoStatus.Value.ToString(),
                    DescricaoStatus = aplicacaoInfo.DescricaoStatus,
                    DtEfetivacao = aplicacaoInfo.DtEfetivacao.Value.ToString("dd/MM/yyyy"),
                    DtSolicitacao = aplicacaoInfo.DtSolicitacao.Value.ToString("dd/MM/yyyy"),
                    ValorSolicitado = aplicacaoInfo.ValorSolicitado.ToString("N2"),

                });
            });

            return lRetorno;
        }
    }
}