using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteSuitability
    {
        #region Propriedades

        public int IdCliente { get; set; }

        public int IdClienteSuitability { get; set; }

        public string Resultado { get; set; }

        public string DataDaRealizacao { get; set; }

        public string Sistema { get; set; }

        public string Usuario { get; set; }

        public string Respostas { get; set; }

        public string LinkParaArquivoCiencia { get; set; }

        public string DataArquivoCiencia { get; set; }

        #endregion

        #region Construtor

        public TransporteSuitability() { }

        public TransporteSuitability(ClienteSuitabilityInfo pInfo) 
        {
            this.IdCliente = pInfo.IdCliente.Value;

            if(pInfo.IdClienteSuitability.HasValue)
                this.IdClienteSuitability = pInfo.IdClienteSuitability.Value;

            this.Resultado = pInfo.ds_perfil;
            this.DataDaRealizacao = pInfo.dt_realizacao.ToString("dd/MM/yyyy");
            this.Respostas = pInfo.ds_respostas;
            this.Sistema = pInfo.ds_fonte;
            this.Usuario = pInfo.ds_loginrealizado;

            this.LinkParaArquivoCiencia = pInfo.ds_arquivo_ciencia;
            this.DataArquivoCiencia = (pInfo.dt_arquivo_upload.HasValue) ? pInfo.dt_arquivo_upload.Value.ToString("dd/MM/yyyy HH:mm") : "";
        }

        #endregion
    }
}