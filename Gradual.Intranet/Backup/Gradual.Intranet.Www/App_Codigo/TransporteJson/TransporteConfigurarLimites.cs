using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Risco.Regra.Lib.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteConfigurarLimites
    {
        #region | Propriedades

        public eTipoLimite TipoLimite { get; set; }

        public bool StAtivo { get; set; }

        public string Limite { get; set; }

        public string Vencimento { get; set; }

        public string NomePermissao { get; set; }

        public string CodigoPermissao { get; set; }

        #endregion

        #region | Métodos

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

        #region | Métodos de apoio

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