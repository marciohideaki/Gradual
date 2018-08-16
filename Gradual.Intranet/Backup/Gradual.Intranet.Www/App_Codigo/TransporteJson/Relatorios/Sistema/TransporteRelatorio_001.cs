using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Relatorios.Sistema;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Sistema
{
    public class TransporteRelatorio_001 : ICodigoEntidade
    {
        public string UsuarioEmail { get; set; }
        public string UsuarioNome { get; set; }
        public string IP { get; set; }
        public string Evento { get; set; }
        public string NomeTela { get; set; }
        public string Data { get; set; }
        public string ClienteNome { get; set; }
        public string CpfCnpj { get; set; }
        public string Observacao { get; set; }

        public List<TransporteRelatorio_001> TraduzirLista(List<SistemaControleLogIntranetRelInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_001>();

            if (null != pParametro)
                pParametro.ForEach(lLogInfo =>
                {
                    lRetorno.Add(new TransporteRelatorio_001()
                    {
                        IP = lLogInfo.DsIp,
                        ClienteNome = lLogInfo.NmCliente.ToStringFormatoNome(),
                        CpfCnpj = lLogInfo.DsCpfCnpj.ToCpfCnpjString(),
                        Data = lLogInfo.DtEvento.ToString("dd/MM/yyyy HH:mm:ss"),
                        Evento = ((TipoAcaoUsuario)lLogInfo.IdEvento).ToString(),
                        NomeTela = this.PadronizarNomeTela(lLogInfo.NmTela),
                        Observacao = lLogInfo.DsObservacao,
                        UsuarioEmail = lLogInfo.DsEmailUsuario.ToLower(),
                        UsuarioNome = lLogInfo.DsNomeUsuario.ToStringFormatoNome(),
                    });
                });

            return lRetorno;
        }

        private string PadronizarNomeTela(string pParametro)
        {
            var lRetorno = string.Empty;

            var lArrayPaginas = pParametro.Split('/');

            lRetorno = lArrayPaginas[lArrayPaginas.Length - 1];

            return lRetorno.Replace(".aspx", string.Empty);
        }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}