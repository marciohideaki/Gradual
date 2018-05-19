using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Sistemas.Ordens
{
    /// <summary>
    /// Mantem a logica de armazenar e consultar os instrumentos.
    /// </summary>
    public class RepositorioInstrumentos
    {
        #region Propriedades

        /// <summary>
        /// Indica a data em que a lista foi carregada.
        /// </summary>
        public DateTime DataCarregamento { get; set; }

        /// <summary>
        /// Contem a lista de instrumentos carregados.
        /// </summary>
        public List<InstrumentoInfo> Instrumentos { get; set; }

        /// <summary>
        /// Índice ordenado por SecurityID.
        /// </summary>
        [XmlIgnore]
        public Dictionary<string, InstrumentoInfo> IndiceSecurityID { get; set; }
        
        /// <summary>
        /// Índice ordenado por Symbol.
        /// </summary>
        [XmlIgnore]
        public Dictionary<string, InstrumentoInfo> IndiceSymbol { get; set; }

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public RepositorioInstrumentos()
        {
            this.LimparLista();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega a lista de instrumentos.
        /// Seta a data de carregamento para a data atual.
        /// </summary>
        /// <param name="instrumentos">Lista de instrumentos a ser carregado</param>
        public void CarregarLista(List<InstrumentoInfo> instrumentos)
        {
            // Carrega a lista
            foreach (InstrumentoInfo instrumento in instrumentos)
                this.AdicionarInstrumento(instrumento);

            // Sinaliza
            this.DataCarregamento = DateTime.Now;
        }

        /// <summary>
        /// Adiciona um instrumento na lista. 
        /// Atualiza os índices.
        /// </summary>
        /// <param name="instrumentoInfo">Instrumento a ser carregado.</param>
        public void AdicionarInstrumento(InstrumentoInfo instrumentoInfo)
        {
            // Adiciona
            this.Instrumentos.Add(instrumentoInfo);
            
            // Atualiza Indice por SecurityID
            if (!this.IndiceSecurityID.ContainsKey(instrumentoInfo.SecurityID))
                this.IndiceSecurityID.Add(instrumentoInfo.SecurityID, instrumentoInfo);
            
            // Atualiza Indice por Symbol
            if (!this.IndiceSymbol.ContainsKey(instrumentoInfo.Symbol))
                this.IndiceSymbol.Add(instrumentoInfo.Symbol, instrumentoInfo);
        }

        /// <summary>
        /// Limpa a lista interna.
        /// </summary>
        public void LimparLista()
        {
            this.Instrumentos = new List<InstrumentoInfo>();
            this.IndiceSecurityID = new Dictionary<string, InstrumentoInfo>();
            this.IndiceSymbol = new Dictionary<string, InstrumentoInfo>();
        }

        /// <summary>
        /// Consulta por securityID.
        /// </summary>
        /// <param name="securityID"></param>
        /// <returns></returns>
        public InstrumentoInfo ConsultarPorSecurityID(string securityID)
        {
            return this.IndiceSecurityID[securityID];
        }

        /// <summary>
        /// Consulta por Symbol.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public InstrumentoInfo ConsultarPorSymbol(string symbol)
        {
            return this.IndiceSymbol[symbol];
        }

        /// <summary>
        /// Salva o conteudo da classe para um arquivo
        /// </summary>
        /// <param name="arquivo"></param>
        public void Salvar(string arquivo)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RepositorioInstrumentos));
            FileStream fs = new FileStream(arquivo, FileMode.Create);
            serializer.Serialize(fs, this);
            fs.Close();
        }

        /// <summary>
        /// Carrega o conteudo de um arquivo para a classe
        /// </summary>
        /// <param name="arquivo"></param>
        public void Carregar(string arquivo)
        {
            FileStream fileStream = File.OpenRead(arquivo);
            XmlSerializer serializer = new XmlSerializer(typeof(RepositorioInstrumentos));
            RepositorioInstrumentos obj = (RepositorioInstrumentos)serializer.Deserialize(fileStream);
            this.CarregarLista(obj.Instrumentos);
            this.DataCarregamento = obj.DataCarregamento;
            fileStream.Close();
        }

        #endregion
    }
}
