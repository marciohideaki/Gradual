using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Interface.Dados;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Persistencias
{
    /// <summary>
    /// Classe de persistencia para banco de dados dos objetos de interface:
    /// GrupoFuncionalidade, GrupoComando e Sistema
    /// </summary>
    public class PersistenciaInterfaceDb : IPersistencia
    {
        private ServicoStatus _status = ServicoStatus.Parado;

        private GrupoFuncionalidadeDbLib _grupoFuncionalidadeDbLib = new GrupoFuncionalidadeDbLib();
        private SistemaDbLib _sistemaDbLib = new SistemaDbLib();

        #region IPersistencia Members

        public ListarTiposResponse ListarTipos(ListarTiposRequest parametros)
        {
            return
                new ListarTiposResponse()
                {
                    Resultado =
                        new List<Type>() 
                        { 
                            typeof(GrupoComandoInterfaceInfo), 
                            typeof(GrupoFuncionalidadeInfo),
                            typeof(ModulosOMS)
                        }
                };
        }

        /// <summary>
        /// Solicita atualização de metadados.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public AtualizarMetadadosResponse AtualizarMetadados(AtualizarMetadadosRequest parametros)
        {
            // ** Precisa atualizar os lista itens
            return new AtualizarMetadadosResponse();
        }

        public ConsultarObjetosResponse<T> ConsultarObjetos<T>(ConsultarObjetosRequest<T> parametros) where T : ICodigoEntidade
        {
            // Inicializa
            Type tipoObjeto = typeof(T);
            ConsultarObjetosResponse<T> resposta = new ConsultarObjetosResponse<T>();

            // Bloco de controle
            try
            {
                // Executa de acordo com o tipo
                if (tipoObjeto == typeof(GrupoFuncionalidadeInfo))
                    resposta.Resultado = _grupoFuncionalidadeDbLib.Consultar(parametros.Condicoes) as List<T>;
                else if (tipoObjeto == typeof(ModulosOMS))
                    resposta.Resultado = _sistemaDbLib.Consultar(parametros.Condicoes) as List<T>;
                //else if (tipoObjeto == typeof(UsuarioGrupoInfo))
                //    resposta.Resultado = _usuarioGrupoDbLib.Consultar(parametros.Condicoes) as List<T>;

            }
            catch (Exception ex)
            {
                // Log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloInterface);

                // Repassa o erro
                throw (ex);
            }

            // Retorna
            return resposta;
        }

        public ReceberObjetoResponse<T> ReceberObjeto<T>(ReceberObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            // Inicializa
            Type tipoObjeto = typeof(T);
            ReceberObjetoResponse<T> resposta = new ReceberObjetoResponse<T>();
            object objetoResposta = null;

            // Bloco de controle
            try
            {
                // Executa de acordo com o tipo
                if (tipoObjeto == typeof(GrupoFuncionalidadeInfo))
                    objetoResposta = _grupoFuncionalidadeDbLib.Receber(parametros.CodigoObjeto);
                else if (tipoObjeto == typeof(ModulosOMS))
                    objetoResposta = _sistemaDbLib.Receber(parametros.CodigoObjeto);
                //else if (tipoObjeto == typeof(UsuarioGrupoInfo))
                //    objetoResposta = _usuarioGrupoDbLib.Receber(parametros.CodigoObjeto);

            }
            catch (Exception ex)
            {
                // Log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloInterface);

                // Repassa o erro
                throw (ex);
            }

            // Insere o objeto na resposta
            resposta.GetType().InvokeMember("Objeto", BindingFlags.SetProperty, null, resposta, new object[] { objetoResposta });

            // Retorna
            return resposta;
        }

        public SalvarObjetoResponse<T> SalvarObjeto<T>(SalvarObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            // Inicializa
            Type tipoObjeto = typeof(T);
            SalvarObjetoResponse<T> resposta = new SalvarObjetoResponse<T>();
            object objetoResposta = null;

            // Bloco de controle
            try
            {
                // Executa de acordo com o tipo
                if (tipoObjeto == typeof(GrupoFuncionalidadeInfo))
                    objetoResposta = _grupoFuncionalidadeDbLib.Salvar((parametros as SalvarObjetoRequest<GrupoFuncionalidadeInfo>).Objeto);
                else if (tipoObjeto == typeof(ModulosOMS))
                    objetoResposta = _sistemaDbLib.Salvar((parametros as SalvarObjetoRequest<SistemaInfo>).Objeto);
                //else if (tipoObjeto == typeof(UsuarioGrupoInfo))
                //    objetoResposta = _usuarioGrupoDbLib.Salvar((parametros as SalvarObjetoRequest<UsuarioGrupoInfo>).Objeto);
            }
            catch (Exception ex)
            {
                // Log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloInterface);

                // Repassa o erro
                throw (ex);
            }

            // Insere o objeto na resposta
            resposta.GetType().InvokeMember("Objeto", BindingFlags.SetProperty, null, resposta, new object[] { objetoResposta });

            // Retorna
            return resposta;
        }

        public RemoverObjetoResponse<T> RemoverObjeto<T>(RemoverObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            // Inicializa
            Type tipoObjeto = typeof(T);
            RemoverObjetoResponse<T> resposta = new RemoverObjetoResponse<T>();

            // Bloco de controle
            try
            {
                // Executa de acordo com o tipo
                if (tipoObjeto == typeof(GrupoFuncionalidadeInfo))
                    _grupoFuncionalidadeDbLib.Remover(parametros.CodigoObjeto);
            }
            catch (Exception ex)
            {
                // Log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloInterface);

                // Repassa o erro
                throw (ex);
            }

            // Retorna
            return resposta;
        }

        #endregion

        #region IServicoControlavel Members

        public void IniciarServico()
        {
        }

        public void PararServico()
        {
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        #endregion
    }
}
