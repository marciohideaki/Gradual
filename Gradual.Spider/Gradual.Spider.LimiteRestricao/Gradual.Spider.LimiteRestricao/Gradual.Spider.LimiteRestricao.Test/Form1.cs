using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Spider.LimiteRestricao.Lib;
using Gradual.Spider.LimiteRestricao.Lib.Mensagens;
using Gradual.Spider.LimiteRestricao.Lib.Dados;
using Gradual.Spider.LimiteRestricao.Test.App_Codigo;
using Gradual.Spider.LimiteRestricao.DbLib;

namespace Gradual.Spider.LimiteRestricao.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool ValidaCodigoCliente()
        {
            bool lRetorno = false;

            if (txtCodigoCliente.Text == "" || txtCodigoCliente.Text.Length > 2)
            {
                lRetorno = false;
            }
            else
            {
                lRetorno = true;
            }

            return lRetorno;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var pRequest = new RiscoSelecionarPlataformaClienteRequest();

            pRequest.Objeto = new RiscoPlataformaClienteInfo();

            pRequest.Objeto.CodigoCliente = 31940;

            var lServico = new RiscoPlataformaDbLib();

            var lResponse = lServico.SelecionarPlataformaClienteSpider(pRequest);
            /*
            try
            {
                if (this.ValidaCodigoCliente())
                {
                    MessageBox.Show("Código cliente ","Codigo de Cliente");
                    return;
                }

                int lCodigoCliente = int.Parse(txtCodigoCliente.Text);
                
                var lRequest = new RiscoListarParametrosClienteRequest();
                lRequest.CodigoCliente = lCodigoCliente;
                
                var lServico = new ServicoSpiderLimiteRestricao();
                
                var lResponseLimite = lServico.ListarLimitePorClienteSpider(lRequest);

                //var lReponseAlocado = lServico.




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, "Erro encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
             * */
            //var lRequest = new RiscoSelecionarPlataformaContaMasterRequest();
            //lRequest.Objeto.CodigoContaMaster = 43871;

            //var lResponse = new RiscoPlataformaDbLib().SelecionarPlataformaContaMasterSpider(lRequest);
        }

        private void SalvarIncluirRenovarParametroRisco(TransporteLimiteBovespa pParametro)
        {
            try
            { 
                if (this.ValidaCodigoCliente())
                {
                    MessageBox.Show("Código cliente ", "Codigo de Cliente");
                    return;
                }

                int lCodigoCliente = int.Parse(txtCodigoCliente.Text);

                var lServicoRegrasRisco = new ServicoSpiderLimiteRestricao();

                var lParametro = new RiscoParametroInfo()
                {
                    CodigoParametro = pParametro.IdParametroLimiteDescobertoMercadoAVista,
                    NomeParametro = "LimiteDescobertoMercadoAVista"
                };

                lServicoRegrasRisco.SalvarParametroRiscoClienteSpider(
                    new RiscoSalvarParametroClienteRequest()
                    {
                        ParametroRiscoCliente = new RiscoParametroClienteInfo()
                        {
                            CodigoCliente = pParametro.CodBovespa,
                            Parametro     = lParametro,
                            Valor         = pParametro.LimiteAVistaDescoberto,
                            DataValidade  = Convert.ToDateTime(pParametro.VencimentoAVistaDescoberto),
                        }
                    });
                
                lParametro = new RiscoParametroInfo()
                {
                    CodigoParametro = pParametro.IdParametroLimiteCompraMercadoAVista,
                    NomeParametro = "LimiteCompraMercadoAVista"
                };

                lServicoRegrasRisco.SalvarParametroRiscoClienteSpider(
                    new RiscoSalvarParametroClienteRequest()
                    {
                        ParametroRiscoCliente = new RiscoParametroClienteInfo()
                        {
                            CodigoCliente = pParametro.CodBovespa,
                            Parametro     = lParametro,
                            Valor         = pParametro.LimiteAVista,
                            DataValidade  = Convert.ToDateTime(pParametro.VencimentoAVista),
                        }
                    });
                
                
                    lParametro = new RiscoParametroInfo()
                    {
                        CodigoParametro = pParametro.IdParametroLimiteDescobertoMercadoOpcoes,
                        NomeParametro = "LimiteDescobertoMercadoOpcoes"
                    };

                    lServicoRegrasRisco.SalvarParametroRiscoClienteSpider(
                        new RiscoSalvarParametroClienteRequest()
                        {
                            ParametroRiscoCliente = new RiscoParametroClienteInfo()
                            {
                                CodigoCliente = pParametro.CodBovespa,
                                Parametro     = lParametro,
                                Valor         = pParametro.LimiteOpcaoDescoberto,
                                DataValidade  = Convert.ToDateTime(pParametro.VencimentoOpcaoDescoberto),
                            }
                        });
                
                
                    lParametro = new RiscoParametroInfo()
                    {
                        CodigoParametro = pParametro.IdParametroLimiteCompraMercadoOpcoes,
                        NomeParametro = "LimiteCompraMercadoOpcoes"
                    };

                    var lRetornoSalvarParametroClienteRisco = lServicoRegrasRisco.SalvarParametroRiscoClienteSpider(
                        new RiscoSalvarParametroClienteRequest()
                        {
                            ParametroRiscoCliente = new RiscoParametroClienteInfo()
                            {
                                CodigoCliente = pParametro.CodBovespa,
                                Parametro     = lParametro,
                                Valor         = pParametro.LimiteOpcao,
                                DataValidade  = Convert.ToDateTime(pParametro.VencimentoOpcao),
                            }
                        });
                
                
                    lParametro = new RiscoParametroInfo()
                    {
                        CodigoParametro = pParametro.IdParametroLimiteMaximoDaOrdem,
                        NomeParametro = "LimiteMaximoDaOrdem"
                    };

                    lServicoRegrasRisco.SalvarParametroRiscoClienteSpider(
                        new RiscoSalvarParametroClienteRequest()
                        {
                            ParametroRiscoCliente = new RiscoParametroClienteInfo()
                            {
                                CodigoCliente = pParametro.CodBovespa,
                                Parametro     = lParametro,
                                Valor         = pParametro.ValorMaximoDaOrdem,
                                DataValidade  = Convert.ToDateTime(pParametro.VencimentoMaximoDaOrdem),
                            }
                        });

                    lParametro = new RiscoParametroInfo()
                    {
                        CodigoParametro = pParametro.IdParametroLimitePerdaMaximaOpcao,
                        NomeParametro = "LimitePerdaMaximaOpcao"
                    };

                    lServicoRegrasRisco.SalvarParametroRiscoClienteSpider(
                        new RiscoSalvarParametroClienteRequest()
                        {
                            ParametroRiscoCliente = new RiscoParametroClienteInfo()
                            {
                                CodigoCliente = pParametro.CodBovespa,
                                Parametro     = lParametro,
                                Valor         = pParametro.LimitePerdaMaximaOpcao,
                                DataValidade  = Convert.ToDateTime(pParametro.VencimentoPerdaMaximaOpcao),
                            }
                        });

                    lParametro = new RiscoParametroInfo()
                    {
                        CodigoParametro = pParametro.IdParametroLimitePerdaMaximaOpcao,
                        NomeParametro   = "LimitePerdaMaximaVista"
                    };

                    lServicoRegrasRisco.SalvarParametroRiscoClienteSpider(
                        new RiscoSalvarParametroClienteRequest()
                        {
                            ParametroRiscoCliente = new RiscoParametroClienteInfo()
                            {
                                CodigoCliente = pParametro.CodBovespa,
                                Parametro     = lParametro,
                                Valor         = pParametro.LimitePerdaMaximaVista,
                                DataValidade  = Convert.ToDateTime(pParametro.VencimentoPerdaMaximaVista),
                            }
                        });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, "Erro encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalvarLimiteBovVista_Click(object sender, EventArgs e)
        {
            var lTrans = this.ConverteTransporte();

            this.SalvarIncluirRenovarParametroRisco(lTrans);
        }



        #region Metodos Auxiliares
        private TransporteLimiteBovespa ConverteTransporte()
        {
            var lTrans = new TransporteLimiteBovespa();

            lTrans.IdParametroLimiteDescobertoMercadoAVista = int.Parse(this.lblIdParametroVendaVista.Text);
            lTrans.IdParametroLimiteCompraMercadoOpcoes = int.Parse(this.lblIdParametroCompraOpcao.Text);
            lTrans.IdParametroLimiteDescobertoMercadoOpcoes = int.Parse(this.lblIdParametroVendaOpcao.Text);
            lTrans.IdParametroLimiteMaximoDaOrdem = int.Parse(this.lblIdParametroLimiteMaximoOrdem.Text);
            lTrans.IdParametroLimitePerdaMaximaOpcao = int.Parse(this.lblIdParametroPerdaMaximaOpcao.Text);
            lTrans.IdParametroLimitePerdaMaximaVista = int.Parse(this.lblIdParametroPerdaMaximaVista.Text);

            lTrans.LimiteAVista = decimal.Parse(this.txtCompraVista.Text);
            lTrans.LimiteAVistaDescoberto = decimal.Parse(this.txtVendaVista.Text);
            lTrans.LimiteOpcao = decimal.Parse(this.txtCompraOpcao.Text);
            lTrans.LimiteOpcaoDescoberto = decimal.Parse(this.txtVendaVista.Text);
            lTrans.LimitePerdaMaximaOpcao = decimal.Parse(this.txtPerdaMaxOpcao.Text);
            lTrans.LimitePerdaMaximaVista = decimal.Parse(this.txtPerdaMaxVista.Text);

            lTrans.ValorMaximoDaOrdem = 0;
            lTrans.VencimentoAVista = dtpVencimentoVista.Text;
            lTrans.VencimentoAVistaDescoberto = dtpVencimentoVista.Text;
            lTrans.VencimentoMaximoDaOrdem = dtpVencimentoVista.Text;
            lTrans.VencimentoOpcao = dtpVencimentoOpcao.Text;
            lTrans.VencimentoOpcaoDescoberto = dtpVencimentoOpcao.Text;
            lTrans.VencimentoPerdaMaximaOpcao = dtpVencimentoOpcao.Text;
            lTrans.VencimentoPerdaMaximaVista = dtpVencimentoVista.Text;

            return lTrans;
        }
        #endregion
    }
}
