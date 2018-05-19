using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Globalization;
using Gradual.Generico.Dados;
using System.Data;
using System.Data.Common;
using Gradual.FIDC.Adm.DbLib.Mensagem;
using Gradual.FIDC.Adm.DbLib.Dados;
using Gradual.FIDC.Adm.DbLib.App_Codigo;

namespace Gradual.FIDC.Adm.DbLib.Persistencia
{
    /// <summary>
    /// Classe de gerenciamento de acesso ao banco de dados das informações de Titulos Liquidados
    /// </summary>
    public class TitulosLiquidadosDB
    {
        #region Propriedades
        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Titulos Liquidado
        /// <summary>
        /// Buscar Titulo Liquidado no banco de dados
        /// </summary>
        /// <param name="pRequest">Request de Titulos Liquidados</param>
        /// <returns>Retorna um objeto de Titulos Liquidados </returns>
        public TitulosLiquidadosResponse BuscarTitulosLiquidados(TitulosLiquidadosRequest pRequest)
        {
            var lRetorno = new TitulosLiquidadosResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_mec_busca_sel"))
                {
                    lAcessaDados.AddInParameter(cmd, "@CodigoFundo", DbType.Int32, pRequest.CodigoFundo);

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    foreach (DataRow dr in table.Rows)
                    {
                        var lCarteira = new TitulosLiquidadosInfo();

                        lCarteira.CodigoFundo   = dr["CodigoFundo"].DBToInt32();
                        lCarteira.DownloadLink  = dr["DownloadLink"].DBToString();
                        lCarteira.NomeFundo     = dr["NomeFundo"].DBToString();
                        lCarteira.Status        = dr["Status"].DBToString();

                        lRetorno.ListaTitulos.Add(lCarteira);
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método BuscarCarteiras", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Atualizar Titulo Liquidado no banco de dados
        /// </summary>
        /// <param name="pRequest">Request de Titulos Liquidados</param>
        /// <returns>Retorna um objeto de Titulos Liquidados </returns>
        public TitulosLiquidadosResponse AtualizarTitulosLiquidados(TitulosLiquidadosRequest pRequest)
        {
            var lRetorno = new TitulosLiquidadosResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_titulos_liquidados_busca_upd"))
                {
                    lAcessaDados.AddInParameter(cmd, "@CodigoFundo", DbType.Int32, pRequest.CodigoFundo);
                    lAcessaDados.AddInParameter(cmd, "@ValorLiquidacao", DbType.Decimal, pRequest.ValorLiquidacao);

                    lAcessaDados.ExecuteNonQuery(cmd);

                    lRetorno.DescricaoResposta = "Título do fundo " + pRequest.CodigoFundo + " atualizado com sucesso.";

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;


                    /*
                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    foreach (DataRow dr in table.Rows)
                    {
                        var lCarteira = new TitulosLiquidadosInfo();

                        lCarteira.CodigoFundo   = dr["CodigoFundo"].DBToInt32();
                        lCarteira.DownloadLink  = dr["DownloadLink"].DBToString();
                        lCarteira.NomeFundo     = dr["NomeFundo"].DBToString();
                        lCarteira.Status        = dr["Status"].DBToString();

                        lRetorno.ListaTitulos.Add(lCarteira);
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método BuscarCarteiras", ex);
            }

            return lRetorno;
        }

        #endregion


    }
}
