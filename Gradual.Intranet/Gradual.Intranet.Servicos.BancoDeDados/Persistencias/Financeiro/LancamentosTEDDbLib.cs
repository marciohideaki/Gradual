using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Financeiro
{
    public static partial class LancamentosTEDDbLib
    {
        public static string gNomeConexaoSinacor
        {
            get { return "SINACOR"; }
        }

        public static ConsultarObjetosResponse<LancamentoTEDInfo> ConsultarLancamentos(ConsultarEntidadeRequest<LancamentoTEDInfo> pRequest)
        {
            ConsultarObjetosResponse<LancamentoTEDInfo> lResposta = new ConsultarObjetosResponse<LancamentoTEDInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

            using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "PRC_LANCAMENTOS_TED_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "DtDe", System.Data.DbType.DateTime, pRequest.Objeto.DtDe);
                lAcessaDados.AddInParameter(lDbCommand, "DtAte", System.Data.DbType.DateTime, pRequest.Objeto.DtAte);
                lAcessaDados.AddInParameter(lDbCommand, "CodigoCliente", System.Data.DbType.Int32, pRequest.Objeto.CodigoCliente);

                System.Data.DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        lResposta.Resultado.Add(CriarRegistro(lDataTable.Rows[i]));

            }

            return lResposta;
        }

        private static LancamentoTEDInfo CriarRegistro(System.Data.DataRow linha)
        {
            return new LancamentoTEDInfo()
            {
                CodigoCliente       = linha["CodigoCliente"].DBToInt32(),
                NomeCliente         = linha["NomeCliente"].DBToString(),
                DataMovimento       = linha["DataMovimento"].DBToDateTime(),
                NumeroLancamento    = linha["NumeroLancamento"].DBToString(),
                Descricao           = linha["Descricao"].DBToString(),
                Valor               = linha["Valor"].DBToDecimal()
            };
        }
    }
}
