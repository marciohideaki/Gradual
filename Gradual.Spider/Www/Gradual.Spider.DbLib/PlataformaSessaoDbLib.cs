using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Spider.Lib.Dados;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using Gradual.IntranetCorp.Lib.Mensagens;

namespace Gradual.Spider.DbLib
{
    public class PlataformaSessaoDbLib
    {
        public const string NomeConexaoSpider       = "GradualSpider";
        public const string NomeConexaoCadastro     = "GradualCadastro";
        public const string NomeConexaoSinacorTrade = "SINACOR";

        public List<PlataformaSessaoInfo> ListarPlataformaSessao(PlataformaSessaoInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new List<PlataformaSessaoInfo>();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_sessao_lst_sp"))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        var lInfo    = new PlataformaSessaoInfo();

                        lInfo.CodigoPlataformaSessao = lRow["id_plataforma_sessao"].DBToInt32();
                        lInfo.CodigoPlataforma       = lRow["id_plataforma"].DBToInt32();
                        lInfo.NomePlataforma         = lRow["ds_plataforma"].DBToString();
                        lInfo.CodigoSessao           = lRow["id_sessao"].DBToInt32();
                        lInfo.NomeSessao             = lRow["ds_sessao"].DBToString();
                        lInfo.StAtivo                = lRow["st_ativo"].DBToBoolean();
                        lInfo.ValorPlataforma        = lRow["vl_plataforma"].DBToDecimal();
                        lInfo.DataUltimoEvento       = lRow["dt_ult_evento"].DBToDateTime();
                        lInfo.CodigoAcesso           = lRow["id_acesso"].DBToInt32();

                        lRetorno.Add(lInfo);
                    }
                }
            }

            return lRetorno;
        }

        public List<PlataformaSessaoInfo> SelecionarPlataformaSessao(PlataformaSessaoInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new List<PlataformaSessaoInfo>();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_sessao_sel_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_plataforma", DbType.Int32, pParametros.CodigoPlataforma);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        var lInfo = new PlataformaSessaoInfo();

                        lInfo.CodigoPlataformaSessao = lRow["id_plataforma_sessao"].DBToInt32();
                        lInfo.CodigoPlataforma       = lRow["id_plataforma"].DBToInt32();
                        lInfo.NomePlataforma         = lRow["ds_plataforma"].DBToString();
                        lInfo.CodigoSessao           = lRow["id_sessao"].DBToInt32();
                        lInfo.NomeSessao             = lRow["ds_sessao"].DBToString();
                        lInfo.StAtivo                = lRow["st_ativo"].DBToBoolean();
                        lInfo.ValorPlataforma        = lRow["vl_plataforma"].DBToDecimal();
                        lInfo.DataUltimoEvento       = lRow["dt_ult_evento"].DBToDateTime();
                        lInfo.CodigoAcesso           = lRow["id_acesso"].DBToInt32();

                        lRetorno.Add(lInfo);
                    }
                }
            }

            return lRetorno;
        }

        public List<PlataformaSessaoInfo> ListarPlataforma(PlataformaSessaoInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new List<PlataformaSessaoInfo>();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_lst_sp"))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        var lInfo = new PlataformaSessaoInfo();

                        lInfo.CodigoPlataforma = lRow["id_plataforma"].DBToInt32();
                        lInfo.NomePlataforma   = lRow["ds_plataforma"].DBToString();

                        lRetorno.Add(lInfo);
                    }
                }
            }

            return lRetorno;
        }

        public List<PlataformaSessaoInfo> ListarSessao(PlataformaSessaoInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new List<PlataformaSessaoInfo>();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sessao_lst_sp"))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        var lInfo = new PlataformaSessaoInfo();

                        lInfo.CodigoSessao     = lRow["id_sessao"].DBToInt32();
                        lInfo.NomeSessao       = lRow["ds_sessao"].DBToString();
                        lInfo.StAtivo          = lRow["st_ativo"].DBToBoolean();
                        lInfo.Finalidade       = lRow["ds_finalidade"].DBToString();

                        lRetorno.Add(lInfo);
                    }
                }
            }

            return lRetorno;
        }

        public PlataformaSalvarResponse InserirPlataformaSessao(List<PlataformaSessaoInfo> pParametros)
        {

            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            var lRetorno = new PlataformaSalvarResponse();

            for (int i = 0; i < pParametros.Count; i++)
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_sessao_ins_upd_sp"))
                {
                    if (pParametros[i].CodigoPlataformaSessao != 0 )
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@id_plataforma_sessao", DbType.Int32, pParametros[i].CodigoPlataformaSessao);
                    }

                    lAcessaDados.AddInParameter(lDbCommand, "@id_plataforma",           DbType.Int32,       pParametros[i].CodigoPlataforma);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_sessao",               DbType.Int32,       pParametros[i].CodigoSessao);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_plataforma",           DbType.Decimal,     pParametros[i].ValorPlataforma);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_ativo",                DbType.Boolean,     pParametros[i].StAtivo);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_ult_evento",           DbType.DateTime,    pParametros[i].DataUltimoEvento);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_acesso",               DbType.Int32,       pParametros[i].CodigoAcesso);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.PlataformaSessao = new PlataformaSessaoInfo();

                    lRetorno.DescricaoResposta = "Plataforma x Sessão inserido com sucesso";

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                }
            }

            return lRetorno;
        }


    }
}
