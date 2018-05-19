using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

using Gradual.OMS.Persistencias.Seguranca.Entidades;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Persistencia;

namespace Gradual.OMS.Persistencias.Seguranca
{
    /// <summary>
    /// Classe para resolver a persistencia para banco de dados.
    /// Repassa a chamada para a classe de entidadeDb correspondente ao tipo de entidade.
    /// </summary>
    public class PersistenciaSegurancaDb : IPersistencia
    {
        /// <summary>
        /// Dicionário com as bibliotecas de persistencia das entidades
        /// </summary>
        private Dictionary<Type, object> _dicionarioEntidadesDb = new Dictionary<Type, object>();

        /// <summary>
        /// Dicionário com a lista de tipos que a persistencia trabalha
        /// </summary>
        private List<Type> _listaTipos = new List<Type>();

        /// <summary>
        /// Construtor default
        /// </summary>
        public PersistenciaSegurancaDb()
        {
            // Pega lista de tipos de entidades db
            string namespaceEntidades = typeof(PersistenciaSegurancaDb).Namespace + ".Entidades";
            foreach (Type tipo in typeof(PersistenciaSegurancaDb).Assembly.GetTypes())
            {
                if (tipo.Namespace == namespaceEntidades)
                {
                    if (tipo.GetInterfaces().Length > 0 && tipo.GetInterfaces()[0].GetGenericArguments().Length > 0)
                    {
                        // Acha o tipo de entidade que esta persistencia trata
                        Type tipoEntidade = tipo.GetInterfaces()[0].GetGenericArguments()[0];
                        _listaTipos.Add(tipoEntidade);

                        // Cria instancia
                        object entidadeDb = Activator.CreateInstance(tipo);

                        // Adiciona no dicionario
                        _dicionarioEntidadesDb.Add(tipoEntidade, entidadeDb);
                    }
                }
            }
        }

        #region IPersistencia Members

        public ListarTiposResponse ListarTipos(ListarTiposRequest parametros)
        {
            return
                new ListarTiposResponse()
                {
                    Resultado = _listaTipos
                };
        }

        public AtualizarMetadadosResponse AtualizarMetadados(AtualizarMetadadosRequest parametros)
        {
            return null;
        }

        public ConsultarObjetosResponse<T> ConsultarObjetos<T>(ConsultarObjetosRequest<T> parametros) where T : ICodigoEntidade
        {
            return repassarMensagem(typeof(T), parametros, "ConsultarObjetos") as ConsultarObjetosResponse<T>;
        }

        public ReceberObjetoResponse<T> ReceberObjeto<T>(ReceberObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            return repassarMensagem(typeof(T), parametros, "ReceberObjeto") as ReceberObjetoResponse<T>;
        }

        public SalvarObjetoResponse<T> SalvarObjeto<T>(SalvarObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            return repassarMensagem(typeof(T), parametros, "SalvarObjeto") as SalvarObjetoResponse<T>;
        }

        public RemoverObjetoResponse<T> RemoverObjeto<T>(RemoverObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            return repassarMensagem(typeof(T), parametros, "RemoverObjeto") as RemoverObjetoResponse<T>;
        }

        private object repassarMensagem(Type tipoEntidade, object mensagem, string nomeMetodo)
        {
            // Acha a entidadeDb e o seu tipo
            object entidadeDb = _dicionarioEntidadesDb[tipoEntidade];
            Type tipoEntidadeDb = entidadeDb.GetType();
            
            // Faz a chamada retornando o resultado
            return 
                tipoEntidadeDb.InvokeMember(
                    nomeMetodo, 
                    System.Reflection.BindingFlags.InvokeMethod, 
                    null, 
                    entidadeDb, 
                    new object[] { mensagem });
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
            return ServicoStatus.Indefinido;
        }

        #endregion
    }
}
