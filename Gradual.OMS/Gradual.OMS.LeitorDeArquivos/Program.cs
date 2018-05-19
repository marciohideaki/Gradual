using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Gradual.OMS.LeitorDeArquivos
{
    class Program
    {
        #region Globais

        private static ImportadorDeArquivos gImportador;

        private const string COMANDO_CRIAR_DEFINICAO = "criardefinicao";
        
        private const string COMANDO_IMPORTAR = "importar";

        private const string ARQUIVO_TEMPLATE = @"
//Arquivo .defimp de template

//
//  Descomentar as linhas que for utilizar!
//

//Conexão:                  Data Source=111.222.333.44; Initial Catalog=BANCO_XXX;User Id=USER_YYY;Password=123456;
//Arquivo:                  C:\Saída.txt
        
//Tabela de Destino:        tbAlgumaTabela
//Proc de Destino:          pcAlgumaCoisa

// (utilizar tabela OU proc, não ambos)

//Proc Anterior:      <valor>    // Nome de uma procedure pra rodar antes da importação começar
//Proc Posterior:     <valor>    // Nome de uma procedure pra rodar depois da importação ser realizada (com sucesso ou com erro)
//Proc Caso Erro:     <valor>    // Nome de uma procedure pra rodar após a 'Proc Posterior', somente se a importação for realizada com erro(s)
//Proc Caso Sucesso:  <valor>    // Nome de uma procedure pra rodar após a 'Proc Posterior', somente se a importação for realizada com sucesso

//Campo de Data de Importação:  dtImportacao        (opcional)
//Campo de Linha do arquivo:    sLinhaImportacao    (opcional)

//Campos:
//    |Nome do Campo      |De        |Até     |Formato de Origem        |Formato de Destino    |Tipo de Destino    |Opções
//    | XXXXXX            | 1        | 11     |                         |                      |                   | 

//Valores válidos:
//  Nome do Campo:           Deve ter @ na frente se usar 'Proc de Destino' nas opções acima
//  De:                      Deve ser um número, não pode estar vazio
//  Até (ou 'Comprimento')   Se for 'Até', o substring irá usar (Até - De) para 'length'. Se for 'Comprimento', usa o valor direto
//  Formato de Origem        Aceita qualquer string que caiba em DateTime.ParseExact(s), para quando o dado de origem for uma data
//  Formato de Destino       Aceita qualquer string que caiba em <Tipo de Destino>.ToString(s), quando o destino for texto
//  Tipo de Destino          Aceita somente os valores: 'texto', 'inteiro', 'decimal' ou 'data' (sem aspas)
//  Opções                   Aceita 'branco=nulo', 'branco=zero', 'erro=nulo', 'trim','espacamento=XXXX' ou 'casasdeciamis=XXX'";

        #endregion

        #region Métodos Private

        private static void ExibirAjuda()
        {
            Console.WriteLine("Lista de Comandos:");
            Console.WriteLine("  CriarDefinicao <caminho_do_arquivo.defimp>");
            Console.WriteLine("  Importar [caminho_do_arquivo.defimp] [caminho_do_arquivo_para_importar]");
        }

        private static void CriarDefinicao(string pCaminhoDoArquivo)
        {
            try
            {
                if (string.IsNullOrEmpty(pCaminhoDoArquivo))
                {
                    pCaminhoDoArquivo = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                    pCaminhoDoArquivo = Path.Combine(pCaminhoDoArquivo, string.Format("Definicao-{0}.defimp", DateTime.Now.ToString("yyMMdd-HHmm")));
                }

                File.WriteAllText(pCaminhoDoArquivo, ARQUIVO_TEMPLATE);

                Console.WriteLine("Arquivo [{0}] criado com sucesso.", pCaminhoDoArquivo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Erro durante a execução: {0}\r\n\r\n{1}", ex.Message, ex.StackTrace));
            }
        }

        private static void ImportarDados(string pCaminhoDoArquivoDeDefinicao, string pCaminhoDoArquivoDeDados)
        {
            gImportador = new ImportadorDeArquivos();

            gImportador.Mensagem += new MensagemDeImportacao(lImportador_ChegadaDeMensagem);

            gImportador.LerArquivoDeDefinicao(pCaminhoDoArquivoDeDefinicao);

            gImportador.RealizarImportacao(pCaminhoDoArquivoDeDados);

            /*
            while (!lImportador.ImportacaoFinalizada)
            {
                System.Threading.Thread.Sleep(300);
            }
            
            Console.WriteLine("Importação finalizada.");
             * */
        }

        private static void lImportador_ChegadaDeMensagem(string pMensagem)
        {
            Console.WriteLine(string.Format("Importador>{0}", pMensagem));
        }

        #endregion

        #region Event Handlers

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Parâmetros inválidos, utilizar -? para ver uma lista de comandos.");

                string lInput = Console.ReadLine();

                if (lInput == "?" || lInput == "-?")
                {
                    ExibirAjuda();
                }
            }
            else
            {
                string lArg = args[0].ToLower();

                if (lArg == "-?" || lArg == "?")
                {
                    ExibirAjuda();
                }
                else if (lArg == COMANDO_CRIAR_DEFINICAO)
                {
                    CriarDefinicao(args.Length > 1 ? args[1] : "");
                }
                else if (lArg == COMANDO_IMPORTAR)
                {
                    if (args.Length < 3)
                    {
                        Console.WriteLine("Número de argumentos insuficiente, favor utilizar Importar [caminho_do_arquivo.defimp] [caminho_do_arquivo_para_importar.xxx]");
                    }
                    else
                    {
                        ImportarDados(args[1], args[2]);

                        while (!gImportador.ImportacaoFinalizada)
                        {
                            System.Threading.Thread.Sleep(50);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Argumento inválido: [{0}]", args[0]);
                }
            }

            //Console.ReadLine();
        }

        #endregion
    }
}
