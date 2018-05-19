using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Spider.Lib.Dados;
using System.Data.Common;
using Gradual.Generico.Dados;
using System.Data;

namespace Gradual.Spider.DbLib
{
    public class ClienteDbLib
    {
        public const string NomeConexaoSpider        = "GradualSpider";
        public const string NomeConexaoCadastro      = "GradualCadastro";
        public const string NomeConexaoSinacorTrade  = "SINACOR";

        public ClienteInfo BuscarCliente (ClienteInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new List<ClienteInfo>();

            lAcessaDados.ConnectionStringName = NomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "spider_cliente_sel_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@cd_bovespa",      DbType.Int32,   pParametros.CodigoBovespa);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_bmf",          DbType.Int32,   pParametros.CodigoBmf);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_conta_mae",    DbType.Int32,   pParametros.ContaMae);
                

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        var lInfo = new ClienteInfo();

                        lInfo.CodigoBovespa           = lRow["cd_bovespa"].DBToInt32();
                        lInfo.CodigoLocalidade        = lRow["id_localidade"].DBToInt32();
                        lInfo.CodigoBmf               = lRow["cd_bmf"].DBToInt32();
                        lInfo.CodigoSessao            = lRow["cd_sessao"].DBToString();
                        lInfo.Nome                    = lRow["ds_nome"].DBToString();
                        lInfo.Email                   = lRow["ds_email"].DBToString();
                        //lInfo.ContaMae              = lRow["cd_conta_mae"].DBToString();
                        lInfo.CodigoAssessor          = lRow["cd_assessor"].DBToInt32();
                        lInfo.IdLogin                 = lRow["id_Login"].DBToInt32();

                        lRetorno.Add(lInfo);
                    }
                }
            }

            
            return  (lRetorno.Count> 0) ?lRetorno[0] : null;
        }

        public ClienteInfo InserirCliente(ClienteInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = NomeConexaoCadastro;
            
            var lRetorno = new ClienteInfo();

            this.SelecionaClienteSinacor(ref pParametros);

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "spider_cliente_ins_upd_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@idlogin",            DbType.Int32,   pParametros.IdLogin);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_bovespa",          DbType.Int32,   pParametros.CodigoBovespa);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_bmf",              DbType.Int32,   pParametros.CodigoBmf);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_conta_mae",        DbType.Int32,   pParametros.ContaMae);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_email",            DbType.String,  pParametros.Email);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_nome",             DbType.String,  pParametros.Nome);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj",          DbType.String,  pParametros.DsCpfCnpj);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor_inicial", DbType.Int32,   pParametros.CodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "@st_pessoavinculada",  DbType.Int32,   pParametros.EhPessoaVinculada);
                lAcessaDados.AddInParameter(lDbCommand, "@tp_pessoa",           DbType.String,  pParametros.TipoPessoa);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_sessao",           DbType.String,  pParametros.CodigoSessao);
                lAcessaDados.AddInParameter(lDbCommand, "@id_localidade",       DbType.Int32,   pParametros.CodigoLocalidade);

                lAcessaDados.ExecuteNonQuery(lDbCommand);

            }

            return lRetorno;
        }

        public void SelecionaClienteSinacor (ref ClienteInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = NomeConexaoSinacorTrade;

            //var lRetorno = new ClienteInfo();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_SPIDER_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCD_BOVESPA",  DbType.Int32, pParametros.CodigoBovespa);
                lAcessaDados.AddInParameter(lDbCommand, "pCD_BMF",      DbType.Int32, pParametros.CodigoBmf);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        pParametros.EhPessoaVinculada = lRow["in_pess_vinc"].ToString() == "S" ? true : false;
                        pParametros.DsCpfCnpj         = lRow["cd_cpfcgc"].DBToString();
                        pParametros.TipoPessoa        = lRow["tp_pessoa"].DBToString();
                    }
                }
            }
        }
    }
}
