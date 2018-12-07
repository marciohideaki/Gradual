using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.DbLib.Dados.MinhaConta
{
    [Serializable]
    [DataContract]
    public class IntegracaoIRInfo
    {
        // <summary>
        /// Id. Clube [Código Bovespa].
        /// </summary>

        public Nullable<int> IdBovespa { get; set; }

        /// <summary>
        /// Codigo Clube [Código BMF].
        /// </summary>

        public Nullable<int> IdBMF { get; set; }

        public string Email { get; set; }

        public string Cidade { get; set; }
        public string Estado { get; set; }
        public DateTime? dataInicio { get; set; }
        public DateTime? dataFim { get; set; }
        public string EstadoBloqueado { get; set; }
        public CodigoEvento CdEvento { get; set; }
        public string Descricao { get; set; }
        public TipoProduto? TPProduto { get; set; }

        public enum TipoProduto
        {
            BOVESPA = 1,
            BMF = 2
        }

        public enum CodigoEvento
        {
            CADASTRO_NOVO = 1,
            ALTERACAO_CLIENTE = 2,
            DESMARCAR_INTEGRACAO = 3,
            CANCELAR = 4,
            ERRO = 99
        }

        public void RetornaCodigoEvento(int codigoEvento)
        {


            switch (codigoEvento)
            {
                case 1:
                    this.CdEvento = CodigoEvento.CADASTRO_NOVO;
                    break;
                case 2:
                    this.CdEvento = CodigoEvento.ALTERACAO_CLIENTE;
                    break;
                case 3:
                    this.CdEvento = CodigoEvento.DESMARCAR_INTEGRACAO;
                    break;
                case 4:
                    this.CdEvento = CodigoEvento.CANCELAR;
                    break;
                case 99:
                    this.CdEvento = CodigoEvento.ERRO;
                    break;
                default:
                    this.CdEvento = CodigoEvento.ERRO;
                    break;
            }



        }
    }
}
