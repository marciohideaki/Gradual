using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Interface.Desktop.Dados
{
    [Serializable]
    public class ItemInfoBase : ICloneable
    {
        public string Id { get; set; }

        public string Nome { get; set; }

        [XmlIgnore]
        public Type TipoInstancia 
        {
            get { return this.TipoInstanciaString != null ? Type.GetType(this.TipoInstanciaString) : null; }
            set { this.TipoInstanciaString = value != null ? value.FullName + ", " + value.Assembly.FullName : null; } 
        }

        [XmlAttribute]
        public string TipoInstanciaString { get; set; }

        public ItemTipoEnum ItemTipo { get; set; }

        public ObjetoSerializado Parametros { get; set; }

        public DockStyle DockStyle { get; set; }

        public ItemInfoBase()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public Type ReceberTipoInstancia()
        {
            // Inicializa
            Type tipoInstancia = null;

            // Cria a instancia do item desejado
            if (this.TipoInstancia != null)
                tipoInstancia = this.TipoInstancia;
            else if (this.TipoInstanciaString != null)
                tipoInstancia = Type.GetType(this.TipoInstanciaString);

            // Retorna
            return tipoInstancia;
        }

        #region ICloneable Members

        public object Clone()
        {
            ItemInfoBase item = 
                (ItemInfoBase)
                    Activator.CreateInstance(this.GetType());
            item.DockStyle = this.DockStyle;
            item.ItemTipo = this.ItemTipo;
            item.Nome = this.Nome;
            item.Parametros = this.Parametros;
            item.TipoInstancia = this.TipoInstancia;
            item.TipoInstanciaString = this.TipoInstanciaString;
            return item;
        }

        #endregion
    }
}
