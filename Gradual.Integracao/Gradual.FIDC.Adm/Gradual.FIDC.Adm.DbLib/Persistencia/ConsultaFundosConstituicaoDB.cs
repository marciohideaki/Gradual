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
    /// Classe de gerenciamento de acesso ao banco de dados das informações de consulta de fundos em constituição
    /// </summary>
    public class ConsultaFundosConstituicaoDB
    {
        #region Propriedades
        public static readonly ILog GLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// Método que seleciona fundos no banco de dados de acordo com os parâmetros informados
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public ConsultaFundosConstituicaoResponse Buscar(ConsultaFundosConstituicaoRequest pRequest)
        {
            ConsultaFundosConstituicaoResponse lRetorno = new ConsultaFundosConstituicaoResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundo_fluxo_aprovacao_consulta_constituicao_sel"))
                {
                    #region Adicionar Parâmetros
                    //tratamento dos parâmetros de entrada                    
                    lAcessaDados.AddInParameter(cmd, "@SelecionarPendentes", DbType.Int32, pRequest.SelecionarPendentes);
                    lAcessaDados.AddInParameter(cmd, "@SelecionarConcluidos", DbType.Int32, pRequest.SelecionarConcluídos);

                    if (pRequest.DtDe != null)
                        lAcessaDados.AddInParameter(cmd, "@DataDe", DbType.DateTime, pRequest.DtDe.Value);
                    if (pRequest.DtAte != null)
                        lAcessaDados.AddInParameter(cmd, "@DataAte", DbType.DateTime, pRequest.DtAte.Value);

                    if (pRequest.IdFundoCadastro > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdFundoCadastro", DbType.Int32, pRequest.IdFundoCadastro);
                    if (pRequest.IdFundoFluxoGrupo > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdFundoFluxoGrupo", DbType.Int32, pRequest.IdFundoFluxoGrupo);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaConsultaFundos = new List<ConsultaFundosConstituicaoInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista = new ConsultaFundosConstituicaoInfo();

                        itemLista.IdFundoCadastro = dr["idFundoCadastro"].DBToInt32();
                        itemLista.NomeFundo = dr["nomeFundo"].DBToString();
                        itemLista.Grupo = dr["DsFundoFluxoGrupo"].DBToString();
                        itemLista.Etapa = dr["DsFundoFluxoGrupoEtapa"].DBToString();
                        itemLista.StatusEtapa = dr["DsFundoFluxoStatus"].DBToString();
                        itemLista.StatusGeral = dr["StatusGeral"].DBToString();

                        lRetorno.ListaConsultaFundos.Add(itemLista);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                GLogger.Error("Erro encontrado no método ConsultaFundosConstituicaoDB.Buscar", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que seleciona fundos no banco de dados de acordo com os parâmetros informados
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public ConsultaFundosConstituicaoResponse BuscarDadosGeraisFundo(ConsultaFundosConstituicaoRequest pRequest)
        {
            var lRetorno = new ConsultaFundosConstituicaoResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundo_fluxo_aprovacao_consulta_constituicao_geral_sel"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdFundoCadastro", DbType.Int32, pRequest.IdFundoCadastro);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaConsultaFundos = new List<ConsultaFundosConstituicaoInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista = new ConsultaFundosConstituicaoInfo();

                        itemLista.IdFundoCadastro = dr["idFundoCadastro"].DBToInt32();
                        itemLista.NomeFundo = dr["nomeFundo"].DBToString();
                        itemLista.Grupo = dr["DsFundoFluxoGrupo"].DBToString();
                        itemLista.Etapa = dr["DsFundoFluxoGrupoEtapa"].DBToString();
                        itemLista.StatusEtapa = dr["DsFundoFluxoStatus"].DBToString();
                        itemLista.StatusGeral = dr["StatusGeral"].DBToString();

                        lRetorno.ListaConsultaFundos.Add(itemLista);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                GLogger.Error("Erro encontrado no método ConsultaFundosConstituicaoDB.BuscarDadosGeraisFundo", ex);
            }

            return lRetorno;
        }

    }
}
