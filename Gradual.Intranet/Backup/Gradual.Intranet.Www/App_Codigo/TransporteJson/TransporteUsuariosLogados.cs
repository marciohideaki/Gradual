using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteUsuariosLogados
    {

        public TransporteUsuariosLogados() { }

        public TransporteUsuariosLogados(Sessao sessao)
        {
            this.CodigoSessao = sessao.SessaoInfo.CodigoSessao;
            this.DataLogIn = sessao.SessaoInfo.DataCriacao.ToString("dd/MM/yyyy HH:mm:ss");
            this.DataLogOut = "";
            this.DsUsuario = sessao.Usuario.UsuarioInfo.Nome;
            this.IdLogin = sessao.Usuario.UsuarioInfo.CodigoUsuario;
            this.Sistema = sessao.SessaoInfo.CodigoSistemaCliente;
            this.IpUsuario = sessao.IP;
            this.ID = sessao.Id.ToString() ;
            this.SessaoAtiva = (sessao.SessaoAtiva) ? "SIM" : "NÃO";
        }

        public string ID { get; set; }
        
        public string IdLogin { get; set; }

        public string DsUsuario { get; set; }

        public string DataLogIn { get; set; }

        public string DataLogOut { get; set; }

        public string Sistema { get; set; }

        public string CodigoSessao { get; set; }

        public string IpUsuario { get; set; }

        public string SessaoAtiva { get; set; }


        public List<TransporteUsuariosLogados> ToListTransporteUsuariosLogados(ListarSessoesResponse lresponse)
        {
            List<TransporteUsuariosLogados> arr = new List<TransporteUsuariosLogados>();
            foreach (Sessao sessao in lresponse.Sessoes)
            {
                arr.Add(new TransporteUsuariosLogados(sessao));
            }
            return arr;
        }

        //public Sessao ToSessao()
        //{
        //    Sessao sessao = new Sessao()
        //}

    }
}