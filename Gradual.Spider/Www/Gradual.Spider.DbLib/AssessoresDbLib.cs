using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Gradual.Spider.Lib;
using Gradual.Generico.Dados;
using Gradual.Generico.Geral;
using Gradual.Spider.Lib.Dados;

namespace Gradual.Spider.DbLib
{
    public class AssessoresDbLib
    {
        public const string NomeConexaoSpider          = "GradualSpider";
        public const string NomeConexaoCadastro        = "GradualCadastro";
        public const string NomeConexaoSinacor         = "SINACOR";
        public const string NomeConexaoSinacorConsulta = "SinacorConsulta";

        public AssessorInfo InserirAtualizarAssessorComplemento(AssessorInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new AssessorInfo();

            lAcessaDados.ConnectionStringName = NomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_complemento_ins_upd_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_login",            DbType.Int32,   pParametros.IdLoginAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "@st_ativo",            DbType.Boolean, pParametros.StAtivo);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_listaassessores",  DbType.String,  pParametros.ListaAssessoresFilhos);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_operador",         DbType.String,  pParametros.CodigoOperador);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_sigla",            DbType.String,  pParametros.CodigoSigla);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_sessao",           DbType.String,  pParametros.CodigoSessao);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_localidade",       DbType.Int32,   pParametros.CodigoLocalidade);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_nome",             DbType.String,  pParametros.NomeAssessor);

                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            lRetorno = pParametros;

            return lRetorno;
        }

        public List<AssessorInfo> ListarAssessorComplemento(AssessorInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new List<AssessorInfo>();

            lAcessaDados.ConnectionStringName = NomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_complemento_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_login",            DbType.Int32, pParametros.IdLoginAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_operador",         DbType.String, pParametros.CodigoOperador);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_sigla",            DbType.String, pParametros.CodigoSigla);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_sessao",           DbType.String, pParametros.CodigoSessao);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_localidade",       DbType.String, pParametros.CodigoLocalidade);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_nome",             DbType.String, pParametros.NomeAssessor);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        var lInfo = new AssessorInfo();

                        lInfo.StAtivo               = lRow["st_ativo"].DBToBoolean();
                        lInfo.CodigoLocalidade      = lRow["id_localidade"].DBToInt32();
                        lInfo.DsLocalidade          = lRow["ds_localidade"].DBToString();
                        lInfo.DtAtualizacao         = lRow["dt_atualizacao"].DBToDateTime();
                        lInfo.ListaAssessoresFilhos = lRow["cd_assessores_filhos"].DBToString();
                        lInfo.IdLoginAssessor       = lRow["id_login"].DBToInt32();
                        lInfo.CodigoOperador        = lRow["cd_operador"].DBToString();
                        lInfo.CodigoSessao          = lRow["cd_sessao"].DBToString();
                        lInfo.CodigoSigla           = lRow["cd_sigla"].DBToString();
                        lInfo.NomeAssessor          = lRow["ds_nome"].DBToString();

                        lRetorno.Add(lInfo);
                    }
                }
            }

            return lRetorno;
        }

        public List<SinacorListaInfo> ConsultarListaSinacor(SinacorListaInfo pParametros)
        {
            try
            {
                var lResposta = new List<SinacorListaInfo>();

                var lAcessaDados = new AcessaDados();

                var lInformacao = pParametros.Informacao;

                if (eInformacao.AssessorPadronizado.Equals(lInformacao))
                {
                    lInformacao = eInformacao.Assessor; //--> Corrigindo para realizar a consulta para assesor parametrizado.
                }

                lAcessaDados.ConnectionStringName = NomeConexaoSinacorConsulta;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ListaComboSinacor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "Informacao", DbType.Int32, (int)lInformacao);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        if (eInformacao.AssessorPadronizado.Equals(pParametros.Informacao))
                        {
                            for (int i = 0; i < lDataTable.Rows.Count; i++) //--> ComboInfoNormalizada (id:{id},value:{id}-{descricao})
                                lResposta.Add(CriarSinacorListaInfoNormalizada(lDataTable.Rows[i]));
                        }
                        else
                        {
                            for (int i = 0; i < lDataTable.Rows.Count; i++) //--> ComboInfoPadrao (id:{id},value:{descricao})
                                lResposta.Add(CriarSinacorListaInfo(lDataTable.Rows[i]));
                        }
                    }
                }

                return lResposta;
            }
            catch (Exception ex)
            {
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

        public Dictionary<int,string> ListarSessaoFix()
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new Dictionary<int, string>();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            string lSql = "Select idSessaoFix, Mnemonico from tb_sessao_fix_server order by Mnemonico asc";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        lRetorno.Add(lRow["idSessaoFix"].DBToInt32(), lRow["Mnemonico"].ToString());
                    }
                }
            }

            return lRetorno;
        }

        public Dictionary<int, string> ListarLocalidadeAssessor()
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new Dictionary<int, string>();

            lAcessaDados.ConnectionStringName = NomeConexaoCadastro;

            string lSql = "Select id_localidade, ds_localidade from tb_localidade_assessor where st_ativo = 1 order by id_localidade asc";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        lRetorno.Add(lRow["id_localidade"].DBToInt32(), lRow["ds_localidade"].ToString());
                    }
                }
            }

            return lRetorno;
        }
    }
}
