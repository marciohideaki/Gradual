using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.OMS.Seguranca.Lib;
using System.Collections.Generic;

namespace Gradual.Intranet.Www.Intranet.Seguranca
{
    [ValidarSegurancaAttribute("9C5DA26B-8C30-4c1d-AA7A-B7A22CF2CA8F", "1", "1")]
    public partial class Default : PaginaBaseAutenticada
    {
        
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            //System.Reflection.MemberInfo info = typeof(Default);
            //object[] attrs = info.GetCustomAttributes(typeof(ValidarSegurancaAttribute), false);

            //List<ItemSegurancaInfo> itensSeg = new List<ItemSegurancaInfo>();

            //for (int i = 0; i < attrs.Length; i++)
            //{
            //    ItemSegurancaInfo isi = ((ValidarSegurancaAttribute)attrs[i]).Seguranca;
            //    itensSeg.Add(isi);
            //}

            //ValidarItemSegurancaResponse lres = ServicoSeguranca.ValidarItemSeguranca(new ValidarItemSegurancaRequest()
            //{
            //    ItensSeguranca = itensSeg,
            //    CodigoSessao = this.CodigoSessao
            //});

        }
    }
}
