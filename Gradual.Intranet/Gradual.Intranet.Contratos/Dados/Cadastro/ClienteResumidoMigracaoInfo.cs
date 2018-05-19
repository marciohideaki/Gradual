using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteResumidoMigracaoInfo : ICodigoEntidade
    {
        //#region | Propriedades Cliente Busca

        public int PaginaCorrente {get; set;}

        public int TotalRegistros {get; set; }

        public string TermoDeBusca { get; set; }

        //public OpcoesBuscarPor OpcaoBuscarPor { get; set; }

        //public OpcoesTipo OpcaoTipo { get; set; }

        //public OpcoesStatus OpcaoStatus { get; set; }

        //public OpcoesPasso OpcaoPasso { get; set; }

        //public OpcoesPendencia OpcaoPendencia { get; set; }

        public TipoDeConsultaClienteResumidoInfo TipoDeConsulta { get; set; }

        //#endregion

        #region | Propriedades Cliente Resumido

        public int IdCliente { get; set; }

        public System.Nullable<int> CodAssessor { get; set; }

        public string CodSistema { get; set; }

        public string CodGradual { get; set; }

        public string CodBovespa { get; set; }

        public string CodBovespaComConta { get; set; }

        public string CodBMF { get; set; }

        public string CodBMFComConta { get; set; }

        public string CodBovespaAtiva { get; set; }

        public string CodBMFAtiva { get; set; }

        public string NomeCliente { get; set; }

        public string CPF { get; set; }

        public string Status { get; set; }

        public string Passo { get; set; }

        public DateTime DataCadastro { get; set; }

        public string DataCadastroString { get { return this.DataCadastro.ToString("dd/MM/yyyy"); } }

        public DateTime DataRecadastro { get; set; }

        public string DataRecadastroString { get { return this.DataRecadastro.ToString("dd/MM/yyyy"); } }

        public string FlagPendencia { get; set; }

        public DateTime DataNascimento { get; set; }

        public string DataNascimentoString { get { return this.DataNascimento.ToString("dd/MM/yyyy"); } }

        public string Email { get; set; }

        public string Sexo { get; set; }

        public string Cise { get; set; }

        /// <summary>
        /// PF: Pessoa Física, PJ: Pessoa Jurídica
        /// </summary>
        public string TipoCliente { get; set; }

        #endregion

        #region | Construtor

        public ClienteResumidoMigracaoInfo()
        {
            this.TipoDeConsulta = TipoDeConsultaClienteResumidoInfo.Clientes;

            //this.OpcaoBuscarPor = OpcoesBuscarPor.NomeCliente;

            //this.OpcaoTipo = OpcoesTipo.ClientePF | OpcoesTipo.ClientePJ;
            //this.OpcaoStatus = OpcoesStatus.Ativo | OpcoesStatus.Inativo;
            //this.OpcaoPasso = OpcoesPasso.Cadastrado | OpcoesPasso.Exportado | OpcoesPasso.Visitante;
            //this.OpcaoPendencia = OpcoesPendencia.ComPendenciaCadastral | OpcoesPendencia.ComSolicitacaoAlteracao;
        }

        public ClienteResumidoMigracaoInfo(string pTermo)
            : this()
        {
            this.TermoDeBusca = pTermo;
        }        

        public string  ReceberCodigo()
        {
 	        throw new NotImplementedException();
        }
    

        #endregion

        #region Métodos Públicos

        public void VerificarTiposDeContas(List<ClienteContaInfo> pContas)
        {
            foreach (ClienteContaInfo item in pContas)
            {
                if (this.CodBovespa == item.CdCodigo.ToString() && item.CdSistema == eAtividade.BOL )
                {
                    if (item.StContaInvestimento)
                    this.CodBovespaComConta = this.CodBovespa.ToCodigoClienteFormatado() + "-CI";
                    else
                        this.CodBovespaComConta = this.CodBovespa.ToCodigoClienteFormatado();
                }

                if (this.CodBMF == item.CdCodigo.ToString() && item.CdSistema == eAtividade.BMF  )
                {
                    if (item.StContaInvestimento)
                        this.CodBMFComConta = this.CodBMF.ToCodigoClienteFormatado() + "-CI";
                    else
                        this.CodBMFComConta = this.CodBMF.ToCodigoClienteFormatado();
                }

            }
        }


        #endregion
    }
}
