﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STIntegracaoPortalHB
{
    public partial class ProcessaLoginAcessoDireto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sessao = Session["atributoAutenticacaoAcessoDireto"].ToString();
        }
    }
}