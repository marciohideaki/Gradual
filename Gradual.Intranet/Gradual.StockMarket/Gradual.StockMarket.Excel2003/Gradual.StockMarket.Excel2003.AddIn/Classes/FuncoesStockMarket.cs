using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Threading;

using MdsBayeuxClient;

using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using System.Globalization;

namespace Gradual.StockMarket
{
    [ComDefaultInterface(typeof(IFuncoesStockMarket))]
    public class FuncoesStockMarket : IFuncoesStockMarket
    {
        #region Globais

        //private MdsHttpClient gMdsHttpClient = null;

        private string gUrlMdsHttpClient = "http://192.168.254.13:8080";

        private Dictionary<string, MdsHttpClient> gLivrosDeOfertas = new Dictionary<string, MdsHttpClient>();

        private Dictionary<string, MdsHttpClient> gNegocios = new Dictionary<string, MdsHttpClient>();

        private Dictionary<string, MdsNegociosDados> gNegociosDados = new Dictionary<string, MdsNegociosDados>();

        private Random gRand = new Random();

        private CultureInfo gCulture = new CultureInfo("pt-BR");

        #endregion

        #region Métodos Private

        private int ConverterParaInt(object pValor)
        {
            if (pValor == null) return 0;

            if (pValor.ToString() == "") return 0;

            return Convert.ToInt32(pValor, gCulture);
        }

        private Int64 ConverterParaInt64(object pValor)
        {
            if (pValor == null) return 0;

            string lValor = pValor.ToString();

            if (lValor == "") return 0;

            if (lValor.Contains(','))
                lValor = lValor.Substring(0, lValor.IndexOf(','));

            return Convert.ToInt64(lValor, gCulture);
        }

        private decimal ConverterParaDecimal(object pValor)
        {
            if (pValor == null) return 0;

            if (pValor.ToString() == "") return 0;

            return Convert.ToDecimal(pValor, gCulture);
        }

        #endregion

        #region Métodos Públicos

        public object[] SM_COTACAO(string Instrumento)
        {
            object[] lRetorno = new object[14];     // o Excel mapeia essa array para colunas / linhas

            if (!string.IsNullOrEmpty(Instrumento))
            {
                Instrumento = Instrumento.ToUpper();

                //pela documentação, temos que o resultado dessa função deve ser:
                /*
                    Papel           >> Nome do Instrumento
                    última          >> Ultima cotação enviada pelo MDS
                    Var (%)         >> Variação do Instrumento em relação a abertura.
                    Cor.Comp        >> Código da melhor corretora compradora
                    Vl.Comp         >> Valor da melhor oferta de compra
                    Cor.Venda       >> Código da melhor corretora vendedora
                    Vl.Venda        >> Valor da melhor oferta de venda
                    Abertura        >> Valor da abertura do dia
                    Mínima          >> Negócio mais barato do dia
                    Máxima          >> Negócio mais caro do dia.
                    Fech.Anterior   >> Valor do fechamento anterior.
                    N.Neg           >> Numero de negócios do dia para o instrumento
                    Volume          >> Volume gerado pelo instrumento
                    Data/Hora       >> Data e hora do último negócio realizado para o instrumento
                */

                /*
                lRetorno[0] = Instrumento;
                lRetorno[1] = 24.77 + gRand.Next(-5, 5);
                lRetorno[2] = 0.78;
                lRetorno[3] = gRand.Next(27, 300).ToString();
                lRetorno[4] = 24.78;
                lRetorno[5] = gRand.Next(27, 300).ToString();
                lRetorno[6] = 24.79;
                lRetorno[7] = 24.6;
                lRetorno[8] = 24.6;
                lRetorno[9] = 24.79;
                lRetorno[10] = 24.5;
                lRetorno[11] = 1500 + gRand.Next(1, 20);
                lRetorno[12] = 44560 + gRand.Next(10, 500);
                lRetorno[13] = DateTime.Now;
                 * */

                if (!gNegociosDados.ContainsKey(Instrumento))
                    gNegociosDados.Add(Instrumento, new MdsNegociosDados());

                if (!gNegocios.ContainsKey(Instrumento))
                {

                    try
                    {
                        gNegocios.Add(Instrumento, new MdsHttpClient());

                        gNegocios[Instrumento].OnNegociosEvent += new MdsHttpClient.OnNegociosHandler(gMdsHttpClient_OnNegociosEvent);

                        gNegocios[Instrumento].Conecta(gUrlMdsHttpClient);

                        try
                        {
                            gNegocios[Instrumento].AssinaNegocios(Instrumento);
                        }
                        catch (Exception ex2)
                        {
                            System.Windows.Forms.MessageBox.Show(string.Format("Erro ao assinar [{0}]: {1}\r\n\r\n{2}", Instrumento, ex2.Message, ex2.StackTrace));

                            return lRetorno;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format("Erro ao conectar em [{0}]: {1}\r\n\r\n{2}", gUrlMdsHttpClient, ex.Message, ex.StackTrace));

                        return lRetorno;
                    }
                }

                lRetorno[0] = Instrumento;
                lRetorno[1] = ConverterParaDecimal(gNegociosDados[Instrumento].preco);        // última
                lRetorno[2] = ConverterParaDecimal(gNegociosDados[Instrumento].sinalVariacao + gNegociosDados[Instrumento].variacao);
                lRetorno[3] = ConverterParaInt(gNegociosDados[Instrumento].compradora);
                lRetorno[4] = ConverterParaDecimal(gNegociosDados[Instrumento].melhorOfertaCompra);
                lRetorno[5] = ConverterParaInt(gNegociosDados[Instrumento].vendedora);
                lRetorno[6] = ConverterParaDecimal(gNegociosDados[Instrumento].melhorOfertaVenda);
                lRetorno[7] = "n/d";    //abertura
                lRetorno[8] = ConverterParaDecimal(gNegociosDados[Instrumento].minima);
                lRetorno[9] = ConverterParaDecimal(gNegociosDados[Instrumento].maxima);
                lRetorno[10] = "n/d";   //fechamento
                lRetorno[11] = ConverterParaInt(gNegociosDados[Instrumento].numeroNegocios);
                lRetorno[12] = ConverterParaInt64(gNegociosDados[Instrumento].volume);
                lRetorno[13] = DateTime.ParseExact(gNegociosDados[Instrumento].data.ToString(), "yyyyMMdd", gCulture);

            }

            return lRetorno;
        }

        public object[,] SM_TICKER(string Instrumento)
        {
            object[,] lRetorno = new object[6, 4];

            if (!string.IsNullOrEmpty(Instrumento))
            {
                Instrumento = Instrumento.ToUpper();

                //pela documentação, temos que o resultado dessa função deve ser:
                /*
                    Ultima       <valor>    Var(%)      <valor>
                    Compra       <valor>    Venda       <valor>
                    Qtd. Compra  <valor>    Qtd. Venda  <valor>
                    Max          <valor>    Min         <valor>
                    Aber         <valor>    Fech        <valor>
                    Nr. Neg      <valor>    Vol         <valor>
                */

                if (!gNegociosDados.ContainsKey(Instrumento))
                    gNegociosDados.Add(Instrumento, new MdsNegociosDados());

                if (!gNegocios.ContainsKey(Instrumento))
                {

                    try
                    {
                        gNegocios.Add(Instrumento, new MdsHttpClient());

                        gNegocios[Instrumento].OnNegociosEvent += new MdsHttpClient.OnNegociosHandler(gMdsHttpClient_OnNegociosEvent);

                        gNegocios[Instrumento].Conecta(gUrlMdsHttpClient);

                        try
                        {
                            gNegocios[Instrumento].AssinaNegocios(Instrumento);
                        }
                        catch (Exception ex2)
                        {
                            System.Windows.Forms.MessageBox.Show(string.Format("Erro ao assinar [{0}]: {1}\r\n\r\n{2}", Instrumento, ex2.Message, ex2.StackTrace));

                            return lRetorno;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format("Erro ao conectar em [{0}]: {1}\r\n\r\n{2}", gUrlMdsHttpClient, ex.Message, ex.StackTrace));

                        return lRetorno;
                    }
                }


                // primeira linha:

                lRetorno[0, 0] = "Última";
                lRetorno[0, 1] = ConverterParaDecimal(gNegociosDados[Instrumento].preco);
                lRetorno[0, 2] = "Var(%)";
                lRetorno[0, 3] = ConverterParaDecimal(gNegociosDados[Instrumento].sinalVariacao + gNegociosDados[Instrumento].variacao);

                // segunda linha:

                lRetorno[1, 0] = "Compra";
                lRetorno[1, 1] = ConverterParaDecimal(gNegociosDados[Instrumento].melhorOfertaCompra);
                lRetorno[1, 2] = "Venda";
                lRetorno[1, 3] = ConverterParaDecimal(gNegociosDados[Instrumento].melhorOfertaVenda);

                // terceira linha:

                lRetorno[2, 0] = "Qtd. Compra";
                lRetorno[2, 1] = ConverterParaInt(gNegociosDados[Instrumento].melhorQuantidadeCompra);
                lRetorno[2, 2] = "Qtd. Venda";
                lRetorno[2, 3] = ConverterParaInt(gNegociosDados[Instrumento].melhorQuantidadeVenda);

                // quarta linha:

                lRetorno[3, 0] = "Max";
                lRetorno[3, 1] = ConverterParaDecimal(gNegociosDados[Instrumento].maxima);
                lRetorno[3, 2] = "Min";
                lRetorno[3, 3] = ConverterParaDecimal(gNegociosDados[Instrumento].minima);

                // quinta linha:

                lRetorno[4, 0] = "Aber";
                lRetorno[4, 1] = "n/d";
                lRetorno[4, 2] = "Fech";
                lRetorno[4, 3] = "n/d";

                // sexta linha:

                lRetorno[5, 0] = "Nr. Neg";
                lRetorno[5, 1] = ConverterParaInt(gNegociosDados[Instrumento].numeroNegocios);
                lRetorno[5, 2] = "Vol";
                lRetorno[5, 3] = ConverterParaInt64(gNegociosDados[Instrumento].volume);
            }

            return lRetorno;
        }

        public object[,] SM_LIVROOFERTAS(string Instrumento)
        {
            object[,] lRetorno = new object[10, 6];

            if (!string.IsNullOrEmpty(Instrumento))
            {
                Instrumento = Instrumento.ToUpper();

                if (!gLivrosDeOfertas.ContainsKey(Instrumento))
                {

                    try
                    {
                        gLivrosDeOfertas.Add(Instrumento, new MdsHttpClient());

                        gLivrosDeOfertas[Instrumento].OnOfertasEvent += new MdsHttpClient.OnOfertasHandler(gMdsHttpClient_OnOfertasEvent);

                        gLivrosDeOfertas[Instrumento].Conecta(gUrlMdsHttpClient);

                        try
                        {
                            gLivrosDeOfertas[Instrumento].AssinaLivroOfertas(Instrumento);
                        }
                        catch (Exception ex2)
                        {
                            System.Windows.Forms.MessageBox.Show(string.Format("Erro ao assinar [{0}]: {1}\r\n\r\n{2}", Instrumento, ex2.Message, ex2.StackTrace));

                            return lRetorno;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format("Erro ao conectar em [{0}]: {1}\r\n\r\n{2}", gUrlMdsHttpClient, ex.Message, ex.StackTrace));

                        return lRetorno;
                    }
                }

                for (int a = 0; a < 10; a++)
                {
                    if (gLivrosDeOfertas[Instrumento].DsLivroDeOfertas.OfertasDeCompra.Rows.Count > a)
                    {
                        lRetorno[a, 0] = ConverterParaInt(gLivrosDeOfertas[Instrumento].DsLivroDeOfertas.OfertasDeCompra.Rows[a]["Corretora"]);
                        lRetorno[a, 1] = ConverterParaInt(gLivrosDeOfertas[Instrumento].DsLivroDeOfertas.OfertasDeCompra.Rows[a]["Quantidade"]);
                        lRetorno[a, 2] = ConverterParaDecimal(gLivrosDeOfertas[Instrumento].DsLivroDeOfertas.OfertasDeCompra.Rows[a]["Preco"]);

                        lRetorno[a, 3] = ConverterParaDecimal(gLivrosDeOfertas[Instrumento].DsLivroDeOfertas.OfertasDeCompra.Rows[a]["Preco"]);
                        lRetorno[a, 4] = ConverterParaInt(gLivrosDeOfertas[Instrumento].DsLivroDeOfertas.OfertasDeCompra.Rows[a]["Quantidade"]);
                        lRetorno[a, 5] = ConverterParaInt(gLivrosDeOfertas[Instrumento].DsLivroDeOfertas.OfertasDeCompra.Rows[a]["Corretora"]);
                    }
                }
            }

            return lRetorno;
        }

        public object SM_ELEMENTO_COTACAO(object pElemento, string pInstrumento)
        {
            object lRetorno = null;

            object[] lCotacao;

            int lIndiceElemento = -1;

            if (!int.TryParse(pElemento.ToString(), out lIndiceElemento))
            {
                string lElemento = pElemento.ToString().ToLower();

                if (lElemento == "papel" || lElemento == "instrumento" || lElemento == "ativo")
                {
                    lIndiceElemento = 0;
                }
                else if (lElemento == "ultima" || lElemento == "última" || lElemento == "ult" || lElemento == "últ")
                {
                    lIndiceElemento = 1;
                }
                else if (lElemento == "variacao" || lElemento == "variação" || lElemento == "var")
                {
                    lIndiceElemento = 2;
                }
                else if (lElemento == "corretora de compra" || lElemento == "corretora compra" || lElemento == "cor comp")
                {
                    lIndiceElemento = 3;
                }
                else if (lElemento == "valor de compra" || lElemento == "valor compra" || lElemento == "val comp" || lElemento == "vl comp")
                {
                    lIndiceElemento = 4;
                }
                else if (lElemento == "corretora de venda" || lElemento == "corretora venda" || lElemento == "cor vend")
                {
                    lIndiceElemento = 5;
                }
                else if (lElemento == "valor de venda" || lElemento == "valor venda" || lElemento == "val comp" || lElemento == "vl vend")
                {
                    lIndiceElemento = 6;
                }
                else if (lElemento == "abertura" || lElemento == "abert")
                {
                    lIndiceElemento = 7;
                }
                else if (lElemento == "mínima" || lElemento == "minima" || lElemento == "mín" || lElemento == "min")
                {
                    lIndiceElemento = 8;
                }
                else if (lElemento == "máxima" || lElemento == "maxima" || lElemento == "máx" || lElemento == "max")
                {
                    lIndiceElemento = 9;
                }
                else if (lElemento == "fechamento anterior" || lElemento == "fech anterior" || lElemento == "fech ant")
                {
                    lIndiceElemento = 10;
                }
                else if (lElemento == "número de negócios" || lElemento == "numero de negocios" || lElemento == "num neg" || lElemento == "n neg")
                {
                    lIndiceElemento = 11;
                }
                else if (lElemento == "volume" || lElemento == "vol")
                {
                    lIndiceElemento = 12;
                }
                else if (lElemento == "data" || lElemento == "hora" || lElemento == "data / hora")
                {
                    lIndiceElemento = 13;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("Parâmetro '{0}' não reconhecido; parâmetros possíveis:\r\n\r\npapel, ult, var, cor comp, vl comp, cor vend, vl vend, abert, min, max, fech ant, n neg", pElemento));
                }
            }

            if (lIndiceElemento >= 0 && lIndiceElemento < 14)
            {
                lCotacao = SM_COTACAO(pInstrumento);

                if (lCotacao.Length > lIndiceElemento)
                {
                    lRetorno = lCotacao[lIndiceElemento];
                }
            }

            return lRetorno;
        }

        #endregion

        #region Event Handlers

        private void gMdsHttpClient_OnOfertasEvent(object sender, MdsOfertasEventArgs e)
        {
            string lInstrumento = e.ofertas.cabecalho.instrumento.ToUpper();

            if (gLivrosDeOfertas.ContainsKey(lInstrumento))
            {
                gLivrosDeOfertas[lInstrumento].AtualizarOfertasCompra(e.ofertas);
                gLivrosDeOfertas[lInstrumento].AtualizarOfertasVenda(e.ofertas);
            }
        }

        private void gMdsHttpClient_OnNegociosEvent(object sender, MdsNegociosEventArgs e)
        {
            string lInstrumento = e.negocios.cabecalho.instrumento.ToUpper();

            if (gNegocios.ContainsKey(lInstrumento))
            {
                gNegociosDados[lInstrumento] = e.negocios.negocio;
            }
        }


        #endregion

        #region Funções para registro COM automático

        [ComRegisterFunctionAttribute]
        public static void RegisterFunction(Type type)
        {
            Registry.ClassesRoot.CreateSubKey(GetSubKeyName(type));

            RegistryKey key = Registry.ClassesRoot.CreateSubKey("CLSID\\{" + type.GUID.ToString().ToUpper() + "}\\InprocServer32");

            key.SetValue("", @"C:\Windows\System32\mscoree.dll"); 
        }

        [ComUnregisterFunctionAttribute]
        public static void UnregisterFunction(Type type)
        {
            Registry.ClassesRoot.DeleteSubKey(GetSubKeyName(type), false);
        }

        private static string GetSubKeyName(Type type)
        {
            string s = @"CLSID\{" + type.GUID.ToString().ToUpper() + @"}\Programmable";
            return s;
        }

        #endregion
    }
}
