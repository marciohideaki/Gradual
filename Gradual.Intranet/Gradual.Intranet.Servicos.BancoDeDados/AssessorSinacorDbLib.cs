using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ReceberObjetoResponse<AssessorSinacorInfo> ReceberAssessorSinacor(ReceberEntidadeRequest<AssessorSinacorInfo> pParametros)
        {
            var lRetorno = new ReceberObjetoResponse<AssessorSinacorInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoSinacorConsulta;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_consultar_assessor_sinacor"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCD_ASSESSOR", DbType.Int32, pParametros.Objeto.CdAssessor);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    lRetorno.Objeto = new AssessorSinacorInfo()
                    {
                        CdAssessor = lDataTable.Rows[0]["CD_ASSESSOR"].DBToInt32(),
                        CdEmpresa = lDataTable.Rows[0]["CD_EMPRESA"].DBToInt32(),
                        CdMunicipio = lDataTable.Rows[0]["CD_MUNICIPIO"].DBToInt32(),
                        CdUsuario = lDataTable.Rows[0]["CD_USUARIO"].DBToInt32(),
                        DsEmail = lDataTable.Rows[0]["NM_E_MAIL"].DBToString(),
                        DsNome = lDataTable.Rows[0]["NM_ASSESSOR"].DBToString(),
                        DsNomeResumido = lDataTable.Rows[0]["NM_RESU_ASSES"].DBToString(),
                        InSituac = lDataTable.Rows[0]["IN_SITUAC"].DBToChar(),
                        PcAdiantamento = lDataTable.Rows[0]["PC_ADIANTAMENTO"].DBToDecimal(),
                        TpOcorrencia = lDataTable.Rows[0]["TP_OCORRENCIA"].DBToChar(),
                    };
                }
            }

            return lRetorno;
        }
    }
}
