using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library.Fix
{
    public enum QuickFixApplicationEvent
    {
        fromAdmin,
        fromApp,
        onCreate,
        onLogon,
        onLogout,
        toAdmin,
        toApp
    }
}
