#region Includes
using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Gradual.Intranet.Contratos.Dados;
using System.Collections.Generic;
#endregion

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{


    public class TransporteAtivarInativarClienteContas
    {
        public int Codigo { get; set; }
        public bool ExisteBovespa { get; set; }
        public bool Bovespa { get; set; }
        public bool ExisteBmf { get; set; }
        public bool Bmf { get; set; }
        public bool Custodia { get; set; }
        public bool ExisteCustodia { get; set; }
        public bool CC { get; set; }
        public bool ExisteCC { get; set; }
        public bool Investimento { get; set; }

        public string GetCkbBovespa
        {
            get
            {
                string lRetorno = "";
                if (this.ExisteBovespa)
                {
                    if (this.Bovespa)
                    {
                        lRetorno += "<input type='checkbox' id='ckbBov" + this.Codigo.ToString() + "' checked='checked' />";
                    }
                    else
                    {
                        lRetorno += "<input type='checkbox' id='ckbBov" + this.Codigo.ToString() + "' />";
                    }
                    lRetorno += "<label for='ckbBov" + this.Codigo.ToString() + "'>&nbsp;</label>";
                    if (this.Investimento)
                        lRetorno += "<label>Investimento</label>";
                }
                else
                {
                    lRetorno += "&nbsp;";
                }
                return lRetorno;
            }
        }
        public string GetCkbBmf
        {
            get
            {
                string lRetorno = "";
                if (this.ExisteBmf)
                {
                    if (this.Bmf)
                    {
                        lRetorno += "<input type='checkbox' id='ckbBmf" + this.Codigo.ToString() + "' checked='checked' />";
                    }
                    else
                    {
                        lRetorno += "<input type='checkbox' id='ckbBmf" + this.Codigo.ToString() + "' />";
                    }
                    lRetorno += "<label for='ckbBmf" + this.Codigo.ToString() + "'>&nbsp;</label>";
                    if (this.Investimento)
                        lRetorno += "<label>Investimento</label>";
                }
                else
                {
                    lRetorno += "&nbsp;";
                }
                return lRetorno;
            }
        }
        public string GetCkbCc
        {
            get
            {
                string lRetorno = "";
                if (this.ExisteCC)
                {
                    if (this.CC)
                    {
                        lRetorno += "<input type='checkbox' id='ckbCc" + this.Codigo.ToString() + "' checked='checked' />";
                    }
                    else
                    {
                        lRetorno += "<input type='checkbox' id='ckbCc" + this.Codigo.ToString() + "' />";
                    }

                    lRetorno += "<label for='ckbCc" + this.Codigo.ToString() + "'>&nbsp;</label>";
                    if (this.Investimento)
                        lRetorno += "<label>Investimento</label>";
                }
                else
                {
                    lRetorno += "&nbsp;";
                }
                return lRetorno;
            }
        }
        public string GetCkbCustodia
        {
            get
            {
                string lRetorno = "";
                if (this.ExisteCustodia)
                {
                    if (this.Custodia)
                    {
                        lRetorno += "<input type='checkbox' id='ckbCus" + this.Codigo.ToString() + "' checked='checked' />";
                    }
                    else
                    {
                        lRetorno += "<input type='checkbox' id='ckbCus" + this.Codigo.ToString() + "' />";
                    }
                    lRetorno += "<label for='ckbCus" + this.Codigo.ToString() + "'>&nbsp;</label>";
                    if (this.Investimento)
                        lRetorno += "<label>Investimento</label>";
                }
                else
                {
                    lRetorno += "&nbsp;";
                }
                return lRetorno;
            }
        }


    }

    public class TransporteAtivarInativarCliente
    {
        public int IdCliente { get; set; }
        public bool LoginAtivo { get; set; }
        public bool CliGerAtivo { get; set; }
        public bool HbAtivo { get; set; }
        public string DataUltimaAtualizacao { get; set; }
        public List<TransporteAtivarInativarClienteContas> Contas { get; set; }


        public TransporteAtivarInativarCliente() { }

        public TransporteAtivarInativarCliente(ClienteAtivarInativarInfo pInfo)
        {
            this.IdCliente = pInfo.IdCliente;
            this.LoginAtivo = pInfo.StLoginAtivo;
            this.CliGerAtivo = pInfo.StClienteGeralAtivo;
            this.HbAtivo = pInfo.StHbAtivo;
            this.DataUltimaAtualizacao = pInfo.DtUltimaAtualizacao.ToString("dd/MM/yyyy hh:mm");
            this.Contas = new List<TransporteAtivarInativarClienteContas>();
            foreach (ClienteAtivarInativarContasInfo item in pInfo.Contas)
            {
                TransporteAtivarInativarClienteContas lConta = new TransporteAtivarInativarClienteContas();
                lConta.Codigo = item.CdCodigo;
                if (null == item.Bovespa)
                {
                    lConta.ExisteBovespa = false;
                }
                else
                {
                    lConta.ExisteBovespa = true;
                    lConta.Bovespa = item.Bovespa.StAtiva;
                    lConta.Investimento = item.Bovespa.StContaInvestimento;
                }
                if (null == item.Bmf)
                {
                    lConta.ExisteBmf = false;
                }
                else
                {
                    lConta.ExisteBmf = true;
                    lConta.Bmf = item.Bmf.StAtiva;
                    lConta.Investimento = item.Bmf.StContaInvestimento;
                }
                if (null == item.CC)
                {
                    lConta.ExisteCC = false;
                }
                else
                {
                    lConta.ExisteCC = true;
                    lConta.CC = item.CC.StAtiva;
                    lConta.Investimento = item.CC.StContaInvestimento;
                }
                if (null == item.Custodia)
                {
                    lConta.ExisteCustodia = false;
                }
                else
                {
                    lConta.ExisteCustodia = true;
                    lConta.Custodia = item.Custodia.StAtiva;
                    lConta.Investimento = item.Custodia.StContaInvestimento;
                }
                this.Contas.Add(lConta);
            }
        }

        public ClienteAtivarInativarInfo ToClienteAtivarInativarInfo()
        {
            ClienteAtivarInativarInfo lRetorno = new ClienteAtivarInativarInfo();

            lRetorno.IdCliente = this.IdCliente;
            lRetorno.StLoginAtivo = this.LoginAtivo;
            lRetorno.StClienteGeralAtivo = this.CliGerAtivo;
            lRetorno.StHbAtivo = this.HbAtivo;
            lRetorno.DtUltimaAtualizacao = DateTime.Now;
            lRetorno.Contas = new List<ClienteAtivarInativarContasInfo>();
            foreach (TransporteAtivarInativarClienteContas item in this.Contas)
            {
                ClienteAtivarInativarContasInfo lConta = new ClienteAtivarInativarContasInfo();
                lConta.CdCodigo = item.Codigo;
                lConta.Bovespa.StAtiva = item.Bovespa;
                if (item.ExisteBmf)
                    lConta.Bmf.StAtiva = item.Bmf;
                if (item.ExisteCC)
                    lConta.CC.StAtiva = item.CC;
                if (item.ExisteCustodia)
                    lConta.Custodia.StAtiva = item.Custodia;
                lRetorno.Contas.Add(lConta);
            }
            return lRetorno;
        }


    }
}