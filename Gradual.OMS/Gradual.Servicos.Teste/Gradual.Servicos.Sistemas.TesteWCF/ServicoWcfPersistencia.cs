using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.Servicos.Contratos.TesteWCF.Mensagens;
using Gradual.Servicos.Contratos.TesteWCF.Dados;

namespace Gradual.Servicos.Sistemas.TesteWCF
{
    public class ServicoWcfPersistencia
    {
        /// <summary>
        /// Referencia para o servico de persistencia
        /// </summary>
        private IServicoPersistencia _servicoPersistencia = Ativador.Get<IServicoPersistencia>();

        public SalvarMensagemDeTextoResponse SalvarMensagemDeTexto(SalvarMensagemDeTextoRequest parametros)
        {
                        // Salva
            _servicoPersistencia.SalvarObjeto<MensagemTextoInfo>(
                new SalvarObjetoRequest<MensagemTextoInfo>() 
                { 
                    Objeto = parametros.MensagemDeTexto
                });

            // Retorna
            return new SalvarMensagemDeTextoResponse();
        }

        public ReceberMensagemDeTextoResponse ReceberMensagemDeTexto(ReceberMensagemDeTextoRequest parametros)
        {
            try
            {
                // Recebe Objeto
                ReceberObjetoResponse<MensagemTextoInfo> lRes = _servicoPersistencia.ReceberObjeto<MensagemTextoInfo>(
                    new ReceberObjetoRequest<MensagemTextoInfo>()
                    {
                        CodigoObjeto = parametros.CodigoMensagemTexto
                    });

                // Retorna
                return new ReceberMensagemDeTextoResponse()
                    {
                        CodigoMensagemRequest = parametros.CodigoMensagem,
                        MensagemTexto = lRes.Objeto,
                        StatusResposta = MensagemResponseStatusEnum.OK
                    };
            }
            catch (Exception ex)
            {
                // Retorna
                return new ReceberMensagemDeTextoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    DescricaoResposta = ex.Message,
                    StatusResposta = MensagemResponseStatusEnum.ErroPrograma
                };
            }
        }

        public ListarMensagensDeTextoResponse ListarMensagensDeTexto(ListarMensagensDeTextoRequest parametros)
        {
            try
            {
                ConsultarObjetosResponse<MensagemTextoInfo> lRes = _servicoPersistencia.ConsultarObjetos<MensagemTextoInfo>(
                    new ConsultarObjetosRequest<MensagemTextoInfo>());
                return new ListarMensagensDeTextoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    MensagensTexto = lRes.Resultado,
                    StatusResposta = MensagemResponseStatusEnum.OK
                };
            }
            catch (Exception ex)
            {
                return new ListarMensagensDeTextoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    DescricaoResposta = ex.Message,
                    StatusResposta = MensagemResponseStatusEnum.ErroPrograma
                };
            }
        }
    }
}