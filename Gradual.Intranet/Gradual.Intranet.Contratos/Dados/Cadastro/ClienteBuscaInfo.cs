using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteBuscaInfos : ICodigoEntidade
    {
        
        #region | Propriedades Cliente Busca

        public string TermoDeBusca { get; set; }

        public OpcoesBuscarPor OpcaoBuscarPor { get; set; }

        public OpcoesTipo OpcaoTipo { get; set; }

        public OpcoesStatus OpcaoStatus { get; set; }

        public OpcoesPasso OpcaoPasso { get; set; }

        public OpcoesPendencia OpcaoPendencia { get; set; }

        public List<ClienteResumidoInfo> ClienteResumidoInfo { get; set; }

        #endregion

        #region | Propriedades Cliente Resumido

        public double IdCliente { get; set; }

        public string CodGradual { get; set; }

        public string CodBovespa { get; set; }

        public string CodBMF { get; set; }

        public string NomeCliente { get; set; }

        public double CPF { get; set; }

        public string Status { get; set; }

        public string Passo { get; set; }

        public DateTime DataCadastro { get; set; }

        public string DataCadastroString { get { return this.DataCadastro.ToString("dd/MM/yyyy"); } }

        public string FlagPendencia { get; set; }

        public DateTime DataNascimento { get; set; }

        public string DataNascimentoString { get { return this.DataNascimento.ToString("dd/MM/yyyy"); } }

        public string Email { get; set; }

        public string Sexo { get; set; }

        /// <summary>
        /// PF: Pessoa Física, PJ: Pessoa Jurídica
        /// </summary>
        public string Tipo { get; set; }

        /*  public List<TelefoneInfo> Telefones { get; set; }

        public string TelefonesString
        {
            get
            {
                string lRetorno = "";

                foreach (TelefoneInfo lTelefone in this.Telefones)
                {
                    lRetorno += string.Format("{0}\r\n", lTelefone.NumeroFormatado);
                }

                return lRetorno;
            }
        }*/

        #endregion

        #region | Construtores

        public ClienteBuscaInfos()
        {
            this.OpcaoBuscarPor = OpcoesBuscarPor.NomeCliente;

            this.OpcaoTipo = OpcoesTipo.ClientePF | OpcoesTipo.ClientePJ;
            this.OpcaoStatus = OpcoesStatus.Ativo | OpcoesStatus.Inativo;
            this.OpcaoPasso = OpcoesPasso.Cadastrado | OpcoesPasso.Exportado | OpcoesPasso.Visitante;
            this.OpcaoPendencia = OpcoesPendencia.ComPendenciaCadastral | OpcoesPendencia.ComSolicitacaoAlteracao;
        }

        public ClienteBuscaInfos(string pTermo)
            : this()
        {
            this.TermoDeBusca = pTermo;
        }

        #endregion

        #region ICodigoEntidade Members

        string ICodigoEntidade.ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
