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
    public class ClienteControladoraDbLib : DbLibBase
    {
        public ConsultarObjetosResponse<ClienteControladoraInfo> ConsultarClienteControladora(ConsultarEntidadeRequest<ClienteControladoraInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteControladoraInfo> resposta = 
                    new ConsultarObjetosResponse<ClienteControladoraInfo>();

                CondicaoInfo info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdClienteControladora);
                pParametros.Condicoes.Add(info);

                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_controladora_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        DataRow linha;

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            linha = lDataTable.NewRow();

                            linha["id_cliente_controlada"] = (lDataTable.Rows[i]["id_cliente_controlada"]).DBToInt32();
                            linha["id_cliente"] = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            linha["ds_nomerazaosocial"] = (lDataTable.Rows[i]["ds_nomerazaosocial"]).DBToString();
                            linha["ds_cpfcnpj"] = (lDataTable.Rows[i]["ds_cpfcnpj"]).DBToString();

                            resposta.Resultado.Add(CriarClienteControladora(linha));
                        }
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

        private static ClienteControladoraInfo CriarClienteControladora(DataRow linha)
        {
            var lClienteControladoraInfo = new ClienteControladoraInfo();

            lClienteControladoraInfo.DsCpfCnpj = linha["ds_cpfcnpj"].DBToString();
            lClienteControladoraInfo.DsNomeRazaoSocial = linha["ds_nomerazaosocial"].DBToString();
            lClienteControladoraInfo.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteControladoraInfo.IdClienteControladora = linha["id_cliente_controlada"].DBToInt32();

            return lClienteControladoraInfo;
        }
    }
}
