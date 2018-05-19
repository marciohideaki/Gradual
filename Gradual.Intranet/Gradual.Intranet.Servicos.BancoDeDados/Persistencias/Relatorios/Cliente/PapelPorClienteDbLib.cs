using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using System.Data.Common;
using System.Data;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios
{
    public class PapelPorClienteDbLib
    {
        public const string gNomeConexaoSinacor = "SinacorExportacao";
        public const string gNomeConexaoCadastro = "Cadastro";

        public PapelPorClienteInfo ConsultarPapeisPorCliente(PapelPorClienteInfo pParametros)
        {
            PapelPorClienteInfo lRetorno = new PapelPorClienteInfo();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

            List<int> lAssessoresVinculados = new List<int>();

            DataTable lDataTable = new DataTable();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_PAPEL_CLIENTE_REL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pDataDe", DbType.DateTime, pParametros.DataInicial);
                lAcessaDados.AddInParameter(lDbCommand, "pDataAte", DbType.DateTime, pParametros.DataFinal);

                if (pParametros.CodigoCliente.HasValue)
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.CodigoCliente.Value);

                    lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);
                }

                if (pParametros.CodigoAssessor.HasValue)
                {
                    lAssessoresVinculados = ReceberListaAssessoresVinculados(pParametros.CodigoAssessor.Value, pParametros.IdUsuarioLogado);
                }

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow linha = lDataTable.Rows[i];

                        if (pParametros.CodigoAssessor.HasValue)
                        {
                            if (lAssessoresVinculados.Contains(linha["cd_assessor"].DBToInt32()))
                            {
                                lRetorno.Resultado.Add(CriarRegistroPapelPorCliente(linha));
                            }
                            else
                            {
                                continue;
                            }

                        }
                        else
                        {
                            lRetorno.Resultado.Add(CriarRegistroPapelPorCliente(linha));
                            
                        }
                    }
                }
            }

            return lRetorno;
        }

        public static List<int> ReceberListaAssessoresVinculados(int CodigoAssessor, int cd_login)
        {
            var lRetorno = new List<int>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "ListarAssessoresVinculados_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int32, CodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_login", DbType.Int32, cd_login);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0) foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Add(lLinha["cd_assessor"].DBToInt32());
            }

            return lRetorno;
        }

        private PapelPorClienteInfo CriarRegistroPapelPorCliente(DataRow linha)
        {
            PapelPorClienteInfo lRetorno = new PapelPorClienteInfo();

            lRetorno.CodigoAssessor = linha["cd_assessor"].DBToInt32();
            lRetorno.CodigoCliente  = linha["cd_cliente"].DBToInt32();
            lRetorno.DataPregao     = linha["dt_pregao"].DBToDateTime();
            lRetorno.Papel          = linha["Papel"] .DBToString();
            lRetorno.QtdeCompras    = linha["QtdeCompras"].DBToInt32();
            lRetorno.QtdeVendas     = linha["QtdeVendas"].DBToInt32();
            lRetorno.QtdeLiquida    = linha["QtdeLiquida"].DBToInt32();
            lRetorno.Preco          = linha["PrecoMedio"].DBToDecimal();
            lRetorno.VolCompras     = linha["VolCompras"].DBToDecimal();
            lRetorno.VolVendas      = linha["VolVendas"].DBToDecimal();
            lRetorno.VolLiquido     = linha["VolLiquido"].DBToDecimal();
            lRetorno.VlNegocio      = linha["vl_negocio"].DBToDecimal();
            lRetorno.MostraTotal    = "nao";
            lRetorno.Ordem = 0;

            return lRetorno;
        }
    }
}
