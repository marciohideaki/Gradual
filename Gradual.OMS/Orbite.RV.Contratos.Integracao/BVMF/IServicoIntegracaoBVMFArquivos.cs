using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Orbite.RV.Contratos.Integracao.BVMF.Dados;

namespace Orbite.RV.Contratos.Integracao.BVMF
{
    /// <summary>
    /// Responsável por interpretar os arquivos da BVMF
    /// </summary>
    [ServiceContract]
    public interface IServicoIntegracaoBVMFArquivos
    {
        ArquivoBVMFInfo InferirLayout(List<LayoutBVMFInfo> layouts, Stream stream, string nomeArquivo);
        ArquivoBVMFInfo InferirLayout(List<LayoutBVMFInfo> layouts, string arquivo);
        DataSet InterpretarArquivo(string arquivo);
        DataSet InterpretarArquivo(Stream stream, string nomeArquivo);
        DataSet InterpretarArquivo(LayoutBVMFInfo layout, string arquivo);
        DataSet InterpretarArquivo(LayoutBVMFInfo layout, Stream stream);
        DataSet InterpretarArquivo(LayoutBVMFInfo layout, Stream stream, string nomeArquivo);
        DataSet CriarSchema(LayoutBVMFInfo layout);
        List<ArquivoBVMFInfo> ListarDiretorio();
        List<ArquivoBVMFInfo> ListarDiretorio(string diretorio);
        List<ArquivoBVMFInfo> ListarDiretorio(string diretorio, string condicoes);
        List<ArquivoBVMFInfo> ListarDiretorio(string[] arquivos);
    }
}
