using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class ResumoGerenteinfo : ICodigoEntidade
    {
        public decimal CorretagemMes { get; set; }
        public decimal CorretagemMesAnterior { get; set; }
        public decimal CorretagemIntervaloData { get; set; }
        public decimal VolumeMes { get; set; }
        public decimal VolumeMesAnterior { get; set; }
        public decimal VolumeIntervaloData { get; set; }
        public decimal CadastradoMes { get; set; }
        public decimal CadastradoMesAnterior { get; set; }
        public decimal CadastradoIntervaloData { get; set; }

        public decimal Porcentagemclientes { get; set; }
        public decimal PorcentagemClienteCustodia { get; set; }
        public decimal ClienteNaoOperaramNoventaDia { get; set; }
        public decimal MediaCorretagem { get; set; }
        public decimal MediaCustodia { get; set; }

        public decimal CodigoFilial { get; set; }



        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        
        public int CodigoEscritorio { get; set; }
        public TipoMercado Mercado { get; set; }

        public List<ResumoAssessorInfo> ListaRessumoAssessor { get; set; }




        public enum TipoMercado
        {
            BMF         = 0,
            BVSP        = 1,
            BMF_BVSP    = 2,
        }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }


    }
}

