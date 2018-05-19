using System;
using System.Collections.Generic;
using System.Text;

namespace Gradual.Utils.Hardware
{
    public interface IWMI
    {
        IList<string> GetPropertyValues();
    }
}
