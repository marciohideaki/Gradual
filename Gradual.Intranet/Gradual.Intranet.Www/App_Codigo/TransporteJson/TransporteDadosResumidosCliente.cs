using System;
using Gradual.Intranet.Contratos.Dados;
using Newtonsoft.Json;
using System.Collections.Generic;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Contratos;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteDadosResumidosCliente
    {
        #region Propriedades

        public double Id { get; set; }

        public string CodAssessor { get; set; }

        public string CodGradual { get; set; }

        public string CodBovespa { get; set; }

        public string CodBovespaComConta { get; set; }

        public string CodBovespaAtiva { get; set; }

        public string CodBMFAtiva { get; set; }

        public string CodBMF { get; set; }
        
        public string CodBMFComConta { get; set; }

        public string NomeCliente { get; set; }

        public string CPF { get; set; }

        public string Status { get; set; }

        public string Passo { get; set; }

        public string Cise { get; set; }

        [JsonIgnore]
        public DateTime DataCadastro { get; set; }

        public string DataCadastroString { get { return this.DataCadastro.ToString("dd/MM/yyyy"); } }

        [JsonIgnore]
        public DateTime DataRecadastro { get; set; }

        public string DataRecadastroString { get { return this.DataRecadastro.ToString("dd/MM/yyyy"); } }

        public string FlagPendencia { get; set; }

        [JsonIgnore]
        public DateTime DataNascimento { get; set; }

        public string DataNascimentoString { get { return this.DataNascimento.ToString("dd/MM/yyyy"); } }

        public string Email { get; set; }

        public string Sexo { get; set; }

        /// <summary>
        /// PF: Pessoa Física, PJ: Pessoa Jurídica
        /// </summary>
        public string TipoCliente { get; set; }

        #endregion

        #region Construtores

        public TransporteDadosResumidosCliente() { }

        public TransporteDadosResumidosCliente(ClienteResumidoInfo pClienteResumidoInfo) 
        { 
            this.Id = pClienteResumidoInfo.IdCliente;

            this.CodAssessor = pClienteResumidoInfo.CodAssessor.ToString();

            this.CodGradual = pClienteResumidoInfo.CodGradual;

            this.CodBovespa = pClienteResumidoInfo.CodBovespa;

            this.CodBovespaComConta = pClienteResumidoInfo.CodBovespaComConta;
            
            this.CodBMF = pClienteResumidoInfo.CodBMF ;

            this.CodBMFComConta = pClienteResumidoInfo.CodBMFComConta ;

            this.CodBMFAtiva = pClienteResumidoInfo.CodBMFAtiva;

            this.CodBovespaAtiva = pClienteResumidoInfo.CodBovespaAtiva;

            this.NomeCliente = pClienteResumidoInfo.NomeCliente;

            this.CPF = pClienteResumidoInfo.CPF;

            this.Status = pClienteResumidoInfo.Status;

            this.Passo = pClienteResumidoInfo.Passo;

            this.DataCadastro = pClienteResumidoInfo.DataCadastro;

            this.DataRecadastro = this.DefinirDataRenovacaoCadastral(pClienteResumidoInfo);

            this.FlagPendencia = pClienteResumidoInfo.FlagPendencia;

            this.DataNascimento = pClienteResumidoInfo.DataNascimento;

            this.Email = pClienteResumidoInfo.Email;

            this.Sexo = pClienteResumidoInfo.Sexo;

            this.TipoCliente = pClienteResumidoInfo.TipoCliente;

            this.Cise = pClienteResumidoInfo.Cise;
        }

        #endregion

        #region Métodos

        private DateTime DefinirDataRenovacaoCadastral(ClienteResumidoInfo pClienteResumidoInfo)
        {
            var lAtivador = Ativador.Get<IServicoPersistenciaCadastro>();

            var lRetorno = lAtivador.ReceberEntidadeCadastro<ClienteRenovacaoCadastralInfo>(new Contratos.Mensagens.ReceberEntidadeCadastroRequest<ClienteRenovacaoCadastralInfo>()
            {
                EntidadeCadastro = new ClienteRenovacaoCadastralInfo() 
                {
                    DsCpfCnpj = pClienteResumidoInfo.CPF 
                } 
            });

            return lRetorno.EntidadeCadastro.DtRenovacao;
        }

        #endregion
    }
}
