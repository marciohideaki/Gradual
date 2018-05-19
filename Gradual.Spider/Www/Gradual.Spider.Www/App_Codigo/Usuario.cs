using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Spider.Www.App_Codigo
{
    public class Usuario
    {       
        #region Propriedades

        public int Id { get; set; }

        public string Nome { get; set; }

        public string EmailLogin { get; set; }

        public bool EhAdministrador { get; set; }

        #endregion
    }
}