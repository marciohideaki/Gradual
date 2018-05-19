using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.Spider.PositionClient.DbLib
{
    [Flags]
    public enum OpcoesPendencia
    {
        ComPendenciaCadastral = 0x01,
        ComSolicitacaoAlteracao = 0x02
    }

    public enum OpcoesBuscaStatusOrdem
    {
        Todos = 0,
        Aberta = 1,
        Cancelada = 2,
        Executado = 3,
    }

    public enum OpcoesBuscarClientePor
    {
        CodBovespa = 0,
        CpfCnpj = 1,
        NomeCliente = 2,
        CodBMF = 3,
        Login = 4,
    }

    public enum OpcoesBuscarPor
    {
        CodBovespa = 0,
        CpfCnpj = 1,
        NomeCliente = 2,
        Email = 3
    }

    [Flags]
    public enum OpcoesTipo
    {
        ClientePF = 0x01,
        ClientePJ = 0x02
    }

    [Flags]
    public enum OpcoesStatus
    {
        Ativo = 0x01,
        Inativo = 0x02
    }

    [Flags]
    public enum OpcoesPasso
    {
        Visitante = 0x01,
        Cadastrado = 0x02,
        Exportado = 0x04
    }

    /// <summary>
    /// Define qual tipo de consulta será feita quando ClienteResumidoInfo for passado como <T> em ConsultarRequest<T>
    /// </summary>
    public enum TipoDeConsultaClienteResumidoInfo
    {
        /// <summary>
        /// Busca feita na tabela de clientes, com parâmetro normais
        /// </summary>
        Clientes,

        /// <summary>
        /// Busca feita pelo Id do Assesor do cliente
        /// </summary>
        ClientesPorAssessor
    }

    /// <summary>
    /// Descrição para os índices da tabela Configuração.
    /// </summary>
    public enum EConfiguracaoDescricao
    {
        PeriodicidadeRenovacaoCadastral = 0,
        PeriodoRegressoDeConsultaParaRenovacaoCadastral = 1
    }

    /// <summary>
    /// Enum para definir qual Migração o sistema deve efetuar
    /// </summary>
    public enum MigracaoClienteAssessorAcao
    {
        MigrarClienteTodos,
        MigrarClienteUnico,
        MigrarClienteParcial
    }

    /// <summary>
    /// Enum para definir qual receber pessoa o sistema deve efetuar
    /// </summary>
    public enum eReceberPessoaVinculada
    {
        PorIdPessoaVinculada = 1,
        PorIdResponsavel = 2
    }

    /// <summary>
    /// Informação(Tabela) a ser pesquisada
    /// </summary>
    public enum eInformacao
    {
        Nacionalidade = 1,
        EstadoCivil = 2,
        TipoDocumento = 3,
        OrgaoEmissor = 4,
        ProfissaoPF = 5,
        Banco = 6,
        Estado = 7,
        Pais = 8,
        Assessor = 9,
        SituacaoLegalRepresentante = 10,
        TipoCliente = 11,
        AtividadePJ = 12,
        TipoInvestidorPJ = 13,
        TipoConta = 14,
        Escolaridade = 15,
        AtividadePF = 16,
        AtividadePFePJ = 17,
        AssessorPadronizado = 18,
    }

    public enum eTipoProc
    {
        Listar = 1,
        Atualizar = 2,
        Inserir = 3,
        Excluir = 4,
        Selecionar = 5
    }

    public enum eTipoEmailDisparo
    {
        Todos = 0,
        Assessor = 1,
        Compliance = 2
    }

    public enum eSistema
    {
        BOL,
        BMF
    }

    public enum eAtividade
    {
        BOL,
        BMF,
        CUS,
        CC
    }

    public enum eDateNull
    {
        Permite,
        DataMinValue
    }

    public enum eIntNull
    {
        Permite,
        Zero
    }

    public enum eTipoAcesso
    {
        Cliente = 0,
        Cadastro = 1,
        Assessor = 2,
        Atendimento = 3,
        TeleMarketing = 4
    }

    public enum eTipoPessoa
    {
        Fisica,
        Juridica,
        Ambos
    }

    public enum eTipoLimite
    {
        Nenhum = 0,
        OperarCompraAVista = 1,
        OperarCompraOpcao = 2,
        OperarDescobertoAvista = 3,
        OperarDescobertoOpcao = 4,
        ValorMaximoOrdem = 5,
    }

    public enum TipoAcaoUsuario
    {
        Consulta = 1,
        Edicao = 2,
        Exclusao = 3,
        Inclusao = 4,
    }

    public enum TipoProcuradorRepresentante
    {
        Administrador = 1,
        Controlador = 2,
        Procurador = 3,
    }
}
