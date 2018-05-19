using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.OMS.WsIntegracao.Arena.Services
{
    public enum eDateNull
    {
        Permite,
        DataMinValue
    }

    public enum eTipoAcesso
    {
        Cliente = 0,
        Cadastro = 1,
        Assessor = 2,
        Atendimento = 3,
        TeleMarketing = 4
    }
}