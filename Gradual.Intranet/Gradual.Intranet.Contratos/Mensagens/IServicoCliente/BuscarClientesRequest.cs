using System;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class BuscarClientesRequest : BaseRequest
    {

        #region Propriedades

        public string TermoDeBusca { get; set; }

        public OpcoesBuscarPor OpcaoBuscarPor { get; set; }

        public OpcoesTipo OpcaoTipo { get; set; }

        public OpcoesStatus OpcaoStatus { get; set; }

        public OpcoesPasso OpcaoPasso { get; set; }

        public OpcoesPendencia OpcaoPendencia { get; set; }

        #endregion

        #region Construtor

        public BuscarClientesRequest()
        {
            this.OpcaoBuscarPor = OpcoesBuscarPor.NomeCliente;

            this.OpcaoTipo = OpcoesTipo.ClientePF | OpcoesTipo.ClientePJ;
            this.OpcaoStatus = OpcoesStatus.Ativo | OpcoesStatus.Inativo;
            this.OpcaoPasso = OpcoesPasso.Cadastrado | OpcoesPasso.Exportado | OpcoesPasso.Visitante;
            this.OpcaoPendencia = OpcoesPendencia.ComPendenciaCadastral | OpcoesPendencia.ComSolicitacaoAlteracao;
        }

        public BuscarClientesRequest(string pTermo) : this()
        {
            this.TermoDeBusca = pTermo;
        }

        #endregion
    }
}
