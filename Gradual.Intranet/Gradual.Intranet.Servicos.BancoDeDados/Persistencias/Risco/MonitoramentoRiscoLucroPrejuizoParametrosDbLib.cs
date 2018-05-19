using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Risco
{
    public static class MonitoramentoRiscoLucroPrejuizoParametrosDbLib
    {

        public static ReceberObjetoResponse<MonitoramentoRiscoLucroPrejuizoParametrosInfo> ReceberMonitoramentoRiscoLucroPrejuizoJanelas(ReceberEntidadeRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo> pParametros)
        {
            var lRetorno = new ReceberObjetoResponse<MonitoramentoRiscoLucroPrejuizoParametrosInfo>();
            
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRisco;

            lRetorno.Objeto = new MonitoramentoRiscoLucroPrejuizoParametrosInfo();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_monitores_risco_janela_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_janela", DbType.Int32, pParametros.Objeto.IdJanela);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    lRetorno.Objeto = new MonitoramentoRiscoLucroPrejuizoParametrosInfo()
                    {
                        IdUsuario  = lDataTable.Rows[0]["id_usuario"].DBToInt32(),
                        Colunas    = lDataTable.Rows[0]["ds_colunas"].ToString(),
                        Consulta   = lDataTable.Rows[0]["ds_consulta"].ToString(),
                        NomeJanela = lDataTable.Rows[0]["ds_nomejanela"].ToString(),
                        IdJanela   = lDataTable.Rows[0]["id_janela"].DBToInt32(),
                    };
                }
            }

            return lRetorno;
        }

        public static ConsultarObjetosResponse<MonitoramentoRiscoLucroPrejuizoParametrosInfo> ConsultarMonitoramentoRiscoLucroPrejuizoJanelas(ConsultarEntidadeRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<MonitoramentoRiscoLucroPrejuizoParametrosInfo>();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRisco;
            lRetorno.Resultado = new List<MonitoramentoRiscoLucroPrejuizoParametrosInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_monitores_risco_janela_lst"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_usuario", DbType.Int32, pParametros.Objeto.IdUsuario);
                

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Resultado.Add(new MonitoramentoRiscoLucroPrejuizoParametrosInfo()
                        {
                            Colunas    = lLinha["ds_colunas"].ToString(),
                            Consulta   = lLinha["ds_consulta"].ToString(),
                            NomeJanela = lLinha["ds_nomejanela"].ToString(),
                            IdJanela   = lLinha["id_janela"].DBToInt32(),
                        });
            }

            return lRetorno;
        }

        private static SalvarEntidadeResponse<MonitoramentoRiscoLucroPrejuizoParametrosInfo> Salvar(SalvarObjetoRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo> pParametros)
        {
            var lRetorno = new SalvarEntidadeResponse<MonitoramentoRiscoLucroPrejuizoParametrosInfo>();

            lRetorno.Objeto = new MonitoramentoRiscoLucroPrejuizoParametrosInfo();

            lRetorno.Objeto = pParametros.Objeto;

            var lAcessaDados = new AcessaDados();
            
            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRisco;


            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_monitores_risco_janela_ins"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_usuario", DbType.Int32, pParametros.Objeto.IdUsuario);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_nomejanela", DbType.AnsiString, pParametros.Objeto.NomeJanela);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_colunas", DbType.AnsiString, pParametros.Objeto.Colunas);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_consulta", DbType.AnsiString, pParametros.Objeto.Consulta);

                lAcessaDados.AddOutParameter(lDbCommand, "@id_janela", DbType.Int32, 8);

                lAcessaDados.ExecuteNonQuery(lDbCommand);

                lRetorno.Objeto .IdJanela = Convert.ToInt32(lDbCommand.Parameters["@id_janela"].Value);

            }

            return lRetorno;
        }

        private static SalvarEntidadeResponse<MonitoramentoRiscoLucroPrejuizoParametrosInfo> Atualizar(SalvarObjetoRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo> pParametros)
        {
            var lRetorno = new SalvarEntidadeResponse<MonitoramentoRiscoLucroPrejuizoParametrosInfo>();
            
            var lAcessaDados = new AcessaDados();
            
            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRisco;

            lRetorno.Objeto = new MonitoramentoRiscoLucroPrejuizoParametrosInfo();

            lRetorno.Objeto = pParametros.Objeto;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_monitores_risco_janela_upd"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_usuario", DbType.Int32, pParametros.Objeto.IdUsuario);
                lAcessaDados.AddInParameter(lDbCommand, "@id_janela", DbType.Int32, pParametros.Objeto.IdJanela);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_nomejanela", DbType.AnsiString, pParametros.Objeto.NomeJanela);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_colunas", DbType.AnsiString, pParametros.Objeto.Colunas);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_consulta", DbType.AnsiString, pParametros.Objeto.Consulta);

                lAcessaDados.ExecuteNonQuery(lDbCommand);

                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
            }

            return lRetorno;
        }

        public static SalvarEntidadeResponse<MonitoramentoRiscoLucroPrejuizoParametrosInfo>   SalvarMonitoramentoRiscoLucroPrejuizoJanelas(SalvarObjetoRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo> pParametros)
        {
            if (pParametros.Objeto.IdJanela > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        public static RemoverObjetoResponse<MonitoramentoRiscoLucroPrejuizoParametrosInfo> RemoverMonitoramentoRiscoLucroPrejuizoJanelas(RemoverEntidadeRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo> pParametros)
        {
            var lRetorno = new RemoverObjetoResponse<MonitoramentoRiscoLucroPrejuizoParametrosInfo>();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRisco;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_monitores_risco_janela_del"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_janela", DbType.Int32, pParametros.Objeto.IdJanela);

                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            return lRetorno;
        }

        
        
    }
}
