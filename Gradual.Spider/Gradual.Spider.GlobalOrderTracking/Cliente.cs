using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Spider.GlobalOrderTracking
{
    public class Cliente : System.ComponentModel.INotifyPropertyChanged
    {
        #region Propriedades

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private string codBovespa;
        public string CodBovespa
        {
            get { return codBovespa; }
            set { SetField(ref codBovespa, value, "CodBovespa"); }
        }

        private string codBMF;
        public string CodBMF
        {
            get { return codBMF; }
            set { SetField(ref codBMF, value, "CodBMF"); }
        }

        public int? codAssessor;
        public int? CodAssessor
        {
            get { return codAssessor; }
            set { SetField(ref codAssessor, value, "CodAssessor"); }
        }

        public int idCliente;
        public int IdCliente
        {
            get { return idCliente; }
            set { SetField(ref idCliente, value, "IdCliente"); }
        }

        public string nome;
        public string Nome
        {
            get { return nome; }
            set { SetField(ref nome, value, "Nome"); }
        }

        public string sexo;
        public string Sexo
        {
            get { return sexo; }
            set { SetField(ref sexo, value, "Sexo"); }
        }

        public string email;
        public string Email
        {
            get { return email; }
            set { SetField(ref email, value, "Email"); }
        }

        public int codigoLogado;
        public int CodigoLogado
        {
            get { return codigoLogado; }
            set { SetField(ref codigoLogado, value, "CodigoLogado"); }
        }

        #endregion

        #region Métodos Públicos

        public override string ToString()
        {
            return CodBovespa + " - " + Nome;
        }

        public String CodigoCliente
        {
            get
            {
                if (!string.IsNullOrEmpty(CodBovespa))
                {
                    return CodBovespa;
                }

                if (!string.IsNullOrEmpty(CodBMF))
                {
                    return CodBMF;
                }

                return "0";
            }
        }

        #endregion

        #region Construtores

        public Cliente() { }

        public Cliente(Gradual.OMS.CadastroCliente.Lib.ClienteResumidoInfo clienteResumido)
        {
            this.CodAssessor = clienteResumido.CodAssessor;
            this.CodBMF = clienteResumido.CodBMF;
            this.CodBovespa = clienteResumido.CodBovespa;
            this.Email = clienteResumido.Email;
            this.IdCliente = clienteResumido.IdCliente;
            this.Nome = clienteResumido.NomeCliente;
            this.Sexo = clienteResumido.Sexo;
        }

        #endregion
    }

}
