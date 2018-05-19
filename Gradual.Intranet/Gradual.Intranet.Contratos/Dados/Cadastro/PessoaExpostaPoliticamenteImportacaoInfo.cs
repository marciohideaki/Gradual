using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class PessoaExpostaPoliticamenteImportacaoInfo : ICodigoEntidade
    {
        #region Propriedades

        public int RegistrosImportadosComSucesso { get; set; }

        public int RegistrosImportadosComErro { get; set; }

        public List<string> MensagensDeErro { get; set; }

        public List<PessoaExpostaPoliticamenteInfo> ListaParaImportar { get; set; }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
