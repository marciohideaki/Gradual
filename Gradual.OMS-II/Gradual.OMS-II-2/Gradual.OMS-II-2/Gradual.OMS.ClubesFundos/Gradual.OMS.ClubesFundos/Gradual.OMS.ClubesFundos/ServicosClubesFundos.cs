#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.ClubesFundos.Lib;
using Gradual.OMS.ClubesFundos.Lib.Dados;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using log4net;
using System.ServiceModel;
#endregion

namespace Gradual.OMS.ClubesFundos
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicosClubesFundos : IServicoClubesFundos, IServicoControlavel
    {
        #region Properties
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServicoStatus _ServicoStatus = ServicoStatus.Indefinido; 
        #endregion

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            _ServicoStatus = ServicoStatus.EmExecucao;
        }

        public void PararServico()
        {
            _ServicoStatus = ServicoStatus.Parado;
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        #endregion

        #region IServicoClubesFundos Members
        /// <summary>
        /// Lista a posição de Clubes de um determinado cliente
        /// </summary>
        /// <param name="Request">Objeto request (Id do cliente)</param>
        /// <returns>Retorna uma lista com a posição de CLUBES de um determinado cliente </returns>
        public ListarClubesResponse ConsultarClientesClubes(ListarClubesRequest Request)
        {
            ListarClubesResponse lRetorno = new ListarClubesResponse();

            lRetorno.Clubes = new List<ClubesInfo>();

            try
            {
                List<ClubesInfo> lClubes = new ClubesDbLib().ConsultarClientesClubes(Request);

                lRetorno.Clubes.AddRange(lClubes);

                lRetorno.DescricaoResposta = "Clube(s) encontrado(s) com sucesso";

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                logger.Info(string.Concat("CLUBES - Entrou no ConsultarClientesClubes e listou ", lRetorno.Clubes.Count, " clube(s)"));
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.Message;
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                logger.Error(ex.StackTrace);
            }

            return lRetorno;
        }

        /// <summary>
        /// Lista a posição de fundos de um determinado cliente
        /// </summary>
        /// <param name="Request">Objeto request</param>
        /// <returns>Retorna uma lista com a posição de FUNDOS de um determinado cliente </returns>
        public ListarFundosResponse ConsultarClientesFundos(ListarFundosRequest Request)
        {
            ListarFundosResponse lRetorno = new ListarFundosResponse();

            lRetorno.Fundos = new List<FundosInfo>();

            try
            {
                logger.DebugFormat("Obtendo posicao de fundos para cliente [{0}] [{1}] [{2}] [{3}]",
                    Request.IdCliente,
                    Request.IdUsuarioLogado,
                    Request.CodigoSessao,
                    Request.DescricaoUsuarioLogado);

                List<FundosInfo> lFundos = new FundosDbLib().ConsultarClientesFundosItau(Request);

                lRetorno.Fundos.AddRange(lFundos);

                lRetorno.DescricaoResposta = "Fundo(s) encontrado(s) com sucesso";

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                logger.Info(string.Concat("FUNDOS - Entrou no ConsultarClientesFundos e listou ", lRetorno.Fundos.Count, " fundos"));
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.Message;
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                logger.Error(ex.StackTrace);
            }

            return lRetorno;
        }

        #endregion
    }
}
