using System.Collections.Generic;
using System.Data;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Risco
{
    public partial class RiscoRelDbLib
    {
        #region | Métodos CRUD

        public ConsultarObjetosResponse<RiscoClienteParametroRelInfo> ConsultarRiscoClienteParametros(ConsultarEntidadeRequest<RiscoClienteParametroRelInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoClienteParametroRelInfo>();
            var lAcessaDados = new ConexaoDbHelper() { ConnectionStringName = ClienteDbLib.gNomeConexaoRisco };

            lRetorno.Resultado = new List<RiscoClienteParametroRelInfo>();

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_relatorio_cliente_parametro_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_bolsa", DbType.Int32, pParametros.Objeto.ConsultaIdBolsa);
                lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametros.Objeto.ConsultaIdGrupo);
                lAcessaDados.AddInParameter(lDbCommand, "@id_parametro", DbType.Int32, pParametros.Objeto.ConsultaIdParametro);

                if(null != pParametros.Objeto.ConsultaEstado)
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_valido", DbType.String, pParametros.Objeto.ConsultaEstado.DBToString());

                if (!string.IsNullOrEmpty(pParametros.Objeto.ConsultaClienteParametro))
                    switch (pParametros.Objeto.ConsultaClienteTipo)
                    {
                        case Gradual.Intranet.Contratos.Dados.Enumeradores.OpcoesBuscarPor.CodBovespa:
                            if (0.Equals(pParametros.Objeto.ConsultaClienteParametro.DBToInt32())) return lRetorno;
                            lAcessaDados.AddInParameter(lDbCommand, "@cd_bovespa", DbType.Int32, pParametros.Objeto.ConsultaClienteParametro.DBToInt32());
                            break;
                        case Gradual.Intranet.Contratos.Dados.Enumeradores.OpcoesBuscarPor.CpfCnpj:
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.ConsultaClienteParametro.Trim().Replace(".", "").Replace("-", ""));
                            break;
                        case Gradual.Intranet.Contratos.Dados.Enumeradores.OpcoesBuscarPor.NomeCliente:
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.ConsultaClienteParametro.Trim());
                            break;
                    }

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoClienteParametroRelInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        #endregion

        #region | Métodos auxiliares

        private RiscoClienteParametroRelInfo CarregarEntidadeRiscoClienteParametroRelInfo(DataRow pLinha)
        {
            return new RiscoClienteParametroRelInfo()
            {
                CpfCnpj = pLinha["ds_cpfcnpj"].DBToString(),
                DsBolsa = pLinha["ds_bolsa"].DBToString(),
                DsGrupo = pLinha["ds_grupo"].DBToString(),
                DsParametro = pLinha["ds_parametro"].DBToString(),
                NomeCliente = pLinha["ds_nome"].DBToString(),
            };
        }

        #endregion
    }
}
