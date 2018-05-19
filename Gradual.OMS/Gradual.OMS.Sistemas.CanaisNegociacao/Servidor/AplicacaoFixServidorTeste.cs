using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library.Fix;

namespace Gradual.OMS.Sistemas.CanaisNegociacao.Servidor
{
    public class AplicacaoFixServidorTeste : AplicacaoFixBase
    {
        protected override void OnFromApp(QuickFix.Message message, QuickFix.SessionID session)
        {
            base.OnFromApp(message, session);
        }

        protected override void OnLogon(QuickFix.SessionID session)
        {
            base.OnLogon(session);
        }

        public override void onMessage(QuickFix40.NewOrderSingle message, QuickFix.SessionID session)
        {
            base.onMessage(message, session);
        }
    }
}
