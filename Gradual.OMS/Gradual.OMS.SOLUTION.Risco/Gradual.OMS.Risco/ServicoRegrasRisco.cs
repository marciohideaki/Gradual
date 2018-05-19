using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Risco.RegraLib;
using Gradual.OMS.Risco.RegraLib.Mensagens;
using Gradual.OMS.Risco.Regra.Persistencia;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Risco.RegraLib.Dados;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Library;

namespace Gradual.OMS.Risco.Regra
{
    public class ServicoRegrasRisco : IServicoRegrasRisco
    {

        PersistenciaRegraDB gPersistencia = new PersistenciaRegraDB(); //Ativador.Get<IServicoPersistencia>();

        #region IServicoRiscoPersistencia Members

        public SalvarParametroRiscoClienteResponse SalvarParametroRiscoCliente(SalvarParametroRiscoClienteRequest lRequest)
        {
            SalvarParametroRiscoClienteResponse lResponse = new SalvarParametroRiscoClienteResponse()
                {
                    CodigoMensagemRequest = lRequest.CodigoMensagem,
                };
            try
            {
                SalvarObjetoResponse<ParametroRiscoClienteInfo> lRes = gPersistencia.SalvarObjeto<ParametroRiscoClienteInfo>(new SalvarObjetoRequest<ParametroRiscoClienteInfo>()
                {
                    Objeto = lRequest.ParametroRiscoCliente
                });

                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
                lResponse.ParametroRiscoCliente = lRes.Objeto;
            }
            catch (Exception ex)
            {
                lResponse.DescricaoResposta = ex.Message;
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lResponse;
        }

        public ListarParametrosRiscoResponse ListarParametrosRisco(ListarParametrosRiscoRequest lRequest)
        {

            ListarParametrosRiscoResponse lResponse = new ListarParametrosRiscoResponse();
            lResponse.CodigoMensagemRequest = lRequest.CodigoMensagem;
            List<CondicaoInfo> lCOndicoes = new List<CondicaoInfo>();
            CondicaoInfo ci1 = new CondicaoInfo("@id_bolsa", CondicaoTipoEnum.Igual, (int)lRequest.Bolsa);
            lCOndicoes.Add(ci1);
            if (lRequest.FiltroNomeParamertro != null && lRequest.FiltroNomeParamertro != string.Empty)
            {
                CondicaoInfo ci2 = new CondicaoInfo("@dscr_parametro", CondicaoTipoEnum.Igual, lRequest.FiltroNomeParamertro);
                lCOndicoes.Add(ci2);
            }

            try
            {
                ConsultarObjetosResponse<ParametroRiscoInfo> lRes = gPersistencia.ConsultarObjetos<ParametroRiscoInfo>(new ConsultarObjetosRequest<ParametroRiscoInfo>()
                {
                    Condicoes = lCOndicoes
                });

                lResponse.ParametrosRisco = lRes.Resultado;
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = ex.Message;
            }
            return lResponse;
        }

        public ListarParametrosRiscoClienteResponse ListarParametrosRiscoCliente(ListarParametrosRiscoClienteRequest lRequest)
        {
            ListarParametrosRiscoClienteResponse lResponse = new ListarParametrosRiscoClienteResponse();
            lResponse.CodigoMensagemRequest = lRequest.CodigoMensagem;
            List<CondicaoInfo> lCOndicoes = new List<CondicaoInfo>();
            CondicaoInfo ci = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, lRequest.CodigoCliente);
            
            lCOndicoes.Add(ci);

            try
            {
                ConsultarObjetosResponse<ParametroRiscoClienteInfo> lRes 
                    = gPersistencia.ConsultarObjetos<ParametroRiscoClienteInfo>(
                    new ConsultarObjetosRequest<ParametroRiscoClienteInfo>()
                    {
                        Condicoes = lCOndicoes
                    });

                lResponse.ParametrosRiscoCliente = lRes.Resultado;
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = ex.Message;
            }
            return lResponse;
        }

        public ReceberParametroRiscoClienteResponse ReceberParametroRiscoCliente(ReceberParametroRiscoClienteRequest lRequest)
        {
            ReceberParametroRiscoClienteResponse lResponse = new ReceberParametroRiscoClienteResponse();
            lResponse.CodigoMensagemRequest = lRequest.CodigoMensagem;

            try
            {
                ReceberObjetoResponse<ParametroRiscoClienteInfo> lRes
                    = gPersistencia.ReceberObjeto<ParametroRiscoClienteInfo>(
                    new ReceberObjetoRequest<ParametroRiscoClienteInfo>()
                    {
                        CodigoObjeto = lRequest.CodigoParametroRiscoCliente.ToString()
                    });

                lResponse.ParametroRiscoCliente = lRes.Objeto;
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = ex.Message;
            }
            return lResponse;
        }

        public ListarPermissoesRiscoResponse ListarPermissoesRisco(ListarPermissoesRiscoRequest lRequest)
        {
            ListarPermissoesRiscoResponse lResponse = new ListarPermissoesRiscoResponse();
            lResponse.CodigoMensagemRequest = lRequest.CodigoMensagem;
            List<CondicaoInfo> lCOndicoes = new List<CondicaoInfo>();
            CondicaoInfo ci = new CondicaoInfo("@id_bolsa", CondicaoTipoEnum.Igual, (int)lRequest.Bolsa);
            lCOndicoes.Add(ci);
            if (lRequest.FiltroNomePermissao != null && lRequest.FiltroNomePermissao != string.Empty)
            {
                CondicaoInfo ci2 = new CondicaoInfo("@dscr_permissao", CondicaoTipoEnum.Igual, lRequest.FiltroNomePermissao);
                lCOndicoes.Add(ci2);
            }

            try
            {
                ConsultarObjetosResponse<PermissaoRiscoInfo> lRes = gPersistencia.ConsultarObjetos<PermissaoRiscoInfo>(new ConsultarObjetosRequest<PermissaoRiscoInfo>()
                {
                    Condicoes = lCOndicoes
                });

                lResponse.Permissoes = lRes.Resultado;
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = ex.Message;
            }
            return lResponse;
        }

        public ListarGruposResponse ListarGrupos(ListarGruposRequest lRequest)
        {
            ListarGruposResponse lResponse = new ListarGruposResponse();
            lResponse.CodigoMensagemRequest = lRequest.CodigoMensagem;

            try
            {
                ConsultarObjetosResponse<GrupoInfo> lRes = 
                    gPersistencia.ConsultarObjetos<GrupoInfo>(new ConsultarObjetosRequest<GrupoInfo>());

                lResponse.Grupos = lRes.Resultado;
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = ex.Message;
            }
            return lResponse;
        }

        #endregion

        #region IServicoRegrasRisco Members


        public ReceberParametroRiscoResponse ReceberParametroRisco(ReceberParametroRiscoRequest lRequest)
        {
            ReceberParametroRiscoResponse lRes = new ReceberParametroRiscoResponse();
            try
            {
                ReceberObjetoRequest<ParametroRiscoInfo> lReqPar = new ReceberObjetoRequest<ParametroRiscoInfo>()
                {
                    CodigoObjeto = lRequest.CodigoParametro.ToString()
                };

                lRes.ParametroRisco = gPersistencia.ReceberObjeto<ParametroRiscoInfo>(lReqPar).Objeto;
            }
            catch (Exception ex)
            {
                lRes.DescricaoResposta = ex.Message;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRes;
        }

        public ReceberPermissaoRiscoResponse ReceberPermissaoRisco(ReceberPermissaoRiscoRequest lRequest)
        {
            ReceberPermissaoRiscoResponse lRes = new ReceberPermissaoRiscoResponse();
            try
            {
                ReceberObjetoRequest<PermissaoRiscoInfo> lReqPar = new ReceberObjetoRequest<PermissaoRiscoInfo>()
                {
                    CodigoObjeto = lRequest.CodigoPermissao.ToString()
                };

                lRes.PermissaoRisco = gPersistencia.ReceberObjeto<PermissaoRiscoInfo>(lReqPar).Objeto;
            }
            catch (Exception ex)
            {
                lRes.DescricaoResposta = ex.Message;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRes;
        }

        public ReceberGrupoResponse ReceberGrupo(ReceberGrupoRequest lRequest)
        {
            ReceberGrupoResponse lRes = new ReceberGrupoResponse();
            try
            {
                ReceberObjetoRequest<GrupoInfo> lReqPar = new ReceberObjetoRequest<GrupoInfo>()
                {
                    CodigoObjeto = lRequest.CodigoGrupo.ToString()
                };

                lRes.Grupo = gPersistencia.ReceberObjeto<GrupoInfo>(lReqPar).Objeto;
            }
            catch (Exception ex)
            {
                lRes.DescricaoResposta = ex.Message;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRes;
        }

        #endregion

        #region IServicoRegrasRisco Members


        public SalvarGrupoResponse SalvarGrupo(SalvarGrupoRequest lRequest)
        {
            SalvarGrupoResponse lRes = new SalvarGrupoResponse();
            try
            {
                SalvarObjetoRequest<GrupoInfo> lReqPar = new SalvarObjetoRequest<GrupoInfo>()
                {
                    Objeto = lRequest.Grupo
                };

                lRes.Grupo = gPersistencia.SalvarObjeto<GrupoInfo>(lReqPar).Objeto;
            }
            catch (Exception ex)
            {
                lRes.DescricaoResposta = ex.Message;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRes;
        }

        #endregion

        #region IServicoRegrasRisco Members


        public SalvarGrupoItemResponse SalvarGrupoItem(SalvarGrupoItemRequest lRequest)
        {
            SalvarGrupoItemResponse lRes = new SalvarGrupoItemResponse();
            try
            {
                SalvarObjetoRequest<GrupoItemInfo> lReqPar = new SalvarObjetoRequest<GrupoItemInfo>();
                lReqPar.Objeto = lRequest.GruopItem;
                lRes.GruopItem = gPersistencia.SalvarObjeto<GrupoItemInfo>(lReqPar).Objeto;
            }
            catch (Exception ex)
            {
                lRes.DescricaoResposta = ex.Message;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRes;
        }

        #endregion

        #region IServicoRegrasRisco Members


        public RemoverGrupoItemResponse RemoverGrupoItem(RemoverGrupoItemRequest lRequest)
        {
            RemoverGrupoItemResponse lRes = new RemoverGrupoItemResponse();
            try
            {
                RemoverObjetoRequest<GrupoItemInfo> lReqPar = new RemoverObjetoRequest<GrupoItemInfo>();
                lReqPar.CodigoObjeto = lRequest.CodigoGrupoItem.ToString();
                RemoverObjetoResponse<GrupoItemInfo> lResRem = gPersistencia.RemoverObjeto<GrupoItemInfo>(lReqPar);
                lRes.StatusResposta = MensagemResponseStatusEnum.OK;
                lRes.DescricaoResposta = "Item excluido com sucesso.";
            }
            catch (Exception ex)
            {
                lRes.DescricaoResposta = ex.Message;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRes;
        }

        #endregion
    }
}
