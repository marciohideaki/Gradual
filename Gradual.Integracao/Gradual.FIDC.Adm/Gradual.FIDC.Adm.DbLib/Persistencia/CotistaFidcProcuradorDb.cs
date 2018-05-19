using System;
using System.Collections.Generic;
using log4net;
using Gradual.Generico.Dados;
using System.Data;
using Gradual.FIDC.Adm.DbLib.Mensagem;
using Gradual.FIDC.Adm.DbLib.Dados;
using Gradual.FIDC.Adm.DbLib.App_Codigo;

namespace Gradual.FIDC.Adm.DbLib.Persistencia
{
    public class CotistaFidcProcuradorDb
    {
        #region Propriedades
        private static readonly ILog GLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        public CotistaFidcProcuradorResponse Inserir(CotistaFidcProcuradorRequest request)
        {
            var lRetorno = new CotistaFidcProcuradorResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotista_fidc_procurador_ins"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdCotistaFidc", DbType.Int32, request.IdCotistaFidc);
                    lAcessaDados.AddInParameter(cmd, "@NomeProcurador", DbType.String, request.NomeProcurador);
                    lAcessaDados.AddInParameter(cmd, "@CPF", DbType.String, request.CPF);
                    #endregion

                    request.IdCotistaFidcProcurador = Convert.ToInt32(lAcessaDados.ExecuteScalar(cmd));
                    
                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método CotistaFidcProcuradorDb.Inserir", ex);

                throw ex;
            }

            return lRetorno;
        }
        
        public CotistaFidcProcuradorResponse Atualizar(CotistaFidcProcuradorRequest request)
        {
            var lRetorno = new CotistaFidcProcuradorResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotista_fidc_procurador_upd"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdCotistaFidcProcurador", DbType.String, request.IdCotistaFidcProcurador);
                    lAcessaDados.AddInParameter(cmd, "@IdCotistaFidc", DbType.Int32, request.IdCotistaFidc);
                    lAcessaDados.AddInParameter(cmd, "@NomeProcurador", DbType.String, request.NomeProcurador);
                    lAcessaDados.AddInParameter(cmd, "@CPF", DbType.String, request.CPF);                    
                    #endregion

                    lAcessaDados.ExecuteNonQuery(cmd);

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método CotistaFidcProcuradorDb.Atualizar", ex);

                throw ex;
            }

            return lRetorno;
        }
        
        public CotistaFidcProcuradorResponse SelecionarLista(CotistaFidcProcuradorRequest pRequest)
        {
            var lRetorno = new CotistaFidcProcuradorResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotista_fidc_procurador_sel"))
                {
                    #region Adicionar Parâmetros
                    //tratamento dos parâmetros de entrada
                    if (pRequest.IdCotistaFidcProcurador > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdCotistaFidcProcurador", DbType.Int32, pRequest.IdCotistaFidcProcurador);
                    if (pRequest.IdCotistaFidc > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdCotistaFidc", DbType.Int32, pRequest.IdCotistaFidc);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaCotistaFidcProcurador = new List<CotistaFidcProcuradorInfo>();
                    
                    #region Preenchimento Retorno
                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista = new CotistaFidcProcuradorInfo
                        {
                            IdCotistaFidcProcurador = dr["IdCotistaFidcProcurador"].DBToInt32(),
                            IdCotistaFidc = dr["IdCotistaFidc"].DBToInt32(),
                            NomeProcurador = dr["NomeProcurador"].DBToString(),
                            CPF = dr["CPF"].DBToString()
                        };
                        
                        lRetorno.ListaCotistaFidcProcurador.Add(itemLista);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                GLogger.Error("Erro encontrado no método CotistaFidcProcuradorDb.SelecionarLista", ex);

                throw ex;
            }

            return lRetorno;
        }

        public CotistaFidcProcuradorResponse Excluir(CotistaFidcProcuradorRequest request)
        {
            var lRetorno = new CotistaFidcProcuradorResponse();

            try
            {
                var lAcessaDados = new AcessaDados { ConnectionStringName = "GradualFundosAdm" };

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotista_fidc_procurador_del"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdCotistaFidcProcurador", DbType.Int32, request.IdCotistaFidcProcurador);
                    #endregion

                    lAcessaDados.ExecuteNonQuery(cmd);

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método CotistaFidcProcuradorDb.Excluir", ex);

                throw ex;
            }

            return lRetorno;
        }
    }
}
