using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Persistencias.Invetimentos
{
    public class PersistenciaLojas
    {
        public Gradual.Site.DbLib.Mensagens.LojaResponse BuscarLojas(Gradual.Site.DbLib.Mensagens.LojaRequest pRequest)
        {
            Gradual.Site.DbLib.Mensagens.LojaResponse lRetorno = new Gradual.Site.DbLib.Mensagens.LojaResponse();
            var lAcessaDados = new Gradual.Generico.Dados.AcessaDados();

            lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoCadastro;

            using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "lojas_lst_sp"))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    lRetorno.ListaLojas = new List<Dados.LojaInfo>();

                    foreach (System.Data.DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.ListaLojas.Add(new Gradual.Site.DbLib.Dados.LojaInfo()
                        {
                            Codigo = lLinha["Codigo"].DBToInt32(),
                            CNPJ = lLinha["CNPJ"].DBToInt64(),
                            RazaoSocial = lLinha["RazaoSocial"].DBToString(),
                            NomeFantasia = lLinha["NomeFantasia"].DBToString(),
                            Endereco = lLinha["Endereco"].DBToString(),
                            Telefone = lLinha["Telefone"].DBToString()
                        });
                    }
                }
            }

            return lRetorno;
        }
    }
}
