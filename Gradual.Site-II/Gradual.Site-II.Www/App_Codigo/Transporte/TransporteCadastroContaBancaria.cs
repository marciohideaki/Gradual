using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Site.Www
{
    public class TransporteCadastroContaBancaria
    {
        #region Propriedades

        public int? IdConta   { get; set; }
        public int  IdCliente { get; set; }

        public string Banco { get; set; }
        public string BancoDesc { get; set; }

        public string Agencia       { get; set; }
        public string AgenciaDigito { get; set; }

        public string Conta         { get; set; }
        public string ContaDigito   { get; set; }

        public string Principal { get; set; }
        
        public string TipoConta     { get; set; }
        public string TipoContaDesc { get; set; }

        public bool Titular { get; set; }

        public string NomeTitular { get; set; }
        public string CPFTitular  { get; set; }

        #endregion

        #region Construtores

        public TransporteCadastroContaBancaria() { }

        public TransporteCadastroContaBancaria(ClienteBancoInfo pInfo)
        {
            this.IdConta = pInfo.IdBanco;
            this.IdCliente = pInfo.IdCliente;

            this.Banco = pInfo.CdBanco;
            this.BancoDesc = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.Banco, this.Banco);

            this.Agencia = pInfo.DsAgencia;
            this.AgenciaDigito = pInfo.DsAgenciaDigito;

            this.Conta = pInfo.DsConta;
            this.ContaDigito = pInfo.DsContaDigito;

            this.Principal = (pInfo.StPrincipal) ? "Sim" : "Não";

            this.NomeTitular = pInfo.DsNomeTitular;
            this.CPFTitular = pInfo.DsCpfTitular;

            this.TipoConta = pInfo.TpConta;
            this.TipoContaDesc = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.TipoConta, this.TipoConta);
        }

        #endregion

        #region Métodos Públicos

        public ClienteBancoInfo ToClienteBancoInfo()
        {
            ClienteBancoInfo lRetorno = new ClienteBancoInfo();

            if (this.IdConta.HasValue)
            {
                lRetorno.IdBanco = this.IdConta;
            }

            lRetorno.IdCliente = lRetorno.IdCliente;

            lRetorno.CdBanco = this.Banco;

            lRetorno.DsAgencia = this.Agencia;
            lRetorno.DsAgenciaDigito = this.AgenciaDigito;

            lRetorno.DsConta = this.Conta;

            if (this.ContaDigito.ToUpper() == "X")
            {
                this.ContaDigito = "0";
            }

            lRetorno.DsContaDigito = this.ContaDigito;

            lRetorno.StPrincipal = (this.Principal == "Sim");
            lRetorno.TpConta = this.TipoConta;

            lRetorno.DsNomeTitular = this.NomeTitular;
            lRetorno.DsCpfTitular = this.CPFTitular;

            return lRetorno;
        }

        #endregion
    }
}