using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Contratos.Dados.Risco;

namespace Gradual.Site.Www
{
    [Serializable]
    public class TransporteConfigurarLimites
    {
        public enum eTipoLimite
        {
            Nenhum = 0,
            OperarCompraAVista = 1,
            OperarCompraOpcao = 2,
            OperarDescobertoAvista = 3,
            OperarDescobertoOpcao = 4,
            ValorMaximoOrdem = 5,
        }

        #region Propriedades

        public eTipoLimite TipoLimite { get; set; }

        public bool StAtivo { get; set; }

        public string Limite { get; set; }

        public string Vencimento { get; set; }

        public string NomePermissao { get; set; }

        public string CodigoPermissao { get; set; }

        public int IdParametro { get; set; }

        public string DsParametro { get; set; }

        public string ValorAlocado { get ; set; }

        public string ValorLimite   { get ; set; }

        public string ValorDisponivel  { get ; set; }

        #endregion

        #region Métodos

        public List<TransporteConfigurarLimites> TraduzirListaLimites(List<ParametroRiscoClienteInfo> pParametro)
        {
            var lRetorno = new List<TransporteConfigurarLimites>();

            if (null != pParametro)
                pParametro.ForEach(delegate(ParametroRiscoClienteInfo prc)
                {
                    lRetorno.Add(new TransporteConfigurarLimites()
                    {
                        StAtivo = true,
                        Limite = prc.Valor.Value.ToString("N2"),
                        Vencimento = prc.DataValidade.Value.ToString("dd/MM/yyyy"),
                        TipoLimite = this.DefinirTipoLimite(prc.Parametro.NomeParametro),
                    });
                });

            return lRetorno;
        }

        public List<TransporteConfigurarLimites> TraduzirListaSaldo(List<RiscoLimiteAlocadoInfo> pParametro)
        {
            var lRetorno = new List<TransporteConfigurarLimites>();

            if (null != pParametro) pParametro.ForEach(delegate(RiscoLimiteAlocadoInfo rcl)
            {
                lRetorno.Add(new TransporteConfigurarLimites()
                {
                    IdParametro     = rcl.IdParametro,
                    DsParametro     = rcl.DsParametro,
                    ValorAlocado    = rcl.VlAlocado.ToString("N2"),
                    ValorLimite     = rcl.VlParametro .ToString("N2"),
                    ValorDisponivel = rcl.VlDisponivel.ToString("N2"),
                });
            });

            return lRetorno;
        }

        public List<TransporteConfigurarLimites> TraduzirListaPermissoes(List<PermissaoRiscoInfo> pParametro)
        {
            var lRetorno = new List<TransporteConfigurarLimites>();

            if (null != pParametro)
                pParametro.ForEach(delegate(PermissaoRiscoInfo pri)
                {
                    lRetorno.Add(
                        new TransporteConfigurarLimites()
                        {

                            NomePermissao = pri.NomePermissao.Replace("Permissao - ", string.Empty),
                            CodigoPermissao = pri.CodigoPermissao.ToString(),
                        });
                });

            lRetorno.Sort(delegate(TransporteConfigurarLimites pr1, TransporteConfigurarLimites pr2) { return Comparer<string>.Default.Compare(pr1.NomePermissao, pr2.NomePermissao); });

            return lRetorno;
        }

        #endregion

        #region Métodos Private

        private eTipoLimite DefinirTipoLimite(string pParametro)
        {
            if (pParametro.Contains("descoberto no mercado a vista"))
                return eTipoLimite.OperarDescobertoAvista;

            else if (pParametro.Contains("descoberto no mercado de opcoes"))
                return eTipoLimite.OperarDescobertoOpcao;

            else if (pParametro.Contains("boleta"))
                return eTipoLimite.ValorMaximoOrdem;

            else if (pParametro.Contains("compra mercado a vista"))
                return eTipoLimite.OperarCompraAVista;

            else if (pParametro.Contains("compra no mercado de opções"))
                return eTipoLimite.OperarCompraOpcao;
            else
                return eTipoLimite.Nenhum;
        }

        #endregion
    }
}