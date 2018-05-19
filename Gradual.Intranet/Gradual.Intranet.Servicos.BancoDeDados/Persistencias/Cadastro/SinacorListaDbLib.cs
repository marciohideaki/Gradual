using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ConsultarObjetosResponse<SinacorListaComboInfo> ConsultarListaComboSinacor(ConsultarEntidadeRequest<SinacorListaComboInfo> pParametros)
        {
            try
            {
                var lRetorno = new ConsultarObjetosResponse<SinacorListaComboInfo>();
                var lAcessaDados = new ConexaoDbHelper();
                var lInformacao = pParametros.Objeto.Informacao;

                if (Contratos.Dados.Enumeradores.eInformacao.AssessorPadronizado.Equals(lInformacao))
                    lInformacao = Contratos.Dados.Enumeradores.eInformacao.Assessor; //--> Corrigindo para realizar a consulta para assesor parametrizado.

                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_SelecionaComboSinacor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "Informacao", DbType.Int32, (int)lInformacao);
                    lAcessaDados.AddInParameter(lDbCommand, "Filtro", DbType.String, pParametros.Objeto.Filtro);

                    var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count.CompareTo(0).Equals(1))
                        if (Contratos.Dados.Enumeradores.eInformacao.AssessorPadronizado.Equals(pParametros.Objeto.Informacao))
                            for (int i = 0; i < lDataTable.Rows.Count; i++) //--> ComboInfoNormalizada (id:{id},value:{id}-{descricao})
                                lRetorno.Resultado.Add(CriarSinacorListaComboInfoNormalizada(lDataTable.Rows[i]));
                        else
                            for (int i = 0; i < lDataTable.Rows.Count; i++) //--> ComboInfoPadrao (id:{id},value:{descricao})
                                lRetorno.Resultado.Add(CriarSinacorListaComboInfoInfo(lDataTable.Rows[i]));
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static ConsultarObjetosResponse<SinacorListaInfo> ConsultarListaSinacor(ConsultarEntidadeRequest<SinacorListaInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<SinacorListaInfo> resposta = new ConsultarObjetosResponse<SinacorListaInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                var lInformacao = pParametros.Objeto.Informacao;

                if (Contratos.Dados.Enumeradores.eInformacao.AssessorPadronizado.Equals(lInformacao))
                    lInformacao = Contratos.Dados.Enumeradores.eInformacao.Assessor; //--> Corrigindo para realizar a consulta para assesor parametrizado.

                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ListaComboSinacor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "Informacao", DbType.Int32, (int)lInformacao);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        if (Contratos.Dados.Enumeradores.eInformacao.AssessorPadronizado.Equals(pParametros.Objeto.Informacao))
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
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static SinacorListaInfo CriarSinacorListaInfo(DataRow linha)
        {
            SinacorListaInfo lSinacorListaInfo = new SinacorListaInfo();

            lSinacorListaInfo.Id = linha["id"].DBToString();
            lSinacorListaInfo.Value = linha["Value"].DBToString();

            return lSinacorListaInfo;

        }

        private static SinacorListaInfo CriarSinacorListaInfoNormalizada(DataRow linha)
        {
            return new SinacorListaInfo()
            {
                Id = linha["id"].DBToString(),
                Value = string.Format("{0} - {1}", linha["id"].DBToString().Trim().PadLeft(4, '0'), linha["Value"].DBToString())
            };
        }

        private static SinacorListaComboInfo CriarSinacorListaComboInfoInfo(DataRow linha)
        {
            return new SinacorListaComboInfo()
            {
                Id = linha["id"].DBToString(),
                Value = linha["Value"].DBToString(),
            };
        }

        private static SinacorListaComboInfo CriarSinacorListaComboInfoNormalizada(DataRow linha)
        {
            return new SinacorListaComboInfo()
            {
                Id = linha["id"].DBToString(),
                Value = string.Format("{0} - {1}", linha["id"].DBToString().Trim().PadLeft(4, '0'), linha["Value"].DBToString())
            };
        }
    }
}
