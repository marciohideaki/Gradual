using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Ordens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.Generico.Dados;
using System.Data;
using System.Data.Common;

namespace Gradual.Site.DbLib.Persistencias.MinhaConta.Ordens
{
    public class PersistenciaOrdens
    {
        public List<OrdemInfo> BuscarOrdem( string  CliOrdId )
        {
            var lRetorno = new List<OrdemInfo>();

            AcessaDados lDados = new AcessaDados();

            Conexao lConexao = new Generico.Dados.Conexao();

            lDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoRisco;

            using (DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_buscar_ordem_alteracao"))
            {
                lDados.AddInParameter(lCommand, "@ClOrdId", DbType.String, CliOrdId);

                DataTable lTabela = lDados.ExecuteDbDataTable(lCommand);

                OrdemInfo lInfo = null;

                foreach (DataRow lRow in lTabela.Rows)
                {
                    lInfo = new OrdemInfo();
                    lInfo.Symbol             = lRow["Symbol"].DBToString();
                    lInfo.ClOrdID            = lRow["ClOrdID"].DBToString();
                    lInfo.OrigClOrdID        = lRow["OrigClOrdID"].DBToString();
                    lInfo.IdOrdem            = lRow["OrderID"].DBToInt32();
                    lInfo.OrderQtyRemmaining = lRow["OrdQtyRemaining"].DBToInt32();
                    lInfo.Price              = lRow["Price"].DBToDouble();
                    lInfo.TransactTime       = lRow["TransactTime"].DBToDateTime();
                    lInfo.CumQty             = lRow["CumQty"].DBToInt32();
                    lInfo.Side               = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                    lInfo.OrdType            = (OrdemTipoEnum)lRow["OrdTypeID"].DBToInt32();
                    lInfo.TimeInForce        = (OrdemValidadeEnum)lRow["TimeInForce"].DBToInt32();


                    lRetorno.Add(lInfo);
                }
            }

            return lRetorno;
        }
    }
}
