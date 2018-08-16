using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteClienteAutorizacaoCadastral
    {
        #region Members

        public string IdAutorizacao { get; set; }
        
        public string IdCliente { get; set; }
        public string CodigoBov { get; set; }

        public string NomeCliente { get; set; }
        public string DataExportacao { get; set; }
        public string CPF { get; set; }
        public string Passo { get; set; }

        public string IdLogin_D1 { get; set; }
        public string DataAutorizacao_D1 { get; set; }
        public string Nome_D1 { get; set; }
        public string Email_D1 { get; set; }

        public string Botao_D1 { get; set; }

        public string IdLogin_D2 { get; set; }
        public string DataAutorizacao_D2 { get; set; }
        public string Nome_D2 { get; set; }
        public string Email_D2 { get; set; }

        public string Botao_D2 { get; set; }

        public string IdLogin_P1 { get; set; }
        public string DataAutorizacao_P1 { get; set; }
        public string Nome_P1 { get; set; }
        public string Email_P1 { get; set; }

        public string Botao_P1 { get; set; }

        public string IdLogin_P2 { get; set; }
        public string DataAutorizacao_P2 { get; set; }
        public string Nome_P2 { get; set; }
        public string Email_P2 { get; set; }

        public string Botao_P2 { get; set; }

        public string IdLogin_T1 { get; set; }
        public string DataAutorizacao_T1 { get; set; }
        public string Nome_T1 { get; set; }
        public string Email_T1 { get; set; }

        public string Botao_T1 { get; set; }

        public string IdLogin_T2 { get; set; }
        public string DataAutorizacao_T2 { get; set; }
        public string Nome_T2 { get; set; }
        public string Email_T2 { get; set; }

        public string Botao_T2 { get; set; }

        public string StAutorizado { get; set; }

        public string TituloCelula_D1 { get { return string.IsNullOrEmpty(this.Nome_D1) ? "" : string.Format("{0} - {1}", this.Nome_D1, this.Email_D1); } }
        public string TituloCelula_D2 { get { return string.IsNullOrEmpty(this.Nome_D2) ? "" : string.Format("{0} - {1}", this.Nome_D2, this.Email_D2); } }

        public string TituloCelula_P1 { get { return string.IsNullOrEmpty(this.Nome_P1) ? "" : string.Format("{0} - {1}", this.Nome_P1, this.Email_P1); } }
        public string TituloCelula_P2 { get { return string.IsNullOrEmpty(this.Nome_P2) ? "" : string.Format("{0} - {1}", this.Nome_P2, this.Email_P2); } }

        public string TituloCelula_T1 { get { return string.IsNullOrEmpty(this.Nome_T1) ? "" : string.Format("{0} - {1}", this.Nome_T1, this.Email_T1); } }
        public string TituloCelula_T2 { get { return string.IsNullOrEmpty(this.Nome_T2) ? "" : string.Format("{0} - {1}", this.Nome_T2, this.Email_T2); } }

        #endregion

        #region Constructor

        public TransporteClienteAutorizacaoCadastral() { }

        public TransporteClienteAutorizacaoCadastral(ClienteAutorizacaoInfo pInfo, bool pPodeAutorizaComoDiretor1, bool pPodeAutorizaComoDiretor2, bool pPodeAutorizaComoProcurador1, bool pPodeAutorizaComoProcurador2, bool pPodeAutorizaComoTestemunha1, bool pPodeAutorizaComoTestemunha2)
        {
            if (pInfo.IdAutorizacao.HasValue)
            {
                this.IdAutorizacao = "";
            }

            this.IdCliente = pInfo.IdCliente.DBToString();

            this.CodigoBov = pInfo.CodigoBov;

            this.NomeCliente = pInfo.NomeCliente;
            this.DataExportacao = pInfo.DataExportacao.ToString("dd/MM/yy HH:mm");
            this.CPF = pInfo.CPF;
            this.Passo = pInfo.Passo;

            if (pInfo.IdLogin_D1.HasValue)
            {
                this.IdLogin_D1 = pInfo.IdLogin_D1.Value.DBToString();
                this.DataAutorizacao_D1 = pInfo.DataAutorizacao_D1.Value.ToString("dd/MM/yy HH:mm");
                this.Nome_D1 = pInfo.Nome_D1;
                this.Email_D1 = pInfo.Email_D1;

                this.Botao_D1 = this.DataAutorizacao_D1;
            }
            else
            {
                if (pPodeAutorizaComoDiretor1)
                {
                    this.Botao_D1 = string.Format("<input id='chkAut_{0}_11' type='checkbox' onclick='return chkAut_Click(this)' /><label for='chkAut_{0}_11'>&nbsp;</label>", this.IdCliente);
                }
            }

            if (pInfo.IdLogin_D2.HasValue)
            {
                this.IdLogin_D2 = pInfo.IdLogin_D2.Value.DBToString();
                this.DataAutorizacao_D2 = pInfo.DataAutorizacao_D2.Value.ToString("dd/MM/yy HH:mm");
                this.Nome_D2 = pInfo.Nome_D2;
                this.Email_D2 = pInfo.Email_D2;

                this.Botao_D2 = this.DataAutorizacao_D2;
            }
            else
            {
                if (pPodeAutorizaComoDiretor2)
                {
                    this.Botao_D2 = string.Format("<input id='chkAut_{0}_12' type='checkbox' onclick='return chkAut_Click(this)' /><label for='chkAut_{0}_12'>&nbsp;</label>", this.IdCliente);
                }
            }

            if (pInfo.IdLogin_P1.HasValue)
            {
                this.IdLogin_P1 = pInfo.IdLogin_P1.Value.DBToString();
                this.DataAutorizacao_P1 = pInfo.DataAutorizacao_P1.Value.ToString("dd/MM/yy HH:mm");
                this.Nome_P1 = pInfo.Nome_P1;
                this.Email_P1 = pInfo.Email_P1;

                this.Botao_P1 = this.DataAutorizacao_P1;
            }
            else
            {
                if (pPodeAutorizaComoProcurador1)
                {
                    this.Botao_P1 = string.Format("<input id='chkAut_{0}_21' type='checkbox' onclick='return chkAut_Click(this)' /><label for='chkAut_{0}_21'>&nbsp;</label>", this.IdCliente);
                }
            }

            if (pInfo.IdLogin_P2.HasValue)
            {
                this.IdLogin_P2 = pInfo.IdLogin_P2.Value.DBToString();
                this.DataAutorizacao_P2 = pInfo.DataAutorizacao_P2.Value.ToString("dd/MM/yy HH:mm");
                this.Nome_P2 = pInfo.Nome_P2;
                this.Email_P2 = pInfo.Email_P2;

                this.Botao_P2 = this.DataAutorizacao_P2;
            }
            else
            {
                if (pPodeAutorizaComoProcurador2)
                {
                    this.Botao_P2 = string.Format("<input id='chkAut_{0}_22' type='checkbox' onclick='return chkAut_Click(this)' /><label for='chkAut_{0}_22'>&nbsp;</label>", this.IdCliente);
                }
            }

            if (pInfo.IdLogin_T1.HasValue)
            {
                this.IdLogin_T1 = pInfo.IdLogin_T1.Value.DBToString();
                this.DataAutorizacao_T1 = pInfo.DataAutorizacao_T1.Value.ToString("dd/MM/yy HH:mm");
                this.Nome_T1 = pInfo.Nome_T1;
                this.Email_T1 = pInfo.Email_T1;

                this.Botao_T1 = this.DataAutorizacao_T1;
            }
            else
            {
                if (pPodeAutorizaComoTestemunha1)
                {
                    this.Botao_T1 = string.Format("<input id='chkAut_{0}_31' type='checkbox' onclick='return chkAut_Click(this)' /><label for='chkAut_{0}_31'>&nbsp;</label>", this.IdCliente);
                }
            }

            if (pInfo.IdLogin_T2.HasValue)
            {
                this.IdLogin_T2 = pInfo.IdLogin_T2.Value.DBToString();
                this.DataAutorizacao_T2 = pInfo.DataAutorizacao_T2.Value.ToString("dd/MM/yy HH:mm");
                this.Nome_T2 = pInfo.Nome_T2;
                this.Email_T2 = pInfo.Email_T2;

                this.Botao_T2 = this.DataAutorizacao_T2;
            }
            else
            {
                if (pPodeAutorizaComoTestemunha2)
                {
                    this.Botao_T2 = string.Format("<input id='chkAut_{0}_32' type='checkbox' onclick='return chkAut_Click(this)' /><label for='chkAut_{0}_32'>&nbsp;</label>", this.IdCliente);
                }
            }

            this.StAutorizado = pInfo.StAutorizado;
        }

        #endregion
    }
}