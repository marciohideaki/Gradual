using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.LeitorDeArquivos
{
    public class ConfiguracaoDeCampo
    {
        #region Propriedades

        //Nome do Campo      |De        |Até     |Formato de Origem        |Formato de Destino    |Tipo de Destino    |Opções

        public string NomeDoCampo { get; set; }

        public int PosicaoInicial { get; set; }

        public int Comprimento { get; set; }

        public string FormatoDeOrigem { get; set; }

        public string FormatoDeDestino { get; set; }

        public Type TipoDeDestino { get; set; }

        public string Opcoes { get; set; }
        
        public bool Opcao_BrancoIgualANulo
        {
            get
            {
                return (this.Opcoes != null && this.Opcoes.ToLower().Contains("branco=nulo"));
            }
        }
        
        public bool Opcao_BrancoIgualAZero
        {
            get
            {
                return (this.Opcoes != null && this.Opcoes.ToLower().Contains("branco=zero"));
            }
        }
        
        public bool Opcao_ErrosSaoNulos
        {
            get
            {
                return (this.Opcoes != null && this.Opcoes.ToLower().Contains("erro=nulo"));
            }
        }
        
        public bool Opcao_TrimNoValor
        {
            get
            {
                return (this.Opcoes != null && this.Opcoes.ToLower().Contains("trim"));
            }
        }

        public int Opcao_Espacamento
        {
            get
            {
                if (this.Opcoes.ToLower().Contains("espacamento="))
                {
                    int lEspacamento;

                    string lOpcao = this.Opcoes.ToLower().Substring(this.Opcoes.ToLower().IndexOf("espacamento=") + 12);


                    lOpcao = lOpcao.Trim();

                    if(lOpcao.Contains(' '))
                        lOpcao = lOpcao.Substring(0, lOpcao.IndexOf(' '));

                    lOpcao = lOpcao.TrimEnd(' ', ',');

                    if(int.TryParse(lOpcao, out lEspacamento))
                    {
                        return lEspacamento;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
        
        public int Opcao_CasasDecimais
        {
            get
            {
                if (this.Opcoes.ToLower().Contains("casasdecimais="))
                {
                    int lCasas;

                    string lOpcao = this.Opcoes.ToLower().Substring(this.Opcoes.ToLower().IndexOf("casasdecimais=") + 14);


                    lOpcao = lOpcao.Trim();

                    if(lOpcao.Contains(' '))
                        lOpcao = lOpcao.Substring(0, lOpcao.IndexOf(' '));

                    lOpcao = lOpcao.TrimEnd(' ', ',');

                    if(int.TryParse(lOpcao, out lCasas))
                    {
                        return lCasas;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        #endregion

        #region Métodos Private

        private void CarregarConfiguracao(string pItemDoCabecalho, string pValor)
        {
            int lValorParse;

            if (pItemDoCabecalho == "nome do campo" || pItemDoCabecalho == "nome")
            {
                this.NomeDoCampo = pValor;
            }
            else if (pItemDoCabecalho == "de")
            {
                if(int.TryParse(pValor, out lValorParse))
                {
                    if(this.Comprimento == -1)
                    {
                        this.PosicaoInicial = lValorParse;
                    }
                    else
                    {
                        this.PosicaoInicial = lValorParse;

                        this.Comprimento = this.PosicaoInicial - this.Comprimento;
                    }
                }
                else
                {
                    throw new Exception(string.Format("Valor numérico inválido para a propriedade 'De' do campo [{0}]: [{1}]", this.NomeDoCampo, pValor));
                }
            }
            else if (pItemDoCabecalho == "até" || pItemDoCabecalho == "ate")
            {
                if(int.TryParse(pValor, out lValorParse))
                {
                    if(this.PosicaoInicial == -1)
                    {
                        //ainda não configurou a posicao inicial, só guarda o valor
                        this.Comprimento = lValorParse;
                    }
                    else
                    {
                        //se já tem posição inicial, calcula o comprimento
                        this.Comprimento = lValorParse - this.PosicaoInicial;
                    }
                }
                else
                {
                    throw new Exception(string.Format("Valor numérico inválido para a propriedade 'Até' do campo [{0}]: [{1}]", this.NomeDoCampo, pValor));
                }
            }
            else if (pItemDoCabecalho == "comprimento")
            {
                if(int.TryParse(pValor, out lValorParse))
                {
                    this.Comprimento = lValorParse;
                }
                else
                {
                    throw new Exception(string.Format("Valor numérico inválido para a propriedade 'Comprimento' do campo [{0}]: [{1}]", this.NomeDoCampo, pValor));
                }
            }
            else if (pItemDoCabecalho == "formato de origem")
            {
                this.FormatoDeOrigem = pValor;
            }
            else if (pItemDoCabecalho == "formato de destino")
            {
                this.FormatoDeDestino = pValor;
            }
            else if (pItemDoCabecalho == "tipo de destino")
            {
                switch (pValor)
                {
                    case "texto":
                        this.TipoDeDestino = typeof(string);
                        break;
                                
                    case "inteiro":
                        this.TipoDeDestino = typeof(int);
                        break;
                                
                    case "decimal":
                        this.TipoDeDestino = typeof(double);
                        break;
                                
                    case "data":
                        this.TipoDeDestino = typeof(DateTime);
                        break;

                    default:

                        throw new Exception(string.Format("Tipo inválido para a propriedade 'Tipo de Destino' do campo [{0}]: [{1}]", this.NomeDoCampo, pValor));

                }
            }
            else if (pItemDoCabecalho == "opções" || pItemDoCabecalho == "opcoes")
            {
                this.Opcoes = pValor;
            }
        }

        private void CarregarConfiguracoesDaLinhaDeConfiguracao(string pCabecalho, string pLinha)
        {
            List<int> lPosicoesDosPipes = new List<int>();

            string lItemDoCabecalho, lValor;

            int lPipeA, lComprimento;

            for (int a = 0; a < pCabecalho.Length; a++)
            {
                if (pCabecalho[a] == '|')
                    lPosicoesDosPipes.Add(a);
            }

            for (int a = 0; a < lPosicoesDosPipes.Count; a++)
            {
                lPipeA = lPosicoesDosPipes[a] + 1;

                if (a < lPosicoesDosPipes.Count - 1)
                {
                    lComprimento = lPosicoesDosPipes[a + 1] - lPipeA;

                    lItemDoCabecalho = pCabecalho.Substring(lPipeA, lComprimento).Trim().ToLower();

                    lValor = pLinha.Substring(lPipeA, lComprimento).Trim();
                }
                else
                {
                    lItemDoCabecalho = pCabecalho.Substring(lPipeA).Trim().ToLower();
                    lValor = pLinha.Substring(lPipeA).Trim();
                }

                CarregarConfiguracao(lItemDoCabecalho, lValor);
            }
        }

        #endregion

        #region Construtor

        public ConfiguracaoDeCampo(string pCabecalho, string pLinhaDeConfiguracao)
        {
            this.PosicaoInicial = -1;
            this.Comprimento = -1;

            this.CarregarConfiguracoesDaLinhaDeConfiguracao(pCabecalho, pLinhaDeConfiguracao);
        }

        #endregion
    }
}
