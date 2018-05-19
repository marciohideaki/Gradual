using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Custodia.Dados;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Contratos.Risco.Regras;
using Gradual.OMS.Contratos.Risco.Regras.Criticas;
using Gradual.OMS.Contratos.Risco.Regras.Dados;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Risco.Regras
{
    /// <summary>
    /// Regra de validação de limites.
    /// </summary>
    [Regra(
        CodigoRegra = "3E04B0BB-5870-4bc5-9B5B-63AAEDD7E905",
        NomeRegra = "Validação de Limites",
        DescricaoRegra = "Faz validação de diversos tipos de limite.",
        TipoConfig = typeof(RegraLimitesConfig),
        RegraDeUsuario = true)]
    [Serializable]
    public class RegraLimites : RegraRiscoBase
    {
        #region Variaveis Locais

        /// <summary>
        /// Referencia para as configurações da regra.
        /// Consegue-se essa referencia na inicialização
        /// </summary>
        private RegraLimitesConfig _config = null;

        /// <summary>
        /// Referencia para objeto de informações da regra
        /// </summary>
        private RegraRiscoInfo _regraRiscoInfo = null;

        /// <summary>
        /// Referencia para o servico de risco
        /// </summary>
        private IServicoRisco _servicoRisco = Ativador.Get<IServicoRisco>();

        /// <summary>
        /// Limite a validar
        /// </summary>
        private LimiteInfo _limite = null;

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor.
        /// Recebe as informações da regra.
        /// </summary>
        /// <param name="regraInfo"></param>
        public RegraLimites(RegraRiscoInfo regraInfo) : base(regraInfo)
        {
            // Seta referencias iniciais
            _regraRiscoInfo = regraInfo;
            _config = (RegraLimitesConfig)_regraRiscoInfo.Config;
            _limite = _config.Limite;
        }

        #endregion

        #region Overloads

        /// <summary>
        /// Overload para realizar a lógica da regra.
        /// A lógica da regra consiste em verificar se os limites informados não foram ultrapassados.
        /// Por enquanto assume que a única mensagem suportada para a validação do risco é ExecutarOrdem
        /// </summary>
        /// <param name="contexto"></param>
        /// <returns></returns>
        protected override bool OnValidar(ContextoValidacaoInfo contexto)
        {
            // Inicializa
            RiscoGrupoInfo agrupamento = contexto.Complementos.ReceberItem<RiscoGrupoInfo>();  // Este é o agrupamento em que a regra foi pega
            ExecutarOrdemRequest mensagem = (ExecutarOrdemRequest)contexto.Mensagem;
            CacheRiscoInfo cacheRisco = contexto.Complementos.ReceberItem<CacheRiscoInfo>();
            bool contemErro = false;

            // Pega o valor da operação
            double valorOperacao = mensagem.OrderQty * mensagem.Price;

            // Lista com as críticas de erros
            List<CriticaInfo> criticasErros = new List<CriticaInfo>();

            // Recebe a custodia correspondente ao grupo que está sendo testado
            List<CustodiaPosicaoInfo> custodiaPosicoes =
                RegrasRiscoLib.ReceberCustodiaDoAgrupamento(
                    cacheRisco.Custodia, agrupamento);

            // Calcula totais
            double tomado = 0;
            double qtdeComprado = 0;
            double qtdeVendido = 0;
            double valorComprado = 0;
            double valorVendido = 0;
            foreach (CustodiaPosicaoInfo custodiaPosicao in custodiaPosicoes)
            {
                tomado += custodiaPosicao.ValorPosicao;
                if (custodiaPosicao.QuantidadeAtual > 0)
                {
                    qtdeComprado += custodiaPosicao.QuantidadeAtual;
                    valorComprado += custodiaPosicao.ValorPosicao;
                }
                if (custodiaPosicao.QuantidadeAtual < 0)
                {
                    qtdeVendido += Math.Abs(custodiaPosicao.QuantidadeAtual);
                    valorVendido += Math.Abs(custodiaPosicao.ValorPosicao);
                }
            }

            // Inclui a quantidade da operação
            if (mensagem.Side == OrdemDirecaoEnum.Compra)
            {
                qtdeComprado += mensagem.OrderQty;
                valorComprado += mensagem.OrderQty * mensagem.Price;
            }
            else if (mensagem.Side == OrdemDirecaoEnum.Venda)
            {
                qtdeVendido += mensagem.OrderQty;
                valorVendido += mensagem.OrderQty * mensagem.Price;
            }

            // Testa limite de quantidade de custodia inferior
            if (mensagem.Side == OrdemDirecaoEnum.Venda && _limite.LimiteQuantidadeCustodiaInferior.HasValue && Math.Abs(_limite.LimiteQuantidadeCustodiaInferior.Value) < qtdeVendido)
                criticasErros.Add(
                    new CriticaRiscoLimiteInfo()
                    {
                        Descricao =
                            string.Format(
                                "Quantidade em custódia vendida ({0}) ultrapassa limite de quantidade de custódia inferior ({1}) para o agrupamento ({2}).",
                                qtdeVendido,
                                Math.Abs(_limite.LimiteQuantidadeCustodiaInferior.Value),
                                agrupamento.ToString()),
                        LimiteInfo = _limite,
                        Status = CriticaStatusEnum.ErroNegocio,
                        Agrupamento = agrupamento,
                        ValorLimite = _limite.LimiteQuantidadeCustodiaInferior.Value,
                        ValorTestado = qtdeVendido
                    });

            // Testa limite de valor de custodia inferior
            if (mensagem.Side == OrdemDirecaoEnum.Venda && _limite.LimiteValorCustodiaInferior.HasValue && Math.Abs(_limite.LimiteValorCustodiaInferior.Value) < valorVendido)
                criticasErros.Add(
                    new CriticaRiscoLimiteInfo()
                    {
                        Descricao =
                            string.Format(
                                "Valor em custódia vendida ({0}) ultrapassa limite de valor de custódia inferior ({1}) para o agrupamento ({2}).",
                                valorVendido,
                                Math.Abs(_limite.LimiteValorCustodiaInferior.Value),
                                agrupamento.ToString()),
                        LimiteInfo = _limite,
                        Status = CriticaStatusEnum.ErroNegocio,
                        Agrupamento = agrupamento,
                        ValorLimite = _limite.LimiteValorCustodiaInferior.Value,
                        ValorTestado = valorVendido
                    });

            // Testa limite de quantidade de custodia superior
            if (mensagem.Side == OrdemDirecaoEnum.Compra && _limite.LimiteQuantidadeCustodiaSuperior.HasValue && Math.Abs(_limite.LimiteQuantidadeCustodiaSuperior.Value) < qtdeComprado)
                criticasErros.Add(
                    new CriticaRiscoLimiteInfo()
                    {
                        Descricao =
                            string.Format(
                                "Quantidade de custódia comprada ({0}) ultrapassa limite de quantidade de custódia superior ({1}) para o agrupamento ({2}).",
                                qtdeComprado,
                                Math.Abs(_limite.LimiteQuantidadeCustodiaSuperior.Value),
                                agrupamento.ToString()),
                        LimiteInfo = _limite,
                        Status = CriticaStatusEnum.ErroNegocio,
                        Agrupamento = agrupamento,
                        ValorLimite = _limite.LimiteQuantidadeCustodiaSuperior.Value,
                        ValorTestado = qtdeComprado
                    });

            // Testa limite de valor de custodia superior
            if (mensagem.Side == OrdemDirecaoEnum.Compra && _limite.LimiteValorCustodiaSuperior.HasValue && Math.Abs(_limite.LimiteValorCustodiaSuperior.Value) < valorComprado)
                criticasErros.Add(
                    new CriticaRiscoLimiteInfo()
                    {
                        Descricao =
                            string.Format(
                                "Valor em custódia comprada ({0}) ultrapassa limite de valor de custódia superior ({1}) para o agrupamento ({2}).",
                                valorComprado,
                                Math.Abs(_limite.LimiteValorCustodiaSuperior.Value),
                                agrupamento.ToString()),
                        LimiteInfo = _limite,
                        Status = CriticaStatusEnum.ErroNegocio,
                        Agrupamento = agrupamento,
                        ValorLimite = _limite.LimiteValorCustodiaSuperior.Value,
                        ValorTestado = valorComprado
                    });

            // Teste limite de quantidade inferior da operação
            if (mensagem.Side == OrdemDirecaoEnum.Venda && _limite.LimiteQuantidadeOperacaoInferior.HasValue && Math.Abs(_limite.LimiteQuantidadeOperacaoInferior.Value) < mensagem.OrderQty)
                criticasErros.Add(
                    new CriticaRiscoLimiteInfo()
                    {
                        Descricao =
                            string.Format(
                                "Quantidade de operação de venda ({0}) ultrapassa limite de quantidade inferior ({1}) para o agrupamento ({2}).",
                                mensagem.OrderQty,
                                Math.Abs(_limite.LimiteQuantidadeOperacaoInferior.Value),
                                agrupamento.ToString()),
                        LimiteInfo = _limite,
                        Status = CriticaStatusEnum.ErroNegocio,
                        Agrupamento = agrupamento,
                        ValorLimite = _limite.LimiteQuantidadeOperacaoInferior.Value,
                        ValorTestado = mensagem.OrderQty
                    });

            // Testa limite de valor da operação inferior
            if (mensagem.Side == OrdemDirecaoEnum.Venda && _limite.LimiteValorOperacaoInferior.HasValue && Math.Abs(_limite.LimiteQuantidadeOperacaoInferior.Value) < valorOperacao)
                criticasErros.Add(
                    new CriticaRiscoLimiteInfo()
                    {
                        Descricao =
                            string.Format(
                                "Valor da operação de venda ({0}) ultrapassa limite de valor de operação inferior ({1}) para o agrupamento ({2}).",
                                valorOperacao,
                                Math.Abs(_limite.LimiteValorOperacaoInferior.Value),
                                agrupamento.ToString()),
                        LimiteInfo = _limite,
                        Status = CriticaStatusEnum.ErroNegocio,
                        Agrupamento = agrupamento,
                        ValorLimite = _limite.LimiteValorOperacaoInferior.Value,
                        ValorTestado = valorOperacao
                    });

            // Teste limite de quantidade superior da operação
            if (mensagem.Side == OrdemDirecaoEnum.Compra && _limite.LimiteQuantidadeOperacaoSuperior.HasValue && Math.Abs(_limite.LimiteQuantidadeOperacaoSuperior.Value) < mensagem.OrderQty)
                criticasErros.Add(
                    new CriticaRiscoLimiteInfo()
                    {
                        Descricao =
                            string.Format(
                                "Quantidade de operação de compra ({0}) ultrapassa limite de quantidade superior ({1}) para o agrupamento ({2}).",
                                mensagem.OrderQty,
                                Math.Abs(_limite.LimiteQuantidadeOperacaoSuperior.Value),
                                agrupamento.ToString()),
                        LimiteInfo = _limite,
                        Status = CriticaStatusEnum.ErroNegocio,
                        Agrupamento = agrupamento,
                        ValorLimite = _limite.LimiteQuantidadeOperacaoSuperior.Value,
                        ValorTestado = mensagem.OrderQty
                    });

            // Testa limite de quantidade da operação superior
            if (mensagem.Side == OrdemDirecaoEnum.Compra && _limite.LimiteValorOperacaoSuperior.HasValue && Math.Abs(_limite.LimiteValorOperacaoSuperior.Value) < valorOperacao)
                criticasErros.Add(
                    new CriticaRiscoLimiteInfo()
                    {
                        Descricao =
                            string.Format(
                                "Valor da operação de compra ({0}) ultrapassa limite de valor de operação superior ({1}) para o agrupamento ({2}).",
                                valorOperacao,
                                Math.Abs(_limite.LimiteValorOperacaoSuperior.Value), agrupamento.ToString()),
                        LimiteInfo = _limite,
                        Status = CriticaStatusEnum.ErroNegocio,
                        Agrupamento = agrupamento,
                        ValorLimite = _limite.LimiteValorOperacaoSuperior.Value,
                        ValorTestado = valorOperacao
                    });

            // Adiciona as críticas
            contexto.Criticas.AddRange(criticasErros);

            // Se não houve crítica deste limite, adiciona sucesso
            if (criticasErros.Count == 0)
                contexto.Criticas.Add(
                    new CriticaRiscoLimiteInfo()
                    {
                        Descricao = "Regra de limite validada.",
                        LimiteInfo = _limite,
                        Status = CriticaStatusEnum.Validado
                    });
            else
                contemErro = true;

            // Retorna
            return contemErro == false;
        }

        /// <summary>
        /// Overload para realizar a lógica de desfazer a validação, caso necessário
        /// </summary>
        /// <param name="contexto"></param>
        public override void OnDesfazer(ContextoValidacaoInfo contexto)
        {
            base.OnDesfazer(contexto);
        }

        #endregion

        public override string ToString()
        {
            return this.NomeRegra;
        }
    }
}
