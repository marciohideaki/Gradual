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
    public class FundoCategoriaSubCategoriaDB
    {
        #region Propriedades
        public static readonly ILog GLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// Busca fundos que pertencem a uma relação categoria x subcategoria
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public FundoCategoriaSubCategoriaResponse BuscarFundosPorCategoriaXSubCategoria(FundoCategoriaSubCategoriaRequest request)
        {
            var lRetorno = new FundoCategoriaSubCategoriaResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};
                
                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundo_categoria_sub_categoria_sel"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdFundoCategoria", DbType.String, request.IdFundoCategoria);
                    lAcessaDados.AddInParameter(cmd, "@IdFundoSubCategoria", DbType.String, request.IdFundoSubCategoria);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaFundos = new List<CadastroFundoInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista = new CadastroFundoInfo();

                        itemLista.IdFundoCadastro = dr["IdFundoCadastro"].DBToInt32();
                        itemLista.NomeFundo = dr["NomeFundo"].DBToString();

                        lRetorno.ListaFundos.Add(itemLista);
                    }
                    #endregion

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método FundoCategoriaDB.Buscar", ex);
            }

            return lRetorno;
        }
        /// <summary>
        /// Busca todos os fundos a partir da categoria e subcategoria informadas, além de trazer todos os demais fundos, utilizando a flag 'Pertence' para distinguir quais pertencem
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public FundoCategoriaSubCategoriaResponse BuscarFundosPorCategoriaXSubCategoriaTodos(FundoCategoriaSubCategoriaRequest request)
        {
            var lRetorno = new FundoCategoriaSubCategoriaResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundos_por_categoria_subcategoria_sel_todos"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdFundoCategoria", DbType.String, request.IdFundoCategoria);
                    lAcessaDados.AddInParameter(cmd, "@IdFundoSubCategoria", DbType.String, request.IdFundoSubCategoria);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaFundos = new List<CadastroFundoInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista = new CadastroFundoInfo();

                        itemLista.IdFundoCadastro = dr["IdFundoCadastro"].DBToInt32();
                        itemLista.NomeFundo = dr["NomeFundo"].DBToString();
                        itemLista.Pertence = (dr["Pertence"].DBToInt32() == 1);

                        lRetorno.ListaFundos.Add(itemLista);
                    }
                    #endregion

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método FundoCategoriaDB.Buscar", ex);
            }

            return lRetorno;
        }
        /// <summary>
        /// Insere uma relação fundo, categoria, subcategoria
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public FundoCategoriaSubCategoriaResponse Inserir(FundoCategoriaSubCategoriaRequest request)
        {
            var lRetorno = new FundoCategoriaSubCategoriaResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundo_categoria_sub_categoria_ins"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdFundoCadastro", DbType.String, request.IdFundoCadastro);
                    lAcessaDados.AddInParameter(cmd, "@IdFundoCategoria", DbType.String, request.IdFundoCategoria);
                    lAcessaDados.AddInParameter(cmd, "@IdFundoSubCategoria", DbType.String, request.IdFundoSubCategoria);
                    #endregion

                    request.IdFundoCategoriaSubCategoria = Convert.ToInt32(lAcessaDados.ExecuteScalar(cmd));

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método FundoCategoriaSubCategoriaDB.Inserir", ex);
            }

            return lRetorno;
        }
        /// <summary>
        /// Remove uma relação fundo, categoria, subcategoria
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public FundoCategoriaSubCategoriaResponse Remover(FundoCategoriaSubCategoriaRequest request)
        {
            var lRetorno = new FundoCategoriaSubCategoriaResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundo_categoria_sub_categoria_del"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdFundoCadastro", DbType.String, request.IdFundoCadastro);
                    lAcessaDados.AddInParameter(cmd, "@IdFundoCategoria", DbType.String, request.IdFundoCategoria);
                    lAcessaDados.AddInParameter(cmd, "@IdFundoSubCategoria", DbType.String, request.IdFundoSubCategoria);
                    #endregion

                    lAcessaDados.ExecuteNonQuery(cmd);

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método FundoCategoriaSubCategoriaDB.Remover", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método de inserção de log no banco de dados
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public FundoCategoriaSubCategoriaLogResponse InserirLog(FundoCategoriaSubCategoriaLogRequest request)
        {
            var lRetorno = new FundoCategoriaSubCategoriaLogResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};
                
                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundo_categoria_sub_categoria_log_ins"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdFundoCadastro", DbType.Int32, request.IdFundoCadastro);
                    lAcessaDados.AddInParameter(cmd, "@IdFundoCategoria", DbType.Int32, request.IdFundoCategoria);
                    lAcessaDados.AddInParameter(cmd, "@IdFundoSubCategoria", DbType.Int32, request.IdFundoSubCategoria);
                    lAcessaDados.AddInParameter(cmd, "@UsuarioLogado", DbType.String, request.UsuarioLogado);
                    lAcessaDados.AddInParameter(cmd, "@DtAlteracao", DbType.DateTime2, request.DtAlteracao);
                    lAcessaDados.AddInParameter(cmd, "@TipoTransacao", DbType.String, request.TipoTransacao);                    
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    //preenche response
                    foreach (DataRow row in table.Rows)
	                {
		                lRetorno.NomeFundo = row["NomeFundo"].DBToString();
                        lRetorno.DsFundoCategoria = row["DsFundoCategoria"].DBToString();
                        lRetorno.DsFundoSubCategoria = row["DsFundoSubCategoria"].DBToString();
                    }

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método FundoCategoriaSubCategoriaDB.InserirLog", ex);
            }

            return lRetorno;
        }
    }
}
