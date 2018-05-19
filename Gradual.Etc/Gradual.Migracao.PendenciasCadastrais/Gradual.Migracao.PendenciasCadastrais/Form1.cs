using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using Gradual.Generico.Dados;
using Gradual.Migracao.PendenciasCadastrais.Dados;
using Gradual.Migracao.PendenciasCadastrais.Entidades;

namespace Gradual.Migracao.PendenciasCadastrais
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ButtonIniciarMigracao_Click(object sender, EventArgs e)
        {
            this.IniciarMigracao();
        }

        private void IniciarMigracao()
        {
            DbConnection lDbConnection;
            DbTransaction lDbTransaction;
            Conexao lConexao = new Conexao();
            lConexao._ConnectionStringName = "ConexaoSQL";
            lDbConnection = lConexao.CreateIConnection();
            lDbConnection.Open();
            lDbTransaction = lDbConnection.BeginTransaction();

            try
            {
                {   //--> Limpando e atualizando a base com a lista de 'Tipo de Pendencias'
                    new CadastroSQLDbLib().ExcluirTipoPendencia(lDbTransaction);

                    new CadastroSQLDbLib().InserirTipoPendencias(lDbTransaction);
                }

                var lListaTipoPendencias = new CadastroSQLDbLib().BuscarTipoPendencias(lDbConnection, lDbTransaction);

                var lListaPendencias = new CadastroOracleDbLib().RecuperarPendenciasPorCliente();

                var lListaPendenciasTraduzidas = new CadastroSQLDbLib().TraduzirPendencias(lListaPendencias, lListaTipoPendencias);

                new CadastroSQLDbLib().InserirClientePendencia(lDbTransaction, lListaPendenciasTraduzidas);

                lDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                lDbTransaction.Rollback();
                throw ex;
            }
            finally
            {
                lDbTransaction.Dispose();
                lDbTransaction = null;
                if (!ConnectionState.Closed.Equals(lDbConnection.State)) lDbConnection.Close();
                lDbConnection.Dispose();
                lDbConnection = null;
            }
        }
    }
}
