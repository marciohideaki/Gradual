using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Ordens
{
    /// <summary>
    /// Implementação do serviço de persistência de ordens utilizando 
    /// o serviço de persistência de objetos (IServicoPersistencia)
    /// </summary>
    public class ServicoOrdensPersistencia : IServicoOrdensPersistencia
    {
        #region Variaveis Locais

        /// <summary>
        /// Referencia para o servico de persistencia
        /// </summary>
        private IServicoPersistencia _servicoPersistencia = Ativador.Get<IServicoPersistencia>();

        #endregion

        #region Construtores e Destrutor

        /// <summary>
        /// Construtor default.
        /// </summary>
        public ServicoOrdensPersistencia()
        {
        }

        #endregion

        #region IServicoOrdensPersistencia Members

        public SalvarOrdemResponse SalvarOrdem(SalvarOrdemRequest parametros)
        {
            // Salva
            _servicoPersistencia.SalvarObjeto<OrdemInfo>(
                new SalvarObjetoRequest<OrdemInfo>() 
                { 
                    Objeto = parametros.OrdemInfo 
                });

            // Retorna
            return
                new SalvarOrdemResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        public ReceberOrdemResponse ReceberOrdem(ReceberOrdemRequest parametros)
        {
            // Inicializa
            OrdemInfo retorno = null;

            // Verifica se pediu pela chave ou outro atributo
            if (parametros.ClOrdID != null)
            {
                // Pega pela chave
                retorno =
                    _servicoPersistencia.ReceberObjeto<OrdemInfo>(
                        new ReceberObjetoRequest<OrdemInfo>()
                        {
                            CodigoObjeto = parametros.ClOrdID
                        }).Objeto;
            }
            else
            {
                // Faz uma consulta
                ConsultarObjetosResponse<OrdemInfo> consultaResponse =
                    _servicoPersistencia.ConsultarObjetos<OrdemInfo>(
                        new ConsultarObjetosRequest<OrdemInfo>()
                        {
                            Condicoes = 
                                new List<CondicaoInfo>() 
                                { 
                                    new CondicaoInfo() 
                                    { 
                                        Propriedade = "CodigoExterno", 
                                        TipoCondicao = CondicaoTipoEnum.Igual, 
                                        Valores = new object[] { parametros.CodigoExterno } 
                                    } 
                                }
                        });

                // Achou?
                if (consultaResponse.Resultado.Count > 0)
                    retorno = consultaResponse.Resultado[0];
            }

            // Retorna a ordem da coleção
            return 
                new ReceberOrdemResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    OrdemInfo = retorno 
                };
        }

        public ListarOrdensResponse ListarOrdens(ListarOrdensRequest parametros)
        {
            // Transforma as condições em lista de condições para a pesquisa na persistencia
            List<CondicaoInfo> condicoes = new List<CondicaoInfo>();
            if (parametros.FiltroCodigoCliente != null)
                condicoes.Add(new CondicaoInfo("CodigoCliente", CondicaoTipoEnum.Igual, parametros.FiltroCodigoCliente));
            if (parametros.FiltroCodigoSessao != null)
                condicoes.Add(new CondicaoInfo("CodigoSessao", CondicaoTipoEnum.Igual, parametros.FiltroCodigoSessao));
            if (parametros.FiltroDataMaiorIgual.HasValue)
                condicoes.Add(new CondicaoInfo("DataReferencia", CondicaoTipoEnum.MaiorIgual, parametros.FiltroDataMaiorIgual.Value));
            if (parametros.FiltroDataMenor.HasValue)
                condicoes.Add(new CondicaoInfo("DataReferencia", CondicaoTipoEnum.Menor, parametros.FiltroDataMenor.Value));
            if (parametros.FiltroDataUltimaAlteracaoMaior.HasValue)
                condicoes.Add(new CondicaoInfo("DataUltimaAlteracao", CondicaoTipoEnum.Menor, parametros.FiltroDataUltimaAlteracaoMaior.Value));
            if (parametros.FiltroStatus.HasValue)
                condicoes.Add(new CondicaoInfo("Status", CondicaoTipoEnum.Igual, parametros.FiltroStatus.Value));
            if (parametros.FiltroCodigoBolsa != null)
                condicoes.Add(new CondicaoInfo("CodigoBolsa", CondicaoTipoEnum.Igual, parametros.FiltroCodigoBolsa));
            if (parametros.FiltroInstrumento != null)
                condicoes.Add(new CondicaoInfo("Instrumento", CondicaoTipoEnum.Igual, parametros.FiltroInstrumento));
            if (parametros.FiltroCodigoExterno != null)
                condicoes.Add(new CondicaoInfo("CodigoExterno", CondicaoTipoEnum.Igual, parametros.FiltroCodigoExterno));
            if (parametros.FiltroCodigoExternoNulo)
                condicoes.Add(new CondicaoInfo("CodigoExterno", CondicaoTipoEnum.Igual, null));
            if (parametros.FiltroCodigoCanal != null)
                condicoes.Add(new CondicaoInfo("CodigoCanal", CondicaoTipoEnum.Igual, parametros.FiltroCodigoCanal));
            if (parametros.FiltroCodigoSistemaCliente != null)
                condicoes.Add(new CondicaoInfo("CodigoSistemaCliente", CondicaoTipoEnum.Igual, parametros.FiltroCodigoSistemaCliente));

            // Retorna a lista de acordo com os filtros
            return
                new ListarOrdensResponse()
                {
                    Ordens = 
                        _servicoPersistencia.ConsultarObjetos<OrdemInfo>(
                            new ConsultarObjetosRequest<OrdemInfo>() 
                            { 
                                Condicoes = condicoes 
                            }).Resultado
                };
        }

        #endregion
    }
}
