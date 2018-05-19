using System;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Www.App_Codigo.Excessoes;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteDadosCompletosPJ : TransporteDadosCompletosClienteBase
    {
        public string DataDeConstituicao { get; set; }
        
        public Nullable<Int64> NIRE { get; set; }
        
        public string NomeFantasia { get; set; }

        public int ObjetivoSocial { get; set; }

        public string Assessor { get; set; }

        public int RamoAtividade { get; set; }

        public string FormaConstituicao { get; set;}

        public bool Interdito { get; set; }

        public bool SituacaoLegalOutros { get; set; }

        public TransporteDadosCompletosPJ() : base() { }

        public string PrincipalAtividade { get; set; }

        public string Pais { get; set; }

        /* preciso das propriedades pra dar o "bind" pelo javascript, mas C# não tem herança múltipla então vai herança ctrl+V */
        
        public bool Flag_USPersonNacional { get; set; }
        public string USPersonNacional_Nome { get; set; }
        public string USPersonNacional_Nacionalidades { get; set; }
        
        public bool Flag_USPersonResidente { get; set; }
        public string USPersonResidente_Nome { get; set; }

        public bool Flag_USPersonGreen { get; set; }
        public bool Flag_USPersonPresenca { get; set; }
        public bool Flag_USPersonNascido { get; set; }

        public string USPersonRenuncia_Motivo { get; set; }
        public string USPersonRenuncia_Documento { get; set; }

        public string DesejaAplicar { get; set; }

        public string DetalhesUSPerson
        {
            get
            {
                TransporteDetalhesUSPersonPJ lDetalhes = new TransporteDetalhesUSPersonPJ();

                lDetalhes.Flag_USPersonNacional = this.Flag_USPersonNacional;
                lDetalhes.USPersonNacional_Nome = this.USPersonNacional_Nome;
                lDetalhes.USPersonNacional_Nacionalidades = this.USPersonNacional_Nacionalidades;
                lDetalhes.Flag_USPersonResidente = this.Flag_USPersonResidente;
                lDetalhes.USPersonResidente_Nome = this.USPersonResidente_Nome;
                lDetalhes.Flag_USPersonGreen = this.Flag_USPersonGreen;
                lDetalhes.Flag_USPersonPresenca = this.Flag_USPersonPresenca;
                lDetalhes.Flag_USPersonNascido = this.Flag_USPersonNascido;
                lDetalhes.USPersonRenuncia_Motivo = this.USPersonRenuncia_Motivo;
                lDetalhes.USPersonRenuncia_Documento = this.USPersonRenuncia_Documento;

                return JsonConvert.SerializeObject(lDetalhes);
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    TransporteDetalhesUSPersonPJ lTrans = JsonConvert.DeserializeObject<TransporteDetalhesUSPersonPJ>(value);

                    this.Flag_USPersonNacional = lTrans.Flag_USPersonNacional;
                    this.USPersonNacional_Nome = lTrans.USPersonNacional_Nome;
                    this.USPersonNacional_Nacionalidades = lTrans.USPersonNacional_Nacionalidades;
                    this.Flag_USPersonResidente = lTrans.Flag_USPersonResidente;
                    this.USPersonResidente_Nome = lTrans.USPersonResidente_Nome;
                    this.Flag_USPersonGreen = lTrans.Flag_USPersonGreen;
                    this.Flag_USPersonPresenca = lTrans.Flag_USPersonPresenca;
                    this.Flag_USPersonNascido = lTrans.Flag_USPersonNascido;
                    this.USPersonRenuncia_Motivo = lTrans.USPersonRenuncia_Motivo;
                    this.USPersonRenuncia_Documento = lTrans.USPersonRenuncia_Documento;
                }
                else
                {
                    this.Flag_USPersonNacional = false;
                    this.USPersonNacional_Nome = string.Empty;
                    this.USPersonNacional_Nacionalidades = string.Empty;
                    this.Flag_USPersonResidente = false;
                    this.USPersonResidente_Nome = string.Empty;
                    this.Flag_USPersonGreen = false;
                    this.Flag_USPersonPresenca = false;
                    this.Flag_USPersonNascido = false;
                    this.USPersonRenuncia_Motivo = string.Empty;
                    this.USPersonRenuncia_Documento = string.Empty;
                }
            }
        }

        public TransporteDadosCompletosPJ(ClienteInfo pDadosDoCliente) : base(pDadosDoCliente)
        {
            this.DataDeConstituicao  = pDadosDoCliente.DtNascimentoFundacao.Value.ToString("dd/MM/yyyy");
            this.FormaConstituicao   = pDadosDoCliente.DsFormaConstituicao;
            this.Interdito           = pDadosDoCliente.StInterdito.Value;
            this.NIRE                = pDadosDoCliente.CdNire.HasValue ?  pDadosDoCliente.CdNire.Value : new Nullable<Int64>();
            this.NomeFantasia        = pDadosDoCliente.DsNomeFantasia;
            this.Assessor            = pDadosDoCliente.IdAssessorInicial.DBToString();
            //this.ObjetivoSocial    = 0;
            this.RamoAtividade       = pDadosDoCliente.CdProfissaoAtividade.Value;
            this.SituacaoLegalOutros = pDadosDoCliente.StSituacaoLegalOutros.Value;
            this.PrincipalAtividade  = pDadosDoCliente.CdAtividadePrincipal.ToString();
            this.Email               = pDadosDoCliente.DsEmail;
            this.IdLogin             = pDadosDoCliente.IdLogin;
            this.Pais                = pDadosDoCliente.CdPaisNascimento;
            this.DetalhesUSPerson    = pDadosDoCliente.DsUSPersonPJDetalhes;

            this.DesejaAplicar = pDadosDoCliente.TpDesejaAplicar;
        }

        public ClienteInfo ToClienteInfo(ClienteInfo pParametro)
        {
            pParametro.DsNome                = this.NomeCliente;
            pParametro.DsNomeFantasia        = this.NomeFantasia;
            pParametro.IdAssessorInicial     = this.Assessor.DBToInt32();
            pParametro.CdNire                = this.NIRE;
            pParametro.CdAtividadePrincipal  = int.Parse(this.PrincipalAtividade);
            pParametro.DsFormaConstituicao   = this.FormaConstituicao;
            pParametro.StInterdito           = this.Interdito;
            pParametro.StSituacaoLegalOutros = this.SituacaoLegalOutros;
            pParametro.DtNascimentoFundacao  = this.DataDeConstituicao.DBToDateTime();
            pParametro.CdProfissaoAtividade  = this.RamoAtividade;
            pParametro.DsEmail               = this.Email;
            pParametro.IdLogin               = this.IdLogin;
            pParametro.TpPessoa = 'J';
            pParametro.TpCliente =int.Parse( this.Tipo);
            pParametro.CdPaisNascimento = this.Pais;

            
            /// <summary>
            /// Regulamento, Prospecto, Lamina: 111 (7) RP_: 110 (6) R_L: 101 (5) R__: 100 (4) _PL: 011 (3) _P_: 010 (2) __L: 001
            /// </summary>
            if (this.CienteRegulamento == true && this.CienteProspecto == true && this.CienteLamina == true)
            {
                pParametro.StCienteDocumentos = 7;
            }
            else if (this.CienteRegulamento == true && this.CienteProspecto == true && this.CienteLamina == false)
            {
                pParametro.StCienteDocumentos = 6;
            }
            else if (this.CienteRegulamento == true && this.CienteProspecto == false && this.CienteLamina == true)
            {
                pParametro.StCienteDocumentos = 5;
            }
            else if (this.CienteRegulamento == true && this.CienteProspecto == false && this.CienteLamina == false)
            {
                pParametro.StCienteDocumentos = 4;
            }
            else if (this.CienteRegulamento == false && this.CienteProspecto == true && this.CienteLamina == true)
            {
                pParametro.StCienteDocumentos = 3;
            }
            else if (this.CienteRegulamento == false && this.CienteProspecto == true && this.CienteLamina == false)
            {
                pParametro.StCienteDocumentos = 2;
            }
            else if (this.CienteRegulamento == false && this.CienteProspecto == false && this.CienteLamina == true)
            {
                pParametro.StCienteDocumentos = 1;
            }

            pParametro.DsPropositoGradual = this.PropositoGradual.ToUpper();

            pParametro.DsUSPersonPJDetalhes = this.DetalhesUSPerson;

            pParametro.TpDesejaAplicar = this.DesejaAplicar;

            return pParametro;
        }

        public override Gradual.Intranet.Contratos.Dados.ClienteInfo ToClienteInfo()
        {
            try
            {
                ClienteInfo lRetorno           = base.ToClienteInfo();
                
                lRetorno.DsNome                = this.NomeCliente;
                lRetorno.DsNomeFantasia        = this.NomeFantasia;
                lRetorno.IdAssessorInicial     = this.Assessor.DBToInt32();
                lRetorno.CdNire                = this.NIRE;
                lRetorno.CdAtividadePrincipal  = int.Parse(this.PrincipalAtividade);
                lRetorno.DsFormaConstituicao   = this.FormaConstituicao;
                lRetorno.StInterdito           = this.Interdito;
                lRetorno.StSituacaoLegalOutros = this.SituacaoLegalOutros;
                lRetorno.DtNascimentoFundacao  = this.DataDeConstituicao.DBToDateTime();
                lRetorno.CdProfissaoAtividade  = this.RamoAtividade;
                lRetorno.DsEmail               = this.Email;
                lRetorno.IdLogin               = this.IdLogin;
                lRetorno.TpPessoa              = 'J';
                lRetorno.TpCliente             =int.Parse( this.Tipo);
                lRetorno.DsSenhaGerada         = PaginaBase.GerarSenha();
                lRetorno.CdPaisNascimento      = this.Pais;
                lRetorno.TpDesejaAplicar       = this.DesejaAplicar;
                /// <summary>
                /// Regulamento, Prospecto, Lamina: 111 (7) RP_: 110 (6) R_L: 101 (5) R__: 100 (4) _PL: 011 (3) _P_: 010 (2) __L: 001
                /// </summary>
                if (this.CienteRegulamento == true && this.CienteProspecto == true && this.CienteLamina == true)
                {
                    lRetorno.StCienteDocumentos = 7;
                }
                else if (this.CienteRegulamento == true && this.CienteProspecto == true && this.CienteLamina == false)
                {
                    lRetorno.StCienteDocumentos = 6;
                }
                else if (this.CienteRegulamento == true && this.CienteProspecto == false && this.CienteLamina == true)
                {
                    lRetorno.StCienteDocumentos = 5;
                }
                else if (this.CienteRegulamento == true && this.CienteProspecto == false && this.CienteLamina == false)
                {
                    lRetorno.StCienteDocumentos = 4;
                }
                else if (this.CienteRegulamento == false && this.CienteProspecto == true && this.CienteLamina == true)
                {
                    lRetorno.StCienteDocumentos = 3;
                }
                else if (this.CienteRegulamento == false && this.CienteProspecto == true && this.CienteLamina == false)
                {
                    lRetorno.StCienteDocumentos = 2;
                }
                else if (this.CienteRegulamento == false && this.CienteProspecto == false && this.CienteLamina == true)
                {
                    lRetorno.StCienteDocumentos = 1;
                }

                lRetorno.DsPropositoGradual = this.PropositoGradual.ToUpper();

                lRetorno.DsUSPersonPJDetalhes = this.DetalhesUSPerson;

                return lRetorno;
            }
            catch (Exception ex)
            {
                throw new ExcessaoConverterParaClienteInfo(ex);
            }
        }
    }

    public class TransporteDetalhesUSPersonPJ
    {
        public bool Flag_USPersonNacional { get; set; }
        public string USPersonNacional_Nome { get; set; }
        public string USPersonNacional_Nacionalidades { get; set; }
        
        public bool Flag_USPersonResidente { get; set; }
        public string USPersonResidente_Nome { get; set; }

        public bool Flag_USPersonGreen { get; set; }
        public bool Flag_USPersonPresenca { get; set; }
        public bool Flag_USPersonNascido { get; set; }

        public string USPersonRenuncia_Motivo { get; set; }
        public string USPersonRenuncia_Documento { get; set; }
    }
}
