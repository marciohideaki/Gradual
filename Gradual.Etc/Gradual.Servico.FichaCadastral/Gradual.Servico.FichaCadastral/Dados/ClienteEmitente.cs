using System;
using System.Data;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;
using Gradual.OMS.Library;
using System.Data.Common;

namespace Gradual.Servico.FichaCadastral.Dados
{
    public class ClienteEmitente : DbLibBase
    {
        public ConsultarObjetosResponse<ClienteEmitenteInfo> ConsultarClienteEmitente(ConsultarEntidadeRequest<ClienteEmitenteInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteEmitenteInfo> resposta =
                    new ConsultarObjetosResponse<ClienteEmitenteInfo>();

                var info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdCliente);
                pParametros.Condicoes.Add(info);

                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_emitente_lst_sp"))
                {
                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, condicao.Propriedade, DbType.Int32, condicao.Valores[0]);

                        DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {

                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            {
                                DataRow linha = lDataTable.NewRow();

                                linha["id_cliente"] = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                                linha["ds_nome"] = (lDataTable.Rows[i]["ds_nome"]).DBToString();
                                linha["ds_cpfcnpj"] = (lDataTable.Rows[i]["ds_cpfcnpj"]).DBToString();
                                linha["st_principal"] = Boolean.Parse(lDataTable.Rows[i]["st_principal"].ToString());
                                linha["ds_numerodocumento"] = (lDataTable.Rows[i]["ds_numerodocumento"]).DBToString();
                                linha["cd_sistema"] = (lDataTable.Rows[i]["cd_sistema"]).DBToString();
                                linha["ds_email"] = (lDataTable.Rows[i]["ds_email"]).DBToString();
                                linha["ds_data"] = (lDataTable.Rows[i]["ds_data"]).DBToDateTime();
                                linha["id_pessoaautorizada"] = (lDataTable.Rows[i]["id_pessoaautorizada"]).DBToInt32();
                                linha["dt_nascimento"] = (lDataTable.Rows[i]["dt_nascimento"]).DBToDateTime();

                                resposta.Resultado.Add(CriarRegistroClienteEmitente(linha));
                            }

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

        private static ClienteEmitenteInfo CriarRegistroClienteEmitente(DataRow linha)
        {
            var lClienteEmitente = new ClienteEmitenteInfo();

            lClienteEmitente.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteEmitente.DsNome = linha["ds_nome"].DBToString();
            lClienteEmitente.NrCpfCnpj = linha["ds_cpfcnpj"].DBToString();
            lClienteEmitente.StPrincipal = Boolean.Parse(linha["st_principal"].ToString());
            lClienteEmitente.DsNumeroDocumento = linha["ds_numerodocumento"].DBToString();
            lClienteEmitente.CdSistema = linha["cd_sistema"].DBToString();
            lClienteEmitente.DsEmail = linha["ds_email"].DBToString();
            lClienteEmitente.DsData = linha["ds_data"].DBToDateTime();
            lClienteEmitente.IdPessoaAutorizada = linha["id_pessoaautorizada"].DBToInt32();
            lClienteEmitente.DtNascimento = linha["dt_nascimento"].DBToDateTime();

            return lClienteEmitente;
        }
    }
}
