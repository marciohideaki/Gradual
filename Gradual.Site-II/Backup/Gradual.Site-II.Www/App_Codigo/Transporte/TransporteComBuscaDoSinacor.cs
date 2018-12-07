using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Library;

namespace Gradual.Site.Www
{
    public class TransporteComBuscaDoSinacor
    {
        public string RecuperarDadoDoSinacor(TransporteSessaoClienteLogado pClienteBase, eInformacao pTipoInformacao, string pFiltro)
        {
            string lRetorno = string.Empty;

            ConsultarEntidadeCadastroRequest<SinacorListaComboInfo> lRequest = new ConsultarEntidadeCadastroRequest<SinacorListaComboInfo>();
            ConsultarEntidadeCadastroResponse<SinacorListaComboInfo> lResponse;

            lRequest = new ConsultarEntidadeCadastroRequest<SinacorListaComboInfo>()
            {
                IdUsuarioLogado = pClienteBase.IdLogin,

                DescricaoUsuarioLogado = pClienteBase.Nome,

                EntidadeCadastro = new SinacorListaComboInfo()
                {
                    Filtro = pFiltro,
                    Informacao = pTipoInformacao,
                }
            };

            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            lResponse = lServico.ConsultarEntidadeCadastro<SinacorListaComboInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado != null && lResponse.Resultado.Count > 0)
                    lRetorno = lResponse.Resultado[0].Value;
            }
            else
            {
                throw new Exception(lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

    }
}