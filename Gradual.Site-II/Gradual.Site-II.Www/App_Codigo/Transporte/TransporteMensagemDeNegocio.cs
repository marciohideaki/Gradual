using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Newtonsoft.Json;
using log4net;

namespace Gradual.Site.Www
{
    [Serializable]
    public class TransporteMensagemDeNegocio
    {
        #region Globais

        private CultureInfo gInfo = new CultureInfo("pt-BR");
        private string gTipo;

        #endregion

        #region Propriedades

        [JsonIgnore]
        public string HeaderTipoMensagem { get; set; }

        /// <summary>
        /// HeaderTipoMensagem
        /// </summary>
        public string jHTM { get { return this.HeaderTipoMensagem; } }

        [JsonIgnore]
        public string HeaderTipoDeBolsa { get; set; }

        /// <summary>
        /// HeaderTipoDeBolsa
        /// </summary>
        public string jHTB { get { return this.HeaderTipoDeBolsa; } }

        [JsonIgnore]
        public string HeaderData { get; set; }

        /// <summary>
        /// HeaderData
        /// </summary>
        public string jHDT { get { return this.HeaderData; } }

        [JsonIgnore]
        public string HeaderHora { get; set; }

        /// <summary>
        /// HeaderHora
        /// </summary>
        public string jHHR { get { return this.HeaderHora; } }

        [JsonIgnore]
        public string CodigoNegocio { get; set; }

        /// <summary>
        /// CodigoNegocio
        /// </summary>
        public string jCN { get { return this.CodigoNegocio; } }

        [JsonIgnore]
        public string DataNeg { get; set; }

        /// <summary>
        /// DataNeg
        /// </summary>
        public string jDN { get { return this.DataNeg; } }

        [JsonIgnore]
        public string HoraNeg { get; set; }

        /// <summary>
        /// HoraNeg
        /// </summary>
        public string jHN { get { return this.HoraNeg; } }

        [JsonIgnore]
        public string CodCorretoraCompradora { get; set; }

        /// <summary>
        /// CodCorretoraCompradora
        /// </summary>
        public string jCC { get { return this.CodCorretoraCompradora; } }

        [JsonIgnore]
        public string CodCorretoraVendedora { get; set; }

        /// <summary>
        /// CodCorretoraVendedora
        /// </summary>
        public string jCV { get { return this.CodCorretoraVendedora; } }

        [JsonIgnore]
        public string CorretoraCompradora { get; set; }

        /// <summary>
        /// CorretoraCompradora
        /// </summary>
        public string jCCN { get { return this.CorretoraCompradora; } }

        [JsonIgnore]
        public string CorretoraVendedora { get; set; }

        /// <summary>
        /// CorretoraVendedora
        /// </summary>
        public string jCVN { get { return this.CorretoraVendedora; } }

        [JsonIgnore]
        public string MelhorPrecoCompra { get; set; }

        /// <summary>
        /// MelhorPrecoCompra
        /// </summary>
        public string jMPC { get { return this.MelhorPrecoCompra; } }

        [JsonIgnore]
        public string MelhorPrecoVenda { get; set; }

        /// <summary>
        /// MelhorPrecoVenda
        /// </summary>
        public string jMPV { get { return this.MelhorPrecoVenda; } }

        [JsonIgnore]
        public string QuantidadeMelhorPrecoCompra { get; set; }

        /// <summary>
        /// QuantidadeMelhorPrecoCompra
        /// </summary>
        public string jQMPC { get { return this.QuantidadeMelhorPrecoCompra; } }

        [JsonIgnore]
        public string QuantidadeMelhorPrecoVenda { get; set; }

        /// <summary>
        /// QuantidadeMelhorPrecoVenda
        /// </summary>
        public string jQMPV { get { return this.QuantidadeMelhorPrecoVenda; } }
        
        [JsonIgnore]
        public string QuantidadeAcumuladaMelhorCompra { get; set; }

        /// <summary>
        /// QuantidadeAcumuladaMelhorCompra
        /// </summary>
        public string jQAMC { get { return (this.HeaderTipoDeBolsa != "BV" || string.IsNullOrEmpty(this.QuantidadeAcumuladaMelhorCompra) ) ? "n/d" : this.QuantidadeAcumuladaMelhorCompra; } }
        
        [JsonIgnore]
        public string QuantidadeAcumuladaMelhorVenda { get; set; }

        /// <summary>
        /// QuantidadeMelhorPrecoCompra
        /// </summary>
        public string jQAMV { get { return (this.HeaderTipoDeBolsa != "BV" || string.IsNullOrEmpty(this.QuantidadeAcumuladaMelhorVenda) ) ? "n/d" : this.QuantidadeAcumuladaMelhorVenda; } }
        
        [JsonIgnore]
        public string Preco { get; set; }

        /// <summary>
        /// Preco
        /// </summary>
        public string jPC { get { return this.Preco; } }

        [JsonIgnore]
        public string PrecoMedio { get; set; }
        
        /// <summary>
        /// Preco Teórico de Abertura
        /// </summary>
        public string jPTA { get { return this.PrecoTeoricoAbertura; } }

        [JsonIgnore]
        public string PrecoTeoricoAbertura { get; set; }
        
        /// <summary>
        /// Variação Teórica
        /// </summary>
        public string jVTO { get { return this.VariacaoTeorica; } }

        [JsonIgnore]
        public string VariacaoTeorica { get; set; }

        /// <summary>
        /// Data Teórica
        /// </summary>
        public string jDTO { get { return this.DataTeorica; } }

        [JsonIgnore]
        public string DataTeorica { get; set; }

        /// <summary>
        /// Preco
        /// </summary>
        public string jPM { get { return this.PrecoMedio; } }

        [JsonIgnore]
        public string Quantidade { get; set; }

        /// <summary>
        /// Quantidade
        /// </summary>
        public string jQT { get { return this.Quantidade; } }

        [JsonIgnore]
        public string MaxDia { get; set; }

        /// <summary>
        /// MaxDia
        /// </summary>
        public string jXD { get { return this.MaxDia; } }

        [JsonIgnore]
        public string MinDia { get; set; }

        /// <summary>
        /// MinDia
        /// </summary>
        public string jND { get { return this.MinDia; } }

        [JsonIgnore]
        public string VolumeAcumulado { get; set; }

        /// <summary>
        /// VolumeAcumulado
        /// </summary>
        public string jLA { get { return this.VolumeAcumulado; } }

        [JsonIgnore]
        public string NumNegocio { get; set; }

        /// <summary>
        /// NumNegocio
        /// </summary>
        public string jNN { get { return this.NumNegocio; } }

        [JsonIgnore]
        public string IndicadorVariacao { get; set; }

        /// <summary>
        /// IndicadorVariacao
        /// </summary>
        public string jIV { get { return this.IndicadorVariacao; } }

        [JsonIgnore]
        public string Variacao { get; set; }

        /// <summary>
        /// Variacao
        /// </summary>
        public string jVR { get { return this.Variacao; } }

        [JsonIgnore]
        public string EstadoDoPapel { get; set; }

        /// <summary>
        /// EstadoDoPapel
        /// </summary>
        public string jEP { get { return this.EstadoDoPapel; } }

        [JsonIgnore]
        public string ValorAbertura { get; set; }

        /// <summary>
        /// ValorAbertura
        /// </summary>
        public string jVA { get { return this.ValorAbertura; } }

        [JsonIgnore]
        public string ValorFechamento { get; set; }

        /// <summary>
        /// ValorFechamento
        /// </summary>
        public string jVF { get { return this.ValorFechamento; } }

        [JsonIgnore]
        public List<string> ErrosDeProcessamento { get; set; }

        [JsonIgnore]
        public string IndicadorDeOpcao { get; set; }
        
        /// <summary>
        /// IndicadorDeOpcao
        /// </summary>
        public string jIO { get { return this.IndicadorDeOpcao; } }

        [JsonIgnore]
        public string PrecoDeExercico { get; set; }
        
        /// <summary>
        /// PrecoDeExercico
        /// </summary>
        public string jPE { get { return this.PrecoDeExercico; } }

        [JsonIgnore]
        public string DataDeExercico { get; set; }
        
        /// <summary>
        /// DataDeExercico
        /// </summary>
        public string jDE { get { return this.DataDeExercico; } }

        [JsonIgnore]
        public string MensagemOriginal { get; set; }

        [JsonIgnore]
        public DateTime DataHoraDeCriacao { get; set; }

        #endregion

        #region Métodos

        private void AdicionarErroDeProcessamento(string pPropriedade, Exception pException)
        {
            if (this.ErrosDeProcessamento == null)
                this.ErrosDeProcessamento = new List<string>();

            this.ErrosDeProcessamento.Insert(0, string.Format("Erro ao processar {0}: {1}", pPropriedade, pException.Message));
        }

        public void ProcessarMensagem(string pMensagem)
        {
            string lValor = "[]";

            this.MensagemOriginal = pMensagem;

            // Exemplos de mensagens:

            //<---              header             --->data    hora     comprad vend    preco        quantidade  max dia      min dia      vol          nneg    VvFech   E            ac vd       qtac pap    prc med      prc teo ab   iVarTeo  dt hr teo     v abert      v fech                mpc          qmpc                 mpv          qmpv        i pc exerc          dt exerc
            //0 2 4       12       21                  41      49       58      66      74           87          99           112          125          138      147      156         168         180         192          205          218      227           241          254                   276          289          302     310          323         335                 354
            //NEBV20131111105311009PETR4               201311111053110000000004500000059000000019,660000000001000000000019,770000000019,500000005294182200002142 00000,102000000004100000000005300000002693400000000019,660000000019,660 00000,10201311111053110000000019.660000000019.64C000000770000000019,66000000000700V000000590000000019,67000000000100X000000.00000000000000000000

            try
            {
                if (!string.IsNullOrEmpty(pMensagem))
                {
                    if (pMensagem.Length >= (  0 +  2))     this.HeaderTipoMensagem = pMensagem.Substring(0, 2);
                    if (pMensagem.Length >= (  2 +  2))     this.HeaderTipoDeBolsa = pMensagem.Substring(2, 2);
                    if (pMensagem.Length >= (  4 +  8))     this.HeaderData = pMensagem.Substring(4, 8);
                    if (pMensagem.Length >= ( 12 +  9))     this.HeaderHora = pMensagem.Substring(12, 9);
                    if (pMensagem.Length >= ( 21 + 20))     this.CodigoNegocio = pMensagem.Substring(21, 20).Trim();

                    if (pMensagem.Length >= ( 41 +  8))     this.DataNeg = pMensagem.Substring(41, 8);
                    if (pMensagem.Length >= ( 49 +  6))     this.HoraNeg = pMensagem.Substring(49, 6);

                    if (pMensagem.Length >= ( 74 + 13))     this.Preco = pMensagem.Substring(74, 13).TrimStart('0');
                    if (pMensagem.Length >= ( 87 + 12))     this.Quantidade = pMensagem.Substring(87, 12).TrimStart('0');

                    if (pMensagem.Length >= ( 99 + 13))     this.MaxDia = pMensagem.Substring(99, 13).TrimStart('0');
                    if (pMensagem.Length >= (112 + 13))     this.MinDia = pMensagem.Substring(112, 13).TrimStart('0');

                    if (pMensagem.Length >= (125 + 13))     this.VolumeAcumulado = pMensagem.Substring(125, 13).TrimStart('0');

                    if (pMensagem.Length >= (138 +  8))     this.NumNegocio = pMensagem.Substring(138, 8).TrimStart('0');

                    if (pMensagem.Length >= (146 +  1))     this.IndicadorVariacao = pMensagem.Substring(146, 1).Trim();

                    if (pMensagem.Length >= (147 +  8))     this.Variacao = pMensagem.Substring(147, 8).TrimStart('0');

                    
                    if (pMensagem.Length >= (289 + 12))     this.QuantidadeMelhorPrecoCompra = pMensagem.Substring(289, 12).TrimStart('0');
                    if (pMensagem.Length >= (323 + 12))     this.QuantidadeMelhorPrecoVenda  = pMensagem.Substring(323, 12).TrimStart('0'); 

                    if (pMensagem.Length >= (156 + 12))     this.QuantidadeAcumuladaMelhorCompra = pMensagem.Substring(156, 12).TrimStart('0');
                    if (pMensagem.Length >= (168 + 12))     this.QuantidadeAcumuladaMelhorVenda  = pMensagem.Substring(168, 12).TrimStart('0');


                    if (pMensagem.Length >= (155 +  1))     this.EstadoDoPapel = pMensagem.Substring(155, 1);


                    if (pMensagem.Length >= (241 + 13))     this.ValorAbertura   = pMensagem.Substring(241, 13).TrimStart('0').Replace('.', ',');
                    if (pMensagem.Length >= (254 + 13))     this.ValorFechamento = pMensagem.Substring(254, 13).TrimStart('0').Replace('.', ',');


                    if (pMensagem.Length >= (335 +  1))     this.IndicadorDeOpcao = pMensagem.Substring(335, 1);
                    if (pMensagem.Length >= (336 + 19))     this.PrecoDeExercico  = pMensagem.Substring(336, 19).TrimStart('0').Replace('.', ',');
                    if (pMensagem.Length >= (343 +  8))     this.DataDeExercico   = pMensagem.Substring(343, 8);

                    if (pMensagem.Length >= (276 + 13))     this.MelhorPrecoCompra = pMensagem.Substring(276, 13).TrimStart('0').Trim();
                    if (pMensagem.Length >= (310 + 13))     this.MelhorPrecoVenda  = pMensagem.Substring(310, 13).TrimStart('0').Trim();
                    
                    if (pMensagem.Length >= (192 + 13))     this.PrecoMedio = pMensagem.Substring(192, 12).TrimStart('0');  //era length 13, itrei a ultima casa 

                    if (gTipo == "LivroNegocio")
                    {
                        if (pMensagem.Length >= ( 58 + 8))   lValor = pMensagem.Substring(58, 8).TrimStart('0');
                    }
                    else
                    {
                        if (pMensagem.Length >= (268 + 8))   lValor = pMensagem.Substring(268, 8).TrimStart('0');
                    }

                    this.CodCorretoraCompradora = lValor;

                    if(!string.IsNullOrEmpty(lValor))        this.CorretoraCompradora = DadosDeAplicacao.NomesDasCorretoras.ContainsKey(Convert.ToInt32(lValor)) ? DadosDeAplicacao.NomesDasCorretoras[Convert.ToInt32(lValor)] : lValor;

                    if (gTipo == "LivroNegocio")
                    {
                        if (pMensagem.Length >= ( 66 + 8))   lValor = pMensagem.Substring(66, 8).TrimStart('0');
                    }
                    else
                    {
                        if (pMensagem.Length >= (302 + 8))   lValor = pMensagem.Substring(302, 8).TrimStart('0');
                    }

                    if (pMensagem.Length >= (205 + 13))     this.PrecoTeoricoAbertura = pMensagem.Substring(205, 12).TrimStart('0');
                    
                    if (pMensagem.Length >= (218 + 9))     this.VariacaoTeorica = pMensagem.Substring(218, 1).Trim() + pMensagem.Substring(219, 8).TrimStart('0');
                    
                    if (pMensagem.Length >= (227 + 14))     this.DataTeorica = pMensagem.Substring(227, 14).Trim().TrimStart('0');

                    this.CodCorretoraVendedora = lValor;

                    if(!string.IsNullOrEmpty(lValor))       this.CorretoraVendedora = DadosDeAplicacao.NomesDasCorretoras.ContainsKey(Convert.ToInt32(lValor)) ? DadosDeAplicacao.NomesDasCorretoras[Convert.ToInt32(lValor)] : lValor;

                    //    if(this.CorretoraCompradora != null)
                    //        this.CorretoraCompradora = this.CorretoraCompradora.TrimStart('0');

                    //    if(this.CorretoraVendedora != null)
                    //        this.CorretoraVendedora  = this.CorretoraVendedora.TrimStart('0');


                    if (!string.IsNullOrEmpty(this.Variacao)              && this.Variacao[0]          == ',')     this.Variacao = '0' + this.Variacao;

                    if (!string.IsNullOrEmpty(this.MelhorPrecoCompra)     && this.MelhorPrecoCompra[0] == ',')     this.MelhorPrecoCompra = '0' + this.MelhorPrecoCompra;

                    if (!string.IsNullOrEmpty(this.MelhorPrecoVenda)      && this.MelhorPrecoVenda[0]  == ',')     this.MelhorPrecoVenda  = '0' + this.MelhorPrecoVenda;

                    if (!string.IsNullOrEmpty(this.ValorAbertura)         && this.ValorAbertura[0]     == ',')     this.ValorAbertura = '0' + this.ValorAbertura;

                    if (!string.IsNullOrEmpty(this.ValorFechamento)       && this.ValorFechamento[0]   == ',')     this.ValorFechamento = '0' + this.ValorFechamento;

                    if (!string.IsNullOrEmpty(this.VolumeAcumulado)       && this.VolumeAcumulado[0]   == ',')     this.VolumeAcumulado = '0' + this.VolumeAcumulado;

                    if (!string.IsNullOrEmpty(this.Preco)                 && this.Preco[0]             == ',')     this.Preco = '0' + this.Preco;

                    if (!string.IsNullOrEmpty(this.Quantidade)            && this.Quantidade[0]        == ',')     this.Quantidade = '0' + this.Quantidade;

                    if (!string.IsNullOrEmpty(this.PrecoDeExercico)       && this.PrecoDeExercico[0]   == ',')     this.PrecoDeExercico = '0' + this.PrecoDeExercico;

                    if (!string.IsNullOrEmpty(this.PrecoMedio)            && this.PrecoMedio[0]        == ',')     this.PrecoMedio = '0' + this.PrecoMedio;

                    if (!string.IsNullOrEmpty(this.MaxDia)                && this.MaxDia[0]            == ',')     this.MaxDia = '0' + this.MaxDia;

                    if (!string.IsNullOrEmpty(this.MinDia)                && this.MinDia[0]            == ',')     this.MinDia = '0' + this.MinDia;
                    
                    if (!string.IsNullOrEmpty(this.VariacaoTeorica)       && this.VariacaoTeorica[0]   == ',')     this.VariacaoTeorica = '0' + this.VariacaoTeorica;

                    if (!string.IsNullOrEmpty(this.PrecoTeoricoAbertura)  && this.PrecoTeoricoAbertura[0] == ',')  this.PrecoTeoricoAbertura = '0' + this.PrecoTeoricoAbertura;

                    if (this.MelhorPrecoCompra == "ABERTURA") this.MelhorPrecoCompra = "ABERT";

                    if (this.MelhorPrecoVenda  == "ABERTURA") this.MelhorPrecoVenda  = "ABERT";

                    if (string.IsNullOrEmpty(this.NumNegocio)) this.NumNegocio = "0";
                    
                    if (string.IsNullOrEmpty(this.VolumeAcumulado)) this.VolumeAcumulado = "0";

                    if (this.HoraNeg.Length >= 6)            this.HoraNeg = this.HoraNeg.Insert(4, ":").Insert(2, ":");

                    if (!string.IsNullOrEmpty(this.Preco))   this.Preco = this.Preco.Substring(0, this.Preco.IndexOf(',') + 3);

                    if (!string.IsNullOrEmpty(this.PrecoDeExercico))   this.PrecoDeExercico = this.PrecoDeExercico.Substring(0, this.PrecoDeExercico.IndexOf(',') + 3);

                    if (string.IsNullOrEmpty(this.IndicadorVariacao))
                    {
                        this.Variacao = "+" + this.Variacao;
                    }
                    else
                    {
                        this.Variacao = "-" + this.Variacao;
                    }
                }
            }
            catch (Exception ex)
            {
                ILog lLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

                lLogger.ErrorFormat("TransporteMensagemDeNegocio: Erro ao processar a mensagem [{0}]\r\n{1}\r\n{2}"
                                    , pMensagem
                                    , ex.Message
                                    , ex.StackTrace);
            }
        }

        #endregion

        #region Construtor

        public TransporteMensagemDeNegocio()
        {
            this.DataHoraDeCriacao = DateTime.Now;
        }

        public TransporteMensagemDeNegocio(string pMensagem) : this()
        {
            this.ProcessarMensagem(pMensagem);
        }

        public TransporteMensagemDeNegocio(string pMensagem, string pTipo) : this()
        {
            gTipo = "LivroNegocio";
            this.ProcessarMensagem(pMensagem);
        }

        #endregion
    }
}