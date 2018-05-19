using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Suitability.Service.Objetos
{
    public class ClienteSuitability
    {
        public string Codigo            { get; set; }
        public string Nome              { get; set; }
        public string CpfCnpj           { get; set; }
        public string Conta             { get; set; }
        public string CodigoAssessor    { get; set; }
        public string Email             { get; set; }
        public string Fonte             { get; set; }
        public string Status            { get; set; }
        public string idPerfil          { get; set; }
        public string idQuestionario    { get; set; }
        public string Descricao         { get; set; }
        public string PerfilAnterior    { get; set; }
        public string Data              { get; set; }
        public string Peso              { get; set; }
        public string Login             { get; set; }
        public string Preenchimento     { get; set; }

        public ClienteSuitability() { }
    }
}
