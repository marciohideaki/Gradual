using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;
using System.Collections.Generic;
using System;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {

        public static List<LogIntranetInfo> ConsultarLogsClientes(int id_Login)
        {
            var lAcessaDados = new ConexaoDbHelper();
            var lRetorno = new List<LogIntranetInfo>();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "log_intranet_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, id_Login);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        LogIntranetInfo lLog = new LogIntranetInfo();

                        lLog.DsCpfCnpjClienteAfetado = lDataTable.Rows[i]["ds_cpfcnpj_cliente"].ToString();
                        lLog.DsIp                    = lDataTable.Rows[i]["ds_ip"].ToString();
                        lLog.DsObservacao            = lDataTable.Rows[i]["ds_observacao"].ToString();
                        lLog.DsTela                  = lDataTable.Rows[i]["ds_tela"].ToString();
                        lLog.DtEvento                = lDataTable.Rows[i]["dt_evento"].DBToDateTime();
                        lLog.IdAcao                  = ((Contratos.Dados.Enumeradores.TipoAcaoUsuario)Enum.Parse(typeof(Contratos.Dados.Enumeradores.TipoAcaoUsuario), lDataTable.Rows[i]["id_acao"].ToString()));
                        lLog.IdLogin                 = lDataTable.Rows[i]["id_login"].DBToInt32() ;
                        //lLog.CdBovespaClienteAfetado = 0;
                        //lLog.IdClienteAfetado        = lDataTable.Rows[i][""].ToString();
                        //lLog.IdLoginClienteAfetado   = lDataTable.Rows[i][""].ToString();
                        //lLog.IdLogIntranet           = lDataTable.Rows[i][""].ToString();


                        lRetorno.Add(lLog);
                    }
                }
            }

            return lRetorno;
        }

        public static LogIntranetInfo RegistrarLog(DbTransaction pTrans, LogIntranetInfo pParametros)
        {
            var lAcessaDados = new ConexaoDbHelper();
            var lRetorno = new LogIntranetInfo();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "log_intranet_ins2"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_login"             , DbType.Int32,     pParametros.IdLogin);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_ip"                , DbType.String,    pParametros.DsIp);
                lAcessaDados.AddInParameter(lDbCommand, "@id_acao"              , DbType.Int32,     (int)pParametros.IdAcao);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_tela"              , DbType.String,    pParametros.DsTela);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_observacao"        , DbType.String,    pParametros.DsObservacao);
                lAcessaDados.AddInParameter(lDbCommand, "@dt_evento"            , DbType.DateTime,  pParametros.DtEvento);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj_cliente"   , DbType.String,    pParametros.DsCpfCnpjClienteAfetado);
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente"           , DbType.Int32,     pParametros.IdClienteAfetado);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_bovespa"           , DbType.Int32,     pParametros.CdBovespaClienteAfetado);

                lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);
            }

            lRetorno = pParametros;

            return lRetorno;
        }

        public static SalvarEntidadeResponse<LogIntranetInfo> RegistrarLog(SalvarObjetoRequest<LogIntranetInfo> pParametros)
        {
            var lAcessaDados = new ConexaoDbHelper();
            var lRetorno = new SalvarEntidadeResponse<LogIntranetInfo>();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "log_intranet_ins2"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLogin);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_ip", DbType.String, pParametros.Objeto.DsIp);
                lAcessaDados.AddInParameter(lDbCommand, "@id_acao", DbType.Int32, (int)pParametros.Objeto.IdAcao);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_tela", DbType.String, pParametros.Objeto.DsTela);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_observacao", DbType.String, pParametros.Objeto.DsObservacao);
                lAcessaDados.AddInParameter(lDbCommand, "@dt_evento", DbType.DateTime, pParametros.Objeto.DtEvento);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj_cliente", DbType.String, pParametros.Objeto.DsCpfCnpjClienteAfetado);
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdClienteAfetado);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_bovespa", DbType.Int32, pParametros.Objeto.CdBovespaClienteAfetado);

                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            return lRetorno;
        }
    }
}
