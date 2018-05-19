using System;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class ParametrosDeBuscaOrdemStarStop: MensagemRequestBase
    {
        public DateTime DataReferenciaOrdemStop { get; set; }
        public string Instrumento { get; set; }
        public OpcoesBuscaStatusOrdem? StatusOrdem { get; set; }
        public string Login { get; set; }
        public OpcoesBuscarClientePor PesquisaClienteFiltro { get; set; }
        public string PesquisaClienteParametro { get; set; }
        public int? CodigoOrdem { get; set; }
    }
}
