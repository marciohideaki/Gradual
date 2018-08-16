using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteDadosBancarios : ITransporteJSON
    {
        #region | Propriedades

        public string Agencia { get; set; }

        public string AgenciaDigito { get; set; }

        public string Banco { get; set; }

        public string BancoNome { get; set; }

        public string ContaCorrente { get; set; }

        public string ContaDigito { get; set; }

        public bool Principal { get; set; }

        public string TipoConta { get; set; }

        public Nullable<Int32> Id { get; set; }

        public int ParentId { get; set; }

        public string TipoDeItem { get { return "Conta"; } }

        public bool Exclusao { get; set; }

        public string NomeTitular { get; set; }

        public string CPFTitular { get; set; }

        #endregion

        #region | Construtores

        public TransporteDadosBancarios() { }

        public TransporteDadosBancarios(ClienteBancoInfo pDadosBancarios, bool pExclusao)
        {
            this.Id = pDadosBancarios.IdBanco;
            this.Agencia = pDadosBancarios.DsAgencia;
            this.AgenciaDigito = pDadosBancarios.DsAgenciaDigito;
            this.Banco = pDadosBancarios.CdBanco;
            this.ContaCorrente = pDadosBancarios.DsConta;
            this.ContaDigito = pDadosBancarios.DsContaDigito;
            this.Principal = pDadosBancarios.StPrincipal;
            this.TipoConta = pDadosBancarios.TpConta;
            this.ParentId = pDadosBancarios.IdCliente;
            this.Exclusao = pExclusao;
            this.NomeTitular = pDadosBancarios.DsNomeTitular;
            this.CPFTitular = pDadosBancarios.DsCpfTitular;
        }

        public TransporteDadosBancarios(ClienteBancoInfo pDadosBancarios, bool pExclusao, List<SinacorListaInfo> pListaBancos)
        {
            this.Id = pDadosBancarios.IdBanco;
            this.Agencia = pDadosBancarios.DsAgencia;
            this.AgenciaDigito = pDadosBancarios.DsAgenciaDigito;
            this.Banco = pDadosBancarios.CdBanco;
            this.BancoNome = this.RecuperarNomeBanco(pDadosBancarios.CdBanco, pListaBancos);
            this.ContaCorrente = pDadosBancarios.DsConta;
            this.ContaDigito = pDadosBancarios.DsContaDigito;
            this.Principal = pDadosBancarios.StPrincipal;
            this.TipoConta = pDadosBancarios.TpConta;
            this.ParentId = pDadosBancarios.IdCliente;
            this.Exclusao = pExclusao;
            this.NomeTitular = pDadosBancarios.DsNomeTitular;
            this.CPFTitular = pDadosBancarios.DsCpfTitular;
        }

        #endregion

        #region | Métodos

        public ClienteBancoInfo ToClienteBancoInfo()
        {
            ClienteBancoInfo lRetorno = new ClienteBancoInfo();
            lRetorno.CdBanco = this.Banco;
            lRetorno.DsAgencia = this.Agencia;
            lRetorno.DsAgenciaDigito = this.AgenciaDigito;
            lRetorno.DsConta = this.ContaCorrente;
            lRetorno.DsContaDigito = this.ContaDigito;
            lRetorno.IdBanco = this.Id;
            lRetorno.IdCliente = this.ParentId;
            lRetorno.StPrincipal = this.Principal;
            lRetorno.TpConta = this.TipoConta;
            lRetorno.DsNomeTitular = this.NomeTitular;
            lRetorno.DsCpfTitular = this.CPFTitular.Replace("-", "").Replace(".", "");

            return lRetorno;
        }

        public List<TransporteDadosBancarios> TraduzirClienteBancoInfo(List<ClienteBancoInfo> pListaClienteBancoInfo, List<SinacorListaInfo> pListaBancos)
        {
            var lRetorno = new List<TransporteDadosBancarios>();

            if (null != pListaClienteBancoInfo && pListaClienteBancoInfo.Count > 0)
                pListaClienteBancoInfo.ForEach(
                    cli =>
                    {
                        lRetorno.Add(new TransporteDadosBancarios()
                        {
                            Agencia = cli.DsAgencia,
                            AgenciaDigito = cli.DsAgenciaDigito,
                            Banco = cli.CdBanco,
                            BancoNome = this.RecuperarNomeBanco(cli.CdBanco, pListaBancos),
                            ContaCorrente = cli.DsConta,
                            ContaDigito = cli.DsContaDigito,
                            Principal = cli.StPrincipal,
                            TipoConta = cli.TpConta,
                            NomeTitular = cli.DsNomeTitular,
                            CPFTitular = cli.DsCpfTitular

                        });
                    });


            return lRetorno;
        }

        private string RecuperarNomeBanco(string pCodigoBanco, List<SinacorListaInfo> pListaBancos)
        {
            var lRetorno = new SinacorListaInfo();

            if (null != pListaBancos && pListaBancos.Count > 0)
                lRetorno = pListaBancos.Find(ban => { return ban.Id == pCodigoBanco; });

            if (null == lRetorno)
                return string.Empty;

            return lRetorno.Value;
        }

        #endregion
    }
}
