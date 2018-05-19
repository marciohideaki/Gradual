using System;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Servicos.BancoDeDados;
using Gradual.OMS.Library;
using Gradual.WebService.AtendimentoUra.Lib;
using Gradual.WebService.AtendimentoUra.Lib.Mensagem;

namespace Gradual.WebService.AtendimentoUra.DbLib
{
    public class LoginAssessorDbLib
    {
        public LoginAssessorUraResponse ConsultarAssessor(LoginAssessorUraRequest pParametro)
        {
            var lRetorno = new LoginAssessorUraResponse();

            try
            {
                var lAcessaDados = new AcessaDados();
                lAcessaDados.ConnectionStringName = "Cadastro";

                pParametro.ObjetoDeConsulta.CodigoAssessor = pParametro.ObjetoDeConsulta.Senha;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_assessor_ura_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int32, pParametro.ObjetoDeConsulta.CodigoAssessor);
                    //lAcessaDados.AddInParameter(lDbCommand, "@cd_senha", DbType.Int32, pParametro.ObjetoDeConsulta.Senha);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lRetorno.ObjetoDeRetorno = new AtendimentoUraAssessorInfo()
                        {
                            CodigoAssessor = lDataTable.Rows[0]["cd_assessor"].DBToString(),
                            CpfCnpj = this.RecuperarCpfCnpj(lDataTable.Rows),
                            IsLoginValido = true,
                        };

                        lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                    }
                    else
                    {
                        throw new Exception("Dado de assessor informado não é válido.");
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.Message;
                lRetorno.ObjetoDeRetorno.IsLoginValido = false;
            }

            return lRetorno;
        }

        private string RecuperarCpfCnpj(DataRowCollection pDataRowCollection)
        {
            var lRetorno = "99999999999";
            var lContador = default(int);

            while (null != pDataRowCollection && pDataRowCollection.Count > lContador)
            {
                if (!string.IsNullOrWhiteSpace(pDataRowCollection[lContador]["ds_cpfcnpj"].DBToString()))
                {
                    lRetorno = pDataRowCollection[lContador]["ds_cpfcnpj"].DBToString();
                    break;
                }

                lContador++;
            }

            return lRetorno;
        }
    }
}
