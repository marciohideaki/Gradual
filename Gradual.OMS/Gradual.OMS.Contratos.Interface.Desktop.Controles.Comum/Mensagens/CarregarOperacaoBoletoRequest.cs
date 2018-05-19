using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Mensagens
{
    /// <summary>
    /// Mensagem para solicitar ao controle OperacaoBoleto carregar
    /// informações na tela
    /// </summary>
    public class CarregarOperacaoBoletoRequest : MensagemInterfaceRequestBase
    {
        public string Bolsa { get; set; }
        public string Cliente { get; set; }
        public string Papel { get; set; }
        public double? Quantidade { get; set; }
        public double? Preco { get; set; }
        public OrdemDirecaoEnum? Direcao { get; set; }
        public OrdemTipoEnum? Tipo { get; set; }
        public OrdemValidadeEnum? Validade { get; set; }
        public bool AtivarJanela { get; set; }
    }
}
