using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.CadastroPapeis.Lib;
using Gradual.OMS.CadastroPapeis.Lib.Info;
using Gradual.OMS.CadastroPapeis.Lib.Mensageria;
using Gradual.OMS.Risco.Persistencia.Lib;

namespace Gradual.OMS.CadastroPapeis
{
    /// <summary>
    /// Classe responsável por realizar a chamada ao servico de persistencia de cadastro de papeis
    /// </summary>
    public class ServicoCadastroPapeis : IServicoCadastroPapeis
    {

        #region IServicoCadastroPapeis Members

        /// <summary>
        /// Metodo responsável por retornar as informações básicas do instrumento oriundas dos arquivos ( PAPD, PAPT,PAPH e DB_FINAL)
        /// </summary>
        /// <param name="pParametros">Objeto contendo o código do instrumento do ativo</param>
        /// <returns>Dados do Instrumento</returns>
        public CadastroPapeisResponse<CadastroPapelInfo> ObterInformacoesIntrumento(CadastroPapeisRequest pParametros)
        {           
            CadastroPapeisResponse<CadastroPapelInfo> Response = new PersistenciaCadastroAtivos().ObterInformacoesPapeis(pParametros);

            if (Response.Objeto != null)
            {
                Response.StatusResposta = Lib.Enum.CriticaMensagemEnum.OK;
                Response.DataResposta = DateTime.Now;
                Response.DescricaoResposta = "Instrumento encontrado com sucesso.";
            }
            else
            {
                Response.StatusResposta = Lib.Enum.CriticaMensagemEnum.Exception;
                Response.DataResposta = DateTime.Now;
                Response.DescricaoResposta = "Instrumento não encontrado";
            }

            return Response;
        }

        /// <summary>
        /// Obtem o security list da Bmf
        /// </summary>
        public SecurityListResponse ObterSecurityList()
        {
            return new PersistenciaCadastroAtivos().ObterSecurityList();

        }

        #endregion
    }
}
