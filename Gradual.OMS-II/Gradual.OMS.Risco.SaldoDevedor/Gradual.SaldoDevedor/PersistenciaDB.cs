using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Generico.Dados;
using System.Data.Common;
using log4net;
using Gradual.SaldoDevedor.lib.Info;
using Gradual.SaldoDevedor.lib.Mensagens;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Gradual.SaldoDevedor
{
    public class PersistenciaDB
    {
        #region Private Members

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string gNomeConexaoSinacor = "SINACOR";

        private const string gNomeConexaoSQL = "SQL";

        #endregion

        public decimal ObterParametroLimiteSaldoDisponivel()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            decimal LimiteSaldoMulta = 0;
            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_parametro"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dsParametro", DbType.String, "LimiteSaldoMulta");

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        LimiteSaldoMulta = (lDataTable.Rows[0]["vlParametro"]).DBToDecimal();
                }
                return LimiteSaldoMulta;
            }
            catch (Exception ex)
            {
                logger.Error("ObterParametroLimiteSaldoDisponivel(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }
        }

        public void GravarParametroLimiteSaldoDisponivel(decimal limiteSaldoDisponivel)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_parametro"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dsParametro", DbType.String, "LimiteSaldoMulta");
                    lAcessaDados.AddInParameter(lDbCommand, "@vlParametroNovo", DbType.Decimal, limiteSaldoDisponivel);
                    object result = lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
            }
            catch (Exception ex)
            {
                logger.Error("GravarParametroLimiteSaldoDisponivel(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }
        }

        public decimal ObterParametroTaxaJuros()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            decimal TaxaJuros = 0;
            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_parametro"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dsParametro", DbType.String, "TaxaJurosAplicada");

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        TaxaJuros = (lDataTable.Rows[0]["vlParametro"]).DBToDecimal();
                }
                return TaxaJuros;
            }
            catch (Exception ex)
            {
                logger.Error("ObterParametroTaxaJuros(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }
        }

        public void GravarParametroTaxaJuros(decimal taxaJuros)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_parametro"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dsParametro", DbType.String, "TaxaJurosAplicada");
                    lAcessaDados.AddInParameter(lDbCommand, "@vlParametroNovo", DbType.Decimal, taxaJuros);
                    object result = lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
            }
            catch (Exception ex)
            {
                logger.Error("GravarParametroTaxaJuros(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }
        }

        public decimal ObterParametroCodigoArquivoTesouraria()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            decimal CodigoArquivoTesouraria = 0;
            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_parametro"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dsParametro", DbType.String, "CodigoArquivoTesouraria");

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        CodigoArquivoTesouraria = (lDataTable.Rows[0]["vlParametro"]).DBToDecimal();
                }
                return CodigoArquivoTesouraria;
            }
            catch (Exception ex)
            {
                logger.Error("ObterParametroCodigoArquivoTesouraria(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }
        }

        public void GravarParametroCodigoArquivoTesouraria(decimal codigoArquivoTesouraria)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_parametro"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dsParametro", DbType.String, "CodigoArquivoTesouraria");
                    lAcessaDados.AddInParameter(lDbCommand, "@vlParametroNovo", DbType.Decimal, codigoArquivoTesouraria);
                    object result = lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
            }
            catch (Exception ex)
            {
                logger.Error("GravarParametroCodigoArquivoTesouraria(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }
        }

        public decimal ObterParametroNotificarAtendimentoDiasAlternados()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            decimal NotificarAtendimentoDiasAlternados = 0;
            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_parametro"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dsParametro", DbType.String, "NotificarAtendimentoDiasAlternados");

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        NotificarAtendimentoDiasAlternados = (lDataTable.Rows[0]["vlParametro"]).DBToDecimal();
                }
                return NotificarAtendimentoDiasAlternados;
            }
            catch (Exception ex)
            {
                logger.Error("ObterParametroNotificarAtendimentoDiasAlternados(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }
        }

        public void GravarParametroNotificarAtendimentoDiasAlternados(decimal notificarAtendimentoDiasAlternados)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_parametro"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dsParametro", DbType.String, "NotificarAtendimentoDiasAlternados");
                    lAcessaDados.AddInParameter(lDbCommand, "@vlParametroNovo", DbType.Decimal, notificarAtendimentoDiasAlternados);
                    object result = lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
            }
            catch (Exception ex)
            {
                logger.Error("GravarParametroNotificarAtendimentoDiasAlternados(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }
        }

        public ExcecaoInfo ObterListaExcecao()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            ExcecaoInfo excecao = new ExcecaoInfo();
            excecao.ListaAssessor = new List<int>();
            excecao.ListaCliente = new List<int>();
            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_lst_excecao"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            int tipo = (lDataTable.Rows[i]["idTipo"]).DBToInt32();
                            if (tipo == 1)
                                excecao.ListaCliente.Add((lDataTable.Rows[i]["idClienteAssessor"]).DBToInt32());
                            else
                                excecao.ListaAssessor.Add((lDataTable.Rows[i]["idClienteAssessor"]).DBToInt32());
                        }
                    }
                }
                return excecao;
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaExcecao(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }
        }

        public void RemoverListaExcecao(int idTipo)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_del_excecao"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idTipo", DbType.Int32, idTipo);

                    object result = lAcessaDados.ExecuteScalar(lDbCommand);
                }
            }
            catch (Exception ex)
            {
                logger.Error("RemoverListaExcecao(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }
        }

        public void GravarItemExcecao(int idTipo, int idClienteAssessor)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_excecao"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idTipo", DbType.Int32, idTipo);
                    lAcessaDados.AddInParameter(lDbCommand, "@idClienteAssessor", DbType.Int32, idClienteAssessor);
                    object result = lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
            }
            catch (Exception ex)
            {
                logger.Error("GravarItemExcecao(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }
        }

        public TextoEmailResponse GravarListaTextoEmail(TextoEmailRequest request)
        {
            TextoEmailResponse response = new TextoEmailResponse();
            response.Retorno = TextoEmailResponse.RETORNO_ERRO;

            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                foreach (TextoEmailInfo item in request.Lista)
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_textoemail"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@idTextoEmail", DbType.Int32, item.Id);
                        lAcessaDados.AddInParameter(lDbCommand, "@dsConteudo", DbType.String, item.Texto);
                        lAcessaDados.AddInParameter(lDbCommand, "@dtAtualizacao", DbType.DateTime, item.DataAtualizacao);
                        object result = lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }
                }
                response.Retorno = TextoEmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("GravarListaTextoEmail(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }

            return response;
        }

        public TextoEmailResponse ObterListaTextoEmail()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            TextoEmailResponse response = new TextoEmailResponse();
            response.Lista = new Dictionary<int, TextoEmailInfo>();
            response.Retorno = TextoEmailResponse.RETORNO_ERRO;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_lst_textoemail"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            TextoEmailInfo textoEmail = new TextoEmailInfo();
                            textoEmail.Id = (lDataTable.Rows[i]["idTextoEmail"]).DBToInt32();
                            textoEmail.Texto = (lDataTable.Rows[i]["dsConteudo"]).DBToString();
                            textoEmail.DataAtualizacao = (lDataTable.Rows[i]["dtAtualizacao"]).DBToDateTime();

                            response.Lista.Add(textoEmail.Id, textoEmail);
                        }
                    }
                }
                response.Retorno = TextoEmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaTextoEmail(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
            }

            return response;
        }

        private decimal CalcularJurosSaldoDevedor(decimal saldoDisponivel)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            decimal juros = 0;
            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_juros_saldodevedor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@Saldo", DbType.Int32, saldoDisponivel);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        juros = (lDataTable.Rows[0]["Juros"]).DBToDecimal();
                }

                return juros;
            }
            catch (Exception ex)
            {
                logger.Error("CalcularJurosSaldoDevedor(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }
        }

        private int ObterQuantidadeDiasNegativoCliente(int codigoCliente, DateTime dataMovimento)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            int qtdDiasAtraso = 0;
            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_dias_saldo_devedor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, codigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "dtMovimento", DbType.DateTime, dataMovimento);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        qtdDiasAtraso = (lDataTable.Rows[0]["qtd_dias_devedor"]).DBToInt32();
                }
                return qtdDiasAtraso;
            }

            catch (Exception ex)
            {
                logger.Error("ObterQuantidadeDiasNegativoCliente(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }
        }

        public EmailResponse GravarListaEmail(EmailRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            EmailResponse response = new EmailResponse();
            response.Retorno = EmailResponse.RETORNO_ERRO;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                foreach (EmailInfo item in request.Lista)
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_email"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@idEmail", DbType.Int32, item.Id);
                        lAcessaDados.AddInParameter(lDbCommand, "@dsEmail", DbType.String, item.Descricao);
                        lAcessaDados.AddInParameter(lDbCommand, "@dsTitulo", DbType.String, item.Titulo);
                        lAcessaDados.AddInParameter(lDbCommand, "@dsAssunto", DbType.String, item.Assunto);
                        lAcessaDados.AddInParameter(lDbCommand, "@dsPara", DbType.String, item.EmailPara);
                        lAcessaDados.AddInParameter(lDbCommand, "@dsCC", DbType.String, item.EmailCopia);
                        lAcessaDados.AddInParameter(lDbCommand, "@dsBCC", DbType.String, item.EmailCopiaOculta);
                        lAcessaDados.AddInParameter(lDbCommand, "@idTextoEmail", DbType.Int32, item.IdTextoEmail);
                        lAcessaDados.AddInParameter(lDbCommand, "@dtAtualizacao", DbType.DateTime, item.DataAtualizacao);
                        object result = lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }
                }
                response.Retorno = EmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("GravarListaEmail(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
            }

            return response;
        }

        public EmailResponse ObterListaEmail()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            EmailResponse response = new EmailResponse();
            response.Lista = new Dictionary<int,EmailInfo>();
            response.Retorno = EmailResponse.RETORNO_ERRO;

            int id = 0;
            int assessor = 0;
            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_lst_assessor_email"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            id = (lDataTable.Rows[i]["idEmail"]).DBToInt32();
                            assessor = (lDataTable.Rows[i]["idAssessor"]).DBToInt32();

                            if (!response.Lista.ContainsKey(id))
                            {
                                response.Lista.Add(id, ObterDetalheEmail(id));
                                response.Lista[id].ListaAssessor = new List<int>();
                            }
                            response.Lista[id].ListaAssessor.Add(assessor);
                        }
                    }
                }
                response.Retorno = EmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaEmail(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
            }

            return response;
        }

        public EmailInfo ObterDetalheEmail(int id)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            EmailInfo email = new EmailInfo();
            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_email"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idEmail", DbType.Int32, id);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        email.Id = (lDataTable.Rows[0]["idEmail"]).DBToInt32();
                        email.Descricao = (lDataTable.Rows[0]["dsEmail"]).DBToString();
                        email.Titulo = (lDataTable.Rows[0]["dsTitulo"]).DBToString();
                        email.Assunto = (lDataTable.Rows[0]["dsAssunto"]).DBToString();
                        email.IdTextoEmail = (lDataTable.Rows[0]["idTextoEmail"]).DBToInt32();
                        email.Conteudo = (lDataTable.Rows[0]["dsConteudo"]).DBToString();
                        email.EmailPara = (lDataTable.Rows[0]["dsPara"]).DBToString();
                        email.EmailCopia = (lDataTable.Rows[0]["dsCC"]).DBToString();
                        email.EmailCopiaOculta = (lDataTable.Rows[0]["dsBCC"]).DBToString();
                        email.Anexos = (lDataTable.Rows[0]["dsAnexos"]).DBToString();
                        email.DataAtualizacao = (lDataTable.Rows[0]["dtAtualizacao"]).DBToDateTime();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("ObterDetalheEmail(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }

            return email;
        }

        public EmailResponse ObterListaDetalheEmail()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            EmailResponse response = new EmailResponse();
            response.Lista = new Dictionary<int, EmailInfo>();
            response.Retorno = EmailResponse.RETORNO_ERRO;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_lst_email"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            EmailInfo email = new EmailInfo();
                            email.Id = (lDataTable.Rows[i]["idEmail"]).DBToInt32();
                            email.Descricao = (lDataTable.Rows[i]["dsEmail"]).DBToString();
                            email.Titulo = (lDataTable.Rows[i]["dsTitulo"]).DBToString();
                            email.Assunto = (lDataTable.Rows[i]["dsAssunto"]).DBToString();
                            email.IdTextoEmail = (lDataTable.Rows[i]["idTextoEmail"]).DBToInt32();
                            email.Conteudo = (lDataTable.Rows[i]["dsConteudo"]).DBToString();
                            email.EmailPara = (lDataTable.Rows[i]["dsPara"]).DBToString();
                            email.EmailCopia = (lDataTable.Rows[i]["dsCC"]).DBToString();
                            email.EmailCopiaOculta = (lDataTable.Rows[i]["dsBCC"]).DBToString();
                            email.DataAtualizacao = (lDataTable.Rows[i]["dtAtualizacao"]).DBToDateTime();
                            response.Lista.Add(email.Id, email);
                        }
                    }
                }
                response.Retorno = EmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaDetalheEmail(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
            }

            return response;
        }

        public AssessorEmailResponse GravarListaAssessorEmail(AssessorEmailRequest request)
        {
            AssessorEmailResponse response = new AssessorEmailResponse();
            response.Retorno = AssessorEmailResponse.RETORNO_ERRO;

            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                foreach (AssessorEmailInfo item in request.Lista)
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_assessoremail"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@idAssessor", DbType.Int32, item.IdAssessor);
                        lAcessaDados.AddInParameter(lDbCommand, "@idEmail", DbType.Int32, item.IdEmail);
                        lAcessaDados.AddInParameter(lDbCommand, "@dtAtualizacao", DbType.DateTime, item.DataAtualizacao);
                        object result = lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }
                }
                response.Retorno = AssessorEmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("GravarListaAssessorEmail(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
            }

            return response;
        }

        public AssessorEmailResponse RemoverListaAssessorEmail(AssessorEmailRequest request)
        {
            AssessorEmailResponse response = new AssessorEmailResponse();
            response.Retorno = AssessorEmailResponse.RETORNO_ERRO;

            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                foreach (AssessorEmailInfo item in request.Lista)
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_del_assessoremail"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@idAssessor", DbType.Int32, item.IdAssessor);

                        object result = lAcessaDados.ExecuteScalar(lDbCommand);
                    }
                }
                response.Retorno = AssessorEmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("RemoverListaAssessorEmail(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
            }

            return response;
        }

        public AssessorEmailResponse ObterListaAssessorEmail()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            AssessorEmailResponse response = new AssessorEmailResponse();
            response.Lista = new List<AssessorEmailInfo>();
            response.Retorno = AssessorEmailResponse.RETORNO_ERRO;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_lst_assessor_email"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            AssessorEmailInfo info = new AssessorEmailInfo();
                            info.IdAssessor = (lDataTable.Rows[i]["idAssessor"]).DBToInt32();
                            info.IdEmail = (lDataTable.Rows[i]["idEmail"]).DBToInt32();
                            info.DataAtualizacao = (lDataTable.Rows[i]["dtAtualizacao"]).DBToDateTime();
                            response.Lista.Add(info);
                        }
                    }
                }
                response.Retorno = AssessorEmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaAssessorEmail(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
            }

            return response;
        }

        public InformacaoClienteResponse ObterListaClientesSaldoDevedor(InformacaoClienteRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            InformacaoClienteResponse response = new InformacaoClienteResponse();
            response.ListaInformacoesCliente = new Dictionary<int, InformacoesClienteInfo>();
            response.Retorno = InformacaoClienteResponse.RETORNO_ERRO;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_saldo_devedor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "dtMovimento", DbType.DateTime, request.DadosCliente.DataMovimento);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            InformacoesClienteInfo ClienteInfo = new InformacoesClienteInfo();
                            ClienteInfo.CodigoAssessor = (lDataTable.Rows[i]["cd_assessor"]).DBToInt32();
                            ClienteInfo.NomeAssessor = (lDataTable.Rows[i]["nm_assessor"]).DBToString();
                            ClienteInfo.CodigoCliente = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();
                            ClienteInfo.NomeCliente = (lDataTable.Rows[i]["nm_cliente"]).DBToString();
                            ClienteInfo.EmailCliente = (lDataTable.Rows[i]["nm_e_mail"]).DBToString();
                            ClienteInfo.SaldoDisponivel = (lDataTable.Rows[i]["vl_disponivel"]).DBToDecimal();
                            ClienteInfo.SaldoTotal = (lDataTable.Rows[i]["vl_total"]).DBToDecimal();
                            ClienteInfo.SaldoProjetadoD1 = (lDataTable.Rows[i]["vl_projet1"]).DBToDecimal();
                            ClienteInfo.SaldoProjetadoD2 = (lDataTable.Rows[i]["vl_projet2"]).DBToDecimal();
                            ClienteInfo.DataMovimento = request.DadosCliente.DataMovimento;

                            ClienteInfo.NrDiasNegativo = ObterQuantidadeDiasNegativoCliente(ClienteInfo.CodigoCliente, ClienteInfo.DataMovimento);
                            ClienteInfo.JurosCalculado = CalcularJurosSaldoDevedor(ClienteInfo.SaldoDisponivel);

                            if (ClienteInfo.SaldoTotal >= 0)
                                ClienteInfo.Desenquadrado = 0;
                            else
                                ClienteInfo.Desenquadrado = 
                                    (ClienteInfo.SaldoTotal > ClienteInfo.SaldoDisponivel ? ClienteInfo.SaldoTotal : ClienteInfo.SaldoDisponivel);

                            if (!response.ListaInformacoesCliente.ContainsKey(ClienteInfo.CodigoCliente))
                                response.ListaInformacoesCliente.Add(ClienteInfo.CodigoCliente, ClienteInfo);
                        }
                    }
                }
                response.Retorno = InformacaoClienteResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaClientesSaldoDevedor(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
            }

            return response;
        }

        public InformacaoClienteResponse GravarMovimentoCliente(InformacaoClienteRequest request)
        {
            InformacaoClienteResponse response = new InformacaoClienteResponse();
            response.Retorno = InformacaoClienteResponse.RETORNO_ERRO;

            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_movimento"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idCliente", DbType.Int32, request.DadosCliente.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@idAssessor", DbType.Int32, request.DadosCliente.CodigoAssessor);
                    lAcessaDados.AddInParameter(lDbCommand, "@vlSaldoAbertura", DbType.Decimal, request.DadosCliente.SaldoDisponivel);
                    lAcessaDados.AddInParameter(lDbCommand, "@vlSaldoProjetado", DbType.Decimal, request.DadosCliente.SaldoTotal);
                    lAcessaDados.AddInParameter(lDbCommand, "@vlJuros", DbType.Decimal, request.DadosCliente.JurosCalculado);
                    lAcessaDados.AddInParameter(lDbCommand, "@nrDiasNegativo", DbType.Int32, request.DadosCliente.NrDiasNegativo);
                    lAcessaDados.AddInParameter(lDbCommand, "@dsAssunto", DbType.String, request.DadosEmail.Assunto);
                    lAcessaDados.AddInParameter(lDbCommand, "@dsPara", DbType.String, request.DadosEmail.EmailPara);
                    lAcessaDados.AddInParameter(lDbCommand, "@dsCC", DbType.String, request.DadosEmail.EmailCopia);
                    lAcessaDados.AddInParameter(lDbCommand, "@dsBCC", DbType.String, request.DadosEmail.EmailCopiaOculta);
                    lAcessaDados.AddInParameter(lDbCommand, "@dsMovimento", DbType.String, request.DadosCliente.DescricaoMovimento);
                    object result = lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                response.Retorno = AssessorEmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("GravarMovimentoCliente(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
            }

            return response;
        }

        public InformacaoClienteResponse GravarHistoricoCliente(InformacaoClienteRequest request)
        {
            InformacaoClienteResponse response = new InformacaoClienteResponse();
            response.Retorno = InformacaoClienteResponse.RETORNO_ERRO;

            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                foreach (InformacoesClienteInfo item in request.Lista)
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_historico"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@dtMovimento", DbType.DateTime, item.DataMovimento);
                        lAcessaDados.AddInParameter(lDbCommand, "@idAssessor", DbType.Int32, item.CodigoAssessor);
                        lAcessaDados.AddInParameter(lDbCommand, "@nmAssessor", DbType.String, item.NomeAssessor);
                        lAcessaDados.AddInParameter(lDbCommand, "@idCliente", DbType.Int32, item.CodigoCliente);
                        lAcessaDados.AddInParameter(lDbCommand, "@nmCliente", DbType.String, item.NomeCliente);
                        lAcessaDados.AddInParameter(lDbCommand, "@vlSaldoAbertura", DbType.Decimal, item.SaldoDisponivel);
                        lAcessaDados.AddInParameter(lDbCommand, "@vlSaldoProjetado", DbType.Decimal, item.SaldoTotal);
                        lAcessaDados.AddInParameter(lDbCommand, "@vlJuros", DbType.Decimal, item.JurosCalculado);
                        lAcessaDados.AddInParameter(lDbCommand, "@nrDiasNegativo", DbType.Int32, item.NrDiasNegativo);
                        lAcessaDados.AddInParameter(lDbCommand, "@vlDesenquadrado", DbType.Decimal, item.Desenquadrado);
                        object result = lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }
                }
                response.Retorno = AssessorEmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("GravarHistoricoCliente(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
            }

            return response;
        }

        public HistoricoDatasResponse ObterListaHistoricoDatas()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            HistoricoDatasResponse response = new HistoricoDatasResponse();
            response.Lista = new List<string>();
            response.Retorno = HistoricoDatasResponse.RETORNO_ERRO;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_lst_historico_datas"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            response.Lista.Add((lDataTable.Rows[i]["vlDias"]).DBToString());
                        }
                    }
                }
                response.Retorno = TextoEmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaHistoricoDatas(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
            }

            return response;
        }

        public HistoricoResponse ObterListaHistorico(HistoricoRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            HistoricoResponse response = new HistoricoResponse();
            response.Lista = new List<InformacoesClienteInfo>();
            response.Retorno = HistoricoDatasResponse.RETORNO_ERRO;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_historico"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dtHistorico", DbType.DateTime, request.DataHistorico);
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            InformacoesClienteInfo ClienteInfo = new InformacoesClienteInfo();
                            ClienteInfo.DataMovimento = (lDataTable.Rows[i]["dtMovimento"]).DBToDateTime();
                            ClienteInfo.CodigoAssessor = (lDataTable.Rows[i]["idAssessor"]).DBToInt32();
                            ClienteInfo.NomeAssessor = (lDataTable.Rows[i]["nmAssessor"]).DBToString();
                            ClienteInfo.CodigoCliente = (lDataTable.Rows[i]["idCliente"]).DBToInt32();
                            ClienteInfo.NomeCliente = (lDataTable.Rows[i]["nmCliente"]).DBToString();
                            ClienteInfo.SaldoDisponivel = (lDataTable.Rows[i]["vlSaldoAbertura"]).DBToDecimal();
                            ClienteInfo.SaldoTotal = (lDataTable.Rows[i]["vlSaldoProjetado"]).DBToDecimal();
                            ClienteInfo.JurosCalculado = (lDataTable.Rows[i]["vlJuros"]).DBToDecimal();
                            ClienteInfo.NrDiasNegativo = (lDataTable.Rows[i]["nrDiasNegativo"]).DBToInt32();
                            ClienteInfo.Desenquadrado = (lDataTable.Rows[i]["vlDesenquadrado"]).DBToDecimal();
                            response.Lista.Add(ClienteInfo);
                        }
                    }
                }
                response.Retorno = TextoEmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaHistorico(): Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
            }

            return response;
        }

        /*
        public List<int> ObterClientesAssessor(List<int> Assessores)
        {

            AcessaDados lAcessaDados = new AcessaDados();

            List<int> lstClientes = new List<int>();                              

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                foreach (int Assessor in Assessores)
                {

                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_gconsulta_assessor"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "pAssessor", DbType.Int32, Assessor);

                        DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {
                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            {
                                int CodigoCliente = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();

                                if (!lstClientes.Contains(CodigoCliente))
                                    lstClientes.Add(CodigoCliente);

                            }
                        }

                    }
                }

                return lstClientes;
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }
        }
        
        public void ImportarClientes()
        {
            string CodigoAssessor, CodigoCliente, NrDIAS, Valor;

            CodigoCliente = string.Empty;

            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                       

                //OleDbConnection conexao = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\SaldoDevedor\SaldoClientes.xlsx;Extended Properties='Excel 12.0 Xml;HDR=YES';");
                OleDbConnection conexao = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\cnakata\Documents\Meus documentos\Projetos\Saldo Devedor\Devedores - V2_tesouraria - 2015-01-12.xlsm;Extended Properties='Excel 12.0 Xml;HDR=YES';");

                OleDbDataAdapter adapter = new OleDbDataAdapter("select * from [D0email$]", conexao);

                DataSet ds = new DataSet();

                conexao.Open();

                adapter.Fill(ds);

                DataTable lDataTable = ds.Tables[0];

                for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                {
                     CodigoAssessor = (lDataTable.Rows[i]["Código Assessor"]).ToString();
                     CodigoCliente = (lDataTable.Rows[i]["Código Cliente"]).ToString();
                     NrDIAS = (lDataTable.Rows[i]["dias em atraso"]).ToString();
                     Valor = (lDataTable.Rows[i]["Saldo de abertura"]).ToString();

                     if (CodigoCliente.Length > 3)
                     {

                         using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_dados_bkp"))
                         {

                             lAcessaDados.AddInParameter(lDbCommand, "@idCliente", DbType.Int32, CodigoCliente.DBToInt32());
                             lAcessaDados.AddInParameter(lDbCommand, "@dtMovimento", DbType.DateTime, DateTime.Now.ToString());

                             //lAcessaDados.AddInParameter(lDbCommand, "@idAssessor", DbType.Int32, CodigoAssessor.DBToInt32());
                             lAcessaDados.AddInParameter(lDbCommand, "@nrDias", DbType.Int32, NrDIAS.DBToInt32());
                             lAcessaDados.AddInParameter(lDbCommand, "@Saldo", DbType.Decimal, Valor.DBToDecimal());

                             object result = lAcessaDados.ExecuteScalar(lDbCommand);
                         }
                     }
                     else
                     {
                         bool entrou = true;
                     }

                }
            }
            catch (Exception ex )
            {
                string Cliente = CodigoCliente.ToString();
                //throw (ex);
            }
        }

        public void EfetuarBackupHistorico()
        {

            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_backup_ClienteResumohistorico"))
                {
                    object result = lAcessaDados.ExecuteScalar(lDbCommand);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        */

    }
}
