using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Contratos.Dados.Compliance;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using System.Data;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {
        public static ReceberObjetoResponse<ClienteCustodiaFinanceiroInfo> ReceberClienteCustodiaFinanceiro(ReceberEntidadeRequest<ClienteCustodiaFinanceiroInfo> pParametros)
        {
            var lRetorno = new ReceberObjetoResponse<ClienteCustodiaFinanceiroInfo>();

            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = "SinacorExportacao";

            lRetorno.Objeto = new ClienteCustodiaFinanceiroInfo();

            lRetorno.Objeto.ListaExtrato = new List<ClienteExtratoInfo>();
            
            lRetorno.Objeto.ListaPosicao = new List<ClientePosicaoInfo>();

            string lNomeCliente   = string.Empty;
            string lCodigoCliente = string.Empty;

            using (var lDbCommandCliente = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_CAB_BOV_BR"))
            {
                lAcessaDados.AddInParameter(lDbCommandCliente, "pClienteCodigo", DbType.Int32, pParametros.Objeto.CodigoCliente);

                lAcessaDados.AddInParameter(lDbCommandCliente, "pData", DbType.DateTime, pParametros.Objeto.Ate);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommandCliente);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.Objeto.CodigoCliente = lDataTable.Rows[i]["CD_CLIENTE"].DBToInt32();
                        lRetorno.Objeto.NomeCliente = lDataTable.Rows[i]["NM_CLIENTE"].ToString();
                    }
                }
            }

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CUS_FINAN_VIS_LST2"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "id_cliente", DbType.Int32, pParametros.Objeto.CodigoCliente);

                lAcessaDados.AddInParameter(lDbCommand, "dt_posicao", DbType.DateTime, pParametros.Objeto.Ate);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.Objeto.ListaPosicao.Add(CriarRegistroClientePosicaoListaAVistaInfo(lDataTable.Rows[i]));

                        //lNomeCliente   = lDataTable.Rows[i]["NOME_CLI"].ToString();
                        //lCodigoCliente = lDataTable.Rows[i]["COD_CLI_ORI"].ToString();
                    }
                }
            }

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CUS_FINAN_OPC_LST2"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "id_cliente", DbType.Int32, pParametros.Objeto.CodigoCliente);

                lAcessaDados.AddInParameter(lDbCommand, "dt_posicao", DbType.DateTime, pParametros.Objeto.Ate);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.Objeto.ListaPosicao.Add(CriarRegistroClientePosicaoListaOpcaoInfo(lDataTable.Rows[i]));
                    }
                }
            }

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CUS_FINAN_TER_LST"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "id_cliente", DbType.Int32, pParametros.Objeto.CodigoCliente);

                lAcessaDados.AddInParameter(lDbCommand, "dt_posicao", DbType.DateTime, pParametros.Objeto.Ate);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.Objeto.ListaPosicao.Add(CriarRegistroClientePosicaoListaTermoInfo(lDataTable.Rows[i]));
                    }
                }
            }

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CUS_FINAN_TES_LST"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "id_cliente", DbType.Int32, pParametros.Objeto.CodigoCliente);

                lAcessaDados.AddInParameter(lDbCommand, "dt_posicao", DbType.DateTime, pParametros.Objeto.Ate);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.Objeto.ListaPosicao.Add(CriarRegistroClientePosicaoListaTesouroInfo(lDataTable.Rows[i]));
                    }
                }
            }

            #region Extrato
            
            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CUS_EXTRATO_VIS_LST"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "id_cliente", DbType.Int32, pParametros.Objeto.CodigoCliente);

                //lAcessaDados.AddInParameter(lDbCommand, "de", DbType.DateTime, pParametros.Objeto.De);

                lAcessaDados.AddInParameter(lDbCommand, "dt_posicao", DbType.DateTime, pParametros.Objeto.Ate);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.Objeto.ListaExtrato.Add(CriarRegistroClienteExtratoListaInfo(lDataTable.Rows[i],"VIS"));
                    }
                }
            }

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CUS_EXTRATO_OPC_LST"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "id_cliente", DbType.Int32, pParametros.Objeto.CodigoCliente);

                lAcessaDados.AddInParameter(lDbCommand, "dt_posicao", DbType.DateTime, pParametros.Objeto.Ate);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.Objeto.ListaExtrato.Add(CriarRegistroClienteExtratoListaInfo(lDataTable.Rows[i],"OPC"));
                    }
                }
            }

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CUS_EXTRATO_TER_LST"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "id_cliente", DbType.Int32, pParametros.Objeto.CodigoCliente);

                lAcessaDados.AddInParameter(lDbCommand, "dt_posicao", DbType.DateTime, pParametros.Objeto.Ate);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.Objeto.ListaExtrato.Add(CriarRegistroClienteExtratoListaInfo(lDataTable.Rows[i],"TER"));
                    }
                }
            }

            #endregion

            return lRetorno;
        }

        private static ClienteExtratoInfo CriarRegistroClienteExtratoListaInfo(DataRow linha, string pTipo)
        {
            return new ClienteExtratoInfo()
            {
                Titulo             = linha["TITULO"].DBToString(),
                Dia                = linha["DIA"].DBToString(),
                DataNegocio        = linha["DTNEGOCIO"].DBToDateTime(),
                Historico          = linha["HISTORICO"].DBToString(),
                Qtde               = linha["QTD"].DBToInt32(),
                Tipo               = pTipo
            };
        }

        private static ClientePosicaoInfo CriarRegistroClientePosicaoListaAVistaInfo(DataRow linha)
        {
            return new ClientePosicaoInfo()
            {
                Titulo     = linha["Titulo"].ToString(),
                Quantidade = linha["QTD"].DBToInt32(),
                Custo      = linha["Custo"].DBToDecimal(),
                ValorAtual = linha["VALOR_ATUAL"].DBToDecimal(),
                Variacao   = linha["VARIACAO"].DBToDecimal(),
                Tipo = "VIS"
            };
        }

        private static ClientePosicaoInfo CriarRegistroClientePosicaoListaOpcaoInfo(DataRow linha)
        {
            return new ClientePosicaoInfo()
            {
                Titulo         = linha["Titulo"].DBToString(),
                Exercicio      = linha["Exercicio"].DBToDecimal(),
                Quantidade     = linha["QTD"].DBToInt32(),
                Custo          = linha["Custo"].DBToDecimal(),
                ValorExercicio = linha["VALOR_EXERCICIO"].DBToDecimal(),
                CustoTotal     = linha["CUSTO_TOTAL"].DBToDecimal(),
                ValorObjeto    = linha["VALOR_OBJETO"].DBToDecimal(),
                Tipo = "OPC"
            };
        }

        private static ClientePosicaoInfo CriarRegistroClientePosicaoListaTermoInfo(DataRow linha)
        {
            return new ClientePosicaoInfo()
            {
                Titulo         = linha["Titulo"].DBToString(),
                DataVencimento = linha["Vencimento"].DBToDateTime(),
                Quantidade     = linha["QTD"].DBToInt32(),
                DataRolagem    = linha["Rolagem"].DBToDateTime(),
                ValorTermo     = linha["VALOR"].DBToDecimal(),
                Tipo = "TER"
            };
        }

        private static ClientePosicaoInfo CriarRegistroClientePosicaoListaTesouroInfo(DataRow linha)
        {
            return new ClientePosicaoInfo()
            {
                Titulo              = linha["Titulo"].DBToString(),
                DataVencimento      = linha["Vencimento"].DBToDateTime(),
                QuantidadeTesouro   = linha["QTD"].DBToDecimal(),
                ValorOriginal       = linha["VALOR_ORIGINAL"].DBToDecimal(),
                Tipo                = "TES"
            };
        }
    }

    
}
