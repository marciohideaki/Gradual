using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Dados.Risco
{
    /// <summary>
    /// Para filtro de Net x SFP
    /// </summary>
    public enum EnumNETxSFP
    {
        TODOS = 0,
        ABAIXO_20 = 1,
        ENTRE_20_E_50 = 2,
        ACIMA_50 = 3
    }

    /// <summary>
    /// Para filtro de Exposição x Posição
    /// </summary>
    public enum EnumEXPxPosicao
    {
        TODOS = 0,
        ABAIXO_20 = 1,
        ENTRE_20_E_50 = 2,
        ACIMA_50 = 3
    }

    /// <summary>
    /// Para Filtro de Net - Volume
    /// </summary>
    public enum EnumNet
    {
        TODOS = 0,
        ABAIXO_500_MIL = 1,
        ENTRE_500_1000_MIL = 2,
        ACIMA_1000 = 3
    }

    /// <summary>
    /// Classe info do Monitoramento do Intradiario de risco p
    /// </summary>
    public class MonitoramentoIntradiarioInfo
    {
        #region Requests
        public Nullable<int> CodigoCliente { get; set; }

        public Nullable<int> CodigoAssessor { get; set; }

        public Nullable<int> CodigoLogin { get; set; }

        public EnumNETxSFP enumNETxSFP { get; set; }

        public EnumNet enumNET { get; set; }

        public EnumEXPxPosicao enumEXPxPosicao { get; set; }

        public Nullable<DateTime> DataDe { get; set; }

        public Nullable<DateTime> DataAte { get; set; }

        public decimal Exposicao { get; set; }

        public decimal Posicao { get; set; }

        public DateTime Data { get; set; }

        public Nullable<int> CodigoClienteBmf { get; set; }
        #endregion

        #region Construtores
        public MonitoramentoIntradiarioInfo()
        {
            
        }
        #endregion

        #region Response
        public List<MonitoramentoIntradiarioInfo> Resultado { get; set; }
        #endregion

        #region Propriedades

        public string NomeCliente { get; set; }
        
        public string NomeAssessor { get; set; }
        
        public decimal Net { get; set; }
        
        public decimal SFP { get; set; }
        
        public decimal NETxSFP { get; set; }
        
        public decimal EXPxPosicao { get; set; }
        #endregion
    }
}
