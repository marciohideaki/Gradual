using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using System.Data.Common;
using System.Data;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios
{
    public class ProventosClienteDbLib
    {
        public const string gNomeConexaoSinacor = "SINACOR";
        public const string gNomeConexaoCadastro = "Cadastro";

        public ProventosClienteInfo ConsultarProventosCliente(ProventosClienteInfo pParametros)
        {
            ProventosClienteInfo lRetorno = new ProventosClienteInfo();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

            List<int> lAssessoresVinculados = new List<int>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_PROVENTOS_LST"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pDataDe", DbType.DateTime, pParametros.DataDe);
                lAcessaDados.AddInParameter(lDbCommand, "pDataAte", DbType.DateTime, pParametros.DataAte);
                
                if (pParametros.CodigoCliente.HasValue)
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.CodigoCliente.Value);
                }

                DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);
                
                if (pParametros.CodigoAssessor.HasValue)
                {
                    lAssessoresVinculados = ReceberListaAssessoresVinculados(pParametros.CodigoAssessor.Value, pParametros.IdUsuarioLogado);
                }

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow linha = lDataTable.Rows[i];

                        if (pParametros.CodigoAssessor.HasValue )
                        {
                            if (lAssessoresVinculados.Contains(linha["cd_assessor"].DBToInt32()))
                            {
                                lRetorno.Resultado.Add(CriarRegistroProventosCliente(linha));
                            }
                            else
                            {
                                continue;
                            }

                        }
                        else
                        {
                            lRetorno.Resultado.Add(CriarRegistroProventosCliente(linha));
                        }
                    }
                }
            }

            return lRetorno;
        }

        private ProventosClienteInfo CriarRegistroProventosCliente(DataRow linha)
        {
            ProventosClienteInfo lRetorno = new ProventosClienteInfo();

            lRetorno.CodigoAssessor = linha["cd_assessor"].DBToInt32();
            lRetorno.CodigoCliente  = linha["cd_cliente"].DBToInt32();
            lRetorno.NomeAssessor   = linha["nm_assessor"].DBToString();
            lRetorno.NomeCliente    = linha["nm_cliente"].DBToString();
            lRetorno.DataPagamento  = linha["dt_pagamento"].DBToDateTime();
            lRetorno.Carteira       = linha["cd_carteira"].DBToInt32();
            lRetorno.Ativo          = linha["ds_ativo"].DBToString();
            lRetorno.Distribuicao   = linha["cd_distribuicao"].DBToInt32();
            lRetorno.GrupoProvento  = linha["ds_grupoprovento"].DBToString();
            lRetorno.Isin           = linha["ds_isin"].DBToString();
            lRetorno.PercentualIR   = linha["vl_percentual"].DBToDecimal();
            lRetorno.Quantidade     = linha["vl_quantidade"].DBToInt32();
            lRetorno.TipoProvento   = linha["tp_provento"].DBToString();
            lRetorno.Valor          = linha["vl_valor"].DBToDecimal();
            lRetorno.ValorIR        = linha["vl_IR"].DBToDecimal();
            lRetorno.ValorLiquido   = linha["vl_Liquido"].DBToDecimal();
            lRetorno.Emitente       = linha["ds_emitente"].DBToString();
            
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
    }
}
