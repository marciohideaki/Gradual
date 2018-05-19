using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using System.Data.Common;
using System.Data;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {

        public static ReceberObjetoResponse<MensagemAjudaInfo> ReceberMensagemPortal(ReceberEntidadeRequest<MensagemAjudaInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<MensagemAjudaInfo> lResposta = new ReceberObjetoResponse<MensagemAjudaInfo>();

                lResposta.Objeto = new MensagemAjudaInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "mensagem_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_mensagem", DbType.Int32, pParametros.Objeto.IdMensagem);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lResposta.Objeto.IdMensagem = (lDataTable.Rows[0]["Id_Mensagem"]).DBToInt32();
                        lResposta.Objeto.DsTitulo = (lDataTable.Rows[0]["Ds_Titulo"]).DBToString();
                        lResposta.Objeto.DsMensagem = (lDataTable.Rows[0]["Ds_Mensagem"]).DBToString();

                    }
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }


    }
}
