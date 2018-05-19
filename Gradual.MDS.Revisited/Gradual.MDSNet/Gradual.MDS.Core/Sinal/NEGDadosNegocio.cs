using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.MDS.Core.Lib;

namespace Gradual.MDS.Core.Sinal
{
    public class NEGDadosNegocio
    {
        public Decimal MelhorPrecoCompra { get; set; }
        public Decimal MelhorPrecoVenda { get; set; }
        public Decimal Preco { get; set; }
        public Decimal PrecoAbertura { get; set; }
        public Decimal PrecoAjuste { get; set; }
        public Decimal PrecoUnitario { get; set; }
        public Decimal PrecoFechamento { get; set; }
        public Decimal PrecoLeilao { get; set; }
        public Decimal PrecoMaximo { get; set; }
        public Decimal PrecoMedio { get; set; }
        public Decimal PrecoMinimo { get; set; }
        public Decimal PrecoTeoricoAbertura { get; set; }
        public Decimal VolumeTotal { get; set; }
        public int EstadoInstrumento { get; set; }
        public long MelhorQuantidadeCompra { get; set; }
        public long MelhorQuantidadeVenda { get; set; }
        public long QtdeNegociadaDia { get; set; }
        public long QtdeNegocios { get; set; }
        public long Quantidade { get; set; }
        public string Compradora { get; set; }
        public string Data { get; set; }
        public DateTime HorarioTeorico { get; set; }
        public string Hora { get; set; }
        public string HoraAbertura { get; set; }
        public string HoraFechamento { get; set; }
        public string HoraPreAbertura { get; set; }
        public string Instrumento { get; set; }
        public string MsgIdAnterior { get; set; }
        public string NumeroNegocio { get; set; }
        public Decimal Variacao { get; set; }
        public Decimal VariacaoTeorica { get; set; }
        public string Vendedora { get; set; }
        public string EstadoGrupo { get; set; }
        public string EstadoPapel2 { get; set; }

        public NEGDadosNegocio(string instrumento)
        {
            this.Instrumento = instrumento;
            this.Hora = DateTime.MinValue.ToString("HHmmss");
            this.Data = DateTime.MinValue.ToString("yyyyMMdd");
            this.EstadoInstrumento = ConstantesMDS.ESTADO_PAPEL_INIBIDO;
            this.EstadoGrupo = ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_CLOSE;
            this.EstadoPapel2 = ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_CLOSE;
        }
    }
}
