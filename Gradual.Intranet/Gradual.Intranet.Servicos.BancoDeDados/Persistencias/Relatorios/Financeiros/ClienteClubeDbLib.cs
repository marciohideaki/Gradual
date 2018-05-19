using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteClubesDbLib
    {
        #region | Métodos CRUD

        public ConsultarObjetosResponse<ClienteClubesInfo> ConsultarClienteClubes(ConsultarEntidadeRequest<ClienteClubesInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<ClienteClubesInfo>();

            var lAcessaDados = new ConexaoDbHelper();

            DateTime UltimoDiaUtil = SelecionaUltimoDiaUtil(); //seleciona o ultimo dia ultil. 

            lAcessaDados.ConnectionStringName = "Clubes";

            List<string> lNomeClubeExiste = new List<string>();

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_MC_CLUBES_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                lAcessaDados.AddInParameter(lDbCommand, "@DT_POSICAO", DbType.DateTime, UltimoDiaUtil);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                lNomeClubeExiste.Clear();

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(CriarRegistroClienteClubesInfo(lDataTable.Rows[i]));

            }

            return lRetorno;
        }

        #endregion

        #region | Métodos de apoio

        private ClienteClubesInfo CriarRegistroClienteClubesInfo(DataRow linha)
        {
            return new ClienteClubesInfo() 
            {
                IdCliente       = linha["cd_cliente"].DBToInt32(),
                Cota            = linha["vl_cota"].DBToDecimal(),
                DataAtualizacao = linha["dt_atualizacao"].DBToDateTime(),
                IOF             = linha["vl_iof"].DBToDecimal(),
                IR              = linha["vl_ir"].DBToDecimal(),
                NomeClube       = linha["ds_nome_clube"].DBToString(),
                Quantidade      = linha["vl_quantidade"].DBToDecimal(),
                ValorBruto      = linha["vl_bruto"].DBToDecimal(),
                ValorLiquido    = linha["vl_liquido"].DBToDecimal(),
            };
        }

        public static DateTime SelecionaUltimoDiaUtil()
        {
            ConexaoDbHelper _AcessaDados = new ConexaoDbHelper();
            DateTime dataRetorno;
            _AcessaDados.ConnectionStringName = "SINACOR";
            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RETORNA_ULTIMO_DIA_UTIL"))
            {
                DataTable tabela = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                dataRetorno = Convert.ToDateTime(tabela.Rows[0]["dataUtil"]);

            }
            return dataRetorno;
        }

        #endregion
    }
}
