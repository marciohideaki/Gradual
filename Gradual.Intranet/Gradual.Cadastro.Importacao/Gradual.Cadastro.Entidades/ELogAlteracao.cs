
using System;
namespace Gradual.Cadastro.Entidades
{
    public class ELogAlteracao
    {
        public int ID_Log { get; set; }
        public int ID_Alteracao { get; set; }
        public int ID_Administrador { get; set; }
        public string DadosAntidos { get; set; }
        public string DadosNovos { get; set; }
        public DateTime Data { get; set; }
    }
}
