using Gradual.FIDC.Adm.DbLib.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.FIDC.Adm.Web.App_Codigo.Transporte
{
    /// <summary>
    /// Classe de transporte de Cadastro de Fundos
    /// </summary>
    public class TransporteConsultaFundosConstituicao
    {
        
        #region Propriedades
        public int IdFundoCadastro { get; set; }
        public string NomeFundo { get; set; }
        public string Grupo { get; set; }
        public string Etapa { get; set; }
        public string StatusEtapa { get; set; }
        public string StatusGeral { get; set; }
        #endregion

        #region Construtores
        public TransporteConsultaFundosConstituicao() { }

        public TransporteConsultaFundosConstituicao(ConsultaFundosConstituicaoInfo pInfo)
        {
            
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método para Tradução de lista para cadastro de fundos
        /// </summary>
        /// <param name="pInfo">Info de cadastro de fundos</param>
        /// <returns>Retorna uma lista de cadastro de fundos</returns>
        public List<TransporteConsultaFundosConstituicao> TraduzirLista(List<ConsultaFundosConstituicaoInfo> pInfo)
        {
            var lRetorno = new List<TransporteConsultaFundosConstituicao>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteConsultaFundosConstituicao()
                    {
                        IdFundoCadastro = info.IdFundoCadastro,
                        NomeFundo = info.NomeFundo,
                        Grupo = info.Grupo,
                        Etapa = info.Etapa,
                        StatusEtapa = info.StatusEtapa,
                        StatusGeral = ((info.StatusGeral == "1") ? "Concluído" : "Pendente"),
                    });
                });
            }

            return lRetorno;
        }
        #endregion
    }
}