using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.ConectorSTM;
using Gradual.OMS.ConectorSTM.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.ConectorSTM.Lib.Mensagens;
using Gradual.OMS.ConectorSTM.Eventos;
using System.IO;
using log4net;
using System.Runtime.Serialization.Formatters.Binary;

namespace AppTesteServicosSTM
{
    public partial class Form1 : Form, IServicoSTMCallback
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string ultimoMsgId = " ";

        public Form1()
        {
            log4net.Config.XmlConfigurator.Configure();

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            IServicoSTM stmserver = Ativador.Get<IServicoSTM>(this);

            AssinarEventosSTMResponse responset = stmserver.AssinarEventosSTM(new AssinarEventosSTMRequest() );
        }

        public void OnCBLC_ConfirmacaoNegocioMegabolsa(Gradual.OMS.ConectorSTM.Lib.Mensagens.CBLCConfirmacaoNegocioMegaBolsaInfo info)
        {
            CBLCConfirmacaoNegocioMegaBolsaInfo fuck = info;
        }

        public void OnCBLC_ConfirmacaoNegocioBovespaFIX(Gradual.OMS.ConectorSTM.Lib.Mensagens.CBLCConfirmacaoNegocioBovespaFixInfo info)
        {
            CBLCConfirmacaoNegocioBovespaFixInfo fix = info;
        }

        public void OnCBLC_ControleFasesMegabolsa(Gradual.OMS.ConectorSTM.Lib.Mensagens.CBLCControleFasesMegaBolsaInfo info)
        {
            throw new NotImplementedException();
        }

        public void OnMega_CancelamentoNegocio(Gradual.OMS.ConectorSTM.Lib.Mensagens.MEGA0100NotificacaoCancelamentoNegocioInfo info)
        {
            MEGA0100NotificacaoCancelamentoNegocioInfo xxxx = info;
        }

        public void OnMega_CriacaoNegocio(Gradual.OMS.ConectorSTM.Lib.Mensagens.MEGA0103CriacaoNegocioInfo info)
        {
            MEGA0103CriacaoNegocioInfo xxxx = info;
        }

        public void OnMega_NotificacaoExecucao(Gradual.OMS.ConectorSTM.Lib.Mensagens.MEGA0105NotificacaoExecucaoInfo info)
        {
            MEGA0105NotificacaoExecucaoInfo xxx = info;
        }

        public void OnMega_OrdemEliminada(Gradual.OMS.ConectorSTM.Lib.Mensagens.MEGA0138OrdemEliminadaInfo info)
        {
            MEGA0138OrdemEliminadaInfo eliminada = info;
        }

        public void OnMega_ConfirmacaoOrdem(Gradual.OMS.ConectorSTM.Lib.Mensagens.MEGA0172ConfirmacaoOrdemInfo info)
        {
            MEGA0172ConfirmacaoOrdemInfo xxx = info;
        }

        public void OnMega_DeclaracaoTermo(Gradual.OMS.ConectorSTM.Lib.Mensagens.MEGA0411DeclaracaoTermoInfo info)
        {
            throw new NotImplementedException();
        }

        public void OnMega_NotificacaoDeclaracaoTermo(Gradual.OMS.ConectorSTM.Lib.Mensagens.MEGA0412NotificacaoDeclaracaoTermoInfo info)
        {
            throw new NotImplementedException();
        }

        public void OnMega_NotificacaoCancelamentoTermo(Gradual.OMS.ConectorSTM.Lib.Mensagens.MEGA0413NotificacaoCancelamentoTermoInfo info)
        {
            throw new NotImplementedException();
        }

        public void OnMega_NotificacaoRejeicaoTermo(Gradual.OMS.ConectorSTM.Lib.Mensagens.MEGA0414NotificacaoRejeicaoTermoInfo info)
        {
            throw new NotImplementedException();
        }

        public void OnMega_NotificacaoExecucaoTermo(Gradual.OMS.ConectorSTM.Lib.Mensagens.MEGA0415NotificacaoExecucaoTermoInfo info)
        {
            throw new NotImplementedException();
        }

        public void OnMega_NotificacaoCancelamentoNegocioTermo(Gradual.OMS.ConectorSTM.Lib.Mensagens.MEGA0417NotificacaoCancelamentoNegocioTermoInfo info)
        {
            throw new NotImplementedException();
        }

        public void OnHeartBeat()
        {
            //MessageBox.Show("Chegou HeartBeat()");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string xxx = "2011-06-060005409801                        0000                      000000000000000 000000000000000000            00000000000000000000000000100847000000000000000000000000000000000000000000000000000000000000000000000000000000000001900-01-010000000000000000000000000000000000000000000000000000000000000                                           00010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000                                                                              ";

            //EventoSTM evento = new EventoSTM("<AN  >", "<AN  >", xxx);

            //ParserCBLCMessage parser = new ParserCBLCMessage();

            //parser.Parse(evento);

            string Formato = "3";
            string preco = "000062550";

            string retorno = STMUtilities.saidaFormatada(Formato[0], preco, false, true, false, 18);

            Decimal xxx = Convert.ToDecimal(retorno, STMUtilities.ciPtBR);

            ultimoMsgId = "caralho da porra";

            _saveUltimoMsgID();

            ultimoMsgId = "muito burro!!!";

            _loadUltimoMsgID();

            retorno = ultimoMsgId;


        }


        private void _saveUltimoMsgID()
        {
            Stream stream = null;
            //string path = ConfigurationManager.AppSettings["MSGIDFile"].ToString();

            try
            {
                logger.Info("Salvando ultimo MSGID processado: " + this.ultimoMsgId);

                stream = File.Open("sunda.bin", FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();

                bformatter.Serialize(stream, ultimoMsgId);

                stream.Close();
                stream = null;
            }
            catch (Exception ex)
            {
                logger.Error("_saveUltimoMsgID(): " + ex.Message, ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

        }


        private bool _loadUltimoMsgID()
        {
            Stream stream = null;
            //string path = ConfigurationManager.AppSettings["MSGIDFile"].ToString();

            try
            {

                stream = File.Open("sunda.bin", FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                ultimoMsgId = (string) bformatter.Deserialize(stream);

                logger.Info("Carregando ultimo MSGID processado: " + this.ultimoMsgId);

                stream.Close();
                stream = null;

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("_loadUltimoMsgID(): " + ex.Message, ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            ultimoMsgId = " ";
            return false;
        }
    }
}
