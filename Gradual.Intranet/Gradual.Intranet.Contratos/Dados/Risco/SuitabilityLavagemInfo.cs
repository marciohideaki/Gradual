using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Dados.Risco
{
    #region Propriedades
    
    public enum enumVolume
    {
        TODOS = 0,
        ABAIXO_500M = 1,
        ENTRE_500M_E_1000M = 2,
        ACIMA_1000M = 3
    }

    public enum enumVOLxSFP
    {
        TODOS = 0,
        ABAIXO_20 = 1,
        ENTRE_20_E_50 = 2,
        ACIMA_50 = 3

    }

    public enum enumEnquadrado
    {
        Todos = -1,
        NaoEnquadrado = 0,
        Enquadrado = 1
    }
    #endregion
    public class SuitabilityClienteProduto
    {
        public string TipoPerfil { get; set; }
        public string Produto { get; set; }
    }

    public struct SuitabilityClienteInstucionais
    {
        public int CodigoCliente { get; set; }
    }

    public class SuitabilityClienteDataNaoEnquadrados
    {
        #region Request
        public DateTime DataDe { get; set; }
        public DateTime DataAte { get; set; }
        #endregion

        public int CodigoBovespa { get; set; }
        public DateTime Data { get; set; }
    }

    public class SuitabilityLavagemInfo
    {
        #region Requests
        public Nullable<int> CodigoCliente { get; set; }

        public Nullable<int> CodigoAssessor { get; set; }

        public Nullable<int> CodigoLogin { get; set; }

        public enumVolume enumVolume { get; set; }

        public decimal PercentualVOLxSFP { get; set; }

        public enumVOLxSFP enumVOLxSFP { get; set; }

        public Nullable<DateTime> DataDe { get; set; }

        public Nullable<DateTime> DataAte { get; set; }

        public enumEnquadrado enumEnquadrado { get; set; }

        public decimal SFP { get; set; }

        public decimal Volume { get; set; }

        public string Suitability { get; set; }
        
        public string ArquivoCiencia { get; set; }
        
        public string ArquivoCienciaData { get; set; }

        public DateTime Data { get; set; }

        public Nullable<int> CodigoClienteBmf { get; set; }
        #endregion

        #region Response
        public List<SuitabilityLavagemInfo> Resultado { get; set; }
        #endregion

        public string NomeAssessor { get; set; }

        public string NomeCliente { get; set; }
    }
}
