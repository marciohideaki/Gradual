using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Comum
{
    /// <summary>
    /// Classe de auxilio para fornecer as constantes dos sistemas do OMS.
    /// Inicialmente criado para auxiliar as rotinas de log
    /// </summary>
    [Serializable]
    public static class ModulosOMS
    {
        [LogOrigem]
        public const string ModuloCanais = "OMS.Canais";

        [LogOrigem]
        public const string ModuloCanaisBMF = "OMS.Canais.BMF";

        [LogOrigem]
        public const string ModuloCanaisBovespa = "OMS.Canais.Bovespa";

        [LogOrigem]
        public const string ModuloComum = "OMS.Comum";

        [LogOrigem]
        public const string ModuloIntegracao = "OMS.Integracao";

        [LogOrigem]
        public const string ModuloInterface = "OMS.Interface";

        [LogOrigem]
        public const string ModuloInterfaceDesktop = "OMS.Interface.Desktop";

        [LogOrigem]
        public const string ModuloInterfaceDesktopPlataforma = "OMS.Interface.Desktop.Plataforma";

        [LogOrigem]
        public const string ModuloInterfaceWebHomeBroker = "OMS.Interface.Web.HomeBroker";

        [LogOrigem]
        public const string ModuloLibrary = "OMS.Library";

        [LogOrigem]
        public const string ModuloLibraryServicos = "OMS.Library.Servicos";

        [LogOrigem]
        public const string ModuloOrdens = "OMS.Ordens";

        [LogOrigem]
        public const string ModuloRisco = "OMS.Risco";

        [LogOrigem]
        public const string ModuloSeguranca = "OMS.Seguranca";

        [LogOrigem]
        public const string ModuloValidacao = "OMS.Validacao";
    }
}
