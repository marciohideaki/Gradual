using System;
using Gradual.Migracao.Educacional.Dados;
using Gradual.Migracao.Educacional.Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gradual.Migracao.Educacional
{
    [TestClass]
    public class UnitTest1
    {
        private EducacionalCompletoMensagemInfo gEducacionalCompletoMensagemInfo = new EducacionalCompletoMensagemInfo();

        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                this.BuscarDadosOracle();

                this.IniciarMigracao();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void IniciarMigracao()
        {
            var lEducacionalSQLDbLib = new EducacionalSQLDbLib(gEducacionalCompletoMensagemInfo);

            lEducacionalSQLDbLib.IniciarMigracao();
        }

        private void BuscarDadosOracle()
        {
            var lEducacionalOracleDbLib = new EducacionalOracleDbLib();

            gEducacionalCompletoMensagemInfo.AvaliacaoInteresseInfo = lEducacionalOracleDbLib.BuscarAvaliacaoInteresseOracle();
            gEducacionalCompletoMensagemInfo.AvaliacaoPalestraInfo = lEducacionalOracleDbLib.BuscarAvaliacaoPalestraOracle();
            gEducacionalCompletoMensagemInfo.ClienteCursoPalestraInfo = lEducacionalOracleDbLib.BuscarClienteCursoPalestraOracle();
            gEducacionalCompletoMensagemInfo.CursoPalestraInfo = lEducacionalOracleDbLib.BuscarCursoPalestraOracle();
            gEducacionalCompletoMensagemInfo.CursoPalestraOnLineInfo = lEducacionalOracleDbLib.BuscarCursoPalestraOnLineOracle();
            gEducacionalCompletoMensagemInfo.EstadoInfo = lEducacionalOracleDbLib.BuscarEstadoOracle();
            gEducacionalCompletoMensagemInfo.FichaPerfilInfo = lEducacionalOracleDbLib.BuscarFichaPerfilOracle();
            gEducacionalCompletoMensagemInfo.LocalidadeInfo = lEducacionalOracleDbLib.BuscarLocalidadeOracle();
            gEducacionalCompletoMensagemInfo.NivelInfo = lEducacionalOracleDbLib.BuscarNivelOracle();
            gEducacionalCompletoMensagemInfo.PalestranteInfo = lEducacionalOracleDbLib.BuscarPalestranteOracle();
            gEducacionalCompletoMensagemInfo.PalestraSobMedidaInfo = lEducacionalOracleDbLib.BuscarPalestraSobMedidaOracle();
            gEducacionalCompletoMensagemInfo.PerfilInfo = lEducacionalOracleDbLib.BuscarPerfilOracle();
            gEducacionalCompletoMensagemInfo.TemaInfo = lEducacionalOracleDbLib.BuscarTemaOracle();
            gEducacionalCompletoMensagemInfo.UsuarioInfo = lEducacionalOracleDbLib.BuscarUsuarioOracle();
        }
    }
}
