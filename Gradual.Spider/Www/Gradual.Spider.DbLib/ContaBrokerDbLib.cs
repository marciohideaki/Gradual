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
    public class ContaBrokerDbLib
    {
        public const string NomeConexaoSpider = "GradualSpider";

        public ContaBrokerInfo InserirContaBroker(ContaBrokerInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new ContaBrokerInfo();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_conta_broker_ins"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_conta_broker", DbType.Int32, pParametros.IdContaBroker);
                lAcessaDados.AddInParameter(lDbCommand, "@id_conta_cliente",DbType.Int32, pParametros.IdContaCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@nome_cliente",    DbType.String, pParametros.NomeCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@st_ativo",        DbType.Boolean, pParametros.StAtivo);

                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            lRetorno = pParametros;

            return lRetorno;
        }

        public ContaBrokerInfo DeletarContaBroker(ContaBrokerInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new ContaBrokerInfo();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_conta_broker_del"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_conta_broker",    DbType.Int32, pParametros.IdContaBroker);
                lAcessaDados.AddInParameter(lDbCommand, "@id_conta_cliente",   DbType.Int32, pParametros.IdContaCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@nome_cliente",       DbType.String, pParametros.NomeCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@st_ativo",           DbType.Boolean, pParametros.StAtivo);

                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            lRetorno = pParametros;

            return lRetorno;
        }

        public List<ContaBrokerInfo> ListarContaBroker(ContaBrokerInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new List<ContaBrokerInfo>();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_conta_broker_lst"))
            {
                //lAcessaDados.AddInParameter(lDbCommand, "@id_conta_broker", DbType.Int32, pParametros.IdContaBroker);
                //lAcessaDados.AddInParameter(lDbCommand, "@id_conta_cliente", DbType.Int32, pParametros.IdContaCliente);
                //lAcessaDados.AddInParameter(lDbCommand, "@nome_cliente",    DbType.String, pParametros.NomeCliente);
                //lAcessaDados.AddInParameter(lDbCommand, "@st_ativo",        DbType.Boolean, pParametros.StAtivo);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        var lInfo = new ContaBrokerInfo();

                        lInfo.IdContaBroker  = lRow["idContaBroker"].DBToInt32();
                        lInfo.IdContaCliente = lRow["idCliente"].DBToInt32();
                        lInfo.NomeCliente    = lRow["dsCliente"].DBToString();
                        lInfo.StAtivo        = lRow["stAtivo"].DBToBoolean();

                        lRetorno.Add(lInfo);
                    }
                }
            }

            return lRetorno;
        }

        public List<ContaBrokerInfo> ConsultarContaBroker(ContaBrokerInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new List<ContaBrokerInfo>();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_conta_broker_lst"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_conta_broker", DbType.Int32, pParametros.IdContaBroker);
                lAcessaDados.AddInParameter(lDbCommand, "@id_conta_cliente", DbType.Int32, pParametros.IdContaCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@nome_cliente",    DbType.String, pParametros.NomeCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@st_ativo",        DbType.Boolean, pParametros.StAtivo);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        var lInfo = new ContaBrokerInfo();

                        lInfo.IdContaBroker  = lRow["idContaBroker"].DBToInt32();
                        lInfo.IdContaCliente = lRow["idCliente"].DBToInt32();
                        lInfo.NomeCliente    = lRow["dsCliente"].DBToString();
                        lInfo.StAtivo        = lRow["stAtivo"].DBToBoolean();

                        lRetorno.Add(lInfo);
                    }
                }
            }

            return lRetorno;
        }
    }
}
