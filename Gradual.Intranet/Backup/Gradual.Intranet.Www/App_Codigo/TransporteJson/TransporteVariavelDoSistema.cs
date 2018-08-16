using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteVariavelDoSistema
    {
        #region Members
        public int Id { get; set; }

        public string Cofiguracao { get; set; }


        public string Valor { get; set;}
        #endregion
        #region Constructor
        public TransporteVariavelDoSistema() { }

        public TransporteVariavelDoSistema(ConfiguracaoInfo pInfo) 
        {
            this.Id = pInfo.IdConfiguracao;
            this.Valor = pInfo.Valor;
            this.Cofiguracao = pInfo.Configuracao.ToString();
        }

        #endregion
    }
}