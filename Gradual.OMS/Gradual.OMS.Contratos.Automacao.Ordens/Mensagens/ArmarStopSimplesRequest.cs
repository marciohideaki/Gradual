using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Automacao.Ordens.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
namespace Gradual.OMS.Contratos.Automacao.Ordens.Mensagens
{
    public class ArmarStopSimplesRequest
    {
        public ArmarStopSimplesRequest()
        {
        }

        /// <summary>
        /// Código do cliente
        /// </summary>
        public string CodigoCliente { get; set; }

        /// <summary>
        /// Código do Sistema Cliente, por exemplo: HB (HomeBroker), Plataforma, Robo, etc.
        /// </summary>
        public string CodigoSistemaCliente { get; set; }

        public string Instrumento { get; set; }
        public string IdStopStart { get; set; }
        public int StopStartTipo { get; set; }
        public decimal PrecoGain { get; set; }
        public decimal PrecoLoss { get; set; }
        public decimal PrecoStart { get; set; }
        public decimal InicioMovel { get; set; }
        public decimal AjusteMovel { get; set; }
    }
}
