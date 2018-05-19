using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Generico.Dados;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Risco
{
    public partial class RiscoDbLib
    {

        public static string gNomeConexaoRisco_GradualOMS
        {
            get { return "RISCO_GRADUALOMS"; }
        }
        #region | Métodos CRUD

        public ConsultarObjetosResponse<RiscoLimiteAlocadoInfo> ConsultarRiscoLimiteAlocadoPorCliente(ConsultarEntidadeRequest<RiscoLimiteAlocadoInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoLimiteAlocadoInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoRisco_GradualOMS;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_limite_alocado_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.ConsultaIdCliente);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                lRetorno.Resultado = new List<RiscoLimiteAlocadoInfo>();

                if (null != lDataTable) for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoLimiteAlocadoInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        public ConsultarObjetosResponse<RiscoLimiteAlocadoInfo> ConsultarRiscoLimiteAlocadoPorClienteNovoOMS(ConsultarEntidadeRequest<RiscoLimiteAlocadoInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoLimiteAlocadoInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoRisco_GradualOMS;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_limite_alocado_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.ConsultaIdCliente);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                lRetorno.Resultado = new List<RiscoLimiteAlocadoInfo>();

                if (null != lDataTable) for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoLimiteAlocadoInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        public ConsultarObjetosResponse<RiscoLimiteAlocadoInfo> ConsultarRiscoLimiteAlocadoPorClienteSpider(ConsultarEntidadeRequest<RiscoLimiteAlocadoInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoLimiteAlocadoInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";// gNomeConexaoRisco_GradualOMS;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_limite_alocado_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.ConsultaIdCliente);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                lRetorno.Resultado = new List<RiscoLimiteAlocadoInfo>();

                if (null != lDataTable) for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoLimiteAlocadoInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        #endregion

        #region | Métodos de apoio

        private RiscoLimiteAlocadoInfo CarregarEntidadeRiscoLimiteAlocadoInfo(DataRow pLinha)
        {
            return new RiscoLimiteAlocadoInfo()
            {
                 DsParametro = pLinha["ds_parametro"].DBToString(),
                 IdParametro = pLinha["id_parametro"].DBToInt32(),
                 VlAlocado = pLinha["vl_alocado"].DBToDecimal(),
                 VlDisponivel = pLinha["vl_disponivel"].DBToDecimal(),
                 VlParametro = pLinha["vl_parametro"].DBToDecimal(),
            };
        }

        #endregion
    }
}
