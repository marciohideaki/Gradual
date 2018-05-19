using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.Email;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Email.Lib;
using System.IO;

namespace TesteEmail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonIniciarProcesso_Click(object sender, EventArgs e)
        {
            var lDestinatarios = new System.Collections.Generic.List<string>();
                lDestinatarios.Add("bribeiro@gradualinvestimentos.com.br");

            var lEmail = new EmailInfo() 
            {
                Assunto = "Teste de e-mail",
                CorpoMensagem = string.Format("<html><body><p>TESTE DO SERVICO DE EMAIL</p><p>Realizado em: {0}</p></body></html>", DateTime.Today.ToString("dd/MM/yyyy HH:mm:ss")),
                Destinatarios = lDestinatarios,
                Remetente = "arosario@gradualinvestimentos.com.br",
            };

            lEmail.Anexos = new List<EmailAnexoInfo>();

            EmailAnexoInfo Anexo = new EmailAnexoInfo();
            Anexo.Nome = "Teste de Anexo.pdf";
            Stream stream = null;
            //stream = File.OpenRead(@"C:\Dev\Gradual\Gradual.Intranet\Gradual.Intranet.Www\Extras\Contratos\DeclaracaoPPE.pdf");
            
            
            //MemoryStream memStream = new MemoryStream();
            //byte[] respBuffer = new byte[1024];
            //int bytesRead = stream.Read(respBuffer, 0, respBuffer.Length);
            //memStream.Write(respBuffer, 0, bytesRead);
            //bytesRead = stream.Read(respBuffer, 0, respBuffer.Length);
            //Anexo.Arquivo = memStream.GetBuffer();
            //memStream.Close();

            Anexo.Arquivo = File.ReadAllBytes(@"C:\Dev\Gradual\Gradual.Intranet\Gradual.Intranet.Www\Extras\Contratos\DeclaracaoPPE.pdf");
            
            //byte[] buffer = new byte[16 * 1024];
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    int read;
            //    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        ms.Write(buffer, 0, read);
            //    }
            //    Anexo.Arquivo = ms.ToArray();
            //} 
            //stream.Close();


            lEmail.Anexos.Add(Anexo);

            //ServicoHostColecao.Default.CarregarConfig("Default");

            //IServicoEmail _servicoEmail = Ativador.Get<IServicoEmail>();
            Gradual.OMS.Email.ServicoEmail email = new ServicoEmail();

            email.Enviar(new EnviarEmailRequest() { Objeto = lEmail });


           // var lStatusEnvio = _servicoEmail.Enviar(new EnviarEmailRequest() { Objeto = lEmail });

           // this.textBoxStatus.Text = lStatusEnvio.StatusResposta.ToString();
        }
    }
}
