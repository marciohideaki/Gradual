using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
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
        public string jQAMC { get { return (this.HeaderTipoDeBolsa != "BV" || string.IsNullOrEmpty(this.QuantidadeAcumuladaMelhorCompra)) ? "n/d" : this.QuantidadeAcumuladaMelhorCompra; } }

        [JsonIgnore]
        public string QuantidadeAcumuladaMelhorVenda { get; set; }

        /// <summary>
        /// QuantidadeMelhorPrecoCompra
        /// </summary>
        public string jQAMV { get { return (this.HeaderTipoDeBolsa != "BV" || string.IsNullOrEmpty(this.QuantidadeAcumuladaMelhorVenda)) ? "n/d" : this.QuantidadeAcumuladaMelhorVenda; } }

        [JsonIgnore]
        public string Preco { get; set; }

        /// <summary>
        /// Preco
        /// </summary>
        public string jPC { get { return this.Preco; } }

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

            //<---              header             --->data    hora     comprad vend    preco        quantidade  max dia      min dia      vol          nneg    VvFech   E
            //0 2 4       12       21                  41      49       58      66      74           87          99           112          125          138      147      156         168         180          193                   215          228          241     249          262         274                 293
            //NEBV20100702162034355PETR4               201007021618470000000013100000027000000026,800000000000200000000026,860000000026,5200209283409,0000010513 00001,322                        0000000029.900000000026.45C000000950000000026,79000000002800V000000270000000026,80000000000200C000053.73000000000020110221 
            //NEBV20100702162238788PETR4               201007021620510000000002700000131000000026,850000000000200000000026,870000000026,5200211588300,0000010621 00001,512                        0000000029.900000000026.45C000000270000000026,85000000000200V000000080000000026,86000000000200

            try
            {
                if (!string.IsNullOrEmpty(pMensagem))
                {
                    if (pMensagem.Length >= (0 + 2)) this.HeaderTipoMensagem = pMensagem.Substring(0, 2);
                    if (pMensagem.Length >= (2 + 2)) this.HeaderTipoDeBolsa = pMensagem.Substring(2, 2);
                    if (pMensagem.Length >= (4 + 8)) this.HeaderData = pMensagem.Substring(4, 8);
                    if (pMensagem.Length >= (12 + 9)) this.HeaderHora = pMensagem.Substring(12, 9);
                    if (pMensagem.Length >= (21 + 20)) this.CodigoNegocio = pMensagem.Substring(21, 20).Trim();

                    if (pMensagem.Length >= (41 + 8)) this.DataNeg = pMensagem.Substring(41, 8);
                    if (pMensagem.Length >= (49 + 6)) this.HoraNeg = pMensagem.Substring(49, 6);

                    if (pMensagem.Length >= (74 + 13)) this.Preco = pMensagem.Substring(74, 13).TrimStart('0');
                    if (pMensagem.Length >= (87 + 12)) this.Quantidade = pMensagem.Substring(87, 12).TrimStart('0');

                    if (pMensagem.Length >= (99 + 13)) this.MaxDia = pMensagem.Substring(99, 13).TrimStart('0');
                    if (pMensagem.Length >= (112 + 13)) this.MinDia = pMensagem.Substring(112, 13).TrimStart('0');

                    if (pMensagem.Length >= (125 + 13)) this.VolumeAcumulado = pMensagem.Substring(125, 13).TrimStart('0');

                    if (pMensagem.Length >= (138 + 8)) this.NumNegocio = pMensagem.Substring(138, 8).TrimStart('0');

                    if (pMensagem.Length >= (146 + 1)) this.IndicadorVariacao = pMensagem.Substring(146, 1).Trim();

                    if (pMensagem.Length >= (147 + 8)) this.Variacao = pMensagem.Substring(147, 8).TrimStart('0');

                    if (pMensagem.Length >= (228 + 12)) this.QuantidadeMelhorPrecoCompra = pMensagem.Substring(228, 12).TrimStart('0');
                    if (pMensagem.Length >= (262 + 12)) this.QuantidadeMelhorPrecoVenda = pMensagem.Substring(262, 12).TrimStart('0');

                    if (pMensagem.Length >= (156 + 12)) this.QuantidadeAcumuladaMelhorCompra = pMensagem.Substring(156, 12).TrimStart('0');
                    if (pMensagem.Length >= (168 + 12)) this.QuantidadeAcumuladaMelhorVenda = pMensagem.Substring(168, 12).TrimStart('0');

                    if (pMensagem.Length >= (155 + 1)) this.EstadoDoPapel = pMensagem.Substring(155, 1);

                    if (pMensagem.Length >= (180 + 13)) this.ValorAbertura = pMensagem.Substring(180, 13).TrimStart('0').Replace('.', ',');
                    if (pMensagem.Length >= (193 + 13)) this.ValorFechamento = pMensagem.Substring(193, 13).TrimStart('0').Replace('.', ',');

                    if (pMensagem.Length >= (274 + 1)) this.IndicadorDeOpcao = pMensagem.Substring(274, 1);
                    if (pMensagem.Length >= (275 + 19)) this.PrecoDeExercico = pMensagem.Substring(275, 19).TrimStart('0').Replace('.', ',');
                    if (pMensagem.Length >= (294 + 8)) this.DataDeExercico = pMensagem.Substring(294, 8);

                    if (pMensagem.Length >= (215 + 13)) this.MelhorPrecoCompra = pMensagem.Substring(215, 13).TrimStart('0');
                    if (pMensagem.Length >= (249 + 13)) this.MelhorPrecoVenda = pMensagem.Substring(249, 13).TrimStart('0');


                    if (gTipo == "LivroNegocio")
                    {
                        if (pMensagem.Length >= (58 + 8)) lValor = pMensagem.Substring(58, 8).TrimStart('0');
                    }
                    else
                    {
                        if (pMensagem.Length >= (207 + 8)) lValor = pMensagem.Substring(207, 8).TrimStart('0');
                    }

                    this.CodCorretoraCompradora = lValor;

                    //if (!string.IsNullOrEmpty(lValor)) this.CorretoraCompradora = DadosDeAplicacao.NomesDasCorretoras.ContainsKey(Convert.ToInt32(lValor)) ? DadosDeAplicacao.NomesDasCorretoras[Convert.ToInt32(lValor)] : lValor;

                    if (gTipo == "LivroNegocio")
                    {
                        if (pMensagem.Length >= (66 + 8)) lValor = pMensagem.Substring(66, 8).TrimStart('0');
                    }
                    else
                    {
                        if (pMensagem.Length >= (241 + 8)) lValor = pMensagem.Substring(241, 8).TrimStart('0');
                    }

                    this.CodCorretoraVendedora = lValor;

                    //if (!string.IsNullOrEmpty(lValor)) this.CorretoraVendedora = DadosDeAplicacao.NomesDasCorretoras.ContainsKey(Convert.ToInt32(lValor)) ? DadosDeAplicacao.NomesDasCorretoras[Convert.ToInt32(lValor)] : lValor;

                    //    if(this.CorretoraCompradora != null)
                    //        this.CorretoraCompradora = this.CorretoraCompradora.TrimStart('0');

                    //    if(this.CorretoraVendedora != null)
                    //        this.CorretoraVendedora  = this.CorretoraVendedora.TrimStart('0');

                    if (!string.IsNullOrEmpty(this.Variacao) && this.Variacao[0] == ',') this.Variacao = '0' + this.Variacao;

                    if (!string.IsNullOrEmpty(this.MelhorPrecoCompra) && this.MelhorPrecoCompra[0] == ',') this.MelhorPrecoCompra = '0' + this.MelhorPrecoCompra;

                    if (!string.IsNullOrEmpty(this.MelhorPrecoVenda) && this.MelhorPrecoVenda[0] == ',') this.MelhorPrecoVenda = '0' + this.MelhorPrecoVenda;

                    if (!string.IsNullOrEmpty(this.ValorAbertura) && this.ValorAbertura[0] == ',') this.ValorAbertura = '0' + this.ValorAbertura;

                    if (!string.IsNullOrEmpty(this.ValorFechamento) && this.ValorFechamento[0] == ',') this.ValorFechamento = '0' + this.ValorFechamento;

                    if (!string.IsNullOrEmpty(this.VolumeAcumulado) && this.VolumeAcumulado[0] == ',') this.VolumeAcumulado = '0' + this.VolumeAcumulado;

                    if (!string.IsNullOrEmpty(this.Preco) && this.Preco[0] == ',') this.Preco = '0' + this.Preco;

                    if (!string.IsNullOrEmpty(this.Quantidade) && this.Quantidade[0] == ',') this.Quantidade = '0' + this.Quantidade;

                    if (!string.IsNullOrEmpty(this.PrecoDeExercico) && this.PrecoDeExercico[0] == ',') this.PrecoDeExercico = '0' + this.PrecoDeExercico;

                    if (!string.IsNullOrEmpty(this.MaxDia) && this.MaxDia[0] == ',') this.MaxDia = '0' + this.MaxDia;

                    if (!string.IsNullOrEmpty(this.MinDia) && this.MinDia[0] == ',') this.MinDia = '0' + this.MinDia;

                    if (this.HoraNeg.Length >= 6) this.HoraNeg = this.HoraNeg.Insert(4, ":").Insert(2, ":");

                    if (!string.IsNullOrEmpty(this.Preco)) this.Preco = this.Preco.Substring(0, this.Preco.IndexOf(',') + 3);

                    if (!string.IsNullOrEmpty(this.PrecoDeExercico)) this.PrecoDeExercico = this.PrecoDeExercico.Substring(0, this.PrecoDeExercico.IndexOf(',') + 3);

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
            catch //(Exception ex)
            {
                //ILog lLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

                //lLogger.ErrorFormat("TransporteMensagemDeNegocio: Erro ao processar a mensagem [{0}]\r\n{1}\r\n{2}"
                //                    , pMensagem
                //                    , ex.Message
                //                    , ex.StackTrace);
            }
        }

        #endregion

        #region Construtor

        public TransporteMensagemDeNegocio()
        {
            this.DataHoraDeCriacao = DateTime.Now;
        }

        public TransporteMensagemDeNegocio(string pMensagem)
            : this()
        {
            this.ProcessarMensagem(pMensagem);
        }

        public TransporteMensagemDeNegocio(string pMensagem, string pTipo)
            : this()
        {
            gTipo = "LivroNegocio";
            this.ProcessarMensagem(pMensagem);
        }

        #endregion
    }
}