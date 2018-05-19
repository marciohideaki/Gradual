using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    [Serializable]
    public class ConversorLayoutBVMFConfig
    {
        /// <summary>
        /// Dicionario de tipos. 
        /// Lista de strings conteudo o par Tipo de Dado e Tipo da Classe de Conversao.
        /// Ex: System.DateTime,Orbite.RV.Sistemas.Integracao.BVMF.ConversorCampoData
        /// </summary>
        public List<string> DicionarioTipos { get; set; }

        /// <summary>
        /// Cria o dicionário com instâncias concretas dos conversores
        /// </summary>
        /// <returns></returns>
        public Dictionary<Type, ConversorCampoBase> CriarDicionario()
        {
            // Inicializa
            Dictionary<Type, ConversorCampoBase> dicionario = new Dictionary<Type, ConversorCampoBase>();
            
            // Lista temporaria com os conversores já criados
            Dictionary<Type, ConversorCampoBase> conversores = new Dictionary<Type, ConversorCampoBase>();

            // Varre o dicionario de configuracao
            foreach (string item in this.DicionarioTipos)
            {
                // Faz o split para receber as duas strings
                string[] item2 = item.Split(',');

                // Cria os tipos
                Type tipoDado = Type.GetType(item2[0]);
                Type tipoConversor = Type.GetType(item2[1]);

                // Verifica se deve criar o conversor
                if (!conversores.ContainsKey(tipoConversor))
                    conversores.Add(tipoConversor, (ConversorCampoBase)Activator.CreateInstance(tipoConversor));

                // Adiciona na coleção de campos
                dicionario.Add(tipoDado, conversores[tipoConversor]);
            }

            // Retorna
            return dicionario;
        }

        public ConversorLayoutBVMFConfig()
        {
            this.DicionarioTipos = new List<string>();
        }
    }
}
