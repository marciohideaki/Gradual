using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Gradual.Generico.Dados;
using System.Data;
using Gradual.Spider.Lib.Dados;
using Gradual.Spider.Lib.Mensagens;

namespace Gradual.Spider.DbLib
{
    public class GerenciadorPlataformaDbLib
    {
        public const string NomeConexaoSpider       = "GradualSpider";
        public const string NomeConexaoCadastro     = "GradualCadastro";
        public const string NomeConexaoSinacorTrade = "SINACOR";

        public List<GerenciadorPlataformaInfo> ListarGerenciadorPlataforma(GerenciadorPlataformaInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new List<GerenciadorPlataformaInfo>();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_gerenciador_plataforma_lst_sp"))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        var lInfo = new GerenciadorPlataformaInfo();

                        lInfo.CodigoTraderPlataforma = lRow["id_trader_plataforma"] .DBToInt32();
                        lInfo.CodigoPlataforma       = lRow["id_plataforma"]        .DBToInt32();
                        lInfo.NomePlataforma         = lRow["ds_plataforma"]        .DBToString();
                        lInfo.CodigoSessao           = lRow["id_sessao"]            .DBToInt32();
                        lInfo.NomeSessao             = lRow["ds_sessao"]            .DBToString();
                        lInfo.DataUltimoEvento       = lRow["dt_atualizacao"]       .DBToDateTime();
                        lInfo.CodigoAcesso           = lRow["id_acesso"]            .DBToInt32();
                        lInfo.CodigoTrader           = lRow["id_trader"]            .DBToInt32();

                        lRetorno.Add(lInfo);
                    }
                }
            }

            return lRetorno;
        }

        public List<GerenciadorPlataformaInfo> SelecionarGerenciadorPlataforma(GerenciadorPlataformaInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new List<GerenciadorPlataformaInfo>();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_gerenciador_plataforma_sel_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_trader", DbType.Int32, pParametros.CodigoTrader);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        var lInfo = new GerenciadorPlataformaInfo();

                        lInfo.CodigoTraderPlataforma = lRow["id_trader_plataforma"].DBToInt32();
                        lInfo.CodigoPlataforma       = lRow["id_plataforma"].DBToInt32();
                        lInfo.NomePlataforma         = lRow["ds_plataforma"].DBToString();
                        lInfo.CodigoSessao           = lRow["id_sessao"].DBToInt32();
                        lInfo.NomeSessao             = lRow["ds_sessao"].DBToString();
                        lInfo.DataUltimoEvento       = lRow["dt_atualizacao"].DBToDateTime();
                        lInfo.CodigoAcesso           = lRow["id_acesso"].DBToInt32();
                        lInfo.CodigoTrader           = lRow["id_trader"].DBToInt32();

                        lRetorno.Add(lInfo);
                    }
                }
            }

            return lRetorno;
        }

        public GerenciadorPlataformaSalvarResponse InserirGerenciadorPlataforma(List<GerenciadorPlataformaInfo> pParametros)
        {
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            var lRetorno = new GerenciadorPlataformaSalvarResponse();

            for (int i = 0; i < pParametros.Count; i++)
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_gerenciador_plataforma_del_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_trader", DbType.Int32, pParametros[i].CodigoTrader);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
            }

            for (int i = 0; i < pParametros.Count; i++)
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_gerenciador_plataforma_ins_upd_sp"))
                {
                    if (pParametros[i].CodigoTraderPlataforma!= 0)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@id_trade_plataforma", DbType.Int32, pParametros[i].CodigoTraderPlataforma);
                    }

                    lAcessaDados.AddInParameter(lDbCommand, "@id_plataforma",   DbType.Int32,   pParametros[i].CodigoPlataforma);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_sessao",       DbType.Int32,   pParametros[i].CodigoSessao);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_ult_evento",   DbType.DateTime,pParametros[i].DataUltimoEvento);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_acesso",       DbType.Int32,   pParametros[i].CodigoAcesso);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_trader",       DbType.Int32,   pParametros[i].CodigoTrader);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.GerenciadorPlataforma = new GerenciadorPlataformaInfo();

                    lRetorno.DescricaoResposta = "Plataforma gerenciada inserida com sucesso";

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }

            return lRetorno;
        }
    }
}
