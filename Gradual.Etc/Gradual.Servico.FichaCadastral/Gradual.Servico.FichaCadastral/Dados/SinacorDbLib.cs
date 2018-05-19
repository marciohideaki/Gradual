using System;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;

namespace Gradual.Servico.FichaCadastral.Dados
{
    public class SinacorDbLib : DbLibBase
    {
        public ConsultarObjetosResponse<SinacorListaInfo> ConsultarListaSinacor(ConsultarEntidadeRequest<SinacorListaInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<SinacorListaInfo>();

                var lAcessaDados = new AcessaDados();

                var lInformacao = pParametros.Objeto.Informacao;

                if (Gradual.Intranet.Contratos.Dados.Enumeradores.eInformacao.AssessorPadronizado.Equals(lInformacao))
                    lInformacao = Gradual.Intranet.Contratos.Dados.Enumeradores.eInformacao.Assessor; //--> Corrigindo para realizar a consulta para assesor parametrizado.

                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ListaComboSinacor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "Informacao", DbType.Int32, (int)lInformacao);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        if (Gradual.Intranet.Contratos.Dados.Enumeradores.eInformacao.AssessorPadronizado.Equals(pParametros.Objeto.Informacao))
                            for (int i = 0; i < lDataTable.Rows.Count; i++) //--> ComboInfoNormalizada (id:{id},value:{id}-{descricao})
                                resposta.Resultado.Add(CriarSinacorListaInfoNormalizada(lDataTable.Rows[i]));
                        else
                            for (int i = 0; i < lDataTable.Rows.Count; i++) //--> ComboInfoPadrao (id:{id},value:{descricao})
                                resposta.Resultado.Add(CriarSinacorListaInfo(lDataTable.Rows[i]));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static SinacorListaInfo CriarSinacorListaInfoNormalizada(DataRow linha)
        {
            return new SinacorListaInfo()
            {
                Id = linha["id"].DBToString(),
                Value = string.Format("{0} - {1}", linha["id"].DBToString().Trim().PadLeft(4, '0'), linha["Value"].DBToString())
            };
        }

        private static SinacorListaInfo CriarSinacorListaInfo(DataRow linha)
        {
            SinacorListaInfo lSinacorListaInfo = new SinacorListaInfo();

            lSinacorListaInfo.Id = linha["id"].DBToString();
            lSinacorListaInfo.Value = linha["Value"].DBToString();

            return lSinacorListaInfo;

        }
    }
}
