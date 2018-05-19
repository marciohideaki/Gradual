using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.Integracao.BVMF.Dados;

namespace Orbite.RV.Contratos.Integracao.BVMF
{
    /// <summary>
    /// Base para a classe concreta que vai efetivamente realizar a conversao do arquivo.
    /// </summary>
    public interface IConversorLayoutBase
    {
        DataSet InterpretarArquivo(LayoutBVMFInfo layout, Stream stream);
        ArquivoBVMFInfo InferirLayout(List<LayoutBVMFInfo> layouts, Stream stream);
    }
}
