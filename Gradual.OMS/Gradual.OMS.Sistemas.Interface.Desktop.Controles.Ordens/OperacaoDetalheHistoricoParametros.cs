using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Ordens
{
    /// <summary>
    /// Classe de parametros do controle OperacaoDetalheHistorico
    /// </summary>
    [Serializable]
    public class OperacaoDetalheHistoricoParametros
    {
        /// <summary>
        /// Mantem a lista de layouts dos controles devexpress
        /// </summary>
        public LayoutsDevExpressHelper LayoutsDevExpress { get; set; }

        /// <summary>
        /// Cores para ExecType em Mensagens
        /// </summary>
        public List<CorEnumeradorInfo<OrdemTipoExecucaoEnum>> CoresMensagemExecType { get; set; }

        /// <summary>
        /// Cores para OrdRejReason em Mensagens
        /// </summary>
        public List<CorEnumeradorInfo<OrdemMotivoRejeicaoEnum>> CoresMensagemOrdRejReason { get; set; }

        /// <summary>
        /// Cores para OrdStatus em Mensagens
        /// </summary>
        public List<CorEnumeradorInfo<OrdemStatusEnum>> CoresMensagemOrdStatus { get; set; }

        /// <summary>
        /// Indica a largura da janela
        /// </summary>
        public int Largura { get; set; }

        /// <summary>
        /// Indica a altura da janela
        /// </summary>
        public int Altura { get; set; }

        /// <summary>
        /// Indica a posição esquerda da janela
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Indica a posição Y da janela
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public OperacaoDetalheHistoricoParametros()
        {
            // Inicializa os objetos e coleções
            this.LayoutsDevExpress = new LayoutsDevExpressHelper();
            this.CoresMensagemExecType = new List<CorEnumeradorInfo<OrdemTipoExecucaoEnum>>();
            this.CoresMensagemOrdRejReason = new List<CorEnumeradorInfo<OrdemMotivoRejeicaoEnum>>();
            this.CoresMensagemOrdStatus = new List<CorEnumeradorInfo<OrdemStatusEnum>>();

            // Cria elementos default
            criarElementosDefault();
        }

        private void criarElementosDefault()
        {
            // -----------------------------------------------------------------
            // ExecType
            // -----------------------------------------------------------------

            this.CoresMensagemExecType.Add(
                new CorEnumeradorInfo<OrdemTipoExecucaoEnum>()
                {
                    Valor = OrdemTipoExecucaoEnum.CancelamentoNegocio,
                    Cor = Color.Red,
                    LinhaInteira = false
                });
            this.CoresMensagemExecType.Add(
                new CorEnumeradorInfo<OrdemTipoExecucaoEnum>()
                {
                    Valor = OrdemTipoExecucaoEnum.CancelamentoOferta,
                    Cor = Color.Red,
                    LinhaInteira = false
                });
            this.CoresMensagemExecType.Add(
                new CorEnumeradorInfo<OrdemTipoExecucaoEnum>()
                {
                    Valor = OrdemTipoExecucaoEnum.CancelamentoPendente,
                    Cor = Color.Red,
                    LinhaInteira = false
                });
            this.CoresMensagemExecType.Add(
                new CorEnumeradorInfo<OrdemTipoExecucaoEnum>()
                {
                    Valor = OrdemTipoExecucaoEnum.NaoImplementado,
                    Cor = Color.Gray,
                    LinhaInteira = false
                });
            this.CoresMensagemExecType.Add(
                new CorEnumeradorInfo<OrdemTipoExecucaoEnum>()
                {
                    Valor = OrdemTipoExecucaoEnum.NaoInformado,
                    Cor = Color.Gray,
                    LinhaInteira = false
                });
            this.CoresMensagemExecType.Add(
                new CorEnumeradorInfo<OrdemTipoExecucaoEnum>()
                {
                    Valor = OrdemTipoExecucaoEnum.Negocio,
                    Cor = Color.LightGreen,
                    LinhaInteira = false
                });
            this.CoresMensagemExecType.Add(
                new CorEnumeradorInfo<OrdemTipoExecucaoEnum>()
                {
                    Valor = OrdemTipoExecucaoEnum.Nova,
                    Cor = Color.LightCyan,
                    LinhaInteira = false
                });
            this.CoresMensagemExecType.Add(
                new CorEnumeradorInfo<OrdemTipoExecucaoEnum>()
                {
                    Valor = OrdemTipoExecucaoEnum.NovaPendente,
                    Cor = Color.LightCyan,
                    LinhaInteira = false
                });
            this.CoresMensagemExecType.Add(
                new CorEnumeradorInfo<OrdemTipoExecucaoEnum>()
                {
                    Valor = OrdemTipoExecucaoEnum.Preenchimento,
                    Cor = Color.LightYellow,
                    LinhaInteira = false
                });
            this.CoresMensagemExecType.Add(
                new CorEnumeradorInfo<OrdemTipoExecucaoEnum>()
                {
                    Valor = OrdemTipoExecucaoEnum.Reconfirmacao,
                    Cor = Color.Orange,
                    LinhaInteira = false
                });
            this.CoresMensagemExecType.Add(
                new CorEnumeradorInfo<OrdemTipoExecucaoEnum>()
                {
                    Valor = OrdemTipoExecucaoEnum.Rejeicao,
                    Cor = Color.Red,
                    LinhaInteira = false
                });
            this.CoresMensagemExecType.Add(
                new CorEnumeradorInfo<OrdemTipoExecucaoEnum>()
                {
                    Valor = OrdemTipoExecucaoEnum.Substituicao,
                    Cor = Color.Orange,
                    LinhaInteira = false
                });
            this.CoresMensagemExecType.Add(
                new CorEnumeradorInfo<OrdemTipoExecucaoEnum>()
                {
                    Valor = OrdemTipoExecucaoEnum.Suspenso,
                    Cor = Color.Red,
                    LinhaInteira = false
                });
            this.CoresMensagemExecType.Add(
                new CorEnumeradorInfo<OrdemTipoExecucaoEnum>()
                {
                    Valor = OrdemTipoExecucaoEnum.TerminoValidade,
                    Cor = Color.Red,
                    LinhaInteira = false
                });
        }
    }
}
