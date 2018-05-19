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
    public class ContratoDbLib : DbLibBase
    {
        public ConsultarObjetosResponse<ContratoInfo> ConsultarContrato(ConsultarEntidadeRequest<ContratoInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<ContratoInfo>();

                pParametros.Condicoes.Add(new CondicaoInfo());

                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "contrato_lst_sp"))
                {
                    DataRow linha;
                        DataTable lDataTable ;
                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                    {
                        lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {
                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            {
                                linha = lDataTable.NewRow();

                                linha["id_contrato"] = (lDataTable.Rows[i]["id_contrato"]).DBToInt32();
                                linha["ds_contrato"] = (lDataTable.Rows[i]["ds_contrato"]).DBToString();
                                linha["ds_path"] = (lDataTable.Rows[i]["ds_path"]).DBToString();
                                linha["st_obrigatorio"] = bool.Parse(lDataTable.Rows[i]["st_obrigatorio"].DBToString());

                                resposta.Resultado.Add(CriarRegistroContrato(linha));
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

        private static ContratoInfo CriarRegistroContrato(DataRow linha)
        {
            ContratoInfo lContratoInfo = new ContratoInfo();

            lContratoInfo.DsContrato = linha["ds_contrato"].DBToString();
            lContratoInfo.DsPath = linha["ds_path"].DBToString();
            lContratoInfo.IdContrato = linha["id_contrato"].DBToInt32();
            lContratoInfo.StObrigatorio = bool.Parse(linha["st_obrigatorio"].ToString());

            return lContratoInfo;
        }
    }
}
