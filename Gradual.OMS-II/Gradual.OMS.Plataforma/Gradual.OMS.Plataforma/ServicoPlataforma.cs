using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Plataforma.Lib;
using Gradual.Generico.Dados;
using System.Data;
using System.Data.Common;
using Gradual.OMS.Library;
using System.ComponentModel;
using log4net;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Plataforma
{
    public class ServicoPlataforma : IServicoPlataforma, IServicoAvisoCliente
    {
        #region Propriedades
        private static readonly log4net.ILog gLogger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region IServicoPlataforma Members

        public BuscarUltimasMensagensDoHistoricoResponse BuscarUltimasMensagensDoHistorico(BuscarUltimasMensagensDoHistoricoRequest pRequest)
        {
            AcessaDados lDados   = null;
            DataTable   lTable   = null;
            DbCommand   lCommand = null;

            BuscarUltimasMensagensDoHistoricoResponse lResponse = new BuscarUltimasMensagensDoHistoricoResponse();

            try
            {
                List<string> lRetorno = new List<string>();

                lDados = new AcessaDados();

                lDados.ConnectionStringName = "MDS";

                lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_tbAtivoNegocioHistorico");

                lDados.AddInParameter(lCommand, "@TopX",   DbType.Int32,      pRequest.TopXRegistros);
                lDados.AddInParameter(lCommand, "@Ativos", DbType.AnsiString, pRequest.ListaDeAtivos);

                lTable = lDados.ExecuteDbDataTable(lCommand);

                foreach (DataRow dr in lTable.Rows)
                {
                    lRetorno.Add(dr["Mensagem"].DBToString());
                }

                lResponse.Mensagens = lRetorno;
                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.Error("BuscarUltimasMensagensDoHistorico(): " + ex.Message, ex);
                lResponse.DescricaoResposta = ex.StackTrace;
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lResponse;
        }

        public BuscarAtivoResponse BuscarAtivo(BuscarAtivoRequest pRequest)
        {
            AcessaDados lDados   = null;
            DataTable   lTable   = null;
            DbCommand   lCommand = null;

            BuscarAtivoResponse lResponse = new BuscarAtivoResponse();

            try
            {
                lDados = new AcessaDados();

                lDados.ConnectionStringName = "OMS";

                lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_TB_ATIVO_BUSCA");

                lDados.AddInParameter(lCommand, "@cd_negociacao", DbType.String, pRequest.CodigoAtivo);

                lTable = lDados.ExecuteDbDataTable(lCommand);

                if (lTable.Rows.Count > 0)
                {
                    lResponse.Ativo = new AtivoInfo();

                    lResponse.Ativo.IdAtivo          = lTable.Rows[0]["id_ativo"].DBToInt32();
                    lResponse.Ativo.IdTipoDeAtivo    = lTable.Rows[0]["id_ativo_tipo"].DBToInt32();
                    lResponse.Ativo.IdBolsa          = lTable.Rows[0]["id_bolsa"].DBToInt32();
                    lResponse.Ativo.CodigoNegociacao = lTable.Rows[0]["cd_negociacao"].DBToString();
                    lResponse.Ativo.ValorAbertura    = lTable.Rows[0]["vl_abertura"].DBToDecimal();
                    lResponse.Ativo.ValorAtual       = lTable.Rows[0]["vl_atual"].DBToDecimal();
                    lResponse.Ativo.ValorFechamento  = lTable.Rows[0]["vl_fechamento"].DBToDecimal();
                }

                return lResponse;
            }
            catch (Exception ex)
            {
                gLogger.Error("BuscarAtivo(): " + ex.Message, ex);
                throw ex;
            }
            finally
            {
                lDados = null;
                lTable = null;
                lCommand.Dispose();
                lCommand = null;
            }

        }

        public BuscarEmpresaDoAtivoResponse BuscarEmpresaDoAtivo(BuscarEmpresaDoAtivoRequest pRequest)
        {
            AcessaDados lDados   = null;
            DataTable   lTable   = null;
            DbCommand   lCommand = null;

            BuscarEmpresaDoAtivoResponse lResponse = new BuscarEmpresaDoAtivoResponse();

            try
            {
                lDados = new AcessaDados();
                lDados.ConnectionStringName = "SINACOR";

                lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "PRC_Nome_Empresa_Sel");

                lDados.AddInParameter(lCommand, "COD_NEG", DbType.String, pRequest.CodigoAtivo);

                lTable = lDados.ExecuteOracleDataTable(lCommand);

                if (lTable.Rows.Count > 0)
                {
                    lResponse.NomeDaEmpresa = lTable.Rows[0][0].DBToString();
                    lResponse.EspecificacaoDoPapel = lTable.Rows[0][1].DBToString();
                }
                else
                {
                    lResponse.NomeDaEmpresa = string.Empty;
                }

                return lResponse;
            }
            catch (Exception ex)
            {
                gLogger.Error("BuscarEmpresasDoAtivo(): " + ex.Message, ex);
                throw ex;
            }
            finally
            {
                lDados = null;
                lTable = null;
                lCommand.Dispose();
                lCommand = null;
            }
        }

        /// <summary>
        /// Método para retornar as corretoras cadastradas
        /// </summary>
        /// <param name="pParametros">Valores de busca</param>
        /// <returns>Retorno da busca</returns>
        public BuscarCorretorasResponse BuscarCorretoras(BuscarCorretorasRequest pParametros)
        {
            BuscarCorretorasResponse lRetorno = new BuscarCorretorasResponse();
            lRetorno.Corretoras = new BindingList<CorretoraInfo>();

            AcessaDados _AcessaDados = null;
            DataTable dtDados = null;
            DbCommand _DbCommand = null;

            try
            {
                _AcessaDados = new AcessaDados();
                _AcessaDados.ConnectionStringName = "DirectTradeRisco";

                _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_corretoras_lst");
                //_AcessaDados.CursorRetorno = "Retorno";

                dtDados = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow dr in dtDados.Rows)
                {
                    lRetorno.Corretoras.Add(new CorretoraInfo()
                    {
                        CodigoCorretora = dr["cdCorretora"].DBToInt32(),
                        Descricao = dr["dsCorretora"].DBToString()
                    });
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                gLogger.Error("BuscarCorretoras(): " + ex.Message, ex);
                throw ex;
            }
            finally
            {
                _AcessaDados = null;
                dtDados = null;
                _DbCommand.Dispose();
                _DbCommand = null;
            }
        }


        //public BuscarCorretorasResponse BuscarCorretoras(BuscarCorretorasRequest pRequest)
        //{
        //    BuscarCorretorasResponse lRetorno = new BuscarCorretorasResponse();

        //    lRetorno.Corretoras = new BindingList<CorretoraInfo>();

        //    AcessaDados lDados   = null;
        //    DataTable   lTable   = null;
        //    DbCommand   lCommand = null;

        //    try
        //    {
        //        lDados = new AcessaDados();
        //        lDados.ConnectionStringName = "SINACOR";

        //        lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "PRC_CORRETORAS_LST");

        //        lTable = lDados.ExecuteOracleDataTable(lCommand);

        //        CorretoraInfo lCorretora;

        //        foreach (DataRow dr in lTable.Rows)
        //        {
        //            lCorretora = new CorretoraInfo();

        //            lCorretora.CodigoCorretora = dr["COD_LOC"].DBToInt32();
        //            lCorretora.Descricao       = dr["DESC_LOC"].DBToString();

        //            lRetorno.Corretoras.Add(lCorretora);
        //        }

        //        return lRetorno;
        //    }
        //    finally
        //    {
        //        lDados = null;
        //        lTable = null;
        //        lCommand.Dispose();
        //        lCommand = null;
        //    }
        //}

        public BuscarTiposDeProventosResponse BuscarTiposDeProventos(BuscarTiposDeProventosRequest pRequest)
        {
            AcessaDados lDados = null;
            DataTable   lTable = null;
            DbCommand lCommand = null;

            BuscarTiposDeProventosResponse lRetorno = new BuscarTiposDeProventosResponse();
            
            try
            {
                lDados = new AcessaDados();
                lDados.ConnectionStringName = "SINACOR2";

                lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_TCFTIPO_PROV");

                lTable = lDados.ExecuteOracleDataTable(lCommand);

                lRetorno.TiposDeProvento = new List<TipoDeProventoInfo>();

                if (lTable.Rows.Count > 0)
                {
                    DataRow lRow;
                    TipoDeProventoInfo lTipo;

                    for (int i = 0; i < lTable.Rows.Count; i++)
                    {
                        lRow = lTable.Rows[i];
                        lTipo = new TipoDeProventoInfo();

                        lTipo.Codigo = lRow["TIPO_PROV"].DBToInt32();
                        lTipo.Descricao = lRow["DESC_TIPO_PROV"].DBToString();

                        lRetorno.TiposDeProvento.Add(lTipo);
                    }
                }

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.Error("BuscarTiposDeProventos(): " + ex.Message, ex);
                throw ex;
            }
            finally
            {
                lDados = null;
                lTable = null;
                lCommand.Dispose();
                lCommand = null;
            }

            return lRetorno;
        }

        public BuscarProventosResponse BuscarProventos(BuscarProventosRequest pRequest)
        {
            AcessaDados lDados = null;
            DataTable   lTable = null;
            DbCommand lCommand = null;

            BuscarProventosResponse lResponse = new BuscarProventosResponse();

            try
            {
                lDados = new AcessaDados();
                lDados.ConnectionStringName = "SINACOR2";

                lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_TCFPROVENTO");

                if (!string.IsNullOrEmpty(pRequest.Ativo))
                    lDados.AddInParameter(lCommand, "pcod_neg", DbType.String, pRequest.Ativo);

                if (pRequest.CodigoTipoDeProvento != null && pRequest.CodigoTipoDeProvento.Value > 0)
                    lDados.AddInParameter(lCommand, "pTipo_prov", DbType.String, pRequest.CodigoTipoDeProvento);

                lTable = lDados.ExecuteOracleDataTable(lCommand);

                if (lTable.Rows.Count > 0)
                {
                    DataRow lRow;

                    ProventoInfo lProvento;

                    for (int i = 0; i < lTable.Rows.Count; i++)
                    {
                        lRow = lTable.Rows[i];

                        lProvento = new ProventoInfo();

                        lProvento.Ativo     = lRow["cod_neg"].DBToString();
                        lProvento.DataPagto = lRow["data_pgto_divi"].DBToDateTime();
                        lProvento.Descricao = lRow["DESC_TIPO_PROV"].DBToString();
                        lProvento.Detalhes  = lRow["Detalhes"].DBToString();

                        lResponse.Proventos.Add(lProvento);
                    }

                    lResponse.DescricaoResposta = "Lista de proventos atualizada com sucesso.";
                    lResponse.StatusResposta    = MensagemResponseStatusEnum.OK;
                }
                else
                {
                    lResponse.DescricaoResposta = "Nenhum item encontrado para o filtro. Ativo: " + pRequest.Ativo + ", Tipo de Provento: " + pRequest.CodigoTipoDeProvento.Value;
                    lResponse.StatusResposta    = MensagemResponseStatusEnum.OK;
                }

                return lResponse;
            }
            catch (Exception ex)
            {
                gLogger.Error("BuscarProventos(): " + ex.Message, ex);
                lResponse.DescricaoResposta = ex.Message + "\n\n" + ex.StackTrace;
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
            }
            finally
            {
                lDados = null;
                lTable = null;
                lCommand.Dispose();
                lCommand = null;
            }

            return lResponse;
        }

        public BuscarAvisosResponse BuscarAvisos(BuscarAvisosRequest pRequest)
        {
            AcessaDados lDados   = null;
            DataTable   lTable   = null;
            DbCommand   lCommand = null;

            BuscarAvisosResponse lResponse = new BuscarAvisosResponse();

            try
            {
                lDados = new AcessaDados();
                lDados.ConnectionStringName = "Cadastro";

                lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_avisos_dia");

                lTable = lDados.ExecuteDbDataTable(lCommand);

                if (lTable.Rows.Count > 0)
                {
                    lResponse.Avisos = new List<AvisoInfo>();
                    
                    DataRow lRow;
                    AvisoInfo lAviso;

                    for (int i = 0; i < lTable.Rows.Count; i++)
                    {
                        lRow = lTable.Rows[i];

                        lAviso = new AvisoInfo();

                        lAviso.DataAviso = lRow["dt_entrada"].DBToDateTime();
                        lAviso.Mensagem = lRow["ds_aviso"].DBToString();

                        lResponse.Avisos.Add(lAviso);
                    }

                    lResponse.StatusResposta    = MensagemResponseStatusEnum.OK;
                    lResponse.DescricaoResposta = "Avisos encontrados com sucesso";
                }
                else
                {
                    lResponse.StatusResposta    = MensagemResponseStatusEnum.OK;
                    lResponse.DescricaoResposta = "Nenhum Aviso foi encontrado";
                }

                return lResponse;
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                lResponse.DescricaoResposta = ex.Message + "\n\n" + ex.StackTrace;
            }
            finally
            {
                lDados = null;
                lTable = null;
                lCommand.Dispose();
                lCommand = null;
            }

            return lResponse;
        }

        #endregion
        #region IServicoAvisoCliente Members

        public BuscarAvisoClienteResponse BuscarAvisosCliente(BuscarAvisosClienteRequest pRequest)
        {
            try
            {
                var lResposta = new BuscarAvisoClienteResponse();
                var lDataTable = new DataTable();
                var lAcessaDados = new AcessaDados();

                lResposta.ClienteAviso = new List<ClienteAvisoInfo>();

                lAcessaDados.ConnectionStringName = "Cadastro";

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_aviso_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_cliente", DbType.Int32, pRequest.CodigoCliente);

                    lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null)
                    {
                        foreach (DataRow lRow in lDataTable.Rows)
                        {
                            lResposta.ClienteAviso.Add(CriarRegistroClienteAviso(lRow));
                        }
                    }
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro em BuscarAvisosCliente:  ", ex);

                throw ex;
            }
        }

        public static ClienteAvisoInfo CriarRegistroClienteAviso(DataRow pRow)
        {
            ClienteAvisoInfo lRetorno = new ClienteAvisoInfo();

            lRetorno.IdAviso = pRow["id_aviso"].DBToInt32();

            lRetorno.IdSistema = pRow["id_sistema"].DBToInt32();

            lRetorno.DsAviso = pRow["ds_aviso"].DBToString();

            lRetorno.DtEntrada = pRow["dt_entrada"].DBToDateTime();

            lRetorno.DtSaida = pRow["dt_saida"].DBToDateTime();

            lRetorno.StAtivacaoManual = pRow["st_ativacaomanual"].DBToString().ToUpper();

            return lRetorno;
        }

        public MarcarAvisoLidoClienteResponse MarcarAvisoLidoCliente(MarcarAvisoLidoClienteRequest pRequest)
        {
            try
            {
                AcessaDados lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "Cadastro";

                MarcarAvisoLidoClienteResponse lResponse = new MarcarAvisoLidoClienteResponse();

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "aviso_lido_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_aviso", DbType.Int32, pRequest.ClienteAviso.IdAviso);

                    lAcessaDados.AddInParameter(lDbCommand, "@cd_cliente", DbType.Int32, int.Parse(pRequest.ClienteAviso.CodigoCliente));

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                return lResponse;
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro em MarcarAvisoLidoCliente:  ", ex);

                throw ex;
            }
        }

        #endregion
    }
}
