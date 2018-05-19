using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Configuração para o cliente achar o localizador
    /// </summary>
    public class LocalizadorClienteConfig
    {
        public ServicoEndPointInfo EndPoint { get; set; }
        public ServicoAtivacaoTipo AtivacaoTipo { get; set; }
        public bool ManterConexao { get; set; }

        public LocalizadorClienteConfig()
        {
            this.AtivacaoTipo = ServicoAtivacaoTipo.Local;
        }
    }
}
