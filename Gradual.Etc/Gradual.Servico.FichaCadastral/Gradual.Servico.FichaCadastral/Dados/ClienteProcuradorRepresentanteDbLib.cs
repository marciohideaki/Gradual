using System;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;

namespace Gradual.Servico.FichaCadastral.Dados
{
    public class ClienteProcuradorRepresentanteDbLib : DbLibBase
    {
        public ConsultarObjetosResponse<ClienteProcuradorRepresentanteInfo> ConsultarClienteProcuradorRepresentante(ConsultarEntidadeRequest<ClienteProcuradorRepresentanteInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteProcuradorRepresentanteInfo> resposta =
                    new ConsultarObjetosResponse<ClienteProcuradorRepresentanteInfo>();

                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_procuradorrepresentante_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        DataRow linha;
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            linha = lDataTable.NewRow();

                            linha["cd_orgaoemissor"] = (lDataTable.Rows[i]["cd_orgaoemissor"]).DBToString();
                            linha["cd_uforgaoemissordocumento"] = (lDataTable.Rows[i]["cd_uforgaoemissordocumento"]).DBToString();
                            linha["ds_cpfcnpj"] = (lDataTable.Rows[i]["ds_cpfcnpj"]).DBToString();
                            linha["ds_nome"] = (lDataTable.Rows[i]["ds_nome"]).DBToString();
                            linha["ds_numerodocumento"] = (lDataTable.Rows[i]["ds_numerodocumento"]).DBToString();
                            linha["dt_nascimento"] = (lDataTable.Rows[i]["dt_nascimento"]).DBToDateTime();
                            linha["id_cliente"] = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            linha["id_procuradorrepresentante"] = (lDataTable.Rows[i]["id_procuradorrepresentante"]).DBToInt32();
                            linha["tp_documento"] = (lDataTable.Rows[i]["tp_documento"]).DBToString();
                            linha["tp_situacaoLegal"] = (lDataTable.Rows[i]["tp_situacaoLegal"]).DBToInt32();
                            linha["dt_validade"] = (lDataTable.Rows[i]["dt_validade"]).DBToDateTime();

                            resposta.Resultado.Add(CriarRegistroClienteProcuradorRepresentanteInfo(linha));
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

        private static ClienteProcuradorRepresentanteInfo CriarRegistroClienteProcuradorRepresentanteInfo(DataRow linha)
        {
            var lClienteProcuradorRepresentanteInfo = new ClienteProcuradorRepresentanteInfo();

            lClienteProcuradorRepresentanteInfo.CdOrgaoEmissor = (linha["cd_orgaoemissor"]).DBToString();
            lClienteProcuradorRepresentanteInfo.CdUfOrgaoEmissor = (linha["cd_uforgaoemissordocumento"]).DBToString();
            lClienteProcuradorRepresentanteInfo.NrCpfCnpj = linha["ds_cpfcnpj"].DBToString();
            lClienteProcuradorRepresentanteInfo.DsNome = linha["ds_nome"].DBToString();
            lClienteProcuradorRepresentanteInfo.DsNumeroDocumento = linha["ds_numerodocumento"].DBToString();
            lClienteProcuradorRepresentanteInfo.DtNascimento = linha["dt_nascimento"].DBToDateTime();
            lClienteProcuradorRepresentanteInfo.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteProcuradorRepresentanteInfo.IdProcuradorRepresentante = linha["id_procuradorrepresentante"].DBToInt32();
            lClienteProcuradorRepresentanteInfo.TpDocumento = linha["tp_documento"].DBToString();
            lClienteProcuradorRepresentanteInfo.TpSituacaoLegal = linha["tp_situacaoLegal"].DBToInt32();
            lClienteProcuradorRepresentanteInfo.DtValidade = linha["dt_validade"].DBToDateTime() == DateTime.MinValue ? new System.Nullable<DateTime>() : linha["dt_validade"].DBToDateTime();

            return lClienteProcuradorRepresentanteInfo;
        }
    }
}
