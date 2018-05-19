using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteInvestidorNaoResidente : ITransporteJSON
    {

        public TransporteInvestidorNaoResidente() { }

        public TransporteInvestidorNaoResidente(ClienteInvestidorNaoResidenteInfo pObjeto, bool pExclusao)
        {
            this.CodigoCVM           = pObjeto.DsCodigoCvm;
            this.CustodianteNoPAS    = pObjeto.DsCustodiante;
            this.Id                  = pObjeto.IdInvestidorNaoResidente;
            this.Nome                = pObjeto.DsRepresentanteLegal;
            this.NomeDoAdministrador = pObjeto.DsNomeAdiministradorCarteira;
            this.PaisDeOrigem        = pObjeto.CdPaisOrigem;
            this.ParentId            = pObjeto.IdCliente;
            this.RDE                 = pObjeto.DsRde;
            this.Exclusao            = pExclusao;
        }

        /// <summary>
        /// Id do Investidor não residente
        /// </summary>       
        public Nullable<int> Id { get; set; }

        /// <summary>
        /// Código do Cliente
        /// </summary>       
        public int ParentId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string TipoDeItem { get { return "RepresentanteParaNaoResidente"; } }

        /// <summary>
        /// Nome do Representante Legal
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Custodiante no País
        /// </summary>
        public string CustodianteNoPAS { get; set; }

        /// <summary>
        /// Retificação de Dados do Empregador
        /// </summary>
        public int RDE { get; set; }

        /// <summary>
        /// Código Operacional CVM
        /// </summary>]
        public int CodigoCVM { get; set; }

        /// <summary>
        /// País de Origem
        /// </summary>       
        public string PaisDeOrigem { get; set; }

        /// <summary>
        /// Nome do administrador da carteira
        /// </summary>       
        public string NomeDoAdministrador { get; set; }

        /// <summary>
        /// Exclusão
        /// </summary>
        public bool Exclusao { get; set; }

        public ClienteInvestidorNaoResidenteInfo ToClienteInvestidorNaoResidenteInfo()
        {
            ClienteInvestidorNaoResidenteInfo lRetorno = new ClienteInvestidorNaoResidenteInfo();
            lRetorno.CdPaisOrigem = this.PaisDeOrigem;
            lRetorno.DsCodigoCvm = this.CodigoCVM;
            lRetorno.DsCustodiante = this.CustodianteNoPAS;
            lRetorno.DsNomeAdiministradorCarteira = this.NomeDoAdministrador;
            lRetorno.DsRde = this.RDE;
            lRetorno.DsRepresentanteLegal = this.Nome;
            lRetorno.IdCliente = this.ParentId;
            lRetorno.IdInvestidorNaoResidente = this.Id;
            return lRetorno;
        }
    }
}