using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.Integracao.BVMF.Dados;

namespace Orbite.RV.Contratos.Integracao.BVMF
{
    /// <summary>
    /// Responsável por persistir os layouts de arquivos da BVMF.
    /// </summary>
    public interface IServicoIntegracaoBVMFPersistenciaLayouts
    {
        /// <summary>
        /// Lista todos os layouts existentes no repositório. 
        /// </summary>
        /// <returns>Lista de layouts encontrados</returns>
        List<LayoutBVMFInfo> ListarLayouts();

        /// <summary>
        /// Lista os layouts existentes no repositório. Opção para filtrar por tipo de conversor.
        /// </summary>
        /// <param name="tipoConversor">Filtro por tipo de conversor</param>
        /// <returns>Lista de layouts encontrados</returns>
        List<LayoutBVMFInfo> ListarLayouts(Type conversorTipo);

        /// <summary>
        /// Recebe um layout específico preenchido.
        /// </summary>
        /// <param name="tipoConversor">Tipo do conversor</param>
        /// <param name="nomeLayout">Nome do layout</param>
        /// <returns>Layout preenchido</returns>
        LayoutBVMFInfo ReceberLayout(Type ConversorTipo, string nomeLayout);

        /// <summary>
        /// Remove um layout do repositório.
        /// </summary>
        void RemoverLayout(Type conversorTipo, string nomeLayout);

        /// <summary>
        /// Salva novo layout ou alterações em um layout existente no repositório.
        /// </summary>
        /// <param name="layout">Layout a ser salvo</param>
        void SalvarLayout(LayoutBVMFInfo layout);
    }
}
