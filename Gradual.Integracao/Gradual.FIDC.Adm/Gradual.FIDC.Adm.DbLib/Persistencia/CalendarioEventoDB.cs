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
    public class CalendarioEventoDB
    {
        #region Propriedades
        private static readonly ILog GLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// Método que seleciona eventos no banco de dados de acordo com os parâmetros informados
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public CalendarioEventoResponse Buscar(CalendarioEventoRequest pRequest)
        {
            CalendarioEventoResponse lRetorno = new CalendarioEventoResponse();

            try
            {
                var lAcessaDados = new AcessaDados { ConnectionStringName = "GradualFundosAdm" };

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_eventos_calendario_sel"))
                {
                    #region Adicionar Parâmetros
                    
                    if (pRequest.IdCalendarioEvento> 0)
                        lAcessaDados.AddInParameter(cmd, "@idCalendarioEvento", DbType.Int32, pRequest.IdCalendarioEvento);

                    if (pRequest.NomeFundo != null && pRequest.NomeFundo.Length > 1)
                        lAcessaDados.AddInParameter(cmd, "@nomeFundo", DbType.String, pRequest.NomeFundo);

                    if (pRequest.IdFundoCadastro > 0)
                        lAcessaDados.AddInParameter(cmd, "@idFundoCadastro", DbType.Int32, pRequest.IdFundoCadastro);

                    if (pRequest.DtEvento != null && pRequest.DtEvento != DateTime.MinValue)
                        lAcessaDados.AddInParameter(cmd, "@dtEvento", DbType.DateTime, pRequest.DtEvento);

                    if (pRequest.DtEventoEnd != null && pRequest.DtEventoEnd != DateTime.MinValue)
                        lAcessaDados.AddInParameter(cmd, "@dtEventoEnd", DbType.DateTime, pRequest.DtEventoEnd.AddDays(1));
                    else if (pRequest.DtEvento != DateTime.MinValue)
                        lAcessaDados.AddInParameter(cmd, "@dtEventoEnd", DbType.DateTime, Convert.ToDateTime(pRequest.DtEvento.AddDays(1).ToString("yyyy-MM-ddT00:00:00.000")));

                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaEventos = new List<CalendarioEventoInfo>();

                    #region Preenchimento Retorno
                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista = new CalendarioEventoInfo
                        {
                            IdCalendarioEvento = dr["idCalendarioEvento"].DBToInt32(),
                            IdFundoCadastro = dr["idFundoCadastro"].DBToInt32(),
                            NomeFundo = dr["nomeFundo"].DBToString(),
                            DtEvento = dr["dtEvento"].DBToDateTime(),
                            DescEvento = dr["descEvento"].DBToString(),
                            EmailEvento = dr["emailEvento"].DBToString(),
                            EnviarNotificacaoDia = dr["enviarNotificacaoDia"].DBToBoolean(),
                            MostrarHome = dr["mostrarHome"].DBToBoolean(),
                        };

                        lRetorno.ListaEventos.Add(itemLista);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                GLogger.Error("Erro encontrado na classe CalendarioEventoDB.Buscar", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que insere um novo evento de calendario no banco de dados
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CalendarioEventoResponse Inserir(CalendarioEventoRequest request)
        {
            var lRetorno = new CalendarioEventoResponse();

            try
            {
                var lAcessaDados = new AcessaDados { ConnectionStringName = "GradualFundosAdm" };

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_eventos_calendario_ins"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@idFundoCadastro", DbType.Int32, request.IdFundoCadastro);
                    lAcessaDados.AddInParameter(cmd, "@dtEvento", DbType.DateTime, request.DtEvento);
                    lAcessaDados.AddInParameter(cmd, "@descEvento", DbType.String, request.DescEvento);
                    lAcessaDados.AddInParameter(cmd, "@emailEvento", DbType.String, request.EmailEvento);
                    lAcessaDados.AddInParameter(cmd, "@enviarNotificacaoDia", DbType.Boolean, request.EnviarNotificacaoDia);
                    lAcessaDados.AddInParameter(cmd, "@mostrarHome", DbType.Boolean, request.MostrarHome);
                    #endregion

                    request.IdCalendarioEvento = Convert.ToInt32(lAcessaDados.ExecuteScalar(cmd));

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método CalendarioEventoDB.Inserir", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que remove evento de calendario do banco de dados
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CalendarioEventoResponse Remover(CalendarioEventoRequest request)
        {
            var lRetorno = new CalendarioEventoResponse();

            try
            {
                var lAcessaDados = new AcessaDados { ConnectionStringName = "GradualFundosAdm" };

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_eventos_calendario_del"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@idCalendarioEvento", DbType.Int32, request.IdCalendarioEvento);
                    #endregion

                    lAcessaDados.ExecuteNonQuery(cmd);

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método CalendarioEventoDB.Inserir", ex);
            }

            return lRetorno;
        }
    }
}
