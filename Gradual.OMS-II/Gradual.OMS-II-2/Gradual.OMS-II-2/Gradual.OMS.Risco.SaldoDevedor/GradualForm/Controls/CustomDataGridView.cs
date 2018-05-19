using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GradualForm.Controls
{
    public partial class CustomDataGridView : System.Windows.Forms.DataGridView
    {
        public CustomDataGridView()
        {
            DoubleBuffered = true;
        }

        public void Sort(Ordenacao ordenacao)
        {
            switch (ordenacao.Direcao)
            {
                case SortDirection.Ascending:
                    {
                        this.Sort(ordenacao.Coluna, ListSortDirection.Ascending);
                        break;
                    }
                case SortDirection.Descending:
                    {
                        this.Sort(ordenacao.Coluna, ListSortDirection.Descending);
                        break;
                    }
                case SortDirection.Unsort:
                    {
                        foreach (System.Windows.Forms.DataGridViewColumn column in this.Columns)
                        {
                            column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                        }
                        break;
                    }
                default:
                    {
                        this.Columns[ordenacao.Coluna.Index].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                        break;
                    }

            }
        }
    }

    public enum SortDirection { Ascending, Descending, Unsort }

    /// <summary>
    ///     Classe recipiente responsável por armazenar as informações de ordenação
    /// </summary>
    public class Ordenacao : System.ComponentModel.INotifyPropertyChanged
    {
        private System.Windows.Forms.DataGridViewColumn coluna;
        private SortDirection direcao;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(System.String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(info));
            }
        }

        // The constructor is private to enforce the factory pattern. 
        public Ordenacao()
        {
            this.Direcao = SortDirection.Ascending;
        }

        public System.Windows.Forms.DataGridViewColumn Coluna
        {
            get
            {
                return this.coluna;
            }

            set
            {
                if (value != this.coluna)
                {
                    this.coluna = value;
                    NotifyPropertyChanged("Coluna");
                }
            }
        }

        public SortDirection Direcao
        {
            get
            {
                return this.direcao;

            }

            set
            {
                if (value != this.direcao)
                {
                    this.direcao = value;
                    NotifyPropertyChanged("Direcao");
                }
            }
        }
    }

}
