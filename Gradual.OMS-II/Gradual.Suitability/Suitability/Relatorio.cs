using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Suitability
{
    public partial class Relatorio : Form
    {
        public Relatorio()
        {
            InitializeComponent();
        }

        List<String> gListaClientes = new List<string>();
        List<ClientePerfil> gListaCLientePerfil = new List<ClientePerfil>();
        List<ClientePerfil> gListaCLientePerfilRelatorio = new List<ClientePerfil>();

        private void CarregarListaClientes()
        {
            Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            Gradual.Generico.Dados.AcessaDados lAcessaDados = new Gradual.Generico.Dados.AcessaDados();

            string lCommand = "SELECT DISTINCT ID_CLIENTE FROM TB_SUITA_REL ORDER BY ID_CLIENTE";

            lAcessaDados.ConnectionStringName = "Cadastro";

            using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.Text, lCommand))
            {
                System.Data.DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        if (!gListaClientes.Contains((lDataTable.Rows[i]["id_cliente"]).DBToString()))
                        {
                            gListaClientes.Add((lDataTable.Rows[i]["id_cliente"]).DBToString());
                        }
                    }
                }
            }
        }

        private void CarregarListaClientePerfil()
        {
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                Gradual.Generico.Dados.AcessaDados lAcessaDados = new Gradual.Generico.Dados.AcessaDados();

                //string lCommand = "SELECT * FROM TB_SUITA_REL where dt_realizacao is not null";
                string lCommand = "select id_cliente, ds_nome, ds_cpfcnpj, ds_fonte, cd_codigo, ds_perfil, CONVERT(DATETIME, ISNULL(dt_realizacao, '1900-01-01')) as dt_realizacao from tb_suita_rel";

                lAcessaDados.ConnectionStringName = "Cadastro";

                using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.Text, lCommand))
                {
                    System.Data.DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            //id_cliente, ds_nome, ds_cpfcnpj, cd_codigo, ds_perfil, dt_realizacao
                            ClientePerfil lClientePerfil = new ClientePerfil();
                            lClientePerfil.id_cliente = (lDataTable.Rows[i]["id_cliente"]).DBToString();
                            lClientePerfil.ds_nome = (lDataTable.Rows[i]["ds_nome"]).DBToString();
                            lClientePerfil.ds_cpfcnpj = (lDataTable.Rows[i]["ds_cpfcnpj"]).DBToString();
                            lClientePerfil.cd_codigo = (lDataTable.Rows[i]["cd_codigo"]).DBToString();
                            lClientePerfil.ds_fonte = (lDataTable.Rows[i]["ds_fonte"]).DBToString();
                            lClientePerfil.ds_perfil = (lDataTable.Rows[i]["ds_perfil"]).DBToString();
                            lClientePerfil.dt_realizacao = Convert.ToDateTime((lDataTable.Rows[i]["dt_realizacao"]).DBToString());
                            gListaCLientePerfil.Add(lClientePerfil);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void MontarRelatorio()
        {
            Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            foreach (String lCliente in gListaClientes)
            {
                var lOcorrenciasClientes = from cliente in gListaCLientePerfil where cliente.id_cliente.Equals(lCliente) orderby cliente.dt_realizacao descending select cliente;

                if (lOcorrenciasClientes.Count() > 0)
                {
                    for (int i=0; i < lOcorrenciasClientes.Count(); i++)
                    {
                        ClientePerfil lClientePerfil = lOcorrenciasClientes.ToList()[i];

                        if (lOcorrenciasClientes.Count() > i + 1)
                        {
                            lClientePerfil.ds_perfil_anterior = lOcorrenciasClientes.ToList()[i + 1].ds_perfil;
                            lClientePerfil.dt_realizacao_anterior = lOcorrenciasClientes.ToList()[i + 1].dt_realizacao;
                        }

                        gListaCLientePerfilRelatorio.Add(lClientePerfil);
                    }
                }
            }

        }

        private void GerarArquivo()
        {
            List<String> lines = new List<String>();
            foreach (ClientePerfil lCliente in gListaCLientePerfilRelatorio)
            {
                String lLinha = String.Format("{0};{1};{2};{3};{4};{5};{6}", lCliente.ds_cpfcnpj, lCliente.cd_codigo, lCliente.ds_nome, lCliente.ds_fonte, lCliente.ds_perfil_anterior, lCliente.ds_perfil, lCliente.dt_realizacao);
                lines.Add(lLinha);
            }

            System.IO.File.WriteAllLines(@"C:\Hideaki\Temp\Relatorio.txt", lines);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CarregarListaClientes();
            CarregarListaClientePerfil();
            MontarRelatorio();
            GerarArquivo();
        }
    }

    public class ClientePerfil
    {
        public string id_cliente {get; set;}
        public string ds_nome {get; set;}
        public string ds_cpfcnpj {get; set;}
        public string cd_codigo {get; set;}
        public string ds_fonte { get; set; }
        public string ds_perfil {get; set;}
        public DateTime dt_realizacao { get; set; }
        public string ds_perfil_anterior { get; set; }
        public DateTime dt_realizacao_anterior { get; set; }
    }

}
