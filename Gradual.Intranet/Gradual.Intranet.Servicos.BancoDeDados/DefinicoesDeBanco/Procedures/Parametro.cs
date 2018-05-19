using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gradual.Generico.Dados;

namespace Gradual.Intranet.Servicos.BancoDeDados.DefinicoesDeBanco.Procedures
{
    public class Parametro
    {
        public string Nome { get; set; }
        public object Valor { get; set; }
        public int Tamanho { get; set; }
        public DbType Tipo { get; set; }
        public ParameterDirection  Direcao { get; set; }

    }
}
