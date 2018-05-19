using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Salvar Representante para não Residente. PJ.
    /// </summary>
    [Permissao(
    "F60D51DD-CC20-45A8-9244-6F96ACA50BF2"
    , "Cliente - Salvar Representante para não Residente. PJ"
    , "Permite Salvar Representante para não Residente. PJ")]
    public class PermissaoClienteRepresentanteNaoResidenteSalvar : PermissaoBase
    {
    }
}
