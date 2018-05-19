

namespace Gradual.OMS.Mensageria
{
    /// <summary>
    /// Interface dos montadores de regras. Os montadores de regras são componentes 
    /// que criam a sequencia das regras a serem validadas de acordo com o conteúdo 
    /// da mensagem.
    /// As regras poderão ser cadastradas fixamente, mas o gerador pode adicionar 
    /// regras de acordo com as características da mensagem.
    /// </summary>
    public interface IGeradorRegra
    {
        /// <summary>
        /// Solicita ao gerador que gere a sequencia de regras a serem validadas
        /// para a mensagem informada.
        /// A geração deve ser feita no proprio contexto, na propriedade RegrasAValidar.
        /// </summary>
        void GerarSequencia(ContextoValidacaoInfo contexto);
    }
}
