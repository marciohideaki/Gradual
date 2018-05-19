using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Gradual.OMS.Contratos.Interface.Desktop.Dados;

namespace Gradual.OMS.Sistemas.Interface.Desktop
{
    public class ServicoInterfaceDesktopConfig
    {
        public string SkinAtual { get; set; }

        public List<SkinInfo> Skins { get; set; }

        public string ArquivoConfiguracoes { get; set; }

        public string ArquivoConfiguracoesDefault { get; set; }

        public ServicoInterfaceDesktopConfig()
        {
            this.Skins = new List<SkinInfo>();
        }

        public SkinInfo ReceberSkinAtual()
        {
            SkinInfo skinInfo = (from s in this.Skins
                                 where s.Nome == this.SkinAtual
                                 select s).FirstOrDefault();
            return skinInfo;
        }
    }
}
