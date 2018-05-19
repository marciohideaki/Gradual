using System;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.Sistema
{
    public class SistemaControleLogIntranetRelInfo : ICodigoEntidade
    {
        #region | Consultas

        public string ConsultaEmailUsuario { get; set; }

        public int? ConsultaTipoAcao { get; set; }

        public string ConsultaTela { get; set; }

        public DateTime ConsultaDataDe { get; set; }

        public DateTime ConsultaDataAte { get; set; }

        public OpcoesBuscarPor ConsultaOpcoesBusca { get; set; }

        public string ConsultaClienteParametro { get; set; }

        #endregion

        #region | Resultados

        public string DsEmailUsuario { get; set; }

        public string DsNomeUsuario { get; set; }

        public string DsIp { get; set; }

        public string NmTela { get; set; }

        public DateTime DtEvento { get; set; }

        public string NmCliente { get; set; }

        public string DsCpfCnpj { get; set; }

        public string DsObservacao { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        public int IdEvento { get; set; }

        #endregion
    }
}
