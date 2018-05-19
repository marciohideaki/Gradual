using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Text;
using Gradual.Cadastro.Entidades;
using Gradual.Generico.Dados;
using Gradual.Generico.Geral;

namespace Gradual.Cadastro.Negocios
{
    public class NLogin
    {
        /// <summary>
        /// Tipo de Login: Administrador, Assessor, Atendimento, etc.
        /// </summary>
        public enum eTipo
        {
            Administrador,
            Assessor,
            Atendimento,
            Telemarketing,
            Todos
        }

        public ELogin SelecionarPorCPF(string pCPF)
        {
            ELogin _ELogin = new ELogin();
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

            _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

            StringBuilder sbSQL = new StringBuilder();

            sbSQL.Append("Select LOGIN.* from LOGIN,cliente where login.id_login = cliente.id_login and cpf = '" + pCPF + "'");

            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
            DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

            if (_table.Rows.Count > 0)
            {
                _ELogin.Ativo = Conversao.ToChar(_table.Rows[0]["Ativo"]).Value;
                _ELogin.Email = Conversao.ToString(_table.Rows[0]["Email"]);
                _ELogin.ID_Login = Conversao.ToInt(_table.Rows[0]["ID_Login"]).Value;
                _ELogin.Nome = Conversao.ToString(_table.Rows[0]["Nome"]);
                _ELogin.Tipo = Conversao.ToInt(_table.Rows[0]["Tipo"]).Value;
                _ELogin.Senha = Conversao.ToString(_table.Rows[0]["Senha"]);
                _ELogin.Assinatura = Conversao.ToString(_table.Rows[0]["Assinatura"]);
            }
            else
            {
                throw new Exception("CPF não encontrado no Sistema Antigo!");
            }

            return _ELogin;
        }

        public ELogin SelecionarCliente(Int64 pCpfCnpj)
        {
            ELogin lRetorno = new ELogin();
            Gradual.Generico.Dados.AcessaDadosAntigo _AcessaDados = new Gradual.Generico.Dados.AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = Gradual.Generico.Dados.ConexaoAntigo.ConnectionName;
            string sbSQL = "SELECT l.ID_Login,l.senha,l.assinatura, l.Ativo, l.Email, l.Tipo, l.Nome from Login l,cliente c  where to_number(c.cpf) = to_number (" + pCpfCnpj .ToString()+ ") and c.id_login=l.id_login ";
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
            DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);
            if (_table.Rows.Count > 0)
            {
                lRetorno.ID_Login = Conversao.ToInt(_table.Rows[0]["ID_Login"]).Value;
                lRetorno.Ativo = Conversao.ToChar(_table.Rows[0]["Ativo"]).Value;
                lRetorno.Email = Conversao.ToString(_table.Rows[0]["Email"]);
                lRetorno.Nome = Conversao.ToString(_table.Rows[0]["Nome"]);
                lRetorno.Tipo = Conversao.ToInt(_table.Rows[0]["Tipo"]).Value;
                lRetorno.Senha = Conversao.ToString(_table.Rows[0]["Senha"]);
                lRetorno.Assinatura = Conversao.ToString(_table.Rows[0]["Assinatura"]);
            }
            else
            {
                throw new Exception("CPF/CNPJ não Encontrado!");
            }
            return lRetorno;
        }

        public List<ELogin> Listar(eTipo pTipo)
        {
            List<ELogin> lRetorno = new List<ELogin>();
            Gradual.Generico.Dados.AcessaDadosAntigo _AcessaDados = new Gradual.Generico.Dados.AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = Gradual.Generico.Dados.ConexaoAntigo.ConnectionName;

            string sbSQL = "";
            switch (pTipo)
            {
                case eTipo.Administrador:
                    sbSQL = "SELECT ID_Login,senha,assinatura, Ativo, Email, Tipo, Nome from Login  where tipo = 1 and ativo = 'S' ";
                    break;
                case eTipo.Assessor:
                    sbSQL = "SELECT login.ID_Login,senha,assinatura, Ativo, Email, Tipo, id_assessorsinacor as nome from Login,assessor  where tipo = 2 and login.id_login = assessor.id_login  and ativo = 'S' ";
                    break;
                case eTipo.Atendimento:
                    sbSQL = "SELECT ID_Login,senha,assinatura, Ativo, Email, Tipo, Nome from Login  where tipo = 4  and ativo = 'S' ";
                    break;
                case eTipo.Telemarketing:
                    sbSQL = "SELECT ID_Login,senha,assinatura, Ativo, Email, Tipo, Nome from Login  where tipo = 5  and ativo = 'S' ";
                    break;
                case eTipo.Todos:
                    break;
                default:
                    break;
            }

            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
            DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);
            ELogin _el;
            foreach (DataRow item in _table.Rows)
            {
                _el = new ELogin();
                _el.ID_Login = Conversao.ToInt(item["ID_Login"]).Value;
                _el.Ativo = Conversao.ToChar(item["Ativo"]).Value;
                _el.Email = Conversao.ToString(item["Email"]);
                _el.Nome = Conversao.ToString(item["Nome"]);
                _el.Tipo = Conversao.ToInt(item["Tipo"]).Value;
                _el.Senha = Conversao.ToString(item["Senha"]);
                _el.Assinatura = Conversao.ToString(item["Assinatura"]);
                lRetorno.Add(_el);
            }

            return lRetorno;
        }

        /// <summary>
        /// Lista Logins
        /// </summary>
        /// <param name="_Nome">Nome</param>
        /// <param name="_Tipo">Tipo de Login</param>
        /// <returns>Lista contendo todos os Logins com o Nome e Tipo informados</returns>
        //public BindingList<ELogin> Listar(string _Nome, eTipo _Tipo)
        //{

        //    BindingList<ELogin> _ELogin = new BindingList<ELogin>();
        //    AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
        //    _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

        //    StringBuilder sbSQL = new StringBuilder();

        //    sbSQL.Append("SELECT ID_Login, Ativo, Email, Tipo, Nome from Login ");
        //    sbSQL.Append(" where upper(nome ) like'%" + _Nome.ToUpper() + "%' ");
        //    switch (_Tipo)
        //    {
        //        case eTipo.Administrador:
        //            sbSQL.Append(" and tipo=1 ");
        //            break;
        //        case eTipo.Assessor:
        //            sbSQL.Append(" and tipo=2 ");
        //            break;
        //        case eTipo.Atendimento:
        //            sbSQL.Append(" and tipo=4 ");
        //            break;
        //        case eTipo.Telemarketing:
        //            sbSQL.Append(" and tipo=5 ");
        //            break;
        //        case eTipo.Todos:
        //            sbSQL.Append(" and tipo in (1,2,4,5) ");
        //            break;
        //        default:
        //            break;
        //    }
        //    sbSQL.Append(" order by nome");
        //    DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

        //    DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

        //    foreach (DataRow item in _table.Rows)
        //    {
        //        ELogin _el = new ELogin();
        //        _el.ID_Login = Conversao.ToInt(item["ID_Login"]).Value;
        //        _el.Ativo = Conversao.ToChar(item["Ativo"]).Value;
        //        _el.Email = Conversao.ToString(item["Email"]);
        //        _el.Nome = Conversao.ToString(item["Nome"]);
        //        _el.Tipo = Conversao.ToInt(item["Tipo"]).Value;

        //        _ELogin.Add(_el);
        //    }

        //    return _ELogin;
        //}

        /// <summary>
        /// Retorna o Nome do Cliente
        /// </summary>
        /// <param name="_email">Email</param>
        /// <returns>Nome do Cliente</returns>
        public string getNome(string _email)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("Select nome from LOGIN LG where upper(Email) = '" + _email.Trim().ToUpper() + "'");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                string retorno = Conversao.ToString(_AcessaDados.ExecuteScalar(_DbCommand));

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Verifica se o Cliente já realizou o Primeiro Acesso
        /// </summary>
        /// <param name="Email">Email</param>
        /// <param name="Cblc">CBLC</param>
        /// <returns>Senha criptografada</returns>
        public string GetPrimeiroAcesso(string Email, string Cblc)
        {
            try
            {
                if (Email == "") { Email = null; } if (Cblc == "") { Cblc = null; }

                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_primeiro_acesso_sel");
                _AcessaDados.AddInParameter(_DbCommand, "pCblc", DbType.AnsiString, Cblc);
                _AcessaDados.AddInParameter(_DbCommand, "pEmail", DbType.AnsiString, Email);

                DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    return Conversao.ToString(_table.Rows[0]["senha"]);
                }

                return string.Empty;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Valida os Dados do Primeiro acesso do Cliente
        /// </summary>
        /// <param name="CPF">CPF</param>
        /// <param name="CBLC">CBLC</param>
        /// <returns>Id do Login</returns>
        public int ValidaAcessoUsuario(string CPF, string CBLC)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_valida_usuario");
                _AcessaDados.AddInParameter(_DbCommand, "pCblc", DbType.AnsiString, CBLC.Trim());
                _AcessaDados.AddInParameter(_DbCommand, "pCpf", DbType.AnsiString, CPF.Trim());

                DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    return Conversao.ToInt(_table.Rows[0]["id_login"]).Value;
                }
                else
                {
                    throw new Exception("Os dados informados estão incorretos. Por favor entre em contato com o Atendimento pelo telefone 0800 723 7444.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lista todos os Administradores
        /// </summary>
        /// <returns>Lista contendo todos os Logins de Administradores</returns>
        public BindingList<ELogin> SelecionarAdministradores()
        {
            try
            {
                BindingList<ELogin> BLLogin = new BindingList<ELogin>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT ID_Administrador, Nome FROM Login INNER JOIN Administrador ");
                sbSQL.Append("ON Login.ID_Login = Administrador.ID_Login ORDER BY Nome");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                ELogin _ELogin;

                foreach (DataRow item in _table.Rows)
                {
                    _ELogin = new ELogin();
                    _ELogin.ID_Login = (int)Conversao.ToInt(item["ID_Administrador"]);
                    _ELogin.Nome = item["Nome"].ToString();
                    BLLogin.Add(_ELogin);
                }

                return BLLogin;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Seleciona um Login
        /// </summary>
        /// <param name="id_login">Id do Login</param>
        /// <returns>Entidade contendo todos os dados de um Login</returns>
        public ELogin Selecionar(int? id_login)
        {
            try
            {
                if (id_login == null)
                    throw new Exception("Código do login não fornecido!");

                ELogin _ELogin = new ELogin();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("Select * from LOGIN where id_login = " + id_login.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    _ELogin.Ativo = Conversao.ToChar(_table.Rows[0]["Ativo"]).Value;
                    _ELogin.Email = Conversao.ToString(_table.Rows[0]["Email"]);
                    _ELogin.ID_Login = Conversao.ToInt(_table.Rows[0]["ID_Login"]).Value;
                    _ELogin.Nome = Conversao.ToString(_table.Rows[0]["Nome"]);
                    _ELogin.Tipo = Conversao.ToInt(_table.Rows[0]["Tipo"]).Value;
                    _ELogin.Senha = Conversao.ToString(_table.Rows[0]["Senha"]);
                    _ELogin.Assinatura = Conversao.ToString(_table.Rows[0]["Assinatura"]);
                }
                else
                {
                    throw new Exception("Registro não encontrado!");
                }

                return _ELogin;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Verifica se o Email já está cadastrado
        /// </summary>
        /// <param name="_Email">Email</param>
        /// <returns>Retorna True se o email está cadastrado</returns>
        public Boolean ValidaEmail(string _Email)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append(" select count(*) from login where email  = '" + _Email.Trim() + "'");
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
               
                int Result = Conversao.ToInt(_AcessaDados.ExecuteScalar(_DbCommand)).Value;
                if (Result == 1)
                    return true;
                else
                    throw new Exception("E-mail não Cadastrado!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Verifica se o Email já está cadastrado
        /// </summary>
        /// <param name="_Email">Email</param>
        /// <returns>Retorna True se o Email já estiver Cadastrado e False se não estiver</returns>
        public Boolean ExisteEmail(string _Email)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append(" select count(*) from login where email  = '" + _Email.Trim() + "'");
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                int Result = Conversao.ToInt(_AcessaDados.ExecuteScalar(_DbCommand)).Value;
                if (Result == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Validação para geração de nova senha
        /// </summary>
        /// <param name="_Email">Email</param>
        /// <param name="_CPF">CPF</param>
        /// <param name="_Nascimento">Data de Nascimento</param>
        /// <returns>Retorna True se todos os dados estiverem corretos</returns>
        public Boolean ValidaEmail(string _Email, string _CPF, DateTime _Nascimento)
        {
            try
            {
                string cpf = _CPF.Trim().Replace(".", "").Replace("-", "");
                Int64 icpf = Int64.Parse(cpf);

                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append(" select count(*) ");
                sbSQL.Append(" from login l, cliente c ");
                sbSQL.Append(" where l.id_login=c.id_login and ");
                sbSQL.Append(" l.email  = '" + _Email.Trim() + "' and ");
                sbSQL.Append(" (c.cpf  = '" + _CPF.Trim().Replace(".", "").Replace("-", "") + "' or c.cpf='" + icpf.ToString() + "' ) and ");
                sbSQL.Append(" c.DataNascimento  = " + Conversao.ToDateOracle(_Nascimento));

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                int Result = Conversao.ToInt(_AcessaDados.ExecuteScalar(_DbCommand)).Value;
                if (Result == 1)
                    return true;
                else
                    throw new Exception("Os dados informados estão incorretos. Em caso de dúvidas, entre em contato com a Central de Atendimento.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Ativa ou Desativa um Login
        /// </summary>
        /// <param name="_idLogin">Id do Login</param>
        /// <param name="_ATIVO">Informa se está sendo realizada uma ativação ou inativação</param>
        /// <returns>Retorna True se a alteração foi realizada com sucesso</returns>
        public bool Ativar(int _idLogin, char _ATIVO)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append("UPDATE LOGIN SET ATIVO = '" + _ATIVO.ToString() + "' Where id_login = " + _idLogin.ToString());
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                int Result = _AcessaDados.ExecuteNonQuery(_DbCommand);
                if (Result == 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Altera um Login
        /// </summary>
        /// <param name="_id_login">Id do Login</param>
        /// <param name="_Email">Email</param>
        /// <param name="_Nome">Nome</param>
        public void Alterar(int _id_login, string _Email, string _Nome)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT Count(*) FROM Login WHERE Email= '" + _Email + "'");
                sbSQL.Append(" AND ID_Login <> " + _id_login);
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                if (_Email != "a@a.a")
                {
                    int Result = Conversao.ToInt(_AcessaDados.ExecuteScalar(_DbCommand)).Value;
                    if (Result != 0)
                        throw new Exception("E-mail já cadastrado!");
                }
                sbSQL = new StringBuilder();
                sbSQL.Append("UPDATE login SET Email = '" + _Email + "', ");
                sbSQL.Append("Nome = '" + _Nome + "' ");
                sbSQL.Append("WHERE ID_Login = " + _id_login);
                _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
