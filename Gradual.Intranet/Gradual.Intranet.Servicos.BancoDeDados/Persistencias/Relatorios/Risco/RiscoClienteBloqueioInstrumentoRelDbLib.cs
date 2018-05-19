using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Risco
{
    public class RiscoClienteBloqueioInstrumentoRelDbLib
    {
        #region | Métodos CRUD

        public ConsultarObjetosResponse<RiscoClienteBloqueioInstrumentoRelInfo> ConsultarClienteBloqueioInstrumento(ConsultarEntidadeRequest<RiscoClienteBloqueioInstrumentoRelInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoClienteBloqueioInstrumentoRelInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRisco;

            lRetorno.Resultado = new List<RiscoClienteBloqueioInstrumentoRelInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_relatorio_cliente_bloqueio_instrumento"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.String, pParametros.Objeto.CdAssessor);
                if (!string.IsNullOrWhiteSpace(pParametros.Objeto.DsCpfCnpj))
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.DsCpfCnpj);
                if (!string.IsNullOrWhiteSpace(pParametros.Objeto.DsNome))
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                if (!string.IsNullOrWhiteSpace(pParametros.Objeto.CdAtivo))
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_ativo", DbType.String, pParametros.Objeto.CdAtivo);
                if (null != pParametros.Objeto.CdCodigo)
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_codigo", DbType.String, pParametros.Objeto.CdCodigo);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoClienteBloqueioInstrumento(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        #endregion

        #region | Métodos de apoio

        private RiscoClienteBloqueioInstrumentoRelInfo CarregarEntidadeRiscoClienteBloqueioInstrumento(DataRow lLinha)
        {
            return new RiscoClienteBloqueioInstrumentoRelInfo()
            {
                CdAssessor = lLinha["cd_assessor"].DBToInt32(),
                CdAtivo = lLinha["cd_ativo"].DBToString(),
                CdCodigo = lLinha["cd_codigo"].DBToString(),
                DsCpfCnpj = lLinha["ds_cpfcnpj"].DBToString(),
                DsDirecao = lLinha["ds_direcao"].DBToString(),
                DsNome = lLinha["ds_nome"].DBToString()
            };
        }

        #endregion
    }
}
