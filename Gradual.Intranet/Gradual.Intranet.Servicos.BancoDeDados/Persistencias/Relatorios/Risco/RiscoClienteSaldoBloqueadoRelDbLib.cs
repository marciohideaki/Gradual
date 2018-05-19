using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Risco
{
    partial class RiscoRelDbLib
    {
        #region | Métodos CRUD

        public ConsultarObjetosResponse<RiscoClienteSaldoBloqueadoRelInfo> ConsultarRiscoClienteSaldoBloqueado(ConsultarEntidadeRequest<RiscoClienteSaldoBloqueadoRelInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoClienteSaldoBloqueadoRelInfo>();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRisco;

            lRetorno.Resultado = new List<RiscoClienteSaldoBloqueadoRelInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_relatorio_cliente_saldo_bloqueado_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.ConsultaIdCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.ConsultaDsNome);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.ConsultaDsCpfCnpj);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_ativo", DbType.String, pParametros.Objeto.ConsultaDsAtivo);
                lAcessaDados.AddInParameter(lDbCommand, "@tp_ordem", DbType.String, pParametros.Objeto.ConsultaTpOrdem);
                lAcessaDados.AddInParameter(lDbCommand, "@dt_transacao_de", DbType.DateTime, pParametros.Objeto.ConsultaDtTransacaoDe);
                lAcessaDados.AddInParameter(lDbCommand, "@dt_transacao_ate", DbType.DateTime, pParametros.Objeto.ConsultaDtTransacaoAte);
                lAcessaDados.AddInParameter(lDbCommand, "@id_canal_bovespa", DbType.Int32, pParametros.Objeto.ConsultaIdCanalBovespa);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoClienteSaldoBloqueado(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        #endregion

        #region | Métodos de apoio

        private RiscoClienteSaldoBloqueadoRelInfo CarregarEntidadeRiscoClienteSaldoBloqueado(DataRow pLinha)
        {
            return new RiscoClienteSaldoBloqueadoRelInfo() 
            {
                CdBovespa = pLinha["cd_codigo"].DBToInt32(),
                DsAtivo = pLinha["symbol"].DBToString(),
                DsCpfCnpj = pLinha["ds_cpfcnpj"].DBToString(),
                DsNome = pLinha["ds_nome"].DBToString(),
                DsStatusOrdem = pLinha["OrderStatusDescription"].DBToString(),
                DtTransacao = pLinha["transactTime"].DBToDateTime(),
                QtOrdem = pLinha["OrderQty"].DBToInt32(),
                TpOrdem = pLinha["side"].DBToString(),
                VlBloqueioOperacaoTotal = pLinha["BloqueioOperacaoTotal"].DBToDecimal(),
                VlPreco = pLinha["Price"].DBToDecimal(),
            };
        }

        #endregion
    }
}
