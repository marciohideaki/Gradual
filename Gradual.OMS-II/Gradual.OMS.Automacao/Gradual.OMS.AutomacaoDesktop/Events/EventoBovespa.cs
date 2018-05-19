using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.AutomacaoDesktop.Events
{
    public class EventoBovespa
    {
        string msgid;
        string tipo;
        string cabecalho;
        string corpo;
        string instrumento;

        public EventoBovespa( string msgid, 
    		                string tipo, 
    		                string cabecalho, 
    		                string corpo,
    		                string instrumento)
        {
            this.msgid = msgid;
            this.tipo = tipo;
            this.cabecalho = cabecalho;
            this.corpo = corpo;
            this.instrumento = instrumento;
        }

        public string MsgID
        {
    	    get {return msgid;}
        }

        public string Tipo
        {
            get { return tipo; }
        }

        public string Cabecalho
        {
    	    get {return cabecalho;}
        }

        public string Corpo
        {
    	    get{return corpo;}
        }

        public string Instrumento
        {
    	    get {return instrumento;}
        }
    
        public override string ToString()
        {
            return "codigo=" + msgid +
        	    " tipo=" + tipo +
        	    " cabecalho=" + cabecalho +
        	    " corpo=" + corpo +
        	    " instrumento=" + instrumento;
        }

        public const string BOVESPA_ULTIMO_PRECO_ABERTURA = "1";

        /*
	     * Layout do cabecalho da mensagem da Bovespa
	     */
	    public const int BOV_CABECALHO_RESERVADO1_INI = 0;
	    public const int BOV_CABECALHO_RESERVADO1_FIM = 3;
	    public const int BOV_CABECALHO_TIPO_MENSAGEM_INI = 3;
	    public const int BOV_CABECALHO_TIPO_MENSAGEM_FIM = 5;
	    public const int BOV_CABECALHO_RESERVADO2_INI = 5;
	    public const int BOV_CABECALHO_RESERVADO2_FIM = 11;
	    public const int BOV_CABECALHO_GRUPO_COTACAO_INI = 11;
	    public const int BOV_CABECALHO_GRUPO_COTACAO_FIM = 13;
	    public const int BOV_CABECALHO_CODIGO_PAPEL_INI = 13;
	    public const int BOV_CABECALHO_CODIGO_PAPEL_FIM = 25;
	    public const int BOV_CABECALHO_RESERVADO3_INI = 25;
	    public const int BOV_CABECALHO_RESERVADO3_FIM = 33;
	    public const int BOV_CABECALHO_DATA_EVENTO_INI = 33;
	    public const int BOV_CABECALHO_DATA_EVENTO_FIM = 41;
	    public const int BOV_CABECALHO_HORA_EVENTO_INI = 41;
	    public const int BOV_CABECALHO_HORA_EVENTO_FIM = 47;

	    /*
	     * Layout do corpo da mensagem de Negocio de Abertura (01) da Bovespa
	     */
	    public const int BOV01_QTD_NEGOCIADA_INI = 0;
	    public const int BOV01_QTD_NEGOCIADA_FIM = 12;
	    public const int BOV01_PRECO_NEGOCIO_FORMATO_INI = 12;
	    public const int BOV01_PRECO_NEGOCIO_FORMATO_FIM = 13;
	    public const int BOV01_PRECO_NEGOCIO_PRECO_INI = 13;
	    public const int BOV01_PRECO_NEGOCIO_PRECO_FIM = 26;
	    public const int BOV01_ID_CORRET_COMPRA_INI = 26;
	    public const int BOV01_ID_CORRET_COMPRA_FIM = 34;
	    public const int BOV01_ID_CORRET_VENDED_INI = 34;
	    public const int BOV01_ID_CORRET_VENDED_FIM = 42;
	    public const int BOV01_QTD_TIT_NEG_ACUM_DIA_INI = 42;
	    public const int BOV01_QTD_TIT_NEG_ACUM_DIA_FIM = 54;
	    public const int BOV01_RESERVADO_01_INI = 54;
	    public const int BOV01_RESERVADO_01_FIM = 65;
	    public const int BOV01_IND_ULTIMO_PRECO_ABERTURA_INI = 65;
	    public const int BOV01_IND_ULTIMO_PRECO_ABERTURA_FIM = 66;
	    public const int BOV01_RESERVADO_02_INI = 66;
	    public const int BOV01_RESERVADO_02_FIM = 68;
	    public const int BOV01_IND_VARIACAO_PRECO_ANTERIOR_INI = 68;
	    public const int BOV01_IND_VARIACAO_PRECO_ANTERIOR_FIM = 69;
	    public const int BOV01_NUMERO_NEGOCIO_INI = 69;
	    public const int BOV01_NUMERO_NEGOCIO_FIM = 76;
	    public const int BOV01_RESERVADO_03_INI = 76;
	    public const int BOV01_RESERVADO_03_FIM = 79;
	    public const int BOV01_SEGMENTO_MERCADO_INI = 79;
	    public const int BOV01_SEGMENTO_MERCADO_FIM = 81;
	    public const int BOV01_RESERVADO_04_INI = 81;
	    public const int BOV01_RESERVADO_04_FIM = 167;
	    public const int BOV01_DATA_NEGOCIO_INI = 167;
	    public const int BOV01_DATA_NEGOCIO_FIM = 175;
	    public const int BOV01_HORA_NEGOCIO_INI = 175;
	    public const int BOV01_HORA_NEGOCIO_FIM = 181;
	    public const int BOV01_ORIGEM_NEGOCIO_INI = 181;
	    public const int BOV01_ORIGEM_NEGOCIO_FIM = 182;
	    public const int BOV01_VAR_FECH_DIA_ANT_FMT_INI = 182;
	    public const int BOV01_VAR_FECH_DIA_ANT_FMT_FIM = 183;
	    public const int BOV01_VAR_FECH_DIA_ANT_VALOR_INI = 183;
	    public const int BOV01_VAR_FECH_DIA_ANT_VALOR_FIM = 196;

	    /*
	     * Layout do corpo da mensagem de Negocio (02) da Bovespa
	     */
	    public const int BOV02_QTD_NEGOCIADA_INI = 0;
	    public const int BOV02_QTD_NEGOCIADA_FIM = 12;
	    public const int BOV02_PRECO_NEGOCIO_FORMATO_INI = 12;
	    public const int BOV02_PRECO_NEGOCIO_FORMATO_FIM = 13;
	    public const int BOV02_PRECO_NEGOCIO_PRECO_INI = 13;
	    public const int BOV02_PRECO_NEGOCIO_PRECO_FIM = 26;
	    public const int BOV02_ID_CORRET_COMPRA_INI = 26;
	    public const int BOV02_ID_CORRET_COMPRA_FIM = 34;
	    public const int BOV02_ID_CORRET_VENDED_INI = 34;
	    public const int BOV02_ID_CORRET_VENDED_FIM = 42;
	    public const int BOV02_QTD_TIT_NEG_ACUM_DIA_INI = 42;
	    public const int BOV02_QTD_TIT_NEG_ACUM_DIA_FIM = 54;
	    public const int BOV02_MAIOR_PRECO_COT_DIA_FMT_INI = 62;
	    public const int BOV02_MAIOR_PRECO_COT_DIA_FMT_FIM = 63;
	    public const int BOV02_MAIOR_PRECO_COT_DIA_PRECO_INI = 63;
	    public const int BOV02_MAIOR_PRECO_COT_DIA_PRECO_FIM = 76;
	    public const int BOV02_MENOR_PRECO_COT_DIA_FMT_INI = 76;
	    public const int BOV02_MENOR_PRECO_COT_DIA_FMT_FIM = 77;
	    public const int BOV02_MENOR_PRECO_COT_DIA_PRECO_INI = 77;
	    public const int BOV02_MENOR_PRECO_COT_DIA_PRECO_FIM = 90;
	    public const int BOV02_TIPO_REGISTRO_INI = 90;
	    public const int BOV02_TIPO_REGISTRO_FIM = 92;
	    public const int BOV02_ORIGEM_NEGOCIO_INI = 96;
	    public const int BOV02_ORIGEM_NEGOCIO_FIM = 97;
	    public const int BOV02_IND_VAR_REL_PRECO_ANT_INI = 99;
	    public const int BOV02_IND_VAR_REL_PRECO_ANT_FIM = 100;
	    public const int BOV02_NUMERO_NEGOCIO_INI = 100;
	    public const int BOV02_NUMERO_NEGOCIO_FIM = 107;
	    public const int BOV02_SEGMENTO_MERCADO_INI = 110;
	    public const int BOV02_SEGMENTO_MERCADO_FIM = 112;
	    public const int BOV02_PRAZO_LIQUIDACAO_INI = 181;
	    public const int BOV02_PRAZO_LIQUIDACAO_FIM = 185;
	    public const int BOV02_DATA_NEGOCIO_INI = 198;
	    public const int BOV02_DATA_NEGOCIO_FIM = 206;
	    public const int BOV02_HORA_NEGOCIO_INI = 206;
	    public const int BOV02_HORA_NEGOCIO_FIM = 212;
	    public const int BOV02_VAR_FECH_DIA_ANT_FMT_INI = 212;
	    public const int BOV02_VAR_FECH_DIA_ANT_FMT_FIM = 213;
	    public const int BOV02_VAR_FECH_DIA_ANT_VALOR_INI = 213;
	    public const int BOV02_VAR_FECH_DIA_ANT_VALOR_FIM = 226;

	    /*
	     * Layout do corpo da mensagem de Mudanca do Estado do Papel (05) da Bovespa
	     */
	    public const int BOV05_ESTADO_NEGOCIACAO_PAPEL_INI = 0;
	    public const int BOV05_ESTADO_NEGOCIACAO_PAPEL_FIM = 1;
	    public const int BOV05_RESERVADO001_INI = 1;
	    public const int BOV05_RESERVADO001_FIM = 17;
	    public const int BOV05_ESTADO_PAPEL_INI = 17;
	    public const int BOV05_ESTADO_PAPEL_FIM = 18;
	    public const int BOV05_TIPO_ACAO_INI = 18;
	    public const int BOV05_TIPO_ACAO_FIM = 19;
	    public const int BOV05_HORARIO_ABERTURA_INI = 19;
	    public const int BOV05_HORARIO_ABERTURA_FIM = 25;
	    public const int BOV05_RESERVADO002_INI = 25;
	    public const int BOV05_RESERVADO002_FIM = 48;
	    public const int BOV05_CODIGO_GRUPO_COTACAO_INI = 48;	// Nao existe no layout da Bovespa, eh um tratamento do MDS 
	    public const int BOV05_CODIGO_GRUPO_COTACAO_FIM = 50;	// Nao existe no layout da Bovespa, eh um tratamento do MDS
	    public const int BOV05_ESTADO_INI = 50;					// Nao existe no layout da Bovespa, eh um tratamento do MDS 
	    public const int BOV05_ESTADO_FIM = 51;					// Nao existe no layout da Bovespa, eh um tratamento do MDS

	    /*
	     * Layout do corpo da mensagem de Horario do Pregao por Grupo de Cotacao (39) da Bovespa
	     */
	    public const int BOV39_CODIGO_GRUPO_COTACAO_INI = 0;
	    public const int BOV39_CODIGO_GRUPO_COTACAO_FIM = 2;
	    public const int BOV39_HORA_PRE_ABERTURA_INI = 2;
	    public const int BOV39_HORA_PRE_ABERTURA_FIM = 8;
	    public const int BOV39_HORA_ABERTURA_INI = 8;
	    public const int BOV39_HORA_ABERTURA_FIM = 14;
	    public const int BOV39_HORA_FECHAMENTO_INI = 14;
	    public const int BOV39_HORA_FECHAMENTO_FIM = 20;
	    public const int BOV39_RESERVADO001_INI = 20;
	    public const int BOV39_RESERVADO001_FIM = 98;

	    /*
	     * Layout do corpo da mensagem de Cadastro Basico de Papel (53) da Bovespa
	     */
	    public const int BOV53_NOME_PAPEL_INI = 0;
	    public const int BOV53_NOME_PAPEL_FIM = 12;
	    public const int BOV53_ESPECIFICACAO_INI = 12;
	    public const int BOV53_ESPECIFICACAO_FIM = 22;
	    public const int BOV53_RESERVADO01_INI = 22;
	    public const int BOV53_RESERVADO01_FIM = 42;
	    public const int BOV53_QUANTIDADE_TITULOS_INI = 42;
	    public const int BOV53_QUANTIDADE_TITULOS_FIM = 57;
	    public const int BOV53_RESERVADO02_INI = 57;
	    public const int BOV53_RESERVADO02_FIM = 196;
	    public const int BOV53_GRUPO_COTACAO_PAPEL_INI = 196;
	    public const int BOV53_GRUPO_COTACAO_PAPEL_FIM = 198;
	    public const int BOV53_RESERVADO03_INI = 198;
	    public const int BOV53_RESERVADO03_FIM = 219;
	    public const int BOV53_LOTE_PADRAO_INI = 219;
	    public const int BOV53_LOTE_PADRAO_FIM = 231;
	    public const int BOV53_RESERVADO04_INI = 231;
	    public const int BOV53_RESERVADO04_FIM = 236;
	    public const int BOV53_FORMA_COTACAO_INI = 236;
	    public const int BOV53_FORMA_COTACAO_FIM = 237;
	    public const int BOV53_DATA_ULTIMO_NEGOCIO_INI = 237;
	    public const int BOV53_DATA_ULTIMO_NEGOCIO_FIM = 245;
	    public const int BOV53_RESERVADO05_INI = 245;
	    public const int BOV53_RESERVADO05_FIM = 334;
	    public const int BOV53_PRECO_ENCERRAMENTO_FORMATO_INI = 334;
	    public const int BOV53_PRECO_ENCERRAMENTO_FORMATO_FIM = 335;
	    public const int BOV53_PRECO_ENCERRAMENTO_PRECO_INI = 335;
	    public const int BOV53_PRECO_ENCERRAMENTO_PRECO_FIM = 348;
	    public const int BOV53_RESERVADO06_INI = 348;
	    public const int BOV53_RESERVADO06_FIM = 362;
	    public const int BOV53_PRECO_EXERCICIO_OPCOES_FORMATO_INI = 362;
	    public const int BOV53_PRECO_EXERCICIO_OPCOES_FORMATO_FIM = 363;
	    public const int BOV53_PRECO_EXERCICIO_OPCOES_PRECO_INI = 363;
	    public const int BOV53_PRECO_EXERCICIO_OPCOES_PRECO_FIM = 376;
	    public const int BOV53_RESERVADO07_INI = 376;
	    public const int BOV53_RESERVADO07_FIM = 449;
	    public const int BOV53_CODIGO_ISIN_PAPEL_INI = 449;
	    public const int BOV53_CODIGO_ISIN_PAPEL_FIM = 461;
	    public const int BOV53_RESERVADO08_INI = 461;
	    public const int BOV53_RESERVADO08_FIM = 545;
	    public const int BOV53_QTD_MEDIA_ULT_PREGOES_INI = 545;
	    public const int BOV53_QTD_MEDIA_ULT_PREGOES_FIM = 556;
	    public const int BOV53_PAR_CONGELA_DIRETO_PORC_PRECO_INI = 556;
	    public const int BOV53_PAR_CONGELA_DIRETO_PORC_PRECO_FIM = 563;
	    public const int BOV53_PAR_CONGELA_DIRETO_PORC_QTD_INI = 563;
	    public const int BOV53_PAR_CONGELA_DIRETO_PORC_QTD_FIM = 570;
	    public const int BOV53_PAR_CONGELA_DIRETO_PORC_CAPITAL_INI = 570;
	    public const int BOV53_PAR_CONGELA_DIRETO_PORC_CAPITAL_FIM = 577;
	    public const int BOV53_PAR_CONGELA_COMUM_PORC_PRECO_INI = 577;
	    public const int BOV53_PAR_CONGELA_COMUM_PORC_PRECO_FIM = 584;
	    public const int BOV53_PAR_CONGELA_COMUM_PORC_QTD_INI = 584;
	    public const int BOV53_PAR_CONGELA_COMUM_PORC_QTD_FIM = 591;
	    public const int BOV53_PAR_CONGELA_COMUM_PORC_CAPITAL_INI = 591;
	    public const int BOV53_PAR_CONGELA_COMUM_PORC_CAPITAL_FIM = 598;
	    public const int BOV53_RESERVADO09_INI = 598;
	    public const int BOV53_RESERVADO09_FIM = 608;
	    public const int BOV53_INDICACAO_OPCAO_INI = 608;
	    public const int BOV53_INDICACAO_OPCAO_FIM = 609;
	    public const int BOV53_DATA_VENCIMENTO_OPCAO_INI = 609;
	    public const int BOV53_DATA_VENCIMENTO_OPCAO_FIM = 617;
	    public const int BOV53_HORA_VENCIMENTO_OPCAO_INI = 617;
	    public const int BOV53_HORA_VENCIMENTO_OPCAO_FIM = 623;
	    public const int BOV53_COD_NEGOCIACAO_PAPEL_INI = 623;
	    public const int BOV53_COD_NEGOCIACAO_PAPEL_FIM = 635;
	    public const int BOV53_RESERVADO10_INI = 635;
	    public const int BOV53_RESERVADO10_FIM = 636;
	    public const int BOV53_SEGMENTO_MERCADO_INI = 636;
	    public const int BOV53_SEGMENTO_MERCADO_FIM = 638;
	    public const int BOV53_RESERVADO11_INI = 638;
	    public const int BOV53_RESERVADO11_FIM = 657;
	    public const int BOV53_IPDRV_INI = 657;
	    public const int BOV53_IPDRV_FIM = 658;
	    public const int BOV53_COEFICIENTE_MULTIPL_QTD_INI = 658;
	    public const int BOV53_COEFICIENTE_MULTIPL_QTD_FIM = 666;
	    public const int BOV53_RESERVADO12_INI = 666;
	    public const int BOV53_RESERVADO12_FIM = 739;

	    /*
	     * Layout do corpo da mensagem de Composicao do Indice (A7) da Bovespa
	     */
	    public const int BOVA7_TAMANHO_OCORRENCIA = 27;
	    public const int BOVA7_QUANTIDADE_OCORRENCIAS = 30;
	
	    public const int BOVA7_CODIGO_PAPEL_30X_INI = 0;
	    public const int BOVA7_CODIGO_PAPEL_30X_FIM = 12;
	    public const int BOVA7_SIMBOLO_PAPEL_30X_INI = 12;
	    public const int BOVA7_SIMBOLO_PAPEL_30X_FIM = 20;
	    public const int BOVA7_CONTRIBUICAO_30X_INI = 20;
	    public const int BOVA7_CONTRIBUICAO_30X_FIM = 27;
	    public const int BOVA7_INDICADOR_ULTIMA_MENSAGEM_INI = 810;
	    public const int BOVA7_INDICADOR_ULTIMA_MENSAGEM_FIM = 811;
	    public const int BOVA7_NUMERO_PAPEIS_INI = 811;
	    public const int BOVA7_NUMERO_PAPEIS_FIM = 814;
	    public const int BOVA7_FECHAMENTO_DIA_ANTERIOR_FORMATO_INI = 814;
	    public const int BOVA7_FECHAMENTO_DIA_ANTERIOR_FORMATO_FIM = 815;
	    public const int BOVA7_FECHAMENTO_DIA_ANTERIOR_INDICE_INI = 815;
	    public const int BOVA7_FECHAMENTO_DIA_ANTERIOR_INDICE_FIM = 822;
	    public const int BOVA7_INDICADOR_FREQUENCIA_CALCULO_INI = 822;
	    public const int BOVA7_INDICADOR_FREQUENCIA_CALCULO_FIM = 823;

	    /*
	     * Layout do corpo da mensagem de Indice (B1) da Bovespa
	     */
	    public const int BOVB1_TIPO_INDICE_INI = 0;
	    public const int BOVB1_TIPO_INDICE_FIM = 1;
	    public const int BOVB1_ULTIMO_INDICE_DIA_INI = 1;
	    public const int BOVB1_ULTIMO_INDICE_DIA_FIM = 8;
	    public const int BOVB1_MAIS_ALTO_INDICE_DIA_INI = 8;
	    public const int BOVB1_MAIS_ALTO_INDICE_DIA_FIM = 15;
	    public const int BOVB1_HORA_MAIS_ALTO_INDICE_DIA_INI = 15;
	    public const int BOVB1_HORA_MAIS_ALTO_INDICE_DIA_FIM = 21;
	    public const int BOVB1_MAIS_BAIXO_INDICE_DIA_INI = 21;
	    public const int BOVB1_MAIS_BAIXO_INDICE_DIA_FIM = 28;
	    public const int BOVB1_HORA_MAIS_BAIXO_INDICE_DIA_INI = 28;
	    public const int BOVB1_HORA_MAIS_BAIXO_INDICE_DIA_FIM = 34;
	    public const int BOVB1_NUMERO_PAPEIS_INDICE_ATIVOS_INI = 34;
	    public const int BOVB1_NUMERO_PAPEIS_INDICE_ATIVOS_FIM = 37;
	    public const int BOVB1_PERCENTAGEM_CAPITALIZACAO_INI = 37;
	    public const int BOVB1_PERCENTAGEM_CAPITALIZACAO_FIM = 42;
	    public const int BOVB1_RESERVADO1_INI = 42;
	    public const int BOVB1_RESERVADO1_FIM = 48;
	    public const int BOVB1_INDICADOR_VARIACAO_INDICE_INI = 48;
	    public const int BOVB1_INDICADOR_VARIACAO_INDICE_FIM = 49;
	    public const int BOVB1_PERCENTAGEM_VARIACAO_INDICE_INI = 49;
	    public const int BOVB1_PERCENTAGEM_VARIACAO_INDICE_FIM = 54;
	    public const int BOVB1_RESERVADO2_INI = 54;
	    public const int BOVB1_RESERVADO2_FIM = 60;
	    public const int BOVB1_INDICADOR_VARIACAO_INDICE_ANO_ANTERIOR_INI = 60;
	    public const int BOVB1_INDICADOR_VARIACAO_INDICE_ANO_ANTERIOR_FIM = 61;
	    public const int BOVB1_PERCENTAGEM_VARIACAO_INDICE_ANO_ANTERIOR_INI = 61;
	    public const int BOVB1_PERCENTAGEM_VARIACAO_INDICE_ANO_ANTERIOR_FIM = 66;
	    public const int BOVB1_RESERVADO3_INI = 66;
	    public const int BOVB1_RESERVADO3_FIM = 82;
	    public const int BOVB1_NUMERO_PAPEIS_EM_BAIXA_CARTEIRA_INI = 82;
	    public const int BOVB1_NUMERO_PAPEIS_EM_BAIXA_CARTEIRA_FIM = 85;
	    public const int BOVB1_NUMERO_PAPEIS_EM_ALTA_CARTEIRA_INI = 85;
	    public const int BOVB1_NUMERO_PAPEIS_EM_ALTA_CARTEIRA_FIM = 88;
	    public const int BOVB1_NUMERO_PAPEIS_SEM_VARIACAO_CARTEIRA_INI = 88;
	    public const int BOVB1_NUMERO_PAPEIS_SEM_VARIACAO_CARTEIRA_FIM = 91;
	    public const int BOVB1_NUMERO_PAPEIS_SEM_COTACAO_CARTEIRA_INI = 91;
	    public const int BOVB1_NUMERO_PAPEIS_SEM_COTACAO_CARTEIRA_FIM = 94;
	    public const int BOVB1_NUMERO_PAPEIS_RESEVADOS_CARTEIRA_INI = 94;
	    public const int BOVB1_NUMERO_PAPEIS_RESEVADOS_CARTEIRA_FIM = 97;
	    public const int BOVB1_NUMERO_PAPEIS_SUSPENSOS_CARTEIRA_INI = 97;
	    public const int BOVB1_NUMERO_PAPEIS_SUSPENSOS_CARTEIRA_FIM = 100;
	    public const int BOVB1_NUMERO_TOTAL_PAPEIS_CARTEIRA_INI = 100;
	    public const int BOVB1_NUMERO_TOTAL_PAPEIS_CARTEIRA_FIM = 103;
	    public const int BOVB1_RESERVADO4_INI = 103;
	    public const int BOVB1_RESERVADO4_FIM = 122;
	    public const int BOVB1_INDICADOR_PRESENCA_DADOS_COMPLEMENTARES_INI = 122;
	    public const int BOVB1_INDICADOR_PRESENCA_DADOS_COMPLEMENTARES_FIM = 123;

    }
}
