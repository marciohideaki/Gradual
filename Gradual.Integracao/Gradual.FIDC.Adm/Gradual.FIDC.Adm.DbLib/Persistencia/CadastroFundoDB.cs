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
    /// <summary>
    /// Classe de gerenciamento de acesso ao banco de dados das informações de fundos
    /// </summary>
    public class CadastroFundoDB
    {
        #region Propriedades
        private static readonly ILog GLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// Método que insere um novo fundo no banco de dados
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CadastroFundoResponse Inserir(CadastroFundoRequest request)
        {
            var lRetorno = new CadastroFundoResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundos_cadastro_ins"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@nomeFundo", DbType.String, request.NomeFundo);
                    lAcessaDados.AddInParameter(cmd, "@cnpjFundo", DbType.String, request.CNPJFundo);
                    lAcessaDados.AddInParameter(cmd, "@nomeAdministrador", DbType.String, request.NomeAdministrador);
                    lAcessaDados.AddInParameter(cmd, "@cnpjAdministrador", DbType.String, request.CNPJAdministrador);
                    lAcessaDados.AddInParameter(cmd, "@nomeCustodiante", DbType.String, request.NomeCustodiante);
                    lAcessaDados.AddInParameter(cmd, "@cnpjCustodiante", DbType.String, request.CNPJCustodiante);
                    lAcessaDados.AddInParameter(cmd, "@nomeGestor", DbType.String, request.NomeGestor);
                    lAcessaDados.AddInParameter(cmd, "@cnpjGestor", DbType.String, request.CNPJGestor);
                    lAcessaDados.AddInParameter(cmd, "@TxGestao", DbType.Decimal, request.TxGestao);
                    lAcessaDados.AddInParameter(cmd, "@TxCustodia", DbType.Decimal, request.TxCustodia);
                    lAcessaDados.AddInParameter(cmd, "@TxConsultoria", DbType.Decimal, request.TxConsultoria);
                    lAcessaDados.AddInParameter(cmd, "@isAtivo", DbType.Boolean, request.IsAtivo);
                    #endregion

                    request.IdFundoCadastro = Convert.ToInt32(lAcessaDados.ExecuteScalar(cmd));
                    
                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método CadastroFundoDB.Inserir", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que atualiza um fundo no banco de dados
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CadastroFundoResponse Atualizar(CadastroFundoRequest request)
        {
            var lRetorno = new CadastroFundoResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};
                
                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundos_cadastro_upd"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@idFundoCadastro", DbType.String, request.IdFundoCadastro);
                    lAcessaDados.AddInParameter(cmd, "@nomeFundo", DbType.String, request.NomeFundo);
                    lAcessaDados.AddInParameter(cmd, "@cnpjFundo", DbType.String, request.CNPJFundo);
                    lAcessaDados.AddInParameter(cmd, "@nomeAdministrador", DbType.String, request.NomeAdministrador);
                    lAcessaDados.AddInParameter(cmd, "@cnpjAdministrador", DbType.String, request.CNPJAdministrador);
                    lAcessaDados.AddInParameter(cmd, "@nomeCustodiante", DbType.String, request.NomeCustodiante);
                    lAcessaDados.AddInParameter(cmd, "@cnpjCustodiante", DbType.String, request.CNPJCustodiante);
                    lAcessaDados.AddInParameter(cmd, "@nomeGestor", DbType.String, request.NomeGestor);
                    lAcessaDados.AddInParameter(cmd, "@cnpjGestor", DbType.String, request.CNPJGestor);
                    lAcessaDados.AddInParameter(cmd, "@TxGestao", DbType.Decimal, request.TxGestao);
                    lAcessaDados.AddInParameter(cmd, "@TxCustodia", DbType.Decimal, request.TxCustodia);
                    lAcessaDados.AddInParameter(cmd, "@TxConsultoria", DbType.Decimal, request.TxConsultoria);
                    lAcessaDados.AddInParameter(cmd, "@isAtivo", DbType.String, request.IsAtivo);
                    #endregion

                    lAcessaDados.ExecuteNonQuery(cmd);

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método CadastroFundoDB.Atualizar", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que seleciona fundos no banco de dados de acordo com os parâmetros informados
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public CadastroFundoResponse Buscar(CadastroFundoRequest pRequest)
        {
            CadastroFundoResponse lRetorno = new CadastroFundoResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};
                
                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundos_cadastro_sel"))
                {
                    #region Adicionar Parâmetros
                    //tratamento dos parâmetros de entrada
                    if (pRequest.IdFundoCadastro > 0)
                        lAcessaDados.AddInParameter(cmd, "@idFundoCadastro", DbType.Int32, pRequest.IdFundoCadastro);
                    if (pRequest.NomeFundo != null && pRequest.NomeFundo.Length > 1)
                        lAcessaDados.AddInParameter(cmd, "@nomeFundo", DbType.String, pRequest.NomeFundo);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaFundos = new List<CadastroFundoInfo>();

                    #region Preenchimento Retorno
                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista = new CadastroFundoInfo
                        {
                            IdFundoCadastro = dr["idFundoCadastro"].DBToInt32(),
                            NomeFundo = dr["nomeFundo"].DBToString(),
                            CNPJFundo  = dr["cnpjFundo"].DBToString(),
                            NomeAdministrador = dr["nomeAdministrador"].DBToString(),
                            CNPJAdministrador = dr["cnpjAdministrador"].DBToString(),
                            NomeCustodiante = dr["nomeCustodiante"].DBToString(),
                            CNPJCustodiante = dr["cnpjCustodiante"].DBToString(),
                            NomeGestor = dr["nomeGestor"].DBToString(),
                            CNPJGestor = dr["cnpjGestor"].DBToString(),
                            TxGestao = dr["TxGestao"].DBToDecimal(),
                            TxCustodia = dr["TxCustodia"].DBToDecimal(),
                            TxConsultoria = dr["TxConsultoria"].DBToDecimal(),
                            IsAtivo = dr["isAtivo"].DBToBoolean()
                        };
                        
                        lRetorno.ListaFundos.Add(itemLista);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                GLogger.Error("Erro encontrado no método CadastroFundoDB.Buscar", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Insere dados na tabela de log de transações
        /// </summary>
        /// <param name="request"></param>
        /// <param name="tipoTransacao"></param>
        /// <param name="usuarioLogado"></param>
        /// <returns></returns>
        public CadastroFundoResponse InserirLog(CadastroFundoRequest request, string tipoTransacao, string usuarioLogado)
        {
            var lRetorno = new CadastroFundoResponse();
            
            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};
                
                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundos_cadastro_log_ins"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@idFundoCadastro", DbType.String, request.IdFundoCadastro);
                    lAcessaDados.AddInParameter(cmd, "@nomeFundo", DbType.String, request.NomeFundo);
                    lAcessaDados.AddInParameter(cmd, "@cnpjFundo", DbType.String, request.CNPJFundo);
                    lAcessaDados.AddInParameter(cmd, "@nomeAdministrador", DbType.String, request.NomeAdministrador);
                    lAcessaDados.AddInParameter(cmd, "@cnpjAdministrador", DbType.String, request.CNPJAdministrador);
                    lAcessaDados.AddInParameter(cmd, "@nomeCustodiante", DbType.String, request.NomeCustodiante);
                    lAcessaDados.AddInParameter(cmd, "@cnpjCustodiante", DbType.String, request.CNPJCustodiante);
                    lAcessaDados.AddInParameter(cmd, "@nomeGestor", DbType.String, request.NomeGestor);
                    lAcessaDados.AddInParameter(cmd, "@cnpjGestor", DbType.String, request.CNPJGestor);
                    lAcessaDados.AddInParameter(cmd, "@isAtivo", DbType.Boolean, request.IsAtivo);
                    lAcessaDados.AddInParameter(cmd, "@tipoTransacao", DbType.String, tipoTransacao);
                    lAcessaDados.AddInParameter(cmd, "@usuarioLogado", DbType.String, usuarioLogado);
                    #endregion
                    
                    lAcessaDados.ExecuteNonQuery(cmd);

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método CadastroFundoDB.InserirLog", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que seleciona fundos no banco de dados de acordo com os parâmetros informados
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public CadastroFundoResponse BuscarPorCategoria(CadastroFundoRequest pRequest)
        {
            CadastroFundoResponse lRetorno = new CadastroFundoResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};
                
                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundos_cadastro_categoria_sub_categoria_sel"))
                {
                    #region Adicionar Parâmetros
                    //tratamento dos parâmetros de entrada
                    if (pRequest.IdFundoCadastro > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdFundoCategoria", DbType.Int32, pRequest.IdFundoCadastro);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaFundos = new List<CadastroFundoInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista = new CadastroFundoInfo
                        {
                            IdFundoCadastro = dr["idFundoCadastro"].DBToInt32(),
                            NomeFundo = dr["nomeFundo"].DBToString()
                        };
                        
                        lRetorno.ListaFundos.Add(itemLista);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                GLogger.Error("Erro encontrado no método CadastroFundoDB.BuscarPorCategoria", ex);

                throw;
            }

            return lRetorno;
        }
    }
}
