#region Includes
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.PlanoCliente.Lib;
using Gradual.OMS.PlanoCliente;
#endregion

namespace Gradual.OMS.PlanoCliente.Test
{
    public partial class Form1 : Form
    {
        #region Constructors
        public Form1()
        {
            InitializeComponent();
            btnListar_Click(null, null);
        }
        #endregion

        private void btnListar_Click(object sender, EventArgs e)
        {
            this.Teste1();

            //ServicoPlanoCliente lServico = new ServicoPlanoCliente();

            //var lLstPlanoCliente = new List<PlanoClienteInfo>();

            //lLstPlanoCliente.Add(new PlanoClienteInfo()
            //{
            //    DsCpfCnpj = "99231601903",
            //    CdCblc = 45121,
            //    StSituacao = 'A',
            //    IdProdutoPlano = 3,
            //    DtOperacao = DateTime.Now,
            //    DtAdesao = DateTime.Now
            //});

            //var lRetorno = lServico.InserirPlanoClienteExistente(new InserirProdutosClienteRequest()
            //{
            //    LstPlanoCliente = lLstPlanoCliente,
            //});

            //if (lRetorno.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            //{

            //}

            //ListarProdutosClienteRequest lRequest = new ListarProdutosClienteRequest();

            //lRequest.DsCpfCnpj = "61047311208";

            //ListarProdutosClienteResponse lResponse = lServico.ListarProdutosCliente(lRequest);


            //if (lResponse.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            //{
            //    MessageBox.Show("Dados salvos com sucesso!!!");
            //}
            //else
            //{
            //    MessageBox.Show(lResponse.DescricaoResposta);
            //}
        }

        private void Teste1()
        {
            new Gradual.OMS.PlanoCliente.ServicoPlanoCliente().ListarProdutos();
        }
    }
}
