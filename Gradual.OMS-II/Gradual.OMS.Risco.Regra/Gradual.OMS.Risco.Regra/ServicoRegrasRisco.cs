using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.ServiceModel;
using Gradual.Generico.Dados;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Risco.Regra.Lib;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;
using Gradual.OMS.Risco.Regra.Persistencia;
using Gradual.OMS.Risco.Regra.Persistencia.Entidades;

namespace Gradual.OMS.Risco.Regra
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ServicoRegrasRisco : IServicoRegrasRisco
    {
        PersistenciaRegraDB gPersistencia = new PersistenciaRegraDB(); //Ativador.Get<IServicoPersistencia>();

        ////ReceberParametroRiscoClienteResponse ReceberParametroRiscoCliente
        //public RemoverPermissaoRiscoResponse RemoverClienteParametro(RemoverPermissaoRiscoRequest pParametro)
        //{
        //    var lRetorno = new RemoverPermissaoRiscoResponse();
        //    var lAcessaDados = new AcessaDados();

        //    lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

        //    try
        //    {
        //        //LOG
        //        ReceberAssociacao(new ReceberClientePermissaoParametroRequest() { DescricaoUsuarioLogado = pParametro.DescricaoUsuarioLogado, IdUsuarioLogado = pParametro.IdUsuarioLogado }, true);

        //        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_listaparametros_del"))
        //        {
        //            lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, 0);//Estava pegando o Id do Usuaio logado...
        //            lAcessaDados.AddInParameter(lDbCommand, "@id_clientepermissao_lista", DbType.String, pParametro.ListaIdClientePermissao);

        //            lAcessaDados.ExecuteNonQuery(lDbCommand);
        //        }
        //        LogRisco.Logar(pParametro, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogRisco.eAcao.Excluir);
        //        lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogRisco.Logar(pParametro, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogRisco.eAcao.Excluir, ex);
        //        lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
        //        lRetorno.DescricaoResposta = ex.ToString();
        //    }


        //    return lRetorno;
        //}

        #region Listar

        public ListarParametrosRiscoClienteResponse ListarLimitePorCliente(ListarParametrosRiscoClienteRequest pParametro)
        {
            var lRetorno = new ListarParametrosRiscoClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.CodigoCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.ParametrosRiscoCliente.Add(
                                new ParametroRiscoClienteInfo()
                                {
                                    CodigoCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                    DataValidade = lDataTable.Rows[i]["dt_validade"].DBToDateTime(),
                                    IdBolsa = lDataTable.Rows[i]["id_bolsa"].DBToInt32(),
                                    StAtivo = lDataTable.Rows[i]["st_ativo"].DBToChar(),
                                    Valor = lDataTable.Rows[i]["vl_parametro"].DBToDecimal(),
                                    Parametro = new ParametroRiscoInfo()
                                    {
                                        CodigoParametro = lDataTable.Rows[i]["id_parametro"].DBToInt32(),
                                        NomeParametro = lDataTable.Rows[i]["dscr_parametro"].DBToString(),
                                    },
                                    Grupo = new GrupoInfo()
                                    {
                                        CodigoGrupo = lDataTable.Rows[i]["id_grupo"].DBToInt32(),
                                    }
                                });
                        }
                }

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(pParametro, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.Message;
            }
            return lRetorno;
        }

        public ListarPermissoesRiscoClienteResponse ListarPermissoesRiscoCliente(ListarPermissoesRiscoClienteRequest lRequest)
        {
            ListarPermissoesRiscoClienteResponse lResponse = new ListarPermissoesRiscoClienteResponse();
            lResponse.CodigoMensagemRequest = lRequest.CodigoMensagem;
            List<CondicaoInfo> lCOndicoes = new List<CondicaoInfo>();
            CondicaoInfo ci1 = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, (int)lRequest.CodigoCliente);
            lCOndicoes.Add(ci1);
            try
            {
                ConsultarObjetosResponse<PermissaoRiscoAssociadaInfo> lRes = gPersistencia.ConsultarObjetos<PermissaoRiscoAssociadaInfo>(new ConsultarObjetosRequest<PermissaoRiscoAssociadaInfo>()
                {
                    Condicoes = lCOndicoes
                });


                lResponse.PermissoesAssociadas = lRes.Resultado;
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = ex.Message;
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
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
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
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
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
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = ex.Message;
            }
            return lResponse;
        }

        public ListarGruposResponse ListarGrupos(ListarGruposRequest lRequest)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new ListarGruposResponse();
            lRetorno.Grupos = new List<GrupoInfo>();
            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, lRequest.FiltroIdGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_grupo", DbType.String, lRequest.FiltroNomeGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_grupo", DbType.Int32, (int)lRequest.FiltroTipoGrupo);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0) foreach (DataRow lLinha in lDataTable.Rows)
                            lRetorno.Grupos.Add(new GrupoInfo()
                            {
                                CodigoGrupo = lLinha["id_grupo"].DBToInt32(),
                                NomeDoGrupo = lLinha["ds_grupo"].DBToString(),
                                TipoGrupo = (EnumRiscoRegra.TipoGrupo)lLinha["tp_grupo"].DBToInt32()
                            });
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        public ListarGrupoItemResponse ListarGrupoItens(ListarGrupoItemRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new ListarGrupoItemResponse();
            lRetorno.GrupoItens = new List<GrupoItemInfo>();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_item_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametro.FiltroIdGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupoitem", DbType.Int32, pParametro.FiltroIdGrupoItem);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_grupo", DbType.Int32, pParametro.FiltroTipoGrupo);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0) foreach (DataRow lLinha in lDataTable.Rows)
                            lRetorno.GrupoItens.Add(new GrupoItemInfo()
                            {
                                CodigoGrupo = lLinha["id_grupo"].DBToInt32(),
                                CodigoGrupoItem = lLinha["id_grupoitem"].DBToInt32(),
                                NomeGrupo = lLinha["ds_grupo"].DBToString(),
                                NomeGrupoItem = lLinha["ds_grupo_item"].DBToString(),
                            });
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        public ListarGrupoItemResponse ListarGrupoItensSpider(ListarGrupoItemRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new ListarGrupoItemResponse();
            lRetorno.GrupoItens = new List<GrupoItemInfo>();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_item_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametro.FiltroIdGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupoitem", DbType.Int32, pParametro.FiltroIdGrupoItem);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_grupo", DbType.Int32, pParametro.FiltroTipoGrupo);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0) foreach (DataRow lLinha in lDataTable.Rows)
                            lRetorno.GrupoItens.Add(new GrupoItemInfo()
                            {
                                CodigoGrupo = lLinha["id_grupo"].DBToInt32(),
                                CodigoGrupoItem = lLinha["id_grupoitem"].DBToInt32(),
                                NomeGrupo = lLinha["ds_grupo"].DBToString(),
                                NomeGrupoItem = lLinha["ds_grupo_item"].DBToString(),
                            });
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        public ListarBolsaResponse ListarBolsasRisco(ListarBolsaRequest lRequest)
        {
            ListarBolsaResponse lResponse = new ListarBolsaResponse();
            lResponse.CodigoMensagemRequest = lRequest.CodigoMensagem;

            try
            {
                ConsultarObjetosResponse<BolsaBancoInfo> lRes =
                    gPersistencia.ConsultarObjetos<BolsaBancoInfo>(new ConsultarObjetosRequest<BolsaBancoInfo>());

                lResponse.Bolsas = lRes.Resultado;
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = ex.Message;
            }
            return lResponse;
        }

        public ListarClientePermissaoParametroResponse ListarAssociacao(ListarClientePermissaoParametroRequest lRequest)
        {
            ListarClientePermissaoParametroResponse lResponse = new ListarClientePermissaoParametroResponse();
            lResponse.CodigoMensagemRequest = lRequest.CodigoMensagem;
            List<CondicaoInfo> lCondicoes = new List<CondicaoInfo>();
            CondicaoInfo par1 = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, lRequest.CodigoCliente);
            lCondicoes.Add(par1);
            try
            {
                ConsultarObjetosResponse<AssociacaoClienteRiscoInfo> lRes = gPersistencia.ConsultarObjetos<AssociacaoClienteRiscoInfo>(new ConsultarObjetosRequest<AssociacaoClienteRiscoInfo>() { Condicoes = lCondicoes });

                lResponse.Associacoes = lRes.Resultado;
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = ex.Message;
            }
            return lResponse;
        }

        public ListarParametroAlavancagemResponse ListarParametroAlavancagem(ListarParametroAlavancagemRequest pParametro)
        {
            var lRetorno = new ListarParametroAlavancagemResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_parametro_grupo_alavancagem_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametro.FiltroIdGrupo);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        foreach (DataRow lLinha in lDataTable.Rows)
                            lRetorno.Objeto.Add(new ParametroAlavancagemInfo()
                            {
                                IdGrupo = lLinha["id_grupo"].DBToInt32(),
                                IdParametroGrupoAlavancagem = lLinha["id_parametro_grupo_alavancagem"].DBToInt32(),
                                PercentualAlavancagemCompraAVista = lLinha["perc_alavanc_cavista"].DBToDecimal(),
                                PercentualAlavancagemCompraOpcao = lLinha["perc_alavanc_copcao"].DBToDecimal(),
                                PercentualAlavancagemVendaAVista = lLinha["perc_alavanc_vavista"].DBToDecimal(),
                                PercentualAlavancagemVendaOpcao = lLinha["perc_alavanc_vopcao"].DBToDecimal(),
                                PercentualContaCorrente = lLinha["perc_cc"].DBToDecimal(),
                                PercentualCustodia = lLinha["perc_cust"].DBToDecimal(),
                                StCarteiraGarantiaPrazo = lLinha["st_carteiraGarantiaPrazo"].DBToChar(),
                                StCarteiraOpcao = lLinha["st_carteiraOpcao"].DBToChar(),
                            });
                }

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        public ListarMonitoramentoRiscoResponse ListarMonitoramentoDeRisco(ListarMonitoramentoRiscoRequest pParametros)
        {
            var lRetorno = new ListarMonitoramentoRiscoResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_monitoramento_risco_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_parametro", DbType.Int32, pParametros.FiltroParametro);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int32, pParametros.FiltroCodigoAssessor);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametros.FiltroGrupoAlavancagem);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_codigo_cliente", DbType.Int32, pParametros.FiltroCodigoCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        foreach (DataRow lLinha in lDataTable.Rows)
                        {
                            lRetorno.Resultado.Add(new MonitoramentoRiscoInfo()
                            {
                                CdAssessor = lLinha["cd_assessor"].DBToInt32(),
                                DsGrupo = lLinha["ds_grupo"].DBToString(),
                                DsParametro = lLinha["ds_parametro"].DBToString(),
                                IdCliente = lLinha["cd_codigo"].DBToInt32(),
                                IdGrupo = lLinha["id_grupo"].DBToInt32(),
                                IdParametro = lLinha["id_parametro"].DBToInt32(),
                                NmCliente = lLinha["ds_nome"].DBToString(),
                                VlAlocado = lLinha["vl_alocado"].DBToDecimal(),
                                VlDisponivel = lLinha["vl_disponivel"].DBToDecimal(),
                                VlLimite = lLinha["vl_limite"].DBToDecimal(),
                            });
                        }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        #endregion

        #region Receber

        public ReceberParametroRiscoClienteResponse ReceberParametroRiscoCliente(ReceberParametroRiscoClienteRequest lRequest, Boolean pEfetuarLog = false)
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
                if (pEfetuarLog)
                    LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Receber);
                lResponse.ParametroRiscoCliente = lRes.Objeto;
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Receber, ex);
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = ex.Message;
            }
            return lResponse;
        }

        public ReceberParametroRiscoResponse ReceberParametroRisco(ReceberParametroRiscoRequest lRequest, Boolean pEfetuarLog = false)
        {
            ReceberParametroRiscoResponse lRes = new ReceberParametroRiscoResponse();
            try
            {
                ReceberObjetoRequest<ParametroRiscoInfo> lReqPar = new ReceberObjetoRequest<ParametroRiscoInfo>()
                {
                    CodigoObjeto = lRequest.CodigoParametro.ToString()
                };

                lRes.ParametroRisco = gPersistencia.ReceberObjeto<ParametroRiscoInfo>(lReqPar).Objeto;
                if (pEfetuarLog)
                    LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Receber);
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Receber, ex);
                lRes.DescricaoResposta = ex.Message;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRes;
        }

        public ReceberPermissaoRiscoResponse ReceberPermissaoRisco(ReceberPermissaoRiscoRequest lRequest, Boolean pEfetuarLog = false)
        {
            ReceberPermissaoRiscoResponse lRes = new ReceberPermissaoRiscoResponse();
            try
            {
                ReceberObjetoRequest<PermissaoRiscoInfo> lReqPar = new ReceberObjetoRequest<PermissaoRiscoInfo>()
                {
                    CodigoObjeto = lRequest.CodigoPermissao.ToString()
                };

                lRes.PermissaoRisco = gPersistencia.ReceberObjeto<PermissaoRiscoInfo>(lReqPar).Objeto;
                if (pEfetuarLog)
                    LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Receber);
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Receber, ex);
                lRes.DescricaoResposta = ex.Message;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRes;
        }

        public ReceberGrupoResponse ReceberGrupo(ReceberGrupoRequest lRequest, Boolean pEfetuarLog = false)
        {
            ReceberGrupoResponse lRes = new ReceberGrupoResponse();
            try
            {
                ReceberObjetoRequest<GrupoInfo> lReqPar = new ReceberObjetoRequest<GrupoInfo>()
                {
                    CodigoObjeto = lRequest.CodigoGrupo.ToString()
                };

                lRes.Grupo = gPersistencia.ReceberObjeto<GrupoInfo>(lReqPar).Objeto;
                if (pEfetuarLog)
                    LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Receber);
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Receber, ex);
                lRes.DescricaoResposta = ex.Message;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRes;
        }

        public ReceberClientePermissaoParametroResponse ReceberAssociacao(ReceberClientePermissaoParametroRequest lRequest, Boolean pEfetuarLog = false)
        {
            ReceberClientePermissaoParametroResponse lRes = new ReceberClientePermissaoParametroResponse();
            try
            {
                ReceberObjetoRequest<AssociacaoClienteRiscoInfo> lReqPar = new ReceberObjetoRequest<AssociacaoClienteRiscoInfo>()
                {
                    CodigoObjeto = lRequest.Associacao.CodigoAssociacao.ToString()
                };

                lRes.Associacao = gPersistencia.ReceberObjeto<AssociacaoClienteRiscoInfo>(lReqPar).Objeto;
                if (pEfetuarLog)
                    LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Receber);
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Receber, ex);
                lRes.DescricaoResposta = ex.Message;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRes;
        }

        public ReceberGrupoItemResponse ReceberGrupoItem(ReceberGrupoItemRequest pRequest)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new ReceberGrupoItemResponse();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_item_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pRequest.FiltroIdGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo_item", DbType.Int32, pRequest.FiltroIdGrupoItem);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        lRetorno.GrupoItem = new GrupoItemInfo()
                        {
                            CodigoGrupo = lDataTable.Rows[0]["id_grupo"].DBToInt32(),
                            CodigoGrupoItem = lDataTable.Rows[0]["id_grupoitem"].DBToInt32(),
                            NomeGrupo = lDataTable.Rows[0]["ds_grupo"].DBToString(),
                            NomeGrupoItem = lDataTable.Rows[0]["ds_grupoitem"].DBToString(),
                        };
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }


            return lRetorno;
        }

        #endregion

        #region Remover

        public RemoverGrupoItemResponse RemoverGrupoItem(RemoverGrupoItemRequest lRequest)
        {
            RemoverGrupoItemResponse lRes = new RemoverGrupoItemResponse();
            try
            {
                //Não possui consulta para o IdItem, apenas para o grupo
                RemoverObjetoRequest<GrupoItemInfo> lReqPar = new RemoverObjetoRequest<GrupoItemInfo>();
                lReqPar.CodigoObjeto = lRequest.CodigoGrupoItem.ToString();
                RemoverObjetoResponse<GrupoItemInfo> lResRem = gPersistencia.RemoverObjeto<GrupoItemInfo>(lReqPar);
                lRes.StatusResposta = MensagemResponseStatusEnum.OK;
                lRes.DescricaoResposta = "Item excluido com sucesso.";
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Excluir);
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Excluir, ex);
                lRes.DescricaoResposta = ex.Message;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRes;
        }

        public RemoverGrupoItemResponse RemoverGrupoItemSpider(RemoverGrupoItemRequest lRequest)
        {
            var lRetorno = new RemoverGrupoItemResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                //LOG
                //if (pRequest.ParametroRiscoCliente.CodigoParametroCliente != 0)
                //    ReceberParametroRiscoCliente(new ReceberParametroRiscoClienteRequest() { DescricaoUsuarioLogado = pRequest.DescricaoUsuarioLogado, IdUsuarioLogado = pRequest.IdUsuarioLogado, CodigoParametroRiscoCliente = pRequest.ParametroRiscoCliente.CodigoParametroCliente }, true);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_item_grupo_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupoitem", DbType.Int32, lRequest.CodigoGrupoItem);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }
            return lRetorno;
        }

        public RemoverParametroRiscoResponse RemoverParametroRisco(RemoverParametroRiscoRequest lRequest)
        {
            RemoverParametroRiscoResponse lResponse = new RemoverParametroRiscoResponse();
            try
            {
                //LOG
                ReceberParametroRisco(new ReceberParametroRiscoRequest() { DescricaoUsuarioLogado = lRequest.DescricaoUsuarioLogado, IdUsuarioLogado = lRequest.IdUsuarioLogado, CodigoParametro = lRequest.CodigoParametro }, true);

                RemoverObjetoRequest<ParametroRiscoInfo> lReqPar = new RemoverObjetoRequest<ParametroRiscoInfo>();
                lReqPar.CodigoObjeto = lRequest.CodigoParametro.ToString();
                RemoverObjetoResponse<ParametroRiscoInfo> lResRem = gPersistencia.RemoverObjeto<ParametroRiscoInfo>(lReqPar);
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
                lResponse.DescricaoResposta = "Item excluido com sucesso.";
                lResponse.BusinessException = false;
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Excluir);
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Excluir, ex);
                lResponse.BusinessException = true;
                lResponse.MessageException = ex.InnerException.Message;
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
                lResponse.DescricaoResposta = "Item não excluído.";
            }

            return lResponse;
        }

        public RemoverClientePermissaoParametroResponse RemoverAssociacao(RemoverClientePermissaoParametroRequest lRequest)
        {
            RemoverClientePermissaoParametroResponse lRes = new RemoverClientePermissaoParametroResponse();
            try
            {
                //LOG
                ReceberAssociacao(new ReceberClientePermissaoParametroRequest() { DescricaoUsuarioLogado = lRequest.DescricaoUsuarioLogado, IdUsuarioLogado = lRequest.IdUsuarioLogado, Associacao = lRequest.Associacao }, true);

                RemoverObjetoRequest<AssociacaoClienteRiscoInfo> lReqPar = new RemoverObjetoRequest<AssociacaoClienteRiscoInfo>();
                lReqPar.CodigoObjeto = lRequest.Associacao.CodigoAssociacao.ToString();
                RemoverObjetoResponse<AssociacaoClienteRiscoInfo> lResRem = gPersistencia.RemoverObjeto<AssociacaoClienteRiscoInfo>(lReqPar);
                lRes.StatusResposta = MensagemResponseStatusEnum.OK;
                lRes.DescricaoResposta = "Item excluido com sucesso.";
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Excluir);
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Excluir, ex);
                lRes.DescricaoResposta = ex.Message;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRes;
        }

        public RemoverPermissaoRiscoResponse RemoverPermissaoRisco(RemoverPermissaoRiscoRequest lRequest)
        {
            RemoverPermissaoRiscoResponse lResponse = new RemoverPermissaoRiscoResponse();
            try
            {
                //LOG
                ReceberPermissaoRisco(new ReceberPermissaoRiscoRequest() { DescricaoUsuarioLogado = lRequest.DescricaoUsuarioLogado, IdUsuarioLogado = lRequest.IdUsuarioLogado, CodigoPermissao = lRequest.CodigoPermissao }, true);

                RemoverObjetoRequest<PermissaoRiscoInfo> lReqPar = new RemoverObjetoRequest<PermissaoRiscoInfo>();
                lReqPar.CodigoObjeto = lRequest.CodigoPermissao.ToString();
                RemoverObjetoResponse<PermissaoRiscoInfo> lResRem = gPersistencia.RemoverObjeto<PermissaoRiscoInfo>(lReqPar);
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
                lResponse.DescricaoResposta = "Item excluido com sucesso.";
                lResponse.BusinessException = false;
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Excluir);
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Excluir, ex);
                lResponse.BusinessException = true;
                lResponse.MessageException = ex.InnerException.Message;
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
                lResponse.DescricaoResposta = "Item não excluído.";
            }

            return lResponse;
        }

        public RemoverGrupoRiscoResponse RemoverGrupoRisco(RemoverGrupoRiscoRequest lRequest)
        {
            RemoverGrupoRiscoResponse lResponse = new RemoverGrupoRiscoResponse();
            try
            {
                //LOG
                ReceberGrupo(new ReceberGrupoRequest() { DescricaoUsuarioLogado = lRequest.DescricaoUsuarioLogado, IdUsuarioLogado = lRequest.IdUsuarioLogado, CodigoGrupo = lRequest.CodigoGrupo }, true);

                RemoverObjetoRequest<GrupoInfo> lReqPar = new RemoverObjetoRequest<GrupoInfo>();
                lReqPar.CodigoObjeto = lRequest.CodigoGrupo.ToString();
                RemoverObjetoResponse<GrupoInfo> lResRem = gPersistencia.RemoverObjeto<GrupoInfo>(lReqPar);
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
                lResponse.DescricaoResposta = "Item excluido com sucesso.";
                lResponse.BusinessException = false;
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Excluir);
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Excluir, ex);
                lResponse.BusinessException = true;
                lResponse.MessageException = ex.InnerException.Message;
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
                lResponse.DescricaoResposta = "Item não excluído.";
            }

            return lResponse;
        }

        public RemoverGrupoRiscoResponse RemoverGrupoRiscoSpider(RemoverGrupoRiscoRequest pRequest)
        {
            var lRetorno = new RemoverGrupoRiscoResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                //LOG
                //if (pRequest.ParametroRiscoCliente.CodigoParametroCliente != 0)
                //    ReceberParametroRiscoCliente(new ReceberParametroRiscoClienteRequest() { DescricaoUsuarioLogado = pRequest.DescricaoUsuarioLogado, IdUsuarioLogado = pRequest.IdUsuarioLogado, CodigoParametroRiscoCliente = pRequest.ParametroRiscoCliente.CodigoParametroCliente }, true);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pRequest.CodigoGrupo);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }
            return lRetorno;
        }

        #endregion

        #region Salvar

          

        public SalvarParametroRiscoClienteResponse SalvarParametroRiscoCliente(SalvarParametroRiscoClienteRequest lRequest)
        {
            SalvarParametroRiscoClienteResponse lResponse = new SalvarParametroRiscoClienteResponse()
                {
                    CodigoMensagemRequest = lRequest.CodigoMensagem,
                };
            try
            {
                //LOG
                if (lRequest.ParametroRiscoCliente.CodigoParametroCliente != 0)
                    ReceberParametroRiscoCliente(new ReceberParametroRiscoClienteRequest() { DescricaoUsuarioLogado = lRequest.DescricaoUsuarioLogado, IdUsuarioLogado = lRequest.IdUsuarioLogado, CodigoParametroRiscoCliente = lRequest.ParametroRiscoCliente.CodigoParametroCliente }, true);

                SalvarObjetoResponse<ParametroRiscoClienteInfo> lRes = gPersistencia.SalvarObjeto<ParametroRiscoClienteInfo>(new SalvarObjetoRequest<ParametroRiscoClienteInfo>()
                {
                    Objeto = lRequest.ParametroRiscoCliente
                });
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
                lResponse.ParametroRiscoCliente = lRes.Objeto;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lResponse.DescricaoResposta = ex.Message;
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lResponse;
        }

        public SalvarParametroRiscoResponse SalvarParametroRisco(SalvarParametroRiscoRequest lRequest)
        {
            SalvarParametroRiscoResponse lRes = new SalvarParametroRiscoResponse();
            try
            {
                //LOG
                if (lRequest.ParametroRisco.CodigoParametro != 0)
                    ReceberParametroRisco(new ReceberParametroRiscoRequest() { DescricaoUsuarioLogado = lRequest.DescricaoUsuarioLogado, IdUsuarioLogado = lRequest.IdUsuarioLogado, CodigoParametro = lRequest.ParametroRisco.CodigoParametro }, true);

                SalvarObjetoRequest<ParametroRiscoInfo> lReqPar = new SalvarObjetoRequest<ParametroRiscoInfo>()
                {
                    Objeto = lRequest.ParametroRisco
                };

                lRes.ParametroRisco = gPersistencia.SalvarObjeto<ParametroRiscoInfo>(lReqPar).Objeto;
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRes.DescricaoResposta = ex.Message;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRes;
        }

        public SalvarParametroRiscoClienteResponse SalvarExpirarLimiteNovoOMS(SalvarParametroRiscoClienteRequest pRequest)
        {

            var lRetorno = new SalvarParametroRiscoClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                //LOG
                if (pRequest.ParametroRiscoCliente.CodigoParametroCliente != 0)
                    ReceberParametroRiscoCliente(new ReceberParametroRiscoClienteRequest() { DescricaoUsuarioLogado = pRequest.DescricaoUsuarioLogado, IdUsuarioLogado = pRequest.IdUsuarioLogado, CodigoParametroRiscoCliente = pRequest.ParametroRiscoCliente.CodigoParametroCliente }, true);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_expirarlimite"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pRequest.ParametroRiscoCliente.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_parametro", DbType.Int32, pRequest.ParametroRiscoCliente.CodigoParametroCliente);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }
            return lRetorno;
        }

        public SalvarParametroRiscoClienteResponse SalvarExpirarLimite(SalvarParametroRiscoClienteRequest pRequest)
        {

            var lRetorno = new SalvarParametroRiscoClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                //LOG
                if (pRequest.ParametroRiscoCliente.CodigoParametroCliente != 0)
                    ReceberParametroRiscoCliente(new ReceberParametroRiscoClienteRequest() { DescricaoUsuarioLogado = pRequest.DescricaoUsuarioLogado, IdUsuarioLogado = pRequest.IdUsuarioLogado, CodigoParametroRiscoCliente = pRequest.ParametroRiscoCliente.CodigoParametroCliente }, true);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_expirarlimite"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pRequest.ParametroRiscoCliente.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_parametro", DbType.Int32, pRequest.ParametroRiscoCliente.CodigoParametroCliente);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }
            return lRetorno;
        }

        public SalvarPermissaoRiscoResponse SalvarPermissaoRisco(SalvarPermissaoRiscoRequest lRequest)
        {
            SalvarPermissaoRiscoResponse lRes = new SalvarPermissaoRiscoResponse();
            try
            {
                //LOG
                if (lRequest.PermissaoRisco.CodigoPermissao != 0)
                    ReceberPermissaoRisco(new ReceberPermissaoRiscoRequest() { DescricaoUsuarioLogado = lRequest.DescricaoUsuarioLogado, IdUsuarioLogado = lRequest.IdUsuarioLogado, CodigoPermissao = lRequest.PermissaoRisco.CodigoPermissao }, true);

                SalvarObjetoRequest<PermissaoRiscoInfo> lReqPar = new SalvarObjetoRequest<PermissaoRiscoInfo>()
{
    Objeto = lRequest.PermissaoRisco
};

                lRes.PermissaoRisco = gPersistencia.SalvarObjeto<PermissaoRiscoInfo>(lReqPar).Objeto;
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRes.DescricaoResposta = ex.Message;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRes;
        }

        public SalvarClientePermissaoParametroResponse SalvarAssociacao(SalvarClientePermissaoParametroRequest lRequest)
        {
            SalvarClientePermissaoParametroResponse lRes = new SalvarClientePermissaoParametroResponse();
            try
            {
                //LOG
                if (lRequest.Associacao.CodigoCliente != 0)
                    ReceberAssociacao(new ReceberClientePermissaoParametroRequest() { DescricaoUsuarioLogado = lRequest.DescricaoUsuarioLogado, IdUsuarioLogado = lRequest.IdUsuarioLogado, Associacao = lRequest.Associacao }, true);

                SalvarObjetoRequest<AssociacaoClienteRiscoInfo> lReqPar = new SalvarObjetoRequest<AssociacaoClienteRiscoInfo>()
                {
                    Objeto = lRequest.Associacao
                };

                lRes.Associacao = gPersistencia.SalvarObjeto<AssociacaoClienteRiscoInfo>(lReqPar).Objeto;

                if (lRes.Associacao == null)
                {
                    lRes.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                    lRes.DescricaoResposta = "Parametro já associado para este cliente.";
                    LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, new Exception("Parametro já associado para este cliente."));
                }
                else
                {
                    LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
                }
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRes.DescricaoResposta = ex.Message;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRes;
        }

        public SalvarGrupoResponse SalvarGrupo(SalvarGrupoRequest lRequest)
        {
            var lRetorno = new SalvarGrupoResponse() { Grupo = new GrupoInfo() };
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_salvar"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, lRequest.Grupo.CodigoGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_grupo", DbType.String, lRequest.Grupo.NomeDoGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_grupo", DbType.Int32, (int)lRequest.Grupo.TipoGrupo);

                    object lIdGrupo = lAcessaDados.ExecuteScalar(lDbCommand);

                    lRetorno.Grupo = new GrupoInfo() { CodigoGrupo = lIdGrupo.DBToInt32() };

                    if (EnumRiscoRegra.TipoGrupo.GrupoAlavancagem.Equals(lRequest.Grupo.TipoGrupo))
                    {   //--> Inserindo um parâmetro zerado padrão para o grupo de alavancagem.
                        this.SalvarParametroAlavancagem(new SalvarParametroAlavancagemRequest()
                        {
                            Objeto = new ParametroAlavancagemInfo()
                            {
                                IdGrupo = lIdGrupo.DBToInt32()
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public SalvarGrupoResponse SalvarGrupoSpider(SalvarGrupoRequest lRequest)
        {
            var lRetorno = new SalvarGrupoResponse() { Grupo = new GrupoInfo() };
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_salvar"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, lRequest.Grupo.CodigoGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_grupo", DbType.String, lRequest.Grupo.NomeDoGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_grupo", DbType.Int32, (int)lRequest.Grupo.TipoGrupo);

                    object lIdGrupo = lAcessaDados.ExecuteScalar(lDbCommand);

                    lRetorno.Grupo = new GrupoInfo() { CodigoGrupo = lIdGrupo.DBToInt32() };

                    if (EnumRiscoRegra.TipoGrupo.GrupoAlavancagem.Equals(lRequest.Grupo.TipoGrupo))
                    {   //--> Inserindo um parâmetro zerado padrão para o grupo de alavancagem.
                        this.SalvarParametroAlavancagem(new SalvarParametroAlavancagemRequest()
                        {
                            Objeto = new ParametroAlavancagemInfo()
                            {
                                IdGrupo = lIdGrupo.DBToInt32()
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public SalvarGrupoItemResponse SalvarGrupoItem(SalvarGrupoItemRequest lRequest)
        {
            var lRetorno = new SalvarGrupoItemResponse();

            try
            {
                var lAcessaDados = new AcessaDados();
                lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_item_ins"))
                {
                    if (null != lRequest.GrupoItemLista && lRequest.GrupoItemLista.Count > 0)
                        lRequest.GrupoItemLista.ForEach(lGrupoItem =>
                        {
                            lDbCommand.Parameters.Clear();

                            lAcessaDados.AddInParameter(lDbCommand, "@id_grupoitem", DbType.Int32, lGrupoItem.CodigoGrupoItem);
                            lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, lGrupoItem.CodigoGrupo);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_item", DbType.String, lGrupoItem.NomeGrupoItem);

                            lGrupoItem.CodigoGrupoItem = lAcessaDados.ExecuteScalar(lDbCommand).DBToInt32();
                        });
                }

                lRetorno.ObjetoDeRetorno = lRequest.GrupoItemLista;

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRetorno.DescricaoResposta = ex.Message;
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRetorno;
        }

        public SalvarGrupoItemResponse SalvarGrupoItemSpider(SalvarGrupoItemRequest lRequest)
        {
            var lRetorno = new SalvarGrupoItemResponse();

            try
            {
                var lAcessaDados = new AcessaDados();
                lAcessaDados.ConnectionStringName = "GradualSpider";

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_item_ins"))
                {
                    if (null != lRequest.GrupoItemLista && lRequest.GrupoItemLista.Count > 0)
                        lRequest.GrupoItemLista.ForEach(lGrupoItem =>
                        {
                            lDbCommand.Parameters.Clear();

                            lAcessaDados.AddInParameter(lDbCommand, "@id_grupoitem", DbType.Int32, lGrupoItem.CodigoGrupoItem);
                            lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, lGrupoItem.CodigoGrupo);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_item", DbType.String, lGrupoItem.NomeGrupoItem);

                            lGrupoItem.CodigoGrupoItem = lAcessaDados.ExecuteScalar(lDbCommand).DBToInt32();
                        });
                }

                lRetorno.ObjetoDeRetorno = lRequest.GrupoItemLista;

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
            }
            catch (Exception ex)
            {
                LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRetorno.DescricaoResposta = ex.Message;
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRetorno;
        }

        public MensagemResponseBase SalvarPermissoesRiscoAssociadas(SalvarPermissoesRiscoAssociadasRequest lRequest)
        {
            MensagemResponseBase lResponse = new MensagemResponseBase();

            RemoverObjetoResponse<PermissaoRiscoAssociadaInfo> lremRes = gPersistencia.RemoverObjeto<PermissaoRiscoAssociadaInfo>(new RemoverObjetoRequest<PermissaoRiscoAssociadaInfo>()
            {
                CodigoObjeto = lRequest.PermissoesAssociadas[0].CodigoCliente.ToString()
            });

            foreach (PermissaoRiscoAssociadaInfo item in lRequest.PermissoesAssociadas)
            {
                try
                {
                    gPersistencia.SalvarObjeto<PermissaoRiscoAssociadaInfo>(new SalvarObjetoRequest<PermissaoRiscoAssociadaInfo>()
                    {
                        Objeto = item
                    });
                    LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
                    lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
                }
                catch (Exception ex)
                {
                    LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                    lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                    lResponse.DescricaoResposta += ex.Message;
                }
            }

            return lResponse;
        }

        public SalvarParametroAlavancagemResponse SalvarParametroAlavancagem(SalvarParametroAlavancagemRequest pRequest)
        {
            var lRetorno = new SalvarParametroAlavancagemResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_parametro_grupo_alavancagem_salvar"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pRequest.Objeto.IdGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@perc_alavanc_cavista", DbType.Decimal, pRequest.Objeto.PercentualAlavancagemCompraAVista);
                    lAcessaDados.AddInParameter(lDbCommand, "@perc_alavanc_copcao", DbType.Decimal, pRequest.Objeto.PercentualAlavancagemCompraOpcao);
                    lAcessaDados.AddInParameter(lDbCommand, "@perc_alavanc_vavista", DbType.Decimal, pRequest.Objeto.PercentualAlavancagemVendaAVista);
                    lAcessaDados.AddInParameter(lDbCommand, "@perc_alavanc_vopcao", DbType.Decimal, pRequest.Objeto.PercentualAlavancagemVendaOpcao);
                    lAcessaDados.AddInParameter(lDbCommand, "@perc_cc", DbType.Decimal, pRequest.Objeto.PercentualContaCorrente);
                    lAcessaDados.AddInParameter(lDbCommand, "@perc_cust", DbType.Decimal, pRequest.Objeto.PercentualCustodia);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_carteiraGarantiaPrazo", DbType.String, pRequest.Objeto.StCarteiraGarantiaPrazo);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_carteiraOpcao", DbType.String, pRequest.Objeto.StCarteiraOpcao);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        #endregion

        #region Metodos de cadastro do Risco

        public RemoverFatFingerClienteResponse RemoverFatFingerCliente(RemoverFatFingerClienteRequest pParametro)
        {
            var lRetorno = new RemoverFatFingerClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";
            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fat_finger_cliente_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.FatFinger.CodigoCliente);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public ReceberFatFingerClienteResponse ReceberFatFingerCliente(ReceberFatFingerClienteRequest pParametro)
        {
            var lRetorno = new ReceberFatFingerClienteResponse();
            var lAcessaDados = new AcessaDados();

            lRetorno.FatFinger = new FatFingerClienteInfo();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fat_finger_cliente_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.FatFinger.CodigoCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lRetorno.FatFinger.CodigoCliente  = lDataTable.Rows[0]["id_cliente"].DBToInt32();
                        lRetorno.FatFinger.DataVencimento = lDataTable.Rows[0]["dt_vencimento"].DBToDateTime();
                        lRetorno.FatFinger.ValorMaximo    = lDataTable.Rows[0]["vl_maximo"].DBToDecimal();
                    }
                }

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(pParametro, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.Message;
            }
            return lRetorno;
        }

        public SalvarFatFingerClienteResponse SalvarFatFingerCliente(SalvarFatFingerClienteRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new SalvarFatFingerClienteResponse();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fat_finger_cliente_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_vencimento", DbType.DateTime, pParametro.FatFinger.DataVencimento);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_maximo"    , DbType.Decimal , pParametro.FatFinger.ValorMaximo);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente"   , DbType.Int32   , pParametro.FatFinger.CodigoCliente);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                lRetorno.FatFinger = pParametro.FatFinger;

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public ReceberTravaExposicaoResponse ReceberTravaExposicao(ReceberTravaExposicaoRequest pParametro)
        {
            var lRetorno = new ReceberTravaExposicaoResponse();
            var lAcessaDados = new AcessaDados();

            lRetorno.Exposicao = new TravaExposicaoInfo();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_trava_exposicao_sel"))
                {
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        TravaExposicaoInfo lInfo = new TravaExposicaoInfo();

                        lInfo.PrecentualOscilacao = lDataTable.Rows[0]["perc_oscilacao"].DBToDecimal();
                        lInfo.PrejuizoMaximo      = lDataTable.Rows[0]["prejuizo_maximo"].DBToDecimal();

                        lRetorno.Exposicao = lInfo;
                    }
                }

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public ReceberTravaExposicaoResponse ReceberTravaExposicaoSpider(ReceberTravaExposicaoRequest pParametro)
        {
            var lRetorno = new ReceberTravaExposicaoResponse();
            var lAcessaDados = new AcessaDados();

            lRetorno.Exposicao = new TravaExposicaoInfo();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_trava_exposicao_sel"))
                {
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        TravaExposicaoInfo lInfo = new TravaExposicaoInfo();

                        lInfo.PrecentualOscilacao = lDataTable.Rows[0]["perc_oscilacao"].DBToDecimal();
                        lInfo.PrejuizoMaximo = lDataTable.Rows[0]["prejuizo_maximo"].DBToDecimal();

                        lRetorno.Exposicao = lInfo;
                    }
                }

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public SalvarTravaExposicaoResponse SalvarTravaExposicao(SalvarTravaExposicaoRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new SalvarTravaExposicaoResponse();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_trava_exposicao_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_perc_oscilacao", DbType.Decimal,  pParametro.Exposicao.PrecentualOscilacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_prej_max", DbType.Decimal,   pParametro.Exposicao.PrejuizoMaximo);
                    

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                lRetorno.Exposicao = pParametro.Exposicao;

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public SalvarTravaExposicaoResponse SalvarTravaExposicaoSpider(SalvarTravaExposicaoRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new SalvarTravaExposicaoResponse();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_trava_exposicao_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_perc_oscilacao", DbType.Decimal, pParametro.Exposicao.PrecentualOscilacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_prej_max", DbType.Decimal, pParametro.Exposicao.PrejuizoMaximo);


                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                lRetorno.Exposicao = pParametro.Exposicao;

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public ListarBloqueiroInstrumentoResponse ListarBloqueioClienteInstrumentoDirecao(ListarBloqueiroInstrumentoRequest pParametro)
        {
            var lRetorno = new ListarBloqueiroInstrumentoResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_bloqueio_lst"))
                {
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.Resultado.Add(
                                new BloqueioInstrumentoInfo()
                                {
                                    CdAtivo   = lDataTable.Rows[i]["cd_ativo"].DBToString(),
                                    Direcao   = lDataTable.Rows[i]["Direcao"].DBToString(),
                                    IdCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                });
                        }
                    }
                }
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public ListarBloqueiroInstrumentoResponse ListarBloqueioClienteInstrumentoDirecaoSpider(ListarBloqueiroInstrumentoRequest pParametro)
        {
            var lRetorno = new ListarBloqueiroInstrumentoResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_bloqueio_lst"))
                {
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.Resultado.Add(
                                new BloqueioInstrumentoInfo()
                                {
                                    CdAtivo   = lDataTable.Rows[i]["cd_ativo"].DBToString(),
                                    Direcao   = lDataTable.Rows[i]["Direcao"].DBToString(),
                                    IdCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                });
                        }
                    }
                }
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public RemoverBloqueioInstumentoResponse RemoverBloqueioClienteInstrumentoDirecao( RemoverClienteBloqueioRequest pParametro)
        {
            var lRetorno = new RemoverBloqueioInstumentoResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";
            
            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand( CommandType.StoredProcedure, "prc_cliente_bloqueio_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente" , DbType.Int32, pParametro.ClienteBloqueioRegra.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_ativo"   , DbType.AnsiString, pParametro.ClienteBloqueioRegra.Ativo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_direcao" , DbType.AnsiString, pParametro.ClienteBloqueioRegra.Direcao);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public RemoverBloqueioInstumentoResponse RemoverBloqueioClienteInstrumentoDirecaoSpider(RemoverClienteBloqueioRequest pParametro)
        {
            var lRetorno = new RemoverBloqueioInstumentoResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_bloqueio_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.ClienteBloqueioRegra.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_ativo", DbType.AnsiString, pParametro.ClienteBloqueioRegra.Ativo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_direcao", DbType.AnsiString, pParametro.ClienteBloqueioRegra.Direcao);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }
        
        private bool VerificaClienteBloqueioInstrumentoDirecao(string codigo)
        {
            var lAcessaDados = new AcessaDados();
            bool lRetorno = false;

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_bloqueio_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_ativo", DbType.String, codigo);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lRetorno = true;
                    }

                }

                
            }
            catch {}


            return lRetorno;
        }

        public SalvarBloqueioInstrumentoResponse SalvarClienteBloqueioInstrumentoDirecao(SalvarBloqueioInstrumentoRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new SalvarBloqueioInstrumentoResponse();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                if (!this.VerificaClienteBloqueioInstrumentoDirecao(pParametro.Objeto.CdAtivo))
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_bloqueio_ins"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_ativo", DbType.String, pParametro.Objeto.CdAtivo);
                        lAcessaDados.AddInParameter(lDbCommand, "@ds_direcao", DbType.String, pParametro.Objeto.Direcao);

                        lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }

                    lRetorno.Objeto = pParametro.Objeto;
                }

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public SalvarBloqueioInstrumentoResponse SalvarClienteBloqueioInstrumentoDirecaoSpider(SalvarBloqueioInstrumentoRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new SalvarBloqueioInstrumentoResponse();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                if (!this.VerificaClienteBloqueioInstrumentoDirecao(pParametro.Objeto.CdAtivo))
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_bloqueio_ins"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_ativo", DbType.String, pParametro.Objeto.CdAtivo);
                        lAcessaDados.AddInParameter(lDbCommand, "@ds_direcao", DbType.String, pParametro.Objeto.Direcao);

                        lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }

                    lRetorno.Objeto = pParametro.Objeto;
                }

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public SalvarRegraGrupoItemResponse SalvarRegraGrupoItemGlobal(SalvarRegraGrupoItemRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new SalvarRegraGrupoItemResponse();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_regra_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametro.RegraGrupoItem.CodigoGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_acao", DbType.Int32, pParametro.RegraGrupoItem.CodigoAcao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_direcao", DbType.String, pParametro.RegraGrupoItem.Sentido);

                    lAcessaDados.AddOutParameter(lDbCommand, "@id_grupo_regra", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    pParametro.RegraGrupoItem.CodigoGrupoRegra = Convert.ToInt32(lDbCommand.Parameters["@id_grupo_regra"].Value);
                }

                lRetorno.RegraGrupoItem = pParametro.RegraGrupoItem;

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public SalvarRegraGrupoItemResponse SalvarRegraGrupoItemGlobalSpider(SalvarRegraGrupoItemRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new SalvarRegraGrupoItemResponse();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_regra_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametro.RegraGrupoItem.CodigoGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_acao", DbType.Int32, pParametro.RegraGrupoItem.CodigoAcao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_direcao", DbType.String, pParametro.RegraGrupoItem.Sentido);

                    lAcessaDados.AddOutParameter(lDbCommand, "@id_grupo_regra", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    pParametro.RegraGrupoItem.CodigoGrupoRegra = Convert.ToInt32(lDbCommand.Parameters["@id_grupo_regra"].Value);
                }

                lRetorno.RegraGrupoItem = pParametro.RegraGrupoItem;

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public SalvarRegraGrupoItemResponse SalvarRegraGrupoItem(SalvarRegraGrupoItemRequest pParametro )
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new SalvarRegraGrupoItemResponse();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_grupo_regra_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.RegraGrupoItem.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_acao", DbType.Int32, pParametro.RegraGrupoItem.CodigoAcao);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametro.RegraGrupoItem.CodigoGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_direcao", DbType.String, pParametro.RegraGrupoItem.Sentido);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_usuario", DbType.String, pParametro.RegraGrupoItem.CodigoUsuario);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
               
                //using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_bloqueio_ins"))
                //{
                //    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.RegraGrupoItem.CodigoCliente);
                //    lAcessaDados.AddInParameter(lDbCommand, "@cd_ativo", DbType.String, pParametro.RegraGrupoItem.CodigoAcao);
                //    lAcessaDados.AddInParameter(lDbCommand, "@ds_direcao", DbType.String, pParametro.RegraGrupoItem.Sentido);

                //    lAcessaDados.ExecuteNonQuery(lDbCommand);
                //}

                lRetorno.RegraGrupoItem = pParametro.RegraGrupoItem;

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public SalvarRegraGrupoItemResponse SalvarRegraGrupoItemSpider(SalvarRegraGrupoItemRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new SalvarRegraGrupoItemResponse();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_grupo_regra_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente",  DbType.Int32,   pParametro.RegraGrupoItem.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_acao",     DbType.Int32,   pParametro.RegraGrupoItem.CodigoAcao);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo",    DbType.Int32,   pParametro.RegraGrupoItem.CodigoGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_direcao",  DbType.String,  pParametro.RegraGrupoItem.Sentido);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_usuario",  DbType.String,  pParametro.RegraGrupoItem.CodigoUsuario);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                //using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_bloqueio_ins"))
                //{
                //    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.RegraGrupoItem.CodigoCliente);
                //    lAcessaDados.AddInParameter(lDbCommand, "@cd_ativo", DbType.String, pParametro.RegraGrupoItem.CodigoAcao);
                //    lAcessaDados.AddInParameter(lDbCommand, "@ds_direcao", DbType.String, pParametro.RegraGrupoItem.Sentido);

                //    lAcessaDados.ExecuteNonQuery(lDbCommand);
                //}

                lRetorno.RegraGrupoItem = pParametro.RegraGrupoItem;

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public RemoverRegraGrupoItemResponse RemoverRegraGrupoItem(RemoverRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new RemoverRegraGrupoItemResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";
            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_grupo_regra_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo"  , DbType.Int32, pParametro.Objeto.CodigoGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_acao"   , DbType.Int32, pParametro.Objeto.CodigoAcao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_direcao", DbType.AnsiString, pParametro.Objeto.Sentido);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.Objeto = pParametro.Objeto;

                    lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public RemoverRegraGrupoItemResponse RemoverRegraGrupoItemSpider(RemoverRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new RemoverRegraGrupoItemResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";
            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_grupo_regra_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametro.Objeto.CodigoGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_acao", DbType.Int32, pParametro.Objeto.CodigoAcao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_direcao", DbType.AnsiString, pParametro.Objeto.Sentido);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.Objeto = pParametro.Objeto;

                    lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public ListarRegraGrupoItemResponse ListarRegraGrupoItem(ListarRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new ListarRegraGrupoItemResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_grupo_regra_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametro.Objeto.CodigoGrupo);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.Resultado.Add(
                                new RegraGrupoItemInfo()
                                {
                                    CodigoCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                    CodigoGrupo   = lDataTable.Rows[i]["id_grupo"].DBToInt32(),
                                    CodigoUsuario = lDataTable.Rows[i]["id_usuario"].DBToInt32(),
                                    CodigoAcao    = lDataTable.Rows[i]["id_acao"].DBToInt32(),
                                    Sentido       = lDataTable.Rows[i]["direcao"].ToString(),
                                });
                        }
                    }
                }
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public ListarRegraGrupoItemResponse ListarRegraGrupoItemSpider(ListarRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new ListarRegraGrupoItemResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_grupo_regra_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametro.Objeto.CodigoGrupo);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.Resultado.Add(
                                new RegraGrupoItemInfo()
                                {
                                    CodigoCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                    CodigoGrupo   = lDataTable.Rows[i]["id_grupo"].DBToInt32(),
                                    CodigoUsuario = lDataTable.Rows[i]["id_usuario"].DBToInt32(),
                                    CodigoAcao    = lDataTable.Rows[i]["id_acao"].DBToInt32(),
                                    Sentido       = lDataTable.Rows[i]["direcao"].ToString(),
                                });
                        }
                    }
                }
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public ListarRegraGrupoItemResponse ListarRegraGrupoItemGlobal(ListarRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new ListarRegraGrupoItemResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_grupo_regra_global_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametro.Objeto.CodigoGrupo);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.Resultado.Add(
                                new RegraGrupoItemInfo()
                                {
                                    CodigoGrupoRegra = lDataTable.Rows[i]["id_grupo_regra"].DBToInt32(),
                                    CodigoGrupo      = lDataTable.Rows[i]["id_grupo"].DBToInt32(),
                                    CodigoAcao       = lDataTable.Rows[i]["id_acao"].DBToInt32(),
                                    Sentido          = lDataTable.Rows[i]["sentido"].ToString(),
                                    NomeAcao         = lDataTable.Rows[i]["ds_acao"].ToString(),
                                    NomeGrupo        = lDataTable.Rows[i]["ds_grupo"].ToString(),
                                });
                        }
                    }
                }
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public ListarRegraGrupoItemResponse ListarRegraGrupoItemGlobalSpider(ListarRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new ListarRegraGrupoItemResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_grupo_regra_global_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametro.Objeto.CodigoGrupo);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.Resultado.Add(
                                new RegraGrupoItemInfo()
                                {
                                    CodigoGrupoRegra = lDataTable.Rows[i]["id_grupo_regra"].DBToInt32(),
                                    CodigoGrupo      = lDataTable.Rows[i]["id_grupo"].DBToInt32(),
                                    CodigoAcao       = lDataTable.Rows[i]["id_acao"].DBToInt32(),
                                    Sentido          = lDataTable.Rows[i]["sentido"].ToString(),
                                    NomeAcao         = lDataTable.Rows[i]["ds_acao"].ToString(),
                                    NomeGrupo        = lDataTable.Rows[i]["ds_grupo"].ToString(),
                                });
                        }
                    }
                }
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public RemoverRegraGrupoItemResponse RemoverRegraGrupoItemGlobal(RemoverRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new RemoverRegraGrupoItemResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";
            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_grupo_regra_global_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo"  , DbType.Int32, pParametro.Objeto.CodigoGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_acao"   , DbType.Int32, pParametro.Objeto.CodigoAcao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_direcao", DbType.AnsiString, pParametro.Objeto.Sentido);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public RemoverRegraGrupoItemResponse RemoverRegraGrupoItemGlobalSpider(RemoverRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new RemoverRegraGrupoItemResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";
            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_grupo_regra_global_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametro.Objeto.CodigoGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_acao", DbType.Int32, pParametro.Objeto.CodigoAcao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_direcao", DbType.AnsiString, pParametro.Objeto.Sentido);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public SalvarBloqueioInstrumentoResponse SalvarBloqueioInstrumento(DbTransaction pDbTransaction, SalvarBloqueioInstrumentoRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new SalvarBloqueioInstrumentoResponse();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pDbTransaction, CommandType.StoredProcedure, "prc_cliente_bloqueio_instrumento_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_ativo", DbType.String, pParametro.Objeto.CdAtivo);
                    lAcessaDados.AddInParameter(lDbCommand, "@direcao", DbType.String, pParametro.Objeto.Direcao);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pDbTransaction);
                }
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public SalvarBloqueioInstrumentoResponse SalvarBloqueioInstrumentoSpider(DbTransaction pDbTransaction, SalvarBloqueioInstrumentoRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new SalvarBloqueioInstrumentoResponse();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pDbTransaction, CommandType.StoredProcedure, "prc_cliente_bloqueio_instrumento_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_ativo", DbType.String, pParametro.Objeto.CdAtivo);
                    lAcessaDados.AddInParameter(lDbCommand, "@direcao", DbType.String, pParametro.Objeto.Direcao);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pDbTransaction);
                }
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public ListarBloqueiroInstrumentoResponse ListarBloqueioPorCliente(ListarBloqueiroInstrumentoRequest pParametro)
        {
            var lRetorno = new ListarBloqueiroInstrumentoResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_bloqueio_instrumento_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.Resultado.Add(
                                new BloqueioInstrumentoInfo()
                                {
                                    CdAtivo = lDataTable.Rows[i]["cd_ativo"].DBToString(),
                                    Direcao = lDataTable.Rows[i]["direcao"].DBToString(),
                                    IdCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                });
                        }
                    }
                }
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public RemoverBloqueioInstumentoResponse RemoverBloqueioPorCliente(DbTransaction pDbTransaction, RemoverBloqueioInstrumentoRequest pParametro)
        {
            var lRetorno = new RemoverBloqueioInstumentoResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";
            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pDbTransaction, CommandType.StoredProcedure, "prc_cliente_bloqueio_instumento_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);
                    lAcessaDados.ExecuteNonQuery(lDbCommand, pDbTransaction);

                    lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public RemoverBloqueioInstumentoResponse RemoverBloqueioPorClienteSpider(DbTransaction pDbTransaction, RemoverBloqueioInstrumentoRequest pParametro)
        {
            var lRetorno = new RemoverBloqueioInstumentoResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";
            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pDbTransaction, CommandType.StoredProcedure, "prc_cliente_bloqueio_instumento_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);
                    lAcessaDados.ExecuteNonQuery(lDbCommand, pDbTransaction);

                    lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public SalvarClienteParametroGrupoResponse SalvarClienteParametroGrupo(DbTransaction pDbTransaction, SalvarClienteParametroGrupoRequest pParametro)
        {
            var lRetorno = new SalvarClienteParametroGrupoResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pDbTransaction, CommandType.StoredProcedure, "prc_cliente_parametro_grupo_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametro.Objeto.IdGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_parametro", DbType.Int32, pParametro.Objeto.IdParametro);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pDbTransaction);

                    lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        public RemoverClienteParametroGrupoResponse RemoverClienteParametroGrupo(DbTransaction pDbTransaction, RemoverClienteParametroGrupoRequest pParametro)
        {
            var lRetorno = new RemoverClienteParametroGrupoResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                if (pParametro.Objeto == null || 0.Equals(pParametro.Objeto.IdCliente) || 0.Equals(pParametro.Objeto.IdParametro))
                    throw new NullReferenceException();

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pDbTransaction, CommandType.StoredProcedure, "prc_cliente_parametro_grupo_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_parametro", DbType.Int32, pParametro.Objeto.IdParametro);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pDbTransaction);

                    lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                }
            }
            catch (NullReferenceException)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroValidacao;
                lRetorno.DescricaoResposta = "Informe o IdCliente e o IdParametro antes de tentar a exclusão.";
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        public ListarClienteParametroGrupoResponse ListarClienteParametroGrupo(ListarClienteParametroGrupoRequest pParametro)
        {
            var lRetorno = new ListarClienteParametroGrupoResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_grupo_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null && lDataTable.Rows.Count > 0)
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                            lRetorno.ListaObjeto.Add(new ClienteParametroGrupoInfo()
                                {
                                    IdCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                    IdGrupo = lDataTable.Rows[i]["id_grupo"].DBToInt32(),
                                    IdParametro = lDataTable.Rows[i]["id_parametro"].DBToInt32(),
                                });
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        #endregion

        #region IServicoRegrasRisco Members

        public ConsultarObjetosResponse<ClienteLimiteInfo> ConsultarLimitesDoCliente(ConsultarObjetosRequest<ClienteLimiteInfo> pParametros)
        {
            return new RiscoMovimentacaoDeLimites().ConsultarLimitesDoCliente(pParametros);
        }

        public ConsultarObjetosResponse<ClienteLimiteMovimentoInfo> ConsultarMovimentacaoDosLimitesDoCliente(ConsultarObjetosRequest<ClienteLimiteMovimentoInfo> pParametros)
        {
            return new RiscoMovimentacaoDeLimites().ConsultarMovimentacaoDosLimitesDoCliente(pParametros);
        }

        #endregion

        #region IServicoRegrasRisco Members
        private PermissaoRiscoInfo MontarObjetoPermicaoRisco(DataRow dr)
        {
            PermissaoRiscoInfo lRetorno = new PermissaoRiscoInfo();

            lRetorno.Bolsa = (BolsaInfo)((int)dr["id_bolsa"]);
            lRetorno.CodigoPermissao = (int)dr["id_permissao"];
            lRetorno.Metodo = dr["nome_metodo"].ToString();
            lRetorno.NameSpace = dr["url_namespace"].ToString();
            lRetorno.NomePermissao = dr["dscr_permissao"].ToString();
            return lRetorno;
        }

        private PermissaoRiscoAssociadaInfo MontarObjetoPermicaoRiscoAssociada(DataRow dr)
        {
            PermissaoRiscoAssociadaInfo lRetorno = new PermissaoRiscoAssociadaInfo();

            lRetorno.CodigoCliente = (int)dr["id_cliente"];
            lRetorno.CodigoPermissaoRiscoAssociada = (int)dr["id_cliente_permissao"];
            if (!Convert.IsDBNull(dr["id_grupo"]))
                lRetorno.Grupo = new GrupoDbLib().ReceberObjeto(new ReceberObjetoRequest<GrupoInfo>() { CodigoObjeto = dr["id_grupo"].ToString() }).Objeto;
            lRetorno.PermissaoRisco = new PermissaoRiscoDbLib().ReceberObjeto(new ReceberObjetoRequest<PermissaoRiscoInfo>() { CodigoObjeto = dr["id_permissao"].ToString() }).Objeto;

            return lRetorno;
        }

        #region NovoOMS
        public ListarParametrosRiscoClienteResponse ListarLimitePorClienteNovoOMS(ListarParametrosRiscoClienteRequest pParametro)
        {
            var lRetorno = new ListarParametrosRiscoClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.CodigoCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.ParametrosRiscoCliente.Add(
                                new ParametroRiscoClienteInfo()
                                {
                                    CodigoCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                    DataValidade = lDataTable.Rows[i]["dt_validade"].DBToDateTime(),
                                    IdBolsa = lDataTable.Rows[i]["id_bolsa"].DBToInt32(),
                                    StAtivo = lDataTable.Rows[i]["st_ativo"].DBToChar(),
                                    Valor = lDataTable.Rows[i]["vl_parametro"].DBToDecimal(),
                                    Parametro = new ParametroRiscoInfo()
                                    {
                                        CodigoParametro = lDataTable.Rows[i]["id_parametro"].DBToInt32(),
                                        NomeParametro = lDataTable.Rows[i]["dscr_parametro"].DBToString(),
                                    },
                                    Grupo = new GrupoInfo()
                                    {
                                        CodigoGrupo = lDataTable.Rows[i]["id_grupo"].DBToInt32(),
                                    }
                                });
                        }
                }

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(pParametro, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.Message;
            }
            return lRetorno;
        }

        public ListarPermissoesRiscoResponse ListarPermissoesRiscoNovoOMS(ListarPermissoesRiscoRequest pParametro)
        {
            var lRetorno = new ListarPermissoesRiscoResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_permissao_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dscr_permissao", DbType.Int32, pParametro.FiltroNomePermissao);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.Permissoes.Add(MontarObjetoPermicaoRisco(lDataTable.Rows[i]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogRisco.Logar(pParametro, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        public ListarPermissoesRiscoClienteResponse ListarPermissoesRiscoClienteNovoOMS(ListarPermissoesRiscoClienteRequest pParametro)
        {

            var lRetorno = new ListarPermissoesRiscoClienteResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_permissao_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.CodigoCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.PermissoesAssociadas.Add(MontarObjetoPermicaoRiscoAssociada(lDataTable.Rows[i]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogRisco.Logar(pParametro, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        public ListarBloqueiroInstrumentoResponse ListarBloqueioPorClienteNovoOMS(ListarBloqueiroInstrumentoRequest pParametro)
        {
            var lRetorno = new ListarBloqueiroInstrumentoResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_bloqueio_instrumento_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.Resultado.Add(
                                new BloqueioInstrumentoInfo()
                                {
                                    CdAtivo   = lDataTable.Rows[i]["cd_ativo"].DBToString(),
                                    Direcao   = lDataTable.Rows[i]["direcao"].DBToString(),
                                    IdCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                });
                        }
                    }
                }
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public ListarClienteParametroGrupoResponse ListarClienteParametroGrupoNovoOMS(ListarClienteParametroGrupoRequest pParametro)
        {
            var lRetorno = new ListarClienteParametroGrupoResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_grupo_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null && lDataTable.Rows.Count > 0)
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                            lRetorno.ListaObjeto.Add(new ClienteParametroGrupoInfo()
                            {
                                IdCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                IdGrupo = lDataTable.Rows[i]["id_grupo"].DBToInt32(),
                                IdParametro = lDataTable.Rows[i]["id_parametro"].DBToInt32(),
                            });
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        public SalvarParametroRiscoClienteResponse SalvarParametroRiscoClienteNovoOMS(SalvarParametroRiscoClienteRequest pRequest)
        {
            var lRetorno = new SalvarParametroRiscoClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                //LOG
                if (pRequest.ParametroRiscoCliente.CodigoParametroCliente != 0)
                    ReceberParametroRiscoCliente(new ReceberParametroRiscoClienteRequest() { DescricaoUsuarioLogado = pRequest.DescricaoUsuarioLogado, IdUsuarioLogado = pRequest.IdUsuarioLogado, CodigoParametroRiscoCliente = pRequest.ParametroRiscoCliente.CodigoParametroCliente }, true);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_salvar"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_parametro", DbType.Int32, pRequest.ParametroRiscoCliente.CodigoParametroCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pRequest.ParametroRiscoCliente.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_parametro", DbType.Int32, pRequest.ParametroRiscoCliente.Parametro.CodigoParametro);

                    if (pRequest.ParametroRiscoCliente.Grupo != null)
                        lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pRequest.ParametroRiscoCliente.Grupo);

                    if (pRequest.ParametroRiscoCliente.Valor != null)
                        lAcessaDados.AddInParameter(lDbCommand, "@vl_parametro", DbType.Decimal, pRequest.ParametroRiscoCliente.Valor);

                    if (pRequest.ParametroRiscoCliente.DataValidade != null)
                        lAcessaDados.AddInParameter(lDbCommand, "@dt_validade", DbType.DateTime, pRequest.ParametroRiscoCliente.DataValidade);


                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }
            return lRetorno;
        }

        public MensagemResponseBase SalvarPermissoesRiscoAssociadasNovoOMS(SalvarPermissoesRiscoAssociadasRequest pRequest)
        {

            var lRetorno = new SalvarParametroRiscoClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_permissao_del"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pRequest.PermissoesAssociadas[0].CodigoCliente);

                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            foreach (PermissaoRiscoAssociadaInfo item in pRequest.PermissoesAssociadas)
            {
                try
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_permissao_salvar"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@id_cliente",           DbType.Int32, item.CodigoCliente);
                        lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_permissao", DbType.Int32, item.CodigoPermissaoRiscoAssociada);
                        lAcessaDados.AddInParameter(lDbCommand, "@id_permissao",         DbType.Int32, item.PermissaoRisco.CodigoPermissao);

                        if (item.Grupo != null)
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, item.Grupo);
                        }

                        lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }
                }
                catch (Exception ex)
                {
                    LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                    lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                    lRetorno.DescricaoResposta += ex.Message;
                }
            }

            return lRetorno;
        }

        public ListarGruposResponse ListarGruposNovoOMS(ListarGruposRequest lRequest)
        {
            var lAcessaDados                  = new AcessaDados();
            var lRetorno                      = new ListarGruposResponse();
            lRetorno.Grupos                   = new List<GrupoInfo>();
            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32,      lRequest.FiltroIdGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_grupo", DbType.String,     lRequest.FiltroNomeGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_grupo", DbType.Int32, (int)lRequest.FiltroTipoGrupo);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow lLinha in lDataTable.Rows)
                        {
                            lRetorno.Grupos.Add(new GrupoInfo()
                            {
                                CodigoGrupo = lLinha["id_grupo"].DBToInt32(),
                                NomeDoGrupo = lLinha["ds_grupo"].DBToString(),
                                TipoGrupo = (EnumRiscoRegra.TipoGrupo)lLinha["tp_grupo"].DBToInt32()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        public ListarGruposResponse ListarGruposSpider(ListarGruposRequest lRequest)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new ListarGruposResponse();
            lRetorno.Grupos = new List<GrupoInfo>();
            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, lRequest.FiltroIdGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_grupo", DbType.String, lRequest.FiltroNomeGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@tp_grupo", DbType.Int32, (int)lRequest.FiltroTipoGrupo);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow lLinha in lDataTable.Rows)
                        {
                            lRetorno.Grupos.Add(new GrupoInfo()
                            {
                                CodigoGrupo = lLinha["id_grupo"].DBToInt32(),
                                NomeDoGrupo = lLinha["ds_grupo"].DBToString(),
                                TipoGrupo = (EnumRiscoRegra.TipoGrupo)lLinha["tp_grupo"].DBToInt32()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }
        #endregion

        #region Spider
        public ListarParametrosRiscoClienteResponse ListarLimitePorClienteSpider(ListarParametrosRiscoClienteRequest pParametro)
        {
            var lRetorno = new ListarParametrosRiscoClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.CodigoCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.ParametrosRiscoCliente.Add(
                                new ParametroRiscoClienteInfo()
                                {
                                    CodigoCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                    DataValidade = lDataTable.Rows[i]["dt_validade"].DBToDateTime(),
                                    IdBolsa = lDataTable.Rows[i]["id_bolsa"].DBToInt32(),
                                    StAtivo = lDataTable.Rows[i]["st_ativo"].DBToChar(),
                                    Valor = lDataTable.Rows[i]["vl_parametro"].DBToDecimal(),
                                    Parametro = new ParametroRiscoInfo()
                                    {
                                        CodigoParametro = lDataTable.Rows[i]["id_parametro"].DBToInt32(),
                                        NomeParametro = lDataTable.Rows[i]["dscr_parametro"].DBToString(),
                                    },
                                    Grupo = new GrupoInfo()
                                    {
                                        CodigoGrupo = lDataTable.Rows[i]["id_grupo"].DBToInt32(),
                                    }
                                });
                        }
                }

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(pParametro, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.Message;
            }
            return lRetorno;
        }

        public ListarPermissoesRiscoResponse ListarPermissoesRiscoSpider(ListarPermissoesRiscoRequest pParametro)
        {
            var lRetorno = new ListarPermissoesRiscoResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_permissao_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dscr_permissao", DbType.Int32, pParametro.FiltroNomePermissao);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.Permissoes.Add(MontarObjetoPermicaoRisco(lDataTable.Rows[i]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogRisco.Logar(pParametro, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        public ListarPermissoesRiscoClienteResponse ListarPermissoesRiscoClienteSpider(ListarPermissoesRiscoClienteRequest pParametro)
        {

            var lRetorno = new ListarPermissoesRiscoClienteResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_permissao_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.CodigoCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.PermissoesAssociadas.Add(MontarObjetoPermicaoRiscoAssociada(lDataTable.Rows[i]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogRisco.Logar(pParametro, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        public ListarBloqueiroInstrumentoResponse ListarBloqueioPorClienteSpider(ListarBloqueiroInstrumentoRequest pParametro)
        {
            var lRetorno = new ListarBloqueiroInstrumentoResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_bloqueio_instrumento_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.Resultado.Add(
                                new BloqueioInstrumentoInfo()
                                {
                                    CdAtivo = lDataTable.Rows[i]["cd_ativo"].DBToString(),
                                    Direcao = lDataTable.Rows[i]["direcao"].DBToString(),
                                    IdCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                });
                        }
                    }
                }
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public ListarClienteParametroGrupoResponse ListarClienteParametroGrupoSpider(ListarClienteParametroGrupoRequest pParametro)
        {
            var lRetorno = new ListarClienteParametroGrupoResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_grupo_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null && lDataTable.Rows.Count > 0)
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                            lRetorno.ListaObjeto.Add(new ClienteParametroGrupoInfo()
                            {
                                IdCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                IdGrupo = lDataTable.Rows[i]["id_grupo"].DBToInt32(),
                                IdParametro = lDataTable.Rows[i]["id_parametro"].DBToInt32(),
                            });
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        public SalvarParametroRiscoClienteResponse SalvarParametroRiscoClienteSpider(SalvarParametroRiscoClienteRequest pRequest)
        {
            var lRetorno = new SalvarParametroRiscoClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                //LOG
                if (pRequest.ParametroRiscoCliente.CodigoParametroCliente != 0)
                    ReceberParametroRiscoCliente(new ReceberParametroRiscoClienteRequest() { DescricaoUsuarioLogado = pRequest.DescricaoUsuarioLogado, IdUsuarioLogado = pRequest.IdUsuarioLogado, CodigoParametroRiscoCliente = pRequest.ParametroRiscoCliente.CodigoParametroCliente }, true);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_salvar"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_parametro", DbType.Int32, pRequest.ParametroRiscoCliente.CodigoParametroCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pRequest.ParametroRiscoCliente.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_parametro", DbType.Int32, pRequest.ParametroRiscoCliente.Parametro.CodigoParametro);

                    if (pRequest.ParametroRiscoCliente.Grupo != null)
                        lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pRequest.ParametroRiscoCliente.Grupo);

                    if (pRequest.ParametroRiscoCliente.Valor != null)
                        lAcessaDados.AddInParameter(lDbCommand, "@vl_parametro", DbType.Decimal, pRequest.ParametroRiscoCliente.Valor);

                    if (pRequest.ParametroRiscoCliente.DataValidade != null)
                        lAcessaDados.AddInParameter(lDbCommand, "@dt_validade", DbType.DateTime, pRequest.ParametroRiscoCliente.DataValidade);


                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }
            return lRetorno;
        }


        public MensagemResponseBase SalvarPermissoesRiscoAssociadasSpider(SalvarPermissoesRiscoAssociadasRequest pRequest)
        {

            var lRetorno = new SalvarParametroRiscoClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_permissao_del"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pRequest.PermissoesAssociadas[0].CodigoCliente);

                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            foreach (PermissaoRiscoAssociadaInfo item in pRequest.PermissoesAssociadas)
            {
                try
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_permissao_salvar"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, item.CodigoCliente);
                        lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_permissao", DbType.Int32, item.CodigoPermissaoRiscoAssociada);
                        lAcessaDados.AddInParameter(lDbCommand, "@id_permissao", DbType.Int32, item.PermissaoRisco.CodigoPermissao);

                        if (item.Grupo != null)
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, item.Grupo);
                        }

                        lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }
                }
                catch (Exception ex)
                {
                    LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                    lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                    lRetorno.DescricaoResposta += ex.Message;
                }
            }

            return lRetorno;
        }
        
        public SalvarParametroRiscoClienteResponse SalvarExpirarLimiteSpider(SalvarParametroRiscoClienteRequest pRequest)
        {

            var lRetorno = new SalvarParametroRiscoClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                //LOG
                if (pRequest.ParametroRiscoCliente.CodigoParametroCliente != 0)
                    ReceberParametroRiscoCliente(new ReceberParametroRiscoClienteRequest() { DescricaoUsuarioLogado = pRequest.DescricaoUsuarioLogado, IdUsuarioLogado = pRequest.IdUsuarioLogado, CodigoParametroRiscoCliente = pRequest.ParametroRiscoCliente.CodigoParametroCliente }, true);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_expirarlimite"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pRequest.ParametroRiscoCliente.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_parametro", DbType.Int32, pRequest.ParametroRiscoCliente.CodigoParametroCliente);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }
            return lRetorno;
        }
        #endregion

        #endregion
    }
}
