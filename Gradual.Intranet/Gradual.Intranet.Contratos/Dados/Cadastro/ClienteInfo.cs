using System;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteInfo : ICodigoEntidade
    {
        #region | Propriedades

        public Nullable<int> IdCliente { get; set; }
        public string DsNome { get; set; }
        public Nullable<int> IdLogin { get; set; }
        public DateTime DtUltimaAtualizacao { get; set; }
        public string DsCpfCnpj { get; set; }
        public DateTime DtPasso1 { get; set; }
        public Nullable<DateTime> DtPasso2 { get; set; }
        public Nullable<DateTime> DtPasso3 { get; set; }
        public Nullable<DateTime> DtPrimeiraExportacao { get; set; }
        public Nullable<DateTime> DtUltimaExportacao { get; set; }
        public string DsOrigemCadastro { get; set; }
        public char TpPessoa { get; set; }
        public int TpCliente { get; set; }
        public int StPasso { get; set; }
        public Nullable<char> CdSexo { get; set; }
        public Nullable<int> CdNacionalidade { get; set; }
        public string CdPaisNascimento { get; set; }
        public string CdUfNascimento { get; set; }
        public string DsUfnascimentoEstrangeiro { get; set; }
        public Nullable<int> CdEstadoCivil { get; set; }
        public string DsConjugue { get; set; }
        public string TpDocumento { get; set; }
        public Nullable<DateTime> DtNascimentoFundacao { get; set; }
        public string CdOrgaoEmissorDocumento { get; set; }
        public string CdUfEmissaoDocumento { get; set; }
        public string CodigoSegurancaCNH { get; set; }
        public Nullable<int> CdProfissaoAtividade { get; set; }
        /// <summary>
        /// 0: Não é vinculada; 1: vinculada a outras corretoras; 2: vinculada à gradual
        /// </summary>
        public int StPessoaVinculada { get; set; }
        public string DsEmpresa { get; set; }
        public Nullable<Boolean> StPPE { get; set; }
        public Nullable<Boolean> StCarteiraPropria { get; set; }
        public string DsAutorizadoOperar { get; set; }
        public Nullable<Boolean> StCVM387 { get; set; }
        public Nullable<Boolean> StEmancipado { get; set; }
        public Nullable<int> IdAssessorInicial { get; set; }
        public Nullable<int> CdEscolaridade { get; set; }
        public Nullable<Boolean> StCadastroPortal { get; set; }
        public string DsNomeFantasia { get; set; }
        public Nullable<Int64> CdNire { get; set; }
        public string DsFormaConstituicao { get; set; }
        public Nullable<Boolean> StInterdito { get; set; }
        public Nullable<Boolean> StSituacaoLegalOutros { get; set; }
        public string DsNumeroDocumento { get; set; }
        public int CdAtividadePrincipal { get; set; }
        public string DsEmail { get; set; }
        public string DsEmailComercial { set; get; }
        public string CdSenha { get; set; }
      
        public string DsCargo { get; set; } //Antigo CdCargo Varchar(3) Agora DsCargo Varchar(40)
        public string DsNomePai { get; set; } //Varchar(60)
        public string DsNomeMae { get; set; } //Varchar(60)
        public Nullable<DateTime> DtEmissaoDocumento { get; set; } //Date
        public string DsNaturalidade { get; set; } //Varchar(20)

        public string NrInscricaoEstadual { get; set; }
        //  public string DsEmailComercial { get; set; }
        public bool StAtivo { get; set; }
        public Nullable<DateTime> DtAtivacaoInativacao { get; set; }
        public string DsSenhaGerada { get; set; }
        
        public string DsComoConheceu { get; set; }
        
        public Nullable<Boolean> StUSPerson { get; set; }
        
        public Nullable<Int16> StCienteDocumentos { get; set; }

        public string DsPropositoGradual { get; set; }

        /// <summary>
        /// Como os campos dos detalhes de "USPerson" são muitos e raramente serão usados, deixarei como JSON no banco
        /// </summary>
        public string DsUSPersonPJDetalhes { get; set; }

        /// <summary>
        /// 'BOVESPA', 'FUNDOS', 'CAMBIO', 'AMBOS'
        /// </summary>
        public string TpDesejaAplicar { get; set; }

        public ClienteNaoOperaPorContaPropriaInfo DadosClienteNaoOperaPorContaPropria { get; set; }

        public bool NovoHB { get; set; }

        #endregion

        #region | Construtor

        public ClienteInfo() { }
        
        public ClienteInfo(string pId) 
        {
            this.IdCliente = int.Parse(pId);
        }

        #endregion

        #region | ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
