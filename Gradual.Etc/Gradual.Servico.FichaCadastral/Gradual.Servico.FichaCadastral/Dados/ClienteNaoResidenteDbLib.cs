using System;
using System.Data;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;

namespace Gradual.Servico.FichaCadastral.Dados
{
    public class ClienteNaoResidenteDbLib : DbLibBase
    {
        public ConsultarObjetosResponse<ClienteInvestidorNaoResidenteInfo> ConsultarClienteNaoResidente(ConsultarEntidadeRequest<ClienteInvestidorNaoResidenteInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ClienteInvestidorNaoResidenteInfo>();
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_investidor_naoresidente_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente == 0 ? (object)System.DBNull.Value : pParametros.Objeto.IdCliente);
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);


                    if (null != lDataTable && lDataTable.Rows.Count.CompareTo(0).Equals(1))
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarRegistroNaoResidente(lDataTable.Rows[i]));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static ClienteInvestidorNaoResidenteInfo CriarRegistroNaoResidente(DataRow dr)
        {
            ClienteInvestidorNaoResidenteInfo lRetorno = new ClienteInvestidorNaoResidenteInfo();

            lRetorno.IdInvestidorNaoResidente = (dr["id_investidor_naoresidente"]).DBToInt32();
            lRetorno.IdCliente = (dr["id_cliente"]).DBToInt32();
            lRetorno.DsRepresentanteLegal = dr["ds_representantelegal"].DBToString();
            lRetorno.DsNomeAdiministradorCarteira = (dr["ds_nomeadiministradorcarteira"]).DBToString();
            lRetorno.CdPaisOrigem = (dr["cd_paisorigem"]).DBToString();
            lRetorno.DsCustodiante = (dr["ds_custodiante"]).DBToString();
            lRetorno.DsRde = (dr["ds_rde"]).DBToInt32();
            lRetorno.DsCodigoCvm = (dr["ds_codigocvm"]).DBToInt32();
            
            return lRetorno;
        }
    }
}
