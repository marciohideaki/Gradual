using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos
{
	public enum IntegracaoFundosStatusOperacaoEnum
	{
        SOLICITADO         = 1
        , CANCELADO         = 2
        , PROCESSAMENTO     = 3
        , EXECUTADO         = 4
        , RECUSADO          = 5
	}
}
