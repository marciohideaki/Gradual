using System.Data;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using System.Collections.Generic;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ConsultarEntidadeResponse<UltimasNegociacoesInfo> ConsultarUltimasNegociacoesCliente(ConsultarEntidadeRequest<UltimasNegociacoesInfo> pParametros)
        {
            var lRetorno = new ConsultarEntidadeResponse<UltimasNegociacoesInfo>();
            lRetorno.Resultado = new List<UltimasNegociacoesInfo>();

            var lAcessaDados = new ConexaoDbHelper();
            lAcessaDados.ConnectionStringName = gNomeConexaoSinacorTrade;

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CLIENTE_DT_NEG"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "CD_CLIENTE",       DbType.String,      pParametros.Objeto.CdCliente);
                lAcessaDados.AddInParameter(lDbCommand, "CD_CLIENTE_BMF",   DbType.String,      pParametros.Objeto.CdClienteBmf);
                lAcessaDados.AddInParameter(lDbCommand, "DT_INICIAL",       DbType.DateTime,    pParametros.Objeto.DataDe);
                lAcessaDados.AddInParameter(lDbCommand, "DT_FINAL",         DbType.DateTime,    pParametros.Objeto.DataAte);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                foreach (DataRow row in lDataTable.Rows)
                {
                    lRetorno.Resultado.Add(new UltimasNegociacoesInfo()
                    {
                        CdCliente = row["CD_CLIENTE"].DBToInt32(),
                        TipoBolsa = row["TIPO_BOLSA"].DBToString(),
                        DtUltimasNegociacoes = row["DT_NEGOCIO"].DBToDateTime()
                    });
                }
            }

            return lRetorno;
        }
    }
}
