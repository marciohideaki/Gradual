using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using System.Data;

namespace Gradual.Intranet.Servicos.Mock
{
    public static class FabricaDeMocks
    {
        #region Métodos Públicos

        public static ClienteResumidoInfo GerarResumoDoCliente(DataRow pDadosIniciais)
        {
            ClienteResumidoInfo lRetorno = new ClienteResumidoInfo()
            {
                IdCliente    = pDadosIniciais["IdCliente"].DBToInt32(),
                NomeCliente  = pDadosIniciais["NomeCliente"].DBToString(),
                CodBovespa   = pDadosIniciais["CodBovespa"].DBToString(),
                CodBMF       = pDadosIniciais["CodBMF"].DBToString(),
                CodGradual   = pDadosIniciais["CodGradual"].DBToString(),
                CPF          = pDadosIniciais["CPF"].DBToString(),
                Sexo         = pDadosIniciais["Sexo"].DBToString(),
                Status       = pDadosIniciais["Status"].DBToString(),
                Passo        = pDadosIniciais["Passo"].DBToString(),
                TipoCliente  = pDadosIniciais["TipoCliente"].DBToString(),
                DataCadastro = pDadosIniciais["DataCadastro"].DBToDateTime()
            };

            lRetorno.Email = string.Format("{0}@email.com", lRetorno.NomeCliente.Split(' ')[0]);

            return lRetorno;
        }

        public static ClienteInfo GerarClienteInfo(DataRow pDadosIniciais)
        {
            ClienteInfo lRetorno = new ClienteInfo();
            
            lRetorno.IdCliente = pDadosIniciais["IdCliente"].DBToInt32();
            lRetorno.DsNome = pDadosIniciais["NomeCliente"].DBToString();
            lRetorno.DsNumeroDocumento = pDadosIniciais["CPF"].DBToString();
            lRetorno.DsCpfCnpj = pDadosIniciais["CPF"].DBToString();
            
            if(pDadosIniciais["Sexo"].DBToString() != "")
                lRetorno.CdSexo = pDadosIniciais["Sexo"].DBToString()[0];
            
            lRetorno.TpCliente = ((pDadosIniciais["TipoCliente"].DBToString() == "PF") ? 1 : 2);
            lRetorno.TpPessoa = pDadosIniciais["TipoCliente"].DBToString()[1];
            lRetorno.DtNascimentoFundacao = pDadosIniciais["DataCadastro"].DBToDateTime();

            lRetorno.IdLogin = 123;
            lRetorno.DtUltimaAtualizacao = DateTime.Now;
            lRetorno.DtPasso1 = DateTime.Now.AddDays(-3);
            lRetorno.DtPasso2 = DateTime.Now.AddDays(-2);
            lRetorno.DtPasso3 = DateTime.Now.AddDays(-1);
            lRetorno.DtPrimeiraExportacao = DateTime.Now.AddDays(-3);
            lRetorno.DtUltimaExportacao = DateTime.Now.AddDays(-1);
            lRetorno.DsOrigemCadastro = "Origem";
            lRetorno.StPasso = 3;
            lRetorno.CdNacionalidade = 1;
            lRetorno.CdPaisNascimento = "Brasil";
            lRetorno.CdUfNascimento = "SP";
            lRetorno.DsUfnascimentoEstrangeiro = "";
            lRetorno.CdEstadoCivil = 1;
            lRetorno.DsConjugue = "Conjuge da Pessoa";
            lRetorno.TpDocumento = "RG";
            lRetorno.CdOrgaoEmissorDocumento = "SSP";
            lRetorno.CdUfEmissaoDocumento = "SP";
            lRetorno.CdProfissaoAtividade = 1;
            lRetorno.DsCargo = "Programador";
            lRetorno.DsEmpresa = "Gradual Investimentos";
            lRetorno.StPPE = true;
            lRetorno.StCarteiraPropria = false;
            lRetorno.DsAutorizadoOperar = "";
            lRetorno.StCVM387 = true;
            lRetorno.StEmancipado = true;
            lRetorno.IdAssessorInicial = 1;
            lRetorno.CdEscolaridade = 3;
            lRetorno.StCadastroPortal = false;
            lRetorno.DsNomeFantasia = "";
            lRetorno.CdNire = 123;
            lRetorno.DsFormaConstituicao = "";
            lRetorno.StInterdito = false;
            lRetorno.StSituacaoLegalOutros = true;
            lRetorno.CdAtividadePrincipal = 1;

            return lRetorno;
        }

        public static ClienteSituacaoFinanceiraPatrimonialInfo GerarClienteSituacaoFinanceiraPatrimonialInfo(int pIdCliente, Nullable<Int32> pId)
        {
            ClienteSituacaoFinanceiraPatrimonialInfo lRetorno = new ClienteSituacaoFinanceiraPatrimonialInfo();

            Random lRand = new Random();

            lRetorno.IdCliente = pIdCliente;
            lRetorno.IdSituacaoFinanceiraPatrimonial = pId;

            lRetorno.VlTotalAplicacaoFinanceira = lRand.Next(1, 99) * 10000;
            lRetorno.VlTotalBensImoveis = lRand.Next(1, 99) * 10000;
            lRetorno.VlTotalBensMoveis = lRand.Next(1, 99) * 10000;
            lRetorno.VlTotalOutrosRendimentos = lRand.Next(1, 99) * 10000;
            lRetorno.VlTotalPatrimonioLiquido = lRand.Next(1, 99) * 10000;
            lRetorno.VlTotalSalarioProLabore = lRand.Next(1, 99) * 1000;
            lRetorno.VTotalCapitalSocial = lRand.Next(1, 99) * 10000;

            lRetorno.DsOutrosRendimentos = "descrição Outros Rendimentos";
            lRetorno.DtAtualizacao = DateTime.Now.AddDays(-1);
            lRetorno.DtCapitalSocial = DateTime.Now.AddDays(-1);
            lRetorno.DtPatrimonioLiquido = DateTime.Now.AddDays(-1);

            return lRetorno;
        }

        public static ClienteCadastradoPeriodoInfo GerarRelatorio_ClienteCadastradoPorPeriodo(DataRow pDadosIniciais)
        {
            ClienteCadastradoPeriodoInfo lRetorno = new ClienteCadastradoPeriodoInfo()
            {
                IdCliente = pDadosIniciais["IdCliente"].DBToInt32(),
                DsNomeCliente = pDadosIniciais["NomeCliente"].DBToString(),
                CodigoBovespa = pDadosIniciais["CodBovespa"].DBToInt32(),
                CodigoBmf = pDadosIniciais["CodBMF"].DBToInt32(),
                DsCpfCnpj = pDadosIniciais["CPF"].DBToString(),
                TipoPessoa = pDadosIniciais["TipoCliente"].DBToString(),
                DtCadastro = pDadosIniciais["DataCadastro"].DBToDateTime()
            };

            Random lRand = new Random();

            lRetorno.BlnExportado = (lRand.Next(0, 1) == 1);
            lRetorno.CodigoAssessor = (lRand.Next(1, 5) * 100);
            lRetorno.DsDDD = string.Format("{0}{0}", lRand.Next(1, 5));
            lRetorno.DsTelefone = string.Format("99{0}{0}-{1}{2}{1}{2}", lRand.Next(1, 9), lRand.Next(0, 9), lRand.Next(0, 9));
            lRetorno.DtCadastro = DateTime.Now.AddDays(lRand.Next(-100, -1));
            lRetorno.DtUltimaAtualizacao = lRetorno.DtCadastro;

            return lRetorno;
        }

        public static List<PessoaExpostaPoliticamenteInfo> GerarListaDePEP()
        {
            List<PessoaExpostaPoliticamenteInfo> lRetorno = new List<PessoaExpostaPoliticamenteInfo>();

            lRetorno.Add(new PessoaExpostaPoliticamenteInfo()
            {
                  IdPEP = 1
                , DsDocumento = "11.111.111-1"
                , DsNome = "Luciano Teste"
                , DsIdentificacao = ""
                , DtImportacao = DateTime.Now
            });


            return lRetorno;
        }

        #endregion
    }
}
