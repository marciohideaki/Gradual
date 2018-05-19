using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.AutomacaoDesktop
{
    public static class ConstantesMDS
    {
        public const string TIPO_REQUISICAO_NEGOCIO = "NE";
	    public const string TIPO_REQUISICAO_LIVRO = "LC";
	    public const string TIPO_REQUISICAO_INDICE = "IN";
	
	    public const string TIPOMSG_FIM_CONEXAO = "XX";
	    public const string TIPOMSG_INTERRUPCAO = "II";
	    public const string TIPOMSG_REQ_ASSINATURA = "RI";
	    public const string TIPOMSG_REQ_CANCELA_ASSINATURA = "CI";
	    public const string TIPOMSG_REQ_ASSINATURA_LIVRO = "RL";
	    public const string TIPOMSG_REQ_CANCELA_ASSINATURA_LIVRO = "CL";
	    public const string TIPOMSG_REQ_SIGNIN = "A3";
	    public const string TIPOMSG_RESP_SIGNIN = "A4";
	    public const string TIPOMSG_REQ_SIGNOUT = "A5";
	    public const string TIPOMSG_REQ_RANKING_PAPEIS = "RP";
	    public const string TIPOMSG_REQ_RANKING_CORRETORAS = "RC";
	    public const string TIPOMSG_REQ_ORDEM_STOP = "SS";
	    public const string TIPOMSG_RESP_ORDEM_STOP = "RS";
	    public const string TIPOMSG_REQ_CANCELA_ORDEM_STOP = "CE";
	    public const string TIPOMSG_RESP_CANCELA_ORDEM_STOP = "CR";
	    public const string TIPOMSG_REQ_CANCELA_ORDEM_STOP_INTERNO = "CW";
	
	    public const string TIPO_REQUISICAO_BOVESPA_NEGOCIO_DE_ABERTURA = "01";
	    public const string TIPO_REQUISICAO_BOVESPA_NEGOCIO = "02";
	    public const string TIPO_REQUISICAO_BOVESPA_MUDANCA_ESTADO_PAPEL = "05";
	    public const string TIPO_REQUISICAO_BOVESPA_HORARIO_PREGAO_GRUPO_COTACAO = "39";
	    public const string TIPO_REQUISICAO_BOVESPA_CADASTRO_BASICO = "53";
	    public const string TIPO_REQUISICAO_BOVESPA_COMPOSICAO_INDICE = "A7";
	    public const string TIPO_REQUISICAO_BOVESPA_INDICES = "B1";
	    public const string TIPO_REQUISICAO_BOVESPA_RETRANSMISSAO_LIVRO_OFERTAS = "S0";
	    public const string TIPO_REQUISICAO_BOVESPA_ATUALIZACAO_LIVRO_OFERTAS = "S3";
	    public const string TIPO_REQUISICAO_BOVESPA_CANCELAMENTO_LIVRO_OFERTAS = "S4";

	    public const string TIPO_REQUISICAO_BMF_INSTRUMENTO = ".";
	    public const string TIPO_REQUISICAO_BMF_HEARTBEAT = "-";
	    public const string TIPO_REQUISICAO_BMF_LIVRO_OFERTAS_COMPRA = "0";
	    public const string TIPO_REQUISICAO_BMF_LIVRO_OFERTAS_VENDA = "1";
	    public const string TIPO_REQUISICAO_BMF_NEGOCIO = "2";
	    public const string TIPO_REQUISICAO_BMF_PRECO_ABERTURA = "4";
	    public const string TIPO_REQUISICAO_BMF_PRECO_FECHAMENTO = "5";
	    public const string TIPO_REQUISICAO_BMF_PRECO_MAXIMO_DIA = "7";
	    public const string TIPO_REQUISICAO_BMF_PRECO_MINIMO_DIA = "8";
	    public const string TIPO_REQUISICAO_BMF_VOLUME_NEGOCIOS = "B";
	    public const string TIPO_REQUISICAO_BMF_PRECOS_REFERENCIAIS = "a";
	    public const string TIPO_REQUISICAO_BMF_FASE_NEGOCIACAO = "b";
	    public const string TIPO_REQUISICAO_BMF_ESTADO = "c";

	    public const string TIPO_ESTADO_BMF_NAO_INICIADO = "100";
	    public const string TIPO_ESTADO_BMF_NEGOCIACAO = "101";
	    public const string TIPO_ESTADO_BMF_LEILAO = "102";
	    public const string TIPO_ESTADO_BMF_PROIBIDO = "103";
	    public const string TIPO_ESTADO_BMF_LEILAO_ENCERRADO = "104";
	    public const string TIPO_ESTADO_BMF_SUSPENSO = "105";
	    public const string TIPO_ESTADO_BMF_PRE_LEILAO = "106";
	    public const string TIPO_ESTADO_BMF_POS_LEILAO = "107";
	    public const string TIPO_ESTADO_BMF_REVOGADO = "108";
	    public const string TIPO_ESTADO_BMF_CANCELADO = "109";
	    public const string TIPO_ESTADO_BMF_FECHAMENTO_ELETRONICO_OFERTAS = "110";
	    public const string TIPO_ESTADO_BMF_SEM_OFERTANTE = "111";
	    public const string TIPO_ESTADO_BMF_OFERTA_MINIMA_NAO_ALCANCADA = "112";
	    public const string TIPO_ESTADO_BMF_ADJUDICADO = "113";
	    public const string TIPO_ESTADO_BMF_ACEITO = "114";
	    public const string TIPO_ESTADO_BMF_IMPUGNADO = "115";
	    public const string TIPO_ESTADO_BMF_PROIBIDO_RESERVADO = "116";
	    public const string TIPO_ESTADO_BMF_SUJEITO_A_INTERFERENCIA = "117";
	    public const string TIPO_ESTADO_BMF_CONGELADO = "118";
	    public const string TIPO_ESTADO_BMF_LEILAO_ESTENDIDO = "119";
	    public const string TIPO_ESTADO_BMF_SEM_VALIDADE = "120";
	    public const string TIPO_ESTADO_BMF_COM_FINAL_ALEATORIO = "121";
	    public const string TIPO_ESTADO_BMF_INTERROMPIDO = "122";
	    public const string TIPO_ESTADO_BMF_SUSPENSO_E_PROIBIDO = "123";
	    public const string TIPO_ESTADO_BMF_NEGOCIACAO_PROIBIDA = "124";
	    public const string TIPO_ESTADO_BMF_CONGELADO_E_PROIBIDO = "125";

	    public const string TIPO_FASE_NEGOCIACAO_BMF_CONSULTA_INICIO_DIA = "C";
	    public const string TIPO_FASE_NEGOCIACAO_BMF_INTERVENCAO_ANTES_ABERTURA = "E";
	    public const string TIPO_FASE_NEGOCIACAO_BMF_NEGOCIACAO_CONTINUA = "S";
	    public const string TIPO_FASE_NEGOCIACAO_BMF_FIM_CONSULTA = "F";
	    public const string TIPO_FASE_NEGOCIACAO_BMF_INTERVENCAO_AREA_ACOMPANHAMENTO_MERCADO = "N";
	    public const string TIPO_FASE_NEGOCIACAO_BMF_FECHAMENTO_MERCADO = "U";
	    public const string TIPO_FASE_NEGOCIACAO_BMF_MINI_BATCH = "M";
	    public const string TIPO_FASE_NEGOCIACAO_BMF_FASE_INDEFINIDA = "I";
	
	
	    public const string TIPO_ATIVO_BMF_FUTURO = "FUT";
	    public const string TIPO_ATIVO_BMF_OPCAO = "OPT";
	    public const string TIPO_ATIVO_BMF_AVISTA_OU_DISPONIVEL = "SPOT";
	    public const string TIPO_ATIVO_BMF_OPCAO_SOBRE_DISPONIVEL = "SOPT";
	    public const string TIPO_ATIVO_BMF_OPCAO_SOBRE_FUTURO = "FOPT";
	    public const string TIPO_ATIVO_BMF_TERMO = "DTERM";
	
	    public const string TIPO_PRECO_REFERENCIAL_BMF_PRECO_AJUSTE = "0";
	    public const string TIPO_PRECO_REFERENCIAL_BMF_PRECO_AJUSTE_DIA_ANTERIOR = "5";
	
	    public const string TIPO_ACAO_BMF_INCLUIR = "0";
	    public const string TIPO_ACAO_BMF_ALTERAR = "1";
	    public const string TIPO_ACAO_BMF_EXCLUIR = "2";
	
	    public const string SEGMENTO_MERCADO_BOVESPA_VISTA = "01";
	    public const string SEGMENTO_MERCADO_BOVESPA_TERMO = "02";
	    public const string SEGMENTO_MERCADO_BOVESPA_FRACIONARIO = "03";
	    public const string SEGMENTO_MERCADO_BOVESPA_OPCOES = "04";
	    public const string SEGMENTO_MERCADO_BOVESPA_LEILAO = "05";
	    public const string SEGMENTO_MERCADO_BOVESPA_LEILAO_DE_NAO_COTADAS = "06";
	    public const string SEGMENTO_MERCADO_BOVESPA_RESERVADO = "07";
	    public const string SEGMENTO_MERCADO_BOVESPA_TERMO_EM_PONTOS = "08";
	    public const string SEGMENTO_MERCADO_BOVESPA_EXERCICIO_DE_OPCOES = "09";
	    public const string SEGMENTO_MERCADO_BOVESPA_INDICES = "90";
	
	    public const string TIPO_REGISTRO_BOVESPA_CANCELAMENTO_NEGOCIO = "00";
	    public const string TIPO_REGISTRO_BOVESPA_CRIACAO_NEGOCIO = "07";
	
	    public const string INDICADOR_OPCAO_BOVESPA_OPCAO_VENDA = "P";
	    public const string INDICADOR_OPCAO_BOVESPA_OPCAO_COMPRA = "C";
	
	    public const string TIPO_INDICE_PRIMEIRO_DIVULGADO = "1";
	    public const string TIPO_INDICE_NORMAL = "2";
	    public const string TIPO_INDICE_REFERENCIA = "5";
	    public const string TIPO_INDICE_MEDIO = "6";
	    public const string TIPO_INDICE_LIQUIDACAO = "7";
	    public const string TIPO_INDICE_DESCONSIDERAR = "8";
	
	    public const string DESCRICAO_DE_BOLSA_BOVESPA = "BV";
	    public const string DESCRICAO_DE_BOLSA_BMF = "BF";

	    public const string PLATAFORMA_TODAS = "TODAS";
	    public const string PLATAFORMA_HOMEBROKER = "HB";
	    public const string PLATAFORMA_DESKTOP = "DESK";
	
	    public const string INDICE_IBOVESPA = "IBOV";
	
	    public const string BOVESPA_MSGID_INICIAL = "                   ";
        public const int BMF_SEQNUM_INICIAL = 1;
	
	    public const string DESCRICAO_RANKING_PAPEIS = "DESTAQUE";
	
	    public const string TABELA_COMPOSICAO_INDICE = "ComposicaoIndice";
	    public const string TABELA_GRUPO_COTACAO = "GrupoCotacao";
	    public const string TABELA_INSTRUMENTOS = "Instrumentos";
	    public const string TABELA_RANKING_CORRETORAS_VENDEDORAS = "CorretorasVendedoras";
	    public const string TABELA_RANKING_CORRETORAS_COMPRADORAS = "CorretorasCompradoras";
        public const string TABELA_LIVRO_OFERTAS = "LivroOfertas";
        public const string TABELA_ULTIMO_MSGID_LIVRO_OFERTAS = "UltimoMsgIdLivroOfertas";

	    public const string ESTADO_PAPEL_BOVESPA_SUSPENSO = "S";
	    public const string ESTADO_PAPEL_BOVESPA_CONGELADO = "G";
	    public const string ESTADO_PAPEL_BOVESPA_EM_LEILAO = "R";
	    public const string ESTADO_PAPEL_BOVESPA_NORMAL = " ";
	    public const string TIPO_ACAO_BOVESPA_EM_NEGOCIACAO = "O";
	    public const string TIPO_ACAO_BOVESPA_INIBIDO = "I";
	
	    public const string COMPOSICAO_INDICE_BOVESPA_ULTIMA_MENSAGEM = "1";
	
	    public const int ESTADO_PAPEL_NAO_NEGOCIADO = 0;
	    public const int ESTADO_PAPEL_EM_LEILAO = 1;
	    public const int ESTADO_PAPEL_EM_NEGOCIACAO = 2;
	    public const int ESTADO_PAPEL_SUSPENSO = 3;
	    public const int ESTADO_PAPEL_CONGELADO = 4;
	    public const int ESTADO_PAPEL_INIBIDO = 5;

	    /*
	     * Dados fixos obtidos da carga inicial do Banco de Dados OMS
	     */
	    public const int TIPO_DE_BOLSA_BOVESPA = 1;
	    public const int TIPO_DE_BOLSA_BMF = 2;
	
	    public const int TIPO_DE_ATIVO_ACAO = 1;
	    public const int TIPO_DE_ATIVO_OPCAO = 2;
	    public const int TIPO_DE_ATIVO_TERMO = 3;
	    public const int TIPO_DE_ATIVO_LEILAO = 4;
	    public const int TIPO_DE_ATIVO_FUTURO = 5;

        public const string INTERVALO_GRAVACAO_LIVRO_BOVESPA_PADRAO = "0";

    }
}
