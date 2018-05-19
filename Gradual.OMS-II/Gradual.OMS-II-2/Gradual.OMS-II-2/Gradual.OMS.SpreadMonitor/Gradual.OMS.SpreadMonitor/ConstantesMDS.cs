using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.SpreadMonitor
{
    public class ConstantesMDS
    {
        //public const string RequestInstrument = "RI";
        //public const string CancelInstrument = "CI";
        public const string Negocio = "NE";
        public const string LivroOferta = "LC";
        public const string A1 = "A1";
        public const string A2 = "A2";
        public const string A3 = "A3";
        public const string A4 = "A4";
        public const string Destaques = "RP";
        public const string RankCorretora = "RC";
        public const string Sonda = "SD";
        public const string LIVRO_AGREGADO = "OA";

        public const int ACAO_REQUISICAO_ASSINAR = 1;
        public const int ACAO_REQUISICAO_CANCELAR_ASSINATURA = 2;

        // Cabecalho das Mensagens
        public const String HTTP_CABECALHO = "cb";
        public const String HTTP_CABECALHO_TIPO_MENSAGEM = "tp";
        public const String HTTP_CABECALHO_TIPO_BOLSA = "tb";
        public const String HTTP_CABECALHO_DATA = "d";
        public const String HTTP_CABECALHO_HORA = "h";
        public const String HTTP_CABECALHO_INSTRUMENTO = "i";
        public const String HTTP_CABECALHO_ACAO = "ac";
        public const String HTTP_CABECALHO_SESSIONID = "id";
        public const String HTTP_CABECALHO_IDNOTICIA = "ni";
        public const String HTTP_CABECALHO_CASAS_DECIMAIS = "cd";

        // Mensagem de Sonda
        public const String HTTP_SONDA = "sd";
        public const String HTTP_SONDA_DATA = "d";
        public const String HTTP_SONDA_HORA = "h";

        public const String TIPO_REQUISICAO_SONDA = "SD";
        public const String TIPO_REQUISICAO_ALGORITMO = "AG";

        // 
        public const int HTTP_ALGORITMOS_TIPO_ACAO_COMPLETO = 0;
        public const int HTTP_ALGORITMOS_TIPO_ACAO_INCLUIR = 1;
        public const int HTTP_ALGORITMOS_TIPO_ACAO_ALTERAR = 2;
        public const int HTTP_ALGORITMOS_TIPO_ACAO_EXCLUIR = 3;
        public const int HTTP_ALGORITMOS_TIPO_ACAO_EXCLUIR_TODAS = 4;

        public const String HTTP_ALGORITMOS_ACAO = "ac";
        public const String HTTP_ALGORITMOS_IDREGISTRO = "ir";
        public const String HTTP_ALGORITMOS_INSTRUMENTO1 = "i1";
        public const String HTTP_ALGORITMOS_INSTRUMENTO2 = "i2";
        public const String HTTP_ALGORITMOS_TIPO_ALGO = "tpa";
        public const String HTTP_ALGORITMOS_SENTIDO_ALGO = "sta";
        public const String HTTP_ALGORITMOS_QUANTIDADE1 = "qt1";
        public const String HTTP_ALGORITMOS_QUANTIDADE2 = "qt2";
        public const String HTTP_ALGORITMOS_VALOR = "vl";
        public const String HTTP_ALGORITMOS_HORA = "h";

    }
}
