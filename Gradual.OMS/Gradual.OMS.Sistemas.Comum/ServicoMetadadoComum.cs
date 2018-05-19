using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Interface para o serviço de metadados para bancos de dados.
    /// A motivação inicial é sincronizar os enumeradores com as tabelas
    /// Lista e ListaItem.
    /// </summary>
    public class ServicoMetadadoComum : IServicoMetadadoComum
    {
        #region IServicoDbMetadado Members

        /// <summary>
        /// Faz a geração e/ou sincronismo dos enumeradores informados
        /// com as tabelas Lista e ListaItem
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public GerarDbMetadadoResponse GerarMetadadoComum(GerarDbMetadadoRequest parametros)
        {
            // Prepara resposta
            GerarDbMetadadoResponse resposta =
                new GerarDbMetadadoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Varre os enumeradores
            foreach (Type tipo in parametros.Enumeradores)
            {
                // Cria Lista
                ListaInfo lista =
                    new ListaInfo()
                    {
                        Mnemonico = tipo.Name
                    };

                // Tem descricao?
                object[] descs = tipo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (descs.Length > 0)
                    lista.Descricao = ((DescriptionAttribute)descs[0]).Description;

                // Varre os elementos
                foreach (FieldInfo field in tipo.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
                {
                    // Cria o item
                    ListaItemInfo item =
                        new ListaItemInfo()
                        {
                            Mnemonico = field.Name,
                            Valor = ((int)field.GetValue(null)).ToString()
                        };

                    // Tem descricao?
                    object[] descs2 = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (descs2.Length > 0)
                        item.Descricao = ((DescriptionAttribute)descs2[0]).Description;

                    // Adiciona na coleção de itens
                    lista.Itens.Add(item);
                }

                // Adiciona lista na resposta
                resposta.Listas.Add(lista);
            }

            // Retorna
            return resposta;
        }

        #endregion
    }
}
