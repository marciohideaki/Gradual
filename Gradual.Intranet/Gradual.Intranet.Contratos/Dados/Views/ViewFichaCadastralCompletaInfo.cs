#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
#endregion

namespace Gradual.Intranet.Contratos.Dados.Views
{
    public class ViewFichaCadastralCompletaInfo : ICodigoEntidade
    {
        #region Propriedades

        public int IdDoCliente { get; set; }

        public string DadosBasicos_NomeCompleto { get; set; }

        public string DadosBasicos_Email { get; set; }

        public string DadosBasicos_CodigoDUC { get; set; }

        public string DadosBasicos_DataDoCadastro { get; set; }

        public string DadosBasicos_ContaGradual { get; set; }

        public string DadosBasicos_LiberadoParaOperar { get; set; }

        public string DadosBasicos_Assessor { get; set; }

        public string DadosBasicos_Filial { get; set; }

        public string DadosBasicos_DataDeNascimento { get; set; }

        public string DadosBasicos_EstadoDeNascimento { get; set; }

        public string DadosBasicos_CidadeDeNascimento { get; set; }

        public string DadosBasicos_NomeDoPai { get; set; }

        public string DadosBasicos_NomeDaMae { get; set; }

        public string DadosBasicos_Sexo { get; set; }

        public string DadosBasicos_EstadoCivil { get; set; }

        public string DadosBasicos_Conjuge { get; set; }

        public string DadosBasicos_Escolaridade { get; set; }

        public string DadosBasicos_CPF { get; set; }

        public string DadosBasicos_TipoDeDocumento { get; set; }

        public string DadosBasicos_OrgaoUfDeEmissao { get; set; }

        public string DadosBasicos_Numero { get; set; }

        public string DadosBasicos_DataDeEmissao { get; set; }

        public string DadosBasicos_TipoPessoa { get; set; }
        
        public string DadosBasicos_DesejaAplicar { get; set; }

        public string InformacoesComerciais_Empresa { get; set; }

        public string InformacoesComerciais_Profissao { get; set; }

        public string InformacoesComerciais_CargoAtualOuFuncao { get; set; }

        public string InformacoesComerciais_Email { get; set; }

        /// <summary>
        /// Todos os endereços já formatados com \r\n
        /// </summary>
        public string DadosParaContato_Enderecos { get; set; }

        public string DadosParaContato_Telefones { get; set; }

        public string DadosBancarios_Contas { get; set; }

        public string InformacoesPatrimoniais_Salario { get; set; }

        public string InformacoesPatrimoniais_OutrosRendimentos { get; set; }

        public string InformacoesPatrimoniais_TotalDeOutrosRendimentos { get; set; }

        public string InformacoesPatrimoniais_BensImoveis { get; set; }

        public string InformacoesPatrimoniais_BensMoveis { get; set; }

        public string InformacoesPatrimoniais_Aplicacoes { get; set; }

        public string InformacoesPatrimoniais_DeclaracoesEAutorizacoes { get; set; }

        public string DadosBasicos_Diretor { get; set; }

        public string DadosBasicos_Emitentes { get; set; }

        public string DadosBasicos_Controladora { get; set; }

        public string DadosBasicos_Representantes { get; set; }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion

        public string Dados_Contas { get; set; }
    }
}
