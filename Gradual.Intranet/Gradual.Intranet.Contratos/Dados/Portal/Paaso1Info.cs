using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class Passo1Info : ICodigoEntidade
    {

        //Retorno
        public int IdCliente { get; set; }

        //Login
        public string DsEmail { get; set; }
        public string CdSenha { get; set; }
        public string CdAssinaturaEletronica { get; set; } 
        
        //Cliente
        public string   DsCpfCnpj                   { get; set; }
        public DateTime DtNascimentoFundacao        { get; set; }
        public string   DsNome                      { get; set; }
        public int      IdAssessorInicial           { get; set; }
        public string   CdSexo                      { get; set; }
        public string   TpPessoa                    { get { return "F"; }}
        public int      TpCliente                   { get  {return 1; }}
        public string   ComoConheceu                { get; set; }
        public int      CodigoTipoOperacaoCliente   { get; set; }
        
        //Celular
        public string DsCelNumero { get; set; }
        public string DsCelDdd { get; set; }

        //Telefone
        public int IdTipoTelefone { get; set; }
        public string DsNumero { get; set; }
        public string DsDdd { get; set; }
        public string DsRamal { get; set; }
        //public int StPrincipal { get { return 1; }}



        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
