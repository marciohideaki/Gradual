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
    /// Classe de configuração do controle de histórico de operações
    /// </summary>
    [Serializable]
    public class OperacaoHistoricoParametros
    {
        /// <summary>
        /// Mantem a lista de layouts dos controles devexpress
        /// </summary>
        public LayoutsDevExpressHelper LayoutsDevExpress { get; set; }
        
        /// <summary>
        /// Serialização do layout do grid de operações
        /// </summary>
        public string LayoutGridOperacoes { get; set; }

        /// <summary>
        /// Serialização do layout do grid de mensagens
        /// </summary>
        public string LayoutGridMensagens { get; set; }

        /// <summary>
        /// Serialização do controle de layout
        /// </summary>
        public string LayoutForm { get; set; }

        /// <summary>
        /// Dicionario com as cores para os status de operacoes
        /// </summary>
        public List<CorEnumeradorInfo<OrdemStatusEnum>> CoresDosStatusOperacoes { get; set; }

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
        /// Construtor default
        /// </summary>
        public OperacaoHistoricoParametros()
        {
            // Inicializa os objetos e coleções
            this.LayoutsDevExpress = new LayoutsDevExpressHelper();
            this.CoresDosStatusOperacoes = new List<CorEnumeradorInfo<OrdemStatusEnum>>();
            this.CoresMensagemExecType = new List<CorEnumeradorInfo<OrdemTipoExecucaoEnum>>();
            this.CoresMensagemOrdRejReason = new List<CorEnumeradorInfo<OrdemMotivoRejeicaoEnum>>();
            this.CoresMensagemOrdStatus = new List<CorEnumeradorInfo<OrdemStatusEnum>>();

            // Cria elementos default
            criarElementosDefault();
        }

        private void criarElementosDefault()
        {
            // -----------------------------------------------------------------
            // Status de Operações
            // -----------------------------------------------------------------

            this.CoresDosStatusOperacoes.Add(
                new CorEnumeradorInfo<OrdemStatusEnum>() 
                { 
                    Valor = OrdemStatusEnum.Cancelada, 
                    Cor = Color.Red,
                    LinhaInteira = true
                });
            this.CoresDosStatusOperacoes.Add(
                new CorEnumeradorInfo<OrdemStatusEnum>()
                {
                    Valor = OrdemStatusEnum.CancelamentoPendente,
                    Cor = Color.Red,
                    LinhaInteira = true
                });
            this.CoresDosStatusOperacoes.Add(
                new CorEnumeradorInfo<OrdemStatusEnum>()
                {
                    Valor = OrdemStatusEnum.Executada,
                    Cor = Color.LightGreen,
                    LinhaInteira = true
                });
            this.CoresDosStatusOperacoes.Add(
                new CorEnumeradorInfo<OrdemStatusEnum>()
                {
                    Valor = OrdemStatusEnum.Expirada,
                    Cor = Color.Red,
                    LinhaInteira = true
                });
            this.CoresDosStatusOperacoes.Add(
                new CorEnumeradorInfo<OrdemStatusEnum>()
                {
                    Valor = OrdemStatusEnum.NaoImplementado,
                    Cor = Color.Gray,
                    LinhaInteira = true
                });
            this.CoresDosStatusOperacoes.Add(
                new CorEnumeradorInfo<OrdemStatusEnum>()
                {
                    Valor = OrdemStatusEnum.NaoInformado,
                    Cor = Color.Gray,
                    LinhaInteira = true
                });
            this.CoresDosStatusOperacoes.Add(
                new CorEnumeradorInfo<OrdemStatusEnum>()
                {
                    Valor = OrdemStatusEnum.Nova,
                    Cor = Color.LightCyan,
                    LinhaInteira = true
                });
            this.CoresDosStatusOperacoes.Add(
                new CorEnumeradorInfo<OrdemStatusEnum>()
                {
                    Valor = OrdemStatusEnum.NovaPendente,
                    Cor = Color.LightCyan,
                    LinhaInteira = true
                });
            this.CoresDosStatusOperacoes.Add(
                new CorEnumeradorInfo<OrdemStatusEnum>()
                {
                    Valor = OrdemStatusEnum.ParcialmenteExecutada,
                    Cor = Color.LightYellow,
                    LinhaInteira = true
                });
            this.CoresDosStatusOperacoes.Add(
                new CorEnumeradorInfo<OrdemStatusEnum>()
                {
                    Valor = OrdemStatusEnum.Rejeitada,
                    Cor = Color.Red,
                    LinhaInteira = true
                });
            this.CoresDosStatusOperacoes.Add(
                new CorEnumeradorInfo<OrdemStatusEnum>()
                {
                    Valor = OrdemStatusEnum.Substituida,
                    Cor = Color.Orange,
                    LinhaInteira = true
                });
            this.CoresDosStatusOperacoes.Add(
                new CorEnumeradorInfo<OrdemStatusEnum>()
                {
                    Valor = OrdemStatusEnum.Suspenso,
                    Cor = Color.Red,
                    LinhaInteira = true
                });

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
