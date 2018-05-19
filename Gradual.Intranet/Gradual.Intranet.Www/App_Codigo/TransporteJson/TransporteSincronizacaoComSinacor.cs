using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class GridSincronizacao 
    {
        /// <summary>
        /// Pendência Cadastral, Entrada no Sinacor, Atualização no Sistema de Cadastro, Exportação de Complementos
        /// </summary>
        public string Tipo { get; set; }
        /// <summary>
        /// Motivo do Erro
        /// </summary>
        public string Descricao { get; set; }

    }

    public class TransporteSincronizacaoComSinacor
    {
        #region Propriedades
        
        public string Resultado { get; set; }
        
        public List<GridSincronizacao> GridErro { get; set; }

        public List<string> Mensagens { get; set; }

        #endregion

        #region Construtor

        public TransporteSincronizacaoComSinacor()
        {
            this.Mensagens = new List<string>();
        }

        #endregion
    }
}