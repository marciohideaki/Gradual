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
    public class ContaCorrenteDbLib : DbLibBase
    {
        public ConsultarObjetosResponse<ClienteContaInfo> ConsultarClienteConta(ConsultarEntidadeRequest<ClienteContaInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ClienteContaInfo>();

                var info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdCliente);
                pParametros.Condicoes.Add(info);

                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_conta_lst_sp"))
                {
                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                        lAcessaDados.AddInParameter(lDbCommand, condicao.Propriedade, DbType.Int32, condicao.Valores[0]);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0) for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarClienteConta(lDataTable.Rows[i]));
                }
                return resposta;
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static ClienteContaInfo CriarClienteConta(DataRow linha)
        {
            ClienteContaInfo lClienteContaInfo = new ClienteContaInfo();

            lClienteContaInfo.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteContaInfo.IdClienteConta = linha["id_cliente_conta"].DBToInt32();
            lClienteContaInfo.StAtiva = bool.Parse(linha["st_ativa"].ToString());
            lClienteContaInfo.StContaInvestimento = bool.Parse(linha["st_containvestimento"].ToString());
            lClienteContaInfo.StPrincipal = bool.Parse(linha["st_principal"].ToString());
            lClienteContaInfo.CdCodigo = linha["cd_codigo"].DBToInt32();
            lClienteContaInfo.CdAssessor = linha["cd_assessor"].DBToInt32();

            string lSis = (linha["cd_sistema"]).DBToString();

            if (lSis == "BOL")
                lClienteContaInfo.CdSistema = Gradual.Intranet.Contratos.Dados.Enumeradores.eAtividade.BOL;
            else if (lSis == "BMF")
                lClienteContaInfo.CdSistema = Gradual.Intranet.Contratos.Dados.Enumeradores.eAtividade.BMF;
            else if (lSis == "CUS")
                lClienteContaInfo.CdSistema = Gradual.Intranet.Contratos.Dados.Enumeradores.eAtividade.CUS;
            else if (lSis == "CC")
                lClienteContaInfo.CdSistema = Gradual.Intranet.Contratos.Dados.Enumeradores.eAtividade.CC;
            else if (lSis == "CAM")
                lClienteContaInfo.CdSistema = Gradual.Intranet.Contratos.Dados.Enumeradores.eAtividade.CAM;

            return lClienteContaInfo;

        }
    }
}
