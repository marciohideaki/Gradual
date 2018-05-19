using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Gradual.Utils
{
    public class ResourceManager : IDisposable
    {
        private Stream _resource;
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}:{1}", Gradual.Utils.MethodHelper.GetCurrentMethod(50, ' '), "Saindo do processo"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                    // free resources here
                    if (_resource != null)
                        _resource.Dispose();
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}:{1}", Gradual.Utils.MethodHelper.GetCurrentMethod(50, ' '), "Objeto eliminado"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                }

                // Indicate that the instance has been disposed.
                _resource = null;
                _disposed = true;
            }
        }
    }
}
