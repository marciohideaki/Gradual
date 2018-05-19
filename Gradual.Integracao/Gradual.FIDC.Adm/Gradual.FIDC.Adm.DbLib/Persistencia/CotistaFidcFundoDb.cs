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
    public class CotistaFidcFundoDb
    {
        #region Propriedades
        private static readonly ILog GLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public AssociacaoCotistaFidcFundoResponse Inserir(AssociacaoCotistaFidcFundoRequest request)
        {
            var lRetorno = new AssociacaoCotistaFidcFundoResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotista_fidc_fundo_ins"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdCotistaFidc", DbType.Int32, request.IdCotistaFidc);
                    lAcessaDados.AddInParameter(cmd, "@IdFundoCadastro", DbType.Int32, request.IdFundoCadastro);
                    lAcessaDados.AddInParameter(cmd, "@DtInclusao", DbType.DateTime2, request.DtInclusao);
                    #endregion

                    request.IdCotistaFidcFundo = Convert.ToInt32(lAcessaDados.ExecuteScalar(cmd));
                    
                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método CotistaFidcFundoDb.Inserir", ex);
            }

            return lRetorno;
        }

        public AssociacaoCotistaFidcFundoResponse Remover(AssociacaoCotistaFidcFundoRequest request)
        {
            var lRetorno = new AssociacaoCotistaFidcFundoResponse();

            try
            {
                var lAcessaDados = new AcessaDados { ConnectionStringName = "GradualFundosAdm" };

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotista_fidc_fundo_del"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdCotistaFidcFundo", DbType.Int32, request.IdCotistaFidcFundo);
                    #endregion

                    Convert.ToInt32(lAcessaDados.ExecuteScalar(cmd));

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método CotistaFidcFundoDb.Remover", ex);
            }

            return lRetorno;
        }

        public AssociacaoCotistaFidcFundoResponse SelecionarListaGrid(AssociacaoCotistaFidcFundoRequest pRequest)
        {
            var lRetorno = new AssociacaoCotistaFidcFundoResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotista_fidc_fundo_sel"))
                {
                    #region Adicionar Parâmetros
                    //tratamento dos parâmetros de entrada
                    if (pRequest.IdCotistaFidcFundo > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdCotistaFidcFundo", DbType.Int32, pRequest.IdCotistaFidcFundo);
                    if (pRequest.IdCotistaFidc > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdCotistaFidc", DbType.Int32, pRequest.IdCotistaFidc);
                    if (pRequest.IdFundoCadastro > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdFundoCadastro", DbType.Int32, pRequest.IdFundoCadastro);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaCotistaFidcFundo = new List<CotistaFidcFundoInfo>();

                    #region Preenchimento Retorno
                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista = new CotistaFidcFundoInfo
                        {
                            IdCotistaFidcFundo = dr["IdCotistaFidcFundo"].DBToInt32(),
                            NomeCotista = dr["NomeCotista"].DBToString(),
                            NomeFundo = dr["nomeFundo"].DBToString(),
                            EmailCotista = dr["EmailCotista"].DBToString(),
                        };
                        
                        lRetorno.ListaCotistaFidcFundo.Add(itemLista);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                GLogger.Error("Erro encontrado no método CotistaFidcFundoDb.SelecionarListaGrid", ex);

                throw ex;
            }

            return lRetorno;
        }
    }
}
