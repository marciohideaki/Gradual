using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using Gradual.OMS.Library.Servicos;
using Gradual.SaldoDevedor.lib;
using Gradual.SaldoDevedor.lib.Info;
using Gradual.SaldoDevedor.lib.Mensagens;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Gradual.SaldoDevedor.WinApp
{
    public partial class frmNotificarAssessores : GradualForm.GradualForm
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IFormatProvider culture = new System.Globalization.CultureInfo("pt-BR", true);

        private Dictionary<int, EmailInfo> ListaEmail;
        private List<InformacoesClienteInfo> ListaDevedoresFiltrada;

        public frmNotificarAssessores(List<InformacoesClienteInfo> lista)
        {
            InitializeComponent();
            GradualForm.StyleSettings.CarregarSkin("Gradual.Consulta");
            GradualForm.Engine.ConfigureFormRender(this);

            ListaDevedoresFiltrada = lista;

            MontarLista();
        }

        private int AssessoresVinculados
        {
            get
            {
                if (ConfigurationManager.AppSettings["AssessoresVinculados"] == null)
                    return 250;
                return Convert.ToInt32(ConfigurationManager.AppSettings["AssessoresVinculados"]);
            }
        }

        private int IdEmailVinculados
        {
            get
            {
                if (ConfigurationManager.AppSettings["IdEmailVinculados"] == null)
                    return 1;
                return Convert.ToInt32(ConfigurationManager.AppSettings["IdEmailVinculados"]);
            }
        }

        private string EmailTestePara
        {
            get
            {
                if (ConfigurationManager.AppSettings["EmailTestePara"] == null)
                    return "";
                return ConfigurationManager.AppSettings["EmailTestePara"];
            }
        }

        private string EmailTesteCopia
        {
            get
            {
                if (ConfigurationManager.AppSettings["EmailTesteCopia"] == null)
                    return "";
                return ConfigurationManager.AppSettings["EmailTesteCopia"];
            }
        }

        private string EmailTesteCopiaOculta
        {
            get
            {
                if (ConfigurationManager.AppSettings["EmailTesteCopiaOculta"] == null)
                    return "";
                return ConfigurationManager.AppSettings["EmailTesteCopiaOculta"];
            }
        }

        private string AssessoresNaoNotificados
        {
            get
            {
                if (ConfigurationManager.AppSettings["AssessoresNaoNotificados"] == null)
                    return "251;253";
                return ConfigurationManager.AppSettings["AssessoresNaoNotificados"];
            }
        }

        private void MontarLista()
        {
            try
            {
                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                EmailResponse dadosEmail = serv.ObterListaEmail();
                ListaEmail = dadosEmail.Lista;

                List<int> listaAssessoresFiltrada = new List<int>();
                foreach (InformacoesClienteInfo item in ListaDevedoresFiltrada)
                    if (!listaAssessoresFiltrada.Contains(item.CodigoAssessor))
                        listaAssessoresFiltrada.Add(item.CodigoAssessor);

                treeEmail.Nodes.Clear();
                if (listaAssessoresFiltrada.Contains(AssessoresVinculados))
                {
                    listaAssessoresFiltrada.Remove(AssessoresVinculados);

                    Dictionary<string, string> ListaEmailOutlook = ObterListaContatosOutlook();

                    foreach (InformacoesClienteInfo vinculado in ListaDevedoresFiltrada)
                    {
                        if (vinculado.CodigoAssessor.Equals(AssessoresVinculados))
                        {
                            EmailResponse dadosEmailVinculados = serv.ObterDetalheEmail(IdEmailVinculados);
                            EmailInfo emailVinculado = dadosEmailVinculados.Lista[IdEmailVinculados];
                            if (!ListaEmail.ContainsKey(IdEmailVinculados))
                                ListaEmail.Add(IdEmailVinculados, emailVinculado);

                            string emailCliente = "";
                            foreach (string item in ListaEmailOutlook.Keys)
                            {
                                if (LevenshteinDistance(item, vinculado.NomeCliente) < 5)
                                {
                                    emailCliente = ListaEmailOutlook[item];
                                    break;
                                }
                            }
                            
                            string nodeVinculado = "-" + vinculado.CodigoCliente.ToString();
                            treeEmail.Nodes.Add(nodeVinculado, "Notificar " + emailVinculado.Descricao + " (" + emailCliente + ")");
                            treeEmail.Nodes[nodeVinculado].Checked = true;
                            treeEmail.Nodes[nodeVinculado].ForeColor = Color.White;

                            treeEmail.Nodes[nodeVinculado].Nodes.Add(
                                vinculado.CodigoCliente.ToString(), vinculado.CodigoCliente + " (" + vinculado.NomeCliente + ")");
                            treeEmail.Nodes[nodeVinculado].Nodes[vinculado.CodigoCliente.ToString()].Checked = true;
                        }
                    }
                }

                List<int> listaAssessoresCadastrados = new List<int>();

                foreach (KeyValuePair<int, EmailInfo> item in ListaEmail)
                {
                    if (item.Key == IdEmailVinculados)
                        continue;

                    foreach (int assessor in item.Value.ListaAssessor)
                        if (!listaAssessoresCadastrados.Contains(assessor))
                            listaAssessoresCadastrados.Add(assessor);

                    List<int> compararAssessores = listaAssessoresFiltrada.Intersect(item.Value.ListaAssessor).ToList();

                    if (compararAssessores.Count > 0)
                    {
                        treeEmail.Nodes.Add(item.Value.Id.ToString(), "Notificar " + item.Value.Descricao + " (" + item.Value.Titulo + ")");
                        treeEmail.Nodes[item.Value.Id.ToString()].Checked = true;
                        treeEmail.Nodes[item.Value.Id.ToString()].ForeColor = Color.White;

                        foreach (int assessor in item.Value.ListaAssessor)
                        {
                            foreach (InformacoesClienteInfo devedor in ListaDevedoresFiltrada)
                            {
                                if (devedor.CodigoAssessor == assessor)
                                {
                                    treeEmail.Nodes[item.Value.Id.ToString()].Nodes.Add(
                                        devedor.CodigoCliente.ToString(), devedor.CodigoCliente + " (" + devedor.NomeCliente + ")");
                                    treeEmail.Nodes[item.Value.Id.ToString()].Nodes[devedor.CodigoCliente.ToString()].Checked = true;
                                }
                            }
                        }
                    }
                }
                treeEmail.ExpandAll();
                treeEmail.AfterCheck += new TreeViewEventHandler(treeEmail_AfterCheck);

                // Avisa ao usuário se existirem assessores que não recebem notificação
                List<int> listaAssessoresNaoCadastrados = listaAssessoresFiltrada.Except(listaAssessoresCadastrados).ToList();

                List<string> ListaAssessoresNaoNotificados = AssessoresNaoNotificados.Split(';').ToList();
                foreach (string assessorNaoNotificado in ListaAssessoresNaoNotificados)
                {
                    if (listaAssessoresNaoCadastrados.Contains(Convert.ToInt32(assessorNaoNotificado)))
                        listaAssessoresNaoCadastrados.Remove(Convert.ToInt32(assessorNaoNotificado));
                }

                if (listaAssessoresNaoCadastrados.Count > 0)
                {
                    string msg = "";
                    foreach (int item in listaAssessoresNaoCadastrados)
                    {
                        msg += "Assessor " + item + " não cadastrado para receber notificação! \n";
                    }
                    MessageBox.Show(msg, "Notificar Assessores", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                string msg = "Montar Lista: " + ex.Message;
                MessageBox.Show(msg, "Notificar Assessores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void treeEmail_AfterCheck(object sender, TreeViewEventArgs e)
        {
            treeEmail.AfterCheck -= new TreeViewEventHandler(treeEmail_AfterCheck);

            TreeNode node = e.Node;
            if (node.Level == 0)
            {
                foreach (TreeNode clienteAssessor in node.Nodes)
                    clienteAssessor.Checked = node.Checked;
            }
            else if (node.Level == 1)
            {
                if (node.Parent.Checked != node.Checked && node.Checked)
                    node.Parent.Checked = node.Checked;
            }
            treeEmail.AfterCheck += new TreeViewEventHandler(treeEmail_AfterCheck);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                EnviarEmailRequest request = new EnviarEmailRequest();

                Dictionary<int, InformacoesClienteInfo> lista = new Dictionary<int, InformacoesClienteInfo>();
                foreach (InformacoesClienteInfo item in ListaDevedoresFiltrada)
                    lista.Add(item.CodigoCliente, item);

                int emailEnviados = 0;
                int emailNaoEnviados = 0;

                foreach (TreeNode notificacao in treeEmail.Nodes)
                {
                    if (notificacao.Checked)
                    {
                        if (notificacao.Name.StartsWith("-"))
                        {
                            request.DadosEmail = new EmailInfo();
                            request.DadosEmail.Anexos = ListaEmail[IdEmailVinculados].Anexos;
                            request.DadosEmail.Conteudo = ListaEmail[IdEmailVinculados].Conteudo;
                            request.DadosEmail.DataAtualizacao = ListaEmail[IdEmailVinculados].DataAtualizacao;
                            request.DadosEmail.Descricao = ListaEmail[IdEmailVinculados].Descricao;
                            request.DadosEmail.EmailCopia = ListaEmail[IdEmailVinculados].EmailCopia;
                            request.DadosEmail.EmailCopiaOculta = ListaEmail[IdEmailVinculados].EmailCopiaOculta;
                            request.DadosEmail.Id = ListaEmail[IdEmailVinculados].Id;
                            request.DadosEmail.IdTextoEmail = ListaEmail[IdEmailVinculados].IdTextoEmail;
                            request.DadosEmail.ListaAssessor = ListaEmail[IdEmailVinculados].ListaAssessor;
                            request.DadosEmail.Titulo = new StringBuilder(ListaEmail[IdEmailVinculados].Titulo).ToString();
                            request.DadosEmail.Assunto = new StringBuilder(ListaEmail[IdEmailVinculados].Assunto).ToString();
                            request.DadosEmail.EmailPara = new StringBuilder(ListaEmail[IdEmailVinculados].EmailPara).ToString();
                            int codCliente = Int32.Parse(notificacao.Name.Substring(1, notificacao.Name.Length - 1));

                            request.ListaClientes = new List<InformacoesClienteInfo>();
                            if (notificacao.Nodes[0].Checked)
                            {
                                request.ListaClientes.Add(lista[codCliente]);
                                request.DadosEmail.Titulo = lista[codCliente].NomeCliente;
                                request.DadosEmail.Assunto += lista[codCliente].NomeCliente;
                                request.DadosEmail.EmailPara = notificacao.Text.Substring(notificacao.Text.IndexOf('(') + 1).Replace(')', ' ').Trim();

                                if (request.DadosEmail.EmailPara.Length == 0)
                                {
                                    string msg = string.Format("Favor preencher e-mail do Cliente Vinculado ({0})", codCliente);
                                    MessageBox.Show(msg, "Notificar Assessores", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }

                            if (request.ListaClientes.Count > 0)
                            {
                                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                                if (!EmailTestePara.Equals(""))
                                    request.DadosEmail.EmailPara = EmailTestePara;
                                if (!EmailTesteCopia.Equals(""))
                                    request.DadosEmail.EmailCopia = EmailTesteCopia;
                                if (!EmailTesteCopiaOculta.Equals(""))
                                    request.DadosEmail.EmailCopiaOculta = EmailTesteCopiaOculta;

                                EnviarEmailResponse resp = serv.EnviarEmailAssessores(request);
                                if (resp.Retorno == EnviarEmailResponse.RETORNO_OK)
                                    emailEnviados++;
                                else
                                    emailNaoEnviados++;
                            }


                        }
                        else
                        {
                            request.DadosEmail = ListaEmail[Int32.Parse(notificacao.Name)];

                            request.ListaClientes = new List<InformacoesClienteInfo>();
                            foreach (TreeNode clienteNotificado in notificacao.Nodes)
                            {
                                if (clienteNotificado.Checked)
                                {
                                    request.ListaClientes.Add(lista[Int32.Parse(clienteNotificado.Name)]);
                                }
                            }

                            if (request.ListaClientes.Count > 0)
                            {
                                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                                if (!EmailTestePara.Equals(""))
                                    request.DadosEmail.EmailPara = EmailTestePara;
                                if (!EmailTesteCopia.Equals(""))
                                    request.DadosEmail.EmailCopia = EmailTesteCopia;
                                if (!EmailTesteCopiaOculta.Equals(""))
                                    request.DadosEmail.EmailCopiaOculta = EmailTesteCopiaOculta;

                                EnviarEmailResponse resp = serv.EnviarEmailAssessores(request);
                                if (resp.Retorno == EnviarEmailResponse.RETORNO_OK)
                                    emailEnviados++;
                                else
                                    emailNaoEnviados++;
                            }
                        }
                    }
                }

                if (emailEnviados > 0)
                {
                    string msg = string.Format("{0} Notificações enviadas com sucesso!", emailEnviados.ToString());
                    MessageBox.Show(msg, "Notificar Assessores", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (emailNaoEnviados > 0)
                {
                    string msg = string.Format("{0} FALHA NO ENVIO DAS NOTIFICAÇÕES!", emailNaoEnviados.ToString());
                    MessageBox.Show(msg, "Notificar Assessores", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                string msg = "Enviar E-mail: " + ex.Message;
                MessageBox.Show(msg, "Notificar Assessores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Dictionary<string, string> ObterListaContatosOutlook()
        {
            Dictionary<string, string> lista = new Dictionary<string, string>();
            try
            {
                Outlook.Application outlook = new Outlook.Application();
                Outlook.AddressList contatos = outlook.Session.GetGlobalAddressList();

                lista.Clear();
                foreach (Outlook.AddressEntry item in contatos.AddressEntries)
                {
                    Outlook.ExchangeUser contato = item.GetExchangeUser();
                    if (contato != null)
                        lista.Add(contato.Name.ToUpper(), contato.PrimarySmtpAddress);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Obter Lista Contatos Outlook: " + ex.Message);
            }
            return lista;
        }

        int LevenshteinDistance(string s, string t)
        {
            if (s == t) return 0;
            if (s.Length == 0) return t.Length;
            if (t.Length == 0) return s.Length;

            int[] v0 = new int[t.Length + 1];
            int[] v1 = new int[t.Length + 1];

            for (int i = 0; i < v0.Length; i++)
                v0[i] = i;

            for (int i = 0; i < s.Length; i++)
            {
                v1[0] = i + 1;

                for (int j = 0; j < t.Length; j++)
                {
                    var cost = (s[i] == t[j]) ? 0 : 1;
                    v1[j + 1] = Math.Min(v1[j] + 1, Math.Min(v0[j + 1] + 1, v0[j] + cost));
                }

                // copy v1 (current row) to v0 (previous row) for next iteration
                for (int j = 0; j < v0.Length; j++)
                    v0[j] = v1[j];
            }

            return v1[t.Length];
        }

        private void treeEmail_Click(object sender, EventArgs e)
        {
            if (nodeEditado != null && nodeEditado.Parent != null)
            {
                treeEmail.SelectedNode = nodeEditado;

                treeEmail.LabelEdit = true;
                if (!nodeEditado.IsEditing)
                    nodeEditado.BeginEdit();
            }

        }

        private TreeNode nodeEditado;

        private void treeEmail_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode node = treeEmail.GetNodeAt(e.X, e.Y);
            if (node.Name.StartsWith("-"))
                nodeEditado = node;
        }

        private void treeEmail_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Text != null)
            {
                string checarTexto = "Notificar " + ListaEmail[IdEmailVinculados].Descricao + " (";
                if (e.Node.Text.Length > 0 && e.Node.Text.Contains(checarTexto))
                {
                    e.Node.EndEdit(false);
                    return;
                }
            }
            e.CancelEdit = true;
            e.Node.BeginEdit();
        }
    }
}
