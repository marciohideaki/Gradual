using System;
using System.Collections.Generic;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    /// <summary>
    /// Retorno do Contrato de Exportação
    /// </summary>
    public class SinacorExportacaoRetornoInfo : ICodigoEntidade
    {
        /// <summary>
        /// Dados do cliente ok para a Importação
        /// </summary>
        public Boolean DadosClienteOk { get; set; }

        /// <summary>
        /// Dados faltando para a importação
        /// </summary>
        public List<SinacorExportacaoRetornoFalhaDadosClienteInfo> DadosClienteMensagens { get; set; }

        /// <summary>
        /// Exportação automática funcionou
        /// </summary>
        public Boolean ExportacaoSinacorOk { get; set; }

        /// <summary>
        /// Lista de Mensagens de Erros ocorridos no processo automático de exportação para o Sinacor
        /// </summary>
        public List<SinacorExportacaoRetornoFalhaBovespaInfo> ExportacaoSinacorMensagens { get; set; }

        /// <summary>
        /// Atualização no Cadastro funcionou
        /// </summary>
        public Boolean ExportacaoAtualizarCadastroOk { get; set; }

        /// <summary>
        /// Lista de Mensagens de Erros ocorridos no processo automático de exportação para o Sinacor
        /// </summary>
        public List<SinacorExportacaoRetornoFalhaSistemaCadastroInfo> ExportacaoAtualizarCadastroMensagens { get; set; }

        /// <summary>
        /// Exportação de todos os dados complementares funcionou
        /// </summary>
        public Boolean ExportacaoComplementosOk { get; set; }

        /// <summary>
        /// Lista de Mensagens de Erros ocorridos no gravação de dados complementares no Sinacor
        /// </summary>
        public List<SinacorExportacaoRetornoFalhaComplementosInfo> ExportacaoComplementosMensagens { get; set; }
 
        /// <summary>
        /// Exportação de todos os parâmetros de risco funcionou
        /// </summary>
        public Boolean ExportacaoRiscoOk { get; set; }

        /// <summary>
        /// Lista de Mensagens de Erros ocorridos no gravação dos parâmetros de Risco
        /// </summary>
        public List<SinacorExportacaoRetornoFalhaRiscoInfo> ExportacaoRiscoMensagens { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Tratamento de exceção na verificação de cliente completo
    /// </summary>
    public class SinacorExportacaoRetornoFalhaDadosClienteInfo : ICodigoEntidade
    {
        public string Mensagem { get; set; }
        public string Tabela { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Tratamento de exceção na atualização do cliente após exportação
    /// </summary>
    public class SinacorExportacaoRetornoFalhaSistemaCadastroInfo : ICodigoEntidade
    {
        public string Mensagem { get; set; }
        public string Tabela { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Tratamento de erro no processo CliExterno do Sinacor
    /// </summary>
    public class SinacorExportacaoRetornoFalhaBovespaInfo : ICodigoEntidade
    {
        public DateTime DT_IMPORTA { get; set; }
        public Int64 CD_CPFCGC { get; set; }
        public DateTime DT_NASC_FUND { get; set; }
        public int CD_CLIENTE { get; set; }
        public string NM_CLIENTE { get; set; }
        public string DS_OBS { get; set; }
        public string DS_AUX { get; set; }
        public string CD_EXTERNO { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Tratamento de erro no complemento da Exportação
    /// </summary>
    public class SinacorExportacaoRetornoFalhaComplementosInfo : ICodigoEntidade
    {
        public string Mensagem { get; set; }
        public string Excessao { get; set; }
        public string Procedure { get; set; }
        public string Tabela { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Tratamento de erro na Inclusão dos Parâmetros de Risco
    /// </summary>
    public class SinacorExportacaoRetornoFalhaRiscoInfo : ICodigoEntidade
    {
        public string Mensagem { get; set; }
        public string Excessao { get; set; }
        public string Procedure { get; set; }
        public string Tabela { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
