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
    public class CotistasFidcDb
    {
        #region Propriedades
        private static readonly ILog GLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        public CadastroCotistasFidcResponse Inserir(CadastroCotistasFidcRequest request)
        {
            var lRetorno = new CadastroCotistasFidcResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotista_fidc_ins"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@NomeCotista", DbType.String, request.NomeCotista);
                    lAcessaDados.AddInParameter(cmd, "@CpfCnpj", DbType.String, request.CpfCnpj);
                    lAcessaDados.AddInParameter(cmd, "@Email", DbType.String, request.Email);
                    lAcessaDados.AddInParameter(cmd, "@DataNascFundacao", DbType.Date, request.DataNascFundacao);
                    lAcessaDados.AddInParameter(cmd, "@IsAtivo", DbType.Boolean, request.IsAtivo);
                    lAcessaDados.AddInParameter(cmd, "@DtInclusao", DbType.DateTime2, System.DateTime.Now);
                    lAcessaDados.AddInParameter(cmd, "@QtdCotas", DbType.Int32, request.QuantidadeCotas);
                    lAcessaDados.AddInParameter(cmd, "@DsClasseCotas", DbType.String, request.ClasseCotas);
                    lAcessaDados.AddInParameter(cmd, "@DtVencimentoCadastro", DbType.DateTime2, request.DtVencimentoCadastro);
                    #endregion

                    request.IdCotistaFidc = Convert.ToInt32(lAcessaDados.ExecuteScalar(cmd));
                    
                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método CotistasFidcDb.Inserir", ex);
            }

            return lRetorno;
        }
                
        public CadastroCotistasFidcResponse Atualizar(CadastroCotistasFidcRequest request)
        {
            var lRetorno = new CadastroCotistasFidcResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotista_fidc_upd"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdCotistaFidc", DbType.String, request.IdCotistaFidc);
                    lAcessaDados.AddInParameter(cmd, "@NomeCotista", DbType.String, request.NomeCotista);
                    lAcessaDados.AddInParameter(cmd, "@CpfCnpj", DbType.String, request.CpfCnpj);
                    lAcessaDados.AddInParameter(cmd, "@Email", DbType.String, request.Email);
                    lAcessaDados.AddInParameter(cmd, "@DataNascFundacao", DbType.Date, request.DataNascFundacao);
                    lAcessaDados.AddInParameter(cmd, "@IsAtivo", DbType.Boolean, request.IsAtivo);
                    lAcessaDados.AddInParameter(cmd, "@QtdCotas", DbType.Int32, request.QuantidadeCotas);
                    lAcessaDados.AddInParameter(cmd, "@DsClasseCotas", DbType.String, request.ClasseCotas);
                    lAcessaDados.AddInParameter(cmd, "@DtVencimentoCadastro", DbType.DateTime2, request.DtVencimentoCadastro);
                    #endregion

                    lAcessaDados.ExecuteNonQuery(cmd);

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método CotistasFidcDb.Atualizar", ex);
            }

            return lRetorno;
        }
                
        public CadastroCotistasFidcResponse SelecionarLista(CadastroCotistasFidcRequest pRequest)
        {
            var lRetorno = new CadastroCotistasFidcResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotista_fidc_sel"))
                {
                    #region Adicionar Parâmetros
                    //tratamento dos parâmetros de entrada
                    if (pRequest.IdCotistaFidc > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdCotistaFidc", DbType.Int32, pRequest.IdCotistaFidc);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaCotistaFidc = new List<CadastroCotistasFidcInfo>();

                    #region Preenchimento Retorno
                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista = new CadastroCotistasFidcInfo
                        {
                            IdCotistaFidc = dr["IdCotistaFidc"].DBToInt32(),
                            NomeCotista = dr["NomeCotista"].DBToString(),
                            CpfCnpj = dr["CpfCnpj"].DBToString(),
                            Email = dr["Email"].DBToString(),
                            DataNascFundacao = dr["DataNascFundacao"].DBToDateTime(),
                            IsAtivo = dr["IsAtivo"].DBToBoolean(),
                            DtInclusao = dr["DtInclusao"].DBToDateTime(),
                            DataNascFundacaoFormatada = dr["DataNascFundacao"].DBToDateTime().ToString("dd/MM/yyyy"),
                            QuantidadeCotas = dr["QtdCotas"].DBToInt32(),
                            ClasseCotas = dr["DsClasseCotas"].DBToString(),
                            DtVencimentoCadastro = dr["DtVencimentoCadastro"].DBToDateTime(),
                            DtVencimentoCadastroFormatada = dr["DtVencimentoCadastro"].DBToDateTime().ToString("dd/MM/yyyy")                            
                        };
                        
                        lRetorno.ListaCotistaFidc.Add(itemLista);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                GLogger.Error("Erro encontrado no método CotistasFidcDb.Buscar", ex);

                throw ex;
            }

            return lRetorno;
        }
    }
}
