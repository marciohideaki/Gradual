using System;
using System.Collections.Generic;
using System.Text;
using Gradual.Intranet.Contratos;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Email.Lib;

namespace Gradual.Intranet.Servicos.RegrasDeNegocio 
{
    public class ClientePendenciaAssessorRnLib
    {
        //#region | Atributos

        //private const string gEmailAssunto = "Assessor você tem clientes com pendência cadastral";

        //private EmailInfo gMail = new EmailInfo();

        //#endregion

        //#region | Propriedades

        //private string CorpoDoEmail
        //{
        //    get
        //    {
        //        var lStringBuilder = new StringBuilder();

        //        lStringBuilder.AppendLine("<html>");
        //        lStringBuilder.AppendLine("     <body style=\"font-family:Trebuchet MS, Arial, Sans-Serif; font-size:14px;\">");
        //        lStringBuilder.AppendLine("          <p>Caro assessor {0}.</p>");
        //        lStringBuilder.AppendLine("          <p>Os clientes abaixo possuem pend&ecirc;ncias cadastrais.</p>");
        //        lStringBuilder.AppendLine("          <br />");
        //        lStringBuilder.AppendLine("          <ul style=\"font-size:12px;\">");
        //        lStringBuilder.AppendLine("{1}");
        //        lStringBuilder.AppendLine("          </ul>");
        //        lStringBuilder.AppendLine("          <br />");
        //        lStringBuilder.AppendLine("          <p>");
        //        lStringBuilder.AppendLine("               Pedimos que entre em contato com os clientes mensionados para regularizar a situação.");
        //        lStringBuilder.AppendLine("          </p>");
        //        lStringBuilder.AppendLine("     </body>");
        //        lStringBuilder.AppendLine("</html>");

        //        return lStringBuilder.ToString();
        //    }
        //}

        //private List<PendenciaClienteAssessorInfo> GetListaPendenciaClienteAssessor
        //{
        //    get
        //    {
        //        var lListaPendenciaClienteAssessor =
        //            Ativador.Get<IServicoPersistenciaCadastro>().ConsultarEntidadeCadastro<PendenciaClienteAssessorInfo>(
        //                new ConsultarEntidadeCadastroRequest<PendenciaClienteAssessorInfo>());

        //        return lListaPendenciaClienteAssessor.Resultado;
        //    }
        //}

        //#endregion

        //#region | Métodos servico

        //public ReceberObjetoResponse<PendenciaClienteAssessorInfo> PendenciaClienteAssessorEnviarEmail(ReceberEntidadeRequest<PendenciaClienteAssessorInfo> pParametros)
        //{
        //    var lNomeAssessor = string.Empty;
        //    var lEmailAssessor = string.Empty;
        //    var lListaIdsAssessores = new List<int>();
        //    var lListaPendenciasTexto = new StringBuilder();
        //    var lListaPendenciasCliente = this.PendenciaClienteAssessorListar();

        //    lListaPendenciasCliente.ForEach(delegate(PendenciaClienteAssessorInfo e)
        //    {   //--> Separa os id's dos assessores numa lista.
        //        if (!lListaIdsAssessores.Contains(e.IdAssessor))
        //            lListaIdsAssessores.Add(e.IdAssessor);
        //    });

        //    lListaIdsAssessores.ForEach(delegate(int lIdAssessor)
        //    {
        //        lListaPendenciasTexto = new StringBuilder();

        //        lListaPendenciasCliente.FindAll(delegate(PendenciaClienteAssessorInfo e)
        //        {   //--> Separa a lista de clientes por assessor.
        //            return e.IdAssessor == lIdAssessor;
        //        })
        //        .ForEach(delegate(PendenciaClienteAssessorInfo e)
        //        {   //--> Roda a lista com os id's dos assessores.
        //            lNomeAssessor = e.NomeAssessor;
        //            lEmailAssessor = e.EmailAssessor;
        //            this.ComporEmail(e, ref lListaPendenciasTexto);//--> Compõe o corpo do email com os dados do cliente em relação ao assessor.
        //        });

        //        this.EnviarEmail(lNomeAssessor, lEmailAssessor, lListaPendenciasTexto.ToString());
        //    });

        //    return null;
        //}

        //#endregion

        //#region | Métodos de apoio

        //private void ComporEmail(PendenciaClienteAssessorInfo pPendenciaClienteAssessor, ref StringBuilder pListaPendenciasTexto)
        //{
        //    pListaPendenciasTexto.AppendFormat("               <li>Nome Cliente: {0} - Pend&ecirc;ncia: {1}</li>{2}", pPendenciaClienteAssessor.NomeCliente, pPendenciaClienteAssessor.DescricaoPendencia, Environment.NewLine);
        //}

        //public List<PendenciaClienteAssessorInfo> PendenciaClienteAssessorListar()
        //{
        //    var lEmailAssessor = string.Empty;
        //    var lNomeAssessor = string.Empty;
        //    var lListaIdsEncontrados = new List<int>();
        //    var lListaPendenciaClienteAssessor = this.GetListaPendenciaClienteAssessor;

        //    lListaPendenciaClienteAssessor.ForEach(delegate(PendenciaClienteAssessorInfo lPendencia)
        //    {
        //        if (lListaIdsEncontrados.Contains(lPendencia.IdAssessor))
        //        {   //--> Caso já seja conhecido o e-mail do assessor não é pesquisado 
        //            lPendencia.EmailAssessor = lEmailAssessor; // no banco novamente.
        //            lPendencia.NomeAssessor = lNomeAssessor;
        //        }
        //        else //Caro Programador, a estrutura abaixo verifica na base do Sinacor o e-mail do assessor e
        //        {   // o atribui à 'lEmailAssessor' e à propriedade do objeto manipulado.

        //            lEmailAssessor = lPendencia.EmailAssessor = this.GetEmailAssessor(lPendencia.IdAssessor.DBToString());
        //            lNomeAssessor = lPendencia.NomeAssessor = this.GetNomeAssessor(lPendencia.IdAssessor.DBToString());

        //            lListaIdsEncontrados.Add(lPendencia.IdAssessor); //--> Atribui à variável que controla os ids já pesquisados no banco.
        //        }
        //    });

        //    return lListaPendenciaClienteAssessor;
        //}

        //private string GetEmailAssessor(string pIdAssessor)
        //{
        //    var lRetorno =
        //        Ativador.Get<IServicoPersistenciaCadastro>().ConsultarEntidadeCadastro<SinacorListaComboInfo>(
        //            new ConsultarEntidadeCadastroRequest<SinacorListaComboInfo>()
        //            {
        //                EntidadeCadastro = new SinacorListaComboInfo()
        //                {
        //                    Informacao = Contratos.Dados.Enumeradores.eInformacao.EmailAssessor,
        //                    Filtro = pIdAssessor,
        //                }
        //            });

        //    if (null != lRetorno.Resultado && lRetorno.Resultado.Count.CompareTo(0).Equals(1))
        //        return
        //            lRetorno.Resultado[0].Value;

        //    else
        //        return
        //            string.Empty;
        //}

        //private string GetNomeAssessor(string pIdAssessor)
        //{
        //    var lRetorno =
        //        Ativador.Get<IServicoPersistenciaCadastro>().ConsultarEntidadeCadastro<SinacorListaComboInfo>(
        //            new ConsultarEntidadeCadastroRequest<SinacorListaComboInfo>()
        //            {
        //                EntidadeCadastro = new SinacorListaComboInfo()
        //                {
        //                    Informacao = Contratos.Dados.Enumeradores.eInformacao.Assessor,
        //                    Filtro = pIdAssessor,
        //                }
        //            });

        //    if (null != lRetorno.Resultado && lRetorno.Resultado.Count.CompareTo(0).Equals(1))
        //        return
        //            lRetorno.Resultado[0].Value;

        //    else
        //        return
        //            string.Empty;
        //}

        //private void EnviarEmail(string pNomeAssessor, string pEmailAssessor, string pListaPendencias)
        //{
        //    this.gMail = new EmailInfo();

        //    this.gMail.Remetente = "cadastro@gradualinvestimentos.com.br";

        //    this.gMail.Assunto = gEmailAssunto;

        //    this.ConfigurarEmail(pEmailAssessor).ForEach(delegate(string email)
        //        {
        //            this.gMail.Destinatarios.Add(email);
        //        });

        //    // Caro Programador, a instrução abaixo monta o corpo do email com os parâmetros com base na propriedade EsqueledoDoEmail.
        //    this.gMail.CorpoMensagem = string.Format(this.CorpoDoEmail, pNomeAssessor, pListaPendencias);

        //    new ServicoEmailRnLib(this.gMail).Enviar();
        //}

        //private List<string> ConfigurarEmail(string pParametros)
        //{  // Caro Programador, este método Corrige problema do 'System.Net.Mail'
        //   // que envia e-mail apenas no envio de e-mail para grupo do Exchange.

        //    if (string.IsNullOrEmpty(pParametros))
        //        return new List<string>();

        //    var lRetorno = new List<string>();

        //    pParametros.Replace(",", ";");

        //    foreach (var item in pParametros.Split(';'))
        //    {
        //        if (item.Contains("@"))
        //            lRetorno.Add(item);
        //        else
        //            lRetorno.Add(string.Format("{0}@gradualinvestimentos.com.br", item));
        //    }

        //    return lRetorno;
        //}

        //#endregion
    }
}
