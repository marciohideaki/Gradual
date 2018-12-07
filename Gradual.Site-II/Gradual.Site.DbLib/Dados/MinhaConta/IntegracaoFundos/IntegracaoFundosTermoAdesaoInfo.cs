using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos
{
    public class IntegracaoFundosTermoAdesaoInfo
    {
        public Nullable<int> IdTermo { get; set; }

        public string NomeTermo      { get; set; }

        public IntegracaoFundosTermoAdesaoInfo()
        {
        }

        public string CodigoCliente  { get; set; }

        public string NomeCliente    { get; set; }

        public string NomeFundo      { get; set; }

        public string CodigoAnbima   { get; set; }

        public int CodigoFundo       { get; set; }

        public DateTime DtHoraAdesao { get; set; }

        public string PathTermo      { get; set; }

        public string Origem         { get; set; }

        public string DsUsuarioLogado { get; set; }

        public int CodigoUsuarioLogado { get; set; }
    }
}
