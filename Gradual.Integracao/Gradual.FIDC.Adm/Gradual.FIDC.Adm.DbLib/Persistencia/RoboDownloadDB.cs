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
    /// Classe de gerenciamento de acesso ao banco de dados das informações de carteiras
    /// </summary>
    public class RoboDownloadDB
    {
        #region Propriedades
        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Carteiras
        /// <summary>
        /// Buscar carteiras no banco de dados
        /// </summary>
        /// <param name="pRequest">Request de carteiras</param>
        /// <returns>Retorna um objeto de Lista de carteiras</returns>
        public CarteiraResponse BuscarCarteiras(CarteiraRequest pRequest)
        {
            var lRetorno = new CarteiraResponse();

            try
            {

                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_carteiras_busca_sel"))
                {
                    lAcessaDados.AddInParameter(cmd, "@CodigoFundo",        DbType.Int32,       pRequest.CodigoFundo);
                    lAcessaDados.AddInParameter(cmd, "@CodigoLocalidade",   DbType.Int32,       pRequest.CodigoLocalidade);
                    lAcessaDados.AddInParameter(cmd, "@DataDe",             DbType.DateTime,    pRequest.DataDe);
                    lAcessaDados.AddInParameter(cmd, "@DataAte",            DbType.DateTime,    pRequest.DataAte);
                    lAcessaDados.AddInParameter(cmd, "@DownloadPendentes",  DbType.String,      pRequest.DownloadsPendentes.ToString());
                    lAcessaDados.AddInParameter(cmd, "@NomeFundo",          DbType.String,      pRequest.NomeFundo);

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    foreach (DataRow dr in table.Rows)
                    {
                        var lCarteira = new CarteirasInfo();

                        lCarteira.Categoria         = dr["Categoria"].ToString();
                        lCarteira.CodigoFundo       = dr["CodigoFundo"].DBToInt32();
                        lCarteira.CodigoLocalidade  = dr["CodigoLocalidade"].DBToInt32();
                        lCarteira.DownloadHora      = dr["DownloadHora"].DBToDateTime();
                        lCarteira.DownloadLink      = dr["DownloadLink"].DBToString();
                        lCarteira.NomeFundo         = dr["NomeFundo"].DBToString();
                        lCarteira.Status            = dr["Status"].DBToString();
                        
                        lRetorno.ListaCarteira.Add(lCarteira);
                    }

                    lRetorno.DescricaoResposta = "Encontrou " + lRetorno.ListaCarteira.Count + " Carteiras.";

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                gLogger.Error("Erro encontrado no método BuscarCarteiras", ex);
            }

            return lRetorno;
        }
        #endregion

        #region Extrato Cotista
        /// <summary>
        /// Lista de Cotistas
        /// </summary>
        /// <returns>REtorna um objeto com uma lista de cotistas</returns>
        public ExtratoCotistaResponse BuscarListaCotistas()
        {
            var lRetorno = new ExtratoCotistaResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotista_busca_lst"))
                {
                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    foreach (DataRow dr in table.Rows)
                    {
                        var lCarteira = new ListaCotistaInfo();

                        lCarteira.CodigoCotista = dr["CodigoCotista"].ToString();
                        lCarteira.NomeCotista   = dr["NomeCotista"].DBToString();

                        lRetorno.ListaCotista.Add(lCarteira);
                    }

                    lRetorno.DescricaoResposta = "Encontrou " + lRetorno.ListaCotista.Count + " Cotistas.";

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                gLogger.Error("Erro encontrado no método BuscarListaFundos", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Extrato de Cotista no banco de dados
        /// </summary>
        /// <param name="pRequest">Request de Extrato de cotista</param>
        /// <returns>Retorna um objeto de Lista de extrato de cotista</returns>
        public ExtratoCotistaResponse BuscarExtratoCotista(ExtratoCotistaRequest pRequest)
        {
            var lRetorno = new ExtratoCotistaResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_extrato_busca_sel"))
                {
                    lAcessaDados.AddInParameter(cmd, "@CpfCnpj",            DbType.String,      pRequest.CpfCnpj);
                    lAcessaDados.AddInParameter(cmd, "@DataDe",             DbType.DateTime,    pRequest.DataDe);
                    lAcessaDados.AddInParameter(cmd, "@DataAte",            DbType.DateTime,    pRequest.DataAte);
                    lAcessaDados.AddInParameter(cmd, "@DownloadPendentes",  DbType.String,      pRequest.DownloadPendentes.ToString());
                    lAcessaDados.AddInParameter(cmd, "@CodigoCotista",      DbType.Int32,       pRequest.CodigoCotista);

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    foreach (DataRow dr in table.Rows)
                    {
                        var lCarteira = new ExtratoCotistaInfo();

                        lCarteira.CpfCnpj       = dr["CpfCnpj"].ToString();
                        lCarteira.NomeCotista   = dr["NomeCotista"].ToString();
                        lCarteira.CodigoFundo   = dr["CodigoFundo"].DBToInt32();
                        lCarteira.DownloadLink  = dr["DownloadLink"].DBToString();
                        lCarteira.NomeFundo     = dr["NomeFundo"].DBToString();
                        lCarteira.Status        = dr["Status"].DBToString();

                        lRetorno.ListaExtrato.Add(lCarteira);
                    }

                    lRetorno.DescricaoResposta = "Encontrou " + lRetorno.ListaExtrato.Count + " Posições de extrato de cotista.";

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                gLogger.Error("Erro encontrado no método BuscarCarteiras", ex);
            }

            return lRetorno;
        }
        #endregion

        #region Mec
        /// <summary>
        /// Buscar Mec no banco de dados
        /// </summary>
        /// <param name="pRequest">Request de Mapa de evolução de cotas</param>
        /// <returns>Retorna um objeto de Mapa de evolução de cotista</returns>
        public MecResponse BuscarMec(MecRequest pRequest)
        {
            var lRetorno = new MecResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_mec_busca_sel"))
                {
                    lAcessaDados.AddInParameter(cmd, "@CodigoFundo",        DbType.Int32, pRequest.CodigoFundo);
                    lAcessaDados.AddInParameter(cmd, "@CodigoLocalidade",   DbType.Int32, pRequest.CodigoLocalidade);
                    lAcessaDados.AddInParameter(cmd, "@DataDe",             DbType.DateTime, pRequest.DataDe);
                    lAcessaDados.AddInParameter(cmd, "@DataAte",            DbType.DateTime, pRequest.DataAte);
                    lAcessaDados.AddInParameter(cmd, "@DownloadPendentes",  DbType.String, pRequest.DownloadsPendentes.ToString());
                    lAcessaDados.AddInParameter(cmd, "@NomeFundo",          DbType.String, pRequest.NomeFundo);

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    foreach (DataRow dr in table.Rows)
                    {
                        var lCarteira = new MecInfo();

                        lCarteira.Categoria         = dr["Categoria"].ToString();
                        lCarteira.CodigoFundo       = dr["CodigoFundo"].DBToInt32();
                        lCarteira.CodigoLocalidade  = dr["CodigoLocalidade"].DBToInt32();
                        lCarteira.DownloadHora      = dr["DownloadHora"].DBToDateTime();
                        lCarteira.DownloadLink      = dr["DownloadLink"].DBToString();
                        lCarteira.NomeFundo         = dr["NomeFundo"].DBToString();
                        lCarteira.Status            = dr["Status"].DBToString();

                        lRetorno.ListaMec.Add(lCarteira);
                    }

                    lRetorno.DescricaoResposta = "Encontrou " + lRetorno.ListaMec.Count + " Mapas de evolução de cotas.";

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                gLogger.Error("Erro encontrado no método BuscarCarteiras", ex);
            }

            return lRetorno;
        }
        #endregion

        #region Titulos Liquidado
        /// <summary>
        /// Buscar Títulos Liquidado no banco de dados
        /// </summary>
        /// <param name="pRequest">Request de Títulos Liquidados</param>
        /// <returns>Retorna um objeto de Títulos Liquidados</returns>
        public TitulosLiquidadosResponse BuscarTitulosLiquidados(TitulosLiquidadosRequest pRequest)
        {
            var lRetorno = new TitulosLiquidadosResponse();

            try
            {
                var lAcessaDados = new AcessaDados();
                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_titulos_liquidados_busca_sel"))
                {
                    lAcessaDados.AddInParameter(cmd, "@CodigoFundo", DbType.Int32, pRequest.CodigoFundo == 0 ? null : pRequest.CodigoFundo);

                    if (!String.IsNullOrEmpty(pRequest.DownloadPendente))
                    {
                        lAcessaDados.AddInParameter(cmd, "@DownloadPendente", DbType.String, pRequest.DownloadPendente);
                    }
                    else
                    {
                        lAcessaDados.AddInParameter(cmd, "@DownloadPendente", DbType.String, null);
                    }

                    lAcessaDados.AddInParameter(cmd, "@DataDe", DbType.DateTime, pRequest.DataDe);
                    lAcessaDados.AddInParameter(cmd, "@DataAte", DbType.DateTime, pRequest.DataAte);

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    foreach (DataRow dr in table.Rows)
                    {
                        var lCarteira = new TitulosLiquidadosInfo();

                        lCarteira.CodigoFundo       = dr["CodigoFundo"].DBToInt32();
                        lCarteira.DownloadLink      = dr["DownloadLink"].DBToString();
                        lCarteira.NomeFundo         = dr["NomeFundo"].DBToString();
                        lCarteira.Status            = dr["Status"].DBToString();
                        lCarteira.Data              = dr["DownloadHora"].DBToDateTime();

                        lRetorno.ListaTitulos.Add(lCarteira);
                    }

                    lRetorno.DescricaoResposta = "Encontrou " + lRetorno.ListaTitulos.Count + " Titulos Liquidados.";

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                gLogger.Error("Erro encontrado no método Titulos Liquidados", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Lista de Fundos 
        /// </summary>
        /// <returns>REtorna um alista de fundos de objetos ListaFundosInfo</returns>
        public ExtratoCotistaResponse BuscarListaFundos()
        {
            var lRetorno = new ExtratoCotistaResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundos_busca_lst"))
                {
                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    foreach (DataRow dr in table.Rows)
                    {
                        var lCarteira = new ListaFundosInfo();

                        lCarteira.CodigoFundo = dr["CodigoFundo"].DBToInt32();
                        lCarteira.NomeFundo = dr["NomeFundo"].DBToString();

                        lRetorno.ListaFundos.Add(lCarteira);
                    }

                    lRetorno.DescricaoResposta = "Encontrou " + lRetorno.ListaFundos.Count + " Fundos.";

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                gLogger.Error("Erro encontrado no método BuscarCarteiras", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Buscar Títulos Liquidado no banco de dados ADM
        /// </summary>
        /// <param name="pRequest">Request de Títulos Liquidados</param>
        /// <returns>Retorna um objeto de Títulos Liquidados ADM</returns>
        public TitulosLiquidadosResponse BuscarTitulosLiquidadosADM(TitulosLiquidadosRequest pRequest)
        {
            var lRetorno = new TitulosLiquidadosResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_titulos_liquidados_adm_busca_sel"))
                {

                    lAcessaDados.AddInParameter(cmd, "@CodigoFundo", DbType.Int32, pRequest.CodigoFundo);
                    lAcessaDados.AddInParameter(cmd, "@DataDe", DbType.DateTime, pRequest.DataDe);
                    lAcessaDados.AddInParameter(cmd, "@DataAte", DbType.DateTime, pRequest.DataAte);

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    foreach (DataRow dr in table.Rows)
                    {
                        var lCarteira = new TitulosLiquidadosInfo();

                        lCarteira.CodigoFundo   = dr["CodigoFundo"].DBToInt32();
                        lCarteira.Data          = dr["DataReferencia"].DBToDateTime();
                        lCarteira.NomeFundo     = dr["NomeFundo"].DBToString();
                        lCarteira.Valor         = dr["Valor"].DBToDecimal();

                        lRetorno.ListaTitulos.Add(lCarteira);
                    }

                    lRetorno.DescricaoResposta = "Encontrou " + lRetorno.ListaTitulos.Count + " Titulos Liquidados.";

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                gLogger.Error("Erro encontrado no método Titulos Liquidados", ex);
            }

            return lRetorno;
        }
        
        /// <summary>
        /// Atualiza Valores de Títulos Liquidado no banco de dados ADM
        /// </summary>
        /// <param name="pRequest">Request de Títulos Liquidados</param>
        /// <returns>Retorna um objeto de Títulos Liquidados ADM</returns>
        public TitulosLiquidadosResponse AplicarValorTitulosLiquidadosADM(TitulosLiquidadosRequest pRequest)
        {
            var lRetorno = new TitulosLiquidadosResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_titulos_liquidados_adm_upd"))
                {
                    lAcessaDados.AddInParameter(cmd, "@CodigoFundo",    DbType.Int32, pRequest.CodigoFundo);
                    lAcessaDados.AddInParameter(cmd, "@ValorFundo",     DbType.Decimal, pRequest.ValorLiquidacao);
                    lAcessaDados.AddInParameter(cmd, "@DataReferencia", DbType.DateTime, pRequest.DataReferencia);

                    var table = lAcessaDados.ExecuteNonQuery(cmd);

                    lRetorno.DescricaoResposta = "Atualizou o valor de "+ pRequest.ValorLiquidacao  +" do fundo " + pRequest.CodigoFundo ;

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                gLogger.Error("Erro encontrado no método Titulos Liquidados", ex);
            }

            return lRetorno;
        }
        #endregion

        #region Carteiras
        /// <summary>
        /// Buscar carteiras no banco de dados
        /// </summary>
        /// <param name="pRequest">Request de carteiras</param>
        /// <returns>Retorna um objeto de Lista de carteiras</returns>
        public OrigemResponse BuscarOrigens()
        {
            var lRetorno = new OrigemResponse();

            try
            {

                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_origemdownloads_sel"))
                {
                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    foreach (DataRow dr in table.Rows)
                    {
                        var lOrigem = new OrigemInfo();

                        lOrigem.Codigo = dr["idOrigem"].DBToInt32();
                        lOrigem.Descricao = dr["dsOrigem"].DBToString();

                        lRetorno.ListaOrigens.Add(lOrigem);
                    }

                    lRetorno.DescricaoResposta = "Encontrou " + lRetorno.ListaOrigens.Count + " Origens.";

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                gLogger.Error("Erro encontrado no método BuscarCarteiras", ex);
            }

            return lRetorno;
        }
        #endregion
    }
}
