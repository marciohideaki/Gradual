using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco
{
    public class TransporteRelatorio_005 : ICodigoEntidade
    {
        #region | Propriedades

        public int IdClienteParametro { get; set; }

        public string NomeCliente { get; set; }

        public string CpfCnpj { get; set; }

        public string Parametro { get; set; }

        public string ValorAlocado { get; set; }

        public string ValorDisponivel { get; set; }

        public string ValorLimite { get; set; }

        public string ValorMovimento { get; set; }

        public string DataMovimento { get; set; }

        public string DataValidade { get; set; }

        public string Historico { get; set; }

        #endregion

        #region | Métodos

        public List<TransporteRelatorio_005> TraduzirListaSaldo(List<RiscoLimiteAlocadoInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_005>();

            if (null != pParametro) pParametro.ForEach(delegate(RiscoLimiteAlocadoInfo rla) 
            {
                lRetorno.Add(new TransporteRelatorio_005()
                {
                    Parametro = rla.DsParametro,
                    ValorAlocado = rla.VlAlocado.ToString("N2"),
                    ValorLimite = rla.VlParametro.ToString("N2"),
                    ValorDisponivel = rla.VlDisponivel.ToString("N2"),
                });
            });

            return lRetorno;
        }

        public List<TransporteRelatorio_005> TraduzirListaSaldo(List<RiscoClienteLimiteRelInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_005>();

            if (null != pParametro) pParametro.ForEach(delegate(RiscoClienteLimiteRelInfo rcl)
                {
                    lRetorno.Add(new TransporteRelatorio_005()
                    {
                        CpfCnpj = rcl.DsCpfCnpj.ToCpfCnpjString(),
                        DataValidade = (null != rcl.DtValidade && rcl.DtValidade.HasValue) ? rcl.DtValidade.Value.ToString("dd/MM/yyyy") : string.Empty,
                        IdClienteParametro = (null != rcl.IdClienteParametro && rcl.IdClienteParametro.HasValue) ? rcl.IdClienteParametro.Value : default(int),
                        Parametro = rcl.DsParametro,
                        NomeCliente = rcl.DsNome.ToStringFormatoNome(),
                        ValorAlocado = rcl.VlAlocado.ToString("N2"),
                        ValorLimite = rcl.VlLimite.ToString("N2"),
                        ValorDisponivel = (rcl.VlLimite - rcl.VlAlocado).ToString("N2"),
                    });
                });

            return lRetorno;
        }

        public List<TransporteRelatorio_005> TraduzirListaMovimento(List<RiscoClienteLimiteMovimentoRelInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_005>();

            if (null != pParametro) pParametro.ForEach(delegate(RiscoClienteLimiteMovimentoRelInfo rcl)
            {
                lRetorno.Add(new TransporteRelatorio_005()
                {
                    DataMovimento = rcl.DataMovimento.ToString("dd/MM/yyyy HH:mm"),
                    ValorMovimento = rcl.ValorMovimento.ToString("N2"),
                    ValorAlocado = rcl.ValorAlocado.ToString("N2"),
                    ValorDisponivel = rcl.ValorDisponivel.ToString("N2"),
                    Historico = rcl.Historico,
                });
            });

            //--> Ordenando por data.
            lRetorno.Sort((tr1, tr2) => Comparer<DateTime>.Default.Compare(tr1.DataMovimento.DBToDateTime(), tr2.DataMovimento.DBToDateTime()));

            return lRetorno;
        }

        #endregion

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}