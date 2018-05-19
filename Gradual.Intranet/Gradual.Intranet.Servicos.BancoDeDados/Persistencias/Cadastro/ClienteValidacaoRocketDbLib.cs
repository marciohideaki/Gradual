using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.OMS.Library;
using System.Data.Common;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ConsultarObjetosResponse<ClienteValidacaoRocketInfo> ConsultarValidacaoClienteRocket(ConsultarEntidadeRequest<ClienteValidacaoRocketInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ClienteValidacaoRocketInfo>();

                CondicaoInfo info = new CondicaoInfo("@CPF", CondicaoTipoEnum.Igual, pParametros.Objeto.CPF);
                pParametros.Condicoes.Add(info);

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_rocket_lst_sp"))
                {
                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                        lAcessaDados.AddInParameter(lDbCommand, condicao.Propriedade, DbType.Int32, condicao.Valores[0]);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0) for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarClienteRocket(lDataTable.Rows[i]));
                }
                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static ClienteValidacaoRocketInfo CriarClienteRocket(DataRow linha)
        {
            var lClienteRocketInfo = new ClienteValidacaoRocketInfo();
            /*
            lClienteContaInfo.IdCliente           = linha["id_cliente"].DBToInt32();
            lClienteContaInfo.IdClienteConta      = linha["id_cliente_conta"].DBToInt32();
            lClienteContaInfo.StAtiva             = bool.Parse(linha["st_ativa"].ToString());
            lClienteContaInfo.StContaInvestimento = bool.Parse(linha["st_containvestimento"].ToString());
            lClienteContaInfo.StPrincipal         = bool.Parse(linha["st_principal"].ToS31tring());
            lClienteContaInfo.CdCodigo            = linha["cd_codigo"].DBToInt32();
            lClienteContaInfo.CdAssessor          = linha["cd_assessor"].DBToInt32();

            string lSis = (linha["cd_sistema"]).DBToString();

            if (lSis == "BOL")
                lClienteContaInfo.CdSistema = Contratos.Dados.Enumeradores.eAtividade.BOL;
            else if (lSis == "BMF")
                lClienteContaInfo.CdSistema = Contratos.Dados.Enumeradores.eAtividade.BMF;
            else if (lSis == "CUS")
                lClienteContaInfo.CdSistema = Contratos.Dados.Enumeradores.eAtividade.CUS;
            else
                lClienteContaInfo.CdSistema = Contratos.Dados.Enumeradores.eAtividade.CC;
            */
            return lClienteRocketInfo;

        }
    }
}
