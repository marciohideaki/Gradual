using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.SpreadMonitor.Lib.Dados;

namespace Gradual.OMS.SpreadMonitor
{
    public class MDSUtils
    {
        public static Dictionary<string, string> montaCabecalhoStreamer(string tipoMensagem, string tipoBolsa, int acao, string instrumento, string sessionID)
        {
            return montaCabecalhoStreamer(tipoMensagem, tipoBolsa, acao, instrumento, 2, sessionID);
        }

        public static Dictionary<string, string> montaCabecalhoStreamer(string tipoMensagem, string tipoBolsa, int acao, string instrumento, int numCasasDecimais, string sessionID)
        {
            Dictionary<String, String> cabecalho = new Dictionary<String, String>();

            if (!String.IsNullOrEmpty(tipoMensagem))
                cabecalho.Add(ConstantesMDS.HTTP_CABECALHO_TIPO_MENSAGEM, tipoMensagem);

            cabecalho.Add(ConstantesMDS.HTTP_CABECALHO_ACAO, acao.ToString());

            cabecalho.Add(ConstantesMDS.HTTP_CABECALHO_CASAS_DECIMAIS, numCasasDecimais.ToString());

            if (!String.IsNullOrEmpty(instrumento))
                cabecalho.Add(ConstantesMDS.HTTP_CABECALHO_INSTRUMENTO, instrumento);

            if (!String.IsNullOrEmpty(sessionID))
                cabecalho.Add(ConstantesMDS.HTTP_CABECALHO_SESSIONID, sessionID);

            if (!String.IsNullOrEmpty(tipoBolsa))
                cabecalho.Add(ConstantesMDS.HTTP_CABECALHO_TIPO_BOLSA, tipoBolsa);

            cabecalho.Add(ConstantesMDS.HTTP_CABECALHO_DATA, DateTime.Now.ToString("yyyyMMdd"));
            cabecalho.Add(ConstantesMDS.HTTP_CABECALHO_HORA, DateTime.Now.ToString("HHmmssfff"));

            return cabecalho;
        }

        public static string montaMensagemHttp(string tipoMensagem, string instrumento, string filtro, string mensagem)
        {
            //EventoHttp eventoHttp = new EventoHttp();
            //eventoHttp.mensagem = mensagem;
            //eventoHttp.instrumento = instrumento;
            //eventoHttp.tipo = tipoMensagem;
            //eventoHttp.filtro = filtro;

            StringBuilder builder = new StringBuilder();

            builder.Append(tipoMensagem.Substring(0, 2));
            //builder.Append(Newtonsoft.Json.JsonConvert.SerializeObject(eventoHttp));
            builder.Append(mensagem);
            return builder.ToString();
        }

        public static Dictionary<string, string> montaMensagemStreamerAlgoritmo(int acao, AlgoStruct algo, int casasDecimais)
        {
            Dictionary<string, string> mensagem = new Dictionary<string, string>();


            mensagem.Add(ConstantesMDS.HTTP_ALGORITMOS_ACAO, acao.ToString());

            mensagem.Add(ConstantesMDS.HTTP_ALGORITMOS_IDREGISTRO, algo.IDRegistro);

            mensagem.Add(ConstantesMDS.HTTP_ALGORITMOS_INSTRUMENTO1, algo.Instrumento1.Trim());

            mensagem.Add(ConstantesMDS.HTTP_ALGORITMOS_INSTRUMENTO2, algo.Instrumento2.Trim());

            mensagem.Add(ConstantesMDS.HTTP_ALGORITMOS_VALOR, String.Format("{0:f" + casasDecimais + "}", algo.Value).Replace('.', ','));

            mensagem.Add(ConstantesMDS.HTTP_ALGORITMOS_TIPO_ALGO, algo.TipoAlgorito.ToString());

            mensagem.Add(ConstantesMDS.HTTP_ALGORITMOS_SENTIDO_ALGO, algo.SentidoAlgoritmo.ToString());

            mensagem.Add(ConstantesMDS.HTTP_ALGORITMOS_HORA, algo.LastCalc.ToString("yyyyMMddHHmmssfff"));


            return mensagem;
        }
    }
}
