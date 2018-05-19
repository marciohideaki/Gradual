using System;
using System.Data.OracleClient;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace Gradual.Cadastro.ExportaClientes
{
    public partial class ExportaCliente : ServiceBase
    {
        private System.Timers.Timer dtpHora = null;

        public ExportaCliente()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Segundo =1000
            //minito = 60
            //rodar a cada 10 min = 10
            dtpHora.Interval = 1000 * 60 * 10;
            dtpHora.Enabled = true;
            dtpHora.Start();
        }

        protected override void OnStop()
        {
            dtpHora.Enabled = false;
        }

        private string getBanco
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["Banco"].ToString(); }
        }

        private string getCaminhoCliente()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ArquivoClientes"].ToString();
        }

        private string getCaminhoAssessor()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ArquivoAssessores"].ToString();
        }

        private int getHora()
        {
            return int.Parse(System.Configuration.ConfigurationManager.AppSettings["Schedule"].ToString());
        }

        private void GerarAssessores()
        {
            try
            {
                var lTextoArquivo = new StringBuilder();

                using (OracleConnection lOracleConnection = new OracleConnection(this.getBanco))
                {
                    var lQuery = new StringBuilder();

                    lQuery.Append(" SELECT login.nome")
                          .Append(" ,      assessor.id_assessorsinacor")
                          .Append(" ,      filial.nome AS localidade")
                          .Append(" FROM   login")
                          .Append(" ,      assessor")
                          .Append(" ,      assessorfilial")
                          .Append(" ,      filial")
                          .Append(" WHERE  login.id_login = assessor.id_login")
                          .Append(" AND    assessor.id_assessor = assessorfilial.id_assessor")
                          .Append(" AND    assessorfilial.id_filial = filial.id_filial");

                    lOracleConnection.Open();

                    using (OracleCommand comCadastro = new OracleCommand(lQuery.ToString(), lOracleConnection))
                    {
                        OracleDataReader lOracleDataReader = comCadastro.ExecuteReader();

                        while (lOracleDataReader.Read())
                        {
                            lTextoArquivo.AppendFormat("{0};", TrataCaracter(lOracleDataReader["nome"].ToString()))
                                         .AppendFormat("{0};", TrataCaracter(lOracleDataReader["id_assessorsinacor"].ToString()))
                                         .AppendFormat("{0}\r\n", TrataCaracter(lOracleDataReader["localidade"].ToString()));
                        }
                    }
                }

                using (StreamWriter sw = new StreamWriter(@getCaminhoAssessor()))
                {
                    sw.Write(lTextoArquivo.ToString());
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GerarClientes()
        {
            try
            {
                var lTextoArquivo = new StringBuilder();

                using (OracleConnection conCadastro = new OracleConnection(this.getBanco))
                {
                    var lQuery = new StringBuilder();

                    lQuery.AppendLine(" SELECT            logincliente.nome nomecliente")
                          .AppendLine(" ,                 cliente.codigobovespa codigocblc")
                          .AppendLine(" ,                 cliente.cpf cpf")
                          .AppendLine(" ,                 cliente.tipo tipocliente")
                          .AppendLine(" ,                 cliente.passo statuscadastro")
                          .AppendLine(" ,                 assessor.id_assessorsinacor codigoassessor")
                          .AppendLine(" ,                 loginassessor.nome nomeassessor")
                          .AppendLine(" ,                 cliente.datacadastroinicial datacadastroinicial")
                          .AppendLine(" ,                 logincliente.email email")
                          .AppendLine(" ,                 cliente.datanascimento datanascimento")
                          .AppendLine(" ,                 endereco.logradouro logradouro")
                          .AppendLine(" ,                 endereco.numero numero")
                          .AppendLine(" ,                 endereco.complemento complemento")
                          .AppendLine(" ,                 endereco.bairro bairro")
                          .AppendLine(" ,                 endereco.cidade cidade")
                          .AppendLine(" ,                 endereco.uf uf")
                          .AppendLine(" ,                 endereco.pais pais")
                          .AppendLine(" ,                 endereco.tipo tipoendereco")
                          .AppendLine(" ,                 telefone.ddd ddd")
                          .AppendLine(" ,                 telefone.telefone telefone")
                          .AppendLine(" ,                 telefone.ramal ramal")
                          .AppendLine(" ,                 telefone.tipo tipo")
                          .AppendLine(" ,                 cliente.dataaprovacaofinal datacadastrofinal")
                          .AppendLine(" FROM              cliente")
                          .AppendLine(" INNER JOIN        login logincliente  ON cliente.id_login = logincliente.id_login")
                          .AppendLine(" LEFT  OUTER  JOIN endereco            ON cliente.id_cliente = endereco.id_cliente AND endereco.correspondencia = 'S'")
                          .AppendLine(" LEFT  OUTER  JOIN telefone            ON cliente.id_cliente = telefone.id_cliente")
                          .AppendLine(" INNER JOIN        assessorfilial      ON cliente.id_assessorfilial = assessorfilial.id_assessorfilial")
                          .AppendLine(" INNER JOIN        assessor            ON assessor.id_assessor = assessorfilial.id_assessor")
                          .AppendLine(" INNER JOIN        login loginassessor ON assessor.id_login = loginassessor.id_login")
                          .AppendLine(" WHERE             telefone.principal = 'S'");

                    conCadastro.Open();

                    using (OracleCommand lOracleCommand = new OracleCommand(lQuery.ToString(), conCadastro))
                    {
                        OracleDataReader lOracleDataReader = lOracleCommand.ExecuteReader();

                        while (lOracleDataReader.Read())
                        {
                            if (lOracleDataReader["StatusCadastro"].ToString().Trim() == "4")
                            {
                                lTextoArquivo.AppendFormat("{0};", TrataCaracter(lOracleDataReader["NomeCliente"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["CodigoCBLC"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["CPF"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["TipoCliente"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["StatusCadastro"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["CodigoAssessor"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["NomeAssessor"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(DateTime.Parse(lOracleDataReader["DataCadastroFinal"].ToString()).ToString("dd/MM/yyyy HH:mm:ss")))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Email"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(DateTime.Parse(lOracleDataReader["DataNascimento"].ToString()).ToString("dd/MM/yyyy HH:mm:ss")))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Logradouro"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Numero"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Complemento"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Bairro"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Cidade"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["UF"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Pais"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["TipoEndereco"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["DDD"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Telefone"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Ramal"].ToString()))
                                             .AppendFormat("{0}\r\n", TrataCaracter(lOracleDataReader["Tipo"].ToString()));
                            }
                            else
                            {
                                lTextoArquivo.AppendFormat("{0};", TrataCaracter(lOracleDataReader["NomeCliente"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["CodigoCBLC"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["CPF"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["TipoCliente"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["StatusCadastro"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["CodigoAssessor"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["NomeAssessor"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(DateTime.Parse(lOracleDataReader["DataCadastroInicial"].ToString()).ToString("dd/MM/yyyy HH:mm:ss")))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Email"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(DateTime.Parse(lOracleDataReader["DataNascimento"].ToString()).ToString("dd/MM/yyyy HH:mm:ss")))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Logradouro"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Numero"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Complemento"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Bairro"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Cidade"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["UF"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Pais"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["TipoEndereco"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["DDD"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Telefone"].ToString()))
                                             .AppendFormat("{0};", TrataCaracter(lOracleDataReader["Ramal"].ToString()))
                                             .AppendFormat("{0}\r\n", TrataCaracter(lOracleDataReader["Tipo"].ToString()));
                            }
                        }

                        if (conCadastro.State == System.Data.ConnectionState.Open)
                            conCadastro.Close();
                    }
                }

                using (StreamWriter lStreamWriter = new StreamWriter(@getCaminhoCliente()))
                {
                    lStreamWriter.Write(lTextoArquivo.ToString());
                    lStreamWriter.Flush();
                    lStreamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string TrataCaracter(string dado)
        {
            return dado.Replace(";", "-").Replace("´", "").Replace("'", "").Trim();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                if (DateTime.Now.Hour == getHora() && DateTime.Now.Minute < 11)
                {
                    this.GerarClientes();
                    this.GerarAssessores();
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Serviço Gradual.Cadastro.ExportaClientes", ex.ToString(), EventLogEntryType.Error);
            }
        }
    }
}
