using System;
using System.Diagnostics;
using System.Text;
using Gradual.Intranet.Contratos;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.RegrasDeNegocio
{
    public class GravacaoLog
    {
        public SalvarEntidadeResponse<LogEmailInfo> GravarLogEmail(SalvarObjetoRequest<LogEmailInfo> pInfo)
        {
            try
            {
                var lDestinatarios = new StringBuilder();
                var lLog = Ativador.Get<IServicoPersistenciaCadastro>();

                {   //--> Resgatando os e-mails para informar no log.
                    pInfo.Objeto.Destinatarios.ForEach(delegate(string email) { lDestinatarios.AppendFormat("{0};", email); });
                    pInfo.Objeto.DestinatariosCC.ForEach(delegate(string email) { lDestinatarios.AppendFormat("{0};", email); });
                    pInfo.Objeto.DestinatariosCO.ForEach(delegate(string email) { lDestinatarios.AppendFormat("{0};", email); });
                }

                lLog.SalvarEntidadeCadastro<EmailDisparadoPeriodoInfo>(new Contratos.Mensagens.SalvarEntidadeCadastroRequest<EmailDisparadoPeriodoInfo>()
                {
                    EntidadeCadastro = new EmailDisparadoPeriodoInfo()
                    {
                        ETipoEmailDisparo     = pInfo.Objeto.ETipoEmailDisparo,
                        DsCorpoEmail          = pInfo.Objeto.CorpoMensagem,
                        DsEmailRemetente      = pInfo.Objeto.Remetente,
                        DsEmailDestinatario   = lDestinatarios.ToString(),
                        DtEnvio               = pInfo.Objeto.DtEnvio,
                        DsAssuntoEmail        = pInfo.Objeto.Assunto,
                        DsPerfil              = pInfo.Objeto.Perfil,
                        IdCliente = pInfo.Objeto.IdCliente.GetValueOrDefault()
                    },
                });
            }
            catch (Exception ex)
            {
                var lSource = "LogEnvioEmail";

                if (!EventLog.Exists(lSource))
                    EventLog.CreateEventSource(lSource, "Application");

                EventLog.WriteEntry(lSource, ex.ToString());
            }

            return new SalvarEntidadeResponse<LogEmailInfo>();
        }
    }
}
