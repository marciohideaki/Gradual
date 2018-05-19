using System;

namespace Gradual.Cadastro.Entidades
{
    public class EAlteracao
    {
        public System.Nullable<int> ID_Alteracao { get; set; }
        public System.Nullable<int> ID_Cliente { get; set; }
        public System.Nullable<int> ID_Administrador { get; set; }
        public string AdministradorNome { get; set; }
        public string NomeAdministrador { get; set; }
        public System.Nullable<DateTime> Data { get; set; }
        public System.Nullable<DateTime> DataRealizada { get; set; }
        public string Campo { get; set; }
        public char Tipo { get; set; }
        public string Descricao { get; set; }
        public string Ip { get; set; }
    }
}
