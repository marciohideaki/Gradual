using System;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Cadastro
{
    public class LogIntranetInfo : ICodigoEntidade
    {
        public int IdLogIntranet { get; set; }

        public string DsCpfCnpjClienteAfetado { get; set; }

        public int? IdLoginClienteAfetado { get; set; }

        public int? IdClienteAfetado { get; set; }

        public int? CdBovespaClienteAfetado { get; set; }

        public int IdLogin { get; set; }

        public string DsIp { get; set; }

        public string DsTela { get; set; }

        public string DsObservacao { get; set; }

        public TipoAcaoUsuario IdAcao { get; set; }

        public DateTime DtEvento { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
