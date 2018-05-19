using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using Gradual.OMS.CarteiraRecomendada.lib;
using Gradual.OMS.CarteiraRecomendada.lib.Mensageria;
using System.Net;
using System.IO;
using log4net;
using Gradual.OMS.Ordens.Lib.Mensageria;
using Gradual.OMS.Ordens.Lib.Info;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.Library.Servicos;

namespace ConsoleApplicationTesteCarteiraRecomendada
{
    class Program
    {
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmTeste());
        }
    }
}
