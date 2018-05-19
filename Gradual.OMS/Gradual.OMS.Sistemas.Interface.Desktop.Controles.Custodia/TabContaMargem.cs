using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Seguranca.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Dados;
using Gradual.OMS.Contratos.Integracao.Sinacor.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Integracao.Sinacor.Dados;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Custodia
{
    /// <summary>
    /// Controle de conta margem
    /// </summary>
    public partial class TabContaMargem : XtraUserControl, IControle
    {
        /// <summary>
        /// Contexto da interface
        /// </summary>
        private InterfaceContextoOMS _contexto = null;

        public TabContaMargem()
        {
            InitializeComponent();
        }

        private InterfaceContextoOMS receberInterfaceContextoOMS()
        {
            // Verifica se o contexto está carregado
            if (_contexto == null)
            {
                // Pega o contexto
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();
            }

            // Retorna
            return _contexto;
        }

        #region IControle Members

        public void Inicializar(Controle controle)
        {
        }

        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            return null;
        }

        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            // Pega o tipo da mensagem
            Type tipoMensagem = parametros.GetType();

            // De acordo com o tipo da mensagem
            if (tipoMensagem == typeof(SinalizarInicializarUsuarioRequest)  && false)
            {
                // Pega a mensagem com o tipo correto
                SinalizarInicializarUsuarioRequest parametros2 = (SinalizarInicializarUsuarioRequest)parametros;

                // Pega o contexto do usuário
                ContextoOMSInfo contextoOMS = parametros2.Usuario.Complementos.ReceberItem<ContextoOMSInfo>();
                if (contextoOMS != null && contextoOMS.CodigoCBLC != null)
                {
                    // Pede os saldos de conta margem
                    ReceberSaldoContaMargemSinacorResponse contaMargem =
                        Ativador.Get<IServicoIntegracaoSinacor>().ReceberSaldoContaMargemSinacor(
                            new ReceberSaldoContaMargemSinacorRequest()
                            {
                                CodigoSessao = receberInterfaceContextoOMS().SessaoInfo.CodigoSessao,
                                CodigoClienteCBLC = contextoOMS.CodigoCBLC,
                                RetornarHistorico = true
                            });

                    // Atribui ao gráfico
                    chartControl.Series[0].Points.Clear();
                    chartControl.Series[1].Points.Clear();
                    chartControl.Series[2].Points.Clear();
                    chartControl.Series[3].Points.Clear();
                    foreach (SaldoContaMargemSinacorInfo saldoContaMargem in contaMargem.SaldosContaMargemSinacor)
                    {
                        chartControl.Series[0].Points.Add(new DevExpress.XtraCharts.SeriesPoint(saldoContaMargem.DataReferencia, new double[] { saldoContaMargem.ValorLimite }));
                        chartControl.Series[1].Points.Add(new DevExpress.XtraCharts.SeriesPoint(saldoContaMargem.DataReferencia, new double[] { saldoContaMargem.ValorDepositoContaCorrente }));
                        chartControl.Series[2].Points.Add(new DevExpress.XtraCharts.SeriesPoint(saldoContaMargem.DataReferencia, new double[] { saldoContaMargem.ValorFinanciado }));
                        chartControl.Series[3].Points.Add(new DevExpress.XtraCharts.SeriesPoint(saldoContaMargem.DataReferencia, new double[] { saldoContaMargem.ValorLimite + saldoContaMargem.ValorDepositoContaCorrente + saldoContaMargem.ValorFinanciado }));
                    }
                }
            }

            // Retorna
            return null;
        }

        #endregion
    }
}
