#region Includes
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.PlanoCliente.Lib;
using log4net;
#endregion

namespace Gradual.OMS.PlanoCliente
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoPlanoCliente : IServicoPlanoCliente, IServicoControlavel
    {
        #region Properties
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServicoStatus _ServicoStatus = ServicoStatus.Indefinido; 
        #endregion

        #region Métodos de Busca
        /// <summary>
        /// Lista os produtos cadastrados com filtros de relatórios
        /// </summary>
        /// <param name="pRequest">Objeto de request do tipo ListarProdutosClienteRequest</param>
        /// <returns>Retorna lista de produto cadastrado com filtros de relatório</returns>
        public ListarProdutosClienteResponse ConsultarProdutosClienteFiltro(ListarProdutosClienteRequest pRequest)
        {
            ListarProdutosClienteResponse lRetorno = new ListarProdutosClienteResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno.LstPlanoCliente = new List<PlanoClienteInfo>();

                lRetorno.LstPlanoCliente = lDb.ConsultarPlanoClientesFiltrado(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                logger.Info(string.Concat("Entrou no ConsultarProdutosCliente e listou ", lRetorno.LstPlanoCliente.Count, " produto(s)"));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                logger.ErrorFormat("Erro em ConsultarProdutosCliente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        /// <summary>
        /// Consultar planos vigentes para clientes num determinado plano
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns>Retorna uma lsita com filro efetuado por request</returns>
        public ListarProdutosClienteResponse ConsultarProdutosVigenteCliente(ListarProdutosClienteRequest pRequest)
        {
            ListarProdutosClienteResponse lRetorno = new ListarProdutosClienteResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno.LstPlanoCliente = new List<PlanoClienteInfo>();

                lRetorno.LstPlanoCliente = lDb.ConsultarPlanoClientesVigente(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                logger.Info(string.Concat("Entrou no ConsultarPlanoClientesVigente e listou ", lRetorno.LstPlanoCliente.Count, " produto(s)"));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                logger.ErrorFormat("Erro em ConsultarPlanoClientesVigente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        /// <summary>
        /// Consultar planos para um cliente onde o intervalo de datas passadas não exista no banco.
        /// </summary>
        /// <param name="pRequest">Retorna uma lsita com filro efetuado por request</param>
        /// <returns>Retorna uma lsita com filro efetuado por request</returns>
        public ListarProdutosClienteResponse ConsultarProdutosClientesRengeData(ListarProdutosClienteRequest pRequest)
        {
            ListarProdutosClienteResponse lRetorno = new ListarProdutosClienteResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno.LstPlanoCliente = new List<PlanoClienteInfo>();

                lRetorno.LstPlanoCliente = lDb.ConsultarPlanoClientesRengeDatas(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                logger.Info(string.Concat("Entrou no ConsultarProdutosClientesRengeData e listou ", lRetorno.LstPlanoCliente.Count, " produto(s)"));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                logger.ErrorFormat("Erro em ConsultarProdutosClientesRengeData - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        #endregion

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            _ServicoStatus = ServicoStatus.EmExecucao;
            
            logger.Info("Iniciando o serviço de Planos do cliente");
        }

        public void PararServico()
        {
            _ServicoStatus = ServicoStatus.Parado;

            logger.Info("Parando o serviço de Planos do cliente");
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        #endregion

        #region IServicoPlanoCliente Members

        /// <summary>
        /// Atualiza os plano do cliente com a adesão atual do cliente
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public InserirProdutosClienteResponse AtualizaPlanoClienteExistente(InserirProdutosClienteRequest pRequest)
        {
            InserirProdutosClienteResponse lRetorno = new InserirProdutosClienteResponse();
            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.AtualizaPlanoClienteExistente(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                logger.Info(string.Concat("Entrou no AtualizaPlanoClienteExistente e inseriu ", lRetorno.LstPlanoCliente.Count, " produto(s)"));

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                logger.ErrorFormat("Erro em AtualizaPlanoClienteExistente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        /// <summary>
        /// Lista os produtos cadastrados no cliente selecionado
        /// </summary>
        /// <param name="pRequest">Objeto de request do tipo ListarProdutosClienteRequest</param>
        /// <returns>Retorna lista de produto cadastrado no cliente selecionado</returns>
        public ListarProdutosClienteResponse ListarProdutosCliente(ListarProdutosClienteRequest pRequest)
        {
            ListarProdutosClienteResponse lRetorno = new ListarProdutosClienteResponse();

            try
            {
                PersistenciaDB lPersistenciaDB = new PersistenciaDB();

                lRetorno.LstPlanoCliente = new List<PlanoClienteInfo>();

                lRetorno.LstPlanoCliente = lPersistenciaDB.ConsultarPlanoClientes(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                logger.Info(string.Concat("Entrou no ListarProdutosCliente e listou ", lRetorno.LstPlanoCliente.Count, " produto(s). Cpf informado: ", pRequest.DsCpfCnpj));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                logger.ErrorFormat("Erro em ConsultarProdutosCliente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public AtualizarProdutosClienteResponse AtualizarPlanoCliente(AtualizarProdutosClienteRequest pRequest)
        {
            var lRetorno = new AtualizarProdutosClienteResponse();

            try
            {
                new PersistenciaDB().AtualizarPlanoCliente(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                logger.Info(string.Concat("Entrou no AtualizarPlanoCliente e atualizou  os produtos do cliente ", pRequest.LstPlanoCliente.Count > 0 ? pRequest.LstPlanoCliente[0].DsCpfCnpj : string.Empty));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                logger.ErrorFormat("Erro em AtualizarPlanoCliente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        /// <summary>
        /// Insere os produtos selecionados pelo cliente
        /// </summary>
        /// <param name="pRequest">Objeto de request dos produtos selecionados</param>
        /// <returns>Retorna o objeto do tipo InserirProdutosClienteResponse</returns>
        public InserirProdutosClienteResponse InserirPordutosCliente(InserirProdutosClienteRequest pRequest)
        {
            InserirProdutosClienteResponse lRetorno = new InserirProdutosClienteResponse();
            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.InserirPlanoCliente(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                logger.Info(string.Concat("Entrou no InserirPordutosCliente e inseriu ", lRetorno.LstPlanoCliente.Count, " produto(s)"));

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                logger.ErrorFormat("Erro em InserirPordutosCliente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public ExcluirProdutosClienteResponse ExcluirProdutosClienteSinacor(ExcluirProdutosClienteRequest pRequest)
        {
            ExcluirProdutosClienteResponse lRetorno = new ExcluirProdutosClienteResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lDb.ExcluiAdesaoCorretagemClienteSinacor(pRequest.PlanoCliente); 

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                logger.Info(string.Concat("Entrou no ExcluirProdutosClienteSinacor e excluiu  os produtos do cliente ", pRequest.PlanoCliente.CdCblc));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                logger.ErrorFormat("Erro em ExcluirProdutosClienteSinacor - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
            return lRetorno;
        }

        /// <summary>
        /// Insere um plano/produto (pelo portal) para o cliente que já tem o cblc 
        /// </summary>
        /// <param name="pRequest">Objeto de request InserirProdutosClienteRequest</param>
        /// <returns>Retorna o objeto inserido</returns>
        public InserirProdutosClienteResponse InserirPlanoClienteExistente(InserirProdutosClienteRequest pRequest)
        {
            InserirProdutosClienteResponse lRetorno = new InserirProdutosClienteResponse();
            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.InserirPlanoClienteExistente(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                logger.Info(string.Concat("Entrou no InserirPlanoClienteExistente e inseriu ", lRetorno.LstPlanoCliente.Count, " produto(s)"));

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                logger.ErrorFormat("Erro em InserirPlanoClienteExistente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
            
        }

        /// <summary>
        /// Lista os Produtos cadastrados
        /// </summary>
        /// <returns>Lista os produtos caadstrados no banco de dados</returns>
        public ListarProdutosResponse ListarProdutos()
        {
            ListarProdutosResponse lRetorno = new ListarProdutosResponse();
            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno.LstProdutos = new List<ProdutoInfo>();

                lRetorno.LstProdutos = lDb.ListarProdutos();

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                logger.Info(string.Concat("Entrou no ListarProdutos e listou ", lRetorno.LstProdutos.Count, " produto(s)"));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                logger.ErrorFormat("Erro em ListarProdutos - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }


        #endregion
    }
}
