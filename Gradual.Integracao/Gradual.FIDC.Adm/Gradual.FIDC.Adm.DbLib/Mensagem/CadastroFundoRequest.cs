using Gradual.OMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe que armazena informações de request referentes a cadastro de fundos
    /// </summary>
    public class CadastroFundoRequest : MensagemRequestBase
    {
        public int IdFundoCadastro { get; set; }
        public string NomeFundo { get; set; }
        public string CNPJFundo { get; set; }
        public string NomeAdministrador { get; set; }
        public string CNPJAdministrador { get; set; }
        public string NomeCustodiante { get; set; }
        public string CNPJCustodiante { get; set; }
        public string NomeGestor { get; set; }
        public string CNPJGestor { get; set; }
        public bool IsAtivo { get; set; }
        public decimal TxGestao { get; set; }
        public decimal TxConsultoria { get; set; }
        public decimal TxCustodia { get; set; }
        public bool Pertence { get; set; }
    }
}
