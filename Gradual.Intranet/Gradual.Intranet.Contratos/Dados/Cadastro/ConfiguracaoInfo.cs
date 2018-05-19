using System;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ConfiguracaoInfo : ICodigoEntidade
    {
        public int IdConfiguracao { get; set; }
        public EConfiguracaoDescricao Configuracao { get; set; }
        public string Valor { get; set; }

        /// <summary>
        /// Traduz uma string para um tipo 'EConfiguracao' compatível.
        /// </summary>
        /// <param name="pDescricaoConfiguracao">String com o nome da descrição da configuração.</param>
        /// <returns>Retorna um tipo 'EConfiguracao' compatível com a string de parâmetro.</returns>
        public static EConfiguracaoDescricao TraduzirEnum(object pDescricaoConfiguracao)
        {
            var lDescricaoConfiguracao = Convert.ToString(pDescricaoConfiguracao).ToLower();

            if (lDescricaoConfiguracao.Equals(EConfiguracaoDescricao.PeriodicidadeRenovacaoCadastral.ToString().ToLower()))
                return EConfiguracaoDescricao.PeriodicidadeRenovacaoCadastral;
            else if (lDescricaoConfiguracao.Equals(EConfiguracaoDescricao.PeriodoRegressoDeConsultaParaRenovacaoCadastral.ToString().ToLower()))
                return EConfiguracaoDescricao.PeriodoRegressoDeConsultaParaRenovacaoCadastral;
            else
                throw new InvalidCastException(string.Format("Não foi possível converter o tipo {0}. Tipo incompatível com o registrado no sistema.", pDescricaoConfiguracao));
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }


}
