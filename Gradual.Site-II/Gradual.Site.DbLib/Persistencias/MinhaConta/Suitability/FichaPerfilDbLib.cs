using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Site.DbLib.Dados.MinhaConta.Suitability;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.DbLib.Persistencias.MinhaConta.Suitability
{
    public class FichaPerfilDbLib
    {
        public FichaPerfilResponse SelecionarFicha(FichaPerfilRequest pParametro)
        {
            var lRetorno = new FichaPerfilResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoEducacional;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_tb_ficha_perfil_sel1"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.ConsultaIdCliente);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Retorno.Add(new FichaPerfilInfo()
                        {
                            DsConhecimentoCapitais = lLinha["ds_conhecimento"].DBToString(),
                            DsFaixaEtaria = lLinha["ds_faixa_etaria"].DBToString(),
                            DsOcupacao = lLinha["ds_ocupacao"].DBToString(),
                            DsRendaFamiliar = lLinha["ds_renda_familiar"].DBToString(),
                            IdCliente = lLinha["id_cliente"].DBToInt32(),
                            IdFichaPerfil = lLinha["id_ficha_perfil"].DBToInt32(),
                            TpInstituicao = lLinha["tp_instituicao"].DBToString(),
                            TpInvestidor = lLinha["tp_investidor"].DBToString(),
                            TpInvestimento = lLinha["tp_investimento"].DBToString()
                        });
            }

            return lRetorno;
        }
    }
}
