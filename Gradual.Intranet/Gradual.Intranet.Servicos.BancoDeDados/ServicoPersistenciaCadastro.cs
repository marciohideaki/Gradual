using System;
using Gradual.Intranet.Contratos;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Persistencia;
using log4net;

namespace Gradual.Intranet.Servicos.BancoDeDados
{
    public class ServicoPersistenciaCadastro : IServicoPersistenciaCadastro
    {

        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region IServicoPersistenciaCadastro Members

        public SalvarEntidadeCadastroResponse SalvarEntidadeCadastro<T>(SalvarEntidadeCadastroRequest<T> parametros) where T : ICodigoEntidade
        {
            // Referencia para o servico de persistencia
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();

            // Começa a criação da mensagem de resposta
            SalvarEntidadeCadastroResponse resposta = 
                new SalvarEntidadeCadastroResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem 
                };

            // Bloco de controle
            try
            {
                // Faz a chamada do serviço de persistencia
                SalvarObjetoResponse<T> respostaSalvar;

                SalvarObjetoRequest<T> lSalvarObjetoRequest;

                SalvarEntidadeResponse<T> lResponseSalvar;


                lSalvarObjetoRequest = new SalvarObjetoRequest<T>();

                lSalvarObjetoRequest.Objeto = parametros.EntidadeCadastro;

                lSalvarObjetoRequest.IdUsuarioLogado = parametros.IdUsuarioLogado;

                lSalvarObjetoRequest.DescricaoUsuarioLogado = parametros.DescricaoUsuarioLogado;


                respostaSalvar = servicoPersistencia.SalvarObjeto<T>(lSalvarObjetoRequest);

                lResponseSalvar = respostaSalvar as SalvarEntidadeResponse<T>;

                resposta.DescricaoResposta = lResponseSalvar.Codigo.ToString();
                resposta.Objeto = lResponseSalvar.Objeto;

                if (respostaSalvar is SalvarEntidadeResponse<ClienteEnderecoInfo>)
                {
                    SalvarEntidadeResponse<ClienteEnderecoInfo> lSalvarEndereco = respostaSalvar as SalvarEntidadeResponse<ClienteEnderecoInfo>;
                    resposta.DescricaoResposta = lSalvarEndereco.Codigo.ToString();
                }
                if (respostaSalvar is SalvarEntidadeResponse<ClienteSituacaoFinanceiraPatrimonialInfo>)
                {
                    SalvarEntidadeResponse<ClienteSituacaoFinanceiraPatrimonialInfo> lSalvarPatrimonial = respostaSalvar as SalvarEntidadeResponse<ClienteSituacaoFinanceiraPatrimonialInfo>;
                    resposta.DescricaoResposta = lSalvarPatrimonial.Codigo.ToString();
                }
                if (respostaSalvar is SalvarEntidadeResponse<ClienteProcuradorRepresentanteInfo>)
                {
                    SalvarEntidadeResponse<ClienteProcuradorRepresentanteInfo> lSalvarProcurador = respostaSalvar as SalvarEntidadeResponse<ClienteProcuradorRepresentanteInfo>;
                    resposta.DescricaoResposta = lSalvarProcurador.Codigo.ToString();
                }
            }
            catch (ArgumentException ex)
            {
                // Tratamento de erro
                logger.Error(parametros, ex);
                resposta.DescricaoResposta = ex.Message;
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            catch (Exception ex)
            {
                // Tratamento de erro
                logger.Error(parametros, ex);
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            // Retorna a resposta
            return resposta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ConsultarEntidadeCadastroResponse<T> ConsultarEntidadeCadastro<T>(ConsultarEntidadeCadastroRequest<T> parametros) where T : ICodigoEntidade
        {
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();

            ConsultarEntidadeCadastroResponse<T> resposta =
                new ConsultarEntidadeCadastroResponse<T>()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            try
            {
                ConsultarObjetosResponse<T> respostaConsultar =
                    servicoPersistencia.ConsultarObjetos<T>(
                        new ConsultarEntidadeRequest<T>()
                        {
                            Condicoes = parametros.Condicoes,
                            Objeto = parametros.EntidadeCadastro,
                            IdUsuarioLogado = parametros.IdUsuarioLogado,
                            DescricaoUsuarioLogado = parametros.DescricaoUsuarioLogado
                        });

                resposta.Resultado = respostaConsultar.Resultado;
            }
            catch (Exception ex)
            {
                logger.Error(parametros, ex);
                             resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return resposta;
        }

        public RemoverEntidadeCadastroResponse RemoverEntidadeCadastro<T>(RemoverEntidadeCadastroRequest<T> parametros) where T : ICodigoEntidade
        {
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();

            Type tipoObjeto = typeof(T);

            RemoverEntidadeCadastroResponse resposta =
                new RemoverEntidadeCadastroResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            try
            {
                RemoverObjetoResponse<T> respostaRemover =
                    servicoPersistencia.RemoverObjeto<T>(
                        new RemoverEntidadeRequest<T>()
                        {
                            Objeto = parametros.EntidadeCadastro, 
                            IdUsuarioLogado = parametros.IdUsuarioLogado , 
                            DescricaoUsuarioLogado = parametros.DescricaoUsuarioLogado
                        });

            }
            catch (Exception ex)
            {
                logger.Error(parametros, ex);
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return resposta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberEntidadeCadastroResponse<T> ReceberEntidadeCadastro<T>(ReceberEntidadeCadastroRequest<T> parametros) where T : ICodigoEntidade
        {
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();
            ReceberEntidadeCadastroResponse<T> resposta =
                new ReceberEntidadeCadastroResponse<T>()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            try
            {
                logger.Error("respostaReceber");
                ReceberObjetoResponse<T> respostaReceber =
                    servicoPersistencia.ReceberObjeto<T>(
                        new ReceberEntidadeRequest<T>()
                        {
                            Objeto = parametros.EntidadeCadastro,
                            IdUsuarioLogado = parametros.IdUsuarioLogado,
                            DescricaoUsuarioLogado = parametros.DescricaoUsuarioLogado                       
                        });

                resposta.EntidadeCadastro = respostaReceber.Objeto;
            }
            catch (Exception ex)
            {
                logger.Error(parametros, ex);
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return resposta;
        }

        public SalvarObjetoResponse<PessoaExpostaPoliticamenteImportacaoInfo> ImportarPessoasExpostasPoliticamente(SalvarObjetoRequest<PessoaExpostaPoliticamenteImportacaoInfo> pParametros)
        {
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();

            SalvarObjetoResponse<PessoaExpostaPoliticamenteImportacaoInfo> lResposta;

            lResposta =  servicoPersistencia.SalvarObjeto<PessoaExpostaPoliticamenteImportacaoInfo>(pParametros);

            lResposta.Objeto = pParametros.Objeto;

            //lResposta.IdUsuarioLogado = pParametros.IdUsuarioLogado;
            //lResposta.DescricaoUsuarioLogado = pParametros.DescricaoUsuarioLogado;

            /*
            try
            {

            }
            catch (Exception ex)
            {
                // Tratamento de erro
                Log.EfetuarLog(ex, pParametros);

                lResposta. = ex.ToString();
                lResposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                //throw ex;

            }
             * */

            return lResposta;
        }

        #endregion
    }
}
