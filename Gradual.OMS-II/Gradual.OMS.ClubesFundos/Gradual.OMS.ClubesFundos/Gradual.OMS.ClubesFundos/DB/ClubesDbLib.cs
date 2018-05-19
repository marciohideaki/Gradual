#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gradual.OMS.ClubesFundos.Lib;
using Gradual.OMS.ClubesFundos.Lib.Dados;
using Gradual.OMS.ClubesFundos.Lib.Util;
using Gradual.Generico.Dados;
using System.Data.Common;
#endregion

namespace Gradual.OMS.ClubesFundos
{
    public class ClubesDbLib
    {
        #region Propriedades
        private const string _ConnectionStringName = "OMS";
        private const string _ConnectionStringClubes = "Clubes";
        private const string _gNomeConexaoSinacorTrade = "TRADE";
        #endregion

        #region ConsultarClientesClubes
        public List<ClubesInfo> ConsultarClientesClubes(ListarClubesRequest pRequest)
        {
            List<ClubesInfo> lRetorno = new List<ClubesInfo>();
            
            AcessaDados acesso = new AcessaDados();

            acesso.ConnectionStringName = _ConnectionStringClubes;

            DateTime lUltimoDiaUtil = SelecionaUltimoDiaUtil(); //seleciona o ultimo dia ultil. 

            List<string> lNomeClubeExiste = new List<string>();

            using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_MC_CLUBES_SEL"))
            {
                acesso.AddInParameter(cmd, "@id_cliente", DbType.Int32, pRequest.IdCliente);

                acesso.AddInParameter(cmd, "@dt_posicao", DbType.DateTime, lUltimoDiaUtil);

                DataTable table = acesso.ExecuteDbDataTable(cmd);

                lNomeClubeExiste.Clear();

                foreach (DataRow dr in table.Rows)
                {
                    if (!lNomeClubeExiste.Contains(dr["ds_nome_clube"].DBToString()))
                    {
                        lNomeClubeExiste.Add(dr["ds_nome_clube"].DBToString());

                        lRetorno.Add(CriarRegistroClienteClubesInfo(dr));
                    }
                }
            }

            return lRetorno;
        }

        public static DateTime SelecionaUltimoDiaUtil()
        {
            AcessaDados _AcessaDados = new AcessaDados();
            DateTime dataRetorno;
            _AcessaDados.ConnectionStringName = _gNomeConexaoSinacorTrade;
            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RETORNA_ULTIMO_DIA_UTIL"))
            {
                DataTable tabela = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                dataRetorno = Convert.ToDateTime(tabela.Rows[0]["dataUtil"]);

            }
            return dataRetorno;
        }
        #endregion

        #region Métodos de Aopio
        private ClubesInfo CriarRegistroClienteClubesInfo(DataRow linha)
        { 
            return new ClubesInfo()
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
        #endregion
    }
}
