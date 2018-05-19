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
    public class ClienteContratoDbLib : DbLibBase
    {
        public ConsultarObjetosResponse<ClienteContratoInfo> ConsultarClienteContrato(ConsultarEntidadeRequest<ClienteContratoInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteContratoInfo> resposta = new ConsultarObjetosResponse<ClienteContratoInfo>();

                string lProcedure = "cliente_contrato_lst_sp";

                if (pParametros.Objeto.CodigoBovespaCliente == null)
                {
                    CondicaoInfo info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdCliente);

                    pParametros.Condicoes.Add(info);
                }
                else
                {
                    CondicaoInfo info = new CondicaoInfo("@cod_bovespa_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.CodigoBovespaCliente);

                    pParametros.Condicoes.Add(info);

                    lProcedure = "cliente_contrato_lst_bov_sp";
                }

                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, lProcedure))
                {
                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, condicao.Propriedade, DbType.Int32, condicao.Valores[0]);
                    }

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            DataRow linha = lDataTable.NewRow();

                            linha["id_cliente"] = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            linha["id_contrato"] = (lDataTable.Rows[i]["id_contrato"]).DBToInt32();
                            linha["dt_assinatura"] = (lDataTable.Rows[i]["dt_assinatura"]).DBToDateTime();

                            resposta.Resultado.Add(CriarClienteContrato(linha));
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

        private static ClienteContratoInfo CriarClienteContrato(DataRow linha)
        {
            ClienteContratoInfo lClienteContrato = new ClienteContratoInfo();

            lClienteContrato.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteContrato.IdContrato = linha["id_contrato"].DBToInt32();
            lClienteContrato.DtAssinatura = linha["dt_assinatura"].DBToDateTime();

            return lClienteContrato;
        }
    }
}
