using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gradual.OMS.Library;
using Gradual.FIDC.Adm.DbLib.Dados;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    public class AlteracaoRegulamentacaoCarregarDadosModalEnvioEmailResponse : MensagemResponseBase
    {
        public List<ConsultaFundosConstituicaoInfo> ListaConsultaFundos { get; set; }
    }
}
