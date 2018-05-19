using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {

        public static SalvarEntidadeResponse<ReservaIPOInfo> EfetuarLogDeReservaDeIPO(SalvarObjetoRequest<ReservaIPOInfo> pParametros)
        {
            LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.EfetuarIPO);
            return new SalvarEntidadeResponse<ReservaIPOInfo>();
        }


    }
}
