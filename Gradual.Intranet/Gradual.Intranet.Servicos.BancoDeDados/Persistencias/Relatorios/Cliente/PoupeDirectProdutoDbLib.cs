using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Contratos.Dados.Cadastro;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Cliente
{
    public class PoupeDirectProdutoDbLib
    {
        public ConsultarObjetosResponse<PoupeDirectProdutoInfo> ConsultarPoupeDirectProduto(ConsultarEntidadeRequest<PoupeDirectProdutoInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<PoupeDirectProdutoInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRendaFixa;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "pr_sproduto"))
            {
                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(new PoupeDirectProdutoInfo()
                        {
                            DsProduto = lDataTable.Rows[i]["DS_PRODUTO"].DBToString(),
                            IdProduto = lDataTable.Rows[i]["ID_PRODUTO"].DBToInt32(),
                            QtDiasParaVencimento = lDataTable.Rows[i]["QTDE_DIAS_PARA_VENCIMENTO"].DBToInt32(),
                            QtDiasRetroTrocaPlano = lDataTable.Rows[i]["QTDE_DIAS_RETRO_TROCA_PLANO"].DBToInt32(),
                            VlPermanenciaMinima = lDataTable.Rows[i]["PERMANENCIA_MINIMA"].DBToInt32(),
                            VlPerrcentualMulta = lDataTable.Rows[i]["PERCENT_MULTA"].DBToDecimal(),
                        });
            }

            return lRetorno;
        }
    }
}
