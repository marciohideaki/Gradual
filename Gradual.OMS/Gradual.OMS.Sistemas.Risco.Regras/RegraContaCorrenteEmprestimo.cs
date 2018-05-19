using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.ContaCorrente.Dados;
using Gradual.OMS.Contratos.Custodia.Dados;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens;
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
    /// Regra de validação de conta corrente e empréstimos.
    /// Permite associar linhas de créditos à agrupamentos que serão checados a cada operação.
    /// </summary>
    [Regra(
        CodigoRegra = "43D5B533-781B-4d9a-A3F2-D6B5DC49128E",
        NomeRegra = "Conta Corrente e Empréstimos",
        DescricaoRegra = "Faz validação de conta corrente + empréstimos concedidos.",
        TipoConfig = typeof(RegraContaCorrenteEmprestimoConfig),
        RegraDeUsuario = true)]
    [Serializable]
    public class RegraContaCorrenteEmprestimo : RegraRiscoBase
    {
        #region Variaveis Locais

        /// <summary>
        /// Referencia para as configurações da regra.
        /// Consegue-se essa referencia na inicialização
        /// </summary>
        private RegraContaCorrenteEmprestimoConfig _config = null;

        /// <summary>
        /// Referencia para objeto de informações da regra
        /// </summary>
        private RegraRiscoInfo _regraRiscoInfo = null;

        /// <summary>
        /// Referencia para o servico de risco
        /// </summary>
        private IServicoRisco _servicoRisco = Ativador.Get<IServicoRisco>();

        /// <summary>
        /// Emprestimo a validar
        /// </summary>
        private EmprestimoInfo _emprestimo = null;

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor.
        /// Recebe as informações da regra.
        /// </summary>
        /// <param name="regraInfo"></param>
        public RegraContaCorrenteEmprestimo(RegraRiscoInfo regraInfo) : base(regraInfo)
        { 
            // Seta referencias iniciais
            _regraRiscoInfo = regraInfo;
            _config = (RegraContaCorrenteEmprestimoConfig)_regraRiscoInfo.Config;
            _emprestimo = _config.Emprestimo;
        }

        #endregion

        #region Overloads

        /// <summary>
        /// Overload para realizar a lógica da regra.
        /// A lógica da regra consiste em somar conta corrente + empréstimos (regulares e conta margem) e ver se o 
        /// valor da operação cabe no resultado.
        /// Por enquanto assume que a única mensagem suportada para a validação do risco é ExecutarOrdem
        /// </summary>
        /// <param name="contexto"></param>
        /// <returns></returns>
        protected override bool OnValidar(ContextoValidacaoInfo contexto)
        {
            // Inicializa
            RiscoGrupoInfo agrupamento = contexto.Complementos.ReceberItem<RiscoGrupoInfo>();
            ExecutarOrdemRequest mensagem = (ExecutarOrdemRequest)contexto.Mensagem;
            CacheRiscoInfo cacheRisco = contexto.Complementos.ReceberItem<CacheRiscoInfo>();
            bool valido = true;
            
            // Pega o valor da operação
            double valorOperacao = mensagem.OrderQty * mensagem.Price;

            // Recebe a custodia correspondente ao grupo que está sendo testado
            List<CustodiaPosicaoInfo> custodiaPosicoes = 
                RegrasRiscoLib.ReceberCustodiaDoAgrupamento(
                    cacheRisco.Custodia, agrupamento);

            // Calcula total tomado
            double tomado = 0;
            foreach (CustodiaPosicaoInfo custodiaPosicao in custodiaPosicoes)
                tomado += custodiaPosicao.ValorPosicao;

            // Acha o saldo: diferença entre disponivel (conta corrente + limites) e tomado (custodia)
            // ** Falta considerar o bloqueio
            double saldo = cacheRisco.ContaCorrente.SaldoRegularAtual + _emprestimo.ValorEmprestimo - tomado;

            // Verifica se operação cabe nesse valor
            if (valorOperacao > saldo)
            {
                // Insere crítica
                contexto.Criticas.Add(
                    new CriticaRiscoEmprestimoInfo()
                    {
                        Status = CriticaStatusEnum.ErroNegocio,
                        Descricao = 
                        string.Format(
                            "Valor da operação ({0}) é maior que o total permitido ({1}) para o agrupamento ({2}).", 
                            valorOperacao, saldo, agrupamento.ToString()),
                        EmprestimoInfo = _emprestimo
                    });

                // Operação não cabe neste limite
                valido = false;
            }
            else
            {
                // Insere crítica de passagem
                contexto.Criticas.Add(
                    new CriticaRiscoEmprestimoInfo()
                    {
                        Status = CriticaStatusEnum.Validado,
                        Descricao = "Limite de empréstimo validado",
                        EmprestimoInfo = _emprestimo
                    });
            }

            // Retorna
            return valido;
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
