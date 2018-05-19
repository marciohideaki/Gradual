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
    public class LoginClienteDbLib
    {
        public LoginClienteUraResponse ConsultarCliente(LoginClienteUraRequest pParametro)
        {
            var lRetorno = new LoginClienteUraResponse();
            try
            {
                var lAcessaDados = new AcessaDados();
                lAcessaDados.ConnectionStringName = "Cadastro";

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "login_cliente_ura_sel_sp"))
                {
                    string lCpfCnpj = null, lCBLC = null;

                    this.ValidarEntradaConsultaCliente(pParametro.ObjetoDeConsulta.CodigoIdentificador, out lCpfCnpj, out lCBLC);

                    lAcessaDados.AddInParameter(lDbCommand, "@CpfCnpj", DbType.String, lCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@clbc", DbType.String, lCBLC);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lRetorno.ObjetoDeRetorno.IsLoginValido = true;
                        lRetorno.ObjetoDeRetorno.CpfCnpj = lDataTable.Rows[0]["ds_cpfcnpj"].DBToString();
                        lRetorno.ObjetoDeRetorno.CodigoAssessor = lDataTable.Rows[0]["cd_assessor"].DBToInt32();
                        lRetorno.ObjetoDeRetorno.TipoDeCliente = (TipoDeCliente)lDataTable.Rows[0]["tp_cliente"].DBToInt32();
                        lRetorno.ObjetoDeRetorno.SegmentacaoDoCliente = (SegmentacaoDoCliente)lDataTable.Rows[0]["tp_ClienteSegmentacao"].DBToInt32();

                        lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                    }
                    else
                    {
                        throw new Exception("Usuário informado não é válido.");
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

        private void ValidarEntradaConsultaCliente(string pCodigoIdentificador, out string pCpfCnpj, out string lCBLC)
        {
            string lCodigoIdentificador = string.IsNullOrWhiteSpace(pCodigoIdentificador) ? null : pCodigoIdentificador.Replace(".", "").Replace("-", "").Replace("/", "");

            pCpfCnpj = null;
            lCBLC = null;

            if (string.IsNullOrWhiteSpace(lCodigoIdentificador))
                throw new NullReferenceException("Identificação do cliente não informada.");

            else if (lCodigoIdentificador.Length <= 6)
                lCBLC = lCodigoIdentificador;

            else if (lCodigoIdentificador.Length < 12)
                pCpfCnpj = lCodigoIdentificador.PadLeft(11, '0');

            else pCpfCnpj = lCodigoIdentificador.PadLeft(14, '0');
        }
    }
}
