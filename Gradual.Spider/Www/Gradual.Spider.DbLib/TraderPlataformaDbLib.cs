using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Spider.Lib.Dados;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;

namespace Gradual.Spider.DbLib
{
    public class TraderPlataformaDbLib
    {
        public const string NomeConexaoSpider = "GradualSpider";

        public List<TraderPlataformaInfo> ListarPlataformaSessao(TraderPlataformaInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new List<TraderPlataformaInfo>();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_trader_plataforma_lst_sp"))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        var lInfo = new TraderPlataformaInfo();

                        lInfo.CodigoPlataforma       = lRow["id_plataforma"].DBToInt32();
                        lInfo.NomePlataforma         = lRow["ds_plataforma"].DBToString();
                        lInfo.CodigoSessao           = lRow["id_sessao"].DBToInt32();
                        lInfo.NomeSessao             = lRow["ds_sessao"].DBToString();
                        lInfo.CodigoAcesso           = lRow["id_acesso"].DBToInt32();
                        lInfo.NomeAcesso             = lRow["ds_acesso"].DBToString();
                        lInfo.DataAtualizacao        = lRow["dt_atualizacao"].DBToDateTime();
                        lInfo.CodigoTrader           = lRow["id_trader"].DBToInt32();
                        lInfo.CodigoTraderPlataforma = lRow["id_trader_plataforma"].DBToInt32();

                        lRetorno.Add(lInfo);
                    }
                }
            }

            return lRetorno;
        }

        public TraderPlataformaInfo InserirPlataformaSessao(TraderPlataformaInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            var lRetorno = new TraderPlataformaInfo();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_trader_plataforma_ins_upd_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_trader_plataforma",DbType.Int32, pParametros.CodigoTraderPlataforma);
                lAcessaDados.AddInParameter(lDbCommand, "@id_trader",           DbType.Int32, pParametros.CodigoTrader);
                lAcessaDados.AddInParameter(lDbCommand, "@id_plataforma",       DbType.Int32, pParametros.CodigoPlataforma);
                lAcessaDados.AddInParameter(lDbCommand, "@id_sessao",           DbType.Int32, pParametros.CodigoSessao);
                lAcessaDados.AddInParameter(lDbCommand, "@id_acesso",           DbType.Int32, pParametros.CodigoAcesso);
                lAcessaDados.AddInParameter(lDbCommand, "@dt_atualizacao",      DbType.DateTime, pParametros.DataAtualizacao);

                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            return lRetorno;
        }
    }
}
