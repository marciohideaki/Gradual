using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Classe utilizada para envio de eventos para um cliente wcf.
    /// Possui mecanismos de serialização para evitar alguns problemas
    /// com contratos wcf.
    /// Classe antiga... foi feita quando eu ainda não tinha domínio sobre
    /// o atributo ServiceKnownType
    /// </summary>
    [Serializable]
    public class EventoInfo
    {
        public EventoInfo()
        {
        }

        public EventoInfo(object obj, EventoInfoSerializacaoTipoEnum serializacaoTipo)
        {
            this.ObjetoSerializar(obj, serializacaoTipo);
        }

        /// <summary>
        /// String que contem o nome do tipo do objeto.
        /// Utilizado para desserializar objetos via serialização Xml
        /// </summary>
        public string ObjetoTipo { get; set; }

        /// <summary>
        /// Tipo da serialização que este objeto está carregando.
        /// Utilizado para saber como desserializar o objeto.
        /// </summary>
        public EventoInfoSerializacaoTipoEnum ObjetoSerializacaoTipo { get; set; }
        
        /// <summary>
        /// Contem o objeto serializado
        /// </summary>
        public byte[] ObjetoSerializado { get; set; }

        /// <summary>
        /// Contem o nome do evento
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Lista dos nomes de parametros que vieram no evento
        /// </summary>
        public List<string> ParametrosNome { get; set; }

        /// <summary>
        /// Lista dos valores dos parametros que vieram no evento
        /// </summary>
        public List<object> ParametrosValor { get; set; }

        /// <summary>
        /// Serializa o objeto e guarda em ObjetoSerializado
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objetoSerializacaoTipo"></param>
        public void ObjetoSerializar(object obj, EventoInfoSerializacaoTipoEnum objetoSerializacaoTipo)
        {
            // Faz a serializacao
            switch (objetoSerializacaoTipo)
            {
                case EventoInfoSerializacaoTipoEnum.XmlProprietario:
                    this.ObjetoSerializado = ASCIIEncoding.Default.GetBytes(Serializador.SerializaParametro(obj));
                    break;
                case EventoInfoSerializacaoTipoEnum.Binario:
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream ms = new MemoryStream();
                    formatter.Serialize(ms, obj);
                    this.ObjetoSerializado = ms.ToArray();
                    break;
            }

            // Indica outras propriedades necessarias
            this.ObjetoSerializacaoTipo = objetoSerializacaoTipo;
            this.ObjetoTipo = obj.GetType().FullName;
        }

        public static EventoInfo Serializar(object obj, EventoInfoSerializacaoTipoEnum serializacaoTipo)
        {
            EventoInfo eventoInfo = new EventoInfo();
            eventoInfo.ObjetoSerializar(obj, serializacaoTipo);
            return eventoInfo;
        }

        /// <summary>
        /// Desserializa o objeto e retorna
        /// </summary>
        /// <returns></returns>
        public object ObjetoDesserializar()
        {
            // Desserializa de acordo com o tipo da serializacao
            object retorno = null;
            switch (this.ObjetoSerializacaoTipo)
            {
                case EventoInfoSerializacaoTipoEnum.XmlProprietario:
                    retorno = ASCIIEncoding.Default.GetString(this.ObjetoSerializado);
                    break;
                case EventoInfoSerializacaoTipoEnum.Binario:
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream ms = new MemoryStream(this.ObjetoSerializado);
                    retorno = formatter.Deserialize(ms);
                    break;
            }

            // Retorna
            return retorno;
        }

        /// <summary>
        /// Adiciona um parametro nas devidas coleções
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="valor"></param>
        public void ParametroSetar(string nome, object valor)
        {
            // Adiciona se ainda não existe
            if (!this.ParametrosNome.Contains(nome))
            {
                this.ParametrosNome.Add(nome);
                this.ParametrosValor.Add(null);
            }

            // Seta o valor
            this.ParametrosValor[this.ParametrosNome.IndexOf(nome)] = valor;
        }

        /// <summary>
        /// Remove um parametro nas devidas coleções
        /// </summary>
        /// <param name="nome"></param>
        public void ParametroRemover(string nome)
        {
            this.ParametrosValor.RemoveAt(this.ParametrosNome.IndexOf(nome));
            this.ParametrosNome.Remove(nome);
        }

        /// <summary>
        /// Recebe um parametro
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public object ParametroReceber(string nome)
        {
            return this.ParametrosValor[this.ParametrosNome.IndexOf(nome)];
        }
    }
}
