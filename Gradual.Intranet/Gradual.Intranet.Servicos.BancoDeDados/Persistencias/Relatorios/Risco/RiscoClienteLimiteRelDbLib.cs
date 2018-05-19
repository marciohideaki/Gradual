using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Risco
{
    public partial class RiscoRelDbLib
    {
        #region | Métodos CRUD

        public ConsultarObjetosResponse<RiscoClienteLimiteRelInfo> ConsultarRiscoClienteLimite(ConsultarEntidadeRequest<RiscoClienteLimiteRelInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoClienteLimiteRelInfo>();
            var lAcessaDados = new ConexaoDbHelper() { ConnectionStringName = ClienteDbLib.gNomeConexaoRisco };

            lRetorno.Resultado = new List<RiscoClienteLimiteRelInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_relatorio_cliente_limite_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_parametro", DbType.Int32, pParametros.Objeto.ConsultaIdParametro);

                if (!string.IsNullOrEmpty(pParametros.Objeto.ConsultaClienteParametro))
                    switch (pParametros.Objeto.ConsultaClienteTipo)
                    {
                        case Gradual.Intranet.Contratos.Dados.Enumeradores.OpcoesBuscarPor.CodBovespa:
                            if (0.Equals(pParametros.Objeto.ConsultaClienteParametro.DBToInt32())) return lRetorno;
                            lAcessaDados.AddInParameter(lDbCommand, "@cd_bovespa", DbType.Int32, pParametros.Objeto.ConsultaClienteParametro.DBToInt32());
                            break;
                        case Gradual.Intranet.Contratos.Dados.Enumeradores.OpcoesBuscarPor.CpfCnpj:
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.ConsultaClienteParametro.Trim().Replace(".", "").Replace("-", "").Replace("/", ""));
                            break;
                        case Gradual.Intranet.Contratos.Dados.Enumeradores.OpcoesBuscarPor.NomeCliente:
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.ConsultaClienteParametro.Trim());
                            break;
                    }

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoClienteLimiteRelInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        public ConsultarObjetosResponse<RiscoClienteLimiteMovimentoRelInfo> ConsultarRiscoClienteLimiteMovimentacao(ConsultarEntidadeRequest<RiscoClienteLimiteMovimentoRelInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoClienteLimiteMovimentoRelInfo>();
            var lAcessaDados = new ConexaoDbHelper() { ConnectionStringName = ClienteDbLib.gNomeConexaoRisco };

            lRetorno.Resultado = new List<RiscoClienteLimiteMovimentoRelInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_relatorio_cliente_limite_movimento_sp"))
            {
                if (!string.IsNullOrEmpty(pParametros.Objeto.ConsultaClienteParametro))
                    switch (pParametros.Objeto.ConsultaClienteTipo)
                    {
                        case Gradual.Intranet.Contratos.Dados.Enumeradores.OpcoesBuscarPor.CodBovespa:
                            if (0.Equals(pParametros.Objeto.ConsultaClienteParametro.DBToInt32())) return lRetorno;
                            lAcessaDados.AddInParameter(lDbCommand, "@cd_bovespa", DbType.Int32, pParametros.Objeto.ConsultaClienteParametro.DBToInt32());
                            break;
                        case Gradual.Intranet.Contratos.Dados.Enumeradores.OpcoesBuscarPor.CpfCnpj:
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.ConsultaClienteParametro.Trim().Replace(".", "").Replace("-", "").Replace("/", ""));
                            break;
                        case Gradual.Intranet.Contratos.Dados.Enumeradores.OpcoesBuscarPor.NomeCliente:
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.ConsultaClienteParametro.Trim());
                            break;
                    }

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoClienteLimiteMovimentoRelInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        public SalvarObjetoResponse<RiscoClienteLimiteRelInfo> AtualizarDataValidadeParametro(SalvarObjetoRequest<RiscoClienteLimiteRelInfo> pParametros)
        {
            var lAcessaDados = new ConexaoDbHelper() { ConnectionStringName = ClienteDbLib.gNomeConexaoRisco };

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_tb_cliente_parametro_dt_validade_upd"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_parametro", DbType.Int32, pParametros.Objeto.IdClienteParametro);
                lAcessaDados.AddInParameter(lDbCommand, "@dt_validade", DbType.DateTime, pParametros.Objeto.DtValidade);

                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            return new SalvarObjetoResponse<RiscoClienteLimiteRelInfo>();
        }

        #endregion

        #region | Métodos de apoio

        private RiscoClienteLimiteRelInfo CarregarEntidadeRiscoClienteLimiteRelInfo(DataRow pLinha)
        {
            return new RiscoClienteLimiteRelInfo()
            {
                DsNome = pLinha["ds_nome"].DBToString(),
                DsCpfCnpj = pLinha["ds_cpfcnpj"].DBToString(),
                DsParametro = pLinha["ds_parametro"].DBToString(),
                VlLimite = pLinha["vl_parametro"].DBToDecimal(),
                VlAlocado = pLinha["vl_alocado"].DBToDecimal(),
                IdClienteParametro = pLinha["id_cliente_parametro"].DBToInt32(),
                DtValidade = pLinha["dt_validade"].DBToDateTime(),
            };
        }

        private RiscoClienteLimiteMovimentoRelInfo CarregarEntidadeRiscoClienteLimiteMovimentoRelInfo(DataRow pLinha)
        {
            return new RiscoClienteLimiteMovimentoRelInfo()
            {
                DataMovimento = pLinha["dt_movimento"].DBToDateTime(),
                Historico = pLinha["ds_historico"].DBToString(),
                IdClienteParametro = pLinha["id_cliente_parametro"].DBToInt32(),
                IdClienteParametroValor = pLinha["id_cliente_parametro_valor"].DBToInt32(),
                ValorAlocado = pLinha["vl_alocado"].DBToDecimal(),
                ValorDisponivel = pLinha["vl_disponivel"].DBToDecimal(),
                ValorMovimento = pLinha["vl_movimento"].DBToDecimal(),
            };
        }

        #endregion
    }
}
