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
    public class TransporteCadastroFundos
    {
        
        #region Propriedades
        public int IdFundoCadastro { get; set; }
        public string NomeFundo { get; set; }
        public string NomeAdministrador { get; set; }
        public string NomeCustodiante { get; set; }
        public string NomeGestor { get; set; }
        public string Status { get; set; }
        /// <summary>
        /// Indica se o fundo já pertence à relação categoria x subcategoria
        /// </summary>
        public bool PertenceACategoriaSubCategoria { get; set; }
        #endregion

        #region Construtores
        public TransporteCadastroFundos() { }

        public TransporteCadastroFundos(CadastroFundoInfo pInfo)
        {
            this.NomeFundo = pInfo.NomeFundo.ToString();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método para Tradução de lista para cadastro de fundos
        /// </summary>
        /// <param name="pInfo">Info de cadastro de fundos</param>
        /// <returns>Retorna uma lista de cadastro de fundos</returns>
        public List<TransporteCadastroFundos> TraduzirLista(List<CadastroFundoInfo> pInfo)
        {
            var lRetorno = new List<TransporteCadastroFundos>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteCadastroFundos()
                    {
                        IdFundoCadastro = info.IdFundoCadastro,
                        NomeFundo = info.NomeFundo,
                        NomeAdministrador = info.NomeAdministrador,
                        NomeCustodiante = info.NomeCustodiante,
                        NomeGestor = info.NomeGestor,
                        Status = info.IsAtivo ? "ativo" : "inativo",
                        PertenceACategoriaSubCategoria = info.Pertence
                    });
                });
            }

            return lRetorno;
        }
        #endregion
    }
}