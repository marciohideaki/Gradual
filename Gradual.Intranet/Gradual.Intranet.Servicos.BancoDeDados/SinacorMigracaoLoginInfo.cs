using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Servicos.BancoDeDados
{
    public class SinacorMigracaoLoginInfo
    {
        public string DsAssinatura { set; get; }
        public string DsCpf { set; get; }
        public string DsEmail { set; get; }
        public string DsNome { set; get; }
        public string DsSenha { set; get; }
        public DateTime DtNascimento { set; get; }
        public int IdAssessor { set; get; }
    }
}
