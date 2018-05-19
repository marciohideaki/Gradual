using System;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;

namespace Gradual.Servico.FichaCadastral.Dados
{
    public class ClienteBancoDbLib : DbLibBase
    {
        public ConsultarObjetosResponse<ClienteBancoInfo> ConsultarClienteBanco(ConsultarEntidadeRequest<ClienteBancoInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ClienteBancoInfo>();

                var info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdCliente);
                pParametros.Condicoes.Add(info);

                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_banco_lst_sp"))
                {
                    DataTable lDataTable;

                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, condicao.Propriedade, DbType.Int32, condicao.Valores[0]);
                        lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                                resposta.Resultado.Add(CriarRegistroClienteBanco(lDataTable.Rows[i]));
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static ClienteBancoInfo CriarRegistroClienteBanco(DataRow linha)
        {
            ClienteBancoInfo lClienteBancoInfo = new ClienteBancoInfo();

            lClienteBancoInfo.CdBanco = linha["cd_banco"].DBToString();
            lClienteBancoInfo.DsAgencia = linha["ds_agencia"].DBToString();
            lClienteBancoInfo.DsAgenciaDigito = linha["ds_agencia_digito"].DBToString();
            lClienteBancoInfo.DsConta = linha["ds_conta"].DBToString();
            lClienteBancoInfo.DsContaDigito = linha["ds_conta_digito"].DBToString();
            lClienteBancoInfo.IdBanco = linha["id_banco"].DBToInt32();
            lClienteBancoInfo.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteBancoInfo.StPrincipal = Boolean.Parse(linha["st_principal"].ToString());
            lClienteBancoInfo.TpConta = linha["tp_conta"].DBToString();

            return lClienteBancoInfo;
        }
    }
}
