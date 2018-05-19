using System;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteDadosCompletosClienteBase
    {

        public string NomeCliente { get; set; }

        public string CPF_CNPJ { get; set; }

        public bool Flag_Emancipado { get; set; }

        public bool Flag_PossuiRepresentante { get; set; }
        
        /// <summary>
        /// 0: Não é vinculada; 1: vinculada a outras corretoras; 2: vinculada à gradual
        /// </summary>
        public int Flag_PessoaVinculada { get; set; }

        public bool Flag_PPE { get; set; }

        public bool Flag_OperaPorContaPropria { get; set; }

        public bool Flag_CVM387 { get; set; }

        public bool Flag_AutorizaTransmissaoPorProcurador { get; set; }
        
        public bool USPerson                   { get; set; }
        public bool CienteRegulamento          { get; set; }
        public bool CienteProspecto            { get; set; }
        public bool CienteLamina               { get; set; }

        public string PropositoGradual           { get; set; }

        public string Id { get; set; }

        public string Email { get; set; }

        public string TipoCliente { get; set; }

        public int? IdLogin { get; set; }

        public string Tipo { get; set; }

        public TransporteDadosCompletosClienteBase()
        {
            //this.IdLogin = 4;
        }

        public TransporteDadosCompletosClienteBase(ClienteInfo pDadosDoCliente)
        {
            this.Id                        = pDadosDoCliente.IdCliente.DBToString();
            this.NomeCliente               = pDadosDoCliente.DsNome;
            this.CPF_CNPJ                  = pDadosDoCliente.DsCpfCnpj;
            this.Flag_CVM387               = pDadosDoCliente.StCVM387.DBToBoolean();
            this.Flag_PPE                  = pDadosDoCliente.StPPE.DBToBoolean();
            this.TipoCliente               = string.Concat("P", pDadosDoCliente.TpPessoa.ToString().ToUpper());
            this.Flag_Emancipado           = pDadosDoCliente.StEmancipado.DBToBoolean();
            this.Flag_OperaPorContaPropria = pDadosDoCliente.StCarteiraPropria.DBToBoolean();
            this.Email                     = pDadosDoCliente.DsEmail;
            this.Tipo                      = pDadosDoCliente.TpCliente.ToString();


            this.PropositoGradual = pDadosDoCliente.DsPropositoGradual;

            if (pDadosDoCliente.StCienteDocumentos.HasValue)
            {
                /// <summary>
                /// Regulamento, Prospecto, Lamina: 111 (7) RP_: 110 (6) R_L: 101 (5) R__: 100 (4) _PL: 011 (3) _P_: 010 (2) __L: 001
                /// </summary>
                if(pDadosDoCliente.StCienteDocumentos.Value == 7)
                {
                    this.CienteRegulamento = true;
                    this.CienteProspecto = true;
                    this.CienteLamina = true;
                }
                else if(pDadosDoCliente.StCienteDocumentos.Value == 6)
                {
                    this.CienteRegulamento = true;
                    this.CienteProspecto = true;
                    this.CienteLamina = false;
                }
                else if(pDadosDoCliente.StCienteDocumentos.Value == 5)
                {
                    this.CienteRegulamento = true;
                    this.CienteProspecto = false;
                    this.CienteLamina = true;
                }
                else if(pDadosDoCliente.StCienteDocumentos.Value == 4)
                {
                    this.CienteRegulamento = true;
                    this.CienteProspecto = false;
                    this.CienteLamina = false;
                }
                else if(pDadosDoCliente.StCienteDocumentos.Value == 3)
                {
                    this.CienteRegulamento = false;
                    this.CienteProspecto = true;
                    this.CienteLamina = true;
                }
                else if(pDadosDoCliente.StCienteDocumentos.Value == 2)
                {
                    this.CienteRegulamento = false;
                    this.CienteProspecto = true;
                    this.CienteLamina = false;
                }
                else if(pDadosDoCliente.StCienteDocumentos.Value == 1)
                {
                    this.CienteRegulamento = false;
                    this.CienteProspecto = false;
                    this.CienteLamina = true;
                }
            }
            
        }

        public virtual ClienteInfo ToClienteInfo()
        {
            ClienteInfo lRetorno = new ClienteInfo();

            if (string.IsNullOrEmpty(this.Id))
            {
                lRetorno.IdCliente = null;
            }
            else
            {
                lRetorno.IdCliente = int.Parse(this.Id);
            }


            lRetorno.DsNome = this.NomeCliente;

            lRetorno.DtUltimaAtualizacao = DateTime.Now;

            lRetorno.DsCpfCnpj = this.CPF_CNPJ;

            lRetorno.DsOrigemCadastro = "Cadastro Direto via Sistema de Administração Cadastral";

            lRetorno.TpPessoa = ((this.TipoCliente == "PF") ? 'F' : 'J');

            lRetorno.TpCliente = int.Parse(this.Tipo);
            
            lRetorno.StCVM387 = this.Flag_CVM387;

            lRetorno.StPPE = this.Flag_PPE;

            lRetorno.StCarteiraPropria = this.Flag_OperaPorContaPropria;

            lRetorno.IdLogin = this.IdLogin;

            lRetorno.DsEmail = this.Email;

            

            return lRetorno;
        }
    }
}
