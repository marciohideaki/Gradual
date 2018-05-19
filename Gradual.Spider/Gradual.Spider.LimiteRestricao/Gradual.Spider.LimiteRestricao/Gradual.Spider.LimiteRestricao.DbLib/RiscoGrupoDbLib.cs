using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using Gradual.Spider.LimiteRestricao.Lib.Dados;
using Gradual.OMS.Library;
using Gradual.Spider.LimiteRestricao.Lib.Mensagens;
using log4net;

namespace Gradual.Spider.LimiteRestricao.DbLib
{
    public class RiscoGrupoDbLib
    {

        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string NomeConexaoSpider = "GradualSpider";

        public RiscoGrupoInfo ReceberEntidadeRiscoGrupoInfo(RiscoGrupoInfo pParametros)
        {
            var lRetorno = new RiscoGrupoInfo();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_grupo_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametros.CodigoGrupo);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno = (this.CarregarEntidadeRiscoGrupoInfo(lDataTable.Rows[i]));
                    }
                }
            }

            return lRetorno;
        }

        private RiscoGrupoInfo CarregarEntidadeRiscoGrupoInfo(DataRow dr)
        {
            var lRetorno         = new RiscoGrupoInfo();
            
            lRetorno.CodigoGrupo = (int)dr["id_grupo"];
            lRetorno.NomeDoGrupo = dr["ds_grupo"].DBToString();

            return lRetorno;
        }

        public RiscoListarRestricaoGlobalResponse ListarRestricaoGlobalCliente(RiscoListarRestricaoGlobalRequest pParametro)
        {
            var lRetorno = new RiscoListarRestricaoGlobalResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            lRetorno.ListaRestricaoGlobal = new List<RiscoRestricaoGlobalInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_restricao_global_lst"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@IdCliente", DbType.Int32, pParametro.RestricaoGlobal.CodigoCliente);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.ListaRestricaoGlobal.Add(this.CarregarEntidadeRestricaoGlobalInfo(lDataTable.Rows[i]));
                    }
                }
            }

            return lRetorno;
        }

        public RiscoListarRestricaoGrupoResponse ListarRestricaoGrupoCliente(RiscoListarRestricaoGrupoRequest pParametro)
        {
            var lRetorno = new RiscoListarRestricaoGrupoResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;
            lRetorno.ListaRestricaoGrupo = new List<RiscoRestricaoGrupoInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_restricao_grupo_lst"))
            {
                //lAcessaDados.AddInParameter(lDbCommand, "@IdCliente", DbType.Int32, pParametro.RestricaoGrupo.CodigoCliente);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.ListaRestricaoGrupo.Add(this.CarregarEntidadeRestricaoGrupoInfo(lDataTable.Rows[i]));
                    }
                }
            }

            return lRetorno;
        }

        public RiscoListarRestricaoAtivoResponse ListarRestricaoAtivoCliente(RiscoListarRestricaoAtivoRequest pParametro)
        {
            var lRetorno = new RiscoListarRestricaoAtivoResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;
            lRetorno.ListaRestricaoAtivo = new List<RiscoRestricaoAtivoInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_restricao_ativo_lst"))
            {
                //lAcessaDados.AddInParameter(lDbCommand, "@IdCliente", DbType.Int32, pParametro.RestricaoAtivo.CodigoCliente);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.ListaRestricaoAtivo.Add(this.CarregarEntidadeRestricaoAtivoInfo(lDataTable.Rows[i]));
                    }
                }
            }

            return lRetorno;
        }

        public RiscoSalvarRestricaoGlobalResponse SalvarRestricaoGlobalCliente(RiscoSalvarRestricaoGlobalRequest pParametro)
        {
            var lRetorno = new RiscoSalvarRestricaoGlobalResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            lRetorno.ListaRestricaoGlobal = new List<RiscoRestricaoGlobalInfo>();

            if (pParametro.ListaRestricoesGlobal != null && pParametro.ListaRestricoesGlobal.Count != 0)
            {
                var lRestricao = pParametro.ListaRestricoesGlobal[0];

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_restricao_global_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@IdCliente", DbType.Int32, lRestricao.CodigoCliente);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
            }

            for (int i = 0; i < pParametro.ListaRestricoesGlobal.Count; i++)
            {
                var lRestricao = pParametro.ListaRestricoesGlobal[i];

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_restricao_global_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@IdCliente",       DbType.Int32,      lRestricao.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@VolumeNet",       DbType.Decimal,    lRestricao.LimiteVolumeNet);
                    lAcessaDados.AddInParameter(lDbCommand, "@QtdeNet",         DbType.Decimal,    lRestricao.QuantidadeNet);
                    lAcessaDados.AddInParameter(lDbCommand, "@PerdaMax",        DbType.Decimal,    lRestricao.LimitePerdaMax);
                    lAcessaDados.AddInParameter(lDbCommand, "@MaxOfertaVolume", DbType.Decimal,    lRestricao.LimiteMaxOfertaVolume);
                    lAcessaDados.AddInParameter(lDbCommand, "@MaxOfertaQtde",   DbType.Decimal,    lRestricao.LimiteMaxOfertaQtde);
                    lAcessaDados.AddInParameter(lDbCommand, "@StAtivo",         DbType.String,     lRestricao.Ativo);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.ListaRestricaoGlobal.Add(lRestricao);
                }
            }

            return lRetorno;
        }

        public RiscoSalvarRestricaoGrupoResponse SalvarRestricaoGrupoCliente(RiscoSalvarRestricaoGrupoRequest pParametro)
        {
            var lRetorno = new RiscoSalvarRestricaoGrupoResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            lRetorno.ListaRestricaoGrupo = new List<RiscoRestricaoGrupoInfo>();

            if (pParametro.ListaRestricoesGrupo != null && pParametro.ListaRestricoesGrupo.Count != 0)
            {
                var lRestricao = pParametro.ListaRestricoesGrupo[0];

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_restricao_grupo_del"))
                {
                    //lAcessaDados.AddInParameter(lDbCommand, "@IdCliente", DbType.Int32, lRestricao.CodigoCliente);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    gLogger.Info("Conseguiu deletar todos os grupos");
                }
            }

            gLogger.InfoFormat("Verificando o valor de pParametro.ListaRestricoesGrupo [{0}, {1}]", pParametro.ListaRestricoesGrupo, pParametro.ListaRestricoesGrupo.Count);

            for (int i = 0; i < pParametro.ListaRestricoesGrupo.Count; i++)
            {
                var lRestricao = pParametro.ListaRestricoesGrupo[i];

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_restricao_grupo_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@IdCliente",       DbType.Int32,   lRestricao.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@VolumeNet",       DbType.Decimal, lRestricao.LimiteVolumeNet);
                    lAcessaDados.AddInParameter(lDbCommand, "@QtdeNet",         DbType.Decimal, lRestricao.QuantidadeNet);
                    lAcessaDados.AddInParameter(lDbCommand, "@PerdaMax",        DbType.Decimal, lRestricao.LimitePerdaMax);
                    lAcessaDados.AddInParameter(lDbCommand, "@MaxOfertaVolume",  DbType.Decimal, lRestricao.LimiteMaxOfertaVolume);
                    lAcessaDados.AddInParameter(lDbCommand, "@MaxOfertaQtde",   DbType.Decimal, lRestricao.LimiteMaxOfertaQtde);
                    lAcessaDados.AddInParameter(lDbCommand, "@IdGrupo",         DbType.Int32,   lRestricao.CodigoGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@StAtivo",         DbType.String,  lRestricao.Ativo);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.ListaRestricaoGrupo.Add(lRestricao);
                }
            }

            return lRetorno;
        }

        public RiscoSalvarRestricaoAtivoResponse SalvarRestricaoAtivoCliente(RiscoSalvarRestricaoAtivoRequest pParametro)
        {
            var lRetorno = new RiscoSalvarRestricaoAtivoResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            lRetorno.ListaRestricaoAtivo = new List<RiscoRestricaoAtivoInfo>();

            if ( pParametro.ListaRestricoesAtivo != null && pParametro.ListaRestricoesAtivo.Count != 0 )
            {
                var lRestricao = pParametro.ListaRestricoesAtivo[0];

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_restricao_ativo_del"))
                {
                    //lAcessaDados.AddInParameter(lDbCommand, "@IdCliente", DbType.Int32, lRestricao.CodigoCliente);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
            }

            for (int i = 0; i < pParametro.ListaRestricoesAtivo.Count; i++)
            {
                var lRestricao = pParametro.ListaRestricoesAtivo[i];

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_restricao_ativo_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@IdCliente",       DbType.Int32,   lRestricao.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@Ativo",           DbType.String,  lRestricao.Ativo);
                    lAcessaDados.AddInParameter(lDbCommand, "@VolumeNet",       DbType.Decimal, lRestricao.LimiteVolumeNet);
                    lAcessaDados.AddInParameter(lDbCommand, "@QtdeNet",         DbType.Decimal, lRestricao.QuantidadeNet);
                    lAcessaDados.AddInParameter(lDbCommand, "@PerdaMax",        DbType.Decimal, lRestricao.LimitePerdaMax);
                    lAcessaDados.AddInParameter(lDbCommand, "@MaxOfertaVolume",  DbType.Decimal, lRestricao.LimiteMaxOfertaVolume);
                    lAcessaDados.AddInParameter(lDbCommand, "@MaxOfertaQtde",   DbType.Decimal, lRestricao.LimiteMaxOfertaQtde);
                    lAcessaDados.AddInParameter(lDbCommand, "@StAtivo",         DbType.String,  lRestricao.Ativo);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.ListaRestricaoAtivo.Add(lRestricao);
                }
            }

            return lRetorno;
        }

        

        private RiscoRestricaoGlobalInfo CarregarEntidadeRestricaoGlobalInfo(DataRow dr)
        {
            var lRetorno = new RiscoRestricaoGlobalInfo();

            lRetorno.CodigoCliente         = dr["Account"].DBToInt32();
            lRetorno.LimiteVolumeNet       = dr["LimiteVolumeNet"].DBToDecimal();
            lRetorno.QuantidadeNet         = dr["QuantidadeNet"].DBToDecimal();
            lRetorno.LimitePerdaMax        = dr["LimitePerdaMax"].DBToDecimal();
            lRetorno.LimiteMaxOfertaVolume = dr["LimiteMaxOfertaVolume"].DBToDecimal();
            lRetorno.LimiteMaxOfertaQtde   = dr["LimiteMaxOfertaQtde"].DBToDecimal();
            lRetorno.Ativo                 = dr["stAtivo"].DBToString();

            return lRetorno;
        }

        private RiscoRestricaoGrupoInfo CarregarEntidadeRestricaoGrupoInfo(DataRow dr)
        {
            var lRetorno = new RiscoRestricaoGrupoInfo();

            lRetorno.CodigoCliente         = dr["Account"].DBToInt32();
            lRetorno.LimiteVolumeNet       = dr["LimiteVolumeNet"].DBToDecimal();
            lRetorno.QuantidadeNet         = dr["QuantidadeNet"].DBToDecimal();
            lRetorno.LimitePerdaMax        = dr["LimitePerdaMax"].DBToDecimal();
            lRetorno.LimiteMaxOfertaVolume = dr["LimiteMaxOfertaVolume"].DBToDecimal();
            lRetorno.LimiteMaxOfertaQtde   = dr["LimiteMaxOfertaQtde"].DBToDecimal();
            lRetorno.CodigoGrupo           = dr["IdGrupo"].DBToString();
            lRetorno.DataAtualizacao       = dr["dtAtualizacao"].DBToDateTime();
            lRetorno.Ativo                 = dr["stAtivo"].DBToString();

            return lRetorno;
        }

        private RiscoRestricaoAtivoInfo CarregarEntidadeRestricaoAtivoInfo(DataRow dr)
        {
            var lRetorno = new RiscoRestricaoAtivoInfo();

            lRetorno.CodigoCliente         = dr["Account"].DBToInt32();
            lRetorno.LimiteVolumeNet       = dr["LimiteVolumeNet"].DBToDecimal();
            lRetorno.QuantidadeNet         = dr["QuantidadeNet"].DBToDecimal();
            lRetorno.LimitePerdaMax        = dr["LimitePerdaMax"].DBToDecimal();
            lRetorno.LimiteMaxOfertaVolume = dr["LimiteMaxOfertaVolume"].DBToDecimal();
            lRetorno.LimiteMaxOfertaQtde   = dr["LimiteMaxOfertaQtde"].DBToDecimal();
            lRetorno.DataAtualizacao       = dr["dtAtualizacao"].DBToDateTime();
            lRetorno.Ativo                 = dr["Symbol"].DBToString();

            return lRetorno;
        }



        public RiscoListarGrupoItemResponse ListarGrupoItensSpider(RiscoListarGrupoItemRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new RiscoListarGrupoItemResponse();
            lRetorno.GrupoItens = new List<RiscoGrupoItemInfo>();

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
                            lRetorno.GrupoItens.Add(new RiscoGrupoItemInfo()
                            {
                                CodigoGrupo     = lLinha["id_grupo"].DBToInt32(),
                                CodigoGrupoItem = lLinha["id_grupoitem"].DBToInt32(),
                                NomeGrupo       = lLinha["ds_grupo"].DBToString(),
                                NomeGrupoItem   = lLinha["ds_grupo_item"].DBToString(),
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

        public RiscoRemoverGrupoResponse RemoverGrupoRiscoSpider(RiscoRemoverGrupoRequest pRequest)
        {
            var lRetorno = new RiscoRemoverGrupoResponse();
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
                //LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                ///LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }
            return lRetorno;
        }

        public RiscoSalvarGrupoItemResponse SalvarGrupoItemSpider(RiscoSalvarGrupoItemRequest lRequest)
        {
            var lRetorno = new RiscoSalvarGrupoItemResponse();

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

                //LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
            }
            catch (Exception ex)
            {
                //LogRisco.Logar(lRequest, lRequest.IdUsuarioLogado, lRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRetorno.DescricaoResposta = ex.Message;
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return lRetorno;
        }

        public RiscoListarGruposResponse ListarGruposSpider(RiscoListarGruposRequest lRequest)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new RiscoListarGruposResponse();
            lRetorno.Grupos = new List<RiscoGrupoInfo>();
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
                            lRetorno.Grupos.Add(new RiscoGrupoInfo()
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

        public RiscoRemoverRegraGrupoItemResponse RemoverRegraGrupoItemSpider(RiscoRemoverRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new RiscoRemoverRegraGrupoItemResponse();
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

        public RiscoRemoverBloqueioInstumentoResponse RemoverBloqueioClienteInstrumentoDirecaoSpider(RiscoRemoverClienteBloqueioRequest pParametro)
        {
            var lRetorno = new RiscoRemoverBloqueioInstumentoResponse();
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

        public RiscoRemoverRegraGrupoItemResponse RemoverRegraGrupoItemGlobalSpider(RiscoRemoverRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new RiscoRemoverRegraGrupoItemResponse();
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

        public RiscoListarRegraGrupoItemResponse ListarRegraGrupoItemGlobalSpider(RiscoListarRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new RiscoListarRegraGrupoItemResponse();
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
                                new RiscoRegraGrupoItemInfo()
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

        public RiscoListarBloqueiroInstrumentoResponse ListarBloqueioClienteInstrumentoDirecaoSpider(RiscoListarBloqueiroInstrumentoRequest pParametro)
        {
            var lRetorno = new RiscoListarBloqueiroInstrumentoResponse();
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
                                new RiscoBloqueioInstrumentoInfo()
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

        public RiscoListarRegraGrupoItemResponse ListarRegraGrupoItemSpider(RiscoListarRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new RiscoListarRegraGrupoItemResponse();
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
                                new RiscoRegraGrupoItemInfo()
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

        private bool VerificaClienteBloqueioInstrumentoDirecao(string codigo)
        {
            var lAcessaDados = new AcessaDados();
            bool lRetorno = false;

            lAcessaDados.ConnectionStringName = "GradualSpider";

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
            catch { }


            return lRetorno;
        }

        public RiscoSalvarBloqueioInstrumentoResponse SalvarClienteBloqueioInstrumentoDirecaoSpider(RiscoSalvarBloqueioInstrumentoRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new RiscoSalvarBloqueioInstrumentoResponse();

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

        public RiscoSalvarRegraGrupoItemResponse SalvarRegraGrupoItemGlobalSpider(RiscoSalvarRegraGrupoItemRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new RiscoSalvarRegraGrupoItemResponse();

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

        public RiscoSalvarRegraGrupoItemResponse SalvarRegraGrupoItemSpider(RiscoSalvarRegraGrupoItemRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new RiscoSalvarRegraGrupoItemResponse();

            lAcessaDados.ConnectionStringName = "GradualSpider";

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

        public RiscoSalvarTravaExposicaoResponse SalvarTravaExposicaoSpider(RiscoSalvarTravaExposicaoRequest pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new RiscoSalvarTravaExposicaoResponse();

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
    }
}
