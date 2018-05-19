using Gradual.FIDC.Adm.DbLib.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.FIDC.Adm.Web.App_Codigo.Transporte
{
    public class TransporteCadastroCotistasFidc
    {
        
        #region Propriedades
        public int IdCotistaFidc { get; set; }
        public string NomeCotista { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public DateTime DataNascFundacao { get; set; }
        public bool IsAtivo { get; set; }
        public DateTime DtInclusao { get; set; }
        public int QuantidadeCotas { get; set; }
        public string ClasseCotas { get; set; }
        public DateTime DtVencimentoCadastro { get; set; }
        #endregion

        #region Construtores
        public TransporteCadastroCotistasFidc() { }
        #endregion

        #region Métodos
        public List<TransporteCadastroCotistasFidc> TraduzirLista(List<CadastroCotistasFidcInfo> pInfo)
        {
            var lRetorno = new List<TransporteCadastroCotistasFidc>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteCadastroCotistasFidc()
                    {
                        IdCotistaFidc = info.IdCotistaFidc,
                        NomeCotista = info.NomeCotista,
                        CpfCnpj = info.CpfCnpj,
                        Email = info.Email,
                        DataNascFundacao = info.DataNascFundacao,
                        IsAtivo = info.IsAtivo,
                        DtInclusao = info.DtInclusao,
                        QuantidadeCotas = info.QuantidadeCotas,
                        ClasseCotas = info.ClasseCotas,
                        DtVencimentoCadastro = info.DtVencimentoCadastro
                    });
                });
            }

            return lRetorno;
        }
        #endregion
    }
}