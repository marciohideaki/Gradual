using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;
using Gradual.Generico.Dados;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ConsultarObjetosResponse<PosicaoConsolidadaPorPapelRelInfo> ConsultarPosicaoConsolidadaPorPapel(ConsultarEntidadeRequest<PosicaoConsolidadaPorPapelRelInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<PosicaoConsolidadaPorPapelRelInfo>();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = gNomeConexaoSinacorTrade;

            lRetorno.Resultado = new List<PosicaoConsolidadaPorPapelRelInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_POSICAO_CONSO_PORPAPEL_LST"))
	        {
                lAcessaDados.AddInParameter(lDbCommand, "pCod_Neg", DbType.String, pParametros.Objeto.ConsultaInstrumento);
                lAcessaDados.AddInParameter(lDbCommand, "pCd_Assessor", DbType.String, pParametros.Objeto.ConsultaCodigoAssessor);

                var lDatatable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDatatable && lDatatable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDatatable.Rows)
                        lRetorno.Resultado.Add(new PosicaoConsolidadaPorPapelRelInfo() 
                        {
                            AssessorCodigo = lLinha["CD_ASSESSOR"].DBToInt32(),
                            AssessorNome = lLinha["NM_ASSESSOR"].DBToString(),
                            ClienteCodigo = lLinha["COD_CLI"].DBToInt32(),
                            ClienteNome = lLinha["NOME_CLI"].DBToString(),
                            ClienteTipo = lLinha["DS_TIPO_CLIENTE"].DBToString(),
                            CodigoNegocio = lLinha["COD_NEG"].DBToString(),
                            DescricaoCarteira = lLinha["DESC_CART"].DBToString(),
                            Locador = lLinha["LOCADOR"].DBToInt32(),
                            QuantidadeD1 = lLinha["QTDE_DA1"].DBToInt32(),
                            QuantidadeD2 = lLinha["QTDE_DA2"].DBToInt32(),
                            QuantidadeD3 = lLinha["QTDE_DA3"].DBToInt32(),
                            QuantidadeDisponivel = lLinha["QTDE_DISP"].DBToInt32(),
                            QuantidadeTotal = lLinha["QTDE_TOT"].DBToInt32(),
                        });
	        }

            return lRetorno;
        }
    }
}
