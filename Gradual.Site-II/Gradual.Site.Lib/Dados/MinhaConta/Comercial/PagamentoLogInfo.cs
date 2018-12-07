using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.Lib.Dados.MinhaConta.Comercial
{
    /// <summary>
    /// Log de comunicações com o gateway de pagamento
    /// </summary>
    public class PagamentoLogInfo
    {
        #region Propriedades

        public int IdPagamentoLog { get; set; }

        public DateTime Data { get; set; }

        public string ReferenciaDaTransacao { get; set; }
        
        public string ReferenciaDaVenda { get; set; }

        /// <summary>
        /// Direção de envio: "E" para envio (Gradual -> Gateway), "R" para recebimento (Gateway -> Gradual)
        /// </summary>
        public string Direcao { get; set; }

        public string Mensagem { get; set; }

        /// <summary>
        /// Conteúdo total do XML que foi enviado/recebido
        /// </summary>
        public string ConteudoXML { get; set; }

        #endregion

        #region Construtores

        public PagamentoLogInfo()
        {
            this.Data = DateTime.Now;
        }
        
        public PagamentoLogInfo(string pDirecao, string pReferenciaDaTransacao, string pReferenciaDaVenda, string pMensagem, string pConteudoXML) : this()
        {
            this.Direcao = pDirecao;
            this.ReferenciaDaTransacao = pReferenciaDaTransacao;
            this.ReferenciaDaVenda = pReferenciaDaVenda;
            this.Mensagem = pMensagem;
            this.ConteudoXML = pConteudoXML;
        }

        #endregion
    }
}
