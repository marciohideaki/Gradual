using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Seguranca.Lib;
using Gradual.OMS.Persistencias.Seguranca.Entidades;
using System.Threading;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca;

namespace TesteDeSeguranca
{
    public partial class Form1 : Form
    {
        IServicoSeguranca servicoSeguranca;
        public Form1()
        {
            InitializeComponent();
            ServicoHostColecao.Default.CarregarConfig("Default");
            servicoSeguranca = Ativador.Get<IServicoSeguranca>();

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //AutenticarUsuarioResponse lRes = servicoSeguranca.AutenticarUsuario(new AutenticarUsuarioRequest()
            //{
            //    Email = this.txtEmail.Text,
            //    Senha = Criptografia.CalculateMD5Hash(this.txtSenha.Text),
            //    IP = Environment.MachineName,
            //    CodigoSistemaCliente = "GTI"
            //});

            AutenticarUsuarioResponse lRes = servicoSeguranca.AutenticarUsuario(new AutenticarUsuarioRequest()
            {
                Email = txtEmail.Text,
                Senha = Criptografia.CalculateMD5Hash(this.txtSenha.Text),
                IP = Environment.MachineName
               // CodigoSistemaCliente = "GTI"
            });

            if (lRes.StatusResposta == Gradual.OMS.Library.MensagemResponseStatusEnum.OK)
            {
                MessageBox.Show("Usuário autenticado com sucesso");
                this.lblCodigoSessao.Text = lRes.Sessao.CodigoSessao;

                ReceberSessaoRequest lEntradaSessao = new ReceberSessaoRequest();
                lEntradaSessao.CodigoSessao = lRes.Sessao.CodigoSessao;
                lEntradaSessao.CodigoSessaoARetornar = lRes.Sessao.CodigoSessao;
                ReceberSessaoResponse lRetornoSessao = servicoSeguranca.ReceberSessao(lEntradaSessao);

                this.propertyGrid1.SelectedObject = lRetornoSessao.Usuario;
            }
            else
            {
                MessageBox.Show(lRes.DescricaoResposta);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            servicoSeguranca.EfetuarLogOut(new Gradual.OMS.Library.MensagemRequestBase()
            {
                CodigoSessao = this.lblCodigoSessao.Text
            });

            this.gvResultados.DataSource = null;
            this.lblCodigoSessao.Text = string.Empty;
            this.txtEmail.Text = string.Empty;
            this.txtSenha.Text = string.Empty;
        }

        private void btnListarUsuarios_Click(object sender, EventArgs e)
        {
            ListarUsuariosResponse lRes = servicoSeguranca.ListarUsuarios(new ListarUsuariosRequest()
            {
                CodigoSessao = lblCodigoSessao.Text
            });
            gvResultados.DataSource = lRes.Usuarios;
        }

        private void btnListarPerfis_Click(object sender, EventArgs e)
        {
            ListarUsuariosRequest lRequest = new ListarUsuariosRequest()
            {
                CodigoSessao = this.lblCodigoSessao.Text,
                FiltroCodigoPerfil = "6"
            };
            var res = servicoSeguranca.ListarUsuarios(lRequest);

            this.gvResultados.DataSource = res.Usuarios;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var lPerfis = new List<string>();

            lPerfis.Add("2");

            var lRetorno = servicoSeguranca.SalvarUsuario(new SalvarUsuarioRequest()
            {
                Usuario = new UsuarioInfo()
                    {
                        CodigoAssessor = 99463,
                        CodigosFilhoAssessor = "3636,25023",
                        CodigoTipoAcesso = 2,
                        CodigoUsuario = "212089",
                        Complementos = new Gradual.OMS.Library.ColecaoTipoInstancia() { Colecao = new List<object>() },
                        Email = "brocha@gradualinvestimentos.com.br",
                        Nome = "BIANCA ROCHA",
                        Perfis = lPerfis,
                        Status = UsuarioStatusEnum.Desabilitado,
                    }
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            PerfilInfo lPerfil = (PerfilInfo)this.listBox1.SelectedItem;
            if (!backgroundWorker2.IsBusy)
            {
                if(lPerfil != null)
                    backgroundWorker2.RunWorkerAsync(lPerfil.CodigoPerfil);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ListarPerfisRequest lReq = new ListarPerfisRequest()
            {
                CodigoSessao = this.lblCodigoSessao.Text
            };
            ListarPerfisResponse lRes = servicoSeguranca.ListarPerfis(lReq);
            this.listBox1.Invoke(new MethodInvoker(delegate()
            {
                this.listBox1.DataSource = lRes.Perfis;
            }));
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (this.listBox1)
            {
                ListarUsuariosRequest lReq = new ListarUsuariosRequest()
                {
                    FiltroCodigoPerfil = e.Argument.ToString(),
                    CodigoSessao = this.lblCodigoSessao.Text
                };
                ListarUsuariosResponse lRes = servicoSeguranca.ListarUsuarios(lReq);
                this.gvResultados.Invoke(new MethodInvoker(delegate()
                    {
                        this.gvResultados.DataSource = lRes.Usuarios;
                    }));
            }
        }

        private void btnNovoPerfil_Click(object sender, EventArgs e)
        {
            SalvarPerfilRequest lReq = new SalvarPerfilRequest()
            {
                CodigoSessao = this.lblCodigoSessao.Text,
                Perfil = new PerfilInfo()
                {
                    CodigoPerfil = "0",
                    NomePerfil = this.txtNovoPerfil.Text
                }
            };

            try
            {
                MensagemResponseBase lRes = servicoSeguranca.SalvarPerfil(lReq);
                if (lRes.StatusResposta == MensagemResponseStatusEnum.OK)
                    MessageBox.Show("Perfil criado com sucesso", "Cadastro de Perfil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show(lRes.DescricaoResposta, "Cadastro de Perfil", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cadastro de Perfil", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            ListarAssessorCompartilhadoRequest _request = new ListarAssessorCompartilhadoRequest();
            _request.CodigoAssessor = 403;

            ListarAssessorCompartilhadoResponse Response = servicoSeguranca.ListarAssessorCompartilhado(_request);
                   

        }

        private void button6_Click(object sender, EventArgs e)
        {

            AlterarPermissaoAcessoRequest _request = new AlterarPermissaoAcessoRequest();
            _request.PermissaoAcessoUsuarioInfo.IdUsuario = 91939;
            _request.PermissaoAcessoUsuarioInfo.UsuarioAcessoAcao = UsuarioAcessoEnum.Desbloqueio;

            AlterarPermissaoAcessoResponse Response = servicoSeguranca.AlterarPermissaoAcesso(_request);
                   
        }
    }
}

