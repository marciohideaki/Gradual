using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.Termo.Lib.Info;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www
{
    public class TransporteAcompanhamentoDeTermo
    {
        #region Propriedades

        public string DataExecucao { get; set; }

        public string DataVencimento { get; set; }
        
        public string DataSolicitacao { get; set; }
        
        [JsonIgnore]
        public DateTime DateSolicitacao { get; set; }

        public string IdCliente { get; set; }

        public string IdAssessor { get; set; }

        public string IdOrdemTermo { get; set; }

        public string StatusOrdemTermo { get; set; }

        public string IdTaxa { get; set; }

        public string TipoSolicitacao { get; set; }

        public string Instrumento { get; set; }

        public string LucroPrejuizo { get; set; }

        public string PrecoDireto { get; set; }

        public string PrecoLimite { get; set; }

        public string PrecoMaximo { get; set; }

        public string PrecoMercado { get; set; }

        public string PrecoMinimo { get; set; }

        public string PrecoTermo { get; set; }

        public string Quantidade { get; set; }

        public string TotalMercado { get; set; }

        public string TotalTermo { get; set; }

        public string ValorTaxa { get; set; }

        public string ClasseDaLinha { get; set; }

        public string MargemRequerida { get; set; }

        public string BotaoLucroPrejuizo
        {
            get
            {
                string lRetorno = "";

                if (this.StatusOrdemTermo == "Executada")
                {
                    lRetorno = "<button class='IconButton IconeInfo' title='Acompanhe' onclick='return AcompanhamentoDeOrdemOnLine_Acompanhe(this)' style='float:right;margin-top:-4px;'></button>";
                }
                else if (this.StatusOrdemTermo == "NDA")
                {
                    //ignora o status NDA
                }
                else
                {
                    lRetorno = "N/C";
                }

                return lRetorno;
            }
        }

        public string BotaoLiquidarRolar
        {
            get
            {
                string lRetorno = "n/c";
                /*
                if (this.StatusOrdemTermo == "SolicitacaoExecutada")
                {
                    lRetorno = "<a href='#' onclick='return lnkTermo_Liquidar_Click(this)'>liquidar</a>&nbsp;&nbsp;<a href='#' onclick='return lnkTermo_Liquidar_Click(this)'>rolar</a>";
                }*/

                // liquidação/rolagem só no HB...

                return lRetorno;
            }
        }
        
        public string BotaoEfetuar
        {
            get
            {
                string lRetorno = "&nbsp;";

                if (this.StatusOrdemTermo == "SolicitacaoEnviada")
                {
                    if (this.TipoSolicitacao == "Alteracao" || this.TipoSolicitacao == "Rolagem" || this.TipoSolicitacao == "Liquidacao" || this.TipoSolicitacao == "NovoTermo")
                    {
                        lRetorno = "<button title='Efetuar' onclick='return  lnkTermo_Efetuar_Click(this)'>Efetuar</button>";
                    }
                }

                return lRetorno;
            }
        }

        public string BotaoCancelar
        {
            get
            {
                string lRetorno = "";

                if (this.StatusOrdemTermo == "SolicitacaoEnviada")
                {
                    lRetorno = "<button title='Cancelar' onclick='return  lnkTermo_Cancelar_Click(this)'>Cancelar</button>";
                }

                return lRetorno;
            }
        }

        #endregion

        #region Construtores

        public TransporteAcompanhamentoDeTermo()
        {
        }
        
        public TransporteAcompanhamentoDeTermo(AcompanhamentoOrdemTermoInfo pAcompanhamento)
        {
            this.DataExecucao       = pAcompanhamento.DataExecucao.ToString("dd/MM/yy");
            this.DataVencimento     = pAcompanhamento.DataVencimento.ToString("dd/MM/yy");
            this.DataSolicitacao    = pAcompanhamento.DataSolicitacao.ToString("dd/MM/yy HH:mm");
            this.DateSolicitacao    = pAcompanhamento.DataSolicitacao;
            this.IdCliente          = pAcompanhamento.IdCliente.ToCodigoClienteFormatado();
            this.IdAssessor         = pAcompanhamento.IdAssessor.ToString();
            this.IdOrdemTermo       = pAcompanhamento.IdOrdemTermo.ToString();

            this.StatusOrdemTermo   = pAcompanhamento.IdStatusOrdemTermo.ToString();

            this.IdTaxa             = pAcompanhamento.IdTaxa.ToString();

            this.TipoSolicitacao    = pAcompanhamento.IdTipoSolicitacao.ToString();

            this.Instrumento        = pAcompanhamento.Instrumento;

            this.LucroPrejuizo      = pAcompanhamento.LucroPrejuizo.ToString("N2");
            this.PrecoDireto        = pAcompanhamento.PrecoDireto.ToString("N2");
            this.PrecoLimite        = pAcompanhamento.PrecoLimite.ToString("N2");
            this.PrecoMaximo        = pAcompanhamento.PrecoMaximo.ToString("N2");
            this.PrecoMercado       = pAcompanhamento.PrecoMercado.ToString("N2");
            this.PrecoMinimo        = pAcompanhamento.PrecoMinimo.ToString("N2");
            this.PrecoTermo         = pAcompanhamento.PrecoTermo.ToString("N2");
            this.Quantidade         = pAcompanhamento.Quantidade.ToString();
            this.TotalMercado       = pAcompanhamento.TotalMercado.ToString("N2");
            this.TotalTermo         = pAcompanhamento.TotalTermo.ToString("N2");
            this.ValorTaxa          = pAcompanhamento.ValorTaxa.ToString("N2");

            this.MargemRequerida = pAcompanhamento.MargemRequerida.ToString("N2");

            switch (pAcompanhamento.IdStatusOrdemTermo)
            {
                case Gradual.OMS.Termo.Lib.EnumStatusTermo.NDA:

                    this.ClasseDaLinha = "NOVA";
                    break;

                case Gradual.OMS.Termo.Lib.EnumStatusTermo.SolicitacaoAceita:

                    this.ClasseDaLinha = "NOVA";
                    break;

                case Gradual.OMS.Termo.Lib.EnumStatusTermo.SolicitacaoAnalise:

                    this.ClasseDaLinha = "NOVA";
                    break;

                case Gradual.OMS.Termo.Lib.EnumStatusTermo.SolicitacaoEnviada:

                    this.ClasseDaLinha = "DISPARADA";
                    break;

                case Gradual.OMS.Termo.Lib.EnumStatusTermo.SolicitacaoExecutada:

                    this.ClasseDaLinha = "EXECUTADA";
                    break;

            }
            //this.ClasseDaLinha      = pAcompanhamento.ClasseDaLinha.ToString();
        }

        #endregion
    }
}